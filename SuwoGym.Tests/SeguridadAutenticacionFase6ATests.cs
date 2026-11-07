using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuwoGym_API.Services;
using SuwoGym_Web.Controllers;
using SuwoGym_Web.Models;
using Xunit;

namespace SuwoGym.Tests;

public class SeguridadAutenticacionFase6ATests
{
    private const string Issuer = "SuwoGym.API";
    private const string Audience = "SuwoGym.Web";
    // Clave ficticia usada solo en pruebas. Nunca usarla en Development/producción.
    private const string TestKey = "TEST-ONLY-key-for-SuwoGym-Fase6A-2026-randomized-not-production";
    private static readonly Guid TestVersionJwt =
        Guid.Parse("22de23c6-3fdc-4d52-9b18-c5b249def833");

    private static UtilesService Servicio()
    {
        var configuracion = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = TestKey,
                ["Jwt:Issuer"] = Issuer,
                ["Jwt:Audience"] = Audience
            })
            .Build();

        return new UtilesService(configuracion, new HttpContextAccessor());
    }

    private static TokenValidationParameters Parametros(
        string? issuer = null, string? audience = null)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer ?? Issuer,
            ValidateAudience = true,
            ValidAudience = audience ?? Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestKey)),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    }

    [Fact]
    public void TokenValidoContieneIdentidadRolEmisorYAudiencia()
    {
        var token = Servicio().GenerarToken(7, 2, TestVersionJwt);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal(Issuer, jwt.Issuer);
        Assert.Contains(Audience, jwt.Audiences);
        Assert.Equal(TestVersionJwt.ToString("D"),
            jwt.Claims.FirstOrDefault(c => c.Type == "VersionJwt")?.Value);

        var identidad = handler.ValidateToken(token, Parametros(), out _);
        Assert.Equal("7", identidad.FindFirst("IdUsuario")?.Value);
        Assert.True(identidad.IsInRole("2"));
    }

    [Fact]
    public void TokenSeRechazaCuandoElEmisorNoCoincide()
    {
        var token = Servicio().GenerarToken(7, 2, TestVersionJwt);
        Assert.Throws<SecurityTokenInvalidIssuerException>(() =>
            new JwtSecurityTokenHandler().ValidateToken(
                token, Parametros(issuer: "OtraAPI"), out _));
    }

    [Fact]
    public void TokenSeRechazaCuandoLaAudienciaNoCoincide()
    {
        var token = Servicio().GenerarToken(7, 2, TestVersionJwt);
        Assert.Throws<SecurityTokenInvalidAudienceException>(() =>
            new JwtSecurityTokenHandler().ValidateToken(
                token, Parametros(audience: "OtraWEB"), out _));
    }

    [Fact]
    public void TokenSeRechazaConClaveDeFirmaDiferente()
    {
        var token = Servicio().GenerarToken(7, 2, TestVersionJwt);
        var parametros = Parametros();
        parametros.IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("TEST-ONLY-other-key-for-SuwoGym-Fase6A-not-production-123"));

        Assert.ThrowsAny<SecurityTokenException>(() =>
            new JwtSecurityTokenHandler().ValidateToken(
                token, parametros, out _));
    }

    [Theory]
    [InlineData("Index")]
    [InlineData("RecuperarAcceso")]
    public void FormulariosAccesoRequierenPostYAntifalsificacion(string nombre)
    {
        var metodo = typeof(HomeController).GetMethod(
            nombre, new[] { typeof(UsuarioModel) });

        Assert.NotNull(metodo);
        Assert.NotNull(metodo!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(metodo.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
