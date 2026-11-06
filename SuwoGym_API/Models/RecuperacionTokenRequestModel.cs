using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models;

public class RecuperacionTokenRequestModel
{
    [Required]
    public string Token { get; set; } = string.Empty;
}
