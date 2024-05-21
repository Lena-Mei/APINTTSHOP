using APINTTShop.Models.Entities;
namespace APINTTShop.Models.Response.ProductoResponse
{
    public class ListaProductoResponse : BaseResponseModel
    {
        public List<Producto> productoLista {  get; set; }
    }
}
