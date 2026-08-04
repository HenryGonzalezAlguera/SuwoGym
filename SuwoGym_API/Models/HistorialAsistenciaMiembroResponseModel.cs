namespace SuwoGym_API.Models
{
    public class HistorialAsistenciaMiembroResponseModel
    {
        public int IdReserva { get; set; }

        public int IdClaseProg { get; set; }

        public int IdMiembro { get; set; }

        public int IdClase { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string? DescripcionClase { get; set; }

        public int IdEntrenador { get; set; }

        public string NombreEntrenador { get; set; } =
            string.Empty;

        public string CorreoEntrenador { get; set; } =
            string.Empty;

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int DuracionMinutos { get; set; }

        public DateTime FechaReserva { get; set; }

        public DateTime? FechaCancelacion { get; set; }

        public bool EstadoReserva { get; set; }

        public int? IdAsistencia { get; set; }

        public string EstadoAsistencia { get; set; } =
            string.Empty;

        public string? Observacion { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public string EstadoHorario { get; set; } =
            string.Empty;
    }
}
