using System.ComponentModel.DataAnnotations;

namespace SuwoGym_API.Models
{
    public class DatoUsuarioResponseModel
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public string NombreRol { get; set; } = string.Empty;



    }
}
