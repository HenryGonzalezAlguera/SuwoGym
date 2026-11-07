using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SuwoGym_API.Services;

/// <summary>
/// Ready solo si SQL está accesible y están aplicadas las migraciones 07 y 08.
/// El endpoint devuelve el estado genérico y no revela rutas ni errores SQL.
/// </summary>
public sealed class SqlReadinessCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var cadena = configuration["ConnectionStringsApi:DefaultConnection"];
            if (string.IsNullOrWhiteSpace(cadena))
                return HealthCheckResult.Unhealthy();

            var datos = new SqlConnectionStringBuilder(cadena)
            {
                ConnectTimeout = 5
            };
            await using var conexion = new SqlConnection(datos.ConnectionString);
            await conexion.OpenAsync(cancellationToken);
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
            return Convert.ToInt32(resultado) == 1
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
        catch (Exception) when (!cancellationToken.IsCancellationRequested)
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}
