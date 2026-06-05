using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Models;
using System.Diagnostics;

namespace SuwoGym_Web.Controllers
{
    public class HomeController : Controller
    {
        #region Iniciar Sesión ---------------------------

        [HttpGet]
        public IActionResult Index() 
        {
            return View();
        }

        /*
        [HttpPost]
        public IActionResult Index(Usuario user)
        {
            return View();
        }
        */

        #endregion

        #region Registrar usuario ------------------------
        [HttpGet]
        public IActionResult Registro() 
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult Registro(Usuario user) 
        {
            return View();
        }

        #endregion

        #region Recuperar Acceso -------------------------
        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecuperarAcceso(Usuario user)
        {
            return View();
        }

        #endregion
        

        public IActionResult Principal()
        {
            return View();
        }

       
    }
}
