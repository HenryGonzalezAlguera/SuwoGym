namespace SuwoGym_API.Models
{
    public class PagoRegistrarRequestModel
    {
        public int IdMembresia { get; set; }

        public string MetodoPago { get; set; } =
            string.Empty;

        public string? NumeroReferencia { get; set; }
    }
}