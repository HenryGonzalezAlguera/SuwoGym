
using System;
using System.Collections.Generic;

namespace SuwoGym_Web.Models
{
    namespace SuwoGym_Web.Models
    {
        public class HomePageModel
        {
            public InfoGeneral InfoGeneral { get; set; } =
                new();

            public List<MetodologiaEntrenamiento> Metodologias { get; set; } =
                new();

            public List<Membresia> Membresias { get; set; } =
                new();

            public List<CronogramaSemanal> Horarios { get; set; } =
                new();
        }
    }

    public class InfoGeneral
    {
        public string NombreGimnasio { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string HorarioAtencion { get; set; } = string.Empty;
        public string RedesSociales { get; set; } = string.Empty;
    }

    public class MetodologiaEntrenamiento
    {
        public string NombreClase { get; set; } =
            string.Empty;

        public string Descripcion { get; set; } =
            string.Empty;

        public string Icono { get; set; } =
            string.Empty;

        public string Color { get; set; } =
            string.Empty;

        public string Beneficio1 { get; set; } =
            string.Empty;

        public string Beneficio2 { get; set; } =
            string.Empty;

        public string Beneficio3 { get; set; } =
            string.Empty;

        public string Beneficio4 { get; set; } =
            string.Empty;

    }

    public class Membresia
    {
        public string NombrePlan { get; set; } =
          string.Empty;

        public decimal Precio { get; set; }

        public string Descripcion { get; set; } =
            string.Empty;

        public int DuracionDias { get; set; }

        public bool EsPopular { get; set; }

        public string Color { get; set; } =
            string.Empty;

        public string Beneficio1 { get; set; } =
           string.Empty;

        public string Beneficio2 { get; set; } =
            string.Empty;

        public string Beneficio3 { get; set; } =
            string.Empty;

        public string Beneficio4 { get; set; } =
            string.Empty;

    }

    public class CronogramaSemanal
    {
        public string NombreClase { get; set; } = string.Empty;
        public string NombreEntrenador { get; set; } = string.Empty;
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string CuposDisponibles { get; set; } = string.Empty;
        public string ColorTag { get; set; } = string.Empty;
    }

}