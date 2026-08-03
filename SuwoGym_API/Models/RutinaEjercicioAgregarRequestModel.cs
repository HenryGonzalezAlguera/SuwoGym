namespace SuwoGym_API.Models
{
    public class RutinaEjercicioAgregarRequestModel
    {
        public int IdRutina { get; set; }

        public int IdEjercicio { get; set; }

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