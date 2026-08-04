namespace SuwoGym_Web.Models
{
    public class ParticipanteClaseEntrenadorModel
    {
        public int IdReserva { get; set; }

        public int IdClaseProg { get; set; }

        public int IdMiembro { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string? DescripcionClase { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public string? Identificacion { get; set; }

        public string NombreMiembro { get; set; } =
            string.Empty;

        public string CorreoElectronico { get; set; } =
            string.Empty;

        public string? Telefono { get; set; }

        public DateTime FechaReserva { get; set; }

        public bool EstadoReserva { get; set; }

        public string EstadoAsistencia { get; set; } =
            string.Empty;

        public string? Observacion { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}