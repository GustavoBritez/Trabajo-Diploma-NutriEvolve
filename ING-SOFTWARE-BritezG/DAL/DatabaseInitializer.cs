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
            if (!File.Exists(scriptPath))
            {
                string altPath = Path.Combine(baseDir, "..", "..", "..", scriptFileName);
                if (File.Exists(altPath)) scriptPath = Path.GetFullPath(altPath);
            }
            string errorLogPath = Path.Combine(baseDir, "error_db.txt");

            // Connection strings dinámicas obtenidas desde DAL.Conexion
            string masterConnString = Conexion.ObtenerCadenaMaster();
            string dbConnString = Conexion.ObtenerCadenaConexion();

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

                            // 2. Read the SQL script with UTF8 encoding
                            string scriptContent = File.ReadAllText(scriptPath, System.Text.Encoding.UTF8);

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
                        else
                        {
                            // If the database already exists, ensure the bitacora table and triggers are created/updated
                            AsegurarEstructuraBitacora(dbConnString);
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

        private static void AsegurarEstructuraBitacora(string dbConnString)
        {
            try
            {
                using (SqlConnection dbConn = new SqlConnection(dbConnString))
                {
                    dbConn.Open();

                    // 1. Asegurar tabla ConsultasNutricionales_Cambios_DNI101 y columna Act
                    string sqlTabla = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ConsultasNutricionales_Cambios_DNI101' AND xtype='U')
                        BEGIN
                            CREATE TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101](
                                [IdCambio] INT IDENTITY(1,1) PRIMARY KEY,
                                [IdConsulta_DNI101] INT NOT NULL,
                                [IdPaciente_DNI101] INT NOT NULL,
                                [DniNutricionista_DNI101] INT NOT NULL,
                                [FechaControl_DNI101] DATETIME2(7) NOT NULL,
                                [EdadMeses_DNI101] INT NOT NULL,
                                [TipoLactancia_DNI101] VARCHAR(50) NULL,
                                [AlimentacionComplementaria_DNI101] VARCHAR(255) NULL,
                                [Alergias_DNI101] VARCHAR(255) NULL,
                                [AntecedentesFamiliares_DNI101] VARCHAR(255) NULL,
                                [Observaciones_DNI101] VARCHAR(MAX) NULL,
                                [FechaCambio] DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
                                [UsuarioDni] INT NOT NULL DEFAULT 11111111,
                                [TipoOperacion] VARCHAR(20) NOT NULL,
                                [Act] BIT NOT NULL CONSTRAINT DF_ConsultasCambios_Act DEFAULT 1
                            ) ON [PRIMARY];
                        END
                        ELSE
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ConsultasNutricionales_Cambios_DNI101]') AND name = 'Act')
                            BEGIN
                                ALTER TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101] ADD [Act] BIT NOT NULL CONSTRAINT DF_ConsultasCambios_Act DEFAULT 1;
                            END
                        END";

                    using (SqlCommand cmd = new SqlCommand(sqlTabla, dbConn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Asegurar Trigger trg_ConsultasNutricionales_BitacoraCambios
                    string sqlTrigger = @"
                        CREATE OR ALTER TRIGGER [dbo].[trg_ConsultasNutricionales_BitacoraCambios]
                        ON [dbo].[ConsultasNutricionales_DNI101]
                        AFTER INSERT, UPDATE, DELETE
                        AS
                        BEGIN
                            SET NOCOUNT ON;

                            IF CAST(SESSION_CONTEXT(N'EsReversion') AS INT) = 1
                            BEGIN
                                RETURN;
                            END

                            IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
                            BEGIN
                                INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
                                    [IdConsulta_DNI101], [IdPaciente_DNI101], [DniNutricionista_DNI101],
                                    [FechaControl_DNI101], [EdadMeses_DNI101], [TipoLactancia_DNI101],
                                    [AlimentacionComplementaria_DNI101], [Alergias_DNI101],
                                    [AntecedentesFamiliares_DNI101], [Observaciones_DNI101],
                                    [FechaCambio], [UsuarioDni], [TipoOperacion], [Act]
                                )
                                SELECT 
                                    i.[IdConsulta_DNI101], i.[IdPaciente_DNI101], i.[DniNutricionista_DNI101],
                                    i.[FechaControl_DNI101], i.[EdadMeses_DNI101], i.[TipoLactancia_DNI101],
                                    i.[AlimentacionComplementaria_DNI101], i.[Alergias_DNI101],
                                    i.[AntecedentesFamiliares_DNI101], i.[Observaciones_DNI101],
                                    SYSDATETIME(), ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
                                    'INSERT', 1
                                FROM inserted i;
                            END
                            ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
                            BEGIN
                                UPDATE [dbo].[ConsultasNutricionales_Cambios_DNI101]
                                SET [Act] = 0
                                WHERE [IdConsulta_DNI101] IN (SELECT [IdConsulta_DNI101] FROM inserted);

                                INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
                                    [IdConsulta_DNI101], [IdPaciente_DNI101], [DniNutricionista_DNI101],
                                    [FechaControl_DNI101], [EdadMeses_DNI101], [TipoLactancia_DNI101],
                                    [AlimentacionComplementaria_DNI101], [Alergias_DNI101],
                                    [AntecedentesFamiliares_DNI101], [Observaciones_DNI101],
                                    [FechaCambio], [UsuarioDni], [TipoOperacion], [Act]
                                )
                                SELECT 
                                    i.[IdConsulta_DNI101], i.[IdPaciente_DNI101], i.[DniNutricionista_DNI101],
                                    i.[FechaControl_DNI101], i.[EdadMeses_DNI101], i.[TipoLactancia_DNI101],
                                    i.[AlimentacionComplementaria_DNI101], i.[Alergias_DNI101],
                                    i.[AntecedentesFamiliares_DNI101], i.[Observaciones_DNI101],
                                    SYSDATETIME(), ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
                                    'UPDATE', 1
                                FROM inserted i;
                            END
                            ELSE IF EXISTS (SELECT 1 FROM deleted)
                            BEGIN
                                UPDATE [dbo].[ConsultasNutricionales_Cambios_DNI101]
                                SET [Act] = 0
                                WHERE [IdConsulta_DNI101] IN (SELECT [IdConsulta_DNI101] FROM deleted);

                                INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
                                    [IdConsulta_DNI101], [IdPaciente_DNI101], [DniNutricionista_DNI101],
                                    [FechaControl_DNI101], [EdadMeses_DNI101], [TipoLactancia_DNI101],
                                    [AlimentacionComplementaria_DNI101], [Alergias_DNI101],
                                    [AntecedentesFamiliares_DNI101], [Observaciones_DNI101],
                                    [FechaCambio], [UsuarioDni], [TipoOperacion], [Act]
                                )
                                SELECT 
                                    d.[IdConsulta_DNI101], d.[IdPaciente_DNI101], d.[DniNutricionista_DNI101],
                                    d.[FechaControl_DNI101], d.[EdadMeses_DNI101], d.[TipoLactancia_DNI101],
                                    d.[AlimentacionComplementaria_DNI101], d.[Alergias_DNI101],
                                    d.[AntecedentesFamiliares_DNI101], d.[Observaciones_DNI101],
                                    SYSDATETIME(), ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
                                    'DELETE', 0
                                FROM deleted d;
                            END
                        END";

                    using (SqlCommand cmd = new SqlCommand(sqlTrigger, dbConn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Poblar datos iniciales si la tabla de cambios está vacía
                    string sqlPoblar = @"
                        IF NOT EXISTS (SELECT 1 FROM [dbo].[ConsultasNutricionales_Cambios_DNI101])
                        BEGIN
                            INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
                                [IdConsulta_DNI101], [IdPaciente_DNI101], [DniNutricionista_DNI101],
                                [FechaControl_DNI101], [EdadMeses_DNI101], [TipoLactancia_DNI101],
                                [AlimentacionComplementaria_DNI101], [Alergias_DNI101],
                                [AntecedentesFamiliares_DNI101], [Observaciones_DNI101],
                                [FechaCambio], [UsuarioDni], [TipoOperacion], [Act]
                            )
                            SELECT 
                                c.[IdConsulta_DNI101], c.[IdPaciente_DNI101], c.[DniNutricionista_DNI101],
                                c.[FechaControl_DNI101], c.[EdadMeses_DNI101], c.[TipoLactancia_DNI101],
                                c.[AlimentacionComplementaria_DNI101], c.[Alergias_DNI101],
                                c.[AntecedentesFamiliares_DNI101], c.[Observaciones_DNI101],
                                c.[FechaControl_DNI101], 11111111, 'INSERT', 1
                            FROM [dbo].[ConsultasNutricionales_DNI101] c;
                        END";

                    using (SqlCommand cmd = new SqlCommand(sqlPoblar, dbConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }
    }
}
