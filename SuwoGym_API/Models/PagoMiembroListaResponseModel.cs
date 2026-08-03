namespace SuwoGym_API.Models
{
    public class PagoMiembroListaResponseModel
    {
        public int IdPago { get; set; }

        public int IdMembresia { get; set; }

        public int IdUsuario { get; set; }

        public int IdPlan { get; set; }

        public string NombrePlan { get; set; } =
            string.Empty;

        public string MetodoPago { get; set; } =
            string.Empty;

        public DateTime FechaEmision { get; set; }

        public string? NumeroReferencia { get; set; }

        public decimal Monto { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string EstadoMembresia { get; set; } =
            string.Empty;
    }
}