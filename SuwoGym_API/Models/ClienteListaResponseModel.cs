namespace SuwoGym_API.Models
{
    public class ClienteListaResponseModel
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } =
            string.Empty;

        public string Apellido { get; set; } =
            string.Empty;

        public string CorreoElectronico { get; set; } =
            string.Empty;

        public bool Estado { get; set; }

        public string NombrePlan { get; set; } = string.Empty;
        
        public string EstadoMembresia { get; set; } = string.Empty;

        public DateTime FechaVencimiento { get; set; }

    }
}