using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuwoGym_Web.Filters;
using SuwoGym_Web.Models;
using System.Net;

namespace SuwoGym_Web.Controllers
{
    [SessionAuthorize]
    public class MiembroController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;


        public MiembroController(
            IHttpClientFactory http,
            IConfiguration config)
        {
            _http = http;
            _config = config;
        }


        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var idRol =
                HttpContext.Session.GetInt32("IdRol");

            if (idRol != 3)
            {
                context.Result =
                    RedirectToAction(
                        "Principal",
                        "Home");

                return;
            }

            base.OnActionExecuting(context);
        }


        #region Perfil del Miembro

        [HttpGet]
        public IActionResult Perfil()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/PerfilAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <MiembroPerfilModel>()
                        .Result;

                if (datos == null)
                {
                    ViewBag.MensajeError =
                        "No se pudo consultar la información del miembro.";

                    return View(
                        new MiembroPerfilModel());
                }

                return View(datos);
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new MiembroPerfilModel());
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }

        #endregion


        #region Rutinas del Miembro

        [HttpGet]
        public IActionResult Rutinas_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/RutinasListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<RutinaMiembroListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<RutinaMiembroListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<RutinaMiembroListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }


        [HttpGet]
        public IActionResult Rutinas_Ver(int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la rutina no es válido.";

                return RedirectToAction(
                    "Rutinas_Lista",
                    "Miembro");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/RutinaDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <RutinaMiembroDetalleModel>()
                        .Result;

                if (datos == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la rutina.";

                    return RedirectToAction(
                        "Rutinas_Lista",
                        "Miembro");
                }

                return View(datos);
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound
                ||
                response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Rutinas_Lista",
                    "Miembro");
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }


        [HttpGet]
        public IActionResult Rutinas_Ejercicios(int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la rutina no es válido.";

                return RedirectToAction(
                    "Rutinas_Lista",
                    "Miembro");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));


            /*
             * Consultar primero el detalle de la rutina
             * para mostrar nombre, nivel y entrenador.
             */

            var urlRutina =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/RutinaDetalleAPI/"
                + i;

            var responseRutina =
                client.GetAsync(urlRutina).Result;

            if (responseRutina.StatusCode ==
                HttpStatusCode.OK)
            {
                var rutina =
                    responseRutina.Content
                        .ReadFromJsonAsync
                        <RutinaMiembroDetalleModel>()
                        .Result;

                if (rutina == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la rutina.";

                    return RedirectToAction(
                        "Rutinas_Lista",
                        "Miembro");
                }

                ViewBag.IdRutina =
                    rutina.IdRutina;

                ViewBag.NombreRutina =
                    rutina.NombreRutina;

                ViewBag.Nivel =
                    rutina.Nivel;

                ViewBag.NombreEntrenador =
                    rutina.NombreEntrenador;
            }
            else if (
                responseRutina.StatusCode ==
                HttpStatusCode.NotFound
                ||
                responseRutina.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    responseRutina.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Rutinas_Lista",
                    "Miembro");
            }
            else if (
                responseRutina.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }
            else if (
                responseRutina.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }
            else
            {
                var detalleErrorRutina =
                    responseRutina.Content
                        .ReadAsStringAsync()
                        .Result;

                throw new Exception(
                    $"La API respondió " +
                    $"{(int)responseRutina.StatusCode} " +
                    $"({responseRutina.StatusCode}): " +
                    detalleErrorRutina);
            }


            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/RutinaEjerciciosAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<RutinaEjercicioMiembroModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<RutinaEjercicioMiembroModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<RutinaEjercicioMiembroModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Rutinas_Lista",
                    "Miembro");
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }

        #endregion
        #region Pagos del Miembro

        [HttpGet]
        public IActionResult Pagos_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/PagosListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<PagoMiembroListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<PagoMiembroListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<PagoMiembroListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }


        [HttpGet]
        public IActionResult Pagos_Ver(int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador del pago no es válido.";

                return RedirectToAction(
                    "Pagos_Lista",
                    "Miembro");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/PagoDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <PagoMiembroDetalleModel>()
                        .Result;

                if (datos == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar el detalle del pago.";

                    return RedirectToAction(
                        "Pagos_Lista",
                        "Miembro");
                }

                return View(datos);
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound
                ||
                response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Pagos_Lista",
                    "Miembro");
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }

        #endregion
        #region Reservas de Clases

        [HttpGet]
        public IActionResult Clases_Disponibles()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/ClasesDisponiblesAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<ClaseDisponibleMiembroModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<ClaseDisponibleMiembroModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<ClaseDisponibleMiembroModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }


        [HttpPost]
        public IActionResult Clases_Reservar(
            ReservaClaseMiembroModel model)
        {
            if (model.IdClaseProg <= 0)
            {
                TempData["MensajeError"] =
                    "Debe seleccionar una clase válida.";

                return RedirectToAction(
                    "Clases_Disponibles",
                    "Miembro");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/ReservaRegistrarAPI";

            var response =
                client.PostAsJsonAsync(
                    urlApi,
                    model).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                TempData["MensajeExito"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Reservas_Lista",
                    "Miembro");
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Clases_Disponibles",
                    "Miembro");
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }


        [HttpGet]
        public IActionResult Reservas_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/ReservasListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<ReservaClaseMiembroListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<ReservaClaseMiembroListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<ReservaClaseMiembroListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }


        [HttpPost]
        public IActionResult Reservas_Cancelar(
            ReservaClaseCancelarModel model)
        {
            if (model.IdReserva <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la reserva no es válido.";

                return RedirectToAction(
                    "Reservas_Lista",
                    "Miembro");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/ReservaCancelarAPI";

            var response =
                client.PutAsJsonAsync(
                    urlApi,
                    model).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                TempData["MensajeExito"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Reservas_Lista",
                    "Miembro");
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Reservas_Lista",
                    "Miembro");
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }

        #endregion
        #region Historial de Asistencia

        [HttpGet]
        public IActionResult Historial_Asistencias()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Miembro/HistorialAsistenciasAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<HistorialAsistenciaMiembroModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<HistorialAsistenciaMiembroModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<HistorialAsistenciaMiembroModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            var detalleError =
                response.Content
                    .ReadAsStringAsync()
                    .Result;

            throw new Exception(
                $"La API respondió " +
                $"{(int)response.StatusCode} " +
                $"({response.StatusCode}): " +
                detalleError);
        }

        #endregion
    }
}