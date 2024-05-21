using APINTTShop.BC;
using APINTTShop.Models;
using APINTTShop.Models.Entities;
using APINTTShop.Models.Request.GestionUsuarioRequest;
using System.Data;
using System.Data.SqlClient;
namespace APINTTShop.DAC
{
    public class GestionUsuarioDAC
    {
       private readonly NttshopContext context = new NttshopContext();

        public Gestionusuario GetGesUsuario(int id)
        {
            Gestionusuario result = new Gestionusuario(); //Creamos un objeto ya que es lo que vamos a devolver
            try
            {
                result = context.Gestionusuarios.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        public List<Gestionusuario> GetAllGesUsuario()
        {
            List<Gestionusuario> result = new List<Gestionusuario> ();
            try
            {
                result = context.Gestionusuarios.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return result;
        }

        public bool InsertGesUsuario(Gestionusuario gesUsuario)
        {
            try
            {
                Encrypt encrypt = new Encrypt();
                gesUsuario.Contrasenya = encrypt.GetMD5Hash(gesUsuario.Contrasenya);
                if (context.Gestionusuarios.Any(u => u.Email == gesUsuario.Email) || context.Gestionusuarios.Any(u => u.Inicio == gesUsuario.Inicio))
                {
                    return false;
                }
                else
                {
                    context.Gestionusuarios.Add(gesUsuario);
                    context.SaveChanges();
                    return true;
                }
    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool UpdateGesUsuario (Gestionusuario gesUsuario)
        {
            try
            {
                var gesUser = context.Gestionusuarios.FirstOrDefault(g => g.IdUsuario == gesUsuario.IdUsuario);
                if(gesUser != null)
                {
                    gesUser.Nombre = gesUsuario.Nombre;
                    gesUser.Inicio = gesUsuario.Inicio;
                    gesUser.Apellido1 = gesUsuario.Apellido1;
                    gesUser.Email = gesUsuario.Email;
                    gesUser.Apellido2 = gesUsuario.Apellido2;
                    gesUser.IsoIdioma = gesUsuario.IsoIdioma;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
               
                
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message, ex);
            }
        }

        public bool DeleteGesUsuario (int id)
        {
            try
            {
                var gesUser = context.Gestionusuarios.FirstOrDefault(g => g.IdUsuario == id);
                if(gesUser != null)
                {
                    context.Remove(gesUser);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                
               
            }
            catch( Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int UpdateContrasenyaGesUsuario (int idAdmin, string contrasenya)
        {
                Encrypt encrypt = new Encrypt();
            try
            {
                var gesUser = context.Gestionusuarios.FirstOrDefault(g => g.IdUsuario == idAdmin);
                if(gesUser == null)
                {
                    return -1;
                }
                else if(gesUser.Contrasenya == encrypt.GetMD5Hash(contrasenya))
                {
                    return -2;
                }
                else
                {
                    gesUser.Contrasenya = encrypt.GetMD5Hash(contrasenya);
                    context.SaveChanges();
                    return 1;
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public Gestionusuario GetLoginManagment(string inicio, string contrasenya)
        {
            Gestionusuario gesUser = new Gestionusuario();
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("GetLoginManagment", conexion);
            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inicio", inicio);
                cmd.Parameters.AddWithValue("@contrasenya", contrasenya);
                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                gesUser.IdUsuario = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
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
            return gesUser;
        }
    }
}
