namespace SuwoGym_API.Models
{
    public class DatoUsuarioResponseModel
    {
        public int IdUsuario { get; set; }

        public string Identificacion { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public string ContrasennaHash { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public bool DebeCambiarContrasenna { get; set; }

        public string Token { get; set; } = string.Empty;

        public int IdRol { get; set; }

        public string NombreRol { get; set; } = string.Empty;
    }
}