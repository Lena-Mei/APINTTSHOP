using APINTTShop.DAC;
using APINTTShop.Models;
using APINTTShop.Models.Response.PedidoResponse;
using APINTTShop.Models.Request.PedidoRequest;
using Azure.Core;
using APINTTShop.Models.Entities;
using APINTTShop.Models.Response.EstadoResponse;


namespace APINTTShop.BC
{
    public class PedidoBC
    {
        private readonly PedidoDAC pedidoDAC = new PedidoDAC();

        public BaseResponseModel GetPedidoidUser(int idUsuario)
        {
            ListaPedidoResponse result = new ListaPedidoResponse();

            if (IdValidation(idUsuario))
            {
                int estado;
                result.pedidoLista = pedidoDAC.GetPedidoIdUser(idUsuario, out estado);

                if (estado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "El id introducido no es válido.";
                }
                else if(estado ==-2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El usuario no tiene ningún pedido.";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.NotFound;
                result.message = "ID no valido";
            }
            return result;
        }
        public BaseResponseModel GetPedido (int idPedido, string idioma, int idRate)
        {
            PedidoResponse result = new PedidoResponse();

            if (IdValidation(idPedido))
            {
                int estado;
                result.pedido = pedidoDAC.GetPedido(idPedido, out estado, idioma, idRate);

                if(estado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El id introducido no es válido.";
                }
                else
                {
                    result.httpStatus=System.Net.HttpStatusCode.OK;
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "ID no valido";
            }
            return result;
        }



        public BaseResponseModel UpdateEstado (int idPedido, int idEstado)
        {
            BaseResponseModel result = new BaseResponseModel();
            string estado;
            if (IdEstadoPedidoValidation(idPedido, idEstado))
            {
                int resultado = pedidoDAC.UpdateEstadoPedido(idPedido, idEstado, out estado);

                if(resultado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El idEstado o/y el idPedido introducido/s no existe/n";
                }
                else if(resultado == -2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El pedido ya está en estado " + estado + " .";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Los datos introducidos no son válidos.";
            }
            return result;
        }
        public BaseResponseModel DeletePedido (int idPedido)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (IdValidation(idPedido))
            {
                int resultado = pedidoDAC.DeletePedido(idPedido);
                if(resultado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El ID introducido no existe.";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "El ID introducido no es válido.";
            }
            return result;
        }


        public BaseResponseModel InsertPedido (PedidoRequest pedido)
        {
            BaseResponseModel result = new BaseResponseModel();
            int estado = pedidoDAC.InsertPedido(pedido.pedido);

            if (InsertValidation(pedido))
            {
                if (estado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "Algún dato introducido es inválido.";
                }
                else if(estado == -2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "Debes rellenar todos los datos.";
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
            }
            else
            {

                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "Debes rellenar todos los datos.";
            }
            return result;
        }

        public ListaPedidoResponse GetAllPedidos(DateTime? fechaDesde = null, DateTime? fechaHasta = null, int? idEstado = null)
        {
            ListaPedidoResponse result = new ListaPedidoResponse();
            int estado;
            result.pedidoLista = pedidoDAC.GetAllPedidos(out estado, fechaDesde, fechaHasta, idEstado);
            if (result.pedidoLista.Count == 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.NotFound;
                result.message = "No hay ningún pedido con esas características.";
            }
            else if (estado == 1)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;

            }
            else if(estado == -1)
            {

                result.httpStatus = System.Net.HttpStatusCode.NotFound;
                result.message = "Algún dato introducido es inválido";
            }
            return result;
        }


        public bool InsertValidation (PedidoRequest pedido)
        {
            if(pedido == null
               || pedido.pedido.detallePedido == null
               || pedido.pedido.idPedido==null
               || pedido.pedido.idEstado == null
               || pedido.pedido.fechaPedido == null
               || pedido.pedido.totalPrecio == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IdValidation (int idPedido)
        {
            if(idPedido <= 0 || idPedido == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IdEstadoPedidoValidation (int idPedido, int idEstado)
        {
            if(idPedido <= 0 || idPedido == null || idEstado <= 0 || idEstado == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ListaEstadoResponse GetAllEstado()
        {
            ListaEstadoResponse result = new ListaEstadoResponse();

            //debemos de llamar al IdiomaDAC para así poder acceder a las consultas 
            result.estadoLista = pedidoDAC.GetAllEstados();

            if (result.estadoLista.Count() > 0)
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
    }
}
