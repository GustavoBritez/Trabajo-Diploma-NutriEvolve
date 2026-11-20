using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL
{
    public class BackupDAL
    {
        private readonly Conexion conexion = new Conexion();

        private string ObtenerNombreBaseDatos()
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(conexion.CadenaConexion);
                if (!string.IsNullOrWhiteSpace(builder.InitialCatalog))
                {
                    return builder.InitialCatalog;
                }
            }
            catch { }
            return "ING";
        }

        private static bool EsErrorDePermisoSO(SqlException ex)
        {
            return ex.Number == 3201 // Cannot open backup device
                || ex.Message.IndexOf("Operating system error 5", StringComparison.OrdinalIgnoreCase) >= 0
                || ex.Message.IndexOf("Access is denied", StringComparison.OrdinalIgnoreCase) >= 0
                || ex.Message.IndexOf("Acceso denegado", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string ObtenerCarpetaStagingPublica()
        {
            string publicDocs = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            if (!string.IsNullOrWhiteSpace(publicDocs) && Directory.Exists(publicDocs))
            {
                return publicDocs;
            }

            string? publicDir = Environment.GetEnvironmentVariable("PUBLIC") ?? @"C:\Users\Public";
            if (Directory.Exists(publicDir))
            {
                return publicDir;
            }

            return Path.GetTempPath();
        }

        private void EjecutarComandoBackupDirecto(string dbName, string ruta)
        {
            string masterConnStr = Conexion.ObtenerCadenaMaster();
            using var cn = new SqlConnection(masterConnStr);
            cn.Open();
            string query = $"BACKUP DATABASE [{dbName}] TO DISK = @ruta WITH INIT";
            using var cmd = new SqlCommand(query, cn)
            {
                CommandTimeout = 300
            };
            cmd.Parameters.AddWithValue("@ruta", ruta);
            cmd.ExecuteNonQuery();
        }

        private void ProbarLecturaBackup(string ruta)
        {
            string masterConnStr = Conexion.ObtenerCadenaMaster();
            using var cn = new SqlConnection(masterConnStr);
            cn.Open();
            using var cmd = new SqlCommand("RESTORE FILELISTONLY FROM DISK = @ruta", cn)
            {
                CommandTimeout = 60
            };
            cmd.Parameters.AddWithValue("@ruta", ruta);
            using var reader = cmd.ExecuteReader();
        }

        private class BackupFileItem
        {
            public string LogicalName { get; set; } = string.Empty;
            public string PhysicalName { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
        }

        private List<BackupFileItem> ObtenerArchivosLogicosDelBackup(string ruta)
        {
            var lista = new List<BackupFileItem>();
            string masterConnStr = Conexion.ObtenerCadenaMaster();
            using var cn = new SqlConnection(masterConnStr);
            cn.Open();
            using var cmd = new SqlCommand("RESTORE FILELISTONLY FROM DISK = @ruta", cn)
            {
                CommandTimeout = 120
            };
            cmd.Parameters.AddWithValue("@ruta", ruta);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new BackupFileItem
                {
                    LogicalName = reader["LogicalName"]?.ToString() ?? "",
                    PhysicalName = reader["PhysicalName"]?.ToString() ?? "",
                    Type = reader["Type"]?.ToString() ?? ""
                });
            }
            return lista;
        }

        private (string? mdf, string? ldf) ObtenerRutasFisicasDestino(string dbName)
        {
            try
            {
                string masterConnStr = Conexion.ObtenerCadenaMaster();
                using var cn = new SqlConnection(masterConnStr);
                cn.Open();

                // 1. Intentar obtener las rutas actuales de la base de datos si ya existe
                string queryFiles = "SELECT physical_name, type_desc FROM sys.master_files WHERE database_id = DB_ID(@dbName)";
                using (var cmd = new SqlCommand(queryFiles, cn))
                {
                    cmd.Parameters.AddWithValue("@dbName", dbName);
                    using var rdr = cmd.ExecuteReader();
                    string? mdf = null;
                    string? ldf = null;
                    while (rdr.Read())
                    {
                        string tipo = rdr["type_desc"]?.ToString() ?? "";
                        string rutaFisica = rdr["physical_name"]?.ToString() ?? "";
                        if (tipo.Equals("ROWS", StringComparison.OrdinalIgnoreCase)) mdf = rutaFisica;
                        else if (tipo.Equals("LOG", StringComparison.OrdinalIgnoreCase)) ldf = rutaFisica;
                    }
                    if (!string.IsNullOrEmpty(mdf) && !string.IsNullOrEmpty(ldf))
                    {
                        return (mdf, ldf);
                    }
                }

                // 2. Si no existe, obtener rutas por defecto de la instancia
                string queryDefaults = @"
                    SELECT 
                        CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS NVARCHAR(512)) AS DataPath,
                        CAST(SERVERPROPERTY('InstanceDefaultLogPath') AS NVARCHAR(512)) AS LogPath";
                using (var cmdDef = new SqlCommand(queryDefaults, cn))
                {
                    using var rdr = cmdDef.ExecuteReader();
                    if (rdr.Read())
                    {
                        string dataPath = rdr["DataPath"]?.ToString() ?? "";
                        string logPath = rdr["LogPath"]?.ToString() ?? "";
                        if (!string.IsNullOrWhiteSpace(dataPath) && !string.IsNullOrWhiteSpace(logPath))
                        {
                            return (Path.Combine(dataPath, $"{dbName}.mdf"), Path.Combine(logPath, $"{dbName}_log.ldf"));
                        }
                    }
                }
            }
            catch { }
            return (null, null);
        }

        public void CrearBackup(string ruta)
        {
            string dbName = ObtenerNombreBaseDatos();
            string rutaDestinoFinal = Path.GetFullPath(ruta);
            string? dirDestino = Path.GetDirectoryName(rutaDestinoFinal);
            if (!string.IsNullOrEmpty(dirDestino) && !Directory.Exists(dirDestino))
            {
                Directory.CreateDirectory(dirDestino);
            }

            // Intento 1: Backup directo a la ruta seleccionada por el usuario
            try
            {
                EjecutarComandoBackupDirecto(dbName, rutaDestinoFinal);
                return;
            }
            catch (SqlException ex) when (EsErrorDePermisoSO(ex))
            {
                // Error de SO 5 (Acceso denegado al servicio SQL Server en carpetas privadas del usuario)
                // Fallback: Realizar backup en carpeta pública accesible para el servicio SQL Server
                // y luego mover el archivo a la ruta deseada con los permisos del usuario de la aplicación.
                string carpetaStaging = ObtenerCarpetaStagingPublica();
                string archivoTemporal = Path.Combine(carpetaStaging, $"BackupING_Temp_{Guid.NewGuid():N}.bak");

                try
                {
                    EjecutarComandoBackupDirecto(dbName, archivoTemporal);
                    File.Copy(archivoTemporal, rutaDestinoFinal, true);
                }
                finally
                {
                    try { if (File.Exists(archivoTemporal)) File.Delete(archivoTemporal); } catch { }
                }
            }
        }

        public void RestaurarBackup(string ruta)
        {
            string dbName = ObtenerNombreBaseDatos();
            string rutaOrigen = Path.GetFullPath(ruta);

            if (!File.Exists(rutaOrigen))
            {
                throw new FileNotFoundException($"El archivo de copia de seguridad no existe en la ruta especificada: {rutaOrigen}", rutaOrigen);
            }

            string rutaBakParaSql = rutaOrigen;
            string? archivoTemporalStaging = null;

            try
            {
                SqlConnection.ClearAllPools();

                // Verificar si SQL Server puede leer directamente el archivo .bak
                // Si da error de permisos (OS error 5), lo copiamos a una carpeta pública de staging
                try
                {
                    ProbarLecturaBackup(rutaOrigen);
                }
                catch (SqlException ex) when (EsErrorDePermisoSO(ex))
                {
                    string carpetaStaging = ObtenerCarpetaStagingPublica();
                    archivoTemporalStaging = Path.Combine(carpetaStaging, $"RestoreING_Temp_{Guid.NewGuid():N}.bak");
                    File.Copy(rutaOrigen, archivoTemporalStaging, true);
                    rutaBakParaSql = archivoTemporalStaging;
                }

                // Obtener nombres lógicos de los archivos dentro del backup mediante RESTORE FILELISTONLY
                var archivosLogicos = ObtenerArchivosLogicosDelBackup(rutaBakParaSql);

                // Obtener rutas de destino físico para .mdf y .ldf en la instancia actual de SQL Server
                var (targetMdf, targetLdf) = ObtenerRutasFisicasDestino(dbName);

                // Construir cláusula WITH MOVE si tenemos los nombres lógicos y rutas físicas
                StringBuilder sbMove = new StringBuilder();
                if (archivosLogicos.Count > 0 && !string.IsNullOrWhiteSpace(targetMdf) && !string.IsNullOrWhiteSpace(targetLdf))
                {
                    var dataFile = archivosLogicos.FirstOrDefault(f => f.Type.Equals("D", StringComparison.OrdinalIgnoreCase));
                    var logFile = archivosLogicos.FirstOrDefault(f => f.Type.Equals("L", StringComparison.OrdinalIgnoreCase));

                    if (dataFile != null && logFile != null)
                    {
                        sbMove.Append($", MOVE '{dataFile.LogicalName}' TO '{targetMdf.Replace("'", "''")}'");
                        sbMove.Append($", MOVE '{logFile.LogicalName}' TO '{targetLdf.Replace("'", "''")}'");
                    }
                }

                string masterConnStr = Conexion.ObtenerCadenaMaster();
                using (var cn = new SqlConnection(masterConnStr))
                {
                    cn.Open();

                    // 1. Desconectar usuarios y poner en SINGLE_USER
                    using (var cmd = new SqlCommand($"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", cn))
                    {
                        cmd.CommandTimeout = 300;
                        try { cmd.ExecuteNonQuery(); } catch { }
                    }

                    try
                    {
                        // 2. Ejecutar RESTORE con REPLACE y MOVE
                        string restoreSql = $"RESTORE DATABASE [{dbName}] FROM DISK = @ruta WITH REPLACE{sbMove};";
                        using (var cmdRestore = new SqlCommand(restoreSql, cn))
                        {
                            cmdRestore.CommandTimeout = 300;
                            cmdRestore.Parameters.AddWithValue("@ruta", rutaBakParaSql);
                            cmdRestore.ExecuteNonQuery();
                        }
                    }
                    finally
                    {
                        // 3. Garantizar siempre volver a MULTI_USER
                        try
                        {
                            using (var cmdMulti = new SqlCommand($"ALTER DATABASE [{dbName}] SET MULTI_USER;", cn))
                            {
                                cmdMulti.CommandTimeout = 300;
                                cmdMulti.ExecuteNonQuery();
                            }
                        }
                        catch { }
                    }
                }

                SqlConnection.ClearAllPools();
            }
            finally
            {
                if (archivoTemporalStaging != null && File.Exists(archivoTemporalStaging))
                {
                    try { File.Delete(archivoTemporalStaging); } catch { }
                }
            }
        }
    }
}
