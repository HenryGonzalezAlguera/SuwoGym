namespace SuwoGym_Web.Models
{
    public class MiembroPerfilModel
    {
        public int IdUsuario { get; set; }

        public string? Identificacion { get; set; }

        public string Nombre { get; set; } =
            string.Empty;

        public string Apellido { get; set; } =
            string.Empty;

        public DateTime? FechaNac { get; set; }

        public string? Telefono { get; set; }

        public string CorreoElectronico { get; set; } =
            string.Empty;

        public string? Direccion { get; set; }

        public bool Estado { get; set; }


        // Membresía actual

        public int? IdMembresia { get; set; }

        public int? IdPlan { get; set; }

        public string? NombrePlan { get; set; }

        public decimal? PrecioPlan { get; set; }

        public string? EstadoMembresia { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaVencimiento { get; set; }


        // Entrenador asignado

        public int? IdEntrenador { get; set; }

        public string? NombreEntrenador { get; set; }

        public string? CorreoEntrenador { get; set; }

        public string? TelefonoEntrenador { get; set; }

        public DateTime? FechaAsignacionEntrenador { get; set; }
    }
}