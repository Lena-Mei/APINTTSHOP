using APINTTShop.DAC;
using APINTTShop.Models;
using APINTTShop.Models.Request.RateRequest;
using APINTTShop.Models.Request.UsuarioRequest;
using APINTTShop.Models.Response.UsuarioResponse;
using Microsoft.Identity.Client;

namespace APINTTShop.BC
{
    public class UsuarioBC
    {
        private readonly UsuarioDAC usuarioDAC = new UsuarioDAC();
        private readonly NttshopContext context = new NttshopContext();


        public BaseResponseModel GetIdUsuario(string inicio)
        {
            Usuario usuario = new Usuario();
            IdLogin login = new IdLogin();
            usuario = usuarioDAC.GetidUser(inicio);
            if(usuario.IdUsuario == 0)
            {
                login.httpStatus = System.Net.HttpStatusCode.NotFound;
                login.message = "No existe un usuario con el nombre de sesión Introducido";
            }
            else
            {
                login.httpStatus = System.Net.HttpStatusCode.OK;
                login.idUsuario = usuario.IdUsuario;
                login.idiomaIso = usuario.IsoIdioma;
                login.idRate = usuario.IdRate;
            }
            return login;
        }

        public BaseResponseModel UpdateEmail(int idUsuario, string email)
        {
            BaseResponseModel result = new BaseResponseModel();
            int resultado = usuarioDAC.UpdateEmail(idUsuario, email);
            if (resultado == 1)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else if(resultado == -1)
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

        public BaseResponseModel GetUsuario (int request)
        {
            IdUsuarioResponse result = new IdUsuarioResponse();

            if (IdValidation(request))
            {
                result.idUsuario = usuarioDAC.GetUsuario(request);
                if (result.idUsuario == null)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No existe un usuario con el id Introducido";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Id no válido";
            }
            

            return result;
        }

        public ListaUsuarioResponse GetAllUsuarios()
        {
            ListaUsuarioResponse result = new ListaUsuarioResponse();

            result.usuarioLista = usuarioDAC.GetAllUsuario();

            if (result.usuarioLista.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {

                result.httpStatus = System.Net.HttpStatusCode.NoContent;
                result.message = "No hay usuarios";
            }
            return result;
        }

        public BaseResponseModel InsertUsuario (GeneralUsuarioRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (InsertUsuarioValidation(request))
            {
                bool correctOpreation = usuarioDAC.InsertUsuario(request.usuario);

                if(correctOpreation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "Falta algún dato o ya está registrado.";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Dato inválido. ";

            }

            return result;
        }
        public BaseResponseModel UpdateUsuario(GeneralUsuarioRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();
            if(UpdateValidationUsuario(request))
            {
                bool correctOperation = usuarioDAC.UpdateUsuario(request.usuario);
                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El Id introducido no existe o algún dato introducido ya existe..";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.NotFound;
                result.message = "Algún dato está vacío o es nulo.";

            }
            return result;
        }
        public BaseResponseModel UpdateContrasenya (int idUsuario, string contrasenya)
        {
            BaseResponseModel result = new BaseResponseModel();
            if(UpdateValidationContrasenya(idUsuario, contrasenya))
            {
                int resultado = usuarioDAC.UpdateContrasenya(idUsuario, contrasenya);
                if(resultado ==1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else if(resultado == -1)
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
        

        public BaseResponseModel DeleteUsuario(int request)
        {
            BaseResponseModel result = new BaseResponseModel();

            if (IdValidation(request))
            {
                bool correctOperation = usuarioDAC.DeleteUsuario(request);
                if(correctOperation)
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
                result.httpStatus= System.Net.HttpStatusCode.BadRequest;
                result.message = "ID no válido";
            }
            return result;
        }

        public BaseResponseModel GetLogin (string inicio, string contrasenya)
        {
            BaseResponseModel result = new BaseResponseModel();
            if(GetLoginValidation(inicio, contrasenya))
            {

                Encrypt encrypt = new Encrypt();
                contrasenya = encrypt.GetMD5Hash(contrasenya);
                bool estado = usuarioDAC.GetLogin(inicio, contrasenya);
                if(estado)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;

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

        private bool IdValidation(int  request)
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
        private bool InsertUsuarioValidation(GeneralUsuarioRequest request)
        {
            if (request != null
               && request.usuario != null
               && !string.IsNullOrWhiteSpace(request.usuario.Inicio)
               && !string.IsNullOrWhiteSpace(request.usuario.Contrasenya)
               && !string.IsNullOrWhiteSpace(request.usuario.Nombre)
               && !string.IsNullOrWhiteSpace(request.usuario.Apellido1)
               && !string.IsNullOrWhiteSpace(request.usuario.Email)
               && request.usuario.Contrasenya.Length >= 10
               && request.usuario.Contrasenya.Any(char.IsUpper)
               && request.usuario.Contrasenya.Any(char.IsLower)
               && request.usuario.Contrasenya.Any(char.IsDigit)
               && request.usuario.IsoIdioma != null
               && request.usuario.IdRate != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool UpdateValidationContrasenya(int idUsuario, string contrasenya)
        {
            Encrypt encrypt = new Encrypt();
            var user = context.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
                if ( !string.IsNullOrEmpty(contrasenya)
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
        private bool UpdateValidationUsuario(GeneralUsuarioRequest request)
        {
            if(request != null && (request.usuario != null
                && !string.IsNullOrWhiteSpace(request.usuario.Inicio)
                        && !string.IsNullOrWhiteSpace(request.usuario.Nombre)
                        && !string.IsNullOrWhiteSpace(request.usuario.Apellido1)
                        && !string.IsNullOrWhiteSpace(request.usuario.Email)
                        && request.usuario.IdUsuario > 0
                        && request.usuario.IsoIdioma != null
                        && request.usuario.IdRate != null))
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
   

