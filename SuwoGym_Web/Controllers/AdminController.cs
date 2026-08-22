using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuwoGym_Web.Filters;
using SuwoGym_Web.Models;
using System.Net;

namespace SuwoGym_Web.Controllers
{
    [SessionAuthorize]
    public class AdminController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            if (HttpContext.Session.GetInt32("IdRol") != 1)
            {
                context.Result = RedirectToAction(
                    "Principal",
                    "Home");

                return;
            }

            base.OnActionExecuting(context);
        }

        #region Empleados

        [HttpGet]
        public IActionResult Empleados_Lista()
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaEmpleadosAPI";

            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<List<UsuarioModel>>()
                    .Result;

                return View(datos ?? new List<UsuarioModel>());
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return View(new List<UsuarioModel>());
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar los empleados.");
        }

        [HttpGet]
        public IActionResult Empleados_Agregar()
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaRolesAPI";

            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<List<RolesLista>>()
                    .Result;

                ViewBag.Roles = new SelectList(
                    datos ?? new List<RolesLista>(),
                    "IdRol",
                    "NombreRol");

                return View();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError = response.Content
                    .ReadAsStringAsync()
                    .Result;

                ViewBag.Roles = new SelectList(
                    new List<RolesLista>(),
                    "IdRol",
                    "NombreRol");

                return View();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar los roles.");
        }

        [HttpPost]
        public IActionResult Empleados_Agregar(UsuarioModel model)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/EmpleadoAgregar";

            var response = client
                .PostAsJsonAsync(urlApi, model)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(
                    "Empleados_Lista",
                    "Admin");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError = response.Content
                    .ReadAsStringAsync()
                    .Result;

                var urlRoles =
                    _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Admin/ListaRolesAPI";

                var responseRoles = client
                    .GetAsync(urlRoles)
                    .Result;

                if (responseRoles.StatusCode == HttpStatusCode.OK)
                {
                    var roles = responseRoles.Content
                        .ReadFromJsonAsync<List<RolesLista>>()
                        .Result;

                    ViewBag.Roles = new SelectList(
                        roles ?? new List<RolesLista>(),
                        "IdRol",
                        "NombreRol",
                        model.IdRol);
                }
                else if (responseRoles.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Logout", "Home");
                }
                else if (responseRoles.StatusCode == HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Principal", "Home");
                }
                else
                {
                    ViewBag.Roles = new SelectList(
                        new List<RolesLista>(),
                        "IdRol",
                        "NombreRol");
                }

                return View(model);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al intentar registrar el empleado.");
        }

        [HttpGet]
        public IActionResult Empleados_Ver(int i)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/EmpleadoDetalles/"
                + i;

            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<UsuarioModel>()
                    .Result;

                return View(datos);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Lista",
                    "Admin");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar el empleado.");
        }

        [HttpGet]
        public IActionResult Empleados_Editar(int i)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlEmpleado =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/EmpleadoDetalles/"
                + i;

            var responseEmpleado = client
                .GetAsync(urlEmpleado)
                .Result;

            if (responseEmpleado.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (responseEmpleado.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }
            else if (responseEmpleado.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] = responseEmpleado.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Lista",
                    "Admin");
            }

            var urlRoles =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaRolesAPI";

            var responseRoles = client
                .GetAsync(urlRoles)
                .Result;

            if (responseRoles.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (responseRoles.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            if (responseEmpleado.StatusCode == HttpStatusCode.OK
                && responseRoles.StatusCode == HttpStatusCode.OK)
            {
                var empleado = responseEmpleado.Content
                    .ReadFromJsonAsync<UsuarioModel>()
                    .Result;

                var roles = responseRoles.Content
                    .ReadFromJsonAsync<List<RolesLista>>()
                    .Result;

                ViewBag.Roles = new SelectList(
                    roles ?? new List<RolesLista>(),
                    "IdRol",
                    "NombreRol",
                    empleado!.IdRol);

                return View(empleado);
            }
            else if (responseRoles.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] = responseRoles.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Lista",
                    "Admin");
            }

            throw new Exception(
                "Ocurrió un error al cargar la información del empleado.");
        }


        [HttpPost]
        public IActionResult Empleados_EditarDatos(UsuarioModel model)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/EmpleadoEditarDatosP";

            var response = client
                .PutAsJsonAsync(urlApi, model)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TempData["MensajeExito"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Ver",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Editar",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al cambiar el rol del empleado.");
        }

        [HttpPost]
        public IActionResult Empleados_EditarRol(UsuarioModel model)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/EmpleadoEditar";

            var response = client
                .PutAsJsonAsync(urlApi, model)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TempData["MensajeExito"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Ver",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Editar",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al cambiar el rol del empleado.");
        }

        [HttpPost]
        public IActionResult Empleados_EditarEstado(UsuarioModel model)
        {
            model.Estado = !model.Estado;

            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/EmpleadoEditar";

            var response = client
                .PutAsJsonAsync(urlApi, model)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TempData["MensajeExito"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Ver",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Empleados_Editar",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al cambiar el estado del empleado.");
        }

        #endregion

        #region Clientes

        [HttpGet]
        public IActionResult Clientes_Lista()
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaClientesAPI";

            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<List<ClienteModel>>()
                    .Result;

                return View(datos ?? new List<ClienteModel>());
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return View(new List<ClienteModel>());
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar los clientes.");
        }

        [HttpGet]
        public IActionResult Clientes_Ver(int i)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteDetallesAPI/"
                + i;

            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<ClienteModel>()
                    .Result;

                return View(datos);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar el cliente.");
        }

        [HttpPost]
        public IActionResult Clientes_CambiarEstado(ClienteModel model)
        {
            model.Estado = !model.Estado;

            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteCambiarEstadoAPI";

            var response = client
                .PutAsJsonAsync(urlApi, model)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TempData["MensajeExito"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Clientes_Ver",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["MensajeError"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Clientes_Ver",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al cambiar el estado del cliente.");
        }

        [HttpGet]
        public IActionResult Clientes_Editar(int i)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteDetallesAPI/"
                + i;

            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content
                    .ReadFromJsonAsync<ClienteModel>()
                    .Result;

                return View(datos);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar el cliente.");
        }

        [HttpGet]
        public IActionResult Clientes_AsignarMembresia(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlCliente =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteDetallesAPI/"
                + i;

            var responseCliente =
                client.GetAsync(urlCliente).Result;

            if (responseCliente.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (responseCliente.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            if (responseCliente.StatusCode ==
                HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] =
                    responseCliente.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }

            if (responseCliente.StatusCode !=
                HttpStatusCode.OK)
            {
                var detalleError =
                    responseCliente.Content
                        .ReadAsStringAsync()
                        .Result;

                throw new Exception(
                    $"La API respondió " +
                    $"{(int)responseCliente.StatusCode} " +
                    $"({responseCliente.StatusCode}): " +
                    detalleError);
            }

            var cliente =
                responseCliente.Content
                    .ReadFromJsonAsync<ClienteModel>()
                    .Result;

            if (cliente == null)
            {
                TempData["MensajeError"] =
                    "No se pudo consultar la información del cliente.";

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }

            if (string.Equals(
                cliente.EstadoMembresia,
                "Activa",
                StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeError"] =
                    "El cliente ya tiene una membresía activa.";

                return RedirectToAction(
                    "Clientes_Ver",
                    "Admin",
                    new
                    {
                        i = cliente.IdUsuario
                    });
            }

            var urlPlanes =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaPlanesAPI";

            var responsePlanes =
                client.GetAsync(urlPlanes).Result;

            if (responsePlanes.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (responsePlanes.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            if (responsePlanes.StatusCode ==
                HttpStatusCode.OK)
            {
                var planes =
                    responsePlanes.Content
                        .ReadFromJsonAsync<List<PlanesMLista>>()
                        .Result;

                ViewBag.Planes =
                    new SelectList(
                        planes ?? new List<PlanesMLista>(),
                        "IdPlan",
                        "NombrePlan");
            }
            else
            {
                ViewBag.Planes =
                    new SelectList(
                        new List<PlanesMLista>(),
                        "IdPlan",
                        "NombrePlan");

                ViewBag.MensajeError =
                    responsePlanes.Content
                        .ReadAsStringAsync()
                        .Result;
            }

            var model =
                new MembresiaAsignarModel
                {
                    IdUsuario =
                        cliente.IdUsuario,

                    NombreCliente =
                        cliente.Nombre
                        + " "
                        + cliente.Apellido
                };

            return View(model);
        }

        [HttpPost]
        public IActionResult Clientes_Editar(ClienteModel model)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " + HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteEditarAPI";

            var response = client
                .PutAsJsonAsync(urlApi, model)
                .Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TempData["MensajeExito"] = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return RedirectToAction(
                    "Clientes_Ver",
                    "Admin",
                    new { i = model.IdUsuario });
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError = response.Content
                    .ReadAsStringAsync()
                    .Result;

                return View(model);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Principal", "Home");
            }

            throw new Exception(
                "Ocurrió un error al actualizar el cliente.");
        }

        [HttpPost]
        public IActionResult Clientes_AsignarMembresia(
            MembresiaAsignarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteAsignarMembresiaAPI";

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
                    "Clientes_Ver",
                    "Admin",
                    new
                    {
                        i = model.IdUsuario
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                var urlPlanes =
                    _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Admin/ListaPlanesAPI";

                var responsePlanes =
                    client.GetAsync(urlPlanes).Result;

                if (responsePlanes.StatusCode ==
                    HttpStatusCode.OK)
                {
                    var planes =
                        responsePlanes.Content
                            .ReadFromJsonAsync<List<PlanesMLista>>()
                            .Result;

                    ViewBag.Planes =
                        new SelectList(
                            planes ?? new List<PlanesMLista>(),
                            "IdPlan",
                            "NombrePlan",
                            model.IdPlan);
                }
                else
                {
                    ViewBag.Planes =
                        new SelectList(
                            new List<PlanesMLista>(),
                            "IdPlan",
                            "NombrePlan");
                }

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
        public IActionResult Clientes_CancelarMembresia(
            MembresiaCancelarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteCancelarMembresiaAPI";

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
                    "Clientes_Ver",
                    "Admin",
                    new
                    {
                        i = model.IdUsuario
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
                    "Clientes_Ver",
                    "Admin",
                    new
                    {
                        i = model.IdUsuario
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

        [HttpGet]
        public IActionResult Clientes_AsignarEntrenador(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlCliente =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteDetallesAPI/"
                + i;

            var responseCliente =
                client.GetAsync(urlCliente).Result;

            if (responseCliente.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (responseCliente.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            if (responseCliente.StatusCode ==
                HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] =
                    responseCliente.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }

            if (responseCliente.StatusCode !=
                HttpStatusCode.OK)
            {
                var detalleError =
                    responseCliente.Content
                        .ReadAsStringAsync()
                        .Result;

                throw new Exception(
                    $"La API respondió " +
                    $"{(int)responseCliente.StatusCode} " +
                    $"({responseCliente.StatusCode}): " +
                    detalleError);
            }

            var cliente =
                responseCliente.Content
                    .ReadFromJsonAsync<ClienteModel>()
                    .Result;

            if (cliente == null)
            {
                TempData["MensajeError"] =
                    "No se pudo consultar la información del cliente.";

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }

            var urlEntrenadores =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaEntrenadoresActivosAPI";

            var responseEntrenadores =
                client.GetAsync(urlEntrenadores).Result;

            if (responseEntrenadores.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (responseEntrenadores.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            if (responseEntrenadores.StatusCode ==
                HttpStatusCode.OK)
            {
                var entrenadores =
                    responseEntrenadores.Content
                        .ReadFromJsonAsync
                        <List<EntrenadorListaModel>>()
                        .Result;

                ViewBag.Entrenadores =
                    new SelectList(
                        entrenadores ??
                        new List<EntrenadorListaModel>(),
                        "IdEntrenador",
                        "NombreCompleto");
            }
            else
            {
                ViewBag.Entrenadores =
                    new SelectList(
                        new List<EntrenadorListaModel>(),
                        "IdEntrenador",
                        "NombreCompleto");

                ViewBag.MensajeError =
                    responseEntrenadores.Content
                        .ReadAsStringAsync()
                        .Result;
            }

            var model =
                new EntrenadorAsignarModel
                {
                    IdCliente =
                        cliente.IdUsuario,

                    NombreCliente =
                        cliente.Nombre
                        + " "
                        + cliente.Apellido
                };

            return View(model);
        }

        [HttpPost]
        public IActionResult Clientes_AsignarEntrenador(
            EntrenadorAsignarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteAsignarEntrenadorAPI";

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
                    "Clientes_Ver",
                    "Admin",
                    new
                    {
                        i = model.IdCliente
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                var urlEntrenadores =
                    _config["ConnectionStringsWeb:urlApi"]
                    + "/api/Admin/ListaEntrenadoresActivosAPI";

                var responseEntrenadores =
                    client.GetAsync(urlEntrenadores).Result;

                if (responseEntrenadores.StatusCode ==
                    HttpStatusCode.OK)
                {
                    var entrenadores =
                        responseEntrenadores.Content
                            .ReadFromJsonAsync
                            <List<EntrenadorListaModel>>()
                            .Result;

                    ViewBag.Entrenadores =
                        new SelectList(
                            entrenadores ??
                            new List<EntrenadorListaModel>(),
                            "IdEntrenador",
                            "NombreCompleto",
                            model.IdEntrenador);
                }
                else
                {
                    ViewBag.Entrenadores =
                        new SelectList(
                            new List<EntrenadorListaModel>(),
                            "IdEntrenador",
                            "NombreCompleto");
                }

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

        #endregion

        #region Planes

        [HttpGet]
        public IActionResult Planes_Lista()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaPlanesAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<PlanesMLista>>()
                        .Result;

                return View(
                    datos ?? new List<PlanesMLista>());
            }
            else if (response.StatusCode ==
                     HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<PlanesMLista>());
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            throw new Exception(
                "Ocurrió un error al consultar los planes de membresía.");
        }


        [HttpGet]
        public IActionResult Planes_Ver(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/PlanDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync<PlanModel>()
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
                    "Planes_Lista",
                    "Admin");
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
                $"La API respondió {(int)response.StatusCode} " +
                $"({response.StatusCode}): {detalleError}");
        }


        [HttpGet]
        public IActionResult Planes_Modificar(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/PlanDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync<PlanModel>()
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
                    "Planes_Lista",
                    "Admin");
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
                $"La API respondió {(int)response.StatusCode} " +
                $"({response.StatusCode}): {detalleError}");
        }
        [HttpPost]
        public IActionResult Planes_Modificar(
            PlanModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/PlanEditarAPI";

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
                    "Planes_Ver",
                    "Admin",
                    new
                    {
                        i = model.IdPlan
                    });
            }
            else if (response.StatusCode ==
                     HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(model);
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            throw new Exception(
                "Ocurrió un error al actualizar el plan de membresía.");
        }

        #endregion

        #region Horarios

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
                + "/api/Admin/HorariosListaAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <List<ClaseProgramadaListaModel>>()
                        .Result;

                return View(
                    datos ??
                    new List<ClaseProgramadaListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<ClaseProgramadaListaModel>());
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
        public IActionResult Horarios_Agregar()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            CargarListasHorario(client);

            var fechaInicio =
                DateTime.Now
                    .AddDays(1)
                    .Date
                    .AddHours(17);

            var model =
                new ClaseProgramadaCrearModel
                {
                    FechaHoraInicio =
                        fechaInicio,

                    FechaHoraFin =
                        fechaInicio.AddHours(1),

                    CupoMaximo =
                        20
                };

            return View(model);
        }


        [HttpPost]
        public IActionResult Horarios_Agregar(
            ClaseProgramadaCrearModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/HorarioRegistrarAPI";

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
                    "Horarios_Lista",
                    "Admin");
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                CargarListasHorario(
                    client,
                    model.IdClase,
                    model.IdEntrenador);

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
        public IActionResult Horarios_Ver(int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la clase programada no es válido.";

                return RedirectToAction(
                    "Horarios_Lista",
                    "Admin");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/HorarioDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync
                        <ClaseProgramadaDetalleModel>()
                        .Result;

                if (datos == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la clase programada.";

                    return RedirectToAction(
                        "Horarios_Lista",
                        "Admin");
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
                    "Admin");
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
        public IActionResult Horarios_Editar(int i)
        {
            if (i <= 0)
            {
                TempData["MensajeError"] =
                    "El identificador de la clase programada no es válido.";

                return RedirectToAction(
                    "Horarios_Lista",
                    "Admin");
            }

            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/HorarioDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var horario =
                    response.Content
                        .ReadFromJsonAsync
                        <ClaseProgramadaDetalleModel>()
                        .Result;

                if (horario == null)
                {
                    TempData["MensajeError"] =
                        "No se pudo consultar la clase programada.";

                    return RedirectToAction(
                        "Horarios_Lista",
                        "Admin");
                }

                var model =
                    new ClaseProgramadaEditarModel
                    {
                        IdClaseProg =
                            horario.IdClaseProg,

                        IdEntrenador =
                            horario.IdEntrenador,

                        IdClase =
                            horario.IdClase,

                        FechaHoraInicio =
                            horario.FechaHoraInicio,

                        FechaHoraFin =
                            horario.FechaHoraFin,

                        CupoMaximo =
                            horario.CupoMaximo
                    };

                CargarListasHorario(
                    client,
                    model.IdClase,
                    model.IdEntrenador);

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
                    "Horarios_Lista",
                    "Admin");
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
        public IActionResult Horarios_Editar(
            ClaseProgramadaEditarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/HorarioEditarAPI";

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
                    "Horarios_Ver",
                    "Admin",
                    new
                    {
                        i = model.IdClaseProg
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                CargarListasHorario(
                    client,
                    model.IdClase,
                    model.IdEntrenador);

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
        public IActionResult Horarios_CambiarEstado(
            ClaseProgramadaEstadoModel model)
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
                + "/api/Admin/HorarioCambiarEstadoAPI";

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
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }
            else if (response.StatusCode ==
                     HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }
            else
            {
                TempData["MensajeError"] =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;
            }

            return RedirectToAction(
                "Horarios_Ver",
                "Admin",
                new
                {
                    i = model.IdClaseProg
                });
        }

        private void CargarListasHorario(
            HttpClient client,
            int? idClaseSeleccionada = null,
            int? idEntrenadorSeleccionado = null)
        {
            var urlClases =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClasesListaAPI";

            var responseClases =
                client.GetAsync(urlClases).Result;

            if (responseClases.StatusCode ==
                HttpStatusCode.OK)
            {
                var clases =
                    responseClases.Content
                        .ReadFromJsonAsync
                        <List<ClaseListaModel>>()
                        .Result;

                ViewBag.Clases =
                    new SelectList(
                        clases ??
                        new List<ClaseListaModel>(),
                        "IdClase",
                        "NombreClase",
                        idClaseSeleccionada);
            }
            else
            {
                ViewBag.Clases =
                    new SelectList(
                        new List<ClaseListaModel>(),
                        "IdClase",
                        "NombreClase");
            }

            var urlEntrenadores =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ListaEntrenadoresActivosAPI";

            var responseEntrenadores =
                client.GetAsync(urlEntrenadores).Result;

            if (responseEntrenadores.StatusCode ==
                HttpStatusCode.OK)
            {
                var entrenadores =
                    responseEntrenadores.Content
                        .ReadFromJsonAsync
                        <List<EntrenadorListaModel>>()
                        .Result;

                ViewBag.Entrenadores =
                    new SelectList(
                        entrenadores ??
                        new List<EntrenadorListaModel>(),
                        "IdEntrenador",
                        "NombreCompleto",
                        idEntrenadorSeleccionado);
            }
            else
            {
                ViewBag.Entrenadores =
                    new SelectList(
                        new List<EntrenadorListaModel>(),
                        "IdEntrenador",
                        "NombreCompleto");
            }
        }

        #endregion

        #region Dashboard

        [HttpGet]
        public IActionResult Dashboard()
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/DashboardAPI";

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync<DashboardModel>()
                        .Result;

                if (datos == null)
                {
                    ViewBag.MensajeError =
                        "No se pudo cargar la información del dashboard.";

                    return View(
                        new DashboardModel());
                }

                datos.PagosRecientes ??=
                    new List<DashboardPagoRecienteModel>();

                datos.ProximasClases ??=
                    new List<DashboardProximaClaseModel>();

                datos.Indicadores ??=
                    new DashboardIndicadoresModel();

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
                    new DashboardModel());
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

        #region Pagos

        [HttpGet]
        public IActionResult Pagos_Registrar(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlCliente =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/ClienteDetallesAPI/"
                + i;

            var responseCliente =
                client.GetAsync(urlCliente).Result;

            if (responseCliente.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }

            if (responseCliente.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }

            if (responseCliente.StatusCode ==
                HttpStatusCode.NotFound)
            {
                TempData["MensajeError"] =
                    responseCliente.Content
                        .ReadAsStringAsync()
                        .Result;

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }

            if (responseCliente.StatusCode !=
                HttpStatusCode.OK)
            {
                var detalleError =
                    responseCliente.Content
                        .ReadAsStringAsync()
                        .Result;

                throw new Exception(
                    $"La API respondió " +
                    $"{(int)responseCliente.StatusCode} " +
                    $"({responseCliente.StatusCode}): " +
                    detalleError);
            }

            var cliente =
                responseCliente.Content
                    .ReadFromJsonAsync<ClienteModel>()
                    .Result;

            if (cliente == null)
            {
                TempData["MensajeError"] =
                    "No se pudo consultar la información del cliente.";

                return RedirectToAction(
                    "Clientes_Lista",
                    "Admin");
            }

            if (!cliente.IdMembresia.HasValue)
            {
                TempData["MensajeError"] =
                    "El cliente no tiene una membresía asignada.";

                return RedirectToAction(
                    "Clientes_Ver",
                    "Admin",
                    new
                    {
                        i = cliente.IdUsuario
                    });
            }

            if (!string.Equals(
                cliente.EstadoMembresia,
                "Activa",
                StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeError"] =
                    "Solo se puede registrar el pago de una membresía activa.";

                return RedirectToAction(
                    "Clientes_Ver",
                    "Admin",
                    new
                    {
                        i = cliente.IdUsuario
                    });
            }

            /*
             * Verificar que la membresía actual
             * todavía no tenga un pago.
             */
            var urlPagos =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/PagosClienteAPI/"
                + cliente.IdUsuario;

            var responsePagos =
                client.GetAsync(urlPagos).Result;

            if (responsePagos.StatusCode ==
                HttpStatusCode.OK)
            {
                var pagos =
                    responsePagos.Content
                        .ReadFromJsonAsync<List<PagoListaModel>>()
                        .Result;

                var pagoExistente =
                    pagos?.Any(x =>
                        x.IdMembresia ==
                        cliente.IdMembresia.Value)
                    ?? false;

                if (pagoExistente)
                {
                    TempData["MensajeError"] =
                        "La membresía activa ya tiene un pago registrado.";

                    return RedirectToAction(
                        "Clientes_Ver",
                        "Admin",
                        new
                        {
                            i = cliente.IdUsuario
                        });
                }
            }
            else if (responsePagos.StatusCode ==
                     HttpStatusCode.Unauthorized)
            {
                return RedirectToAction(
                    "Logout",
                    "Home");
            }
            else if (responsePagos.StatusCode ==
                     HttpStatusCode.Forbidden)
            {
                return RedirectToAction(
                    "Principal",
                    "Home");
            }
            else if (responsePagos.StatusCode !=
                     HttpStatusCode.NotFound)
            {
                var detalleError =
                    responsePagos.Content
                        .ReadAsStringAsync()
                        .Result;

                throw new Exception(
                    $"La API respondió " +
                    $"{(int)responsePagos.StatusCode} " +
                    $"({responsePagos.StatusCode}): " +
                    detalleError);
            }

            var subtotal =
                cliente.Precio ?? 0;

            var impuesto =
                Math.Round(
                    subtotal * 0.13m,
                    2);

            var model =
                new PagoRegistrarModel
                {
                    IdUsuario =
                        cliente.IdUsuario,

                    IdMembresia =
                        cliente.IdMembresia.Value,

                    NombreCliente =
                        cliente.Nombre
                        + " "
                        + cliente.Apellido,

                    NombrePlan =
                        cliente.NombrePlan
                        ?? string.Empty,

                    Subtotal =
                        subtotal,

                    Impuesto =
                        impuesto,

                    Total =
                        subtotal + impuesto
                };

            ViewBag.MetodosPago =
                new SelectList(
                    new List<string>
                    {
                "Efectivo",
                "Tarjeta",
                "SINPE Móvil",
                "Transferencia"
                    });

            return View(model);
        }


        [HttpPost]
        public IActionResult Pagos_Registrar(
            PagoRegistrarModel model)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/PagoRegistrarAPI";

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
                    "Clientes_Pagos",
                    "Admin",
                    new
                    {
                        i = model.IdUsuario
                    });
            }

            if (response.StatusCode ==
                HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                ViewBag.MetodosPago =
                    new SelectList(
                        new List<string>
                        {
                    "Efectivo",
                    "Tarjeta",
                    "SINPE Móvil",
                    "Transferencia"
                        },
                        model.MetodoPago);

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
        public IActionResult Clientes_Pagos(int i)
        {
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/PagosClienteAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            ViewBag.IdUsuario = i;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync<List<PagoListaModel>>()
                        .Result;

                return View(
                    datos ?? new List<PagoListaModel>());
            }

            if (response.StatusCode ==
                HttpStatusCode.NotFound)
            {
                ViewBag.MensajeError =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

                return View(
                    new List<PagoListaModel>());
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
            using var client =
                _http.CreateClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                "Bearer " +
                HttpContext.Session.GetString("Token"));

            var urlApi =
                _config["ConnectionStringsWeb:urlApi"]
                + "/api/Admin/PagoDetalleAPI/"
                + i;

            var response =
                client.GetAsync(urlApi).Result;

            if (response.StatusCode ==
                HttpStatusCode.OK)
            {
                var datos =
                    response.Content
                        .ReadFromJsonAsync<PagoDetalleModel>()
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
                    "Admin");
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
                    "Admin");
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
