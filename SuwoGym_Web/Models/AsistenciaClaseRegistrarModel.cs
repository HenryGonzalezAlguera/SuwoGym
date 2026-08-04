namespace SuwoGym_Web.Models
{
    public class AsistenciaClaseRegistrarModel
    {
        public int IdReserva { get; set; }

        /*
         * Se utiliza en el WEB para regresar
         * a la clase después de guardar.
         */
        public int IdClaseProg { get; set; }

        public string EstadoAsistencia { get; set; } =
            string.Empty;

        public string? Observacion { get; set; }
    }
}