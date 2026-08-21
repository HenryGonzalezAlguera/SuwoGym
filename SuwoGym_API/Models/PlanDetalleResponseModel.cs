namespace SuwoGym_API.Models
{
    public class PlanDetalleResponseModel
    {
        public int IdPlan { get; set; }

        public string NombrePlan { get; set; } =
            string.Empty;

        public string? Descripcion { get; set; }

        public int DuracionDias { get; set; }

        public decimal Precio { get; set; }

        public string Beneficio1 { get; set; } = string.Empty;
        public string Beneficio2 { get; set; } = string.Empty;
        public string Beneficio3 { get; set; } = string.Empty;
        public string Beneficio4 { get; set; } = string.Empty;
    }
}