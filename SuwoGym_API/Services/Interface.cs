namespace SuwoGym_API.Services
{
    public interface IUtilesService
    {
        string GenerarContrasena();

        Task EnviarCorreoAsync(
            string destinatario,
            string asunto,
            string cuerpoHtml);

        string GenerarToken(
            int idUsuario,
            int idRol,
            Guid versionJwt);

        int ObtenerIdUsuarioToken();
    }
}