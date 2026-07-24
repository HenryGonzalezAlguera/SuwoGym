using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class RecuperarAccesoRequestModel
    {
        [Required]
        public string CorreoElectronico { get; set; } =
            string.Empty;
    }
}