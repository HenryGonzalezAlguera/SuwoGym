using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Models;

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
        public async Task <IActionResult> Empleados_Lista()
        {
            var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaEmpleadosAPI";
            var response = await client.GetFromJsonAsync<List<EmpleadoLista>>(urlApi);

            // Verificar respuesta
            if (response != null && response.Count() > 0) // 200
            {
                return View(response);
            }
            else if (response == null || response.Count() <= 0)
            {
                ViewBag.MensajeError = "No hay empleados registrados. Por favor registre un empleado.";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
        }



        [HttpGet]
        public IActionResult Empleados_Agregar()
        { 
            return View();
        }


        #endregion


        #region Clientes ----------------------------------

        [HttpGet]
        public async Task<IActionResult> Clientes_Lista()
        {
            var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaClientesAPI";
            var response = await client.GetFromJsonAsync<List<ClienteLista>>(urlApi);

            // Verificar respuesta
            if (response != null && response.Count() > 0) // 200
            {
                return View(response);
            }
            else if (response == null || response.Count() <= 0)
            {
                ViewBag.MensajeError = "No hay clientes registrados.";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
        }


        [HttpGet]
        public IActionResult Clientes_Agregar()
        {
            return View();
        }


        #endregion


        #region Planes ----------------------------------

        [HttpGet]
        public async Task<IActionResult> Planes_Lista()
        {
            var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaPlanesAPI";
            var response = await client.GetFromJsonAsync<List<PlanesMLista>>(urlApi);

            // Verificar respuesta
            if (response != null && response.Count() > 0) // 200
            {
                return View(response);
            }
            else if (response == null || response.Count() <= 0)
            {
                ViewBag.MensajeError = "No hay clientes registrados.";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
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
