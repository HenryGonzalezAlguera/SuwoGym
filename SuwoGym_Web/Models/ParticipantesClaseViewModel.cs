namespace SuwoGym_Web.Models
{
    public class ParticipantesClaseViewModel
    {
        public ClaseEntrenadorDetalleModel Clase { get; set; } =
            new ClaseEntrenadorDetalleModel();

        public List<ParticipanteClaseEntrenadorModel>
            Participantes
        { get; set; } =
                new List<ParticipanteClaseEntrenadorModel>();
    }
}