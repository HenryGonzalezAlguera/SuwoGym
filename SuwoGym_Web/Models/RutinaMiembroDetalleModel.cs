namespace SuwoGym_Web.Models
{
    public class RutinaMiembroDetalleModel
    {
        public int IdRutina { get; set; }

        public int IdEntrenador { get; set; }

        public int IdCliente { get; set; }

        public string NombreRutina { get; set; } =
            string.Empty;

        public string Nivel { get; set; } =
            string.Empty;

        public string Detalles { get; set; } =
            string.Empty;

        public DateTime FechaCreacion { get; set; }

        public bool Estado { get; set; }

        public string NombreEntrenador { get; set; } =
            string.Empty;

        public string CorreoEntrenador { get; set; } =
            string.Empty;

        public string? TelefonoEntrenador { get; set; }

        public int CantidadEjercicios { get; set; }
    }
}