using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SuwoGym_Web.Deployment;

/// <summary>Comprueba conectividad HTTPS con API sin utilizar JWT ni datos personales.</summary>
public sealed class ApiReadinessCheck(
    IHttpClientFactory httpFactory, IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var origen = configuration["ConnectionStringsWeb:urlApi"];
        if (!Uri.TryCreate(origen, UriKind.Absolute, out var uri))
            return HealthCheckResult.Unhealthy();

        try
        {
            using var cliente = httpFactory.CreateClient();
            cliente.Timeout = TimeSpan.FromSeconds(5);
            using var respuesta = await cliente.GetAsync(
                origen!.TrimEnd('/') + "/health/ready", cancellationToken);
            return respuesta.IsSuccessStatusCode
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
        catch (Exception) when (!cancellationToken.IsCancellationRequested)
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}
