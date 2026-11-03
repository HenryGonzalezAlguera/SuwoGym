using System.ComponentModel.DataAnnotations;

namespace SuwoGym_Web.Models
{
    /// <summary>
    /// Datos solicitados exclusivamente en la página de registro.
    /// Evita aplicar la confirmación de contraseña al inicio de sesión.
    /// </summary>
    public class RegistroUsuarioViewModel
    {
        [Required(ErrorMessage = "Ingresa tu nombre.")]
        public string nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa tu apellido.")]
        public string apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa tu correo electrónico.")]
        [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
        public string correoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa tu identificación.")]
        public string identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa una contraseña.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string contrasenna { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma tu contraseña.")]
        [Compare(nameof(contrasenna), ErrorMessage = "Las contraseñas no coinciden.")]
        public string confirmarContrasenna { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un plan de membresía.")]
        public int PlanMembresia { get; set; }
    }
}
