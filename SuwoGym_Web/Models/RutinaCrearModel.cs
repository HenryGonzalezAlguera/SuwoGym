namespace SuwoGym_Web.Models
{
    public class RutinaCrearModel
    {
        public int IdCliente { get; set; }

        public string NombreRutina { get; set; } =
            string.Empty;

        public string Nivel { get; set; } =
            string.Empty;

        public string Detalles { get; set; } =
            string.Empty;
    }
}