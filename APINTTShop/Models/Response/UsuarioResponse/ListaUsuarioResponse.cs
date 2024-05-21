using APINTTShop.Models.Entities;
namespace APINTTShop.Models.Response.UsuarioResponse
{
    public class ListaUsuarioResponse : BaseResponseModel
    {
        public List<Usuario> usuarioLista { get; set; }
    }
}
