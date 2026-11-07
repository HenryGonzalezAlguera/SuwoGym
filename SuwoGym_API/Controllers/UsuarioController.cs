using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;

namespace SuwoGym_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(
        IConfiguration _databasekey,
        IUtilesService _utiles) : ControllerBase
    {
        // La validación real de VersionJwt, Estado e IdRol ocurre
        // antes, en JwtBearerEvents.OnTokenValidated de Program.cs.
        [HttpGet("ValidarSesionAPI")]
        public IActionResult ValidarSesionAPI() => NoContent();

        #region Consultar usuario

        [HttpGet("ConsultarUsuarioAPI")]
        public IActionResult ConsultarUsuarioAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdUsuario",
                _utiles.ObtenerIdUsuarioToken());

            var response =
                context.QueryFirstOrDefault
                <DatoUsuarioResponseModel>(
                    "spConsultarUsuario",
                    parameters);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "El usuario no se pudo encontrar.");
        }

        #endregion

        #region Cambiar contraseña

        [HttpPut("CambiarContrasennaAPI")]
        public IActionResult CambiarContrasennaAPI(
            CambiarAccesoRequestModel model)
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdUsuario",
                _utiles.ObtenerIdUsuarioToken());

            parameters.Add(
                "@ContrasennaHash",
                model.contrasenna);

            parameters.Add(
                "@DebeCambiarContrasenna",
                false);

            var actualizacion =
                context.Execute(
                    "spActualizarContrasenna",
                    parameters);

            if (actualizacion > 0)
            {
                return Ok(actualizacion);
            }

            return BadRequest(
                "La contraseña no se pudo actualizar correctamente.");
        }

        #endregion

    }
}