namespace SuwoGym_API.Models
{
    public class ReservaClaseMiembroListaResponseModel
    {
        public int IdReserva { get; set; }

        public int IdClaseProg { get; set; }

        public int IdMiembro { get; set; }

        public int IdClase { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string? DescripcionClase { get; set; }

        public string NombreEntrenador { get; set; } =
            string.Empty;

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }

        public int CupoActual { get; set; }

        public int CuposDisponibles { get; set; }

        public DateTime FechaReserva { get; set; }

        public DateTime? FechaCancelacion { get; set; }

        public bool Estado { get; set; }

        public string EstadoReserva { get; set; } =
            string.Empty;

        public bool PuedeCancelar { get; set; }
    }
}