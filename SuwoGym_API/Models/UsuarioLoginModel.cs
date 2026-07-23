using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class UsuarioLoginModel
    {
        [Required]
        public string correoElectronico { get; set; } = string.Empty;

        [Required]
        public string contrasenna { get; set; } = string.Empty;



    }
}
