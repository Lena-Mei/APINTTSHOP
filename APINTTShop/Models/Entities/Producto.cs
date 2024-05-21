using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace APINTTShop.Models.Entities
{
    public class Producto
    {
        public int idProducto { get; set; }
        public int stock { get; set; }
        public bool habilitado { get; set; }
        public string imagen { get; set; }


        public List<DesProducto> descripcion {  get; set; }
        public List<ProductoRate> rate { get; set; }

        public Producto()
        {
            descripcion = new List<DesProducto>();
            rate = new List<ProductoRate>();
        }
    }
}
