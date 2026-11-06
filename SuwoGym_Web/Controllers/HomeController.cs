using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuwoGym_Web.Filters;
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
        [ValidateAntiForgeryToken]
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

            // Error de límite de intentos: informar sin exponer respuestas internas.
            ViewBag.Mensaje = response.StatusCode == HttpStatusCode.TooManyRequests
                ? "Se han realizado demasiados intentos. Espere un minuto e inténtelo de nuevo."
                : "No se pudo iniciar sesión. Inténtelo nuevamente.";
            return View();
        }

        #endregion

        #region Registrar usuarios

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            var planes = await ObtenerPlanesRegistroAsync();
            ViewBag.Membresias = planes;

            if (planes.Count == 0)
            {
                ViewBag.Mensaje =
                    "No hay planes de membresía disponibles. Verifica los datos iniciales de SQL Server y la conexión con la API.";
            }

            return View(new RegistroUsuarioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroUsuarioViewModel model)
        {
            var planes = await ObtenerPlanesRegistroAsync();
            ViewBag.Membresias = planes;

            if (planes.Count == 0)
            {
                ModelState.AddModelError(
                    nameof(model.PlanMembresia),
                    "No hay planes disponibles. Comprueba la conexión y los catálogos de membresía.");
            }
            else if (!planes.Any(p => p.IdPlan == model.PlanMembresia))
            {
                ModelState.AddModelError(
                    nameof(model.PlanMembresia),
                    "Selecciona un plan de membresía válido.");
            }

            if (!ModelState.IsValid)
                return View(model);

            // No se cambia la contraseña en el modelo del formulario:
            // ante un error de la API se conservan los datos y se puede reintentar.
            var solicitud = new
            {
                model.nombre,
                model.apellido,
                model.correoElectronico,
                model.identificacion,
                contrasenna = BCrypt.Net.BCrypt.HashPassword(model.contrasenna),
                model.PlanMembresia
            };

            try
            {
                using var client = _http.CreateClient();
                var urlApi = _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Home/RegistroAPI";
                var response = await client.PostAsJsonAsync(urlApi, solicitud);

                if (response.IsSuccessStatusCode)
                {
                    TempData["RegistroExitoso"] =
                        "¡Registro exitoso! Tu cuenta ha sido creada correctamente. Por favor, inicia sesión.";
                    return RedirectToAction("Index", "Home");
                }

                // Nunca imprimir excepciones de SQL Server en el navegador.
                ViewBag.Mensaje = response.StatusCode == HttpStatusCode.BadRequest
                    ? "No se pudo completar el registro. Comprueba que el correo y la identificación no estén registrados y que la API esté actualizada."
                    : "No se pudo completar el registro. Verifica que la API esté funcionando.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Mensaje = "No se pudo conectar con la API. Comprueba que esté iniciada.";
            }
            catch (TaskCanceledException)
            {
                ViewBag.Mensaje = "La API tardó demasiado en responder. Inténtalo nuevamente.";
            }

            return View(model);
        }

        private async Task<List<PlanModel>> ObtenerPlanesRegistroAsync()
        {
            try
            {
                using var client = _http.CreateClient();
                var urlApi = _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Home/MembresiasAPI";
                using var response = await client.GetAsync(urlApi);

                if (!response.IsSuccessStatusCode)
                    return new List<PlanModel>();

                return await response.Content
                    .ReadFromJsonAsync<List<PlanModel>>()
                    ?? new List<PlanModel>();
            }
            catch (HttpRequestException)
            {
                return new List<PlanModel>();
            }
            catch (TaskCanceledException)
            {
                return new List<PlanModel>();
            }
        }

        #endregion

        #region Recuperar acceso

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                ViewBag.Mensaje =
                    "Hay demasiadas solicitudes de recuperación. Espere 15 minutos e inténtelo de nuevo.";
                return View();
            }

            if (response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Mensaje =
                    "No se pudo procesar la solicitud de recuperación.";
                return View();
            }

            throw new Exception(
                "Ocurrió un error al intentar recuperar su acceso.");
        }

        #endregion

        #region Cerrar sesión

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Index",
                "Home");
        }

        #endregion

        [HttpGet]
        [SessionAuthorize]
        public IActionResult Principal()
        {
            return View();
        }
    }
}