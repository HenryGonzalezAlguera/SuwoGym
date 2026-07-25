namespace SuwoGym_API.Models
{
    public class EmpleadoCrearRequestModel
    {
        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Identificacion { get; set; } = string.Empty;

        public DateTime FechaNac { get; set; }

        public string Telefono { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public int Rol { get; set; }

        public string Direccion { get; set; } = string.Empty;
    }
}
