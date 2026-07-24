namespace SuwoGym_API.Models
{
    public class PlanesMListaResponseModel
    {
        public string NombrePlan { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public int DuracionDias { get; set; }
        
        public decimal Precio { get; set; }
    
    }
}
