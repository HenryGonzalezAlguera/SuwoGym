namespace SuwoGym_Web.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string correoElectronico { get; set; } = string.Empty;
        public string contrasenna { get; set; } = string.Empty;
        public string identificacion { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
    }
}

