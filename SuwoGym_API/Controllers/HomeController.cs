using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using System.Data;

namespace SuwoGym_API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(
        IConfiguration _databasekey,
        IUtilesService _utiles) : ControllerBase
    {
        #region Registro API

        [HttpPost("RegistroAPI")]
        public IActionResult RegistroAPI(
            UsuarioRegistroModel model)
        {
            try
            {
                using var context = new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

                var parameters = new DynamicParameters();

                parameters.Add(
                    "@Nombre",
                    model.nombre);

                parameters.Add(
                    "@Apellido",
                    model.apellido);

                parameters.Add(
                    "@CorreoElectronico",
                    model.correoElectronico);

                parameters.Add(
                    "@ContrasennaHash",
                    model.contrasenna);

                parameters.Add(
                    "@Identificacion",
                    model.identificacion);

                var response =
                    context.QueryFirstOrDefault
                    <DatoUsuarioResponseModel>(
                        "spRegistrarUsuario",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response != null &&
                    response.IdUsuario > 0)
                {
                    return Ok(response);
                }

                return BadRequest(
                    "La información no se pudo registrar correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Iniciar sesión API

        [HttpPost("IniciarSesionAPI")]
        public IActionResult IniciarSesionAPI(
            UsuarioLoginModel model)
        {
            using var context = new SqlConnection(
                _databasekey[
                    "ConnectionStringsApi:DefaultConnection"]);

            var parameters = new DynamicParameters();

            parameters.Add(
                "@CorreoElectronico",
                model.correoElectronico);

            parameters.Add(
                "@ContrasennaHash",
                model.contrasenna);

            var response =
                context.QueryFirstOrDefault
                <DatoUsuarioResponseModel>(
                    "spIniciarSesionUsuario",
                    parameters);

            if (response != null &&
                BCrypt.Net.BCrypt.Verify(
                    model.contrasenna,
                    response.ContrasennaHash))
            {
                response.Token =
                    _utiles.GenerarToken(
                        response.IdUsuario);

                return Ok(response);
            }

            return NotFound(
                "La información no se pudo validar correctamente");
        }

        #endregion

        #region Recuperar acceso API

        [HttpPost("RecuperarAccesoAPI")]
        public async Task<IActionResult>
            RecuperarAccesoAPI(
                RecuperarAccesoRequestModel model)
        {
            using var context = new SqlConnection(
                _databasekey[
                    "ConnectionStringsApi:DefaultConnection"]);

            var parameters = new DynamicParameters();

            parameters.Add(
                "@CorreoElectronico",
                model.CorreoElectronico);

            var response =
                context.QueryFirstOrDefault
                <DatoUsuarioResponseModel>(
                    "spValidarCorreo",
                    parameters);

            if (response == null)
            {
                return NotFound(
                    "La información no se pudo validar correctamente");
            }

            // Generar nueva contraseña temporal
            var temporal =
                _utiles.GenerarContrasena();

            var temporalHash =
                BCrypt.Net.BCrypt.HashPassword(
                    temporal);

            parameters = new DynamicParameters();

            parameters.Add(
                "@IdUsuario",
                response.IdUsuario);

            parameters.Add(
                "@ContrasennaHash",
                temporalHash);

            parameters.Add(
                "@DebeCambiarContrasenna",
                true);

            var actualizacion = context.Execute(
                "spActualizarContrasenna",
                parameters);

            if (actualizacion > 0)
            {
                // Enviar correo con la contraseña temporal
                string ruta = Path.Combine(
                    AppContext.BaseDirectory,
                    "Templates",
                    "RecuperarAcceso.html");

                string plantilla =
                    System.IO.File.ReadAllText(ruta);

                plantilla = plantilla.Replace(
                    "{{NOMBRE}}",
                    response.Nombre);

                plantilla = plantilla.Replace(
                    "{{TEMPORAL}}",
                    temporal);

                await _utiles.EnviarCorreoAsync(
                    response.CorreoElectronico,
                    "Recuperación de acceso",
                    plantilla);

                return Ok(response);
            }

            return BadRequest(
                "No se ha recuperado su acceso, por favor intente nuevamente.");
        }

        #endregion
    }
}