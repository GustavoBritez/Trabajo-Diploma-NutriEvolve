using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;

namespace DAL
{
    public class Conexion
    {
        private static string? _cadenaConexionEstatica;
        private readonly string _cadenaConexion;
        private const int TimeoutSegundos = 30;

        public Conexion()
        {
            _cadenaConexion = ObtenerCadenaConexion();
        }

        public Conexion(string cadenaPersonalizada)
        {
            _cadenaConexion = !string.IsNullOrWhiteSpace(cadenaPersonalizada)
                ? NormalizarCadena(cadenaPersonalizada)
                : ObtenerCadenaConexion();
        }

        public string CadenaConexion => _cadenaConexion;

        public static void EstablecerCadenaConexion(string nuevaCadena)
        {
            if (!string.IsNullOrWhiteSpace(nuevaCadena))
            {
                _cadenaConexionEstatica = NormalizarCadena(nuevaCadena);
            }
        }

        public static string ObtenerCadenaConexion()
        {
            if (!string.IsNullOrWhiteSpace(_cadenaConexionEstatica))
            {
                return _cadenaConexionEstatica;
            }

            // 1. Intentar leer desde ConfigurationManager (App.config / *.dll.config)
            try
            {
                var connSetting = ConfigurationManager.ConnectionStrings["MiConexionDB"]
                               ?? ConfigurationManager.ConnectionStrings["ING"];
                if (connSetting != null && !string.IsNullOrWhiteSpace(connSetting.ConnectionString))
                {
                    _cadenaConexionEstatica = NormalizarCadena(connSetting.ConnectionString);
                    return _cadenaConexionEstatica;
                }
            }
            catch { }

            // 2. Intentar leer desde un archivo de texto plano local 'db_connection.txt' en la carpeta de la app
            try
            {
                string pathTxt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_connection.txt");
                if (File.Exists(pathTxt))
                {
                    string contenido = File.ReadAllText(pathTxt).Trim();
                    if (!string.IsNullOrWhiteSpace(contenido))
                    {
                        _cadenaConexionEstatica = NormalizarCadena(contenido);
                        return _cadenaConexionEstatica;
                    }
                }
            }
            catch { }

            // 3. Fallback inteligente: Auto-detectar instancia disponible de SQL Server en la máquina
            _cadenaConexionEstatica = DetectarInstanciaDisponible();
            return _cadenaConexionEstatica;
        }

        public static string ObtenerCadenaMaster()
        {
            var builder = new SqlConnectionStringBuilder(ObtenerCadenaConexion())
            {
                InitialCatalog = "master"
            };
            return builder.ConnectionString;
        }

        private static string NormalizarCadena(string cadena)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(cadena);
                builder.TrustServerCertificate = true;
                if (builder.ConnectTimeout < 5)
                {
                    builder.ConnectTimeout = 30;
                }
                return builder.ConnectionString;
            }
            catch
            {
                return cadena;
            }
        }

        private static string DetectarInstanciaDisponible()
        {
            string[] instancias = new[]
            {
                ".",
                @".\SQLEXPRESS",
                @"localhost",
                @"(LocalDB)\MSSQLLocalDB"
            };

            foreach (var inst in instancias)
            {
                try
                {
                    string connStr = $"Data Source={inst};Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;Connect Timeout=2";
                    using (var cn = new SqlConnection(connStr))
                    {
                        cn.Open();
                        return NormalizarCadena($"Data Source={inst};Initial Catalog=ING;Integrated Security=True;Trust Server Certificate=True;Connect Timeout=30");
                    }
                }
                catch
                {
                    // Continuar probando siguiente instancia
                }
            }

            return "Data Source=.;Initial Catalog=ING;Integrated Security=True;Trust Server Certificate=True;Connect Timeout=30";
        }

        /// <summary>
        /// Crea una nueva conexión lista para abrirse y administrarse mediante el pool de ADO.NET.
        /// </summary>
        public SqlConnection CrearConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }

        /// <summary>
        /// Ejecuta una acción dentro de una transacción SQL explícita con commit y rollback automáticos.
        /// </summary>
        public void ExecuteTransaction(Action<SqlTransaction> accion)
        {
            if (accion == null) throw new ArgumentNullException(nameof(accion));

            using (SqlConnection cn = CrearConexion())
            {
                cn.Open();
                using (SqlTransaction transaccion = cn.BeginTransaction())
                {
                    try
                    {
                        accion(transaccion);
                        transaccion.Commit();
                    }
                    catch
                    {
                        try { transaccion.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Ejecuta una función con retorno dentro de una transacción SQL explícita con commit y rollback automáticos.
        /// </summary>
        public T ExecuteTransaction<T>(Func<SqlTransaction, T> funcion)
        {
            if (funcion == null) throw new ArgumentNullException(nameof(funcion));

            using (SqlConnection cn = CrearConexion())
            {
                cn.Open();
                using (SqlTransaction transaccion = cn.BeginTransaction())
                {
                    try
                    {
                        T resultado = funcion(transaccion);
                        transaccion.Commit();
                        return resultado;
                    }
                    catch
                    {
                        try { transaccion.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = CrearConexion())
            {
                cn.Open();
                using (SqlCommand cmd = CrearComando(query, cn, null, parametros))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int ExecuteNonQueryTran(string query, SqlTransaction transaccion, params SqlParameter[] parametros)
        {
            if (transaccion == null || transaccion.Connection == null)
            {
                return ExecuteNonQuery(query, parametros);
            }

            using (SqlCommand cmd = CrearComando(query, transaccion.Connection, transaccion, parametros))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteReader(string query, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = CrearConexion())
            {
                cn.Open();
                using (SqlCommand cmd = CrearComando(query, cn, null, parametros))
                {
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public DataTable ExecuteReaderTran(string query, SqlTransaction transaccion, params SqlParameter[] parametros)
        {
            if (transaccion == null || transaccion.Connection == null)
            {
                return ExecuteReader(query, parametros);
            }

            using (SqlCommand cmd = CrearComando(query, transaccion.Connection, transaccion, parametros))
            {
                DataTable dt = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
                return dt;
            }
        }

        public object? ExecuteScalar(string query, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = CrearConexion())
            {
                cn.Open();
                using (SqlCommand cmd = CrearComando(query, cn, null, parametros))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }

        public object? ExecuteScalarTran(string query, SqlTransaction transaccion, params SqlParameter[] parametros)
        {
            if (transaccion == null || transaccion.Connection == null)
            {
                return ExecuteScalar(query, parametros);
            }

            using (SqlCommand cmd = CrearComando(query, transaccion.Connection, transaccion, parametros))
            {
                return cmd.ExecuteScalar();
            }
        }

        public void ExecuteNonQueryMaster(string query, params SqlParameter[] parametros)
        {
            var builder = new SqlConnectionStringBuilder(_cadenaConexion)
            {
                InitialCatalog = "master"
            };

            using (SqlConnection cn = new SqlConnection(builder.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = CrearComando(query, cn, null, parametros))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static SqlCommand CrearComando(string query, SqlConnection cn, SqlTransaction? tran, SqlParameter[]? parametros)
        {
            SqlCommand cmd = new SqlCommand(query, cn, tran)
            {
                CommandType = CommandType.Text,
                CommandTimeout = TimeoutSegundos
            };

            if (parametros != null && parametros.Length > 0)
            {
                foreach (var p in parametros)
                {
                    if (p != null)
                    {
                        var clone = new SqlParameter(p.ParameterName, p.SqlDbType)
                        {
                            Value = p.Value ?? DBNull.Value,
                            Direction = p.Direction
                        };
                        cmd.Parameters.Add(clone);
                    }
                }
            }

            return cmd;
        }
    }
}
