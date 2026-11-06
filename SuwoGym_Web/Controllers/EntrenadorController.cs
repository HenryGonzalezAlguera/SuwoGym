using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuwoGym_Web.Filters;
using SuwoGym_Web.Models;
using System.Net;

namespace SuwoGym_Web.Controllers
{
    [SessionAuthorize(2)]
    [AutoValidateAntiforgeryToken]
    public class EntrenadorController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
#region Clientes asignados

        [HttpGet]
        public IActionResult Clientes_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/ClientesAsignadosAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<EntrenadorClienteModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<EntrenadorClienteModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<EntrenadorClienteModel>());
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
        public IActionResult Clientes_Ver(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/ClienteDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <EntrenadorClienteDetalleModel>()
                        .Result;

                return View(datos);
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Clientes_Lista",
                    "Entrenador");
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Clientes_Lista",
                    "Entrenador");
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

        #region Gestión de Rutinas

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
                + "/api/Entrenador/RutinasListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<RutinaListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<RutinaListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<RutinaListaModel>());
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
        public IActionResult Rutinas_Agregar()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/ClientesAsignadosAPI";

            var response =
                client.GetAsync(urlApi).Result;

            ViewBag.Niveles =
                new SelectList(
                    new List<string>
                    {
                "Principiante",
                "Intermedio",
                "Avanzado"
                    });

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var clientes =
                    response.Content
                        .ReadFromJsonAsync
                        <List<EntrenadorClienteModel>>()
                        .Result;

                var clientesSeleccion =
                    (clientes ??
                     new List<EntrenadorClienteModel>())
                    .Select(x => new
                    {
                        x.IdUsuario,

                        NombreCompleto =
                            x.Nombre + " " + x.Apellido
                    })
                    .ToList();

                ViewBag.Clientes =
                    new SelectList(
                        clientesSeleccion,
                        "IdUsuario",
                        "NombreCompleto");

                return View(
                    new RutinaCrearModel());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                ViewBag.Clientes =
                    new SelectList(
                        new List<EntrenadorClienteModel>(),
                        "IdUsuario",
                        "Nombre");

                return View(
                    new RutinaCrearModel());
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
        public IActionResult Rutinas_Agregar(
            RutinaCrearModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaRegistrarAPI";

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
                    "Rutinas_Lista",
                    "Entrenador");
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                var urlClientes =
                    _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Entrenador/ClientesAsignadosAPI";

                var responseClientes =
                    client.GetAsync(urlClientes).Result;

                if (responseClientes.StatusCode ==
                    HttpStatusCode.OK)
                {
                    var clientes =
                        responseClientes.Content
                            .ReadFromJsonAsync
                            <List<EntrenadorClienteModel>>()
                            .Result;

                    var clientesSeleccion =
                        (clientes ??
                         new List<EntrenadorClienteModel>())
                        .Select(x => new
                        {
                            x.IdUsuario,

                            NombreCompleto =
                                x.Nombre + " " + x.Apellido
                        })
                        .ToList();

                    ViewBag.Clientes =
                        new SelectList(
                            clientesSeleccion,
                            "IdUsuario",
                            "NombreCompleto",
                            model.IdCliente);
                }
                else
                {
                    ViewBag.Clientes =
                        new SelectList(
                            new List<EntrenadorClienteModel>(),
                            "IdUsuario",
                            "Nombre");
                }

                ViewBag.Niveles =
                    new SelectList(
                        new List<string>
                        {
                    "Principiante",
                    "Intermedio",
                    "Avanzado"
                        },
                        model.Nivel);

                return View(model);
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
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <RutinaDetalleModel>()
                        .Result;

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
                    "Entrenador");
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
        public IActionResult Rutinas_Editar(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var rutina =
                    response.Content
                        .ReadFromJsonAsync
                        <RutinaDetalleModel>()
                        .Result;

                if (rutina == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la rutina.";

                    return RedirectToAction(
                        "Rutinas_Lista",
                        "Entrenador");
                }

                var model =
                    new RutinaEditarModel
                    {
                        IdRutina =
                            rutina.IdRutina,

                        NombreRutina =
                            rutina.NombreRutina,

                        Nivel =
                            rutina.Nivel,

                        Detalles =
                            rutina.Detalles
                    };

                ViewBag.Niveles =
                    new SelectList(
                        new List<string>
                        {
                    "Principiante",
                    "Intermedio",
                    "Avanzado"
                        },
                        model.Nivel);

                return View(model);
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
                    "Entrenador");
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
        public IActionResult Rutinas_Editar(
            RutinaEditarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaEditarAPI";

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
                    "Rutinas_Ver",
                    "Entrenador",
                    new
                    {
                        i = model.IdRutina
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                ViewBag.Niveles =
                    new SelectList(
                        new List<string>
                        {
                    "Principiante",
                    "Intermedio",
                    "Avanzado"
                        },
                        model.Nivel);

                return View(model);
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
        public IActionResult Rutinas_CambiarEstado(
            RutinaEstadoModel model)
        {
            model.Estado =
                !model.Estado;

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaCambiarEstadoAPI";

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
                    "Rutinas_Ver",
                    "Entrenador",
                    new
                    {
                        i = model.IdRutina
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Rutinas_Ver",
                    "Entrenador",
                    new
                    {
                        i = model.IdRutina
                    });
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

        #region Gestión de Ejercicios

        [HttpGet]
        public IActionResult Ejercicios_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/EjerciciosListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<EjercicioListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<EjercicioListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<EjercicioListaModel>());
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
        public IActionResult Ejercicios_Agregar()
        {
            return View(
                new EjercicioCrearModel());
        }


        [HttpPost]
        public IActionResult Ejercicios_Agregar(
            EjercicioCrearModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/EjercicioRegistrarAPI";

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
                    "Ejercicios_Lista",
                    "Entrenador");
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(model);
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
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            /*
             * Primero se consulta la rutina para comprobar
             * que pertenece al entrenador.
             */

            var urlRutina =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaDetalleAPI/"
                + i;

            var responseRutina =
                client.GetAsync(urlRutina).Result;

            if (responseRutina.StatusCode ==
                HttpStatusCode.OK)
            {
                var rutina =
                    responseRutina.Content
                        .ReadFromJsonAsync
                        <RutinaDetalleModel>()
                        .Result;

                if (rutina == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la rutina.";

                    return RedirectToAction(
                        "Rutinas_Lista",
                        "Entrenador");
                }

                ViewBag.IdRutina =
                    rutina.IdRutina;

                ViewBag.NombreRutina =
                    rutina.NombreRutina;

                ViewBag.NombreCliente =
                    rutina.NombreCliente;
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
                    "Entrenador");
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
                + "/api/Entrenador/RutinaEjerciciosListaAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<RutinaEjercicioListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<RutinaEjercicioListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<RutinaEjercicioListaModel>());
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
                    "Entrenador");
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
        public IActionResult Rutinas_EjercicioAgregar(
            int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlRutina =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaDetalleAPI/"
                + i;

            var responseRutina =
                client.GetAsync(urlRutina).Result;

            if (responseRutina.StatusCode ==
                HttpStatusCode.OK)
            {
                var rutina =
                    responseRutina.Content
                        .ReadFromJsonAsync
                        <RutinaDetalleModel>()
                        .Result;

                if (rutina == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la rutina.";

                    return RedirectToAction(
                        "Rutinas_Lista",
                        "Entrenador");
                }

                ViewBag.NombreRutina =
                    rutina.NombreRutina;

                ViewBag.NombreCliente =
                    rutina.NombreCliente;
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
                    "Entrenador");
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


            var urlEjercicios =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/EjerciciosListaAPI";

            var responseEjercicios =
                client.GetAsync(urlEjercicios).Result;

            if (responseEjercicios.StatusCode ==
                HttpStatusCode.OK)
            {
                var ejercicios =
                    responseEjercicios.Content
                        .ReadFromJsonAsync
                        <List<EjercicioListaModel>>()
                        .Result;

                var ejerciciosActivos =
                    (ejercicios ??
                     new List<EjercicioListaModel>())
                    .Where(x => x.Estado)
                    .Select(x => new
                    {
                        x.IdEjercicio,

                        NombreMostrar =
                            x.NombreEjercicio
                            + " - "
                            + x.GrupoMuscular
                    })
                    .ToList();

                ViewBag.Ejercicios =
                    new SelectList(
                        ejerciciosActivos,
                        "IdEjercicio",
                        "NombreMostrar");
            }
            else if (
                responseEjercicios.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    responseEjercicios.Content
                        .ReadAsStringAsync()
                        .Result;

                ViewBag.Ejercicios =
                    new SelectList(
                        new List<EjercicioListaModel>(),
                        "IdEjercicio",
                        "NombreEjercicio");
            }
            else if (
                responseEjercicios.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }
            else if (
                responseEjercicios.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }
            else
            {
                var detalleErrorEjercicios =
                    responseEjercicios.Content
                        .ReadAsStringAsync()
                        .Result;

                throw new Exception(
                    $"La API respondió " +
                    $"{(int)responseEjercicios.StatusCode} " +
                    $"({responseEjercicios.StatusCode}): " +
                    detalleErrorEjercicios);
            }


            ViewBag.Dias =
                new SelectList(
                    new List<string>
                    {
                "Lunes",
                "Martes",
                "Miércoles",
                "Jueves",
                "Viernes",
                "Sábado",
                "Domingo"
                    });


            var model =
                new RutinaEjercicioAgregarModel
                {
                    IdRutina = i,
                    Series = 3,
                    Repeticiones = 10,
                    DescansoSegundos = 60,
                    Orden = 1
                };

            return View(model);
        }
        [HttpGet]
        public IActionResult Rutinas_EjercicioEditar(
    int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador del ejercicio asignado no es válido.";

                return RedirectToAction(
                    "Rutinas_Lista",
                    "Entrenador");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaEjercicioDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var ejercicio =
                    response.Content
                        .ReadFromJsonAsync
                        <RutinaEjercicioDetalleModel>()
                        .Result;

                if (ejercicio == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar el ejercicio de la rutina.";

                    return RedirectToAction(
                        "Rutinas_Lista",
                        "Entrenador");
                }

                var model =
                    new RutinaEjercicioEditarModel
                    {
                        IdRutinaEjercicio =
                            ejercicio.IdRutinaEjercicio,

                        IdRutina =
                            ejercicio.IdRutina,

                        NombreEjercicio =
                            ejercicio.NombreEjercicio,

                        GrupoMuscular =
                            ejercicio.GrupoMuscular,

                        DiaSemana =
                            ejercicio.DiaSemana,

                        Series =
                            ejercicio.Series,

                        Repeticiones =
                            ejercicio.Repeticiones,

                        PesoSugerido =
                            ejercicio.PesoSugerido,

                        DescansoSegundos =
                            ejercicio.DescansoSegundos,

                        Observacion =
                            ejercicio.Observacion,

                        Orden =
                            ejercicio.Orden
                    };

                ViewBag.Dias =
                    new SelectList(
                        new List<string>
                        {
                    "Lunes",
                    "Martes",
                    "Miércoles",
                    "Jueves",
                    "Viernes",
                    "Sábado",
                    "Domingo"
                        },
                        model.DiaSemana);

                return View(model);
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
                    "Entrenador");
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
        public IActionResult Rutinas_EjercicioEditar(
    RutinaEjercicioEditarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaEjercicioEditarAPI";

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
                    "Rutinas_EjercicioVer",
                    "Entrenador",
                    new
                    {
                        i = model.IdRutinaEjercicio
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                /*
                 * Se consulta nuevamente el detalle para recuperar
                 * el nombre, grupo muscular y rutina.
                 */

                var urlDetalle =
                    _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Entrenador/RutinaEjercicioDetalleAPI/"
                    + model.IdRutinaEjercicio;

                var responseDetalle =
                    client.GetAsync(urlDetalle).Result;

                if (responseDetalle.StatusCode ==
                    HttpStatusCode.OK)
                {
                    var ejercicio =
                        responseDetalle.Content
                            .ReadFromJsonAsync
                            <RutinaEjercicioDetalleModel>()
                            .Result;

                    if (ejercicio != null)
                    {
                        model.IdRutina =
                            ejercicio.IdRutina;

                        model.NombreEjercicio =
                            ejercicio.NombreEjercicio;

                        model.GrupoMuscular =
                            ejercicio.GrupoMuscular;
                    }
                }

                ViewBag.Dias =
                    new SelectList(
                        new List<string>
                        {
                    "Lunes",
                    "Martes",
                    "Miércoles",
                    "Jueves",
                    "Viernes",
                    "Sábado",
                    "Domingo"
                        },
                        model.DiaSemana);

                return View(model);
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
        public IActionResult Rutinas_EjercicioAgregar(
            RutinaEjercicioAgregarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaEjercicioAgregarAPI";

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
                    "Rutinas_Ejercicios",
                    "Entrenador",
                    new
                    {
                        i = model.IdRutina
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                /*
                 * Volver a cargar los datos necesarios
                 * para los select de la vista.
                 */

                var urlRutina =
                    _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Entrenador/RutinaDetalleAPI/"
                    + model.IdRutina;

                var responseRutina =
                    client.GetAsync(urlRutina).Result;

                if (responseRutina.StatusCode ==
                    HttpStatusCode.OK)
                {
                    var rutina =
                        responseRutina.Content
                            .ReadFromJsonAsync
                            <RutinaDetalleModel>()
                            .Result;

                    ViewBag.NombreRutina =
                        rutina?.NombreRutina;

                    ViewBag.NombreCliente =
                        rutina?.NombreCliente;
                }


                var urlEjercicios =
                    _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Entrenador/EjerciciosListaAPI";

                var responseEjercicios =
                    client.GetAsync(urlEjercicios).Result;

                if (responseEjercicios.StatusCode ==
                    HttpStatusCode.OK)
                {
                    var ejercicios =
                        responseEjercicios.Content
                            .ReadFromJsonAsync
                            <List<EjercicioListaModel>>()
                            .Result;

                    var ejerciciosActivos =
                        (ejercicios ??
                         new List<EjercicioListaModel>())
                        .Where(x => x.Estado)
                        .Select(x => new
                        {
                            x.IdEjercicio,

                            NombreMostrar =
                                x.NombreEjercicio
                                + " - "
                                + x.GrupoMuscular
                        })
                        .ToList();

                    ViewBag.Ejercicios =
                        new SelectList(
                            ejerciciosActivos,
                            "IdEjercicio",
                            "NombreMostrar",
                            model.IdEjercicio);
                }
                else
                {
                    ViewBag.Ejercicios =
                        new SelectList(
                            new List<EjercicioListaModel>(),
                            "IdEjercicio",
                            "NombreEjercicio");
                }


                ViewBag.Dias =
                    new SelectList(
                        new List<string>
                        {
                    "Lunes",
                    "Martes",
                    "Miércoles",
                    "Jueves",
                    "Viernes",
                    "Sábado",
                    "Domingo"
                        },
                        model.DiaSemana);

                return View(model);
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
        public IActionResult Rutinas_EjercicioVer(
            int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaEjercicioDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <RutinaEjercicioDetalleModel>()
                        .Result;

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
                    "Entrenador");
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
        public IActionResult Rutinas_EjercicioQuitar(
            RutinaEjercicioQuitarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/RutinaEjercicioQuitarAPI";

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
                    "Rutinas_Ejercicios",
                    "Entrenador",
                    new
                    {
                        i = model.IdRutina
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest
                ||
                response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Rutinas_Ejercicios",
                    "Entrenador",
                    new
                    {
                        i = model.IdRutina
                    });
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

        #region Horarios del Entrenador

        [HttpGet]
        public IActionResult Horarios_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/HorariosListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<ClaseEntrenadorListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<ClaseEntrenadorListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<ClaseEntrenadorListaModel>());
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
        public IActionResult Horarios_Ver(int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la clase programada no es válido.";

                return RedirectToAction(
                    "Horarios_Lista",
                    "Entrenador");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/HorarioDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <ClaseEntrenadorDetalleModel>()
                        .Result;

                if (datos == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar el detalle de la clase.";

                    return RedirectToAction(
                        "Horarios_Lista",
                        "Entrenador");
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
                    "Horarios_Lista",
                    "Entrenador");
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

        #region Asistencia de Clases

        [HttpGet]
        public IActionResult Asistencias_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/AsistenciasListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<AsistenciaClaseResumenModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<AsistenciaClaseResumenModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<AsistenciaClaseResumenModel>());
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
        public IActionResult Asistencias_Participantes(int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la clase programada no es válido.";

                return RedirectToAction(
                    "Asistencias_Lista",
                    "Entrenador");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/ParticipantesClaseAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <ParticipantesClaseViewModel>()
                        .Result;

                if (datos == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la información de la clase.";

                    return RedirectToAction(
                        "Asistencias_Lista",
                        "Entrenador");
                }

                datos.Participantes ??=
                    new List<ParticipanteClaseEntrenadorModel>();

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
                    "Asistencias_Lista",
                    "Entrenador");
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
        public IActionResult Asistencias_Registrar(
            AsistenciaClaseRegistrarModel model)
        {
            if (model.IdReserva <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la reserva no es válido.";

                return RedirectToAction(
                    "Asistencias_Lista",
                    "Entrenador");
            }

            if (model.IdClaseProg <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la clase programada no es válido.";

                return RedirectToAction(
                    "Asistencias_Lista",
                    "Entrenador");
            }

            model.EstadoAsistencia =
                model.EstadoAsistencia?
                    .Trim()
                ?? string.Empty;

            if (!string.Equals(
                    model.EstadoAsistencia,
                    "Presente",
                    StringComparison.OrdinalIgnoreCase)
                &&
                !string.Equals(
                    model.EstadoAsistencia,
                    "Ausente",
                    StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeError"] =
                    "Debe seleccionar Presente o Ausente.";

                return RedirectToAction(
                    "Asistencias_Participantes",
                    "Entrenador",
                    new
                    {
                        i = model.IdClaseProg
                    });
            }

            if (!string.IsNullOrWhiteSpace(
                model.Observacion))
            {
                model.Observacion =
                    model.Observacion.Trim();

                if (model.Observacion.Length > 300)
                {
                    TempData["MensajeError"] =
                        "La observación no puede superar los 300 caracteres.";

                    return RedirectToAction(
                        "Asistencias_Participantes",
                        "Entrenador",
                        new
                        {
                            i = model.IdClaseProg
                        });
                }
            }
            else
            {
                model.Observacion = null;
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Entrenador/AsistenciaRegistrarAPI";

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
                    "Asistencias_Participantes",
                    "Entrenador",
                    new
                    {
                        i = model.IdClaseProg
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Asistencias_Participantes",
                    "Entrenador",
                    new
                    {
                        i = model.IdClaseProg
                    });
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