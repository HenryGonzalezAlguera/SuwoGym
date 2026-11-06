using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using System.Data;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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

        [HttpGet("MembresiasAPI")]
        public IActionResult ObtenerMembresiasAPI()
        {
            using var context = new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<HomePageMembresiasResponseModel>(
                    "spObtenerPlanMembresiaDetalle");

            if (response.Any())
                return Ok(response);

            return NotFound("No hay membresías registradas");
        }


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
                
                parameters.Add(
                    "@PlanMembresia",
                    model.PlanMembresia);

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
        [EnableRateLimiting("auth-login")]
        public IActionResult IniciarSesionAPI(
            UsuarioLoginModel model)
        {
            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@CorreoElectronico",
                    model.correoElectronico);

            
                var response =
                    context.QueryFirstOrDefault
                    <DatoUsuarioResponseModel>(
                        "spIniciarSesionUsuario",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response == null)
                {
                    return NotFound(
                        "El correo o la contraseña son incorrectos.");
                }

                var contrasennaCorrecta =
                    BCrypt.Net.BCrypt.Verify(
                        model.contrasenna,
                        response.ContrasennaHash);

                if (!contrasennaCorrecta)
                {
                    return NotFound(
                        "El correo o la contraseña son incorrectos.");
                }

                response.Token =
                    _utiles.GenerarToken(
                        response.IdUsuario,
                        response.IdRol);

                return Ok(response);
            }
            catch (Exception)
            {
                // Nunca devolver detalles de SQL Server ni del hash al cliente.
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "No se pudo procesar el inicio de sesión.");
            }
        }

        #endregion

        #region Recuperar acceso API

        [HttpPost("RecuperarAccesoAPI")]
        [EnableRateLimiting("auth-recovery")]
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