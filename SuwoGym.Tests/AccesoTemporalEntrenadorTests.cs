using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuwoGym_API.Services;
using SuwoGym_Web.Controllers;
using SuwoGym_Web.Filters;
using Xunit;

namespace SuwoGym.Tests;

public class AccesoTemporalEntrenadorTests
{
    [Fact]
    public void GeneradorCreaClavesTemporalesLargas()
    {
        var clave = GeneradorClavesTemporales.Generar();
        Assert.Equal(24, clave.Length);
        Assert.All(clave, c => Assert.Contains(c,
            "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%"));
    }

    [Fact]
    public void GeneradorNoRepiteLaMismaClave()
    {
        var claves = Enumerable.Range(0, 20)
            .Select(_ => GeneradorClavesTemporales.Generar())
            .ToHashSet();
        Assert.Equal(20, claves.Count);
    }

    [Fact]
    public void RestablecimientoWebEsPostProtegidoContraCsrf()
    {
        var metodo = typeof(AdminController).GetMethod(
            nameof(AdminController.Empleados_RestablecerAccesoTemporal),
            new[] { typeof(int) });
        Assert.NotNull(metodo);
        Assert.NotNull(metodo!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(metodo.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.NotNull(typeof(AdminController)
            .GetCustomAttribute<SessionAuthorizeAttribute>());
    }

    [Fact]
    public void RestablecimientoApiSoloDisponibleParaAdministrador()
    {
        var controlador = typeof(SuwoGym_API.Controllers.AdminController);
        var autorizacion = controlador.GetCustomAttribute<AuthorizeAttribute>();
        Assert.NotNull(autorizacion);
        Assert.Equal("1", autorizacion!.Roles);

        var metodo = controlador.GetMethod("EmpleadoRestablecerAccesoTemporalAPI");
        Assert.NotNull(metodo);
        Assert.NotNull(metodo!.GetCustomAttribute<HttpPostAttribute>());
    }
}
