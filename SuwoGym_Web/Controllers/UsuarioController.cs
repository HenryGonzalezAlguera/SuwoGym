using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Filters;
using SuwoGym_Web.Models;
using System.Net;
using System.Net.Http.Headers;

namespace SuwoGym_Web.Controllers
{
    [SessionAuthorize]
    [AutoValidateAntiforgeryToken]
    public class UsuarioController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
        #region Configuración

        [HttpGet]
        public IActionResult Configuracion()
        {
            var urlApi =
     _config["ConnectionStringsWeb:urlApi"]
     + "/api/Usuario/ConsultarUsuarioAPI";

            using var client = _http.CreateClient();

            var token = HttpContext.Session
                .GetString("Token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);


            var response = client
                .GetAsync(urlApi)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<UsuarioModel>()
                    .Result;

                return View(datos);
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar la información del usuario.");
        }

        #endregion

        #region Cambiar Contraseña

        [HttpPost]
        public IActionResult CambiarContrasenna(
            UsuarioModel model)
        {
            if (model.contrasenna !=
                model.confirmarContrasenna)
            {
                ViewBag.MensajeSeguridad =
                    "Las contraseñas no coinciden.";

                ViewBag.ClaseMensajeSeguridad =
                    "danger";

                return View(
                    "Configuracion",
                    model);
            }

          

            model.contrasenna =
                BCrypt.Net.BCrypt.HashPassword(
                    model.contrasenna);

            using var client = _http.CreateClient();

            var token = HttpContext.Session
                .GetString("Token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Usuario/CambiarContrasennaAPI";

            var response = client
                .PutAsJsonAsync(
                    urlApi,
                    model)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }
            else if (response.StatusCode ==
                     HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeSeguridad =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                ViewBag.ClaseMensajeSeguridad =
                    "danger";

                return View(
                    "Configuracion",
                    model);
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            throw new Exception(
                "Ocurrió un error al actualizar la contraseña.");
        }

        #endregion

    }
}