namespace SuwoGym_API.Models;

/// <summary>
/// Respuesta transitoria de una operación local, protegida por Admin + HTTPS.
/// No almacenar esta respuesta en logs, cookies, TempData o base de datos.
/// </summary>
public class AccesoTemporalEntrenadorResponseModel
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string ContrasennaTemporal { get; set; } = string.Empty;
}
