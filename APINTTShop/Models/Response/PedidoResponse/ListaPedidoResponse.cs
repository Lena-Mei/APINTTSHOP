using APINTTShop.Models.Entities;
namespace APINTTShop.Models.Response.PedidoResponse
{
    public class ListaPedidoResponse : BaseResponseModel
    {
        public List<Pedido> pedidoLista { get; set; }
    }
}
