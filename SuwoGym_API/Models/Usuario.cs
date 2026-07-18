namespace SuwoGym_API.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string ContrasennaHash { get; set; } = string.Empty;
        public bool DebeCambiarContrasenna { get; set; } = false;
        public bool Estado { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? FechaUltimoAcceso { get; set; }

        public Rol? Rol { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
