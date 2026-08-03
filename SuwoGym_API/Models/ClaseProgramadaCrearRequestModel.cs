namespace SuwoGym_API.Models
{
    public class ClaseProgramadaCrearRequestModel
    {
        public int IdEntrenador { get; set; }

        public int IdClase { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }
    }
}