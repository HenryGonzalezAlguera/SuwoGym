using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using SuwoGym_API.Models;
using SuwoGym_Web.Middleware;
using Xunit;

namespace SuwoGym.Tests;

public class SeguridadRevocacionFase6CTests
{
    private static readonly Guid VersionInicial =
        Guid.Parse("fe0c5597-65d8-4af0-9e0c-bc6e8922fd55");
    private static readonly Guid VersionNueva =
        Guid.Parse("23d107bf-075b-48de-b5b9-8dd1e0ed7e04");

    [Fact]
    public void VersionVigenteYRolActualPermitenElJwt()
    {
        var usuario = new EstadoSeguridadJwt
        {
            VersionJwt = VersionInicial, IdRol = 2, Estado = true
        };

        Assert.True(usuario.PermiteToken(VersionInicial, 2));
        Assert.False(usuario.PermiteToken(VersionNueva, 2));
        Assert.False(usuario.PermiteToken(VersionInicial, 1));

        usuario.Estado = false;
        Assert.False(usuario.PermiteToken(VersionInicial, 2));

        usuario.Estado = true;
        usuario.VersionJwt = Guid.Empty;
        Assert.False(usuario.PermiteToken(Guid.Empty, 2));
    }

    [Fact]
    public async Task MiddlewarePermiteSesionSiApiDevuelve204()
    {
        var (http, sesion) = CrearContexto();
        var invoked = false;
        var middleware = new ValidarSesionJwtMiddleware(_ =>
        {
            invoked = true;
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(http,
            new FactorySimulada(HttpStatusCode.NoContent),
            Configuracion());

        Assert.True(invoked);
        Assert.Equal("1", sesion.GetString("Autenticado"));
        Assert.Equal(200, http.Response.StatusCode);
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    public async Task MiddlewareLimpiaSesionSiApiRevocaToken(HttpStatusCode respuesta)
    {
        var (http, sesion) = CrearContexto();
        var invoked = false;
        var middleware = new ValidarSesionJwtMiddleware(_ =>
        {
            invoked = true;
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(http,
            new FactorySimulada(respuesta),
            Configuracion());

        Assert.False(invoked);
        Assert.Null(sesion.GetString("Token"));
        Assert.Equal("/Home/Index", http.Response.Headers.Location.ToString());
    }

    [Fact]
    public async Task MiddlewareNoPermiteVistaSiApiNoEstaDisponible()
    {
        var (http, sesion) = CrearContexto();
        var invoked = false;
        var middleware = new ValidarSesionJwtMiddleware(_ =>
        {
            invoked = true;
            return Task.CompletedTask;
        });

        await middleware.InvokeAsync(http,
            new FactorySimulada(HttpStatusCode.ServiceUnavailable),
            Configuracion());

        Assert.False(invoked);
        Assert.Equal(503, http.Response.StatusCode);
        Assert.NotNull(sesion.GetString("Token"));
    }

    private static IConfiguration Configuracion() =>
        new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["ConnectionStringsWeb:urlApi"] = "https://localhost:7129"
            }).Build();

    private static (DefaultHttpContext, FakeSession) CrearContexto()
    {
        var ctx = new DefaultHttpContext();
        ctx.Request.Path = "/Entrenador/Rutinas_Lista";
        var sesion = new FakeSession();
        sesion.SetString("Autenticado", "1");
        sesion.SetString("Token", "token-de-prueba");
        ctx.Features.Set<ISessionFeature>(
            new FakeFeature { Session = sesion });
        return (ctx, sesion);
    }

    private sealed class FactorySimulada(HttpStatusCode codigo) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) =>
            new(new SimularRespuesta(codigo));
    }

    private sealed class SimularRespuesta(HttpStatusCode codigo)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(codigo));
    }

    private sealed class FakeFeature : ISessionFeature
    {
        public ISession Session { get; set; } = null!;
    }

    private sealed class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _datos = new();
        public bool IsAvailable => true;
        public string Id => "session-test";
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
