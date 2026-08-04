using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SuwoGym_API.Controllers
{
    [Authorize(Roles = "1")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(
      IConfiguration _databasekey,
      IUtilesService _utiles) : ControllerBase
    {
        #region Gestión de Empleados

        [HttpGet("ListaEmpleadosAPI")]
        public IActionResult ListaEmpleadosAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdUser",
                _utiles.ObtenerIdUsuarioToken());

            var response =
                context.Query<EmpleadoListaResponseModel>(
                    "spListarEmpleados",
                    parameters);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay empleados registrados.");
        }


        [HttpGet("ListaRolesAPI")]
        public IActionResult ListaRolesAPI()
        {
            using var context = new SqlConnection(
                _databasekey[
                    "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<RolesListaResponseModel>(
                    "spListarRoles");

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay roles disponibles.");
        }


        [HttpPost("EmpleadoAgregar")]
        public async Task<IActionResult> EmpleadoAgregar(
    EmpleadoCrearRequestModel model)
        {
            if (model.IdRol != 1 &&
                model.IdRol != 2)
            {
                return BadRequest(
                    "El rol seleccionado no es válido para un empleado.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var temporal =
                    _utiles.GenerarContrasena();

                var temporalHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        temporal);

                var parameters =
                    new DynamicParameters();

                parameters.Add("@Nombre", model.Nombre);
                parameters.Add("@Apellido", model.Apellido);
                parameters.Add(
                    "@Identificacion",
                    model.Identificacion);

                parameters.Add(
                    "@FechaNacimiento",
                    model.FechaNac);

                parameters.Add(
                    "@Telefono",
                    model.Telefono);

                parameters.Add(
                    "@CorreoElectronico",
                    model.CorreoElectronico);

                parameters.Add(
                    "@Rol",
                    model.IdRol);

                parameters.Add(
                    "@Direccion",
                    model.Direccion);

                parameters.Add(
                    "@ContrasennaHash",
                    temporalHash);

                parameters.Add(
                    "@FechaRegistro",
                    DateTime.Now);

                var response =
                    context.Execute(
                        "spRegistrarEmpleado",
                        parameters);

                if (response <= 0)
                {
                    return BadRequest(
                        "El empleado no se pudo registrar.");
                }

                string ruta =
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "Templates",
                        "BienvenidaEmpleado.html");

                string plantilla =
                    System.IO.File.ReadAllText(ruta);

                plantilla =
                    plantilla.Replace(
                        "{{NOMBRE}}",
                        model.Nombre);

                plantilla =
                    plantilla.Replace(
                        "{{APELLIDO}}",
                        model.Apellido);

                plantilla =
                    plantilla.Replace(
                        "{{TEMPORAL}}",
                        temporal);

                await _utiles.EnviarCorreoAsync(
                    model.CorreoElectronico,
                    "Nueva cuenta en SUWÓ",
                    plantilla);

                return Ok(
                    "El empleado se registró correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpGet("EmpleadoDetalles/{id}")]
        public IActionResult EmpleadoDetalles(int id)
        {
            using var context = new SqlConnection(
                _databasekey[
                    "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdUser",
                id);

            var response =
                context.QueryFirstOrDefault
                <EmpleadoDetalleResponseModel>(
                    "spDetalleEmpleado",
                    parameters);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "No se encontró la información del empleado.");
        }


        [HttpPut("EmpleadoEditar")]
        public IActionResult EmpleadoEditar(
    EmpleadoEditarRequestModel model)
        {
            var idUsuarioActual =
                _utiles.ObtenerIdUsuarioToken();

            if (model.IdUsuario ==
                idUsuarioActual)
            {
                return BadRequest(
                    "No puede modificar su propio rol o estado.");
            }

            if (model.IdRol != 1 &&
                model.IdRol != 2)
            {
                return BadRequest(
                    "El rol seleccionado no es válido para un empleado.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdUser",
                    model.IdUsuario);

                parameters.Add(
                    "@Estado",
                    model.Estado);

                parameters.Add(
                    "@Rol",
                    model.IdRol);

                parameters.Add(
                    "@IdUsuarioActual",
                    idUsuarioActual);

                var response =
                    context.Execute(
                        "spEditarEmpleado",
                        parameters);

                if (response > 0)
                {
                    return Ok(
                        "La información del empleado se actualizó correctamente.");
                }

                return BadRequest(
                    "No se pudo actualizar la información del empleado.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
        #endregion


        #region Gestión de Clientes

        [HttpGet("ListaClientesAPI")]
        public IActionResult ListaClientesAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<ClienteListaResponseModel>(
                    "spListarClientes");

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay clientes registrados.");
        }


        [HttpGet("ClienteDetallesAPI/{id}")]
        public IActionResult ClienteDetallesAPI(int id)
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdUser",
                id);

            var response =
                context.QueryFirstOrDefault
                <ClienteDetalleResponseModel>(
                    "spDetalleCliente",
                    parameters);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "No se encontró la información del cliente.");
        }


        [HttpPut("ClienteCambiarEstadoAPI")]
        public IActionResult ClienteCambiarEstadoAPI(
            ClienteCambiarEstadoRequestModel model)
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdUser",
                model.IdUsuario);

            parameters.Add(
                "@Estado",
                model.Estado);

            var response =
                context.Execute(
                    "spEditarEstadoCliente",
                    parameters);

            if (response > 0)
            {
                return Ok(
                    "El estado del cliente se actualizó correctamente.");
            }

            return BadRequest(
                "No se pudo actualizar el estado del cliente.");
        }

        [HttpPut("ClienteEditarAPI")]
        public IActionResult ClienteEditarAPI(
    ClienteEditarRequestModel model)
        {
            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdUser",
                    model.IdUsuario);

                parameters.Add(
                    "@Nombre",
                    model.Nombre);

                parameters.Add(
                    "@Apellido",
                    model.Apellido);

                parameters.Add(
                    "@FechaNacimiento",
                    model.FechaNac);

                parameters.Add(
                    "@Telefono",
                    model.Telefono);

                parameters.Add(
                    "@CorreoElectronico",
                    model.CorreoElectronico);

                parameters.Add(
                    "@Direccion",
                    model.Direccion);

                var response =
                    context.Execute(
                        "spEditarCliente",
                        parameters);

                if (response > 0)
                {
                    return Ok(
                        "Los datos del cliente se actualizaron correctamente.");
                }

                return BadRequest(
                    "No se pudo actualizar la información del cliente.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }

        }
        [HttpPost("ClienteAsignarMembresiaAPI")]
        public IActionResult ClienteAsignarMembresiaAPI(
    MembresiaAsignarRequestModel model)
        {
            if (model.IdUsuario <= 0)
            {
                return BadRequest(
                    "El identificador del cliente no es válido.");
            }

            if (model.IdPlan <= 0)
            {
                return BadRequest(
                    "Debe seleccionar un plan de membresía.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdUsuario",
                    model.IdUsuario);

                parameters.Add(
                    "@IdPlan",
                    model.IdPlan);

                var response =
                    context.Execute(
                        "spAsignarMembresiaCliente",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La membresía se asignó correctamente.");
                }

                return BadRequest(
                    "No se pudo asignar la membresía al cliente.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
        [HttpPut("ClienteCancelarMembresiaAPI")]
        public IActionResult ClienteCancelarMembresiaAPI(
    MembresiaCancelarRequestModel model)
        {
            if (model.IdMembresia <= 0)
            {
                return BadRequest(
                    "El identificador de la membresía no es válido.");
            }

            if (model.IdUsuario <= 0)
            {
                return BadRequest(
                    "El identificador del cliente no es válido.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdMembresia",
                    model.IdMembresia);

                parameters.Add(
                    "@IdUsuario",
                    model.IdUsuario);

                var response =
                    context.Execute(
                        "spCancelarMembresiaCliente",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La membresía se canceló correctamente.");
                }

                return BadRequest(
                    "No se pudo cancelar la membresía.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
        [HttpGet("ListaEntrenadoresActivosAPI")]
        public IActionResult ListaEntrenadoresActivosAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<EntrenadorListaResponseModel>(
                    "spListarEntrenadoresActivos",
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay entrenadores activos disponibles.");
        }
        [HttpPost("ClienteAsignarEntrenadorAPI")]
        public IActionResult ClienteAsignarEntrenadorAPI(
    EntrenadorAsignarRequestModel model)
        {
            if (model.IdCliente <= 0)
            {
                return BadRequest(
                    "El identificador del cliente no es válido.");
            }

            if (model.IdEntrenador <= 0)
            {
                return BadRequest(
                    "Debe seleccionar un entrenador.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdCliente",
                    model.IdCliente);

                parameters.Add(
                    "@IdEntrenador",
                    model.IdEntrenador);

                var response =
                    context.Execute(
                        "spAsignarEntrenadorCliente",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "El entrenador se asignó correctamente.");
                }

                return BadRequest(
                    "No se pudo asignar el entrenador al cliente.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
        #endregion




        #region Gestión de Planes de Membresía

        [HttpGet("ListaPlanesAPI")]
        public IActionResult ListaPlanesAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<PlanesMListaResponseModel>(
                    "spListarPlanesMembresia",
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay planes de membresía registrados.");
        }


        [HttpGet("PlanDetalleAPI/{id}")]
        public IActionResult PlanDetalleAPI(int id)
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdPlan",
                id);

            var response =
                context.QueryFirstOrDefault<PlanDetalleResponseModel>(
                    "spDetallePlanMembresia",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "No se encontró el plan de membresía.");
        }


        [HttpPut("PlanEditarAPI")]
        public IActionResult PlanEditarAPI(
            PlanEditarRequestModel model)
        {
            if (model.IdPlan <= 0)
            {
                return BadRequest(
                    "El identificador del plan no es válido.");
            }

            if (string.IsNullOrWhiteSpace(
                    model.NombrePlan))
            {
                return BadRequest(
                    "El nombre del plan es obligatorio.");
            }

            if (model.DuracionDias <= 0)
            {
                return BadRequest(
                    "La duración debe ser mayor que cero.");
            }

            if (model.Precio <= 0)
            {
                return BadRequest(
                    "El precio debe ser mayor que cero.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdPlan",
                    model.IdPlan);

                parameters.Add(
                    "@NombrePlan",
                    model.NombrePlan);

                parameters.Add(
                    "@Descripcion",
                    model.Descripcion);

                parameters.Add(
                    "@DuracionDias",
                    model.DuracionDias);

                parameters.Add(
                    "@Precio",
                    model.Precio);

                var response =
                    context.Execute(
                        "spEditarPlanMembresia",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "El plan de membresía se actualizó correctamente.");
                }

                return BadRequest(
                    "No se pudo actualizar el plan de membresía.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        #endregion
        #region Gestión de Pagos

        [HttpPost("PagoRegistrarAPI")]
        public IActionResult PagoRegistrarAPI(
            PagoRegistrarRequestModel model)
        {
            if (model.IdMembresia <= 0)
            {
                return BadRequest(
                    "El identificador de la membresía no es válido.");
            }

            if (string.IsNullOrWhiteSpace(
                model.MetodoPago))
            {
                return BadRequest(
                    "Debe seleccionar un método de pago.");
            }

            if (model.MetodoPago != "Efectivo"
                &&
                string.IsNullOrWhiteSpace(
                    model.NumeroReferencia))
            {
                return BadRequest(
                    "El número de referencia es obligatorio.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdMembresia",
                    model.IdMembresia);

                parameters.Add(
                    "@MetodoPago",
                    model.MetodoPago);

                parameters.Add(
                    "@NumeroReferencia",
                    model.NumeroReferencia);

                var response =
                    context.Execute(
                        "spRegistrarPagoMembresia",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "El pago se registró correctamente.");
                }

                return BadRequest(
                    "No se pudo registrar el pago.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpGet("PagosClienteAPI/{idUsuario}")]
        public IActionResult PagosClienteAPI(
            int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(
                    "El identificador del cliente no es válido.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdUsuario",
                idUsuario);

            var response =
                context.Query<PagoListaResponseModel>(
                    "spListarPagosCliente",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "El cliente no tiene pagos registrados.");
        }


        [HttpGet("PagoDetalleAPI/{idPago}")]
        public IActionResult PagoDetalleAPI(
            int idPago)
        {
            if (idPago <= 0)
            {
                return BadRequest(
                    "El identificador del pago no es válido.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdPago",
                idPago);

            var response =
                context.QueryFirstOrDefault
                <PagoDetalleResponseModel>(
                    "spDetallePago",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "No se encontró el pago indicado.");
        }

        #endregion
        #region Gestión de Horarios y Clases

        [HttpGet("ClasesListaAPI")]
        public IActionResult ClasesListaAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<ClaseListaResponseModel>(
                    "spListarClases",
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay clases registradas.");
        }


        [HttpGet("HorariosListaAPI")]
        public IActionResult HorariosListaAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query
                <ClaseProgramadaListaResponseModel>(
                    "spListarClasesProgramadas",
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay clases programadas.");
        }


        [HttpGet("HorarioDetalleAPI/{idClaseProg}")]
        public IActionResult HorarioDetalleAPI(
            int idClaseProg)
        {
            if (idClaseProg <= 0)
            {
                return BadRequest(
                    "El identificador de la clase programada no es válido.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdClaseProg",
                idClaseProg);

            var response =
                context.QueryFirstOrDefault
                <ClaseProgramadaDetalleResponseModel>(
                    "spDetalleClaseProgramada",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "La clase programada no existe.");
        }


        [HttpPost("HorarioRegistrarAPI")]
        public IActionResult HorarioRegistrarAPI(
            ClaseProgramadaCrearRequestModel model)
        {
            if (model.IdEntrenador <= 0)
            {
                return BadRequest(
                    "Debe seleccionar un entrenador válido.");
            }

            if (model.IdClase <= 0)
            {
                return BadRequest(
                    "Debe seleccionar una clase válida.");
            }

            if (model.FechaHoraInicio ==
                default)
            {
                return BadRequest(
                    "La fecha y hora de inicio son obligatorias.");
            }

            if (model.FechaHoraFin ==
                default)
            {
                return BadRequest(
                    "La fecha y hora de finalización son obligatorias.");
            }

            if (model.FechaHoraInicio <=
                DateTime.Now)
            {
                return BadRequest(
                    "La fecha de inicio debe ser posterior a la fecha actual.");
            }

            if (model.FechaHoraFin <=
                model.FechaHoraInicio)
            {
                return BadRequest(
                    "La hora de finalización debe ser posterior a la hora de inicio.");
            }

            if (model.CupoMaximo <= 0)
            {
                return BadRequest(
                    "El cupo máximo debe ser mayor que cero.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdEntrenador",
                    model.IdEntrenador);

                parameters.Add(
                    "@IdClase",
                    model.IdClase);

                parameters.Add(
                    "@FechaHoraInicio",
                    model.FechaHoraInicio);

                parameters.Add(
                    "@FechaHoraFin",
                    model.FechaHoraFin);

                parameters.Add(
                    "@CupoMaximo",
                    model.CupoMaximo);

                var response =
                    context.Execute(
                        "spRegistrarClaseProgramada",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La clase se programó correctamente.");
                }

                return BadRequest(
                    "No se pudo programar la clase.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpPut("HorarioEditarAPI")]
        public IActionResult HorarioEditarAPI(
            ClaseProgramadaEditarRequestModel model)
        {
            if (model.IdClaseProg <= 0)
            {
                return BadRequest(
                    "El identificador de la clase programada no es válido.");
            }

            if (model.IdEntrenador <= 0)
            {
                return BadRequest(
                    "Debe seleccionar un entrenador válido.");
            }

            if (model.IdClase <= 0)
            {
                return BadRequest(
                    "Debe seleccionar una clase válida.");
            }

            if (model.FechaHoraInicio ==
                default)
            {
                return BadRequest(
                    "La fecha y hora de inicio son obligatorias.");
            }

            if (model.FechaHoraFin ==
                default)
            {
                return BadRequest(
                    "La fecha y hora de finalización son obligatorias.");
            }

            if (model.FechaHoraFin <=
                model.FechaHoraInicio)
            {
                return BadRequest(
                    "La hora de finalización debe ser posterior a la hora de inicio.");
            }

            if (model.CupoMaximo <= 0)
            {
                return BadRequest(
                    "El cupo máximo debe ser mayor que cero.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdClaseProg",
                    model.IdClaseProg);

                parameters.Add(
                    "@IdEntrenador",
                    model.IdEntrenador);

                parameters.Add(
                    "@IdClase",
                    model.IdClase);

                parameters.Add(
                    "@FechaHoraInicio",
                    model.FechaHoraInicio);

                parameters.Add(
                    "@FechaHoraFin",
                    model.FechaHoraFin);

                parameters.Add(
                    "@CupoMaximo",
                    model.CupoMaximo);

                var response =
                    context.Execute(
                        "spEditarClaseProgramada",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La clase programada se actualizó correctamente.");
                }

                return BadRequest(
                    "No se pudo actualizar la clase programada.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpPut("HorarioCambiarEstadoAPI")]
        public IActionResult HorarioCambiarEstadoAPI(
            ClaseProgramadaEstadoRequestModel model)
        {
            if (model.IdClaseProg <= 0)
            {
                return BadRequest(
                    "El identificador de la clase programada no es válido.");
            }

            try
            {
                using var context =
                    new SqlConnection(
                        _databasekey[
                            "ConnectionStringsApi:DefaultConnection"]);

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@IdClaseProg",
                    model.IdClaseProg);

                parameters.Add(
                    "@Estado",
                    model.Estado);

                var response =
                    context.Execute(
                        "spCambiarEstadoClaseProgramada",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        model.Estado
                            ? "La clase programada se activó correctamente."
                            : "La clase programada se desactivó correctamente.");
                }

                return BadRequest(
                    "No se pudo cambiar el estado de la clase programada.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        #endregion
        #region Dashboard Administrativo

        [HttpGet("DashboardAPI")]
        public IActionResult DashboardAPI()
        {
            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);


            /*
             * Consultar los indicadores generales.
             */

            var indicadores =
                context.QueryFirstOrDefault
                <DashboardIndicadoresResponseModel>(
                    "spDashboardAdministrador",
                    commandType:
                        CommandType.StoredProcedure);


            if (indicadores == null)
            {
                return NotFound(
                    "No se pudieron consultar los indicadores del dashboard.");
            }


            /*
             * Consultar los cinco pagos más recientes.
             */

            var pagosRecientes =
                context.Query
                <DashboardPagoRecienteResponseModel>(
                    "spDashboardPagosRecientes",
                    commandType:
                        CommandType.StoredProcedure)
                .ToList();


            /*
             * Consultar las cinco próximas clases.
             */

            var proximasClases =
                context.Query
                <DashboardProximaClaseResponseModel>(
                    "spDashboardProximasClases",
                    commandType:
                        CommandType.StoredProcedure)
                .ToList();


            var response =
                new DashboardResponseModel
                {
                    Indicadores =
                        indicadores,

                    PagosRecientes =
                        pagosRecientes,

                    ProximasClases =
                        proximasClases
                };


            return Ok(response);
        }

        #endregion
    }
}