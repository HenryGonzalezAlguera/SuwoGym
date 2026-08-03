namespace SuwoGym_Web.Models
{
    public class PagoRegistrarModel
    {
        public int IdUsuario { get; set; }

        public int IdMembresia { get; set; }

        public string NombreCliente { get; set; } =
            string.Empty;

        public string NombrePlan { get; set; } =
            string.Empty;

        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public string MetodoPago { get; set; } =
            string.Empty;

        public string? NumeroReferencia { get; set; }
    }
}