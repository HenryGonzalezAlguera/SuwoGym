namespace SuwoGym_API.Models
{
    public class AsistenciaClaseRegistrarRequestModel
    {
        public int IdReserva { get; set; }

        public string EstadoAsistencia { get; set; } =
            string.Empty;

        public string? Observacion { get; set; }
    }
}