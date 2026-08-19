using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using SuwoGym_API.Services;

namespace SuwoGym_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController (
        IConfiguration _databasekey,
        IUtilesService _utiles) : ControllerBase
    {

        [HttpGet("MetodologiasAPI")]
        public IActionResult ObtenerMetodologiasAPI()
        {
            using var context =
                new SqlConnection( _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<HomePageMetodologiasResponseModel>(
                    "spObtenerClasesDetalle");

            if (response.Any())
                return Ok(response);
           
            return NotFound( "No hay metodologías registradas");
        }



        [HttpGet("MembresiasAPI")]
        public IActionResult ObtenerMembresiasAPI()
        {
            using var context =
                new SqlConnection( _databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<HomePageMembresiasResponseModel>(
                    "spObtenerPlanMembresiaDetalle");

            if (response.Any())
                return Ok(response);
           
            return NotFound( "No hay membresías registradas");
        }

        [HttpGet("CronogramaAPI")]
        public IActionResult ObtenerCronogramaSemanalAPI()
        {
            using var context =
                new SqlConnection(_databasekey[
                        "ConnectionStringsApi:DefaultConnection"]);

            var response =
                context.Query<HomePageCronogramaResponseModel>(
                    "spProximasClases");

            if (response.Any())
                return Ok(response);

            return NotFound("No hay clases registradas");
        }

    }
}
