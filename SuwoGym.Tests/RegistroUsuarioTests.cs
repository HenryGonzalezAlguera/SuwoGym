using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SuwoGym_Web.Controllers;
using SuwoGym_Web.Models;
using Xunit;

namespace SuwoGym.Tests;

public class RegistroUsuarioTests
{
    private static RegistroUsuarioViewModel ModeloValido() => new()
    {
        nombre = "Ana",
        apellido = "Prueba",
        identificacion = "DEMO-001",
        correoElectronico = "ana@example.test",
        contrasenna = "ClaveLocal2026!",
        confirmarContrasenna = "ClaveLocal2026!",
        PlanMembresia = 1
    };

    [Fact]
    public void FormularioValido_PasaValidaciones()
    {
        var errores = new List<ValidationResult>();
        var modelo = ModeloValido();
        Assert.True(Validator.TryValidateObject(
            modelo, new ValidationContext(modelo), errores, true));
        Assert.Empty(errores);
    }

    [Theory]
    [InlineData("distinta", 1)]
    [InlineData("", 1)]
    [InlineData("ClaveLocal2026!", 0)]
    public void FormularioIncompleto_NoPasaValidaciones(
        string confirmacion, int plan)
    {
        var modelo = ModeloValido();
        modelo.confirmarContrasenna = confirmacion;
        modelo.PlanMembresia = plan;
        var errores = new List<ValidationResult>();
        Assert.False(Validator.TryValidateObject(
            modelo, new ValidationContext(modelo), errores, true));
        Assert.NotEmpty(errores);
    }

    [Fact]
    public async Task PlanNoDisponible_NoLlamaAlRegistroAPI()
    {
        var handler = new FakeApiHandler(planDisponible: false);
        var controlador = CrearControlador(handler);
        var resultado = await controlador.Registro(ModeloValido());

        Assert.IsType<ViewResult>(resultado);
        Assert.False(controlador.ModelState.IsValid);
        Assert.Equal(0, handler.RegistroLlamadas);
    }

    [Fact]
    public async Task FormularioIncorrecto_NoLlamaAlRegistroAPI()
    {
        var handler = new FakeApiHandler(planDisponible: true);
        var controlador = CrearControlador(handler);
        controlador.ModelState.AddModelError(
            nameof(RegistroUsuarioViewModel.confirmarContrasenna),
            "Las contraseñas no coinciden.");

        var resultado = await controlador.Registro(ModeloValido());
        Assert.IsType<ViewResult>(resultado);
        Assert.Equal(0, handler.RegistroLlamadas);
    }

    [Fact]
    public async Task SolicitudValida_EnviaHashYPlan_SinConfirmacion()
    {
        var handler = new FakeApiHandler(planDisponible: true);
        var controlador = CrearControlador(handler);
        var modelo = ModeloValido();
        var resultado = await controlador.Registro(modelo);

        Assert.IsType<ViewResult>(resultado); // La API simulada devuelve BadRequest.
        Assert.Equal(1, handler.RegistroLlamadas);
        Assert.NotNull(handler.CuerpoRegistro);
        using var json = System.Text.Json.JsonDocument.Parse(handler.CuerpoRegistro!);
        var valores = json.RootElement;
        Assert.Equal(1, valores.GetProperty("planMembresia").GetInt32());
        Assert.Equal("ana@example.test",
            valores.GetProperty("correoElectronico").GetString());
        Assert.False(valores.TryGetProperty("confirmarContrasenna", out _));
        var hash = valores.GetProperty("contrasenna").GetString();
        Assert.NotEqual(modelo.contrasenna, hash);
        Assert.True(BCrypt.Net.BCrypt.Verify(modelo.contrasenna, hash));
        Assert.Equal("ClaveLocal2026!", modelo.contrasenna);
    }

    [Fact]
    public void PostRegistro_ProtegidoContraCSRF()
    {
        var post = typeof(HomeController).GetMethod(
            "Registro", new[] { typeof(RegistroUsuarioViewModel) });
        Assert.NotNull(post);
        Assert.NotNull(post!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }

    private static HomeController CrearControlador(FakeApiHandler handler)
    {
        var fabrica = new FakeClientFactory(handler);
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStringsWeb:urlApi"] = "https://localhost:7129"
            }).Build();
        return new HomeController(fabrica, config)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    private sealed class FakeClientFactory(HttpMessageHandler handler)
        : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) =>
            new(handler, disposeHandler: false);
    }

    private sealed class FakeApiHandler(bool planDisponible)
        : HttpMessageHandler
    {
        public int RegistroLlamadas { get; private set; }
        public string? CuerpoRegistro { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri?.AbsolutePath == "/api/Home/MembresiasAPI")
                return planDisponible
                    ? new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = JsonContent.Create(new[]
                        {
                            new { idPlan = 1, nombrePlan = "Mensual",
                                  duracionDias = 30, precio = 10600m }
                        })
                    }
                    : new HttpResponseMessage(HttpStatusCode.NotFound);

            if (request.RequestUri?.AbsolutePath == "/api/Home/RegistroAPI")
            {
                RegistroLlamadas++;
                CuerpoRegistro = await request.Content!.ReadAsStringAsync(cancellationToken);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            throw new InvalidOperationException("Ruta no esperada en la prueba.");
        }
    }
}
