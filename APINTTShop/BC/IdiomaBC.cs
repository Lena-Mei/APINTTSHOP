using APINTTShop.DAC;
using APINTTShop.Models;
using APINTTShop.Models.Request.IdiomaRequest;
using APINTTShop.Models.Response.IdiomaResponse;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace APINTTShop.BC
{

    //método que se llamará desde el controller que devolverá la respuesta que hemos creado
    public class IdiomaBC
    {
        private readonly IdiomaDAC idiomaDAC = new IdiomaDAC();
        private readonly NttshopContext context = new NttshopContext();


        public ListaIdiomaResponse GetAllIdiomas()
        {
            ListaIdiomaResponse result = new ListaIdiomaResponse();

            //debemos de llamar al IdiomaDAC para así poder acceder a las consultas 
            result.idiomaLista = idiomaDAC.GetAllIdiomas();

            if(result.idiomaLista.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.httpStatus= System.Net.HttpStatusCode.NoContent;
                result.message = "No Content uwu";
            }

            return result;
        }
        public BaseResponseModel GetIdioma(int request)
        {
            IdIdiomaResponse result = new IdIdiomaResponse();
            if (IdValidation(request))
            {
                result.idIdioma = idiomaDAC.GetIdiomas(request);

                if (result.idIdioma.idIdioma == 0)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No existe un idioma con el Id introducido";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "ID no valido";
            }
            return result;
        }

  

        public BaseResponseModel InsertIdioma(GeneralIdiomaRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (InsertIdiomaValidation(request))
            {
                int resultado = idiomaDAC.InsertIdioma(request.idioma);

                if (resultado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "Algún dato introducido ya existe.";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Algún dato introducido es inválido.";
            }

            return result;
        }

        public BaseResponseModel UpdateIdioma(GeneralIdiomaRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();

            if(UpdateIdiomaValidation(request))
            {
                int resultado = idiomaDAC.UpdateIdioma(request.idioma);

                if(resultado==1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else{
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "Algún dato introducido no es válido o está repetido.";
                }
            }
            else
            {
                result.httpStatus=System.Net.HttpStatusCode.BadRequest;
                result.message = "Debes de introducir los datos correctamente.";

            }
            return result;
        }

        public BaseResponseModel DeleteIdioma(int request)
        {
            BaseResponseModel result = new BaseResponseModel();

            if (IdValidation(request))
            {
                int resultado = idiomaDAC.DeleteIdioma(request);
                if (resultado==1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No existe un idioma con el Id introducido";
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
        private bool UpdateIdiomaValidation(GeneralIdiomaRequest request)
        {
            if (request != null
               && request.idioma != null
               && !string.IsNullOrWhiteSpace(request.idioma.descripcion)
               && !string.IsNullOrWhiteSpace(request.idioma.iso)
               && request.idioma.idIdioma > 0)

            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool InsertIdiomaValidation(GeneralIdiomaRequest request)
        {
            if (request != null
               && request.idioma != null
               && !string.IsNullOrWhiteSpace(request.idioma.descripcion)
               && !string.IsNullOrWhiteSpace(request.idioma.iso))

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
