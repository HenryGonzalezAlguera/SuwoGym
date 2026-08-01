using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuwoGym_Web.Models;
using System.Net;

namespace SuwoGym_Web.Controllers
{
    public class AdminController(IHttpClientFactory _http, IConfiguration _config) : Controller
    {

        #region Empleados ----------------------------------

        // LISTA DE EMPLEADOS ---

        [HttpGet]
        public async Task <IActionResult> Empleados_Lista()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario")!.Value;

            var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaEmpleadosAPI/"+idUsuario;
            var response = await client.GetFromJsonAsync<List<UsuarioModel>>(urlApi);

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

        
        // AGREGAR EMPLEADO 

        [HttpGet]
        public async Task<IActionResult> Empleados_Agregar()
        {
            var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaRolesAPI/";
            var response = await client.GetFromJsonAsync<List<RolesLista>>(urlApi);

            // Verificar respuesta
            if (response != null) // 200
            {
                ViewBag.Roles = new SelectList(response, "IdRol", "NombreRol");
                return View();
            }
            else if (response == null)
            {
                ViewBag.MensajeError = "No hay roles disponible";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");

        }


        [HttpPost]
        public IActionResult Empleados_Agregar(UsuarioModel model)
        {
            using var client = _http.CreateClient();
            var urlApi =_config["ConnectionStringsWeb:urlApi"]+ "/api/Admin/EmpleadoAgregar";
            var response = client.PostAsJsonAsync(urlApi, model).Result;

            if(response.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.MensajeExito = "Se ha agregado el usuario correctamente";
                return RedirectToAction("Empleados_Lista", "Admin");
            }
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError = response.Content.ReadAsStringAsync().Result;
                return View();
            }
            throw new Exception(
                "Ocurrió un error al intentar registrar el empleado.");
        }



        // VER DETALLES EMPLEADO

        [HttpGet]
        public async Task<IActionResult> Empleados_Ver(int i)
        {
            using var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/EmpleadoDetalles/" + i;
            var response = await client.GetFromJsonAsync<UsuarioModel>(urlApi);

            // Verificar respuesta
            if (response != null) // 200
            {
                return View(response);
            }
            else if (response == null)
            {
                ViewBag.MensajeError = "No se ha podido recuperar la información del empleado";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
         }



        // EDITAR EMPLEADOS

        [HttpGet]
        public async Task<IActionResult> Empleados_Editar(int i)
        {
            using var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/EmpleadoDetalles/" + i;
            var response = await client.GetFromJsonAsync<UsuarioModel>(urlApi);

            var urlApi2 = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaRolesAPI/";
            var response2 = await client.GetFromJsonAsync<List<RolesLista>>(urlApi2);


            // Verificar respuesta
            if (response != null &&  response2 != null) // 200
            {
                ViewBag.Roles = new SelectList(response2, "IdRol", "NombreRol", response.IdRol);
                return View(response);
            }
            else if (response == null || response2 == null)
            {
                ViewBag.MensajeError = "No se ha podido recuperar la información del empleado";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
        }


        [HttpPost]
        public IActionResult Empleados_EditarRol(UsuarioModel model)
        {
            using var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/EmpleadoEditar";
            var response = client.PutAsJsonAsync(urlApi, model).Result;

            // Verificar respuesta
            if (response != null) // 200
            {
                ViewBag.MensajeExito = "Se ha agregado el usuario correctamente";
                return RedirectToAction("Empleados_Editar", "Admin", new { i = model.IdUsuario });
            }
            else if (response == null)
            {
                ViewBag.MensajeError = "No se ha podido cambiar el  rol del empleado.";
                return RedirectToAction("Empleados_Lista", "Admin");
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
        }


        [HttpPost]
        public IActionResult Empleados_EditarEstado(UsuarioModel model)
        {
            if (model.Estado) model.Estado = false;
            else model.Estado = true;

            using var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/EmpleadoEditar";
            var response = client.PutAsJsonAsync(urlApi, model).Result;

            // Verificar respuesta
            if (response != null) // 200
            {
                ViewBag.MensajeExito = "Se ha agregado el usuario correctamente";
                return RedirectToAction("Empleados_Editar", "Admin", new { i = model.IdUsuario });
            }
            else if (response == null)
            {
                ViewBag.MensajeError = "No se ha posido cambiar el estado del empleado.";
                return RedirectToAction("Principal", "Admin");
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
        }

        #endregion


        #region Clientes ----------------------------------

        [HttpGet]
        public async Task<IActionResult> Clientes_Lista()
        {
            var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaClientesAPI";
            var response = await client.GetFromJsonAsync<List<ClienteModel>>(urlApi);

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


        // VER DETALLES CLIENTE

        [HttpGet]
        public async Task<IActionResult> Clientes_Ver(int i)
        {
            using var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ClienteDetallesAPI/" + i;
            var response = await client.GetFromJsonAsync<ClienteModel>(urlApi);

            // Verificar respuesta
            if (response != null) // 200
            {
                return View(response);
            }
            else if (response == null)
            {
                ViewBag.MensajeError = "No se ha podido recuperar la información del usuario";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
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


        
        [HttpGet]        
        public IActionResult Planes_Modificar()
        {
            // NOTA: buscar manera de que no sea por medio de una vista adicional,
        // sino por un pop up o algo
        // Este solo se le varían los precios
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
        public async Task<IActionResult> Horarios_Lista()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Horarios_Agregar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Horarios_Ver()
        {
            return View();
        }


        #endregion


        #region Dashboard ----------------------------------
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        #endregion




        #region Prueba visual --------------------------

        [HttpGet]
        public async Task<IActionResult> Prueba_Lista()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario")!.Value;

            var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/ListaEmpleadosAPI/" + idUsuario;
            var response = await client.GetFromJsonAsync<List<UsuarioModel>>(urlApi);

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
        public IActionResult Prueba_Agregar ()
        {
            return View();
        }
       

        [HttpGet]
        public async Task<IActionResult> Prueba_Detalles (int i)
        {
            using var client = _http.CreateClient();
            var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Admin/EmpleadoDetalles/" + i;
            var response = await client.GetFromJsonAsync<UsuarioModel>(urlApi);

            // Verificar respuesta
            if (response != null) // 200
            {
                return View(response);
            }
            else if (response == null)
            {
                ViewBag.MensajeError = "No se ha podido recuperar la información del empleado";
                return View();
            }
            throw new Exception("Lo sentimos, se ha presentado un problema.");
        }




        #endregion

    }
}
