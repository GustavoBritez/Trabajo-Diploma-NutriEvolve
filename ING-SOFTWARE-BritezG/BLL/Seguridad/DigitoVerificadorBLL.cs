using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BLL
{
    public class DigitoVerificadorBLL
    {
        private const string NombreTablaGlobal = "__BD__";
        private readonly DigitoVerificadorDAL digitoVerificadorDAL = new();

        /// <summary>
        /// Recalcula y persiste todos los Dígitos Verificadores del sistema:
        /// 1. DV individual por fila en todas las tablas que poseen columna 'DV'.
        /// 2. DV horizontal (DVH) y vertical (DVV) de cada tabla.
        /// 3. DV global (__BD__) de toda la base de datos.
        /// Todo resuelto en una sola pasada y persistido en una única transacción en DAL.
        /// </summary>
        public void RecalcularYPersistir()
        {
            var (resumen, filasDV) = CalcularIntegridadCompleta(calcularFilasIndividuales: true);
            digitoVerificadorDAL.PersistirIntegridadCompleta(resumen, filasDV);
        }

        /// <summary>
        /// Comprueba la integridad de toda la base de datos comparando con dbo.DV.
        /// Devuelve la lista de nombres de las tablas alteradas.
        /// Si la lista está vacía, no hay inconsistencias.
        /// </summary>
        public List<string> ObtenerTablasAlteradas()
        {
            List<string> tablasAlteradas = new();

            if (!digitoVerificadorDAL.ExisteTablaDV())
            {
                tablasAlteradas.Add("DV");
                return tablasAlteradas;
            }

            var (resumenCalculado, _) = CalcularIntegridadCompleta(calcularFilasIndividuales: false);
            Dictionary<string, ResumenDigitoVerificador> calculados = resumenCalculado
                .ToDictionary(item => item.Tabla, StringComparer.OrdinalIgnoreCase);

            List<ResumenDigitoVerificador> persistidos = digitoVerificadorDAL.ObtenerResumenPersistido();

            // Comparar tablas persistidas contra las calculadas en tiempo real
            foreach (ResumenDigitoVerificador item in persistidos)
            {
                if (string.Equals(item.Tabla, NombreTablaGlobal, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!calculados.TryGetValue(item.Tabla, out ResumenDigitoVerificador? calculado))
                {
                    tablasAlteradas.Add(item.Tabla);
                }
                else if (!string.Equals(item.DVH, calculado.DVH, StringComparison.OrdinalIgnoreCase) ||
                         !string.Equals(item.DVV, calculado.DVV, StringComparison.OrdinalIgnoreCase))
                {
                    tablasAlteradas.Add(item.Tabla);
                }
            }

            // Detectar si aparecieron tablas nuevas no registradas en dbo.DV
            foreach (ResumenDigitoVerificador calculado in resumenCalculado)
            {
                if (string.Equals(calculado.Tabla, NombreTablaGlobal, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!persistidos.Any(p => string.Equals(p.Tabla, calculado.Tabla, StringComparison.OrdinalIgnoreCase)))
                {
                    if (!tablasAlteradas.Contains(calculado.Tabla))
                    {
                        tablasAlteradas.Add(calculado.Tabla);
                    }
                }
            }

            // Fallback: Si no se identificó tabla individual pero difiere el hash global de la BD
            if (tablasAlteradas.Count == 0)
            {
                var globalPersistido = persistidos.FirstOrDefault(p => string.Equals(p.Tabla, NombreTablaGlobal, StringComparison.OrdinalIgnoreCase));
                var globalCalculado = resumenCalculado.FirstOrDefault(p => string.Equals(p.Tabla, NombreTablaGlobal, StringComparison.OrdinalIgnoreCase));

                if (globalPersistido != null && globalCalculado != null)
                {
                    if (!string.Equals(globalPersistido.DVH, globalCalculado.DVH, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(globalPersistido.DVV, globalCalculado.DVV, StringComparison.OrdinalIgnoreCase))
                    {
                        tablasAlteradas.Add("Base de Datos");
                    }
                }
            }

            return tablasAlteradas;
        }

        public bool VerificarBaseDatos()
        {
            return ObtenerTablasAlteradas().Count == 0;
        }

        public List<string> ObtenerUsuariosCorruptos()
        {
            List<string> usuariosCorruptos = new();
            DataTable dtUsuarios = digitoVerificadorDAL.ObtenerDatosTabla("dbo", "Usuarios");

            if (!dtUsuarios.Columns.Contains("DV"))
            {
                return usuariosCorruptos;
            }

            string columnaId = dtUsuarios.Columns.Contains("NombreDeUsuario") ? "NombreDeUsuario" : dtUsuarios.Columns[0].ColumnName;

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
                    string usuarioAfectado = fila[columnaId]?.ToString() ?? "ID Desconocido";
                    usuariosCorruptos.Add(usuarioAfectado);
                }
            }

            return usuariosCorruptos;
        }

        private (List<ResumenDigitoVerificador> Resumen, List<(string Schema, string Tabla, string ColumnaId, object ValorId, string DV)> FilasDV)
            CalcularIntegridadCompleta(bool calcularFilasIndividuales)
        {
            List<ResumenDigitoVerificador> resumen = new();
            List<(string Schema, string Tabla, string ColumnaId, object ValorId, string DV)> filasDV = new();

            BigInteger totalHorizontalBD = BigInteger.Zero;
            BigInteger totalVerticalBD = BigInteger.Zero;

            foreach ((string schema, string table) in digitoVerificadorDAL.ObtenerTablasPersistentes())
            {
                DataTable datosTabla = digitoVerificadorDAL.ObtenerDatosTabla(schema, table);
                bool tieneColumnaDV = datosTabla.Columns.Contains("DV");
                string? columnaId = (tieneColumnaDV && datosTabla.Columns.Count > 0) ? datosTabla.Columns[0].ColumnName : null;

                BigInteger totalHorizontalTabla = BigInteger.Zero;

                // 1. Recorrer filas de la tabla
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

                    totalHorizontalTabla += totalFila;

                    if (calcularFilasIndividuales && tieneColumnaDV && columnaId != null)
                    {
                        string dvCalculado = FormatearHexadecimal(totalFila);
                        object valorId = fila[columnaId];
                        filasDV.Add((schema, table, columnaId, valorId, dvCalculado));
                    }
                }

                // 2. Calcular DV vertical de la tabla
                BigInteger totalVerticalTabla = BigInteger.Zero;
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

                    totalVerticalTabla += totalColumna;
                }

                string dvhTabla = FormatearHexadecimal(totalHorizontalTabla);
                string dvvTabla = FormatearHexadecimal(totalVerticalTabla);

                resumen.Add(new ResumenDigitoVerificador(table, dvhTabla, dvvTabla));

                totalHorizontalBD += ConvertirHexadecimalAEntero(dvhTabla);
                totalVerticalBD += ConvertirHexadecimalAEntero(dvvTabla);
            }

            resumen.Add(new ResumenDigitoVerificador(NombreTablaGlobal, FormatearHexadecimal(totalHorizontalBD), FormatearHexadecimal(totalVerticalBD)));

            return (resumen, filasDV);
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
            RecalcularYPersistir();
        }
    }
}