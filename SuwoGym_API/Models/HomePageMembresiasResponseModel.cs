namespace SuwoGym_API.Models
{
    public class HomePageMembresiasResponseModel
    {
        public string Tipo { get; set; } =
           string.Empty;

        public decimal Precio { get; set; }

        public string Descripcion { get; set; } =
            string.Empty;

        public int DuracionDias { get; set; } 

        public bool EsPopular { get; set; }

        public string Color { get; set; } =
            string.Empty;

        public string Beneficio1 { get; set; } =
           string.Empty;

        public string Beneficio2 { get; set; } =
            string.Empty;

        public string Beneficio3 { get; set; } =
            string.Empty;

        public string Beneficio4 { get; set; } =
            string.Empty;

    }
}
