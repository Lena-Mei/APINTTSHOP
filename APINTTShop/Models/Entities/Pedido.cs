namespace APINTTShop.Models.Entities
{
    public class Pedido
    {
        public int idPedido { get; set; }
        public DateTime fechaPedido { get; set; }
        public int idEstado { get; set; }
        public decimal totalPrecio { get; set; }
       
        public int idUsuario { get; set; }

        public List<DetallePedido> detallePedido { get; set; }

        public Pedido()
        {
            detallePedido = new List<DetallePedido>();
        }

    }
}
