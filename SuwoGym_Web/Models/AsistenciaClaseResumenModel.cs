namespace SuwoGym_Web.Models
{
    public class AsistenciaClaseResumenModel
    {
        public int IdClaseProg { get; set; }

        public int IdClase { get; set; }

        public int IdEntrenador { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string? DescripcionClase { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }

        public int CupoActual { get; set; }

        public int TotalParticipantes { get; set; }

        public int TotalPresentes { get; set; }

        public int TotalAusentes { get; set; }

        public int TotalPendientes { get; set; }

        public bool Estado { get; set; }

        public string EstadoHorario { get; set; } =
            string.Empty;
    }
}