namespace SuwoGym_API.Models
{
    public class DashboardProximaClaseResponseModel
    {
        public int IdClaseProg { get; set; }

        public string NombreClase { get; set; } =
            string.Empty;

        public string NombreEntrenador { get; set; } =
            string.Empty;

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }

        public int CupoActual { get; set; }

        public int CuposDisponibles { get; set; }
    }
}