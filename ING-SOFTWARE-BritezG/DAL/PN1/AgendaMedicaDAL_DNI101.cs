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

        public DataTable ListarBloquesDisponibles(DateTime fecha, int dniNutricionista)
        {
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

                return _conexion.ExecuteReader(query,
                    new SqlParameter("@fecha", fecha.Date),
                    new SqlParameter("@dni", dniNutricionista)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar bloques disponibles: {ex.Message}");
                return new DataTable();
            }
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
