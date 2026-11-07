using System.Data;
using System.Security.Cryptography;
using BCrypt.Net;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SuwoGym.SqlIntegration;

internal static class Program
{
    private const string DbExclusiva = "SuwoGym_Test_7B";
    private const string Confirmacion = "YES_TEST_DATABASE_ONLY";

    public static async Task<int> Main()
    {
        var cadena = Environment.GetEnvironmentVariable("SUWOGYM_7B_CONNECTION");
        var permiso = Environment.GetEnvironmentVariable("SUWOGYM_7B_CONFIRM");

        if (permiso != Confirmacion || string.IsNullOrWhiteSpace(cadena))
        {
            Console.Error.WriteLine(
                "Pruebas SQL NO ejecutadas. Configure SUWOGYM_7B_CONNECTION y " +
                "SUWOGYM_7B_CONFIRM=YES_TEST_DATABASE_ONLY. Consulte docs/FASE7B_SQL_REAL.md.");
            return 2;
        }

        SqlConnectionStringBuilder configuracion;
        try
        {
            configuracion = new SqlConnectionStringBuilder(cadena);
        }
        catch (ArgumentException)
        {
            Console.Error.WriteLine("Cadena SQL inválida. No se estableció conexión.");
            return 2;
        }

        if (!string.Equals(configuracion.InitialCatalog, DbExclusiva,
                StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("EJECUCIÓN BLOQUEADA: solo se permite " + DbExclusiva + ".");
            return 2;
        }

        try
        {
            await using var conexion = new SqlConnection(cadena);
            await conexion.OpenAsync();
            await VerificarAislamientoAsync(conexion);

            // Protege IDs de catálogo y limpieza ante dos runners simultáneos.
            // El bloqueo pertenece a esta conexión y se libera al cerrarla.
            var bloqueo = await conexion.ExecuteScalarAsync<int>(@"
DECLARE @resultado int;
EXEC @resultado = sys.sp_getapplock
    @Resource = N'SuwoGym7B_Integracion_Exclusiva',
    @LockMode = N'Exclusive',
    @LockOwner = N'Session',
    @LockTimeout = 0;
SELECT @resultado;");
            if (bloqueo < 0)
                throw new InvalidOperationException(
                    "Hay otra ejecución de pruebas SQL en esta base.");

            Console.WriteLine("Base de PRUEBAS aislada verificada. Sin SQL en producción.");
            var suite = new PruebasSql(conexion, cadena);
            return await suite.EjecutarAsync();
        }
        catch (Exception e)
        {
            // No mostrar cadenas SQL, contraseñas, correos ni enlaces de recuperación.
            Console.Error.WriteLine("Error antes de completar pruebas SQL (" +
                                    e.GetType().Name + ").");
            return 1;
        }
    }

    private static async Task VerificarAislamientoAsync(SqlConnection conexion)
    {
        var actual = await conexion.ExecuteScalarAsync<string>("SELECT DB_NAME();");
        if (!string.Equals(actual, DbExclusiva, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("La conexión apunta a otra base de datos.");

        var marca = await conexion.ExecuteScalarAsync<string>(
            "SELECT CONVERT(nvarchar(30), value) FROM sys.extended_properties " +
            "WHERE class = 0 AND name = N'SuwoGym7BTestOnly';");

        if (marca != "YES")
            throw new InvalidOperationException(
                "Falta la marca explícita de base ficticia SuwoGym7BTestOnly.");

        var objetos = await conexion.ExecuteScalarAsync<int>(@"
SELECT CASE WHEN
  OBJECT_ID(N'dbo.Usuario', N'U') IS NOT NULL AND
  OBJECT_ID(N'dbo.ClaseProgramada', N'U') IS NOT NULL AND
  OBJECT_ID(N'dbo.Pago', N'U') IS NOT NULL AND
  OBJECT_ID(N'dbo.spReservarClaseMiembro', N'P') IS NOT NULL AND
  OBJECT_ID(N'dbo.spCancelarReservaClaseMiembro', N'P') IS NOT NULL AND
  OBJECT_ID(N'dbo.spRegistrarPagoMembresia', N'P') IS NOT NULL AND
  OBJECT_ID(N'dbo.spRecuperacionSolicitar', N'P') IS NOT NULL AND
  OBJECT_ID(N'dbo.spRecuperacionValidar', N'P') IS NOT NULL AND
  OBJECT_ID(N'dbo.spRecuperacionConsumir', N'P') IS NOT NULL AND
  OBJECT_ID(N'dbo.trg_Usuario_RevocarJwt', N'TR') IS NOT NULL AND
  COL_LENGTH(N'dbo.Usuario', N'VersionJwt') IS NOT NULL
THEN 1 ELSE 0 END;");
        if (objetos != 1)
            throw new InvalidOperationException("Faltan tablas/SP o migraciones 07/08.");

        var roles = await conexion.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.Rol WHERE IdRol IN (2,3);");
        if (roles != 2)
            throw new InvalidOperationException(
                "La BD ficticia debe incluir roles 2 y 3 (sin ejecutar el script 02 de usuarios).");
    }
}

internal sealed class PruebasSql(SqlConnection conexion, string cadena)
{
    private readonly string _etiqueta = Guid.NewGuid().ToString("N")[..12];
    private int _miembro1;
    private int _miembro2;
    private int _entrenador;
    private int _plan;
    private int _clase;
    private int _claseProgramada;
    private int _membresia;
    private int _fallidas;
    private int _aprobadas;

    public async Task<int> EjecutarAsync()
    {
        try
        {
            await CrearDatosFicticiosAsync();
            await VerificarAsync("Concurrencia: un único cupo disponible",
                ReservaConcurrenteAsync);
            await VerificarAsync("Cancelar reserva libera el cupo",
                CancelacionAsync);
            await VerificarAsync("Pago: subtotal, IVA 13 % y rechazo de duplicado",
                PagoConImpuestoAsync);
            await VerificarAsync("Recuperación: consumo único y revocación JWT",
                TokenUnSoloUsoAsync);
            await VerificarAsync("Recuperación: enlace expirado no cambia hash",
                TokenCaducadoAsync);
            await VerificarAsync("Desactivación/reactivación rota versión JWT",
                CambioEstadoAsync);
        }
        finally
        {
            // Únicamente se borran las filas creadas en esta ejecución.
            // No se elimina la base de pruebas ni objetos del esquema.
            try
            {
                await LimpiarDatosFicticiosAsync();
                Console.WriteLine("[OK] Limpieza de filas ficticias completada.");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("[FALLO] Limpieza: " + e.GetType().Name +
                    ". Revisar los datos de prueba T7B en la base aislada.");
                _fallidas++;
            }
        }

        Console.WriteLine("Resultado: " + _aprobadas + " aprobadas, " +
            _fallidas + " fallidas.");
        return _fallidas == 0 ? 0 : 1;
    }

    private async Task VerificarAsync(string titulo, Func<Task> prueba)
    {
        try
        {
            await prueba();
            Console.WriteLine("[OK] " + titulo);
            _aprobadas++;
        }
        catch (Exception e)
        {
            // Diagnóstico limitado a tipo de excepción, sin datos sensibles.
            Console.Error.WriteLine("[FALLO] " + titulo + " (" +
                e.GetType().Name + ").");
            _fallidas++;
        }
    }

    private async Task CrearDatosFicticiosAsync()
    {
        _plan = await conexion.ExecuteScalarAsync<int>(
            "SELECT ISNULL(MAX(IdPlan), 0) + 1 FROM dbo.PlanMembresia;");
        await conexion.ExecuteAsync(@"
INSERT dbo.PlanMembresia(IdPlan, NombrePlan, DuracionDias, Precio)
VALUES (@id, @nombre, 30, 10000.00);",
            new { id = _plan, nombre = "Prueba7B " + _etiqueta });

        _clase = await conexion.ExecuteScalarAsync<int>(
            "SELECT ISNULL(MAX(IdClase), 0) + 1 FROM dbo.Clase;");
        await conexion.ExecuteAsync(@"
INSERT dbo.Clase(IdClase, NombreClase)
VALUES (@id, @nombre);",
            new { id = _clase, nombre = "Prueba7B " + _etiqueta });

        _entrenador = await CrearUsuarioAsync("entrenador", 2);
        _miembro1 = await CrearUsuarioAsync("miembro1", 3);
        _miembro2 = await CrearUsuarioAsync("miembro2", 3);

        _membresia = await CrearMembresiaAsync(_miembro1);
        await CrearMembresiaAsync(_miembro2);

        var inicio = DateTime.Now.Date.AddDays(3).AddHours(9);
        _claseProgramada = await conexion.ExecuteScalarAsync<int>(@"
INSERT dbo.ClaseProgramada
  (IdEntrenador, IdClase, FechaHoraInicio, FechaHoraFin,
   CupoMaximo, CupoActual, Estado)
OUTPUT INSERTED.IdClaseProg
VALUES (@entrenador, @clase, @inicio, @fin, 1, 0, 1);",
            new
            {
                entrenador = _entrenador,
                clase = _clase,
                inicio,
                fin = inicio.AddHours(1)
            });
    }

    private async Task<int> CrearUsuarioAsync(string tipo, int rol)
    {
        var correo = "t7b-" + _etiqueta + "-" + tipo + "@example.invalid";
        // Contraseña aleatoria no registrada ni mostrada; jamás se envía correo.
        var hash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N"));
        return await conexion.ExecuteScalarAsync<int>(@"
INSERT dbo.Usuario
  (IdRol, Nombre, Apellido, CorreoElectronico, Identificacion,
   ContrasennaHash, DebeCambiarContrasenna, Estado, FechaRegistro)
OUTPUT INSERTED.IdUsuario
VALUES (@rol, 'Prueba7B', @tipo, @correo, @identificacion, @hash, 0, 1, SYSDATETIME());",
            new
            {
                rol,
                tipo,
                correo,
                identificacion = "T7B-" + _etiqueta + "-" + tipo,
                hash
            });
    }

    private async Task<int> CrearMembresiaAsync(int idUsuario)
    {
        return await conexion.ExecuteScalarAsync<int>(@"
INSERT dbo.MembresiaUsuario
  (IdUsuario, IdPlan, FechaRegistro, EstadoMembresia, FechaVencimiento)
OUTPUT INSERTED.IdMembresia
VALUES (@usuario, @plan, SYSDATETIME(), N'Activa',
        DATEADD(DAY, 30, SYSDATETIME()));",
            new { usuario = idUsuario, plan = _plan });
    }

    private async Task ReservaConcurrenteAsync()
    {
        var intentos = await Task.WhenAll(
            IntentarReservaAsync(_miembro1),
            IntentarReservaAsync(_miembro2));

        Requerir(intentos.Count(x => x.Aceptada) == 1,
            "Dos peticiones simultáneas deben competir por un único cupo.");

        var cupo = await conexion.ExecuteScalarAsync<int>(
            "SELECT CupoActual FROM dbo.ClaseProgramada WHERE IdClaseProg=@id;",
            new { id = _claseProgramada });
        var activas = await conexion.ExecuteScalarAsync<int>(@"
SELECT COUNT(*) FROM dbo.ReservaClase WHERE IdClaseProg=@id AND Estado=1;",
            new { id = _claseProgramada });

        Requerir(cupo == 1 && activas == 1,
            "El cupo y las reservas activas deben coincidir.");
    }

    private async Task<(int Miembro, bool Aceptada)> IntentarReservaAsync(int idMiembro)
    {
        await using var otraConexion = new SqlConnection(cadena);
        await otraConexion.OpenAsync();
        try
        {
            await otraConexion.ExecuteAsync("dbo.spReservarClaseMiembro",
                new { IdMiembro = idMiembro, IdClaseProg = _claseProgramada },
                commandType: CommandType.StoredProcedure);
            return (idMiembro, true);
        }
        catch (SqlException)
        {
            return (idMiembro, false);
        }
    }

    private async Task CancelacionAsync()
    {
        var reserva = await conexion.QuerySingleOrDefaultAsync<ReservaFicticia>(@"
SELECT TOP (1) IdReserva, IdMiembro
FROM dbo.ReservaClase WHERE IdClaseProg=@id AND Estado=1;",
            new { id = _claseProgramada });
        Requerir(reserva is not null, "Falta la reserva creada para cancelar.");

        await conexion.ExecuteAsync("dbo.spCancelarReservaClaseMiembro",
            new { reserva!.IdMiembro, reserva.IdReserva },
            commandType: CommandType.StoredProcedure);

        var cupo = await conexion.ExecuteScalarAsync<int>(
            "SELECT CupoActual FROM dbo.ClaseProgramada WHERE IdClaseProg=@id;",
            new { id = _claseProgramada });
        var activas = await conexion.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.ReservaClase WHERE IdClaseProg=@id AND Estado=1;",
            new { id = _claseProgramada });
        Requerir(cupo == 0 && activas == 0,
            "La cancelación debe liberar el cupo sin dejar reserva activa.");
    }

    private async Task PagoConImpuestoAsync()
    {
        await conexion.ExecuteAsync("dbo.spRegistrarPagoMembresia",
            new
            {
                IdMembresia = _membresia,
                MetodoPago = "Tarjeta",
                NumeroReferencia = "T7B-" + _etiqueta
            }, commandType: CommandType.StoredProcedure);

        var pago = await conexion.QuerySingleAsync<PagoFicticio>(@"
SELECT Subtotal, Impuesto, Total FROM dbo.Pago WHERE IdMembresia=@id;",
            new { id = _membresia });
        Requerir(pago.Subtotal == 10000m && pago.Impuesto == 1300m &&
                 pago.Total == 11300m, "La conciliación de IVA no coincide.");

        var rechazado = false;
        try
        {
            await conexion.ExecuteAsync("dbo.spRegistrarPagoMembresia",
                new
                {
                    IdMembresia = _membresia,
                    MetodoPago = "Tarjeta",
                    NumeroReferencia = "T7B-" + _etiqueta + "-DUP"
                }, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException)
        {
            rechazado = true;
        }

        var cantidad = await conexion.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.Pago WHERE IdMembresia=@id;",
            new { id = _membresia });
        Requerir(rechazado && cantidad == 1,
            "Un pago duplicado no debe crear un segundo registro.");
    }

    private async Task TokenUnSoloUsoAsync()
    {
        var antes = await conexion.QuerySingleAsync<UsuarioFicticio>(@"
SELECT VersionJwt, ContrasennaHash FROM dbo.Usuario WHERE IdUsuario=@id;",
            new { id = _miembro1 });
        var token = SHA256.HashData(RandomNumberGenerator.GetBytes(32));
        var correo = "t7b-" + _etiqueta + "-miembro1@example.invalid";

        var receptor = await conexion.ExecuteScalarAsync<int>(
            "dbo.spRecuperacionSolicitar",
            new
            {
                CorreoElectronico = correo,
                TokenHash = token,
                ExpiraUtc = DateTime.UtcNow.AddMinutes(15)
            }, commandType: CommandType.StoredProcedure);
        Requerir(receptor == _miembro1, "No se registró el token de prueba.");

        var valido = await conexion.ExecuteScalarAsync<bool>(
            "dbo.spRecuperacionValidar",
            new { TokenHash = token }, commandType: CommandType.StoredProcedure);
        Requerir(valido, "Un token nuevo debe ser válido.");

        var nuevoHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N"));
        var consumido = await conexion.ExecuteScalarAsync<bool>(
            "dbo.spRecuperacionConsumir",
            new { TokenHash = token, ContrasennaHash = nuevoHash },
            commandType: CommandType.StoredProcedure);

        var despues = await conexion.QuerySingleAsync<UsuarioFicticio>(@"
SELECT VersionJwt, ContrasennaHash FROM dbo.Usuario WHERE IdUsuario=@id;",
            new { id = _miembro1 });

        Requerir(consumido && despues.ContrasennaHash == nuevoHash &&
                 despues.ContrasennaHash != antes.ContrasennaHash &&
                 despues.VersionJwt != antes.VersionJwt,
            "El cambio de hash debe revocar inmediatamente la versión JWT.");

        var segundoUso = await conexion.ExecuteScalarAsync<bool>(
            "dbo.spRecuperacionConsumir",
            new
            {
                TokenHash = token,
                ContrasennaHash = BCrypt.Net.BCrypt.HashPassword(
                    Guid.NewGuid().ToString("N"))
            }, commandType: CommandType.StoredProcedure);
        Requerir(!segundoUso, "Un token utilizado no debe aceptarse otra vez.");

        var invalido = await conexion.ExecuteScalarAsync<bool>(
            "dbo.spRecuperacionValidar",
            new { TokenHash = token }, commandType: CommandType.StoredProcedure);
        Requerir(!invalido, "Un token consumido debe dejar de validarse.");
    }

    private async Task TokenCaducadoAsync()
    {
        var token = SHA256.HashData(RandomNumberGenerator.GetBytes(32));
        var antes = await conexion.ExecuteScalarAsync<string>(@"
SELECT ContrasennaHash FROM dbo.Usuario WHERE IdUsuario=@id;",
            new { id = _miembro1 });

        await conexion.ExecuteAsync(@"
INSERT dbo.RecuperacionContrasennaToken
  (IdUsuario, TokenHash, CreadoUtc, ExpiraUtc)
VALUES (@id, @hash, DATEADD(DAY, -2, SYSUTCDATETIME()),
        DATEADD(DAY, -1, SYSUTCDATETIME()));",
            new { id = _miembro1, hash = token });

        var valido = await conexion.ExecuteScalarAsync<bool>(
            "dbo.spRecuperacionValidar",
            new { TokenHash = token }, commandType: CommandType.StoredProcedure);
        var consumido = await conexion.ExecuteScalarAsync<bool>(
            "dbo.spRecuperacionConsumir",
            new
            {
                TokenHash = token,
                ContrasennaHash = BCrypt.Net.BCrypt.HashPassword(
                    Guid.NewGuid().ToString("N"))
            }, commandType: CommandType.StoredProcedure);
        var despues = await conexion.ExecuteScalarAsync<string>(@"
SELECT ContrasennaHash FROM dbo.Usuario WHERE IdUsuario=@id;",
            new { id = _miembro1 });
        Requerir(!valido && !consumido && antes == despues,
            "Un enlace vencido no debe modificar el hash.");
    }

    private async Task CambioEstadoAsync()
    {
        var inicial = await conexion.ExecuteScalarAsync<Guid>(
            "SELECT VersionJwt FROM dbo.Usuario WHERE IdUsuario=@id;",
            new { id = _miembro2 });

        await conexion.ExecuteAsync(
            "UPDATE dbo.Usuario SET Estado=0 WHERE IdUsuario=@id;",
            new { id = _miembro2 });
        var desactivado = await conexion.ExecuteScalarAsync<Guid>(
            "SELECT VersionJwt FROM dbo.Usuario WHERE IdUsuario=@id;",
            new { id = _miembro2 });

        await conexion.ExecuteAsync(
            "UPDATE dbo.Usuario SET Estado=1 WHERE IdUsuario=@id;",
            new { id = _miembro2 });
        var reactivado = await conexion.ExecuteScalarAsync<Guid>(
            "SELECT VersionJwt FROM dbo.Usuario WHERE IdUsuario=@id;",
            new { id = _miembro2 });

        Requerir(inicial != desactivado && desactivado != reactivado &&
                 inicial != reactivado,
            "Desactivar/reactivar debe rotar la versión en cada transición.");
    }

    private async Task LimpiarDatosFicticiosAsync()
    {
        if (_claseProgramada > 0)
        {
            await conexion.ExecuteAsync(
                "DELETE dbo.ReservaClase WHERE IdClaseProg=@id;",
                new { id = _claseProgramada });
        }

        if (_membresia > 0)
        {
            await conexion.ExecuteAsync(
                "DELETE dbo.Pago WHERE IdMembresia=@id;",
                new { id = _membresia });
        }

        var ids = new[] { _miembro1, _miembro2, _entrenador }
            .Where(id => id > 0).ToArray();
        if (ids.Length > 0)
        {
            await conexion.ExecuteAsync(
                "DELETE dbo.RecuperacionContrasennaToken WHERE IdUsuario IN @ids;",
                new { ids });
            await conexion.ExecuteAsync(
                "DELETE dbo.MembresiaUsuario WHERE IdUsuario IN @ids;",
                new { ids });
        }

        if (_claseProgramada > 0)
        {
            await conexion.ExecuteAsync(
                "DELETE dbo.ClaseProgramada WHERE IdClaseProg=@id;",
                new { id = _claseProgramada });
        }

        if (_clase > 0)
        {
            await conexion.ExecuteAsync(
                "DELETE dbo.Clase WHERE IdClase=@id;",
                new { id = _clase });
        }

        if (_plan > 0)
        {
            await conexion.ExecuteAsync(
                "DELETE dbo.PlanMembresia WHERE IdPlan=@id;",
                new { id = _plan });
        }

        if (ids.Length > 0)
        {
            await conexion.ExecuteAsync(
                "DELETE dbo.Usuario WHERE IdUsuario IN @ids;",
                new { ids });
        }
    }

    private static void Requerir(bool condicion, string descripcion)
    {
        if (!condicion)
            throw new InvalidOperationException(descripcion);
    }

    private sealed class ReservaFicticia
    {
        public int IdReserva { get; set; }
        public int IdMiembro { get; set; }
    }

    private sealed class PagoFicticio
    {
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }

    private sealed class UsuarioFicticio
    {
        public Guid VersionJwt { get; set; }
        public string ContrasennaHash { get; set; } = string.Empty;
    }
}
