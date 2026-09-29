using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class AgendaMedicaDAL_DNI101
    {
        private readonly Conexion _conexion = new();

        public AgendaMedicaDAL_DNI101()
        {
            TurnosDatabaseInitializer.AsegurarTablas();
        }

        public List<BloqueHorarioBE_DNI101> ListarBloquesDisponibles(DateTime fecha, int dniNutricionista)
        {
            var bloques = new List<BloqueHorarioBE_DNI101>();
            try
            {
                string query = @"
SELECT b.*
FROM BloquesHorarios_DNI101 b
INNER JOIN AgendasMedicas_DNI101 a ON b.IdAgenda_DNI101 = a.IdAgenda_DNI101
WHERE a.Fecha_DNI101 = @fecha
  AND a.DniNutricionista_DNI101 = @dni
  AND b.EstadoBloque_DNI101 = 'Disponible'
ORDER BY b.HoraInicio_DNI101 ASC;";

                DataTable dt = _conexion.ExecuteReader(query,
                    new SqlParameter("@fecha", fecha.Date),
                    new SqlParameter("@dni", dniNutricionista)
                );

                foreach (DataRow row in dt.Rows)
                {
                    bloques.Add(new BloqueHorarioBE_DNI101
                    {
                        IdBloque_DNI101 = Convert.ToInt32(row["IdBloque_DNI101"]),
                        IdAgenda_DNI101 = Convert.ToInt32(row["IdAgenda_DNI101"]),
                        HoraInicio_DNI101 = (TimeSpan)row["HoraInicio_DNI101"],
                        HoraFin_DNI101 = (TimeSpan)row["HoraFin_DNI101"],
                        EstadoBloque_DNI101 = row["EstadoBloque_DNI101"].ToString() ?? "Disponible",
                        DV = row["DV"] != DBNull.Value ? row["DV"].ToString() : null
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar bloques disponibles: {ex.Message}");
            }
            return bloques;
        }

        public bool ActualizarEstadoBloque(int idBloque, string nuevoEstado)
        {
            try
            {
                string query = @"UPDATE BloquesHorarios_DNI101 SET EstadoBloque_DNI101 = @estado WHERE IdBloque_DNI101 = @id;";
                _conexion.ExecuteNonQuery(query,
                    new SqlParameter("@estado", nuevoEstado),
                    new SqlParameter("@id", idBloque)
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estado del bloque: {ex.Message}");
                return false;
            }
        }
    }
}
