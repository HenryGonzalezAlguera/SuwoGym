using System.ComponentModel.DataAnnotations;

namespace SuwoGym_Web.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }

        public string nombre { get; set; } =
            string.Empty;

        public string apellido { get; set; } =
            string.Empty;
       
        public string identificacion { get; set; } =
            string.Empty;

        public string correoElectronico { get; set; } =
            string.Empty;


        [Required]
        public string contrasenna { get; set; } =
            string.Empty;

        [Required]
        public string confirmarContrasenna { get; set; } =
            string.Empty;

        public bool DebeCambiarContrasenna { get; set; }

        public string Token { get; set; } =
            string.Empty;

        public bool Estado { get; set; }



        // ROL DE USUARIO

        public int IdRol { get; set; }

        public string NombreRol { get; set; } =
            string.Empty;


        // CAMPOS OPCIONALES
        public DateTime fechaNac { get; set; }

        public string telefono { get; set; } = string.Empty;

        public string direccion { get; set; } = string.Empty;

        public DateTime fechaRegistro { get; set; }


        // Para asignar membresia
        public int PlanMembresia { get; set; }

    }
}