namespace SuwoGym_API.Models
{
    public class ClaseEntrenadorListaResponseModel
    {
        public int IdClaseProg { get; set; }

        public int IdEntrenador { get; set; }

        public int IdClase { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string? DescripcionClase { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }

        public int CupoActual { get; set; }

        public int CuposDisponibles { get; set; }

        public bool Estado { get; set; }

        public string EstadoHorario { get; set; } =
            string.Empty;
    }
}