using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Models;
using SuwoGym_Web.Models.SuwoGym_Web.Models;
using System.Collections.Generic;
using System.Net;

namespace HomePage.Controllers
{
    public class HomePageController (
        IHttpClientFactory _http,
        IConfiguration _config): Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            using var client = _http.CreateClient();
            
            //Metodologías
            var urlApi_Meto = _config["ConnectionStringsWeb:urlApi"] + "/api/HomePage/MetodologiasAPI";
            var metodologias = client.GetAsync(urlApi_Meto).Result;
            var data1 = new List <MetodologiaEntrenamiento>();
            if (metodologias.StatusCode == HttpStatusCode.OK)
            {
               data1 = metodologias.Content
                       .ReadFromJsonAsync<List<MetodologiaEntrenamiento>>()
                       .Result;
            }
           
            //Membresías
            var urlApi_Memb = _config["ConnectionStringsWeb:urlApi"] + "/api/HomePage/MembresiasAPI";
            var membresias = client.GetAsync(urlApi_Memb).Result;
            var data2 = new List<Membresia>();
            if (membresias.StatusCode == HttpStatusCode.OK)
            {
                data2 = membresias.Content
                        .ReadFromJsonAsync<List<Membresia>>()
                        .Result;
            }

            //Conograma
            var urlApi_Crono = _config["ConnectionStringsWeb:urlApi"] + "/api/HomePage/CronogramaAPI";
            var cronograma = client.GetAsync(urlApi_Crono).Result;
            var data3 = new List<CronogramaSemanal>();
            if (cronograma.StatusCode == HttpStatusCode.OK)
            {
                data3 = cronograma.Content
                        .ReadFromJsonAsync<List<CronogramaSemanal>>()
                        .Result;
            }
            else if(cronograma.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.CronoVacio = cronograma.Content
                    .ReadAsStringAsync()
                    .Result;
            }


            var model = new HomePageModel
            {
                InfoGeneral = new InfoGeneral
                {
                    NombreGimnasio = "SUWO GYM",
                    Direccion = "Av. Principal #123, San Jose, Costa rica",
                    Telefono = "+506 22606269",
                    Email = "suwo@gmail.com",
                    Descripcion = "El mejor lugar para transformar tu cuerpo y mente. Contamos con instalaciones de primer nivel y entrenadores certificados.",
                    HorarioAtencion = "Lun - Vie: 6:00 AM - 10:00 PM | Sáb: 8:00 AM - 6:00 PM",
                    RedesSociales = "@suwogym"
                },
                Metodologias = data1,
                Membresias = data2,
                Horarios = data3
            };

            return View(model);
           

            throw new Exception(
               "Ocurrió un error al consultar los empleados.");
        }

       
    }
}