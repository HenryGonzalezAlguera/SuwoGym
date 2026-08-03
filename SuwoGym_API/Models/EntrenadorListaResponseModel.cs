namespace SuwoGym_API.Models
{
    public class EntrenadorListaResponseModel
    {
        public int IdEntrenador { get; set; }

        public string? Identificacion { get; set; }

        public string Nombre { get; set; } =
            string.Empty;

        public string Apellido { get; set; } =
            string.Empty;

        public string NombreCompleto { get; set; } =
            string.Empty;

        public string CorreoElectronico { get; set; } =
            string.Empty;
    }
}