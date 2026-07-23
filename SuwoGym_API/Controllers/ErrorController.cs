using Microsoft.AspNetCore.Mvc;

namespace SuwoGym_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("RegistrarError")]
        public IActionResult RegistrarError()
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error en el servidor." });
        }
    }
}
