using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SuwoGym_Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() // Inicio de sesión
        {
            return View();
        }

        public IActionResult Registro() 
        {
            return View();
        }
       
        public IActionResult RecuperarAcceso()
        {
            return View();
        }
       
        public IActionResult Principal()
        {
            return View();
        }

       
    }
}
