using System.ComponentModel.DataAnnotations;

namespace SuwoGym_Web.Models
{
    public class LoginUsuario
    {
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
