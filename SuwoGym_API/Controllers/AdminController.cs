using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SuwoGym_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController (IConfiguration _databasekey, IUtilesService _utiles) : ControllerBase
    {

        #region Gestión de Empleados --------------------------------------------------------------------

        [HttpGet("ListaEmpleadosAPI/{id}")]
        public async Task<IActionResult> ListaEmpleadosAPI(int id)
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);
            var parameters= new DynamicParameters();
            parameters.Add("@IdUser", id);
            var response = await context.QueryAsync<EmpleadoListaResponseModel>("spListarEmpleados",parameters);
            return Ok(response);
        }

        [HttpGet("ListaRolesAPI")]
        public async Task<IActionResult> ListaRolesAPI()
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);
            var response = await context.QueryAsync<RolesListaResponseModel>("spListarRoles");
            return Ok(response);
        }




        [HttpPost("EmpleadoAgregar")]
        public async Task<IActionResult> EmpleadoAgregar(EmpleadoCrearRequestModel model)
        {
            // Conexión
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);

            // Generar nueva contraseña temporal (para que el Admin no lo haga)
            var temporal = _utiles.GenerarContrasena();
            var temporalHash = BCrypt.Net.BCrypt.HashPassword(temporal);

            // Parámetros
            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@Apellido", model.Apellido);
            parameters.Add("@Identificacion", model.Identificacion);
            parameters.Add("@FechaNacimiento", model.FechaNac);
            parameters.Add("@Telefono", model.Telefono);
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);
            parameters.Add("@Rol", model.Rol);
            parameters.Add("@Direccion", model.Direccion);
            parameters.Add("@ContrasennaHash", temporalHash);
            parameters.Add("@FechaRegistro", DateTime.Now);

            var response = context.Execute("spRegistrarEmpleado", parameters);


            if (response > 0)
            {
                // Enviar correo con la contraseña temporal
                string ruta = Path.Combine( AppContext.BaseDirectory, "Templates", "BienvenidaEmpleado.html");
                string plantilla = System.IO.File.ReadAllText(ruta);

                // Agregar Datos a la plantilla
                plantilla = plantilla.Replace("{{NOMBRE}}",model.Nombre);
                plantilla = plantilla.Replace("{{APELLIDO}}",model.Apellido);
                plantilla = plantilla.Replace("{{TEMPORAL}}",temporal);

                // Enviar correos y respuesta
                await _utiles.EnviarCorreoAsync(model.CorreoElectronico, "Nueva Cuenta en SUWÓ", plantilla);
                return Ok(response);
            } 
                
            return BadRequest("La información no se pudo validar correctamente");
        }


        [HttpGet("EmpleadoDetalles/{id}")]
        public async Task<IActionResult> EmpleadoDetalles(int id)
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);
            var parameters = new DynamicParameters();
            parameters.Add("@IdUser", id);
            var response = context.QueryFirstOrDefault<EmpleadoDetalleResponseModel>("spDetalleEmpleado",parameters);

            if (response != null) 
                return Ok(response);

            return BadRequest("No se ha podido encontrar información del empleado");
        }


        [HttpPut("EmpleadoEditar")]
        public async Task<IActionResult> Empleados_Editar(EmpleadoEditarRequestModel model)
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);
            var parameters = new DynamicParameters();
            parameters.Add("@IdUser", model.IdUsuario);
            parameters.Add("@Estado", model.Estado);
            parameters.Add("@Rol", model.IdRol);
            var response = context.Execute("spEditarEmpleado", parameters);

            if (response > 0) 
                return Ok(response);

            return BadRequest("No se ha podido encontrar información del empleado");
        }


        #endregion



        #region Gestión de Clientes --------------------------------------------------------------------

        [HttpGet("ListaClientesAPI")]
        public async Task<IActionResult> ListaClientesAPI()
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);
            var response = await context.QueryAsync<ClienteListaResponseModel>("spListarClientes");
            return Ok(response);
        }


        #endregion



        #region Gestión de Planes de Membresía --------------------------------------------------------------------

        [HttpGet("ListaPlanesAPI")]
        public async Task<IActionResult> ListaPlanesAPI()
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);
            var response = await context.QueryAsync<PlanesMListaResponseModel>("spListarPlanesMembresia");
            return Ok(response);
        }


        #endregion






    }
}
