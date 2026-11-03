using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SuwoGym_Web.Controllers;
using SuwoGym_Web.Filters;
using System.Reflection;
using Xunit;

namespace SuwoGym.Tests;

public class SessionAuthorizeAttributeTests
{
    [Fact]
    public void SesionVacia_RedireccionaALogin()
    {
        var result = Ejecutar(new SessionAuthorizeAttribute(), new FakeSession());
        AssertRedireccion(result, "Index", "Home");
    }

    [Theory]
    [InlineData("0", 1, 1, "token")]
    [InlineData("1", 0, 1, "token")]
    [InlineData("1", 1, 0, "token")]
    [InlineData("1", 1, 4, "token")]
    [InlineData("1", 1, 1, "")]
    [InlineData("1", 1, 1, "  ")]
    public void SesionParcialOInvalida_RedireccionaALogin(
        string autenticado, int idUsuario, int idRol, string token)
    {
        var sesion = CrearSesion(autenticado, idUsuario, idRol, token);
        var result = Ejecutar(new SessionAuthorizeAttribute(), sesion);
        AssertRedireccion(result, "Index", "Home");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void SesionCompleta_PermiteAccesoGeneral(int rol)
    {
        var result = Ejecutar(
            new SessionAuthorizeAttribute(), CrearSesion("1", 7, rol, "jwt.test"));
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    public void RolCorrecto_PermiteAcceso(int requerido, int rol)
    {
        var result = Ejecutar(
            new SessionAuthorizeAttribute(requerido),
            CrearSesion("1", 7, rol, "jwt.test"));
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 1)]
    [InlineData(3, 2)]
    public void RolIncorrecto_RedireccionaAlPanelPrincipal(int requerido, int rol)
    {
        var result = Ejecutar(
            new SessionAuthorizeAttribute(requerido),
            CrearSesion("1", 7, rol, "jwt.test"));
        AssertRedireccion(result, "Principal", "Home");
    }

    [Fact]
    public void LosControladoresTienenProteccionDeclarativa()
    {
        Assert.NotNull(typeof(AdminController).GetCustomAttribute<SessionAuthorizeAttribute>());
        Assert.NotNull(typeof(EntrenadorController).GetCustomAttribute<SessionAuthorizeAttribute>());
        Assert.NotNull(typeof(MiembroController).GetCustomAttribute<SessionAuthorizeAttribute>());
        Assert.NotNull(typeof(UsuarioController).GetCustomAttribute<SessionAuthorizeAttribute>());

        var principal = typeof(HomeController).GetMethod(nameof(HomeController.Principal));
        Assert.NotNull(principal?.GetCustomAttribute<SessionAuthorizeAttribute>());
    }

    [Fact]
    public void NoAceptaRolesFueraDelRango()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new SessionAuthorizeAttribute(4));
    }

    private static IActionResult? Ejecutar(
        SessionAuthorizeAttribute filtro, FakeSession sesion)
    {
        var http = new DefaultHttpContext();
        http.Features.Set<ISessionFeature>(new FakeSessionFeature { Session = sesion });
        var actionContext = new ActionContext(http, new RouteData(), new ActionDescriptor());
        var context = new ActionExecutingContext(
            actionContext, new List<IFilterMetadata>(),
            new Dictionary<string, object?>(), new object());
        filtro.OnActionExecuting(context);
        return context.Result;
    }

    private static FakeSession CrearSesion(
        string autenticado, int idUsuario, int idRol, string token)
    {
        var sesion = new FakeSession();
        sesion.SetString("Autenticado", autenticado);
        sesion.SetInt32("IdUsuario", idUsuario);
        sesion.SetInt32("IdRol", idRol);
        sesion.SetString("Token", token);
        return sesion;
    }

    private static void AssertRedireccion(
        IActionResult? result, string accion, string controlador)
    {
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(accion, redirect.ActionName);
        Assert.Equal(controlador, redirect.ControllerName);
    }

    private sealed class FakeSessionFeature : ISessionFeature
    {
        public ISession Session { get; set; } = null!;
    }

    private sealed class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _datos = new();
        public bool IsAvailable => true;
        public string Id => "test-session";
        public IEnumerable<string> Keys => _datos.Keys;
        public void Clear() => _datos.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
        public void Remove(string key) => _datos.Remove(key);
        public void Set(string key, byte[] value) => _datos[key] = value;
        public bool TryGetValue(string key, out byte[] value) =>
            _datos.TryGetValue(key, out value!);
    }
}
