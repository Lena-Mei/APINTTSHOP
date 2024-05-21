using APINTTShop.BC;
using APINTTShop.Handle;
using APINTTShop.Models;
using APINTTShop.Models.Request.RateRequest;
using APINTTShop.Models.Response.RateResponse;
using Microsoft.AspNetCore.Mvc;
using APINTTShop.Helpers.Interface;

namespace APINTTShop.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RateController(IhttpHandleResponse httpHandleResponse)
    {

        private readonly IhttpHandleResponse _httpHandleResponse = httpHandleResponse;

        private readonly RateBC rateBC = new RateBC(); //Objeto creado para llamar a las funciones de BC



        [HttpGet]
        [Route("getAllRate")]
        public ActionResult<ListaRateResponse> GetAllRates()
        {
            ListaRateResponse result = rateBC.GetAllRates();

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPost]
        [Route("insertRate")]
        public ActionResult<BaseResponseModel> InsertRate(GeneralRateRequest request)
        {
            BaseResponseModel result = rateBC.InsertRate(request);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPut]
        [Route("updateRate")]
        public ActionResult<BaseResponseModel> UpdateRate(GeneralRateRequest request)
        {
            BaseResponseModel result = rateBC.UpdateRate(request);

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getRate/{id}")]
        public ActionResult<BaseResponseModel> GetRate(int id)
        {
            BaseResponseModel result = rateBC.GetRate(id);
            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpDelete]
        [Route("deleteRate/{id}")]
        public ActionResult<BaseResponseModel> DeleteRate(int id)
        {
            BaseResponseModel result = rateBC.DeleteRate(id);
            return _httpHandleResponse.HandleResponse(result);
        }
    }
}
