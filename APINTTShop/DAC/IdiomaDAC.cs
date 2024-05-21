using APINTTShop.BC;
using APINTTShop.Models.Entities;
using System.Data;
using System.Data.SqlClient;


namespace APINTTShop.DAC
{
    public class IdiomaDAC
    {
        //Listado de objetos que simula la información de la base de datos
        //Lugar donde se realizan las consultas
        public List<Idioma> GetAllIdiomas()
        {
            List<Idioma> result = new List<Idioma>();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conexion.Open();

                SqlCommand command = new SqlCommand("SELECT idIdioma, descripcion, iso FROM IDIOMA", conexion);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Idioma idioma = new Idioma();
                        idioma.idIdioma = int.Parse(reader["idIdioma"].ToString());
                        idioma.descripcion = reader["descripcion"].ToString();
                        idioma.iso = reader["iso"].ToString();

                        result.Add(idioma);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }

            return result;
        }
        public Idioma GetIdiomas (int id)
        {
            Idioma result = new Idioma ();
            SqlConnection conexion = new SqlConnection( ConnectionManager.getConnectionString());
            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SELECT idIdioma, descripcion, iso FROM IDIOMA WHERE idIdioma=" + id, conexion);
                 using (SqlDataReader reader = command.ExecuteReader())
                 {

                    while(reader.Read())
                    { 
                        result.idIdioma = int.Parse(reader["idIdioma"].ToString());
                        result.descripcion = reader["descripcion"].ToString();
                        result.iso = reader["iso"].ToString();
                    }
                 }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
            return result;
        }
        public int InsertIdioma (Idioma idioma)
        {
            SqlConnection conexion = new SqlConnection( ConnectionManager.getConnectionString());
            try{
                conexion.Open();
                SqlCommand command = new SqlCommand(" SET @resultado = 1; IF EXISTS (SELECT * FROM IDIOMA WHERE iso = @iso) OR EXISTS (SELECT * FROM IDIOMA WHERE descripcion = @descripcion) BEGIN SET @resultado = -1;  END ELSE BEGIN INSERT IDIOMA (descripcion, iso) VALUES (@descripcion, @iso) END", conexion);
                command.Parameters.AddWithValue("@descripcion", idioma.descripcion);
                command.Parameters.AddWithValue("@iso", idioma.iso);

                command.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int resultado = Convert.ToInt32(command.Parameters["@resultado"].Value);
                return resultado;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public int UpdateIdioma(Idioma idioma)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());

            try
            { 
                conexion.Open();
                SqlCommand command = new SqlCommand(" SET @resultado = 1; IF EXISTS (SELECT * FROM IDIOMA WHERE iso = @iso AND idIdioma != @idIdioma) OR EXISTS (SELECT * FROM IDIOMA WHERE descripcion = @descripcion AND idIdioma != @idIdioma) OR NOT EXISTS (SELECT * FROM IDIOMA WHERE idIdioma = @idIdioma)  BEGIN SET @resultado = -1;  END ELSE BEGIN UPDATE IDIOMA SET descripcion=@descripcion, iso=@iso WHERE idIdioma=@idIdioma  END", conexion);
                command.Parameters.AddWithValue("@descripcion", idioma.descripcion);
                command.Parameters.AddWithValue("@iso", idioma.iso);
                command.Parameters.AddWithValue("@idIdioma", idioma.idIdioma);

                //nos devuelve la cantidad de cambios que se han producido
                command.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int resultado = Convert.ToInt32(command.Parameters["@resultado"].Value);
                return resultado;


            }
            catch (Exception ex)
            {
                throw new Exception("Hay un error en la consulta", ex);
            }
            finally
            {
                conexion.Close();
            }
        }


        public int DeleteIdioma (int id)
        {
            SqlConnection conexion = new SqlConnection( ConnectionManager.getConnectionString());
            try
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("SET @resultado = 1; IF NOT EXISTS (SELECT * FROM IDIOMA WHERE idIdioma = " + id +  ") BEGIN SET @resultado = -1 END ELSE BEGIN DELETE FROM IDIOMA WHERE idIdioma=" + id +" END", conexion);
                command.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int resultado = Convert.ToInt32(command.Parameters["@resultado"].Value);
                return resultado;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }


       
    }
}
