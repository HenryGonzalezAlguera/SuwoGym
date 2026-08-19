namespace SuwoGym_API.Models
{
    public class HomePageCronogramaResponseModel
    {
        public string NombreClase { get; set; } = string.Empty;
        public string NombreEntrenador { get; set; } = string.Empty;
        public DateTime FechaHoraInicio { get; set; } 
        public DateTime FechaHoraFin { get; set; }
        public string CuposDisponibles { get; set; } = string.Empty;
        public string ColorTag { get; set; } = string.Empty;
    }
}
