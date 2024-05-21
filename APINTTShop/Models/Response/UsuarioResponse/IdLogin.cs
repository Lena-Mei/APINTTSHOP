namespace APINTTShop.Models.Response.UsuarioResponse
{
    public class IdLogin : BaseResponseModel
    {
        public int idUsuario {  get; set; }
        public string idiomaIso {  get; set; }
        public int? idRate { get; set; }
    }
}
