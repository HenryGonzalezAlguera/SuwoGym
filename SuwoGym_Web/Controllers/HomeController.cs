using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Models;
using SuwoGym_Web.Services;
using System.Diagnostics;

namespace SuwoGym_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuthService _authService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AuthService authService, ILogger<HomeController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        #region Iniciar Sesión ---------------------------

        [HttpGet]
        public IActionResult Index()
        {
            // Si ya está autenticado, redirigir a Principal
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {
                return RedirectToAction("Principal");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUsuario user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var resultado = await _authService.LoginAsync(user);

            if (resultado.Exitoso)
            {
                return RedirectToAction("Principal");
            }

            ModelState.AddModelError("", resultado.Mensaje);
            return View(user);
        }

        #endregion

        #region Registrar usuario ------------------------

        [HttpGet]
        public IActionResult Registro()
        {
            // Si ya está autenticado, redirigir a Principal
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("IdUsuario")))
            {
                return RedirectToAction("Principal");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var resultado = await _authService.RegistroAsync(user);

            if (resultado.Exitoso)
            {
                TempData["Mensaje"] = "Registro exitoso. Por favor, inicia sesión.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado.Mensaje);
            return View(user);
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
            // Implementar lógica de recuperación de acceso
            return View();
        }

        #endregion

        public IActionResult Principal()
        {
            // Verificar si el usuario está autenticado
            var idUsuario = HttpContext.Session.GetString("IdUsuario");
            if (string.IsNullOrEmpty(idUsuario))
            {
                return RedirectToAction("Index");
            }

            // Pasar los datos del usuario a la vista
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var nombreRol = HttpContext.Session.GetString("NombreRol");

            ViewBag.NombreUsuario = nombreUsuario;
            ViewBag.NombreRol = nombreRol;

            return View();
        }

        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Index");
        }
    }
}
