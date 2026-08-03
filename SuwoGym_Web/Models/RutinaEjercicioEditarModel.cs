namespace SuwoGym_Web.Models
{
    public class RutinaEjercicioEditarModel
    {
        public int IdRutinaEjercicio { get; set; }

        public int IdRutina { get; set; }

        public string NombreEjercicio { get; set; } =
            string.Empty;

        public string GrupoMuscular { get; set; } =
            string.Empty;

        public string DiaSemana { get; set; } =
            string.Empty;

        public int Series { get; set; }

        public int Repeticiones { get; set; }

        public decimal? PesoSugerido { get; set; }

        public int? DescansoSegundos { get; set; }

        public string? Observacion { get; set; }

        public int Orden { get; set; }
    }
}