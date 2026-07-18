namespace SuwoGym_API.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public bool Estado { get; set; } = true;

        public Usuario? Usuario { get; set; }
    }
}
