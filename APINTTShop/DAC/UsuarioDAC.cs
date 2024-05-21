using APINTTShop.Models.Entities;
using System.Data.SqlClient;
using APINTTShop.BC;
using APINTTShop.Models;
using APINTTShop.Models.Request.UsuarioRequest;
using System.Data;
using Azure.Core;

namespace APINTTShop.DAC
{
    public class UsuarioDAC
    {
        private readonly NttshopContext context = new NttshopContext();

        public Usuario GetUsuario(int id)
        {
            Usuario result = new Usuario();

            try
            {
                result = context.Usuarios.Find(id);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        public Usuario GetidUser(string inicio)
        {
            Usuario result = new Usuario();
            try
            {
                result = context.Usuarios.FirstOrDefault(u => u.Inicio == inicio);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }


        public List<Usuario> GetAllUsuario()
        {
            List <Usuario> result = new List<Usuario>();
            try
            {
               result = context.Usuarios.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        public bool InsertUsuario (Usuario usuario)
        {
            try
            {
                Encrypt encrypt = new Encrypt();
                usuario.Contrasenya= encrypt.GetMD5Hash(usuario.Contrasenya);
                if(context.Usuarios.Any(u => u.Email == usuario.Email) || context.Usuarios.Any(u => u.Inicio == usuario.Inicio))
                {
                    return false;
                }
                else
                {
                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool UpdateUsuario (Usuario usuario)
        {
            try
            {
                var user = context.Usuarios.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
                if(user != null)
                {
                    //user.Inicio = usuario.Inicio;
                    user.Nombre = usuario.Nombre;
                    user.Apellido1 = usuario.Apellido1;
                    user.Apellido2 = usuario.Apellido2;
                    user.Direccion = usuario.Direccion;
                    user.Provincia = usuario.Provincia;
                    user.Ciudad = usuario.Ciudad;
                    user.CodigoPostal = usuario.CodigoPostal;
                    user.Telefono = usuario.Telefono;
                    //user.Email = usuario.Email;
                    //user.IsoIdioma = usuario.IsoIdioma;
                    user.IdRate = usuario.IdRate;

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
                throw new Exception(ex.Message, ex);
            }

        }

        public bool DeleteUsuario(int id)
        {
            try
            {
                var usuarioEliminar = context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if(usuarioEliminar == null)
                {
                    return false;
                }
                else
                {
                    context.Usuarios.Remove(usuarioEliminar);
                    context.SaveChanges();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public int UpdateContrasenya (int idUsuario, string contrasenya)
        {
            Encrypt encrypt = new Encrypt();
            try
            {
                var user = context.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
                
                if (user == null)
                {
                    return -1;
                }
                else if (user.Contrasenya == encrypt.GetMD5Hash(contrasenya))
                {
                    return -2;
                }
                else
                {
                    user.Contrasenya = encrypt.GetMD5Hash(contrasenya);
                    context.SaveChanges();
                    return 1;
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        public bool GetLogin (string inicio, string contrasenya)
        {
            SqlConnection conexion = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand cmd = new SqlCommand("GetLogin", conexion);
            try
            {
                conexion.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inicio", inicio);
                cmd.Parameters.AddWithValue("@contrasenya", contrasenya);
                cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
                if(resultado == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
    }
}

