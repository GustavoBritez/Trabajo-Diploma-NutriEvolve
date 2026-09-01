using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace DAL
{
    public record ResumenDigitoVerificador(string Tabla, string DVH, string DVV);

    public class DigitoVerificadorDAL
    {
        // Instancia de la clase centralizada de conexiones
        private readonly Conexion _conexion;

        public DigitoVerificadorDAL()
        {
            _conexion = new Conexion();
        }

        public List<(string Schema, string Table)> ObtenerTablasPersistentes()
        {
            const string query = @"SELECT TABLE_SCHEMA, TABLE_NAME
                                   FROM INFORMATION_SCHEMA.TABLES
                                   WHERE TABLE_TYPE = 'BASE TABLE'
                                     AND TABLE_NAME NOT IN ('DV', 'sysdiagrams', '__EFMigrationsHistory')
                                   ORDER BY TABLE_SCHEMA, TABLE_NAME";

            // Usamos ExecuteReader de tu clase Conexion
            DataTable dt = _conexion.ExecuteReader(query);
            List<(string Schema, string Table)> tablas = new();

            foreach (DataRow row in dt.Rows)
            {
                tablas.Add((row["TABLE_SCHEMA"].ToString() ?? string.Empty, row["TABLE_NAME"].ToString() ?? string.Empty));
            }

            return tablas;
        }

        public DataTable ObtenerDatosTabla(string schema, string table)
        {
            string query = $"SELECT * FROM [{schema}].[{table}]";
            return _conexion.ExecuteReader(query);
        }

        public bool ExisteTablaDV()
        {
            const string query = @"SELECT CAST(CASE WHEN OBJECT_ID(N'dbo.DV', N'U') IS NULL THEN 0 ELSE 1 END AS BIT) AS Existe";

            DataTable dt = _conexion.ExecuteReader(query);

            if (dt.Rows.Count > 0 && dt.Rows[0]["Existe"] != DBNull.Value)
            {
                return Convert.ToBoolean(dt.Rows[0]["Existe"]);
            }
            return false;
        }

        public List<ResumenDigitoVerificador> ObtenerResumenPersistido()
        {
            const string query = @"SELECT NombreTabla, DVH, DVV
                                   FROM dbo.DV
                                   ORDER BY NombreTabla";

            DataTable dt = _conexion.ExecuteReader(query);
            List<ResumenDigitoVerificador> resumen = new();

            foreach (DataRow row in dt.Rows)
            {
                resumen.Add(new ResumenDigitoVerificador(
                    row["NombreTabla"].ToString() ?? string.Empty,
                    row["DVH"].ToString() ?? string.Empty,
                    row["DVV"].ToString() ?? string.Empty));
            }

            return resumen;
        }

        public void ReemplazarResumen(IEnumerable<ResumenDigitoVerificador> resumen)
        {
            // Para mantener la transaccionalidad sin usar SqlTransaction (ya que ExecuteNonQuery cierra la conexion),
            // armamos un gran bloque SQL transaccional (T-SQL) y lo enviamos de una sola vez.

            StringBuilder queryBatch = new StringBuilder();
            List<SqlParameter> parametros = new List<SqlParameter>();

            // SET XACT_ABORT ON asegura que si hay un error, SQL Server haga rollback automático
            queryBatch.AppendLine("SET XACT_ABORT ON;");
            queryBatch.AppendLine("BEGIN TRAN;");

            queryBatch.AppendLine(@"
            IF OBJECT_ID(N'dbo.DV', N'U') IS NULL
            BEGIN
                CREATE TABLE [dbo].[DV](
                    [NombreTabla] [nvarchar](128) NOT NULL,
                    [DVH] [nvarchar](128) NOT NULL,
                    [DVV] [nvarchar](128) NOT NULL,
                    [FechaCalculo] [datetime2](7) NOT NULL CONSTRAINT [DF_DV_FechaCalculo] DEFAULT (SYSDATETIME()),
                    CONSTRAINT [PK_DV] PRIMARY KEY CLUSTERED ([NombreTabla] ASC)
                )
            END;");

            // Limpiamos la tabla
            queryBatch.AppendLine("DELETE FROM dbo.DV;");

            // Insertamos los registros dinámicamente parametrizados para evitar inyección SQL
            int i = 0;
            foreach (ResumenDigitoVerificador item in resumen)
            {
                queryBatch.AppendLine($"INSERT INTO dbo.DV (NombreTabla, DVH, DVV, FechaCalculo) VALUES (@t{i}, @h{i}, @v{i}, SYSDATETIME());");

                parametros.Add(new SqlParameter($"@t{i}", item.Tabla));
                parametros.Add(new SqlParameter($"@h{i}", item.DVH));
                parametros.Add(new SqlParameter($"@v{i}", item.DVV));
                i++;
            }

            queryBatch.AppendLine("COMMIT TRAN;");

            // Ejecutamos todo el lote de una sola vez
            _conexion.ExecuteNonQuery(queryBatch.ToString(), parametros.ToArray());
        }

        public void ActualizarDVRegistro(string schema, string tabla, string nombreColumnaId, object valorId, string dvCalculado)
        {
            // Armamos un UPDATE dinámico para actualizar solo la columna DV de una fila específica
            string query = $"UPDATE [{schema}].[{tabla}] SET DV = @dv WHERE {nombreColumnaId} = @id";

            SqlParameter[] parametros = new SqlParameter[]
            {
        new SqlParameter("@dv", dvCalculado),
        new SqlParameter("@id", valorId)
            };

            // Usamos tu clase de conexión para ejecutar el comando
            _conexion.ExecuteNonQuery(query, parametros);
        }


    }
}