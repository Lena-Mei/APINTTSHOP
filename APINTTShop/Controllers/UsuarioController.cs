using APINTTShop.BC;
using APINTTShop.Helpers.Interface;
using APINTTShop.Models;
using APINTTShop.Models.Response.UsuarioResponse;
using APINTTShop.Models.Request.UsuarioRequest;
using Microsoft.AspNetCore.Mvc;
using APINTTShop.Models.Response.RateResponse;
using APINTTShop.Models.Request.RateRequest;

namespace APINTTShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController(IhttpHandleResponse httpHandleResponse)
    {
        private readonly IhttpHandleResponse _httpHandleResponse = httpHandleResponse;

        private readonly UsuarioBC usuarioBC = new UsuarioBC();

        [HttpGet]
        [Route("getUsuario/{id}")]
        public ActionResult<BaseResponseModel> GetUsuario(int id)
        {
            BaseResponseModel result = usuarioBC.GetUsuario(id);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getIdUsuario/{inicio}")]
        public ActionResult<BaseResponseModel> GetIdUsuario(string inicio)
        {
            BaseResponseModel result = usuarioBC.GetIdUsuario(inicio);
            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpGet]
        [Route("getLogin")]
        public ActionResult<BaseResponseModel> GetLogin(string inicio, string contrasenya)
        {
            BaseResponseModel result = usuarioBC.GetLogin(inicio, contrasenya);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getAllUsuario")]
        public ActionResult<ListaUsuarioResponse> GetAllUsuario()
        {
            ListaUsuarioResponse result = usuarioBC.GetAllUsuarios();

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPost]
        [Route("insertUsuario")]
        public ActionResult<BaseResponseModel> InsertUsuario(GeneralUsuarioRequest request)
        {
            BaseResponseModel result = usuarioBC.InsertUsuario(request);
            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpPut]
        [Route("updateUsuario")]
        public ActionResult<BaseResponseModel> UpdateUsuario(GeneralUsuarioRequest request)
        {
            BaseResponseModel result = usuarioBC.UpdateUsuario(request);

            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPut]
        [Route("updateContrasenya")]
        public ActionResult<BaseResponseModel> UpdateContrasenya(int idUsuario, string contrasenya)
        {
            BaseResponseModel result = usuarioBC.UpdateContrasenya(idUsuario, contrasenya);

            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpDelete]
        [Route("deleteUsuario/{id}")]
        public ActionResult<BaseResponseModel> DeleteUsuario(int id)
        {
            BaseResponseModel result = usuarioBC.DeleteUsuario(id);
            return _httpHandleResponse.HandleResponse(result);
        }



    }
}
