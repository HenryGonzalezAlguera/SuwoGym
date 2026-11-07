namespace SuwoGym_Web.Services;

/// <summary>
/// Reglas mínimas de publicación. En desarrollo no se requieren.
/// La ubicación de claves debe ser un volumen persistente externo al sitio.
/// </summary>
public static class ConfiguracionPreproduccionWeb
{
    public static void Validar(IConfiguration config)
    {
        var hosts = config["AllowedHosts"];
        var opciones = hosts?.Split(';', StringSplitOptions.TrimEntries |
                                          StringSplitOptions.RemoveEmptyEntries);
        if (opciones is null || opciones.Length == 0 ||
            opciones.Any(x => x == "*" || x.Contains("localhost",
                StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException(
                "AllowedHosts de WEB debe contener los dominios reales.");

        var urlApi = config["ConnectionStringsWeb:urlApi"];
        if (!Uri.TryCreate(urlApi, UriKind.Absolute, out var url) ||
            url.Scheme != Uri.UriSchemeHttps || url.IsLoopback ||
            !string.IsNullOrEmpty(url.Query) || !string.IsNullOrEmpty(url.Fragment))
            throw new InvalidOperationException(
                "ConnectionStringsWeb:urlApi requiere HTTPS y un host no local.");

        var ruta = config["Deployment:DataProtectionKeysPath"];
        if (string.IsNullOrWhiteSpace(ruta) ||
            !Path.IsPathFullyQualified(ruta) || !Directory.Exists(ruta))
            throw new InvalidOperationException(
                "Deployment:DataProtectionKeysPath debe ser una carpeta existente en volumen persistente.");

        var rutaNormalizada = Path.GetFullPath(ruta);
        if (rutaNormalizada.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Any(seg => seg.Equals("wwwroot", StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException(
                "Las claves Data Protection no pueden almacenarse en wwwroot.");
    }
}
