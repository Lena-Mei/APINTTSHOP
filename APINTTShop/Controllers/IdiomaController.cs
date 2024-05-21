using APINTTShop.BC;
using APINTTShop.Handle;
using APINTTShop.Models;
using APINTTShop.Models.Request.IdiomaRequest;
using APINTTShop.Models.Response.IdiomaResponse;
using Microsoft.AspNetCore.Mvc;
using APINTTShop.Helpers.Interface;

namespace APINTTShop.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class IdiomaController(IhttpHandleResponse httpHandleResponse)
    {

        private readonly IhttpHandleResponse _httpHandleResponse = httpHandleResponse;

        private readonly IdiomaBC idiomaBC = new IdiomaBC(); //Objeto creado para llamar a las funciones de BC



        [HttpGet]
        [Route("getAllIdiomas")]
        public ActionResult<ListaIdiomaResponse> GetAllIdiomas()
        {
            ListaIdiomaResponse result = idiomaBC.GetAllIdiomas();

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPost]
        [Route("insertIdioma")]
        public ActionResult<BaseResponseModel> InsertIdioma(GeneralIdiomaRequest request)
        {
            BaseResponseModel result = idiomaBC.InsertIdioma(request);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPut]
        [Route("updateIdioma")]
        public ActionResult<BaseResponseModel> UpdateIdioma(GeneralIdiomaRequest request)
        {
            BaseResponseModel result = idiomaBC.UpdateIdioma(request);

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getIdioma/{id}")]
        public ActionResult<BaseResponseModel> GetIdioma(int id)
        {
            BaseResponseModel result = idiomaBC.GetIdioma(id);
            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpDelete]
        [Route("deleteIdioma/{id}")]
        public ActionResult<BaseResponseModel> DeleteIdioma(int id)
        {
            BaseResponseModel result = idiomaBC.DeleteIdioma(id);
            return _httpHandleResponse.HandleResponse(result);
        }
    }
}
