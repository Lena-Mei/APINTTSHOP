using APINTTShop.Models.Entities;

namespace APINTTShop.Models.Response.IdiomaResponse
{
    public class ListaIdiomaResponse : BaseResponseModel//Lista de los idiomas 
    {
        public List<Idioma> idiomaLista {  get; set; }
    }
}
