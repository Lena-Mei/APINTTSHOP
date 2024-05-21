namespace APINTTShop.Models.Request.PedidoRequest
{
    public class GetAllPedidosRequest
    {
        public int? idEstado {  get; set; }
        public DateTime? fechaDesde { get; set; }
        public DateTime? fechaHasta { get; set; }
    }
}
