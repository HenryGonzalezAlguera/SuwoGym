namespace SuwoGym_Web.Models
{
    public class EntrenadorClienteDetalleModel
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

        public DateTime FechaAsignacion { get; set; }


        // Membresía actual

        public int? IdMembresia { get; set; }

        public int? IdPlan { get; set; }

        public string? NombrePlan { get; set; }

        public string? EstadoMembresia { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaVencimiento { get; set; }
    }
}