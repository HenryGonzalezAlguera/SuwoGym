using System.Reflection;
using Microsoft.AspNetCore.Mvc;
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
}
