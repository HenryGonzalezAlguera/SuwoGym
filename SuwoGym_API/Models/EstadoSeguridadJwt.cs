namespace SuwoGym_API.Models;

/// <summary>Estado de revocación consultado en SQL; nunca serializar como respuesta pública.</summary>
public sealed class EstadoSeguridadJwt
{
    public Guid VersionJwt { get; set; }
    public int IdRol { get; set; }
    public bool Estado { get; set; }

    public bool PermiteToken(Guid version, int rol) =>
        Estado && VersionJwt != Guid.Empty && version == VersionJwt && rol == IdRol;
}
