using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Controllers;
using SuwoGym_Web.Filters;
using SuwoGym_Web.Models;
using Xunit;

namespace SuwoGym.Tests;

public class CatalogoClasesTests
{
    [Fact]
    public void ClaseConNombreValido_PasaValidacion()
    {
        var modelo = new ClaseCrearModel
        {
            NombreClase = "Entrenamiento funcional",
            Descripcion = "Sesión de acondicionamiento general."
        };
        Assert.Empty(Validar(modelo));
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public void NombreInvalido_SeRechaza(string nombre)
    {
        var modelo = new ClaseCrearModel { NombreClase = nombre };
        Assert.NotEmpty(Validar(modelo));
    }

    [Fact]
    public void NombreDemasiadoLargo_SeRechaza()
    {
        var modelo = new ClaseCrearModel { NombreClase = new string('A', 51) };
        Assert.NotEmpty(Validar(modelo));
    }

    [Fact]
    public void DescripcionDemasiadoLarga_SeRechaza()
    {
        var modelo = new ClaseCrearModel
        {
            NombreClase = "Yoga",
            Descripcion = new string('D', 201)
        };
        Assert.NotEmpty(Validar(modelo));
    }

    [Fact]
    public void FormularioAltaClase_RequierePostYAntiForgery()
    {
        var accion = typeof(AdminController).GetMethod(
            nameof(AdminController.Clases_Agregar),
            new[] { typeof(ClaseCrearModel) });

        Assert.NotNull(accion);
        Assert.NotNull(accion!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(accion.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.NotNull(typeof(AdminController)
            .GetCustomAttribute<SessionAuthorizeAttribute>());
    }

    private static List<ValidationResult> Validar(ClaseCrearModel modelo)
    {
        var errores = new List<ValidationResult>();
        Validator.TryValidateObject(
            modelo, new ValidationContext(modelo), errores, validateAllProperties: true);
        return errores;
    }
}
