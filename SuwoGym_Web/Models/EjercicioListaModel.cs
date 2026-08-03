namespace SuwoGym_Web.Models
{
    public class EjercicioListaModel
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