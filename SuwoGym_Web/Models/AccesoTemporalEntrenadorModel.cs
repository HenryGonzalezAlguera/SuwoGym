namespace SuwoGym_Web.Models;

public class AccesoTemporalEntrenadorModel
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string ContrasennaTemporal { get; set; } = string.Empty;
}
