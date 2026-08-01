namespace SuwoGym_API.Models
{
    public class ClienteDetalleResponseModel
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public bool Estado { get; set; }


        // CAMPOS OPCIONALES
        public string FechaNac { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; }



        // ROL DE USUARIO

        public int IdRol { get; set; }

        public string NombreRol { get; set; } =
            string.Empty;



        // DATOS DEL PLAN

        public string NombrePlan { get; set; } = string.Empty;

        public string EstadoMembresia { get; set; } = string.Empty;

        public DateTime FechaVencimiento { get; set; }

    }
}
