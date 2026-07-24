using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Models;
using System.Net;

namespace SuwoGym_Web.Controllers
{
    public class HomeController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
        #region Iniciar sesión

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(
            UsuarioModel model)
        {
            using var client =
                _http.CreateClient();

            var urlApi =
                _config[
                    "ConnectionStringsWeb:urlApi"]
                + "/api/Home/IniciarSesionAPI";

            var response =
                client.PostAsJsonAsync(
                    urlApi,
                    model).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<UsuarioModel>()
                    .Result;

                HttpContext.Session.SetString(
                    "Autenticado",
                    "1");

                HttpContext.Session.SetInt32(
                    "IdUsuario",
                    datos!.IdUsuario);

                HttpContext.Session.SetString(
                    "Nombre",
                    datos.nombre);

                HttpContext.Session.SetString(
                    "Apellido",
                    datos.apellido);

                HttpContext.Session.SetString(
                    "CorreoElectronico",
                    datos.correoElectronico);

                HttpContext.Session.SetString(
                    "Token",
                    datos.Token);

                HttpContext.Session.SetInt32(
                    "IdRol",
                    datos.IdRol);

                /*
                 * Conservamos la llave "Rol" porque
                 * las vistas actuales de SUWÓ la utilizan.
                 */
                HttpContext.Session.SetString(
                    "Rol",
                    datos.NombreRol);

                HttpContext.Session.SetString(
                    "NombreRol",
                    datos.NombreRol);

                if (datos.DebeCambiarContrasenna)
                {
                    return RedirectToAction(
                        "Configuracion",
                        "Usuario");
                }

                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.Mensaje =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View();
            }

            throw new Exception(
                "Ocurrió un error al intentar iniciar sesión.");
        }

        #endregion

        #region Registrar usuarios

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(
            UsuarioModel model)
        {
            /*
             * Igual que RepoSM:
             * la WEB genera el hash antes de enviarlo.
             */
            model.contrasenna =
                BCrypt.Net.BCrypt.HashPassword(
                    model.contrasenna);

            using var client =
                _http.CreateClient();

            var urlApi =
                _config[
                    "ConnectionStringsWeb:urlApi"]
                + "/api/Home/RegistroAPI";

            var response =
                client.PostAsJsonAsync(
                    urlApi,
                    model).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                TempData["RegistroExitoso"] =
                    "¡Registro exitoso! Tu cuenta ha sido creada correctamente. Por favor, inicia sesión.";

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.Mensaje =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View();
            }

            throw new Exception(
                "Ocurrió un error al intentar registrar el usuario.");
        }

        #endregion

        #region Recuperar acceso

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(
            UsuarioModel model)
        {
            using var client =
                _http.CreateClient();

            var urlApi =
                _config[
                    "ConnectionStringsWeb:urlApi"]
                + "/api/Home/RecuperarAccesoAPI";

            var response =
                client.PostAsJsonAsync(
                    urlApi,
                    model).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            if (response.StatusCode ==
                    HttpStatusCode.NotFound
                ||
                response.StatusCode ==
                    HttpStatusCode.BadRequest)
            {
                ViewBag.Mensaje =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View();
            }

            throw new Exception(
                "Ocurrió un error al intentar recuperar su acceso.");
        }

        #endregion

        #region Cerrar sesión

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Index",
                "Home");
        }

        #endregion

        [HttpGet]
        public IActionResult Principal()
        {
            return View();
        }
    }
}