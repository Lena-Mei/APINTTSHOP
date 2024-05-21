using System.Data;
using System.Data.SqlClient;
using APINTTShop.BC;
using APINTTShop.Models.Entities;
using APINTTShop.Models.Request.ProductoRequest;
using APINTTShop.Models.Response.ProductoResponse;
using Azure;

namespace APINTTShop.DAC
{
    public class ProductoDAC
    {

        public List<Producto> GetAllProductos(out int estado, string? idioma = null, int? idRate = null)
        {
            List<Producto> productos = new List<Producto>();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("GetAllProductos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idioma", idioma);
                cmd.Parameters.AddWithValue("@idRate", idRate);
                SqlParameter outputParam = new SqlParameter("@resultado", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idProducto = int.Parse(reader["idProducto"].ToString());
                        Producto productoActual = productos.FirstOrDefault(p => p.idProducto == idProducto);

                        // Si el producto no existe en la lista, créalo y agréguelo
                        if (productoActual == null)
                        {
                            productoActual = new Producto
                            {
                                idProducto = idProducto,
                                stock = int.Parse(reader["stock"].ToString()),
                                habilitado = bool.Parse(reader["habilitado"].ToString()),
                                imagen = reader["imagen"].ToString(),
                                descripcion = new List<DesProducto>(),
                                rate = new List<ProductoRate>()
                            };

                            productos.Add(productoActual);
                        }

                        // Agrega la descripción del producto si está presente
                        if (!reader.IsDBNull(reader.GetOrdinal("idDesProducto")))
                        {
                            DesProducto des = new DesProducto
                            {
                                idDesProducto = int.Parse(reader["idDesProducto"].ToString()),
                                idProducto = idProducto,
                                isoIdioma = reader["isoIdioma"].ToString(),
                                nombre = reader["nombre"].ToString(),
                                descripcion = reader["descripcion"].ToString()
                            };
                            if (!productoActual.descripcion.Any(d => d.idDesProducto == des.idDesProducto))
                            {
                                productoActual.descripcion.Add(des);
                            }
                        }

                        // Agrega todos los ProductoRate asociados al producto
                        if (!reader.IsDBNull(reader.GetOrdinal("idRate")))
                        {
                            ProductoRate proRate = new ProductoRate
                            {
                                idProducto = idProducto,
                                idRate = int.Parse(reader["idRate"].ToString()),
                                precio = decimal.Parse(reader["precio"].ToString())
                            };
                            productoActual.rate.Add(proRate);
                        }
                    }
                }
                estado = Convert.ToInt32(outputParam.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conexion.Close();
            }

            return productos;
        }


        public Producto GetProducto(int id, out int estado, string? idioma= null, int? idRate = null)
        {
            Producto response = new Producto();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("GetProducto", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", id);
                cmd.Parameters.AddWithValue("@idioma", idioma);
                cmd.Parameters.AddWithValue("@idRate", idRate);
                SqlParameter outputParam = new SqlParameter("@resultado", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        response.idProducto = int.Parse(reader["idProducto"].ToString());
                        response.stock = int.Parse(reader["stock"].ToString());
                        response.imagen = reader["imagen"].ToString();
                        response.habilitado = bool.Parse(reader["habilitado"].ToString());

                    }

                    if (reader.NextResult())
                    {
                        response.descripcion = new List<DesProducto>();
                        while (reader.Read())
                        {
                            DesProducto des = new DesProducto();

                            des.idDesProducto = int.Parse(reader["idDesProducto"].ToString());
                            des.idProducto = int.Parse(reader["idProducto"].ToString());
                            des.isoIdioma = reader["isoIdioma"].ToString();
                            des.nombre = reader["nombre"].ToString();
                            des.descripcion = reader["descripcion"].ToString();

                            response.descripcion.Add(des);
                        }
                    }
                    if (reader.NextResult())
                    {

                        response.rate = new List<ProductoRate>();
                        while (reader.Read())
                        {
                            ProductoRate proRate = new ProductoRate();


                            proRate.idProducto = int.Parse(reader["idProducto"].ToString());
                            proRate.idRate = int.Parse(reader["idRate"].ToString());
                            proRate.precio = decimal.Parse(reader["precio"].ToString());

                            response.rate.Add(proRate);
                        }
                    }
                }
            estado = Convert.ToInt32(outputParam.Value);
            }
                

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {

                conexion.Close();
            }

            return response;
        }



        public int DeleteProducto(int id)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("DeleteProducto", conexion);
            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", id);
                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output; //Obtener el parámetro de salida
                cmd.ExecuteNonQuery();
                int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conexion.Close();
                cmd.Parameters.Clear();

            }
        }


        //public int UpdateDesProducto(DesProducto des)
        //{
        //    SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
        //    SqlCommand cmd = new SqlCommand("UpdateDesProducto", conexion);
        //    try
        //    {
        //        conexion.Open();
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@idDesProducto", des.idDesProducto);
        //        cmd.Parameters.AddWithValue("@isoIdioma", des.isoIdioma);
        //        cmd.Parameters.AddWithValue("@nombre", des.nombre);
        //        cmd.Parameters.AddWithValue("@descripcion", des.descripcion);
        //        cmd.Parameters.AddWithValue("@idProducto", des.idProducto);


        //        cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //        cmd.ExecuteNonQuery();
        //        int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
        //        return resultado;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //    finally
        //    {
        //        cmd.Parameters.Clear();
        //        conexion.Close();
        //    }
        //}
        public int UpdateProducto(Producto producto)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("UpdateProducto", conexion);
            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //Se actualiza los datos del producto.
                cmd.Parameters.AddWithValue("@idProducto", producto.idProducto);
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.Parameters.AddWithValue("@habilitado", producto.habilitado);
                cmd.Parameters.AddWithValue("@imagen", producto.imagen);
              
                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);


                if (producto.descripcion != null && producto.descripcion.Count > 0)
                {
                    //int bien = 0; 
                    foreach(var des in producto.descripcion)
                    {
                    SqlCommand cmdDes = new SqlCommand("UpdateDesProducto", conexion);
                    cmdDes.CommandType = CommandType.StoredProcedure;
                        cmdDes.Parameters.AddWithValue("@idDesProducto", des.idDesProducto);
                        cmdDes.Parameters.AddWithValue("@idProducto", des.idProducto);
                        cmdDes.Parameters.AddWithValue("@nombre", des.nombre);
                        cmdDes.Parameters.AddWithValue("@descripcion", des.descripcion);
                        cmdDes.Parameters.AddWithValue("@isoIdioma", des.isoIdioma);
                        cmdDes.ExecuteNonQuery();
                   
                    }
                }
                if(producto.rate != null && producto.rate.Count >= 0)
                {
                   foreach(var rate in producto.rate)
                    {
                        SetPrecio(rate.idProducto, rate.idRate, rate.precio);
                    }
                }
               
                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                cmd.Parameters.Clear();
                
                conexion.Close();

            }
        }

        //public int UpdateProdRate (ProductoRate producto)
        //{
        //    SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
        //    SqlCommand cmd = new SqlCommand("InsertRate", conexion);
        //    try
        //    {
        //        conexion.Open();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@idProducto", producto.idProducto);
        //        cmd.Parameters.AddWithValue("@idRate", producto.idRate);

        //        cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //        cmd.ExecuteNonQuery();
        //        int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
        //        return resultado;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //    finally
        //    {
        //        cmd.Parameters.Clear();
        //        conexion.Close();

        //    }
        //}

        public int InsertProducto (Producto producto)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("InsertProducto", conexion);
            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.Parameters.AddWithValue("@habilitado", producto.habilitado);

               
                DesProducto desProducto = producto.descripcion[0];
                cmd.Parameters.AddWithValue("@isoIdioma", desProducto.isoIdioma);
                cmd.Parameters.AddWithValue("@nombre", desProducto.nombre);
                cmd.Parameters.AddWithValue("@descripcion", desProducto.descripcion);

                ProductoRate productoRate = producto.rate[0];
                cmd.Parameters.AddWithValue("@idRate", productoRate.idRate);

                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                cmd.Parameters.Clear();
                conexion.Close();
            }
        }
        public int SetPrecio(int idProducto, int idRate, decimal precio)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SetPrecio", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                cmd.Parameters.AddWithValue("@idRate", idRate);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
                cmd.Parameters.Clear();
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conexion.Close();
            }
        }





    }
}

