using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SuwoGym_API.Services;

/// <summary>
/// Ready solo si SQL está accesible y están aplicadas las migraciones 07 y 08.
/// La respuesta HTTP sigue siendo genérica. Los registros solo contienen fase,
/// código SQL o tipo de error; nunca cadenas de conexión ni excepciones completas.
/// </summary>
public sealed class SqlReadinessCheck(
    IConfiguration configuration,
    ILogger<SqlReadinessCheck> logger) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var fase = "configuracion";
        try
        {
            var cadena = configuration["ConnectionStringsApi:DefaultConnection"];
            if (string.IsNullOrWhiteSpace(cadena))
            {
                logger.LogWarning("SUWO_SQL_READY_CONFIG_AUSENTE");
                return HealthCheckResult.Unhealthy();
            }

            var datos = new SqlConnectionStringBuilder(cadena)
            {
                // Mantener temporalmente el timeout previo para diagnosticar
                // si la demora es la causa del 503.
                ConnectTimeout = 5
            };
            fase = "conexion";
            await using var conexion = new SqlConnection(datos.ConnectionString);
            await conexion.OpenAsync(cancellationToken);

            fase = "consulta";
            await using var consulta = conexion.CreateCommand();
            consulta.CommandType = CommandType.Text;
            consulta.CommandTimeout = 5;
            consulta.CommandText = @"SELECT CASE WHEN
                OBJECT_ID(N'dbo.Usuario', N'U') IS NOT NULL AND
                OBJECT_ID(N'dbo.spSeguridadConsultarVersionJwt', N'P') IS NOT NULL AND
                OBJECT_ID(N'dbo.spRecuperacionConsumir', N'P') IS NOT NULL AND
                COL_LENGTH(N'dbo.Usuario', N'VersionJwt') IS NOT NULL
                THEN 1 ELSE 0 END";
            var resultado = await consulta.ExecuteScalarAsync(cancellationToken);
            if (Convert.ToInt32(resultado) != 1)
            {
                logger.LogWarning("SUWO_SQL_READY_ESQUEMA_NO_DISPONIBLE");
                return HealthCheckResult.Unhealthy();
            }

            return HealthCheckResult.Healthy();
        }
        catch (SqlException ex) when (!cancellationToken.IsCancellationRequested)
        {
            // No registrar ex ni ex.Message: podrían incluir el servidor o datos sensibles.
            logger.LogWarning(
                "SUWO_SQL_READY_ERROR_SQL Fase={Fase} Codigo={Codigo} Estado={Estado}",
                fase, ex.Number, ex.State);
            return HealthCheckResult.Unhealthy();
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            // Solo nombre de tipo, no mensaje ni stack trace.
            logger.LogWarning(
                "SUWO_SQL_READY_ERROR_TIPO Fase={Fase} Tipo={Tipo}",
                fase, ex.GetType().Name);
            return HealthCheckResult.Unhealthy();
        }
    }
}
