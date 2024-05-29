using Microsoft.AspNetCore.Mvc;
using APINTTShop.Helpers.Interface;
using APINTTShop.Models;
using APINTTShop.Models.Response.GestionUsuarioResponse;
using APINTTShop.Models.Request.GestionUsuarioRequest;
using APINTTShop.BC;
using APINTTShop.DAC;


namespace APINTTShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestionUsuarioController(IhttpHandleResponse httpHandleResponse)
    {
        private readonly IhttpHandleResponse _httpHandleResponse = httpHandleResponse;

        private readonly GestionUsuarioBC gesUsuarioBC = new GestionUsuarioBC();

        [HttpGet]
        [Route("getGesUsuario/{id}")]
        public ActionResult<BaseResponseModel> GetGesUsuario(int id)
        {
            BaseResponseModel result = gesUsuarioBC.GetGesUser(id);
            return _httpHandleResponse.HandleResponse(result);
        }



        [HttpPut]
        [Route("updateEmail")]
        public ActionResult<BaseResponseModel> UpdateEmail(int idUsuario, string correo)
        {
            BaseResponseModel result = gesUsuarioBC.UpdateEmail(idUsuario, correo);

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getAllGesUsuario")]
        public ActionResult<ListaGesUsuarioResponse> GetAllGesUsuario()
        {
            ListaGesUsuarioResponse result = gesUsuarioBC.GetAllGesUser();

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPost]
        [Route("insertGesUsuario")]
        public ActionResult<BaseResponseModel> InsertGesUsuario(GeneralGesUsuario request)
        {
            BaseResponseModel result = gesUsuarioBC.InsertGesUser(request);
            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpPut]
        [Route("updateGesUsuario")]
        public ActionResult<BaseResponseModel> UpdateUsuario(GeneralGesUsuario request)
        {
            BaseResponseModel result = gesUsuarioBC.UpdateGesUser(request);

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getLoginManagment")]
        public ActionResult<BaseResponseModel> GetLoginManagment(string inicio, string contrasenya)
        {
            BaseResponseModel result = gesUsuarioBC.GetLoginManagment(inicio, contrasenya);
            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpPut]
        [Route("updateGesContrasenya")]
        public ActionResult<BaseResponseModel> UpdateGesContrasenya(int idAdmin, string contrasenya)
        {
            BaseResponseModel result = gesUsuarioBC.UpdateContrasenya(idAdmin, contrasenya);

            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpDelete]
        [Route("deleteGesUsuario/{id}")]
        public ActionResult<BaseResponseModel> DeleteGesUsuario(int id)
        {
            BaseResponseModel result = gesUsuarioBC.DeleteGesUser(id);
            return _httpHandleResponse.HandleResponse(result);
        }
    }
}
