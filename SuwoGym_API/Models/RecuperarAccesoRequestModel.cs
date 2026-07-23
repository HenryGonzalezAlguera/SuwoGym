using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class RecuperarAccesoRequestModel
    {

        [Required]
        public string correoElectronico { get; set; } = string.Empty;





    }
}
