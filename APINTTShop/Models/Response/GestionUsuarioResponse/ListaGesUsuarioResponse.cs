using APINTTShop.Models.Entities;

namespace APINTTShop.Models.Response.GestionUsuarioResponse
{
    public class ListaGesUsuarioResponse : BaseResponseModel
    {
        public List<Gestionusuario> gesUsuarioLista { get; set; }
    }
}
