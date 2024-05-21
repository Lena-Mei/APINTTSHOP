using APINTTShop.BC;
using APINTTShop.Models.Entities;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APINTTShop.DAC
{
    public class RateDAC
    {
        public Rate GetRate(int id)
        {
            Rate result = new Rate();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SELECT idRate, descripcion, [default] FROM RATE WHERE idRate=" + id, conexion);

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.idRate = int.Parse(reader["idRate"].ToString());
                        result.descripcion = reader["descripcion"].ToString();
                        result.defecto = bool.Parse(reader["default"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conexion.Close();
            }
            return result;

        }
        public List<Rate> GetAllRates()
        {
            List <Rate> result = new List<Rate>();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SELECT idRate, descripcion, [default] FROM RATE", conexion);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Rate rate = new Rate();
                        rate.idRate = int.Parse(reader["idRate"].ToString());
                        rate.descripcion = reader["descripcion"].ToString();
                        rate.defecto = bool.Parse(reader["default"].ToString());

                        result.Add(rate);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally { conexion.Close(); }

            return result;
        }


        public int InsertRate(Rate rate)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SET @resultado = 1; IF EXISTS (SELECT * FROM RATE WHERE descripcion = @descripcion) BEGIN  SET @resultado=-1; END ELSE BEGIN  INSERT RATE (descripcion, [default]) VALUES (@descripcion, @defecto) END", conexion);
                command.Parameters.AddWithValue("@descripcion", rate.descripcion);
                command.Parameters.AddWithValue("@defecto", rate.defecto);
                command.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int resultado = Convert.ToInt32(command.Parameters["@resultado"].Value);
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

        public int UpdateRate(Rate rate)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SET @resultado = 1; IF NOT EXISTS (SELECT * FROM RATE WHERE idRate = @idRate) OR EXISTS (SELECT * FROM RATE WHERE descripcion = @descripcion AND idRate != @idRate) BEGIN SET @resultado = -1; END ELSE BEGIN UPDATE RATE SET descripcion=@descripcion, [default]=@defecto WHERE idRate=@idRate; END", conexion);
                command.Parameters.AddWithValue("@descripcion", rate.descripcion);
                command.Parameters.AddWithValue("@defecto", rate.defecto);
                command.Parameters.AddWithValue("@idRate", rate.idRate);
                command.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int resultado = Convert.ToInt32(command.Parameters["@resultado"].Value);
                return resultado;

              
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conexion.Close();
            }
        }

        public int DeleteRate(int id)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SET @resultado =1; IF NOT EXISTS (SELECT * FROM RATE WHERE idRate = @idRate) BEGIN  SET @resultado =-1;   END ELSE IF EXISTS (SELECT p.idPedido FROM PEDIDO p, PRODUCTORATE pr WHERE pr.idRate = @idRate) BEGIN SET @resultado =-2;  END  ELSE BEGIN   DELETE FROM RATE WHERE idRate=" + id +"; END",conexion);
                command.Parameters.AddWithValue("@idRate", id);
                command.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int resultado = Convert.ToInt32(command.Parameters["@resultado"].Value);
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
