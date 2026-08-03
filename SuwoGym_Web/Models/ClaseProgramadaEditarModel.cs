namespace SuwoGym_Web.Models
{
    public class ClaseProgramadaEditarModel
    {
        public int IdClaseProg { get; set; }

        public int IdEntrenador { get; set; }

        public int IdClase { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public int CupoMaximo { get; set; }
    }
}