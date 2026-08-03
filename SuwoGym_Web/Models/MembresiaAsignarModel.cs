namespace SuwoGym_Web.Models
{
    public class MembresiaAsignarModel
    {
        public int IdUsuario { get; set; }

        public int IdPlan { get; set; }

        public string NombreCliente { get; set; } =
            string.Empty;
    }
}