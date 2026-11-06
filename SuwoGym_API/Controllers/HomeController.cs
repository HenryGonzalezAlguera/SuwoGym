using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using System.Data;
using System.Net;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SuwoGym_API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(
        IConfiguration _databasekey,
        IUtilesService _utiles,
        ILogger<HomeController>? _logger = null) : ControllerBase
    {

        #region Registro API

        [HttpGet("MembresiasAPI")]
        public IActionResult ObtenerMembresiasAPI()
        {
            using var context = new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<HomePageMembresiasResponseModel>(
                    "spObtenerPlanMembresiaDetalle");

            if (response.Any())
                return Ok(response);

            return NotFound("No hay membresías registradas");
        }


        [HttpPost("RegistroAPI")]
        public IActionResult RegistroAPI(
            UsuarioRegistroModel model)
        {
            try
            {
                using var context = new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

                var parameters = new DynamicParameters();

                parameters.Add(
                    "@Nombre",
                    model.nombre);

                parameters.Add(
                    "@Apellido",
                    model.apellido);

                parameters.Add(
                    "@CorreoElectronico",
                    model.correoElectronico);

                parameters.Add(
                    "@ContrasennaHash",
                    model.contrasenna);

                parameters.Add(
                    "@Identificacion",
                    model.identificacion);
                
                parameters.Add(
                    "@PlanMembresia",
                    model.PlanMembresia);

                var response =
                    context.QueryFirstOrDefault
                    <DatoUsuarioResponseModel>(
                        "spRegistrarUsuario",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response != null &&
                    response.IdUsuario > 0)
                {
                    return Ok(response);
                }

                return BadRequest(
                    "La información no se pudo registrar correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Iniciar sesión API

        [HttpPost("IniciarSesionAPI")]
        [EnableRateLimiting("auth-login")]
        public IActionResult IniciarSesionAPI(
            UsuarioLoginModel model)
        {
            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@CorreoElectronico",
                    model.correoElectronico);

            
                var response =
                    context.QueryFirstOrDefault
                    <DatoUsuarioResponseModel>(
                        "spIniciarSesionUsuario",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response == null)
                {
                    return NotFound(
                        "El correo o la contraseña son incorrectos.");
                }

                var contrasennaCorrecta =
                    BCrypt.Net.BCrypt.Verify(
                        model.contrasenna,
                        response.ContrasennaHash);

                if (!contrasennaCorrecta)
                {
                    return NotFound(
                        "El correo o la contraseña son incorrectos.");
                }

                response.Token =
                    _utiles.GenerarToken(
                        response.IdUsuario,
                        response.IdRol);

                return Ok(response);
            }
            catch (Exception)
            {
                // Nunca devolver detalles de SQL Server ni del hash al cliente.
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "No         #region Recuperar acceso API — Fase 6B-2

        // Respuesta genérica para evitar averiguar si existe un correo.
        private const string RespuestaRecuperacion =
            "Si el correo pertenece a una cuenta activa, recibirá un enlace de recuperación.";

        [HttpPost("RecuperarAccesoAPI")]
        [EnableRateLimiting("auth-recovery")]
        public async Task<IActionResult> RecuperarAccesoAPI(
            RecuperarAccesoRequestModel model)
        {
            // Sin SMTP, no generar tokens ni tocar la contraseña actual.
            if (string.IsNullOrWhiteSpace(_databasekey["Correos:Correo"]) ||
                string.IsNullOrWhiteSpace(_databasekey["Correos:AppPassword"]))
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    "El correo de recuperación no está configurado.");
            }

            var urlBase = _databasekey["Recuperacion:UrlBaseWeb"];
            if (!Uri.TryCreate(urlBase, UriKind.Absolute, out var origen) ||
                origen.Scheme != Uri.UriSchemeHttps ||
                !string.IsNullOrEmpty(origen.Query) ||
                !string.IsNullOrEmpty(origen.Fragment))
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    "La dirección de recuperación no está configurada.");
            }

            var nuevo = TokenRecuperacionSeguro.Crear();
            try
            {
                using var context = new SqlConnection(
                    _databasekey["ConnectionStringsApi:DefaultConnection"]);

                var destinatario =
                    await context.QuerySingleOrDefaultAsync<DestinatarioRecuperacionModel>(
                        "dbo.spRecuperacionSolicitar",
                        new
                        {
                            CorreoElectronico = model.CorreoElectronico,
                            TokenHash = nuevo.Hash,
                            ExpiraUtc = DateTime.UtcNow.AddMinutes(15)
                        },
                        commandType: CommandType.StoredProcedure);

                if (destinatario is not null)
                {
                    try
                    {
                        var enlace = new Uri(
                            new Uri(origen.GetLeftPart(UriPartial.Authority) + "/"),
                            "Home/RestablecerContrasenna?token=" +
                            Uri.EscapeDataString(nuevo.Token)).AbsoluteUri;

                        var ruta = Path.Combine(AppContext.BaseDirectory,
                            "Templates", "RecuperarAcceso.html");
                        var plantilla = await System.IO.File.ReadAllTextAsync(ruta);
                        plantilla = plantilla.Replace("{{NOMBRE}}",
                                WebUtility.HtmlEncode(destinatario.Nombre))
                            .Replace("{{ENLACE}}", WebUtility.HtmlEncode(enlace));

                        await _utiles.EnviarCorreoAsync(
                            destinatario.CorreoElectronico,
                            "Restablecimiento de contraseña SUWÓ GYM",
                            plantilla);
                    }
                    catch (Exception)
                    {
                        // Revocar el enlace que no se pudo entregar.
                        _logger?.LogWarning("No fue posible enviar el enlace de recuperación.");
                        try
                        {
                            await context.ExecuteAsync(
                                "dbo.spRecuperacionRevocar",
                                new { TokenHash = nuevo.Hash },
                                commandType: CommandType.StoredProcedure);
                        }
                        catch (Exception)
                        {
                            _logger?.LogWarning("No se pudo revocar el enlace no entregado.");
                        }
                    }
                }

                return Accepted(new { Mensaje = RespuestaRecuperacion });
            }
            catch (Exception)
            {
                // Error de infraestructura, no devolver excepciones SQL.
                _logger?.LogError("Falló la solicitud de recuperación.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    "El servicio de recuperación no está disponible.");
            }
        }

        [HttpPost("ValidarEnlaceRecuperacionAPI")]
        [EnableRateLimiting("auth-reset")]
        public async Task<IActionResult> ValidarEnlaceRecuperacionAPI(
            RecuperacionTokenRequestModel model)
        {
            if (!TokenRecuperacionSeguro.TryObtenerHash(model.Token, out var hash))
                return NotFound();

            try
            {
                using var context = new SqlConnection(
                    _databasekey["ConnectionStringsApi:DefaultConnection"]);
                var valido = await context.QuerySingleAsync<bool>(
                    "dbo.spRecuperacionValidar",
                    new { TokenHash = hash },
                    commandType: CommandType.StoredProcedure);

                return valido ? Ok() : NotFound();
            }
            catch (Exception)
            {
                _logger?.LogError("Falló la validación de enlace de recuperación.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
        }

        [HttpPost("RestablecerContrasennaAPI")]
        [EnableRateLimiting("auth-reset")]
        public async Task<IActionResult> RestablecerContrasennaAPI(
            RestablecerContrasennaRequestModel model)
        {
            if (!TokenRecuperacionSeguro.TryObtenerHash(model.Token, out var hash) ||
                Encoding.UTF8.GetByteCount(model.NuevaContrasenna) > 72)
                return BadRequest("El enlace o la contraseña no es válido.");

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.NuevaContrasenna);
                using var context = new SqlConnection(
                    _databasekey["ConnectionStringsApi:DefaultConnection"]);

                var actualizado = await context.QuerySingleAsync<bool>(
                    "dbo.spRecuperacionConsumir",
                    new { TokenHash = hash, ContrasennaHash = passwordHash },
                    commandType: CommandType.StoredProcedure);

                if (!actualizado)
                    return BadRequest("El enlace no es válido o ya expiró.");

                return Ok(new { Mensaje = "Contraseña actualizada correctamente." });
            }
            catch (Exception)
            {
                _logger?.LogError("Falló el consumo del enlace de recuperación.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
        }

          "No se ha recuperado su acceso, por favor intente nuevamente.");
        }

        #endregion

    }
}