using Microsoft.Data.SqlClient;

namespace SuwoGym_API.Services;

/// <summary>
/// Validación de configuración para Staging/Production.
/// No registra ni muestra secretos. No se ejecuta en Development.
/// </summary>
public static class ConfiguracionPreproduccionApi
{
    public static void Validar(IConfiguration config)
    {
        static string Requerido(IConfiguration c, string clave) =>
            !string.IsNullOrWhiteSpace(c[clave])
                ? c[clave]!
                : throw new InvalidOperationException(
                    "Falta una configuración obligatoria de la API para publicación.");

        var jwt = Requerido(config, "Jwt:SecretKey");
        if (System.Text.Encoding.UTF8.GetByteCount(jwt) < 32)
            throw new InvalidOperationException("Jwt:SecretKey es demasiado corta.");

        Requerido(config, "Jwt:Issuer");
        Requerido(config, "Jwt:Audience");
        Requerido(config, "Correos:Correo");
        Requerido(config, "Correos:AppPassword");
        ValidarHosts(Requerido(config, "AllowedHosts"));

        var baseWeb = Requerido(config, "Recuperacion:UrlBaseWeb");
        if (!Uri.TryCreate(baseWeb, UriKind.Absolute, out var web) ||
            web.Scheme != Uri.UriSchemeHttps || web.IsLoopback ||
            !string.IsNullOrEmpty(web.Query) || !string.IsNullOrEmpty(web.Fragment))
            throw new InvalidOperationException(
                "Recuperacion:UrlBaseWeb debe ser un origen HTTPS público.");

        SqlConnectionStringBuilder sql;
        try
        {
            sql = new SqlConnectionStringBuilder(
                Requerido(config, "ConnectionStringsApi:DefaultConnection"));
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException(
                "La conexión SQL de la API no tiene un formato válido.");
        }

        if (string.IsNullOrWhiteSpace(sql.DataSource) ||
            string.IsNullOrWhiteSpace(sql.InitialCatalog) ||
            sql.TrustServerCertificate ||
            sql.Encrypt == SqlConnectionEncryptOption.Optional)
            throw new InvalidOperationException(
                "SQL requiere servidor, base, TLS y TrustServerCertificate=False.");
    }

    private static void ValidarHosts(string hosts)
    {
        var partes = hosts.Split(';', StringSplitOptions.TrimEntries |
                                        StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 0 || partes.Any(x => x == "*" || x.Contains("localhost",
                StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException(
                "AllowedHosts debe restringirse a dominios de despliegue.");
    }
}
