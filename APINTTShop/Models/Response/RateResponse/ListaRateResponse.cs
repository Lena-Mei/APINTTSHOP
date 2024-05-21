using APINTTShop.Models.Entities;

namespace APINTTShop.Models.Response.RateResponse
{
    public class ListaRateResponse : BaseResponseModel
    {
        public List<Rate> rateLista { get; set; }
    }
}
