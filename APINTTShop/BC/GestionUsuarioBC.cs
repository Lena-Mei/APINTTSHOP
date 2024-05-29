using APINTTShop.DAC;
using APINTTShop.Models;
using APINTTShop.Models.Entities;
using APINTTShop.Models.Request.GestionUsuarioRequest;
using APINTTShop.Models.Request.UsuarioRequest;
using APINTTShop.Models.Response.GestionUsuarioResponse;
using APINTTShop.Models.Response.UsuarioResponse;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Xml.Linq;

namespace APINTTShop.BC
{
    public class GestionUsuarioBC
    {
        private readonly GestionUsuarioDAC gesUsuarioDAC = new GestionUsuarioDAC();
        private readonly NttshopContext context = new NttshopContext();

        public BaseResponseModel GetGesUser (int request)
        {
            IdGesUserResponse result = new IdGesUserResponse();
            if(IdValidation(request))
            {
                result.idGesUser = gesUsuarioDAC.GetGesUsuario(request);
                if(result.idGesUser !=null)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No existe un usuario con el id Introducido";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Id no válido";
            }


            return result;
        }

        public BaseResponseModel UpdateEmail(int idUsuario, string email)
        {
            BaseResponseModel result = new BaseResponseModel();
            int resultado = gesUsuarioDAC.UpdateEmail(idUsuario, email);
            if (resultado == 1)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else if (resultado == -1)
            {
                result.httpStatus = System.Net.HttpStatusCode.NotFound;
                result.message = "No existe un usuario con el id Introducido";
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Ya existe un correo registrado. Inserte otro";
            }
            return result;
        }

        public ListaGesUsuarioResponse GetAllGesUser()
        {
            ListaGesUsuarioResponse result = new ListaGesUsuarioResponse();

            result.gesUsuarioLista = gesUsuarioDAC.GetAllGesUsuario();

            if (result.gesUsuarioLista.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {

                result.httpStatus = System.Net.HttpStatusCode.NoContent;
                result.message = "No hay usuarios registrados.";
            }
            return result;
        }

        public BaseResponseModel InsertGesUser(GeneralGesUsuario request)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (InsertValidation(request))
            {
                bool correctOpreation = gesUsuarioDAC.InsertGesUsuario(request.gesUsuario);

                if (correctOpreation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "Algún dato introducido no es válido o ya está repetido.";

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Algún dato está vacío o no es válido. ";
            }

            return result;
        }
        public BaseResponseModel UpdateContrasenya(int idAdmin, string contrasenya)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (UpdateValidationContrasenya(idAdmin, contrasenya))
            {
                int resultado = gesUsuarioDAC.UpdateContrasenyaGesUsuario(idAdmin, contrasenya);
                if (resultado == 1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else if (resultado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El id del usuario introducido no existe.";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "La contraseña ya existe";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Datos no válidos";

            }
            return result;
        }

        public BaseResponseModel UpdateGesUser(GeneralGesUsuario request)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (UpdateValidation(request))
            {
                bool correctOperation = gesUsuarioDAC.UpdateGesUsuario(request.gesUsuario);
                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "El Id introducido no existe o algún dato introducido ya existe..";

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Algún dato está vacío o es nulo.";

            }
            return result;
        }

        public BaseResponseModel DeleteGesUser(int request)
        {
            BaseResponseModel result = new BaseResponseModel();

            if (IdValidation(request))
            {
                bool correctOperation = gesUsuarioDAC.DeleteGesUsuario(request);
                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No existe un USUARIO con el id introducido";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "ID no válido";
            }
            return result;
        }

        public BaseResponseModel GetLoginManagment(string inicio, string contrasenya)
        {
            Gestionusuario gesUser = new Gestionusuario();
            IdLoginGes result = new IdLoginGes();
            if (GetLoginValidation(inicio, contrasenya))
            {

                Encrypt encrypt = new Encrypt();
                contrasenya = encrypt.GetMD5Hash(contrasenya);
                gesUser = gesUsuarioDAC.GetLoginManagment(inicio, contrasenya);
                if (gesUser.IdUsuario != -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                    result.idGesUser = gesUser.IdUsuario;

                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "Usuario o contraseña no válidos.";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Todos los campos son obligatorios.";
            }
            return result;
        }

        public bool IdValidation(int request)
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
        private bool InsertValidation(GeneralGesUsuario request)
        {
            if (request != null
               && request.gesUsuario != null
               && !string.IsNullOrWhiteSpace(request.gesUsuario.Inicio)
               && !string.IsNullOrWhiteSpace(request.gesUsuario.Contrasenya)
               && !string.IsNullOrWhiteSpace(request.gesUsuario.Nombre)
               && !string.IsNullOrWhiteSpace(request.gesUsuario.Apellido1)
               && !string.IsNullOrWhiteSpace(request.gesUsuario.Email)
               && request.gesUsuario.Contrasenya.Length >= 10
               && request.gesUsuario.Contrasenya.Any(char.IsUpper)
               && request.gesUsuario.Contrasenya.Any(char.IsLower)
               && request.gesUsuario.Contrasenya.Any(char.IsDigit)
               && request.gesUsuario.IsoIdioma != null)

            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool UpdateValidationContrasenya(int idUsuario, string contrasenya )
        {
            Encrypt encrypt = new Encrypt();
                if (!string.IsNullOrEmpty(contrasenya)
                    && contrasenya.Length >= 10
                    && contrasenya.Any(char.IsUpper)
                    && contrasenya.Any(char.IsLower)
                    && contrasenya.Any(char.IsDigit)
                    && idUsuario > 0
                    && contrasenya != null)
                {

                    return true;
                }
                else
                {
                    return false;
                }

        

        }
        private bool UpdateValidation(GeneralGesUsuario request)
        {
            if (request != null && request.gesUsuario != null
                        && !string.IsNullOrWhiteSpace(request.gesUsuario.Inicio)
                        && !string.IsNullOrWhiteSpace(request.gesUsuario.Nombre)
                        && !string.IsNullOrWhiteSpace(request.gesUsuario.Apellido1)
                        && !string.IsNullOrWhiteSpace(request.gesUsuario.Email)
                        && request.gesUsuario.IdUsuario > 0
                        && request.gesUsuario.IsoIdioma != null)
                    {
                        return true;
                    }

                else
                {
                    return false;
                }
  

        }

        private bool GetLoginValidation(string inicio, string contrasenya)
        {
            if (!string.IsNullOrWhiteSpace(inicio) || !string.IsNullOrWhiteSpace(contrasenya))
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
