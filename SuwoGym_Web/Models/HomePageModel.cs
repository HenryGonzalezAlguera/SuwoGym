
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

            public CronogramaSemanal Horarios { get; set; } =
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
        public int Id { get; set; }

        public string Nombre { get; set; } =
            string.Empty;

        public string Descripcion { get; set; } =
            string.Empty;

        public string Icono { get; set; } =
            string.Empty;

        public string Color { get; set; } =
            string.Empty;

        public List<string> Beneficios { get; set; } =
            new();

        public string ImagenUrl { get; set; } =
            string.Empty;
    }

    public class Membresia
    {
        public int Id { get; set; }

        public string Tipo { get; set; } =
            string.Empty;

        public decimal Precio { get; set; }

        public string Descripcion { get; set; } =
            string.Empty;

        public List<string> Beneficios { get; set; } =
            new();

        public string Duracion { get; set; } =
            string.Empty;

        public bool EsPopular { get; set; }

        public string Color { get; set; } =
            string.Empty;
    }

    public class CronogramaSemanal
    {
        public List<DiaHorario> Dias { get; set; } =
     new();
    }

    public class DiaHorario
    {
        public string Dia { get; set; } =
            string.Empty;

        public string DiaSemana { get; set; } =
            string.Empty;

        public List<Clase> Clases { get; set; } =
            new();
    }
    public class Clase
    {
        public string HoraInicio { get; set; } = string.Empty;
        public string HoraFin { get; set; } = string.Empty;
        public string Actividad { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public string ColorTag { get; set; } = string.Empty;
    }
}