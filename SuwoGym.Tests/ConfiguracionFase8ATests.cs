using Microsoft.Extensions.Configuration;
using SuwoGym_API.Services;
using SuwoGym_Web.Deployment;
using Xunit;

namespace SuwoGym.Tests;

/// <summary>
/// Reglas de configuración preproducción, sin conexión a SQL ni correos.
/// Datos y dominios de este archivo son ficticios.
/// </summary>
public class ConfiguracionFase8ATests
{
    [Fact]
    public void ApiAceptaConfiguracionDeStagingConTlsYHttps()
    {
        var configuracion = ConfiguracionApi();
        ConfiguracionPreproduccionApi.Validar(configuracion);
    }

    [Theory]
    [InlineData("AllowedHosts", "*")]
    [InlineData("Recuperacion:UrlBaseWeb", "http://localhost:7035")]
    [InlineData("ConnectionStringsApi:DefaultConnection",
        "Server=sql.example.test;Database=Gym_Stage;Encrypt=True;TrustServerCertificate=True")]
    [InlineData("Correos:AppPassword", "")]
    [InlineData("Jwt:SecretKey", "corta")]
    public void ApiBloqueaConfiguracionNoSegura(string clave, string valor)
    {
        var configuracion = ConfiguracionApi(new Dictionary<string, string?>
        {
            [clave] = valor
        });
        Assert.Throws<InvalidOperationException>(() =>
            ConfiguracionPreproduccionApi.Validar(configuracion));
    }

    [Fact]
    public void WebAceptaHttpsYRutaPersistente()
    {
        ConfiguracionPreproduccionWeb.Validar(ConfiguracionWeb());
    }

    [Theory]
    [InlineData("AllowedHosts", "*")]
    [InlineData("ConnectionStringsWeb:urlApi", "http://localhost:7129")]
    [InlineData("Deployment:DataProtectionKeysPath", "ruta/relativa")]
    public void WebBloqueaConfiguracionNoSegura(string clave, string valor)
    {
        var configuracion = ConfiguracionWeb(new Dictionary<string, string?>
        {
            [clave] = valor
        });
        Assert.Throws<InvalidOperationException>(() =>
            ConfiguracionPreproduccionWeb.Validar(configuracion));
    }

    private static IConfiguration ConfiguracionApi(
        Dictionary<string, string?>? reemplazos = null)
    {
        var datos = new Dictionary<string, string?>
        {
            ["Jwt:SecretKey"] = new string('X', 64),
            ["Jwt:Issuer"] = "SuwoGym.API",
            ["Jwt:Audience"] = "SuwoGym.Web",
            ["Correos:Correo"] = "remitente@example.invalid",
            ["Correos:AppPassword"] = "CLAVE-FICTICIA-NO-OPERATIVA",
            ["AllowedHosts"] = "api.staging.example.test",
            ["Recuperacion:UrlBaseWeb"] = "https://web.staging.example.test",
            ["ConnectionStringsApi:DefaultConnection"] =
                "Server=sql.staging.example.test;Database=SuwoGym_Staging;" +
                "Encrypt=True;TrustServerCertificate=False;Integrated Security=True"
        };
        if (reemplazos is not null)
            foreach (var campo in reemplazos)
                datos[campo.Key] = campo.Value;
        return new ConfigurationBuilder().AddInMemoryCollection(datos).Build();
    }

    private static IConfiguration ConfiguracionWeb(
        Dictionary<string, string?>? reemplazos = null)
    {
        var datos = new Dictionary<string, string?>
        {
            ["AllowedHosts"] = "web.staging.example.test",
            ["ConnectionStringsWeb:urlApi"] = "https://api.staging.example.test",
            ["Deployment:DataProtectionKeysPath"] = Path.GetTempPath()
        };
        if (reemplazos is not null)
            foreach (var campo in reemplazos)
                datos[campo.Key] = campo.Value;
        return new ConfigurationBuilder().AddInMemoryCollection(datos).Build();
    }
}
