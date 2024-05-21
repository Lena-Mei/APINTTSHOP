namespace APINTTShop.Models.Entities
{
    public class DetallePedido
    {
        public int idPedido {  get; set; }
        public int idProducto { get; set; }
        public decimal precio { get; set; }
        public int unidades { get; set; }

        public Producto ? producto { get; set; }

    }
}
