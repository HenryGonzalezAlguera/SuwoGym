using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class UsuarioRegistroModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string Contrasenna { get; set; } = string.Empty;

        [Required]
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
