using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class UsuarioLoginModel
    {
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
