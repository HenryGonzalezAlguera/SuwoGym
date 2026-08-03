namespace SuwoGym_Web.Models
{
    public class ClaseProgramadaCrearModel
    {
        public int IdEntrenador { get; set; }

        public int IdClase { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }
    }
}