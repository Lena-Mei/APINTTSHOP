using APINTTShop.Models.Entities;
namespace APINTTShop.Models.Response.PedidoResponse
{
    public class PedidoResponse : BaseResponseModel
    {
        public Pedido pedido { get; set; }
    }
}
