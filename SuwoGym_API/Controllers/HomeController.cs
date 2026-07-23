using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;

namespace SuwoGym_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IConfiguration _databasekey) : ControllerBase
    {

        #region RegistroApi
        [HttpPost("RegistroAPI")]
        public IActionResult RegistroAPI(UsuarioRegistroModel model)
        {
            try
            {
                using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);

                var parameters = new DynamicParameters();
                parameters.Add("@Nombre", model.nombre);
                parameters.Add("@Apellido", model.apellido);
                parameters.Add("@CorreoElectronico", model.correoElectronico);
                parameters.Add("@ContrasennaHash", model.contrasenna);
                parameters.Add("@Identificacion", model.identificacion);

                var response = context.QueryFirstOrDefault<DatoUsuarioResponseModel>("spRegistrarUsuario", parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (response != null && response.IdUsuario > 0)
                {
                    return Ok(new { mensaje = "Registro exitoso. Bienvenido a SuwoGym.", usuario = response });
                }
                else
                {
                    return BadRequest("Error al registrar el usuario.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region IniciarSesionApi
        [HttpPost("IniciarSesionAPI")]
        public IActionResult IniciarSesionAPI(UsuarioLoginModel model)
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@CorreoElectronico", model.correoElectronico);
            parameters.Add("@ContrasennaHash", model.contrasenna);

            var response = context.QueryFirstOrDefault<DatoUsuarioResponseModel>("spIniciarSesionUsuario", parameters);

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound("Usuario no encontrado o credenciales incorrectas.");
            }
        }

        #endregion

        #region RecuperarAccesoApi

        [HttpPost("RecuperarAccesoAPI")]
        public IActionResult RecuperarAccesoAPI(RecuperarAccesoRequestModel model)
        {

            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);


            var parameters = new DynamicParameters();

            parameters.Add("@CorreoElectronico", model.correoElectronico);


            var response = context.QueryFirstOrDefault<DatoUsuarioResponseModel>("spValidarCorreo", parameters);


            if (response == null)
            {
                return NotFound("Usuario no encontrado o credenciales incorrectas.");
            }
            else
            {
                // Generar una contraseña temporal y actualizarla en la base de datos

                var temporal = GenerarContrasena();

                parameters = new DynamicParameters();
                parameters.Add("@IdUsuario", response.IdUsuario);
                parameters.Add("@ContrasennaHash", temporal);


                var actualizacion = context.Execute("spActualizarContrasenna", parameters);
                return Ok(response);
            }

        }

        #endregion

        private static string GenerarContrasena(int longitud = 10)
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var chars = new char[longitud];

            for (int i = 0; i < longitud; i++)
                chars[i] = caracteres[random.Next(caracteres.Length)];

            return new string(chars);
        }
    }
}


