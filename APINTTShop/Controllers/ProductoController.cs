using APINTTShop.BC;
using APINTTShop.Handle;
using APINTTShop.Models;
using APINTTShop.Models.Request.ProductoRequest;
using APINTTShop.Models.Response.ProductoResponse;
using Microsoft.AspNetCore.Mvc;
using APINTTShop.Helpers.Interface;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using APINTTShop.Models.Response.IdiomaResponse;
using APINTTShop.Models.Entities;

namespace APINTTShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IhttpHandleResponse _httpHandleResponse;
        private readonly ProductoBC productoBC = new ProductoBC();

        public ProductoController(IhttpHandleResponse httpHandleResponse)
        {
            _httpHandleResponse = httpHandleResponse;
        }

        [HttpGet]
        [Route("getProducto")]
        public ActionResult<BaseResponseModel> GetProducto(int id, string? idioma = null, int? idRate = null)
        {
            BaseResponseModel result = productoBC.GetProducto(id, idioma, idRate);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPost]
        [Route("insertProducto")]
        public ActionResult<BaseResponseModel> InsertProducto(ProductoRequest request)
        {
            BaseResponseModel result = productoBC.InsertProducto(request);
            return _httpHandleResponse.HandleResponse(result);
        }


        [HttpPut]
        [Route("setPrecio")]
        public ActionResult<BaseResponseModel> SetPrecio(int idProducto, int idRate, decimal precio)
        {
            BaseResponseModel result = productoBC.SetPrecio(idProducto, idRate, precio);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpDelete]
        [Route("deleteProducto/{id}")]
        public ActionResult<BaseResponseModel> DeleteProducto(int id)
        {
            BaseResponseModel result = productoBC.DeleteProducto(id);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpGet]
        [Route("getAllProductos")]
        public ActionResult<ListaProductoResponse> GetAllProductos(string? idioma = null, int? idRate = null)
        {
            ListaProductoResponse result = productoBC.GetAllProductos(idioma, idRate);
            return _httpHandleResponse.HandleResponse(result);
        }

        [HttpPut]
        [Route("updateProducto")]
        public ActionResult<BaseResponseModel> UpdateProducto(ProductoRequest request)
        {
            BaseResponseModel result = productoBC.UpdateProducto(request);
            return _httpHandleResponse.HandleResponse(result);
        }

        //[HttpPut]
        //[Route("updateDesProducto")]
        //public ActionResult<BaseResponseModel> UpdateDesProducto(DesProducto request)
        //{
        //    BaseResponseModel result = productoBC.UpdateDesProducto(request);
        //    return _httpHandleResponse.HandleResponse(result);
        //}

        //[HttpPut]
        //[Route("updateRateProducto")]
        //public ActionResult<BaseResponseModel> UpdateRateProducto(ProdRateRequest request)
        //{
        //    BaseResponseModel result = productoBC.UpdateRateProducto(request);
        //    return _httpHandleResponse.HandleResponse(result);

        //}
    }
    }
