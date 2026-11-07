using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;

namespace SuwoGym_API.Services;

/// <summary>
/// Consulta SQL para validar vigencia del JWT en cada petición protegida.
/// Si la base no está disponible, la autenticación falla de forma segura.
/// </summary>
public sealed class SeguridadJwtVersionService(IConfiguration config)
    : ISeguridadJwtVersionService
{
    public async Task<EstadoSeguridadJwt?> ConsultarAsync(
        int idUsuario, CancellationToken cancellationToken = default)
    {
        if (idUsuario <= 0)
            return null;

        await using var connection = new SqlConnection(
            config["ConnectionStringsApi:DefaultConnection"]);

        return await connection.QuerySingleOrDefaultAsync<EstadoSeguridadJwt>(
            new CommandDefinition(
                "dbo.spSeguridadConsultarVersionJwt",
                new { IdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }
}
