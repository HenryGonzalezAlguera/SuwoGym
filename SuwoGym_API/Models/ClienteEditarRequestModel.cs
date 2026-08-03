namespace SuwoGym_API.Models
{
    public class ClienteEditarRequestModel
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } =
            string.Empty;

        public string Apellido { get; set; } =
            string.Empty;

        public DateTime? FechaNac { get; set; }

        public string? Telefono { get; set; }

        public string CorreoElectronico { get; set; } =
            string.Empty;

        public string? Direccion { get; set; }
    }
}