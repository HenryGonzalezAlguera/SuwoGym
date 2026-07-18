using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SuwoGym_API.Models;
using System.Security.Cryptography;
using System.Text;

namespace SuwoGym_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private string GetConnectionString()
        {
            return _configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioLoginModel request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CorreoElectronico) || string.IsNullOrEmpty(request.Contrasenna))
                {
                    return BadRequest(new ResponseModel
                    {
                        Exitoso = false,
                        Mensaje = "El correo y contraseña son requeridos."
                    });
                }

                string contrasennaHash = HashPassword(request.Contrasenna);

                using var connection = new SqlConnection(GetConnectionString());
                var parameters = new DynamicParameters();
                parameters.Add("@CorreoElectronico", request.CorreoElectronico);
                parameters.Add("@ContrasennaHash", contrasennaHash);

                var usuario = connection.QueryFirstOrDefault<DatoUsuarioResponseModel>(
                    "spIniciarSesionUsuario", parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (usuario == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        Exitoso = false,
                        Mensaje = "Correo o contraseña incorrectos."
                    });
                }

                return Ok(new ResponseModel
                {
                    Exitoso = true,
                    Mensaje = "Inicio de sesión exitoso.",
                    Usuario = usuario
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en login: {ex.Message}");
                return StatusCode(500, new ResponseModel
                {
                    Exitoso = false,
                    Mensaje = "Error al procesar la solicitud."
                });
            }
        }

        [HttpPost("registro")]
        public IActionResult Registro([FromBody] UsuarioRegistroModel request)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.Apellido) ||
                    string.IsNullOrEmpty(request.CorreoElectronico) || string.IsNullOrEmpty(request.Contrasenna))
                {
                    return BadRequest(new ResponseModel
                    {
                        Exitoso = false,
                        Mensaje = "Todos los campos son requeridos."
                    });
                }

                if (request.Contrasenna != request.ConfirmarContrasenna)
                {
                    return BadRequest(new ResponseModel
                    {
                        Exitoso = false,
                        Mensaje = "Las contraseñas no coinciden."
                    });
                }

                if (request.Contrasenna.Length < 8)
                {
                    return BadRequest(new ResponseModel
                    {
                        Exitoso = false,
                        Mensaje = "La contraseña debe tener al menos 8 caracteres."
                    });
                }

                string contrasennaHash = HashPassword(request.Contrasenna);

                using var connection = new SqlConnection(GetConnectionString());
                var parameters = new DynamicParameters();
                parameters.Add("@Nombre", request.Nombre);
                parameters.Add("@Apellido", request.Apellido);
                parameters.Add("@CorreoElectronico", request.CorreoElectronico);
                parameters.Add("@ContrasennaHash", contrasennaHash);

                try
                {
                    var resultado = connection.QueryFirstOrDefault<int>(
                        "spRegistrarUsuario", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    if (resultado > 0)
                    {
                        return Ok(new ResponseModel
                        {
                            Exitoso = true,
                            Mensaje = "Registro exitoso. Por favor, inicia sesión."
                        });
                    }
                    else
                    {
                        return BadRequest(new ResponseModel
                        {
                            Exitoso = false,
                            Mensaje = "Error al registrar el usuario."
                        });
                    }
                }
                catch (SqlException ex) when (ex.Message.Contains("registrado"))
                {
                    return BadRequest(new ResponseModel
                    {
                        Exitoso = false,
                        Mensaje = "El correo electrónico ya está registrado."
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en registro: {ex.Message}");
                return StatusCode(500, new ResponseModel
                {
                    Exitoso = false,
                    Mensaje = "Error al procesar el registro."
                });
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
    }
}

