using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using System.Data;

namespace SuwoGym_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "3")]
    public class MiembroController : ControllerBase
    {
        private readonly IConfiguration _databasekey;
        private readonly IUtilesService _utiles;


        public MiembroController(
            IConfiguration databasekey,
            IUtilesService utiles)
        {
            _databasekey = databasekey;
            _utiles = utiles;
        }


        #region Perfil del Miembro

        [HttpGet("PerfilAPI")]
        public IActionResult PerfilAPI()
        {
            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            var response =
                context.QueryFirstOrDefault
                <MiembroPerfilResponseModel>(
                    "spDetalleMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "No se encontró la información del miembro.");
        }

        #endregion


        #region Rutinas del Miembro

        [HttpGet("RutinasListaAPI")]
        public IActionResult RutinasListaAPI()
        {
            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            var response =
                context.Query
                <RutinaMiembroListaResponseModel>(
                    "spListarRutinasMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene rutinas activas asignadas.");
        }


        [HttpGet("RutinaDetalleAPI/{idRutina}")]
        public IActionResult RutinaDetalleAPI(
            int idRutina)
        {
            if (idRutina <= 0)
            {
                return BadRequest(
                    "El identificador de la rutina no es válido.");
            }

            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            parameters.Add(
                "@IdRutina",
                idRutina);

            var response =
                context.QueryFirstOrDefault
                <RutinaMiembroDetalleResponseModel>(
                    "spDetalleRutinaMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "La rutina no existe, está inactiva o no pertenece a este miembro.");
        }


        [HttpGet("RutinaEjerciciosAPI/{idRutina}")]
        public IActionResult RutinaEjerciciosAPI(
            int idRutina)
        {
            if (idRutina <= 0)
            {
                return BadRequest(
                    "El identificador de la rutina no es válido.");
            }

            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            /*
             * Primero se comprueba que la rutina exista,
             * esté activa y pertenezca al miembro autenticado.
             */

            var parametersRutina =
                new DynamicParameters();

            parametersRutina.Add(
                "@IdMiembro",
                idMiembro);

            parametersRutina.Add(
                "@IdRutina",
                idRutina);

            var rutina =
                context.QueryFirstOrDefault
                <RutinaMiembroDetalleResponseModel>(
                    "spDetalleRutinaMiembro",
                    parametersRutina,
                    commandType:
                        CommandType.StoredProcedure);

            if (rutina == null)
            {
                return NotFound(
                    "La rutina no existe, está inactiva o no pertenece a este miembro.");
            }


            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            parameters.Add(
                "@IdRutina",
                idRutina);

            var response =
                context.Query
                <RutinaEjercicioMiembroResponseModel>(
                    "spListarEjerciciosRutinaMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "La rutina no tiene ejercicios activos asignados.");
        }

        #endregion
        #region Pagos del Miembro

        [HttpGet("PagosListaAPI")]
        public IActionResult PagosListaAPI()
        {
            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            var response =
                context.Query
                <PagoMiembroListaResponseModel>(
                    "spListarPagosMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene pagos registrados.");
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

            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            parameters.Add(
                "@IdPago",
                idPago);

            var response =
                context.QueryFirstOrDefault
                <PagoMiembroDetalleResponseModel>(
                    "spDetallePagoMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "El pago no existe o no pertenece a este miembro.");
        }

        #endregion
        #region Reservas de Clases

        [HttpGet("ClasesDisponiblesAPI")]
        public IActionResult ClasesDisponiblesAPI()
        {
            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            var response =
                context.Query
                <ClaseDisponibleMiembroResponseModel>(
                    "spListarClasesDisponiblesMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No hay clases disponibles para reservar.");
        }


        [HttpGet("ReservasListaAPI")]
        public IActionResult ReservasListaAPI()
        {
            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            var response =
                context.Query
                <ReservaClaseMiembroListaResponseModel>(
                    "spListarReservasMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene reservas registradas.");
        }


        [HttpPost("ReservaRegistrarAPI")]
        public IActionResult ReservaRegistrarAPI(
            ReservaClaseMiembroRequestModel model)
        {
            if (model.IdClaseProg <= 0)
            {
                return BadRequest(
                    "Debe seleccionar una clase válida.");
            }

            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
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
                    "@IdMiembro",
                    idMiembro);

                parameters.Add(
                    "@IdClaseProg",
                    model.IdClaseProg);

                var response =
                    context.Execute(
                        "spReservarClaseMiembro",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La clase se reservó correctamente.");
                }

                return BadRequest(
                    "No se pudo registrar la reserva.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpPut("ReservaCancelarAPI")]
        public IActionResult ReservaCancelarAPI(
            ReservaClaseCancelarRequestModel model)
        {
            if (model.IdReserva <= 0)
            {
                return BadRequest(
                    "El identificador de la reserva no es válido.");
            }

            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
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
                    "@IdMiembro",
                    idMiembro);

                parameters.Add(
                    "@IdReserva",
                    model.IdReserva);

                var response =
                    context.Execute(
                        "spCancelarReservaClaseMiembro",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La reserva se canceló correctamente.");
                }

                return BadRequest(
                    "No se pudo cancelar la reserva.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        #endregion
        #region Historial de Asistencia

        [HttpGet("HistorialAsistenciasAPI")]
        public IActionResult HistorialAsistenciasAPI()
        {
            var idMiembro =
                _utiles.ObtenerIdUsuarioToken();

            if (idMiembro <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al miembro.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdMiembro",
                idMiembro);

            var response =
                context.Query
                <HistorialAsistenciaMiembroResponseModel>(
                    "spListarHistorialAsistenciaMiembro",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene registros de asistencia.");
        }

        #endregion
    }
}