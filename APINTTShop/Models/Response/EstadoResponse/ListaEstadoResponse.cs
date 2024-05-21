using APINTTShop.Models.Entities;

namespace APINTTShop.Models.Response.EstadoResponse
{
    public class ListaEstadoResponse :BaseResponseModel
    {
        public List<Estado> estadoLista {  get; set; }
    }
}
