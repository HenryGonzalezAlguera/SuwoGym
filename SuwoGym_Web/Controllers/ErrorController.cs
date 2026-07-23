using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SuwoGym_Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CapturarError()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return View("Error");
        }
    }
}
