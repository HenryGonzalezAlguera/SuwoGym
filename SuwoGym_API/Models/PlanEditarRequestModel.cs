namespace SuwoGym_API.Models
{
    public class PlanEditarRequestModel
    {
        public int IdPlan { get; set; }

        public string NombrePlan { get; set; } =
            string.Empty;

        public string? Descripcion { get; set; }

        public int DuracionDias { get; set; }

        public decimal Precio { get; set; }
    }
}