using System.ComponentModel.DataAnnotations;

namespace SuwoGym_Web.Models;

public class RestablecerContrasennaViewModel
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe la nueva contraseña.")]
    [StringLength(72, MinimumLength = 12,
        ErrorMessage = "La contraseña debe tener entre 12 y 72 caracteres.")]
    [DataType(DataType.Password)]
    public string NuevaContrasenna { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma la nueva contraseña.")]
    [Compare(nameof(NuevaContrasenna), ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    public string ConfirmarContrasenna { get; set; } = string.Empty;

    public bool TokenValido { get; set; }
    public string? MensajeError { get; set; }
}
