namespace SuwoGym_Web.Models
{
    public class ClaseProgramadaListaModel
    {
        public int IdClaseProg { get; set; }

        public int IdEntrenador { get; set; }

        public int IdClase { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string? DescripcionClase { get; set; }

        public string NombreEntrenador { get; set; } =
            string.Empty;

        public string CorreoEntrenador { get; set; } =
            string.Empty;

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }

        public int CupoActual { get; set; }

        public int CuposDisponibles { get; set; }

        public bool Estado { get; set; }
    }
}