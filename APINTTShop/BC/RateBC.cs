using APINTTShop.DAC;
using APINTTShop.Models;
using APINTTShop.Models.Request.RateRequest;
using APINTTShop.Models.Response.RateResponse;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace APINTTShop.BC
{

    //método que se llamará desde el controller que devolverá la respuesta que hemos creado
    public class RateBC
    {
        private readonly RateDAC rateDAC = new RateDAC();
        public BaseResponseModel GetRate(int request)
        {
            IdRateResponse result = new IdRateResponse();
            if (IdValidation(request))
            {
                result.idRate = rateDAC.GetRate(request);

                if (result.idRate.idRate > 0)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No existe una TARIFA con el Id introducido";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "El id introducido no es válido.";
            }
            return result;
        }

        public ListaRateResponse GetAllRates()
        {
            ListaRateResponse result = new ListaRateResponse();

            //debemos de llamar al IdiomaDAC para así poder acceder a las consultas 
            result.rateLista = rateDAC.GetAllRates();

            if (result.rateLista.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.NoContent;
                result.message = "No Content uwu";
            }

            return result;
        }

        public BaseResponseModel InsertRate(GeneralRateRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (InsertRateValidation(request))
            {
                int resultado = rateDAC.InsertRate(request.rate);

                if (resultado==1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "La descripción introducida ya existe.";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Datos no válidos...";
            }

            return result;
        }

        public BaseResponseModel UpdateRate(GeneralRateRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();

            if (UpdateRateValidation(request))
            {
                int resultado = rateDAC.UpdateRate(request.rate);

                if (resultado==1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "Algún dato ya existe en la bbdd.";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Los datos introducidos no son válidos";

            }
            return result;
        }

        public BaseResponseModel DeleteRate(int request)
        {
            BaseResponseModel result = new BaseResponseModel();

            if (IdValidation(request))
            {
                int resultado = rateDAC.DeleteRate(request);
                if (resultado==1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else if (resultado==-1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No existe una tarifa con el Id introducido";
                }
                else if(resultado == -2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "No se puede eliminar la tarifa introducida ya que está en uso en un pedido.";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "ID no valido";
            }
            return result;

        }

        private bool IdValidation(int request)
        {

            if (request != null && request > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool UpdateRateValidation(GeneralRateRequest request)
        {
            if (request != null
               && request.rate != null
               && !string.IsNullOrWhiteSpace(request.rate.descripcion)
               && request.rate.defecto== true || request.rate.defecto==false
               && request.rate.idRate > 0)

            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool InsertRateValidation(GeneralRateRequest request)
        {
            if (request != null
               && request.rate != null
               && !string.IsNullOrWhiteSpace(request.rate.descripcion)
               && request.rate.defecto == true || request.rate.defecto == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
