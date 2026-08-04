namespace SuwoGym_API.Models
{
    public class DashboardResponseModel
    {
        public DashboardIndicadoresResponseModel Indicadores
        {
            get;
            set;
        } = new DashboardIndicadoresResponseModel();


        public List<DashboardPagoRecienteResponseModel>
            PagosRecientes
        {
            get;
            set;
        } = new List<DashboardPagoRecienteResponseModel>();


        public List<DashboardProximaClaseResponseModel>
            ProximasClases
        {
            get;
            set;
        } = new List<DashboardProximaClaseResponseModel>();
    }
}