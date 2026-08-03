namespace SuwoGym_API.Models
{
    public class EjercicioListaResponseModel
    {
        public int IdEjercicio { get; set; }

        public int IdEntrenador { get; set; }

        public string NombreEjercicio { get; set; } =
            string.Empty;

        public string GrupoMuscular { get; set; } =
            string.Empty;

        public string? Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool Estado { get; set; }
    }
}