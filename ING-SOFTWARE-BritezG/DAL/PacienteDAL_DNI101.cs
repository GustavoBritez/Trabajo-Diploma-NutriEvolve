using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class PacienteDAL_DNI101
    {
        private readonly Conexion _conexion = new();

        public PacienteDAL_DNI101()
        {
            TurnosDatabaseInitializer.AsegurarTablas();
        }

        public int Guardar(PacienteBE_DNI101 paciente)
        {
            try
            {
                string query = @"
INSERT INTO Pacientes_DNI101 (DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101, DV)
VALUES (@dni, @nombre, @apellido, @telefono, @email, @fechaNac, @sexo, @obraSocial, @dv);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dt = _conexion.ExecuteReader(query,
                    new SqlParameter("@dni", paciente.DNINiño_DNI101),
                    new SqlParameter("@nombre", paciente.Nombre_DNI101),
                    new SqlParameter("@apellido", paciente.Apellido_DNI101),
                    new SqlParameter("@telefono", (object?)paciente.Telefono_DNI101 ?? DBNull.Value),
                    new SqlParameter("@email", (object?)paciente.Email_DNI101 ?? DBNull.Value),
                    new SqlParameter("@fechaNac", paciente.FechaNacimiento_DNI101),
                    new SqlParameter("@sexo", (object?)paciente.Sexo_DNI101 ?? DBNull.Value),
                    new SqlParameter("@obraSocial", (object?)paciente.ObraSocial_DNI101 ?? DBNull.Value),
                    new SqlParameter("@dv", (object?)paciente.DV ?? DBNull.Value)
                );

                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    int idGenerado = Convert.ToInt32(dt.Rows[0][0]);
                    paciente.IdPaciente_DNI101 = idGenerado;
                    return idGenerado;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar paciente: {ex.Message}");
                throw;
            }
        }

        public bool Modificar(PacienteBE_DNI101 paciente)
        {
            try
            {
                string query = @"
UPDATE Pacientes_DNI101
SET Nombre_DNI101 = @nombre,
    Apellido_DNI101 = @apellido,
    Telefono_DNI101 = @telefono,
    Email_DNI101 = @email,
    FechaNacimiento_DNI101 = @fechaNac,
    Sexo_DNI101 = @sexo,
    ObraSocial_DNI101 = @obraSocial,
    DV = @dv
WHERE IdPaciente_DNI101 = @idPaciente;";

                _conexion.ExecuteNonQuery(query,
                    new SqlParameter("@idPaciente", paciente.IdPaciente_DNI101),
                    new SqlParameter("@nombre", paciente.Nombre_DNI101),
                    new SqlParameter("@apellido", paciente.Apellido_DNI101),
                    new SqlParameter("@telefono", (object?)paciente.Telefono_DNI101 ?? DBNull.Value),
                    new SqlParameter("@email", (object?)paciente.Email_DNI101 ?? DBNull.Value),
                    new SqlParameter("@fechaNac", paciente.FechaNacimiento_DNI101),
                    new SqlParameter("@sexo", (object?)paciente.Sexo_DNI101 ?? DBNull.Value),
                    new SqlParameter("@obraSocial", (object?)paciente.ObraSocial_DNI101 ?? DBNull.Value),
                    new SqlParameter("@dv", (object?)paciente.DV ?? DBNull.Value)
                );

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar paciente: {ex.Message}");
                throw;
            }
        }

        public PacienteBE_DNI101? ObtenerPacientePorDNI(string dniNiño)
        {
            try
            {
                string query = @"
SELECT p.*
FROM Pacientes_DNI101 p
WHERE p.DniNiño_DNI101 = @dni;";

                DataTable dt = _conexion.ExecuteReader(query, new SqlParameter("@dni", dniNiño.Trim()));
                if (dt.Rows.Count > 0)
                {
                    return MapearPaciente(dt.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente por DNI: {ex.Message}");
                return null;
            }
        }

        public PacienteBE_DNI101? ObtenerPorId(int idPaciente)
        {
            try
            {
                string query = @"
SELECT p.*
FROM Pacientes_DNI101 p
WHERE p.IdPaciente_DNI101 = @id;";

                DataTable dt = _conexion.ExecuteReader(query, new SqlParameter("@id", idPaciente));
                if (dt.Rows.Count > 0)
                {
                    return MapearPaciente(dt.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente por ID: {ex.Message}");
                return null;
            }
        }

        public List<PacienteBE_DNI101> ListarTodos()
        {
            var lista = new List<PacienteBE_DNI101>();
            try
            {
                string query = @"
SELECT p.*
FROM Pacientes_DNI101 p
ORDER BY p.Apellido_DNI101, p.Nombre_DNI101;";

                DataTable dt = _conexion.ExecuteReader(query);
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(MapearPaciente(row));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar pacientes: {ex.Message}");
            }
            return lista;
        }

        private PacienteBE_DNI101 MapearPaciente(DataRow row)
        {
            return new PacienteBE_DNI101
            {
                IdPaciente_DNI101 = Convert.ToInt32(row["IdPaciente_DNI101"]),
                DNINiño_DNI101 = row["DniNiño_DNI101"].ToString() ?? string.Empty,
                Nombre_DNI101 = row["Nombre_DNI101"].ToString() ?? string.Empty,
                Apellido_DNI101 = row["Apellido_DNI101"].ToString() ?? string.Empty,
                Telefono_DNI101 = row["Telefono_DNI101"] != DBNull.Value ? row["Telefono_DNI101"].ToString() : null,
                Email_DNI101 = row["Email_DNI101"] != DBNull.Value ? row["Email_DNI101"].ToString() : null,
                FechaNacimiento_DNI101 = row["FechaNacimiento_DNI101"] != DBNull.Value ? Convert.ToDateTime(row["FechaNacimiento_DNI101"]) : DateTime.Today,
                Sexo_DNI101 = row["Sexo_DNI101"] != DBNull.Value ? row["Sexo_DNI101"].ToString() : null,
                ObraSocial_DNI101 = row["ObraSocial_DNI101"] != DBNull.Value ? row["ObraSocial_DNI101"].ToString() : null,
                DV = row["DV"] != DBNull.Value ? row["DV"].ToString() : null
            };
        }
    }
}
