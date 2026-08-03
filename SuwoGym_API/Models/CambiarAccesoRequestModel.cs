using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class CambiarAccesoRequestModel
    {
        [Required]
        public string contrasenna { get; set; } =
            string.Empty;
    }
}