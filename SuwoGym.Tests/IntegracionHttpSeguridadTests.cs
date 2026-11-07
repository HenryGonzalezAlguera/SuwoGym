using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using Xunit;

namespace SuwoGym.Tests;

/// <summary>
/// Pruebas HTTP reales de autenticación/autorización contra la API en memoria.
/// La BD se sustituye SOLO en estas pruebas por un servicio de versiones falso.
/// Ningún endpoint de negocio ni procedimiento SQL se ejecuta.
/// </summary>
public sealed class IntegracionHttpSeguridadTests
{
    private const int IdUsuario = 1701;
    private const string Emisor = "SuwoGym.API";
    private const string Audiencia = "SuwoGym.Web";
    private const string ClavePruebas =
        "TEST-ONLY-SUWO-GYM-FASE7-HTTP-INTEGRACION-2026-CLAVE-SIN-VALOR-REAL";
    private static readonly Guid Version =
        Guid.Parse("ff31e464-c5d8-4370-8ee3-3bfb5c3e21d4");

    [Fact]
    public async Task RutaPrivadaSinJwtDevuelve401()
    {
        using var api = new ApiParaPruebas();
        using var client = CrearCliente(api);

        using var respuesta = await client.GetAsync(
            "/api/Usuario/ValidarSesionAPI");

        Assert.Equal(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task JwtVigenteDeCualquierRolPuedeValidarSuSesion(int rol)
    {
        using var api = new ApiParaPruebas();
        api.Estado.IdRol = rol;
        using var client = CrearCliente(api, CrearToken(rol, Version));

        using var respuesta = await client.GetAsync(
            "/api/Usuario/ValidarSesionAPI");

        Assert.Equal(HttpStatusCode.NoContent, respuesta.StatusCode);
    }

    [Theory]
    [InlineData(1, "/api/Entrenador/ClientesAsignadosAPI")]
    [InlineData(2, "/api/Administrador/ListaEmpleadosAPI")]
    [InlineData(3, "/api/Admin/ListaEmpleadosAPI")]
    public async Task JwtValidoNoAutorizaModuloDeOtroRol(
        int rol, string rutaNoPermitida)
    {
        using var api = new ApiParaPruebas();
        api.Estado.IdRol = rol;
        using var client = CrearCliente(api, CrearToken(rol, Version));

        using var respuesta = await client.GetAsync(rutaNoPermitida);

        // Para la ruta Admin hay un controlador de verdad; para los demás
        // casos también usamos rutas existentes al construir las teorías.
        Assert.Equal(HttpStatusCode.Forbidden, respuesta.StatusCode);
    }

    [Fact]
    public async Task VersionCambiadaPorSqlRevocaJwtExistente()
    {
        using var api = new ApiParaPruebas();
        using var client = CrearCliente(api, CrearToken(2, Version));

        using var antes = await client.GetAsync("/api/Usuario/ValidarSesionAPI");
        Assert.Equal(HttpStatusCode.NoContent, antes.StatusCode);

        api.Estado.VersionJwt = Guid.NewGuid();
        using var despues = await client.GetAsync("/api/Usuario/ValidarSesionAPI");

        Assert.Equal(HttpStatusCode.Unauthorized, despues.StatusCode);
    }

    [Fact]
    public async Task CuentaDesactivadaRevocaJwtExistente()
    {
        using var api = new ApiParaPruebas();
        using var client = CrearCliente(api, CrearToken(2, Version));

        api.Estado.Estado = false;
        using var respuesta = await client.GetAsync(
            "/api/Usuario/ValidarSesionAPI");

        Assert.Equal(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    }

    [Fact]
    public async Task CambioDeRolRevocaJwtExistente()
    {
        using var api = new ApiParaPruebas();
        using var client = CrearCliente(api, CrearToken(2, Version));

        api.Estado.IdRol = 1;
        using var respuesta = await client.GetAsync(
            "/api/Usuario/ValidarSesionAPI");

        Assert.Equal(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    }

    [Fact]
    public async Task SiLaConsultaSqlFallaNoSePermiteElJwt()
    {
        using var api = new ApiParaPruebas();
        using var client = CrearCliente(api, CrearToken(2, Version));

        api.Estado.FallarConsulta = true;
        using var respuesta = await client.GetAsync(
            "/api/Usuario/ValidarSesionAPI");

        Assert.Equal(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    }

    [Fact]
    public async Task FirmaDistintaNoAutorizaElAcceso()
    {
        using var api = new ApiParaPruebas();
        var firmaFalsa = new string('X', 64);
        using var client = CrearCliente(
            api, CrearToken(2, Version, firmaFalsa));

        using var respuesta = await client.GetAsync(
            "/api/Usuario/ValidarSesionAPI");

        Assert.Equal(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    }

    private static HttpClient CrearCliente(
        ApiParaPruebas api, string? token = null)
    {
        var client = api.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });

        if (token is not null)
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    private static string CrearToken(
        int rol, Guid version, string? clave = null)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = Emisor,
                ["Jwt:Audience"] = Audiencia,
                ["Jwt:SecretKey"] = clave ?? ClavePruebas
            }).Build();

        var service = new UtilesService(config, new HttpContextAccessor());
        return service.GenerarToken(IdUsuario, rol, version);
    }

    private sealed class ApiParaPruebas
        : WebApplicationFactory<global::Program>
    {
        public VersionFalsa Estado { get; } = new()
        {
            IdRol = 2, Estado = true, VersionJwt = Version
        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["Jwt:Issuer"] = Emisor,
                        ["Jwt:Audience"] = Audiencia,
                        ["Jwt:SecretKey"] = ClavePruebas,
                        ["ConnectionStringsApi:DefaultConnection"] =
                            "Server=localhost;Database=SUWO_TEST_NO_CONECTAR;"
                    });
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<ISeguridadJwtVersionService>();
                services.AddSingleton<ISeguridadJwtVersionService>(Estado);
            });
        }
    }

    private sealed class VersionFalsa : ISeguridadJwtVersionService
    {
        public Guid VersionJwt { get; set; }
        public int IdRol { get; set; }
        public bool Estado { get; set; }
        public bool FallarConsulta { get; set; }

        public Task<EstadoSeguridadJwt?> ConsultarAsync(
            int idUsuario, CancellationToken cancellationToken = default)
        {
            if (FallarConsulta)
                throw new InvalidOperationException("SQL simulado no disponible");

            EstadoSeguridadJwt? usuario = idUsuario == IdUsuario
                ? new EstadoSeguridadJwt
                {
                    VersionJwt = VersionJwt,
                    IdRol = IdRol,
                    Estado = Estado
                }
                : null;

            return Task.FromResult(usuario);
        }
    }
}
