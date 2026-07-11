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
            ViewBag.Titulo1 = "INICIAR "; 
            ViewBag.Titulo2 = "SESIÓN";
            ViewBag.Subtitulo = "Bienvenido de nuevo a SUWÓ";
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
            ViewBag.Titulo1 = "CREA TU ";
            ViewBag.Titulo2 = "CUENTA";
            ViewBag.Subtitulo = "Empieza tu transformación hoy";
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
