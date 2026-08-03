using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SuwoGym_API.Services
{
    public class UtilesService(
        IConfiguration _config,
        IHttpContextAccessor _httpContext)
        : IUtilesService
    {
        public string GenerarContrasena()
        {
            const string caracteres =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var chars = new char[10];

            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] =
                    caracteres[
                        random.Next(caracteres.Length)];
            }

            return new string(chars);
        }

        public async Task EnviarCorreoAsync(
            string destinatario,
            string asunto,
            string cuerpoHtml)
        {
            var correo =
                _config["Correos:Correo"]!;

            var appPassword =
                _config["Correos:AppPassword"]!;

            if (string.IsNullOrEmpty(appPassword))
            {
                return;
            }

            var mensaje =
                new MimeMessage();

            mensaje.From.Add(
                new MailboxAddress(
                    string.Empty,
                    correo));

            mensaje.To.Add(
                MailboxAddress.Parse(
                    destinatario));

            mensaje.Subject =
                asunto;

            mensaje.Body =
                new TextPart(TextFormat.Html)
                {
                    Text = cuerpoHtml
                };

            using var cliente =
                new SmtpClient();

            await cliente.ConnectAsync(
                "smtp.gmail.com",
                587,
                SecureSocketOptions.StartTls);

            await cliente.AuthenticateAsync(
                correo,
                appPassword);

            await cliente.SendAsync(
                mensaje);

            await cliente.DisconnectAsync(
                true);
        }

        public string GenerarToken(
            int idUsuario,
            int idRol)
        {
            var tokenHandler =
                new JwtSecurityTokenHandler();

            var key =
                System.Text.Encoding.UTF8.GetBytes(
                    _config["Jwt:SecretKey"]!);

            var claims =
                new[]
                {
                    new Claim(
                        "IdUsuario",
                        idUsuario.ToString()),

                    new Claim(
                        "IdRol",
                        idRol.ToString()),

                    new Claim(
                        ClaimTypes.Role,
                        idRol.ToString())
                };

            var tokenDescriptor =
                new Microsoft.IdentityModel.Tokens
                    .SecurityTokenDescriptor
                {
                    Subject =
                        new ClaimsIdentity(claims),

                    Expires =
                        DateTime.UtcNow
                            .AddMinutes(30),

                    SigningCredentials =
                        new Microsoft.IdentityModel.Tokens
                            .SigningCredentials(
                                new Microsoft.IdentityModel.Tokens
                                    .SymmetricSecurityKey(key),

                                Microsoft.IdentityModel.Tokens
                                    .SecurityAlgorithms
                                    .HmacSha256Signature)
                };

            var token =
                tokenHandler.CreateToken(
                    tokenDescriptor);

            return tokenHandler.WriteToken(
                token);
        }

        public int ObtenerIdUsuarioToken()
        {
            var valor =
                _httpContext.HttpContext?
                    .User
                    .FindFirstValue(
                        "IdUsuario");

            return int.TryParse(
                valor,
                out var idUsuario)
                    ? idUsuario
                    : 0;
        }
    }
}