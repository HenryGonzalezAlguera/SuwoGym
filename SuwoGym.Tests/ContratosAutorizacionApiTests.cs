using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SuwoGym.Tests;

/// <summary>
/// Contratos de autorización de la API. La modificación accidental de un
/// controlador no puede pasar CI sin revisar explícitamente estos controles.
/// </summary>
public class ContratosAutorizacionApiTests
{
    [Theory]
    [InlineData(typeof(SuwoGym_API.Controllers.AdminController), "1")]
    [InlineData(typeof(SuwoGym_API.Controllers.EntrenadorController), "2")]
    [InlineData(typeof(SuwoGym_API.Controllers.MiembroController), "3")]
    public void LosControladoresPorRolExigenElRolEspecifico(
        Type controlador, string rolEsperado)
    {
        var autorizacion = controlador.GetCustomAttribute<AuthorizeAttribute>();
        Assert.NotNull(autorizacion);
        Assert.Equal(rolEsperado, autorizacion!.Roles);
        Assert.Null(controlador.GetCustomAttribute<AllowAnonymousAttribute>());
    }

    [Fact]
    public void ConsultarSesionNoEsPublico()
    {
        var tipo = typeof(SuwoGym_API.Controllers.UsuarioController);
        Assert.NotNull(tipo.GetCustomAttribute<AuthorizeAttribute>());
        Assert.Null(tipo.GetCustomAttribute<AllowAnonymousAttribute>());

        var metodo = tipo.GetMethod("ValidarSesionAPI");
        Assert.NotNull(metodo);
        Assert.NotNull(metodo!.GetCustomAttribute<HttpGetAttribute>());
        Assert.Null(metodo.GetCustomAttribute<AllowAnonymousAttribute>());
    }

    [Fact]
    public void ElControladorPublicoConservaRateLimitingEnLoginYRecuperacion()
    {
        var tipo = typeof(SuwoGym_API.Controllers.HomeController);
        Assert.NotNull(tipo.GetCustomAttribute<AllowAnonymousAttribute>());
        foreach (var accion in new[]
        {
            "IniciarSesionAPI",
            "RecuperarAccesoAPI",
            "ValidarEnlaceRecuperacionAPI",
            "RestablecerContrasennaAPI"
        })
        {
            var metodo = tipo.GetMethod(accion);
            Assert.NotNull(metodo);
            Assert.NotNull(metodo!.GetCustomAttribute<HttpPostAttribute>());
            Assert.NotNull(metodo.GetCustomAttribute<
                Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute>());
        }
    }
}
