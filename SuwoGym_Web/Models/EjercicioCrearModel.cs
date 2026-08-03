namespace SuwoGym_Web.Models
{
    public class EjercicioCrearModel
    {
        public string NombreEjercicio { get; set; } =
            string.Empty;

        public string GrupoMuscular { get; set; } =
            string.Empty;

        public string? Descripcion { get; set; }
    }
}