using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class TurnoDAL_DNI101
    {
        private readonly Conexion _conexion = new();

        public TurnoDAL_DNI101()
        {
            TurnosDatabaseInitializer.AsegurarTablas();
        }

        public int Guardar(string codigoTurno, DateTime fecha, TimeSpan hora, string motivo, string estado, int idPaciente, int dniNutricionista, int? idBloque, string? dv)
        {
            try
            {
                if (string.IsNullOrEmpty(codigoTurno))
                {
                    codigoTurno = $"TRN-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";
                }

                string query = @"
INSERT INTO Turnos_DNI101 (CodigoTurno_DNI101, FechaTurno_DNI101, HoraTurno_DNI101, MotivoConsulta_DNI101, EstadoTurno_DNI101, IdPaciente_DNI101, DniNutricionista_DNI101, IdBloque_DNI101, DV)
VALUES (@codigo, @fecha, @hora, @motivo, @estado, @idPaciente, @dniNutri, @idBloque, @dv);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dt = _conexion.ExecuteReader(query,
                    new SqlParameter("@codigo", codigoTurno),
                    new SqlParameter("@fecha", fecha),
                    new SqlParameter("@hora", hora),
                    new SqlParameter("@motivo", motivo),
                    new SqlParameter("@estado", estado),
                    new SqlParameter("@idPaciente", idPaciente),
                    new SqlParameter("@dniNutri", dniNutricionista),
                    new SqlParameter("@idBloque", (object?)idBloque ?? DBNull.Value),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value)
                );

                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    return Convert.ToInt32(dt.Rows[0][0]);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar turno: {ex.Message}");
                throw;
            }
        }

        public bool ActualizarEstado(int idTurno, string nuevoEstado)
        {
            try
            {
                string query = @"UPDATE Turnos_DNI101 SET EstadoTurno_DNI101 = @estado WHERE IdTurno_DNI101 = @idTurno;";
                _conexion.ExecuteNonQuery(query,
                    new SqlParameter("@estado", nuevoEstado),
                    new SqlParameter("@idTurno", idTurno)
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estado del turno: {ex.Message}");
                return false;
            }
        }

        public bool ActualizarEstadoBloque(int idBloque, string nuevoEstado)
        {
            try
            {
                return new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(idBloque, nuevoEstado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estado del bloque: {ex.Message}");
                return false;
            }
        }

        public bool ReprogramarTurno(int idTurno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque, int? dniNutricionista = null, string? dv = null)
        {
            try
            {
                string query = @"
UPDATE Turnos_DNI101
SET FechaTurno_DNI101 = @fecha,
    HoraTurno_DNI101 = @hora,
    IdBloque_DNI101 = @idBloque,
    EstadoTurno_DNI101 = 'Confirmado',
    DniNutricionista_DNI101 = ISNULL(@dniNutri, DniNutricionista_DNI101),
    DV = ISNULL(@dv, DV)
WHERE IdTurno_DNI101 = @idTurno;";

                _conexion.ExecuteNonQuery(query,
                    new SqlParameter("@fecha", nuevaFecha),
                    new SqlParameter("@hora", nuevaHora),
                    new SqlParameter("@idBloque", (object?)nuevoIdBloque ?? DBNull.Value),
                    new SqlParameter("@dniNutri", (object?)dniNutricionista ?? DBNull.Value),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value),
                    new SqlParameter("@idTurno", idTurno)
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reprogramar turno: {ex.Message}");
                return false;
            }
        }

        public bool ModificarTurno(int idTurno, DateTime fecha, TimeSpan hora, string motivo, string estado, string? dv = null)
        {
            try
            {
                string query = @"
UPDATE Turnos_DNI101
SET FechaTurno_DNI101 = @fecha,
    HoraTurno_DNI101 = @hora,
    MotivoConsulta_DNI101 = @motivo,
    EstadoTurno_DNI101 = @estado,
    DV = ISNULL(@dv, DV)
WHERE IdTurno_DNI101 = @idTurno;";

                _conexion.ExecuteNonQuery(query,
                    new SqlParameter("@fecha", fecha),
                    new SqlParameter("@hora", hora),
                    new SqlParameter("@motivo", motivo),
                    new SqlParameter("@estado", estado),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value),
                    new SqlParameter("@idTurno", idTurno)
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar turno: {ex.Message}");
                return false;
            }
        }

        public bool CancelarTurno(int idTurno, string motivo, string? dv = null)
        {
            try
            {
                string query = @"
UPDATE Turnos_DNI101
SET EstadoTurno_DNI101 = 'Cancelado',
    MotivoConsulta_DNI101 = MotivoConsulta_DNI101 + ' | Cancelado: ' + @motivo,
    DV = ISNULL(@dv, DV)
WHERE IdTurno_DNI101 = @idTurno;";

                _conexion.ExecuteNonQuery(query,
                    new SqlParameter("@motivo", motivo),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value),
                    new SqlParameter("@idTurno", idTurno)
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cancelar turno: {ex.Message}");
                return false;
            }
        }

        public int RegistrarTurnoConBloque(string codigoTurno, DateTime fecha, TimeSpan hora, string motivo, string estado, int idPaciente, int dniNutricionista, int? idBloque, string? dv)
        {
            if (string.IsNullOrEmpty(codigoTurno))
            {
                codigoTurno = $"TRN-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";
            }

            return _conexion.ExecuteTransaction(tran =>
            {
                string queryTurno = @"
INSERT INTO Turnos_DNI101 (CodigoTurno_DNI101, FechaTurno_DNI101, HoraTurno_DNI101, MotivoConsulta_DNI101, EstadoTurno_DNI101, IdPaciente_DNI101, DniNutricionista_DNI101, IdBloque_DNI101, DV)
VALUES (@codigo, @fecha, @hora, @motivo, @estado, @idPaciente, @dniNutri, @idBloque, @dv);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dt = _conexion.ExecuteReaderTran(queryTurno, tran,
                    new SqlParameter("@codigo", codigoTurno),
                    new SqlParameter("@fecha", fecha),
                    new SqlParameter("@hora", hora),
                    new SqlParameter("@motivo", motivo),
                    new SqlParameter("@estado", estado),
                    new SqlParameter("@idPaciente", idPaciente),
                    new SqlParameter("@dniNutri", dniNutricionista),
                    new SqlParameter("@idBloque", (object?)idBloque ?? DBNull.Value),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value)
                );

                int idGenerado = 0;
                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    idGenerado = Convert.ToInt32(dt.Rows[0][0]);
                }

                if (idBloque.HasValue && idBloque.Value > 0)
                {
                    string queryBloque = "UPDATE BloquesHorarios_DNI101 SET EstadoBloque_DNI101 = 'Ocupado' WHERE IdBloque_DNI101 = @idBloque";
                    _conexion.ExecuteNonQueryTran(queryBloque, tran, new SqlParameter("@idBloque", idBloque.Value));
                }

                return idGenerado;
            });
        }

        public bool ReprogramarTurnoConBloques(int idTurno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque, int? idBloqueAnterior, int? dniNutricionista = null, string? dv = null)
        {
            return _conexion.ExecuteTransaction(tran =>
            {
                string queryTurno = @"
UPDATE Turnos_DNI101
SET FechaTurno_DNI101 = @fecha,
    HoraTurno_DNI101 = @hora,
    IdBloque_DNI101 = @idBloque,
    EstadoTurno_DNI101 = 'Confirmado',
    DniNutricionista_DNI101 = ISNULL(@dniNutri, DniNutricionista_DNI101),
    DV = ISNULL(@dv, DV)
WHERE IdTurno_DNI101 = @idTurno;";

                _conexion.ExecuteNonQueryTran(queryTurno, tran,
                    new SqlParameter("@fecha", nuevaFecha),
                    new SqlParameter("@hora", nuevaHora),
                    new SqlParameter("@idBloque", (object?)nuevoIdBloque ?? DBNull.Value),
                    new SqlParameter("@dniNutri", (object?)dniNutricionista ?? DBNull.Value),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value),
                    new SqlParameter("@idTurno", idTurno)
                );

                if (idBloqueAnterior.HasValue && idBloqueAnterior.Value > 0)
                {
                    _conexion.ExecuteNonQueryTran("UPDATE BloquesHorarios_DNI101 SET EstadoBloque_DNI101 = 'Disponible' WHERE IdBloque_DNI101 = @id", tran, new SqlParameter("@id", idBloqueAnterior.Value));
                }

                if (nuevoIdBloque.HasValue && nuevoIdBloque.Value > 0)
                {
                    _conexion.ExecuteNonQueryTran("UPDATE BloquesHorarios_DNI101 SET EstadoBloque_DNI101 = 'Ocupado' WHERE IdBloque_DNI101 = @id", tran, new SqlParameter("@id", nuevoIdBloque.Value));
                }

                return true;
            });
        }

        public bool CancelarTurnoConBloque(int idTurno, string motivo, int? idBloque, string? dv = null)
        {
            return _conexion.ExecuteTransaction(tran =>
            {
                string queryTurno = @"
UPDATE Turnos_DNI101
SET EstadoTurno_DNI101 = 'Cancelado',
    MotivoConsulta_DNI101 = MotivoConsulta_DNI101 + ' | Cancelado: ' + @motivo,
    DV = ISNULL(@dv, DV)
WHERE IdTurno_DNI101 = @idTurno;";

                _conexion.ExecuteNonQueryTran(queryTurno, tran,
                    new SqlParameter("@motivo", motivo),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value),
                    new SqlParameter("@idTurno", idTurno)
                );

                if (idBloque.HasValue && idBloque.Value > 0)
                {
                    string queryBloque = "UPDATE BloquesHorarios_DNI101 SET EstadoBloque_DNI101 = 'Disponible' WHERE IdBloque_DNI101 = @idBloque";
                    _conexion.ExecuteNonQueryTran(queryBloque, tran, new SqlParameter("@idBloque", idBloque.Value));
                }

                return true;
            });
        }

        public DataTable ObtenerPorId(int idTurno)
        {
            try
            {
                string query = @"
SELECT t.*, p.DniNiño_DNI101, p.Nombre_DNI101, p.Apellido_DNI101, p.Telefono_DNI101, p.Email_DNI101, p.ObraSocial_DNI101
FROM Turnos_DNI101 t
INNER JOIN Pacientes_DNI101 p ON t.IdPaciente_DNI101 = p.IdPaciente_DNI101
WHERE t.IdTurno_DNI101 = @id;";

                return _conexion.ExecuteReader(query, new SqlParameter("@id", idTurno));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener turno por ID: {ex.Message}");
                return new DataTable();
            }
        }

        public DataTable ObtenerPorCodigo(string codigoTurno)
        {
            try
            {
                string query = @"
SELECT t.*, p.DniNiño_DNI101, p.Nombre_DNI101, p.Apellido_DNI101, p.Telefono_DNI101, p.Email_DNI101, p.ObraSocial_DNI101
FROM Turnos_DNI101 t
INNER JOIN Pacientes_DNI101 p ON t.IdPaciente_DNI101 = p.IdPaciente_DNI101
WHERE t.CodigoTurno_DNI101 = @codigo;";

                return _conexion.ExecuteReader(query, new SqlParameter("@codigo", codigoTurno));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener turno por código: {ex.Message}");
                return new DataTable();
            }
        }

        public DataTable ListarTodos()
        {
            try
            {
                string query = @"
SELECT t.*, p.DniNiño_DNI101, p.Nombre_DNI101, p.Apellido_DNI101, p.Telefono_DNI101, p.Email_DNI101, p.ObraSocial_DNI101
FROM Turnos_DNI101 t
INNER JOIN Pacientes_DNI101 p ON t.IdPaciente_DNI101 = p.IdPaciente_DNI101
ORDER BY t.FechaTurno_DNI101 DESC, t.HoraTurno_DNI101 DESC;";

                return _conexion.ExecuteReader(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar turnos: {ex.Message}");
                return new DataTable();
            }
        }

        public DataTable ListarTurnosPorFecha(DateTime fecha)
        {
            try
            {
                string query = @"
SELECT t.*, p.DniNiño_DNI101, p.Nombre_DNI101, p.Apellido_DNI101, p.Telefono_DNI101, p.Email_DNI101, p.ObraSocial_DNI101
FROM Turnos_DNI101 t
INNER JOIN Pacientes_DNI101 p ON t.IdPaciente_DNI101 = p.IdPaciente_DNI101
WHERE CAST(t.FechaTurno_DNI101 AS DATE) = CAST(@fecha AS DATE)
ORDER BY t.HoraTurno_DNI101 ASC;";

                return _conexion.ExecuteReader(query, new SqlParameter("@fecha", fecha.Date));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar turnos por fecha: {ex.Message}");
                return new DataTable();
            }
        }

        public DataTable ListarTurnosPorPaciente(int idPaciente)
        {
            try
            {
                string query = @"
SELECT t.*, p.DniNiño_DNI101, p.Nombre_DNI101, p.Apellido_DNI101, p.Telefono_DNI101, p.Email_DNI101, p.ObraSocial_DNI101
FROM Turnos_DNI101 t
INNER JOIN Pacientes_DNI101 p ON t.IdPaciente_DNI101 = p.IdPaciente_DNI101
WHERE t.IdPaciente_DNI101 = @idPaciente
ORDER BY t.FechaTurno_DNI101 DESC;";

                return _conexion.ExecuteReader(query, new SqlParameter("@idPaciente", idPaciente));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar turnos por paciente: {ex.Message}");
                return new DataTable();
            }
        }

        public bool ExisteTurnoParaProfesional(int dniNutricionista, DateTime fecha, TimeSpan hora, int? idTurnoExcluir = null)
        {
            try
            {
                string query = @"
SELECT COUNT(*)
FROM Turnos_DNI101
WHERE DniNutricionista_DNI101 = @dni
  AND CAST(FechaTurno_DNI101 AS DATE) = CAST(@fecha AS DATE)
  AND HoraTurno_DNI101 = @hora
  AND EstadoTurno_DNI101 <> 'Cancelado'
  AND (@idTurnoExcluir IS NULL OR IdTurno_DNI101 <> @idTurnoExcluir);";

                DataTable dt = _conexion.ExecuteReader(query,
                    new SqlParameter("@dni", dniNutricionista),
                    new SqlParameter("@fecha", fecha.Date),
                    new SqlParameter("@hora", hora),
                    new SqlParameter("@idTurnoExcluir", (object?)idTurnoExcluir ?? DBNull.Value)
                );

                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    return Convert.ToInt32(dt.Rows[0][0]) > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar existencia de turno: {ex.Message}");
                return false;
            }
        }

        public List<TimeSpan> ObtenerHorariosOcupadosPorProfesional(int dniNutricionista, DateTime fecha)
        {
            var horarios = new List<TimeSpan>();
            try
            {
                string query = @"
SELECT HoraTurno_DNI101
FROM Turnos_DNI101
WHERE DniNutricionista_DNI101 = @dni
  AND CAST(FechaTurno_DNI101 AS DATE) = CAST(@fecha AS DATE)
  AND EstadoTurno_DNI101 <> 'Cancelado';";

                DataTable dt = _conexion.ExecuteReader(query,
                    new SqlParameter("@dni", dniNutricionista),
                    new SqlParameter("@fecha", fecha.Date)
                );

                foreach (DataRow row in dt.Rows)
                {
                    if (row["HoraTurno_DNI101"] != DBNull.Value)
                    {
                        horarios.Add((TimeSpan)row["HoraTurno_DNI101"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener horarios ocupados: {ex.Message}");
            }
            return horarios;
        }
    }
}
