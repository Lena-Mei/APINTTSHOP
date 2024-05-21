using APINTTShop.BC;
using APINTTShop.DAC;
using APINTTShop.Helpers.Interface;
using APINTTShop.Models;
using APINTTShop.Models.Entities;
using APINTTShop.Models.Request.PedidoRequest;
using APINTTShop.Models.Request.ProductoRequest;
using APINTTShop.Models.Response.PedidoResponse;
using APINTTShop.Models.Response.ProductoResponse;
using Microsoft.AspNetCore.Mvc;

namespace APINTTShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IhttpHandleResponse _httpHandleResponse;
        private readonly PedidoBC pedidoBC = new PedidoBC();

        public PedidoController(IhttpHandleResponse httpHandleResponse)
        {
            _httpHandleResponse = httpHandleResponse;
        }

        [HttpGet]
        [Route("getPedido")]
        public ActionResult<BaseResponseModel> GetPedido(int id, string idioma, int idRate)
        {
            BaseResponseModel result = pedidoBC.GetPedido(id, idioma, idRate);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getPedidoIdUser/{id}")]
        public ActionResult<BaseResponseModel> GetPedidoidUser(int id)
        {
            BaseResponseModel result = pedidoBC.GetPedidoidUser(id);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpDelete]
        [Route("deletePedido/{id}")]
        public ActionResult<BaseResponseModel> DeletePedido(int id)
        {
            BaseResponseModel result = pedidoBC.DeletePedido(id);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPut]
        [Route("updateEstadoPedido/{idPedido}/{idEstado}")]
        public ActionResult<BaseResponseModel> UpdateEstadoPedido (int idPedido, int idEstado)
        {
            BaseResponseModel result = pedidoBC.UpdateEstado(idPedido, idEstado);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPost]
        [Route("insertPedido")]
        public ActionResult<BaseResponseModel> InsertPedido(PedidoRequest request)
        {
            BaseResponseModel result = pedidoBC.InsertPedido(request);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPost]
        [Route("getAllPedidos")]
        public ActionResult<BaseResponseModel> GetAllPedidos(DateTime? fechaDesde = null, DateTime? fechaHasta = null, int? idEstado = null)
        {
            BaseResponseModel result = pedidoBC.GetAllPedidos(fechaDesde, fechaHasta, idEstado);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPut]
        [Route("getAllEstados")]
        public ActionResult<BaseResponseModel> GetAllEstados()
        {
            BaseResponseModel result = pedidoBC.GetAllEstado();
            return _httpHandleResponse.HandleResponse(result);
        }

    }
}
