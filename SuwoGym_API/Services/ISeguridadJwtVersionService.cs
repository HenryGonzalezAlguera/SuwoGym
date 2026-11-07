using SuwoGym_API.Models;

namespace SuwoGym_API.Services;

public interface ISeguridadJwtVersionService
{
    Task<EstadoSeguridadJwt?> ConsultarAsync(
        int idUsuario, CancellationToken cancellationToken = default);
}
