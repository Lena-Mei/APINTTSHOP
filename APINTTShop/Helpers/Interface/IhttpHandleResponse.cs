using APINTTShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace APINTTShop.Helpers.Interface
{
    public interface IhttpHandleResponse
    {
        public ActionResult HandleResponse(BaseResponseModel response);
    }
}
