using DAL;
using System.Data;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace BLL
{
    public class DigitoVerificadorBLL
    {
        private const string NombreTablaGlobal = "__BD__";
        private readonly DigitoVerificadorDAL digitoVerificadorDAL = new();

        public void RecalcularYPersistir()
        {
            digitoVerificadorDAL.ReemplazarResumen(CalcularResumenActual());
        }

        public bool VerificarBaseDatos()
        {
            if (!digitoVerificadorDAL.ExisteTablaDV())
            {
                return false;
            }

            Dictionary<string, ResumenDigitoVerificador> calculados = CalcularResumenActual()
                .ToDictionary(item => item.Tabla, StringComparer.OrdinalIgnoreCase);

            List<ResumenDigitoVerificador> persistidos = digitoVerificadorDAL.ObtenerResumenPersistido();

            if (persistidos.Count != calculados.Count)
            {
                return false;
            }

            foreach (ResumenDigitoVerificador item in persistidos)
            {
                if (!calculados.TryGetValue(item.Tabla, out ResumenDigitoVerificador? calculado))
                {
                    return false;
                }

                if (!string.Equals(item.DVH, calculado.DVH, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (!string.Equals(item.DVV, calculado.DVV, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
        public List<string> ObtenerUsuariosCorruptos()
        {
            List<string> usuariosCorruptos = new();

            DataTable dtUsuarios = digitoVerificadorDAL.ObtenerDatosTabla("dbo", "Usuarios");

            if (!dtUsuarios.Columns.Contains("DV"))
            {
                return usuariosCorruptos;
            }
            string columnaIdentificadora = dtUsuarios.Columns.Contains("Nombre") ? "Nombre" : dtUsuarios.Columns[0].ColumnName;

            foreach (DataRow fila in dtUsuarios.Rows)
            {
                BigInteger totalFila = BigInteger.Zero;

                foreach (DataColumn columna in dtUsuarios.Columns)
                {
                    if (string.Equals(columna.ColumnName, "DV", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    totalFila += ObtenerValorHexadecimal(fila[columna]);
                }

                string dvCalculado = FormatearHexadecimal(totalFila);
                string dvGuardado = fila["DV"]?.ToString() ?? string.Empty;

                if (!string.Equals(dvCalculado, dvGuardado, StringComparison.OrdinalIgnoreCase))
                {
                    string usuarioAfectado = fila[columnaIdentificadora]?.ToString() ?? "ID Desconocido";
                    usuariosCorruptos.Add(usuarioAfectado);
                }
            }

            return usuariosCorruptos;
        }
        private List<ResumenDigitoVerificador> CalcularResumenActual()
        {
            List<ResumenDigitoVerificador> resumen = new();
            BigInteger totalHorizontalBD = BigInteger.Zero;
            BigInteger totalVerticalBD = BigInteger.Zero;

            foreach ((string schema, string table) in digitoVerificadorDAL.ObtenerTablasPersistentes())
            {
                DataTable datosTabla = digitoVerificadorDAL.ObtenerDatosTabla(schema, table);
                string dvhTabla = CalcularDVHorizontalTabla(datosTabla);
                string dvvTabla = CalcularDVVerticalTabla(datosTabla);

                resumen.Add(new ResumenDigitoVerificador(table, dvhTabla, dvvTabla));

                totalHorizontalBD += ConvertirHexadecimalAEntero(dvhTabla);
                totalVerticalBD += ConvertirHexadecimalAEntero(dvvTabla);
            }

            resumen.Add(new ResumenDigitoVerificador(NombreTablaGlobal, FormatearHexadecimal(totalHorizontalBD), FormatearHexadecimal(totalVerticalBD)));
            return resumen;
        }

        private static string CalcularDVHorizontalTabla(DataTable datosTabla)
        {
            BigInteger totalHorizontal = BigInteger.Zero;

            foreach (DataRow fila in datosTabla.Rows)
            {
                BigInteger totalFila = BigInteger.Zero;

                foreach (DataColumn columna in datosTabla.Columns)
                {
                    if (string.Equals(columna.ColumnName, "DV", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    totalFila += ObtenerValorHexadecimal(fila[columna]);
                }

                totalHorizontal += totalFila;
            }

            return FormatearHexadecimal(totalHorizontal);
        }

        private static string CalcularDVVerticalTabla(DataTable datosTabla)
        {
            BigInteger totalVertical = BigInteger.Zero;

            foreach (DataColumn columna in datosTabla.Columns)
            {
                if (string.Equals(columna.ColumnName, "DV", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                BigInteger totalColumna = BigInteger.Zero;

                foreach (DataRow fila in datosTabla.Rows)
                {
                    totalColumna += ObtenerValorHexadecimal(fila[columna]);
                }

                totalVertical += totalColumna;
            }

            return FormatearHexadecimal(totalVertical);
        }

        private static BigInteger ObtenerValorHexadecimal(object? valor)
        {
            string texto = FormatearValor(valor);
            BigInteger total = BigInteger.Zero;

            foreach (byte byteValor in Encoding.UTF8.GetBytes(texto))
            {
                total += byteValor;
            }

            return total;
        }

        private static string FormatearValor(object? valor)
        {
            if (valor is null || valor == DBNull.Value)
            {
                return string.Empty;
            }

            return valor switch
            {
                DateTime fecha => fecha.ToString("O", CultureInfo.InvariantCulture),
                bool booleano => booleano ? "1" : "0",
                byte[] bytes => Convert.ToHexString(bytes),
                IFormattable formateable => formateable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty,
                _ => valor.ToString() ?? string.Empty
            };
        }

        private static BigInteger ConvertirHexadecimalAEntero(string valorHexadecimal)
        {
            if (string.IsNullOrWhiteSpace(valorHexadecimal))
            {
                return BigInteger.Zero;
            }

            return BigInteger.Parse(valorHexadecimal, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        }

        private static string FormatearHexadecimal(BigInteger valor)
        {
            return valor.ToString("X");
        }

        public void ActualizarDVIndividualesUsuarios()
        {
            // 1. Traemos todos los usuarios actuales
            DataTable dtUsuarios = digitoVerificadorDAL.ObtenerDatosTabla("dbo", "Usuarios");

            // Si por algún motivo la tabla no tiene la columna DV, cancelamos para evitar errores
            if (!dtUsuarios.Columns.Contains("DV"))
            {
                return;
            }

            // 2. Identificamos cuál es la columna clave (Primary Key). 
            // Por lo general, en 'SELECT *', el ID suele ser la primera columna (índice 0).
            string columnaId = dtUsuarios.Columns[0].ColumnName;

            // 3. Recorremos fila por fila
            foreach (DataRow fila in dtUsuarios.Rows)
            {
                BigInteger totalFila = BigInteger.Zero;

                // Sumamos todas las columnas de este usuario
                foreach (DataColumn columna in dtUsuarios.Columns)
                {
                    if (string.Equals(columna.ColumnName, "DV", StringComparison.OrdinalIgnoreCase))
                    {
                        continue; // No sumamos la columna DV para evitar que el hash se modifique a sí mismo
                    }

                    totalFila += ObtenerValorHexadecimal(fila[columna]);
                }

                // Formateamos el resultado final
                string dvCalculado = FormatearHexadecimal(totalFila);
                object valorId = fila[columnaId];

                // 4. Mandamos a la base de datos a guardar el código en la fila de este usuario
                digitoVerificadorDAL.ActualizarDVRegistro("dbo", "Usuarios", columnaId, valorId, dvCalculado);
            }
        }
    }
}