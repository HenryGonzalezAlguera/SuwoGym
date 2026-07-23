using Microsoft.AspNetCore.Mvc;

namespace SuwoGym_Web.Controllers
{
    public class AdminController(IHttpClientFactory _http, IConfiguration _config) : Controller
    {

        #region Dashboard ----------------------------------
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        #endregion


        #region Empleados ----------------------------------

        [HttpGet]
        public IActionResult Empleados_Lista()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Empleados_Agregar()
        {
            return View();
        }


        #endregion


        #region Clientes ----------------------------------

        [HttpGet]
        public IActionResult Clientes_Lista()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Clientes_Agregar()
        {
            return View();
        }


        #endregion


        #region Planes ----------------------------------

        [HttpGet]
        public IActionResult Planes_Lista()
        {
            return View();
        }


        // NOTA: buscar manera de que no sea por medio de una vista adicional,
        // sino por un pop up o algo
        // Este solo se le varían los precios
        [HttpGet]        
        public IActionResult Planes_Modificar()
        {
            return View();
        }


        #endregion


        #region Rutinas ----------------------------------

        [HttpGet]
        public IActionResult Rutinas_Lista()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Rutinas_Agregar()
        {
            return View();
        }


        #endregion


        #region Horarios ----------------------------------

        [HttpGet]
        public IActionResult Horarios_Lista()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Horarios_Agregar()
        {
            return View();
        }


        #endregion


    }
}
