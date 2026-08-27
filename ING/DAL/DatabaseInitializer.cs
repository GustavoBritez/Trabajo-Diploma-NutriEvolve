using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace DAL
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase()
        {
            string dbName = "ING";
            string scriptFileName = "script.sql";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string scriptPath = Path.Combine(baseDir, scriptFileName);
            string errorLogPath = Path.Combine(baseDir, "error_db.txt");

            // Connection strings
            string masterConnString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30";
            string dbConnString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ING;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection conn = new SqlConnection(masterConnString))
                {
                    conn.Open();

                    // Check if the database already exists
                    string checkDbQuery = "SELECT database_id FROM sys.databases WHERE Name = @dbName";
                    using (SqlCommand checkCmd = new SqlCommand(checkDbQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@dbName", dbName);
                        object result = checkCmd.ExecuteScalar();

                        // If it doesn't exist, create it and run script
                        if (result == null)
                        {
                            if (!File.Exists(scriptPath))
                            {
                                File.WriteAllText(errorLogPath, $"Error: No se encontró el archivo de script SQL en: {scriptPath}");
                                return;
                            }

                            // 1. Create the database
                            string createDbQuery = $"CREATE DATABASE [{dbName}]";
                            using (SqlCommand createCmd = new SqlCommand(createDbQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }

                            // Wait a short moment to ensure the DB is fully ready in LocalDB
                            System.Threading.Thread.Sleep(1000);

                            // 2. Read the SQL script
                            string scriptContent = File.ReadAllText(scriptPath);

                            // 3. Split the script by GO statements (case-insensitive, on its own line)
                            string[] batches = Regex.Split(
                                scriptContent, 
                                @"^\s*GO\s*$", 
                                RegexOptions.Multiline | RegexOptions.IgnoreCase
                            );

                            // 4. Connect directly to the new database to execute the batches
                            using (SqlConnection dbConn = new SqlConnection(dbConnString))
                            {
                                dbConn.Open();
                                foreach (string batch in batches)
                                {
                                    string trimmedBatch = batch.Trim();
                                    if (!string.IsNullOrEmpty(trimmedBatch))
                                    {
                                        using (SqlCommand cmd = new SqlCommand(trimmedBatch, dbConn))
                                        {
                                            cmd.CommandTimeout = 300; // Allow enough time for tables creation and inserts
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any technical error to a file so it can be diagnosed
                try
                {
                    File.WriteAllText(errorLogPath, $"Error al inicializar la base de datos desde script.sql:\n{ex.Message}\n{ex.StackTrace}");
                }
                catch { }
            }
        }
    }
}
