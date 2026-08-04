namespace SuwoGym_Web.Models
{
    public class DashboardModel
    {
        public DashboardIndicadoresModel Indicadores
        {
            get;
            set;
        } = new DashboardIndicadoresModel();


        public List<DashboardPagoRecienteModel>
            PagosRecientes
        {
            get;
            set;
        } = new List<DashboardPagoRecienteModel>();


        public List<DashboardProximaClaseModel>
            ProximasClases
        {
            get;
            set;
        } = new List<DashboardProximaClaseModel>();
    }
}