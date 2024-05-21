using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.Arm;
using APINTTShop.BC;
using APINTTShop.Models.Entities;
using Azure;
namespace APINTTShop.DAC
{
    public class PedidoDAC
    {
        private readonly ProductoDAC productoDAC = new ProductoDAC();

        public List<Pedido> GetPedidoIdUser(int idUsuario, out int estado)
        {
            List<Pedido> listaPedido= new List<Pedido>();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("GetAllPedidosIdUser", conexion);
            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUser", idUsuario);
                SqlParameter outputRes = new SqlParameter("@resultado", SqlDbType.Int);
                outputRes.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputRes);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pedido pedido = new Pedido();
                        pedido.idPedido = int.Parse(reader["idPedido"].ToString());
                        pedido.fechaPedido = Convert.ToDateTime(reader["fechaPedido"]);
                        pedido.idEstado = int.Parse(reader["idEstado"].ToString());
                        pedido.totalPrecio = decimal.Parse(reader["totalPrecio"].ToString());
                        pedido.idUsuario = int.Parse(reader["idUsuario"].ToString());
                        listaPedido.Add(pedido);
                    }

                    estado = Convert.ToInt32(outputRes.Value);
                }

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
            return listaPedido;
        }
        public Pedido GetPedido(int idPedido, out int estado, string idioma, int idRate)
        {
            Pedido response = new Pedido();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("GetPedido", conexion);
            int estado1;

            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                SqlParameter outputRes = new SqlParameter("@resultado", SqlDbType.Int);
                outputRes.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputRes);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        response.idPedido = int.Parse(reader["idPedido"].ToString());
                        response.fechaPedido = Convert.ToDateTime(reader["fechaPedido"]);
                        response.idEstado = int.Parse(reader["idEstado"].ToString());
                        response.totalPrecio = decimal.Parse(reader["totalPrecio"].ToString());
                        response.idUsuario = int.Parse(reader["idUsuario"].ToString());

                    }

                    if (reader.NextResult())
                    {
                        response.detallePedido = new List<DetallePedido>();
                        while (reader.Read())
                        {
                            DetallePedido det = new DetallePedido();

                            det.idPedido = int.Parse(reader["idPedido"].ToString());
                            det.idProducto = int.Parse(reader["idProducto"].ToString());
                            det.precio = decimal.Parse(reader["precio"].ToString());
                            det.unidades = int.Parse(reader["unidades"].ToString());

                            try
                            {
                                Producto producto = new Producto();
                                producto = productoDAC.GetProducto(det.idProducto,out estado1, idioma, idRate );
                                det.producto = producto;

                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message, ex);
                            }
                            response.detallePedido.Add(det);
                        }

                    }
                    estado = Convert.ToInt32(outputRes.Value);
                }

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
            return response;
        }

        public int DeletePedido (int idPedido)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("DeletePedido", conexion);

            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
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

        public int UpdateEstadoPedido(int idPedido, int idEstado, out string estado)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("UpdateEstadoPedido", conexion);

            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                cmd.Parameters.AddWithValue("@idEstado", idEstado);
                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@estado", SqlDbType.VarChar,20).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
                estado = cmd.Parameters["@estado"].Value.ToString();
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

        public int InsertPedido(Pedido pedido)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("InsertPedido", conexion);

            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //primero insertar el Pedido
                cmd.Parameters.AddWithValue("@fechaPedido", pedido.fechaPedido);
                cmd.Parameters.AddWithValue("@idEstado", pedido.idEstado);
                cmd.Parameters.AddWithValue("@idUsuario", pedido.idUsuario);
                cmd.Parameters.AddWithValue("@totalPrecio", pedido.totalPrecio);

                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int idPedido = Convert.ToInt32(cmd.Parameters["@resultado"].Value);

                
                //Después, insertar el pedidoDetalle
                if (pedido.detallePedido != null && pedido.detallePedido.Count > 0)
                {
                    foreach (var detalle in pedido.detallePedido)
                    {
                        SqlCommand cmdDetalle = new SqlCommand("InsertDetallePedido", conexion);
                        cmdDetalle.CommandType = CommandType.StoredProcedure;
                        cmdDetalle.Parameters.AddWithValue("@idPedido", idPedido);
                        cmdDetalle.Parameters.AddWithValue("@idProducto", detalle.idProducto);
                        cmdDetalle.Parameters.AddWithValue("@precio", detalle.precio);
                        cmdDetalle.Parameters.AddWithValue("@unidades", detalle.unidades);
                        cmdDetalle.ExecuteNonQuery();
                    }
                }

                return idPedido;
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

        public List<Pedido> GetAllPedidos(out int resultado, DateTime? fechaDesde = null, DateTime? fechaHasta = null, int? idEstado = null )
        {
            List<Pedido> pedidos = new List<Pedido>();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("GetAllPedidos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEstado", idEstado);
                cmd.Parameters.AddWithValue("@desdeFecha", fechaDesde);
                cmd.Parameters.AddWithValue("@hastaFecha", fechaHasta);
                SqlParameter outputParam = new SqlParameter("@resultado", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idPedido = int.Parse(reader["idPedido"].ToString());
                        Pedido pedidoActual = pedidos.FirstOrDefault(p => p.idPedido == idPedido);

                        if(pedidoActual == null)
                        {
                            pedidoActual = new Pedido
                            {
                                idPedido = idPedido,
                                fechaPedido = Convert.ToDateTime(reader["fechaPedido"]),
                                idEstado = int.Parse(reader["idEstado"].ToString()),
                                totalPrecio = decimal.Parse(reader["totalPrecio"].ToString()),
                                idUsuario = int.Parse(reader["idUsuario"].ToString()),
                                detallePedido = new List<DetallePedido>()
                            };

                            pedidos.Add(pedidoActual);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("idPedido")))
                        {
                            DetallePedido dp = new DetallePedido();

                            dp.idPedido = idPedido;
                            dp.idProducto = int.Parse(reader["idProducto"].ToString());
                            dp.precio = decimal.Parse(reader["precio"].ToString());
                            dp.unidades = int.Parse(reader["unidades"].ToString());
                            if (!pedidoActual.detallePedido.Any(d => d.idPedido == dp.idPedido))
                            {
                                pedidoActual.detallePedido.Add(dp);
                            }
                        }
                    }
                }
                    resultado = Convert.ToInt32(outputParam.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conexion.Close();
            }

            return pedidos;

        }

        public List<Estado> GetAllEstados()
        {
            List<Estado> result = new List<Estado>();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM ESTADOPEDIDO", conexion);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Estado estado = new Estado();
                        estado.idEstado = int.Parse(reader["idEstado"].ToString());
                        estado.descripcion = reader["descripcion"].ToString();

                        result.Add(estado);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally { conexion.Close(); }

            return result;
        }
    }
}

      
  