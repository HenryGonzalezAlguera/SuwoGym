using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models;

public class RestablecerContrasennaRequestModel
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [StringLength(72, MinimumLength = 12)]
    public string NuevaContrasenna { get; set; } = string.Empty;
}
