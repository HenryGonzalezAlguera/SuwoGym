using System.Net;
using System.Net.Http.Headers;

namespace SuwoGym_Web.Middleware;

/// <summary>
/// Valida la sesión JWT en la API antes de servir rutas privadas MVC.
/// No guarda ni compara permisos con un cache local. La API consulta SQL.
/// </summary>
public sealed class ValidarSesionJwtMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IHttpClientFactory httpFactory,
        IConfiguration config)
    {
        var path = context.Request.Path;
        var privada =
            path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Entrenador", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Miembro", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Usuario", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Home/Principal", StringComparison.OrdinalIgnoreCase);

        if (!privada)
        {
            await next(context);
            return;
        }

        var session = context.Session;
        var jwt = session.GetString("Token");
        if (session.GetString("Autenticado") != "1" ||
            string.IsNullOrWhiteSpace(jwt))
        {
            session.Clear();
            context.Response.Redirect("/Home/Index");
            return;
        }

        var api = config["ConnectionStringsWeb:urlApi"];
        if (string.IsNullOrWhiteSpace(api))
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return;
        }

        try
        {
            using var client = httpFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                api.TrimEnd('/') + "/api/Usuario/ValidarSesionAPI");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);

            using var response = await client.SendAsync(
                request, context.RequestAborted);

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                session.Clear();
                context.Response.Redirect("/Home/Index");
                return;
            }

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                context.Response.StatusCode =
                    StatusCodes.Status503ServiceUnavailable;
                return;
            }
        }
        catch (HttpRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return;
        }
        catch (TaskCanceledException) when (!context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return;
        }

        await next(context);
    }
}
