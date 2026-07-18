namespace SuwoGym_API.Models
{
    public class ResponseModel
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DatoUsuarioResponseModel? Usuario { get; set; }
    }
}
