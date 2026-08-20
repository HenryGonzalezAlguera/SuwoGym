using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class UsuarioRegistroModel
    {
        [Required]
        public string nombre { get; set; } = string.Empty;

        [Required]
        public string apellido { get; set; } = string.Empty;

        [Required]
        public string correoElectronico { get; set; } = string.Empty;

        [Required]
        public string contrasenna { get; set; } = string.Empty;

        [Required]
        public string identificacion { get; set; } = string.Empty;

        public int PlanMembresia { get; set; }
    }
}
