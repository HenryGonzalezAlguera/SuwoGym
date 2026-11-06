using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using SuwoGym_Web.Models;
using Xunit;

namespace SuwoGym.Tests;

public class RecuperacionSeguraFase6B2Tests
{
    [Fact]
    public void TokenEsAleatorioTiene256BitsYHashDeterminista()
    {
        var primero = TokenRecuperacionSeguro.Crear();
        var segundo = TokenRecuperacionSeguro.Crear();

        Assert.Equal(43, primero.Token.Length);
        Assert.Equal(32, primero.Hash.Length);
        Assert.NotEqual(primero.Token, segundo.Token);
        Assert.NotEqual(Convert.ToHexString(primero.Hash),
            Convert.ToHexString(segundo.Hash));

        Assert.True(TokenRecuperacionSeguro.TryObtenerHash(
            primero.Token, out var calculado));
        Assert.Equal(Convert.ToHexString(primero.Hash),
            Convert.ToHexString(calculado));
    }

    [Theory]
    [InlineData("")]
    [InlineData("no-es-un-token")]
    [InlineData("abc")]
    [InlineData("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")]
    public void RechazaTokensMalformados(string token)
    {
        Assert.False(TokenRecuperacionSeguro.TryObtenerHash(token, out _));
    }

    [Fact]
    public void SolicitudYConsumoSiguenSiendoPostPublicosLimitados()
    {
        var controlador = typeof(SuwoGym_API.Controllers.HomeController);
        foreach (var nombre in new[]
                 {
                     "RecuperarAccesoAPI",
                     "ValidarEnlaceRecuperacionAPI",
                     "RestablecerContrasennaAPI"
                 })
        {
            var accion = controlador.GetMethod(nombre);
            Assert.NotNull(accion);
            Assert.NotNull(accion!.GetCustomAttribute<HttpPostAttribute>());
            Assert.NotNull(accion.GetCustomAttribute<
                Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute>());
        }
    }

    [Fact]
    public void NuevaContrasennaExigeLongitudMinima()
    {
        var solicitud = new RestablecerContrasennaRequestModel
        {
            Token = TokenRecuperacionSeguro.Crear().Token,
            NuevaContrasenna = "Corta1!"
        };

        var resultados = new List<ValidationResult>();
        var valido = Validator.TryValidateObject(
            solicitud, new ValidationContext(solicitud), resultados, true);

        Assert.False(valido);
        Assert.NotEmpty(resultados);
    }

    [Fact]
    public void FormularioDeCambioRequiereCsrf()
    {
        var metodo = typeof(SuwoGym_Web.Controllers.HomeController)
            .GetMethod("RestablecerContrasenna",
                new[] { typeof(RestablecerContrasennaViewModel) });

        Assert.NotNull(metodo);
        Assert.NotNull(metodo!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(metodo.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }

    [Fact]
    public void ConfirmacionDeContrasennaDebeCoincidir()
    {
        var modelo = new RestablecerContrasennaViewModel
        {
            Token = TokenRecuperacionSeguro.Crear().Token,
            NuevaContrasenna = "ContrasennaSegura2026!",
            ConfirmarContrasenna = "ContrasennaDiferente2026!"
        };
        var errores = new List<ValidationResult>();
        Assert.False(Validator.TryValidateObject(
            modelo, new ValidationContext(modelo), errores, true));
        Assert.NotEmpty(errores);
    }
}
