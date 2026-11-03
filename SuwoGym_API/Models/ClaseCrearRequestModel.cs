using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class ClaseCrearRequestModel
    {
        [Required(ErrorMessage = "Indica el nombre de la clase.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener de 2 a 50 caracteres.")]
        public string NombreClase { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción puede tener hasta 200 caracteres.")]
        public string? Descripcion { get; set; }
    }
}
