using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace UniversalSetup
{
    public static class SqlExecutor
    {
        public static string DetectActiveInstance()
        {
            string[] candidateInstances = new[]
            {
                ".",
                @".\SQLEXPRESS",
                "localhost",
                @"(LocalDB)\MSSQLLocalDB",
                Environment.MachineName,
                $@"{Environment.MachineName}\SQLEXPRESS"
            };

            foreach (var instance in candidateInstances)
            {
                if (TestServerConnection(instance, true, null, null))
                {
                    return instance;
                }
            }

            return ".";
        }

        public static string BuildConnectionString(string server, string database, bool useWindowsAuth, string? user, string? password)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = useWindowsAuth,
                TrustServerCertificate = true,
                ConnectTimeout = 10
            };

            if (!useWindowsAuth)
            {
                builder.UserID = user ?? "sa";
                builder.Password = password ?? "";
            }

            return builder.ConnectionString;
        }

        public static bool TestServerConnection(string server, bool useWindowsAuth, string? user, string? password)
        {
            try
            {
                string masterConn = BuildConnectionString(server, "master", useWindowsAuth, user, password);
                using var conn = new SqlConnection(masterConn);
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DatabaseExists(string server, string database, bool useWindowsAuth, string? user, string? password)
        {
            try
            {
                string masterConn = BuildConnectionString(server, "master", useWindowsAuth, user, password);
                using var conn = new SqlConnection(masterConn);
                conn.Open();
                using var cmd = new SqlCommand("SELECT COUNT(*) FROM sys.databases WHERE name = @dbName", conn);
                cmd.Parameters.AddWithValue("@dbName", database);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        public static void DropDatabase(string server, string database, bool useWindowsAuth, string? user, string? password, Action<string>? logger = null)
        {
            string masterConn = BuildConnectionString(server, "master", useWindowsAuth, user, password);
            using var conn = new SqlConnection(masterConn);
            conn.Open();

            logger?.Invoke($"[SQL] Cerrando conexiones activas y eliminando base de datos '{database}'...");
            string dropSql = $@"
                IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{database}')
                BEGIN
                    ALTER DATABASE [{database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{database}];
                END";

            using var cmd = new SqlCommand(dropSql, conn);
            cmd.ExecuteNonQuery();
            logger?.Invoke($"[SQL] Base de datos '{database}' eliminada correctamente.");
        }

        public static void EnsureDatabaseCreated(string server, string database, bool useWindowsAuth, string? user, string? password, Action<string>? logger = null)
        {
            string masterConn = BuildConnectionString(server, "master", useWindowsAuth, user, password);
            using var conn = new SqlConnection(masterConn);
            conn.Open();

            string createSql = $@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{database}')
                BEGIN
                    CREATE DATABASE [{database}];
                END";

            using var cmd = new SqlCommand(createSql, conn);
            cmd.ExecuteNonQuery();
            logger?.Invoke($"[SQL] Catálogo '{database}' listo en el servidor.");
        }

        public static void ExecuteScript(string server, string database, string scriptPath, bool useWindowsAuth, string? user, string? password, Action<string>? logger = null, bool esReinstalacion = false)
        {
            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"No se encontró el script SQL en la ruta: {scriptPath}");
            }

            logger?.Invoke($"[SQL] Leyendo script SQL maestro: {Path.GetFileName(scriptPath)}...");
            string fullScript = File.ReadAllText(scriptPath);

            EnsureDatabaseCreated(server, database, useWindowsAuth, user, password, logger);

            string targetConn = BuildConnectionString(server, database, useWindowsAuth, user, password);
            using var conn = new SqlConnection(targetConn);
            conn.Open();

            var batches = SplitSqlBatches(fullScript);
            logger?.Invoke($"[SQL] Ejecutando {batches.Count} bloques de comandos SQL...");

            int current = 0;
            int ejecutados = 0;
            int omitidos = 0;
            foreach (var batch in batches)
            {
                current++;
                if (string.IsNullOrWhiteSpace(batch)) continue;

                if (esReinstalacion)
                {
                    if (Regex.IsMatch(batch, @"\b(DELETE\s+FROM|TRUNCATE\s+TABLE|DROP\s+TABLE)\b", RegexOptions.IgnoreCase) ||
                        Regex.IsMatch(batch, @"\bINSERT\s+INTO\s+\[?(Pacientes_DNI101|Paciente_Emails_DNI101|Paciente_Telefonos_DNI101|ConsultasNutricionales_DNI101|PlanesAlimentarios_DNI101|MedicionesAntropometricas_DNI101|DiagnosticosNutricionales_DNI101)\]?", RegexOptions.IgnoreCase))
                    {
                        omitidos++;
                        continue;
                    }
                }

                try
                {
                    using var cmd = new SqlCommand(batch, conn)
                    {
                        CommandTimeout = 180
                    };
                    cmd.ExecuteNonQuery();
                    ejecutados++;
                }
                catch (SqlException ex)
                {
                    logger?.Invoke($"[SQL ADVERTENCIA] Bloque {current}: {ex.Message}");
                }
            }

            if (esReinstalacion)
            {
                logger?.Invoke($"[SQL] Actualización completada: {ejecutados} bloques ejecutados ({omitidos} bloques de prueba/destructivos omitidos para proteger datos existentes).");
            }
            else
            {
                logger?.Invoke("[SQL] Script SQL completado exitosamente.");
            }
        }

        private static List<string> SplitSqlBatches(string script)
        {
            var batches = new List<string>();
            var lines = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var currentBatch = new System.Text.StringBuilder();

            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (Regex.IsMatch(trimmed, @"^GO(\s+.*)?$", RegexOptions.IgnoreCase))
                {
                    string b = currentBatch.ToString().Trim();
                    if (!string.IsNullOrEmpty(b))
                    {
                        batches.Add(b);
                    }
                    currentBatch.Clear();
                }
                else
                {
                    currentBatch.AppendLine(line);
                }
            }

            string finalBatch = currentBatch.ToString().Trim();
            if (!string.IsNullOrEmpty(finalBatch))
            {
                batches.Add(finalBatch);
            }

            return batches;
        }

        public static bool TryRecalculateDigitosVerificadores(string appDirectory, Action<string>? logger = null)
        {
            try
            {
                logger?.Invoke("[DV] Buscando módulo de Dígitos Verificadores en ensamblados de la aplicación...");

                string bllDll = Path.Combine(appDirectory, "BLL.dll");
                if (!File.Exists(bllDll))
                {
                    logger?.Invoke("[DV] No se encontró BLL.dll, se omite sellado de DV.");
                    return false;
                }

                Assembly bllAssembly = Assembly.LoadFrom(bllDll);

                Type? dvType = bllAssembly.GetType("BLL.DigitoVerificadorBLL") 
                            ?? bllAssembly.GetType("BLL.DigitoVerificador")
                            ?? bllAssembly.GetType("BLL.DV_BLL");

                if (dvType != null)
                {
                    var staticMethod = dvType.GetMethod("RecalcularYPersistir", BindingFlags.Public | BindingFlags.Static)
                                    ?? dvType.GetMethod("RecalcularTodosLosDigitos", BindingFlags.Public | BindingFlags.Static)
                                    ?? dvType.GetMethod("RecalcularDV", BindingFlags.Public | BindingFlags.Static)
                                    ?? dvType.GetMethod("RecalcularYPersistirStatic", BindingFlags.Public | BindingFlags.Static);

                    if (staticMethod != null)
                    {
                        logger?.Invoke($"[DV] Ejecutando método estático {dvType.Name}.{staticMethod.Name}()...");
                        staticMethod.Invoke(null, null);
                        logger?.Invoke("[DV] Dígitos Verificadores calculados y sellados con éxito!");
                        return true;
                    }

                    var instanceMethod = dvType.GetMethod("RecalcularYPersistir", BindingFlags.Public | BindingFlags.Instance)
                                      ?? dvType.GetMethod("RecalcularTodosLosDigitos", BindingFlags.Public | BindingFlags.Instance)
                                      ?? dvType.GetMethod("RecalcularDV", BindingFlags.Public | BindingFlags.Instance);

                    if (instanceMethod != null)
                    {
                        logger?.Invoke($"[DV] Instanciando {dvType.Name} y ejecutando {instanceMethod.Name}()...");
                        object? instance = Activator.CreateInstance(dvType);
                        instanceMethod.Invoke(instance, null);
                        logger?.Invoke("[DV] Dígitos Verificadores calculados y sellados con éxito!");
                        return true;
                    }
                }

                logger?.Invoke("[DV] Módulo de DV no implementa método de recálculo estándar, omitiendo.");
                return false;
            }
            catch (Exception ex)
            {
                logger?.Invoke($"[DV AVISO] No se pudo recalcular DV automáticamente: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }
    }
}
