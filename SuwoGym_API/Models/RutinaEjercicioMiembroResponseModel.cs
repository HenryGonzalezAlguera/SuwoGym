namespace SuwoGym_API.Models
{
    public class RutinaEjercicioMiembroResponseModel
    {
        public int IdRutinaEjercicio { get; set; }

        public int IdRutina { get; set; }

        public int IdEjercicio { get; set; }

        public string NombreEjercicio { get; set; } =
            string.Empty;

        public string GrupoMuscular { get; set; } =
            string.Empty;

        public string? Descripcion { get; set; }

        public string DiaSemana { get; set; } =
            string.Empty;

        public int Series { get; set; }

        public int Repeticiones { get; set; }

        public decimal? PesoSugerido { get; set; }

        public int? DescansoSegundos { get; set; }

        public string? Observacion { get; set; }

        public int Orden { get; set; }

        public DateTime FechaAsignacion { get; set; }

        public bool Estado { get; set; }
    }
}