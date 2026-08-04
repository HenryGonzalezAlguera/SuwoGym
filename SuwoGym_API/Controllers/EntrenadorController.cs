using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;
using System.Data;

namespace SuwoGym_API.Controllers
{
    [Authorize(Roles = "2")]
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenadorController(
        IConfiguration _databasekey,
        IUtilesService _utiles) : ControllerBase
    {
        #region Clientes asignados

        [HttpGet("ClientesAsignadosAPI")]
        public IActionResult ClientesAsignadosAPI()
        {
            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            var response =
                context.Query<EntrenadorClienteResponseModel>(
                    "spListarClientesEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene clientes asignados.");
        }
        [HttpGet("ClienteDetalleAPI/{idCliente}")]
        public IActionResult ClienteDetalleAPI(
    int idCliente)
        {
            if (idCliente <= 0)
            {
                return BadRequest(
                    "El identificador del cliente no es válido.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            parameters.Add(
                "@IdCliente",
                idCliente);

            var response =
                context.QueryFirstOrDefault
                <EntrenadorClienteDetalleResponseModel>(
                    "spDetalleClienteEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "El cliente no está asignado a este entrenador.");
        }
        #endregion

        #region Gestión de Rutinas

        [HttpGet("RutinasListaAPI")]
        public IActionResult RutinasListaAPI()
        {
            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            var response =
                context.Query<RutinaListaResponseModel>(
                    "spListarRutinasEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene rutinas registradas.");
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

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            parameters.Add(
                "@IdRutina",
                idRutina);

            var response =
                context.QueryFirstOrDefault
                <RutinaDetalleResponseModel>(
                    "spDetalleRutinaEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "La rutina no existe o no pertenece a este entrenador.");
        }


        [HttpPost("RutinaRegistrarAPI")]
        public IActionResult RutinaRegistrarAPI(
            RutinaCrearRequestModel model)
        {
            if (model.IdCliente <= 0)
            {
                return BadRequest(
                    "El identificador del cliente no es válido.");
            }

            if (string.IsNullOrWhiteSpace(
                model.NombreRutina))
            {
                return BadRequest(
                    "El nombre de la rutina es obligatorio.");
            }

            if (model.Nivel != "Principiante"
                &&
                model.Nivel != "Intermedio"
                &&
                model.Nivel != "Avanzado")
            {
                return BadRequest(
                    "El nivel seleccionado no es válido.");
            }

            if (string.IsNullOrWhiteSpace(
                model.Detalles))
            {
                return BadRequest(
                    "Los detalles de la rutina son obligatorios.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@IdCliente",
                    model.IdCliente);

                parameters.Add(
                    "@NombreRutina",
                    model.NombreRutina);

                parameters.Add(
                    "@Nivel",
                    model.Nivel);

                parameters.Add(
                    "@Detalles",
                    model.Detalles);

                var response =
                    context.Execute(
                        "spRegistrarRutinaEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La rutina se registró correctamente.");
                }

                return BadRequest(
                    "No se pudo registrar la rutina.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpPut("RutinaEditarAPI")]
        public IActionResult RutinaEditarAPI(
            RutinaEditarRequestModel model)
        {
            if (model.IdRutina <= 0)
            {
                return BadRequest(
                    "El identificador de la rutina no es válido.");
            }

            if (string.IsNullOrWhiteSpace(
                model.NombreRutina))
            {
                return BadRequest(
                    "El nombre de la rutina es obligatorio.");
            }

            if (model.Nivel != "Principiante"
                &&
                model.Nivel != "Intermedio"
                &&
                model.Nivel != "Avanzado")
            {
                return BadRequest(
                    "El nivel seleccionado no es válido.");
            }

            if (string.IsNullOrWhiteSpace(
                model.Detalles))
            {
                return BadRequest(
                    "Los detalles de la rutina son obligatorios.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@IdRutina",
                    model.IdRutina);

                parameters.Add(
                    "@NombreRutina",
                    model.NombreRutina);

                parameters.Add(
                    "@Nivel",
                    model.Nivel);

                parameters.Add(
                    "@Detalles",
                    model.Detalles);

                var response =
                    context.Execute(
                        "spEditarRutinaEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La rutina se actualizó correctamente.");
                }

                return BadRequest(
                    "No se pudo actualizar la rutina.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpPut("RutinaCambiarEstadoAPI")]
        public IActionResult RutinaCambiarEstadoAPI(
            RutinaEstadoRequestModel model)
        {
            if (model.IdRutina <= 0)
            {
                return BadRequest(
                    "El identificador de la rutina no es válido.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@IdRutina",
                    model.IdRutina);

                parameters.Add(
                    "@Estado",
                    model.Estado);

                var response =
                    context.Execute(
                        "spCambiarEstadoRutinaEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        model.Estado
                            ? "La rutina se activó correctamente."
                            : "La rutina se desactivó correctamente.");
                }

                return BadRequest(
                    "No se pudo cambiar el estado de la rutina.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        #endregion
        #region Gestión de Ejercicios

        [HttpGet("EjerciciosListaAPI")]
        public IActionResult EjerciciosListaAPI()
        {
            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            var response =
                context.Query<EjercicioListaResponseModel>(
                    "spListarEjerciciosEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene ejercicios registrados.");
        }


        [HttpPost("EjercicioRegistrarAPI")]
        public IActionResult EjercicioRegistrarAPI(
            EjercicioCrearRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(
                model.NombreEjercicio))
            {
                return BadRequest(
                    "El nombre del ejercicio es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(
                model.GrupoMuscular))
            {
                return BadRequest(
                    "El grupo muscular es obligatorio.");
            }

            if (model.NombreEjercicio.Length > 120)
            {
                return BadRequest(
                    "El nombre del ejercicio no puede superar los 120 caracteres.");
            }

            if (model.GrupoMuscular.Length > 80)
            {
                return BadRequest(
                    "El grupo muscular no puede superar los 80 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(
                    model.Descripcion)
                &&
                model.Descripcion.Length > 300)
            {
                return BadRequest(
                    "La descripción no puede superar los 300 caracteres.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@NombreEjercicio",
                    model.NombreEjercicio);

                parameters.Add(
                    "@GrupoMuscular",
                    model.GrupoMuscular);

                parameters.Add(
                    "@Descripcion",
                    model.Descripcion);

                var response =
                    context.Execute(
                        "spRegistrarEjercicioEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "El ejercicio se registró correctamente.");
                }

                return BadRequest(
                    "No se pudo registrar el ejercicio.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpGet("RutinaEjerciciosListaAPI/{idRutina}")]
        public IActionResult RutinaEjerciciosListaAPI(
            int idRutina)
        {
            if (idRutina <= 0)
            {
                return BadRequest(
                    "El identificador de la rutina no es válido.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            /*
             * Antes de listar los ejercicios se valida
             * que la rutina pertenezca al entrenador.
             */

            var parametersRutina =
                new DynamicParameters();

            parametersRutina.Add(
                "@IdEntrenador",
                idEntrenador);

            parametersRutina.Add(
                "@IdRutina",
                idRutina);

            var rutina =
                context.QueryFirstOrDefault
                <RutinaDetalleResponseModel>(
                    "spDetalleRutinaEntrenador",
                    parametersRutina,
                    commandType:
                        CommandType.StoredProcedure);

            if (rutina == null)
            {
                return NotFound(
                    "La rutina no existe o no pertenece a este entrenador.");
            }

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            parameters.Add(
                "@IdRutina",
                idRutina);

            var response =
                context.Query
                <RutinaEjercicioListaResponseModel>(
                    "spListarEjerciciosRutinaEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "La rutina no tiene ejercicios agregados.");
        }


        [HttpGet("RutinaEjercicioDetalleAPI/{idRutinaEjercicio}")]
        public IActionResult RutinaEjercicioDetalleAPI(
            int idRutinaEjercicio)
        {
            if (idRutinaEjercicio <= 0)
            {
                return BadRequest(
                    "El identificador del ejercicio asignado no es válido.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            parameters.Add(
                "@IdRutinaEjercicio",
                idRutinaEjercicio);

            var response =
                context.QueryFirstOrDefault
                <RutinaEjercicioDetalleResponseModel>(
                    "spDetalleEjercicioRutinaEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "El ejercicio asignado no existe o no pertenece a este entrenador.");
        }


        [HttpPost("RutinaEjercicioAgregarAPI")]
        public IActionResult RutinaEjercicioAgregarAPI(
            RutinaEjercicioAgregarRequestModel model)
        {
            if (model.IdRutina <= 0)
            {
                return BadRequest(
                    "El identificador de la rutina no es válido.");
            }

            if (model.IdEjercicio <= 0)
            {
                return BadRequest(
                    "El identificador del ejercicio no es válido.");
            }

            var diasValidos =
                new List<string>
                {
            "Lunes",
            "Martes",
            "Miércoles",
            "Jueves",
            "Viernes",
            "Sábado",
            "Domingo"
                };

            if (!diasValidos.Contains(
                model.DiaSemana))
            {
                return BadRequest(
                    "El día de la semana seleccionado no es válido.");
            }

            if (model.Series <= 0)
            {
                return BadRequest(
                    "La cantidad de series debe ser mayor que cero.");
            }

            if (model.Repeticiones <= 0)
            {
                return BadRequest(
                    "La cantidad de repeticiones debe ser mayor que cero.");
            }

            if (model.PesoSugerido.HasValue
                &&
                model.PesoSugerido.Value < 0)
            {
                return BadRequest(
                    "El peso sugerido no puede ser negativo.");
            }

            if (model.DescansoSegundos.HasValue
                &&
                model.DescansoSegundos.Value < 0)
            {
                return BadRequest(
                    "El tiempo de descanso no puede ser negativo.");
            }

            if (model.Orden <= 0)
            {
                return BadRequest(
                    "El orden debe ser mayor que cero.");
            }

            if (!string.IsNullOrWhiteSpace(
                    model.Observacion)
                &&
                model.Observacion.Length > 250)
            {
                return BadRequest(
                    "La observación no puede superar los 250 caracteres.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@IdRutina",
                    model.IdRutina);

                parameters.Add(
                    "@IdEjercicio",
                    model.IdEjercicio);

                parameters.Add(
                    "@DiaSemana",
                    model.DiaSemana);

                parameters.Add(
                    "@Series",
                    model.Series);

                parameters.Add(
                    "@Repeticiones",
                    model.Repeticiones);

                parameters.Add(
                    "@PesoSugerido",
                    model.PesoSugerido);

                parameters.Add(
                    "@DescansoSegundos",
                    model.DescansoSegundos);

                parameters.Add(
                    "@Observacion",
                    model.Observacion);

                parameters.Add(
                    "@Orden",
                    model.Orden);

                var response =
                    context.Execute(
                        "spAgregarEjercicioRutinaEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "El ejercicio se agregó correctamente a la rutina.");
                }

                return BadRequest(
                    "No se pudo agregar el ejercicio a la rutina.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }


        [HttpPut("RutinaEjercicioQuitarAPI")]
        public IActionResult RutinaEjercicioQuitarAPI(
            RutinaEjercicioQuitarRequestModel model)
        {
            if (model.IdRutinaEjercicio <= 0)
            {
                return BadRequest(
                    "El identificador del ejercicio asignado no es válido.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@IdRutinaEjercicio",
                    model.IdRutinaEjercicio);

                var response =
                    context.Execute(
                        "spQuitarEjercicioRutinaEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "El ejercicio se retiró correctamente de la rutina.");
                }

                return BadRequest(
                    "No se pudo retirar el ejercicio de la rutina.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
        [HttpPut("RutinaEjercicioEditarAPI")]
        public IActionResult RutinaEjercicioEditarAPI(
    RutinaEjercicioEditarRequestModel model)
        {
            if (model.IdRutinaEjercicio <= 0)
            {
                return BadRequest(
                    "El identificador del ejercicio asignado no es válido.");
            }

            var diasValidos =
                new List<string>
                {
            "Lunes",
            "Martes",
            "Miércoles",
            "Jueves",
            "Viernes",
            "Sábado",
            "Domingo"
                };

            if (!diasValidos.Contains(
                model.DiaSemana))
            {
                return BadRequest(
                    "El día de la semana seleccionado no es válido.");
            }

            if (model.Series <= 0)
            {
                return BadRequest(
                    "La cantidad de series debe ser mayor que cero.");
            }

            if (model.Repeticiones <= 0)
            {
                return BadRequest(
                    "La cantidad de repeticiones debe ser mayor que cero.");
            }

            if (model.PesoSugerido.HasValue
                &&
                model.PesoSugerido.Value < 0)
            {
                return BadRequest(
                    "El peso sugerido no puede ser negativo.");
            }

            if (model.DescansoSegundos.HasValue
                &&
                model.DescansoSegundos.Value < 0)
            {
                return BadRequest(
                    "El tiempo de descanso no puede ser negativo.");
            }

            if (model.Orden <= 0)
            {
                return BadRequest(
                    "El orden debe ser mayor que cero.");
            }

            if (!string.IsNullOrWhiteSpace(
                    model.Observacion)
                &&
                model.Observacion.Length > 250)
            {
                return BadRequest(
                    "La observación no puede superar los 250 caracteres.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@IdRutinaEjercicio",
                    model.IdRutinaEjercicio);

                parameters.Add(
                    "@DiaSemana",
                    model.DiaSemana);

                parameters.Add(
                    "@Series",
                    model.Series);

                parameters.Add(
                    "@Repeticiones",
                    model.Repeticiones);

                parameters.Add(
                    "@PesoSugerido",
                    model.PesoSugerido);

                parameters.Add(
                    "@DescansoSegundos",
                    model.DescansoSegundos);

                parameters.Add(
                    "@Observacion",
                    model.Observacion);

                parameters.Add(
                    "@Orden",
                    model.Orden);

                var response =
                    context.Execute(
                        "spEditarEjercicioRutinaEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "El ejercicio de la rutina se actualizó correctamente.");
                }

                return BadRequest(
                    "No se pudo actualizar el ejercicio de la rutina.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
        #endregion
        #region Horarios del Entrenador

        [HttpGet("HorariosListaAPI")]
        public IActionResult HorariosListaAPI()
        {
            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            var response =
                context.Query
                <ClaseEntrenadorListaResponseModel>(
                    "spListarClasesEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene clases programadas.");
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

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            parameters.Add(
                "@IdClaseProg",
                idClaseProg);

            var response =
                context.QueryFirstOrDefault
                <ClaseEntrenadorDetalleResponseModel>(
                    "spDetalleClaseEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound(
                "La clase programada no existe o no pertenece a este entrenador.");
        }

        #endregion
        #region Asistencia de Clases

        [HttpGet("AsistenciasListaAPI")]
        public IActionResult AsistenciasListaAPI()
        {
            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@IdEntrenador",
                idEntrenador);

            var response =
                context.Query
                <AsistenciaClaseResumenResponseModel>(
                    "spListarAsistenciaClaseEntrenador",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound(
                "No tiene clases disponibles para gestionar asistencia.");
        }


        [HttpGet("ParticipantesClaseAPI/{idClaseProg}")]
        public IActionResult ParticipantesClaseAPI(
            int idClaseProg)
        {
            if (idClaseProg <= 0)
            {
                return BadRequest(
                    "El identificador de la clase programada no es válido.");
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
            }

            /*
             * Primero verificamos que la clase exista
             * y pertenezca al entrenador autenticado.
             */

            using var context =
                new SqlConnection(
                    _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var parametrosClase =
                new DynamicParameters();

            parametrosClase.Add(
                "@IdEntrenador",
                idEntrenador);

            parametrosClase.Add(
                "@IdClaseProg",
                idClaseProg);

            var clase =
                context.QueryFirstOrDefault
                <ClaseEntrenadorDetalleResponseModel>(
                    "spDetalleClaseEntrenador",
                    parametrosClase,
                    commandType:
                        CommandType.StoredProcedure);

            if (clase == null)
            {
                return NotFound(
                    "La clase programada no existe o no pertenece a este entrenador.");
            }

            var parametrosParticipantes =
                new DynamicParameters();

            parametrosParticipantes.Add(
                "@IdEntrenador",
                idEntrenador);

            parametrosParticipantes.Add(
                "@IdClaseProg",
                idClaseProg);

            var participantes =
                context.Query
                <ParticipanteClaseEntrenadorResponseModel>(
                    "spListarParticipantesClaseEntrenador",
                    parametrosParticipantes,
                    commandType:
                        CommandType.StoredProcedure);

            return Ok(new
            {
                Clase = clase,

                Participantes =
                    participantes.ToList()
            });
        }


        [HttpPut("AsistenciaRegistrarAPI")]
        public IActionResult AsistenciaRegistrarAPI(
            AsistenciaClaseRegistrarRequestModel model)
        {
            if (model.IdReserva <= 0)
            {
                return BadRequest(
                    "El identificador de la reserva no es válido.");
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
                return BadRequest(
                    "El estado de asistencia debe ser Presente o Ausente.");
            }

            /*
             * Normalizar el estado para enviarlo
             * con el mismo formato utilizado en SQL.
             */

            model.EstadoAsistencia =
                string.Equals(
                    model.EstadoAsistencia,
                    "Presente",
                    StringComparison.OrdinalIgnoreCase)
                    ? "Presente"
                    : "Ausente";

            if (!string.IsNullOrWhiteSpace(
                model.Observacion))
            {
                model.Observacion =
                    model.Observacion.Trim();

                if (model.Observacion.Length > 300)
                {
                    return BadRequest(
                        "La observación no puede superar los 300 caracteres.");
                }
            }
            else
            {
                model.Observacion = null;
            }

            var idEntrenador =
                _utiles.ObtenerIdUsuarioToken();

            if (idEntrenador <= 0)
            {
                return Unauthorized(
                    "No se pudo identificar al entrenador.");
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
                    idEntrenador);

                parameters.Add(
                    "@IdReserva",
                    model.IdReserva);

                parameters.Add(
                    "@EstadoAsistencia",
                    model.EstadoAsistencia);

                parameters.Add(
                    "@Observacion",
                    model.Observacion);

                var response =
                    context.Execute(
                        "spRegistrarAsistenciaClaseEntrenador",
                        parameters,
                        commandType:
                            CommandType.StoredProcedure);

                if (response > 0)
                {
                    return Ok(
                        "La asistencia se registró correctamente.");
                }

                return BadRequest(
                    "No se pudo registrar la asistencia.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        #endregion
    }
}