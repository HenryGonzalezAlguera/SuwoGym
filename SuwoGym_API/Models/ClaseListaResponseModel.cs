namespace SuwoGym_API.Models
{
    public class ClaseListaResponseModel
    {
        public int IdClase { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string? Descripcion { get; set; }
    }
}