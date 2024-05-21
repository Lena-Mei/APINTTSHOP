using APINTTShop.DAC;
using APINTTShop.Models;
using APINTTShop.Models.Entities;
using APINTTShop.Models.Request.ProductoRequest;
using APINTTShop.Models.Response.ProductoResponse;
using Azure.Core;
namespace APINTTShop.BC
{
    public class ProductoBC
    {
        private readonly ProductoDAC productoDAC = new ProductoDAC();

        public ListaProductoResponse GetAllProductos(string? idioma = null, int? idRate = null)
        {
            ListaProductoResponse result = new ListaProductoResponse();
            int estado;
            result.productoLista = productoDAC.GetAllProductos(out estado, idioma, idRate);

            
                if (estado == 1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else if (estado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "El idioma introducido no es correcto.";

                }
                else if (estado == -2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "El idioma no puede estar vacío.";
                }
            

           
            return result;
        }

        public bool GetAllValidation(string idioma)
        {
            if(!string.IsNullOrEmpty(idioma))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public BaseResponseModel DeleteProducto (int id)
        {
            BaseResponseModel result = new BaseResponseModel();
            if (GetValidation(id))
            {
                int resultado = productoDAC.DeleteProducto(id);

                if (resultado == 1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                    //result.message = "Se ha eliminado correctamente el producto.";


                }
                else if(resultado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.Conflict;
                    result.message = "El ID no existe.";
                }
                else if( resultado == -2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "El producto está en un pedido";

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "El ID introducido no es válido";
            }
            return result;
        }

        public BaseResponseModel SetPrecio(int idProducto, int idRate, decimal precio)
        {
            BaseResponseModel result = new BaseResponseModel();
            int resultado = productoDAC.SetPrecio(idProducto, idRate, precio);
            result.message = "Errores: \n";
            if(SetPrecioValidation(idProducto, idRate, precio))
            {
                if (resultado == 0)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else if (resultado == -1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message += "El idProducto o/y el idRate no existe en la bbdd";
                }
                else if(resultado == -3)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message += "El precio no puede tener un valor negativo ni 0";
                }
                else if (resultado == -2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message += "No puede haber ningún dato vacío/nulo";
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message += "Error. Algún dato introducido está vacío";
            }
               
            return result;
        }

        //public BaseResponseModel UpdateDesProducto(DesProducto request)
        //{
        //    BaseResponseModel result = new BaseResponseModel();
        //    int estado = productoDAC.UpdateDesProducto(request);
        //    if (estado == 1)
        //    {
        //        result.httpStatus = System.Net.HttpStatusCode.OK;
        //        //result.message = "Se ha actualizado correctamente el producto.";
        //    }
        //    else if (estado == -1)
        //    {
        //        result.httpStatus = System.Net.HttpStatusCode.NotFound;
        //        result.message = "Algún dato es incorrecto, o no existe en la base de datos.";
        //    }
        //    return result;
        //}

        //public BaseResponseModel UpdateRateProducto(ProdRateRequest request)
        //{
        //    BaseResponseModel result = new BaseResponseModel();
        //    int estado = productoDAC.UpdateProdRate(request.productoRate);
        //    if (estado == 1)
        //    {
        //        result.httpStatus = System.Net.HttpStatusCode.OK;
        //        //result.message = "Se ha actualizado correctamente el producto.";
        //    }
        //    else if (estado == -1)
        //    {
        //        result.httpStatus = System.Net.HttpStatusCode.NotFound;
        //        result.message = "Algún dato es incorrecto, o no existe en la base de datos.";
        //    }
        //    return result;
        //}

        public BaseResponseModel UpdateProducto (ProductoRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();
            int estado = productoDAC.UpdateProducto(request.producto);
            if(estado == 1)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
                //result.message = "Se ha actualizado correctamente el producto.";
            }
            else if(estado == -1)
            {
                result.httpStatus = System.Net.HttpStatusCode.NotFound;
                result.message = "Algún dato es incorrecto, o no existe en la base de datos.";
            }
            return result;

        }

        public BaseResponseModel InsertProducto (ProductoRequest request)
        {
            BaseResponseModel result = new BaseResponseModel();
            int estado = productoDAC.InsertProducto(request.producto);

                if (estado == 1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "La iso o el rate introducido no existe en la base de datos o el stock está en negativo.";
                }
            return result;
        }

        public BaseResponseModel GetProducto(int id, string? idioma= null, int? idRate = null)
        {
            GetProductoResponse result = new GetProductoResponse();
            if (GetValidation(id))
            {
                int estado;
                result.producto = productoDAC.GetProducto(id, out estado, idioma, idRate);

                if(estado ==1)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
        
                }
                else if(estado == -2)
                {
                    result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                    result.message = "El idioma o/y el idProducto no es válido .";
                }
                else if (estado == -3)
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "El producto introducido no contiene ninguna descripción en el idioma " + idioma + '.';
                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
                result.message = "ID no valido";
            }

            return result;
        }

        private bool SetPrecioValidation(int idRate, int idProducto, decimal precio) {
            if(idRate == null || idProducto== null || precio == null || precio<0)
            {
                return false;
            }
            else
            {
                return true;
            }
        
        }

        private bool GetValidation(int id)
        {
            if (id != null && id >= 0)
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

     