namespace SuwoGym_API.Models
{
    public class EjercicioCrearRequestModel
    {
        public string NombreEjercicio { get; set; } =
            string.Empty;

        public string GrupoMuscular { get; set; } =
            string.Empty;

        public string? Descripcion { get; set; }
    }
}