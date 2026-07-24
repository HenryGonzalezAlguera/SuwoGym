namespace SuwoGym_API.Models
{
    public class EmpleadoListaResponseModel
    {
        public string Identificacion { get; set; } = string.Empty;
        
        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;
        
        public string NombreRol { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}
