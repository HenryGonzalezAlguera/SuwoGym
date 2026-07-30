namespace SuwoGym_Web.Models
{
    public class EmpleadoDetalle
    {
        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Identificacion { get; set; } = string.Empty;

        public string FechaNac { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public DateTime FechaRegistro {  get; set; } 

        public bool Estado { get; set; }

    }
}
