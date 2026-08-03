namespace SuwoGym_API.Models
{
    public class PagoMiembroDetalleResponseModel
    {
        public int IdPago { get; set; }

        public int IdMembresia { get; set; }

        public int IdUsuario { get; set; }

        public int IdPlan { get; set; }


        // Información del miembro

        public string? Identificacion { get; set; }

        public string NombreMiembro { get; set; } =
            string.Empty;

        public string CorreoElectronico { get; set; } =
            string.Empty;


        // Información del plan

        public string NombrePlan { get; set; } =
            string.Empty;

        public string? DescripcionPlan { get; set; }

        public int DuracionDias { get; set; }


        // Información del pago

        public string MetodoPago { get; set; } =
            string.Empty;

        public DateTime FechaEmision { get; set; }

        public string? NumeroReferencia { get; set; }

        public decimal Monto { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }


        // Información de la membresía

        public DateTime FechaInicio { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string EstadoMembresia { get; set; } =
            string.Empty;
    }
}