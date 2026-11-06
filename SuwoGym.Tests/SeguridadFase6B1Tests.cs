using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SuwoGym_API.Models;
using Microsoft.AspNetCore.RateLimiting;
using SuwoGym_Web.Controllers;
using Xunit;

namespace SuwoGym.Tests;

/// <summary>
/// Pruebas de contratos de seguridad: no requieren SQL Server ni SMTP.
/// Las pruebas de token real en navegador y las políticas del middleware
/// se completan manualmente en el entorno Development.
/// </summary>
public class SeguridadFase6B1Tests
{
    [Theory]
    [InlineData(typeof(AdminController))]
    [InlineData(typeof(EntrenadorController))]
    [InlineData(typeof(MiembroController))]
    [InlineData(typeof(UsuarioController))]
    public void ControladoresDeRolExigenAntifalsificacionParaPost(Type controlador)
    {
        var filtro = controlador.GetCustomAttribute<AutoValidateAntiforgeryTokenAttribute>();
        Assert.NotNull(filtro);
    }

    [Fact]
    public void CerrarSesionSoloAceptaPostConAntifalsificacion()
    {
        var accion = typeof(HomeController).GetMethod(
            nameof(HomeController.Logout), Type.EmptyTypes);

        Assert.NotNull(accion);
        Assert.NotNull(accion!.GetCustomAttribute<HttpPostAttribute>());
        Assert.Null(accion.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(accion.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }

    [Theory]
    [InlineData("IniciarSesionAPI")]
    [InlineData("RecuperarAccesoAPI")]
    public void EndpointsPublicosDeAutenticacionExigenRateLimiting(string nombreAccion)
    {
        var accion = typeof(SuwoGym_API.Controllers.HomeController)
            .GetMethod(nombreAccion);
        Assert.NotNull(accion);
        Assert.NotNull(accion!.GetCustomAttribute<EnableRateLimitingAttribute>());
    }

    [Fact]
    public void HashDeContrasenaNoSeIncluyeEnRespuestasJson()
    {
        var modelo = new DatoUsuarioResponseModel
        {
            IdUsuario = 23,
            ContrasennaHash = "$2a$12$test-private-hash",
            Token = "jwt-ficticio"
        };

        var json = JsonSerializer.Serialize(modelo);

        Assert.DoesNotContain("ContrasennaHash", json);
        Assert.DoesNotContain("test-private-hash", json);
        Assert.Contains("IdUsuario", json);
        // El campo sigue accesible para BCrypt/Dapper internamente.
        Assert.Contains("test-private-hash", modelo.ContrasennaHash);
    }

    [Fact]
    public async Task RecuperacionSinSmtpSeRechazaAntesDeModificarContrasena()
    {
        var configuracion = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Correos:Correo"] = "",
                ["Correos:AppPassword"] = ""
            })
            .Build();
        var controlador = new SuwoGym_API.Controllers.HomeController(
            configuracion, null!);

        var resultado = await controlador.RecuperarAccesoAPI(
            new RecuperarAccesoRequestModel
            {
                CorreoElectronico = "prueba@example.invalid"
            });

        var error = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(503, error.StatusCode);
    }
}
