namespace SuwoGym_Web.Models
{
    public class DashboardPagoRecienteModel
    {
        public int IdPago { get; set; }

        public int IdMembresia { get; set; }

        public int IdUsuario { get; set; }

        public string NombreCliente { get; set; } =
            string.Empty;

        public string NombrePlan { get; set; } =
            string.Empty;

        public string MetodoPago { get; set; } =
            string.Empty;

        public DateTime FechaEmision { get; set; }

        public decimal Total { get; set; }
    }
}