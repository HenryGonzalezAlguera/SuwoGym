using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;

namespace SuwoGym_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController (IConfiguration _databasekey) : ControllerBase
    {

        #region Gestión de Empleados --------------------------------------------------------------------

        [HttpGet("ListaEmpleadosAPI")]
        public async Task<IActionResult> ListaEmpleadosAPI()
        {
            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);
            var response = await context.QueryAsync<EmpleadoListaResponseModel>("spListarEmpleados");
            return Ok(response);
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
