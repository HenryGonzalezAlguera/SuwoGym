namespace SuwoGym_API.Models
{
    public class EmpleadoEditarDPRequestModel
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Identificacion { get; set; } = string.Empty;

        public DateTime FechaNac { get; set; }

        public string Telefono { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;
    }
}
