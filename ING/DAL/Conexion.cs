using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer;
using System.Configuration;

namespace DAL
{
    internal class Conexion
    {
        private readonly string _cadenaConexion;
        private const int time = 30;
        private SqlConnection conexion;

        public Conexion()
        {
            _cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexionDB"].ConnectionString;
            conexion = new SqlConnection(_cadenaConexion);
        }

        public bool AbrirConexion()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    conexion.Open();
                    Console.WriteLine("Conexion Abierta Exitosamente");
                    return true;
                }
                return true;
            }
            catch ( SqlException ex)
            {
                throw new Exception($"Error al abrir la conexión: {ex.Message}");
            }
        }

        public bool CerrarConexion()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Open)
                {
                    conexion.Close();
                    Console.WriteLine("Conexion Cerrada Exitosamente");
                    return true;
                }
                return true;
            }
            catch( SqlException ex )
            {
                Console.Write($"Error al cerrar la conexion {ex.Message}");
                return false;
            }
        }

        public void ExecuteNonQuery(string stringQuery, params SqlParameter[] parametros)
        {
            try
            {
                AbrirConexion();
                using (SqlCommand comando = new SqlCommand(stringQuery, conexion))
                {
                    comando.CommandType = CommandType.Text;
                    comando.CommandTimeout = time;
                    
                    if (parametros != null && parametros.Length > 0)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    
                    comando.ExecuteNonQuery();
                    Console.WriteLine("Comando ejecutado exitosamente.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al ejecutar el comando: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
        }

        public DataTable ExecuteReader(string stringQuery, params SqlParameter[] parametros)
        {
            DataTable dtResultados = new DataTable();

            try
            {
                AbrirConexion();
                using (SqlCommand comando = new SqlCommand(stringQuery, conexion))
                {
                    comando.CommandType = CommandType.Text;
                    comando.CommandTimeout = time;
                    
                    if (parametros != null && parametros.Length > 0)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    
                    using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                    {
                        adaptador.Fill(dtResultados);
                    }
                }
                Console.WriteLine("Consulta ejecutada exitosamente.");
                return dtResultados;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al ejecutar la consulta: {ex.Message}");
                try
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_db.txt"), $"Error crítico de base de datos:\n{ex.Message}");
                }
                catch { }
                return dtResultados;
            }
            finally
            {
                CerrarConexion();
            }
        }
        public void ExecuteNonQueryMaster(string stringQuery, params SqlParameter[] parametros)
        {
            const string conexionMaster =
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection cn = new SqlConnection(conexionMaster))
                {
                    cn.Open();

                    using (SqlCommand comando = new SqlCommand(stringQuery, cn))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.CommandTimeout = time;

                        if (parametros != null && parametros.Length > 0)
                        {
                            comando.Parameters.AddRange(parametros);
                        }

                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al ejecutar el comando sobre master: {ex.Message}");
                throw;
            }
        }
    }
}
