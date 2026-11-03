using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models;

public class RestablecerAccesoEntrenadorRequestModel
{
    [Range(1, int.MaxValue)]
    public int IdUsuario { get; set; }
}
