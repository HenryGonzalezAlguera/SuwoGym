using SuwoGym_Web.Models;
using System.Text;
using System.Text.Json;

namespace SuwoGym_Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponse> LoginAsync(LoginUsuario usuario)
        {
            try
            {
                var url = $"{_configuration["ConnectionStringsWeb:urlApi"]}/api/auth/login";

                var json = JsonSerializer.Serialize(new
                {
                    usuario.CorreoElectronico,
                    usuario.Contrasenna
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<LoginResponse>(jsonResponse, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result?.Exitoso == true && result.Usuario != null)
                    {
                        // Guardar en sesión
                        var httpContext = _httpContextAccessor.HttpContext;
                        httpContext?.Session.SetString("IdUsuario", result.Usuario.IdUsuario.ToString());
                        httpContext?.Session.SetString("NombreUsuario", $"{result.Usuario.Nombre} {result.Usuario.Apellido}");
                        httpContext?.Session.SetString("CorreoElectronico", result.Usuario.CorreoElectronico);
                        httpContext?.Session.SetString("NombreRol", result.Usuario.NombreRol);
                    }

                    return result ?? new LoginResponse { Exitoso = false, Mensaje = "Error desconocido" };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorResult = JsonSerializer.Deserialize<LoginResponse>(errorContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return errorResult ?? new LoginResponse { Exitoso = false, Mensaje = "Error en la respuesta del servidor" };
            }
            catch (Exception ex)
            {
                return new LoginResponse { Exitoso = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<LoginResponse> RegistroAsync(Usuario usuario)
        {
            try
            {
                var url = $"{_configuration["ConnectionStringsWeb:urlApi"]}/api/auth/registro";

                var json = JsonSerializer.Serialize(new
                {
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.CorreoElectronico,
                    usuario.Contrasenna,
                    usuario.ConfirmarContrasenna
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LoginResponse>(jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? new LoginResponse { Exitoso = false, Mensaje = "Error desconocido" };
            }
            catch (Exception ex)
            {
                return new LoginResponse { Exitoso = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public void Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext?.Session.Clear();
        }
    }
}
