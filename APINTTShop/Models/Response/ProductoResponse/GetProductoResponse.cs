using APINTTShop.Models.Entities;
namespace APINTTShop.Models.Response.ProductoResponse
{
    public class GetProductoResponse : BaseResponseModel
    {
        public Producto producto { get; set; }

        //public List<DesProducto> desProducto { get; set; }
        //public List<ProductoRate> productoRate { get; set; }
    }
}
