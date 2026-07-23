using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Models;
using System.Diagnostics;
using System.Net;

namespace SuwoGym_Web.Controllers
{
    public class HomeController(IHttpClientFactory _http, IConfiguration _config) : Controller
    {

        #region Iniciar Sesion

        [HttpGet]
        public IActionResult Index()
        {



            return View();
        }

        [HttpPost]
        public IActionResult Index(UsuarioModel model)
        {

            using (var client = _http.CreateClient())
            {



                var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Home/IniciarSesionAPI";

                var response = client.PostAsJsonAsync(urlApi, model).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var datos = response.Content.ReadFromJsonAsync<UsuarioModel>().Result;
                    HttpContext.Session.SetString("Autenticado", "1");
                    HttpContext.Session.SetString("Nombre", datos!.nombre);
                    HttpContext.Session.SetString("Apellido", datos!.apellido);
                    HttpContext.Session.SetString("Rol", datos!.NombreRol);
                    HttpContext.Session.SetString("CorreoElectronico", datos!.correoElectronico);

                    return RedirectToAction("Principal", "Home");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {


                    ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                    return View();

                }
                else
                {

                    throw new Exception("Ocurrió un error al intentar iniciar sesión.");
                }



            }



        }
        #endregion

        #region RegistrarUsuarios
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(UsuarioModel model)
        {

            using (var client = _http.CreateClient())
            {


                var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Home/RegistroAPI";
                ;
                var response = client.PostAsJsonAsync(urlApi, model).Result;


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Mostrar mensaje de éxito y redirigir al login
                    TempData["RegistroExitoso"] = "¡Registro exitoso! Tu cuenta ha sido creada correctamente. Por favor, inicia sesión.";
                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {


                    ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                    return View();

                }
                else
                {

                    throw new Exception("Ocurrió un error al intentar registrar el usuario.");
                }


            }

        }
        #endregion

        #region Cerrar Sesión

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Salir()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Recuperar Acceso

        [HttpGet]
        public IActionResult RecuperarAcceso()

        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(UsuarioModel model)

        {
            using (var client = _http.CreateClient())
            {



                var urlApi = _config["ConnectionStringsWeb:urlApi"] + "/api/Home/RecuperarAccesoAPI";

                var response = client.PostAsJsonAsync(urlApi, model).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {


                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {


                    ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                    return View();

                }
                else
                {

                    throw new Exception("Ocurrió un error al intentar recuperar su acceso");
                }
            }

        }
        #endregion

        public IActionResult Principal()
        {
            // Obtener el nombre del usuario desde la sesión
            var nombre = HttpContext.Session.GetString("Nombre");
            var rol = HttpContext.Session.GetString("Rol");

            // Si no hay usuario autenticado, redirigir a login
            if (string.IsNullOrEmpty(nombre))
            {
                return RedirectToAction("Index", "Home");
            }

            // Pasar los datos a la vista
            ViewBag.NombreUsuario = nombre;
            ViewBag.NombreRol = rol;

            return View();
        }

    }
}

