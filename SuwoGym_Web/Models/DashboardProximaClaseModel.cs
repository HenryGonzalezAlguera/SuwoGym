namespace SuwoGym_Web.Models
{
    public class DashboardProximaClaseModel
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