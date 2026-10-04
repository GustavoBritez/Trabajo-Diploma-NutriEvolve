using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class BitacoraDAL
    {
        private readonly Conexion conexion;
        private const string TABLA_BITACORA = "Bitacora";

        public BitacoraDAL()
        {
            conexion = new();
        }

        public List<BitacoraBE> FiltrarBitacora(DateTime desde, DateTime hasta)
        {
            List<BitacoraBE> bitacoras = new List<BitacoraBE>();

            try
            {
                string query = $@"SELECT Criticidad, Descripcion, Dni, Fecha, Id_Event, Modulo
                                  FROM {TABLA_BITACORA}
                                  WHERE Fecha BETWEEN @desde AND @hasta
                                  ORDER BY Fecha DESC";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@desde", desde),
                    new SqlParameter("@hasta", hasta)
                };

                DataTable dt = conexion.ExecuteReader(query, parametros);

                foreach (DataRow row in dt.Rows)
                {
                    bitacoras.Add(new BitacoraBE(
                        criticidad: (int)row["Criticidad"],
                        descripcion: row["Descripcion"].ToString(),
                        dni: (int)row["Dni"],
                        fecha: (DateTime)row["Fecha"],
                        modulo: row["Modulo"].ToString(),
                        id_bitacora: (int)row["Id_Event"]
                    ));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener registros de bitacora por fecha: {ex.Message}");
                throw;
            }

            return bitacoras;
        }

        public void GuardarBitacora(BitacoraBE newBitacora)
        {
            try
            {
                string query = $@"INSERT INTO {TABLA_BITACORA} (Criticidad, Descripcion, Dni, Fecha, Modulo)
                                  VALUES (@criticidad, @descripcion, @dni, @fecha, @modulo)";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@criticidad", newBitacora._Criticidad),
                    new SqlParameter("@descripcion", newBitacora._Descripcion),
                    new SqlParameter("@dni", newBitacora._Dni),
                    new SqlParameter("@fecha", newBitacora._Fecha),
                    new SqlParameter("@modulo", newBitacora._Modulo)
                };

                conexion.ExecuteNonQuery(query, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar en bitacora: {ex.Message}");
                throw;
            }
        }

        public List<BitacoraBE> ObtenerBitacora()
        {
            List<BitacoraBE> bitacoras = new List<BitacoraBE>();

            try
            {
                string query = $@"SELECT Criticidad, Descripcion, Dni, Fecha, Id_Event, Modulo
                                  FROM {TABLA_BITACORA}
                                  ORDER BY Fecha DESC";

                DataTable dt = conexion.ExecuteReader(query, null);

                foreach (DataRow row in dt.Rows)
                {
                    bitacoras.Add(new BitacoraBE(
                        criticidad: (int)row["Criticidad"],
                        descripcion: row["Descripcion"].ToString(),
                        dni: (int)row["Dni"],
                        fecha: (DateTime)row["Fecha"],
                        modulo: row["Modulo"].ToString(),
                        id_bitacora: (int)row["Id_Event"]
                    ));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los registros de bitacora: {ex.Message}");
                throw;
            }

            return bitacoras;
        }
    }

    [Obsolete("Usar BitacoraDAL")]
    public class EventoDAL : BitacoraDAL
    {
    }
}
