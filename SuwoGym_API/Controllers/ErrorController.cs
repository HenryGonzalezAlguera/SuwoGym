using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Services;

namespace SuwoGym_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController (IConfiguration _databasekey, IUtilesService _utiles)
        : ControllerBase
    {
        [HttpGet("RegistrarError")]
        public IActionResult RegistrarError()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            using var context = new SqlConnection(_databasekey["ConnectionStringsApi:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", _utiles.ObtenerIdUsuarioToken());
            parameters.Add("@Modulo", exception?.Path);
            parameters.Add("@Mensaje", exception?.Error.Message);
            parameters.Add("@FechaHora", DateTime.Now);

            context.Execute("spRegistrarError", parameters);

            return StatusCode(500, new { mensaje = "Ocurrió un error en el servidor." });
        }
    }
}
