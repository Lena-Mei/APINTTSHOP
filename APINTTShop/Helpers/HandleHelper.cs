using APINTTShop.Helpers.Interface;
using APINTTShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace APINTTShop.Handle
{
    public class HandleHelper : Controller, IhttpHandleResponse
    {
        //No es una función de la API, Por lo que la etiqueta [NonAction] hace que 
        //que no aaprezca en el swather ese 

        public ActionResult HandleResponse(BaseResponseModel response)
        {
            if (response.httpStatus == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
                
            }
            if (response.httpStatus == System.Net.HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            if (response.httpStatus == System.Net.HttpStatusCode.BadRequest)
            {
                return BadRequest(response.message);
            }
            if (response.httpStatus == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(response.message);
            }
            if (response.httpStatus == System.Net.HttpStatusCode.Conflict)
            {
                return Conflict(response.message);
            }
            else
            {
                return Forbid();
            }
        }
    }
}
