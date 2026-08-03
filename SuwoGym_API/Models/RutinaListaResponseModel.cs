namespace SuwoGym_API.Models
{
    public class RutinaListaResponseModel
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

        public string? Identificacion { get; set; }

        public string NombreCliente { get; set; } =
            string.Empty;

        public string CorreoElectronico { get; set; } =
            string.Empty;
    }
}