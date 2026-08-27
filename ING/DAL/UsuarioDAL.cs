using BE;
using Services;
using Microsoft.Data.SqlClient;
using System.Data;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class UsuarioDAL
    {
        private readonly Conexion conexion = new();
        private const string TABLA_USUARIOS = "Usuarios";
        private readonly string Modulo = "UsuarioDAL";

        public void CrearUsuario(UsuarioBE usuario)
        {
            try
            {
                // Agregamos DV al INSERT
                string query = $@"INSERT INTO {TABLA_USUARIOS} (DNI, NombreDeUsuario, Nombre, Apellido, Contraseña, ID_Perfil, Bloqueado, Estado, Idioma, DV) 
                                  VALUES (@dni, @nombreDeUsuario, @nombre, @apellido, @contraseña, @idPerfil, @bloqueado, @estado, @idioma, @dv)";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@dni", usuario._Dni),
                    new SqlParameter("@nombreDeUsuario", usuario._NombreDeUsuario),
                    new SqlParameter("@nombre", usuario._Nombre),
                    new SqlParameter("@apellido", usuario._Apellido),
                    new SqlParameter("@contraseña", usuario._Contraseña),
                    new SqlParameter("@idPerfil", usuario._IdPerfil),
                    new SqlParameter("@bloqueado", usuario._Bloqueado),
                    new SqlParameter("@estado", usuario._Estado),
                    new SqlParameter("@idioma", usuario._Idioma),
                    new SqlParameter("@dv", (object)usuario.DV ?? DBNull.Value) // Pasamos el DV
                };

                conexion.ExecuteNonQuery(query, parametros);
                Console.WriteLine($"Usuario {usuario._NombreDeUsuario} registrado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                throw;
            }
        }

        public void CambiarContraseña(UsuarioBE usuario)
        {
            try
            {
                // Agregamos DV al UPDATE
                string query = $@"UPDATE {TABLA_USUARIOS} 
                                  SET Nombre = @nombre, Apellido = @apellido, NombreDeUsuario = @nombredeusuario, Contraseña = @contraseña, 
                                      ID_Perfil = @idPerfil, Bloqueado = @bloqueado , Estado = @estado, Idioma=@idioma, DV = @dv
                                  WHERE DNI = @dni";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@nombre", usuario._Nombre),
                    new SqlParameter("@apellido", usuario._Apellido),
                    new SqlParameter("@contraseña", usuario._Contraseña),
                    new SqlParameter("@nombredeusuario", usuario._NombreDeUsuario),
                    new SqlParameter("@idPerfil", usuario._IdPerfil),
                    new SqlParameter("@bloqueado", usuario._Bloqueado),
                    new SqlParameter("@dni", usuario._Dni),
                    new SqlParameter("@estado", usuario._Estado),
                    new SqlParameter("@idioma", usuario._Idioma),
                    new SqlParameter("@dv", (object)usuario.DV ?? DBNull.Value)
                };

                conexion.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: No se cambio la contraseña ");
                throw;
            }
        }

        public UsuarioBE ObtenerUsuario(string nombreDeUsuario)
        {
            try
            {
                // Sumamos la columna DV
                string query = $@"SELECT DNI, NombreDeUsuario, Nombre, Apellido, Contraseña, ID_Perfil, Bloqueado, Estado, Idioma, DV
                                  FROM {TABLA_USUARIOS} 
                                  WHERE NombreDeUsuario = @nombreDeUsuario";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@nombreDeUsuario", nombreDeUsuario)
                };

                DataTable dt = conexion.ExecuteReader(query, parametros);

                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                UsuarioBE usuarioEncontrado = new UsuarioBE(
                    dt.Rows[0]["Nombre"].ToString(),
                    dt.Rows[0]["Apellido"].ToString(),
                    Convert.ToInt32(dt.Rows[0]["DNI"]),
                    dt.Rows[0]["NombreDeUsuario"].ToString(),
                    dt.Rows[0]["Contraseña"].ToString(),
                    Convert.ToInt32(dt.Rows[0]["ID_Perfil"]),
                    Convert.ToBoolean(dt.Rows[0]["Bloqueado"]),
                    Convert.ToBoolean(dt.Rows[0]["Estado"]),
                    dt.Rows[0]["Idioma"].ToString()
                );

                // Seteamos la propiedad DV por fuera del constructor
                usuarioEncontrado.DV = dt.Rows[0]["DV"] != DBNull.Value ? dt.Rows[0]["DV"].ToString() : "";

                return usuarioEncontrado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario por nombre: {ex.Message}");
                return null;
            }
        }

        public UsuarioBE BuscarUsuario(int dni)
        {
            try
            {
                // Sumamos la columna DV
                string query = $@"SELECT DNI, NombreDeUsuario, Nombre, Apellido, Contraseña, ID_Perfil, Bloqueado, Estado, Idioma, DV
                                  FROM {TABLA_USUARIOS} 
                                  WHERE DNI = @dni";

                SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@dni", dni) };
                DataTable dt = conexion.ExecuteReader(query, parametros);

                if (dt.Rows.Count == 0) return null;

                UsuarioBE usuario = new UsuarioBE(
                    dt.Rows[0]["Nombre"].ToString(),
                    dt.Rows[0]["Apellido"].ToString(),
                    Convert.ToInt32(dt.Rows[0]["DNI"]),
                    dt.Rows[0]["NombreDeUsuario"].ToString(),
                    dt.Rows[0]["Contraseña"].ToString(),
                    Convert.ToInt32(dt.Rows[0]["ID_Perfil"]),
                    Convert.ToBoolean(dt.Rows[0]["Bloqueado"]),
                    Convert.ToBoolean(dt.Rows[0]["Estado"]),
                    dt.Rows[0]["Idioma"].ToString()
                );

                usuario.DV = dt.Rows[0]["DV"] != DBNull.Value ? dt.Rows[0]["DV"].ToString() : "";

                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario: {ex.Message}");
                return null;
            }
        }

        public void ModificarUsuario(UsuarioBE usuario)
        {
            try
            {
                // Agregamos DV al UPDATE
                string query = $@"UPDATE {TABLA_USUARIOS} 
                                  SET Nombre = @nombre, Apellido = @apellido, NombreDeUsuario = @nombredeusuario, 
                                      Contraseña = @contraseña, ID_Perfil = @idPerfil, Bloqueado = @bloqueado, 
                                      Estado = @estado, Idioma=@idioma, DV = @dv
                                  WHERE DNI = @dni";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@nombre", usuario._Nombre),
                    new SqlParameter("@apellido", usuario._Apellido),
                    new SqlParameter("@contraseña", usuario._Contraseña),
                    new SqlParameter("@idPerfil", usuario._IdPerfil),
                    new SqlParameter("@nombredeusuario", usuario._NombreDeUsuario),
                    new SqlParameter("@bloqueado", usuario._Bloqueado),
                    new SqlParameter("@dni", usuario._Dni),
                    new SqlParameter("@estado", usuario._Estado),
                    new SqlParameter("@idioma", usuario._Idioma),
                    new SqlParameter("@dv", (object)usuario.DV ?? DBNull.Value)
                };

                conexion.ExecuteNonQuery(query, parametros);
                Console.WriteLine($"Usuario {usuario._NombreDeUsuario} modificado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar usuario: {ex.Message}");
                throw;
            }
        }

        public void CambioEstado(UsuarioBE usuario)
        {
            try
            {
                // Agregamos DV al UPDATE
                string query = $@"UPDATE {TABLA_USUARIOS} 
                                  SET Estado = @estado, DV = @dv
                                  WHERE DNI = @dni";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@estado", usuario._Estado),
                    new SqlParameter("@dni", usuario._Dni),
                    new SqlParameter("@dv", (object)usuario.DV ?? DBNull.Value)
                };

                conexion.ExecuteNonQuery(query, parametros);
                Console.WriteLine($"Usuario {usuario._NombreDeUsuario} Cambio de estado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar de estado, usuario: {ex.Message}");
                throw;
            }
        }

        public List<UsuarioBE> ListaUsuarios()
        {
            List<UsuarioBE> usuarios = new List<UsuarioBE>();

            try
            {
                // Sumamos la columna DV
                string query = $@"SELECT DNI, NombreDeUsuario, Nombre, Apellido, Contraseña, ID_Perfil, Bloqueado, Estado, Idioma, DV
                                  FROM {TABLA_USUARIOS} 
                                  ORDER BY NombreDeUsuario";

                DataTable dt = conexion.ExecuteReader(query);

                foreach (DataRow row in dt.Rows)
                {
                    UsuarioBE usuario = new UsuarioBE(
                        row["Nombre"].ToString(),
                        row["Apellido"].ToString(),
                        Convert.ToInt32(row["DNI"]),
                        row["NombreDeUsuario"].ToString(),
                        row["Contraseña"].ToString(),
                        Convert.ToInt32(row["ID_Perfil"]),
                        Convert.ToBoolean(row["Bloqueado"]),
                        Convert.ToBoolean(row["Estado"]),
                        row["Idioma"].ToString()
                    );

                    usuario.DV = row["DV"] != DBNull.Value ? row["DV"].ToString() : "";

                    usuarios.Add(usuario);
                }

                Console.WriteLine($"Se obtuvieron {usuarios.Count} usuarios.");
                return usuarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar usuarios: {ex.Message}");
                return usuarios;
            }
        }

        public void Desbloquear(UsuarioBE usuario)
        {
            // Agregamos DV al UPDATE
            string query = $@"UPDATE {TABLA_USUARIOS} 
                              SET Nombre = @nombre, Apellido = @apellido, NombreDeUsuario = @nombredeusuario, Contraseña = @contraseña, 
                                  ID_Perfil = @idPerfil, Bloqueado = @bloqueado , Estado = @estado, Idioma = @idioma, DV = @dv
                              WHERE DNI = @dni";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@nombre", usuario._Nombre),
                new SqlParameter("@apellido", usuario._Apellido),
                new SqlParameter("@contraseña", usuario._Contraseña),
                new SqlParameter("@nombredeusuario", usuario._NombreDeUsuario),
                new SqlParameter("@idPerfil", usuario._IdPerfil),
                new SqlParameter("@bloqueado", usuario._Bloqueado),
                new SqlParameter("@dni", usuario._Dni),
                new SqlParameter("@estado", usuario._Estado),
                new SqlParameter("@idioma", usuario._Idioma),
                new SqlParameter("@dv", (object)usuario.DV ?? DBNull.Value)
            };

            conexion.ExecuteNonQuery(query, parametros);
        }

        public void CambiarIdiomaUsuario(UsuarioBE usuario)
        {
            try
            {
                // Agregamos DV al UPDATE
                string query = $@"UPDATE {TABLA_USUARIOS}
                                  SET Idioma = @idioma, DV = @dv
                                  WHERE DNI = @dni";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@idioma", usuario._Idioma),
                    new SqlParameter("@dni", usuario._Dni),
                    new SqlParameter("@dv", (object)usuario.DV ?? DBNull.Value)
                };

                conexion.ExecuteNonQuery(query, parametros);
                Console.WriteLine($"Idioma del usuario {usuario._NombreDeUsuario} actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar idioma del usuario: {ex.Message}");
                throw;
            }
        }
    }
}