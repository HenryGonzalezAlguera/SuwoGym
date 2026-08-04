namespace SuwoGym_Web.Models
{
    public class DashboardIndicadoresModel
    {
        public int TotalClientes { get; set; }

        public int ClientesActivos { get; set; }

        public int EntrenadoresActivos { get; set; }

        public int MembresiasActivas { get; set; }

        public int MembresiasVencidas { get; set; }

        public int MembresiasCanceladas { get; set; }

        public decimal IngresosTotales { get; set; }

        public decimal IngresosMesActual { get; set; }

        public int PagosMesActual { get; set; }

        public int ClasesProgramadas { get; set; }

        public int ReservasActivas { get; set; }

        public int TotalPresentes { get; set; }

        public int TotalAusentes { get; set; }
    }
}