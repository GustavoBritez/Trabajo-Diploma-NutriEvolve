using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class TutorDAL_DNI101
    {
        private readonly Conexion _conexion = new();

        public TutorDAL_DNI101()
        {
            TurnosDatabaseInitializer.AsegurarTablas();
        }

        public int Guardar(TutorBE_DNI101 tutor)
        {
            try
            {
                string query = @"
INSERT INTO Tutores_DNI101 (DniTutor_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, Parentesco_DNI101, FechaRegistro_DNI101, DV)
VALUES (@dni, @nombre, @apellido, @telefono, @email, @parentesco, @fecha, @dv);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dt = _conexion.ExecuteReader(query,
                    new SqlParameter("@dni", tutor.DniTutor_DNI101),
                    new SqlParameter("@nombre", tutor.Nombre_DNI101),
                    new SqlParameter("@apellido", tutor.Apellido_DNI101),
                    new SqlParameter("@telefono", (object?)tutor.Telefono_DNI101 ?? DBNull.Value),
                    new SqlParameter("@email", (object?)tutor.Email_DNI101 ?? DBNull.Value),
                    new SqlParameter("@parentesco", (object?)tutor.Parentesco_DNI101 ?? DBNull.Value),
                    new SqlParameter("@fecha", tutor.FechaRegistro_DNI101),
                    new SqlParameter("@dv", (object?)tutor.DV ?? DBNull.Value)
                );

                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    int idGenerado = Convert.ToInt32(dt.Rows[0][0]);
                    tutor.IdTutor_DNI101 = idGenerado;
                    return idGenerado;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar tutor: {ex.Message}");
                throw;
            }
        }

        public TutorBE_DNI101? ObtenerPorDNI(string dniTutor)
        {
            try
            {
                string query = @"SELECT * FROM Tutores_DNI101 WHERE DniTutor_DNI101 = @dni;";
                DataTable dt = _conexion.ExecuteReader(query, new SqlParameter("@dni", dniTutor.Trim()));
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new TutorBE_DNI101
                    {
                        IdTutor_DNI101 = Convert.ToInt32(row["IdTutor_DNI101"]),
                        DniTutor_DNI101 = row["DniTutor_DNI101"].ToString() ?? string.Empty,
                        Nombre_DNI101 = row["Nombre_DNI101"].ToString() ?? string.Empty,
                        Apellido_DNI101 = row["Apellido_DNI101"].ToString() ?? string.Empty,
                        Telefono_DNI101 = row["Telefono_DNI101"] != DBNull.Value ? row["Telefono_DNI101"].ToString() : null,
                        Email_DNI101 = row["Email_DNI101"] != DBNull.Value ? row["Email_DNI101"].ToString() : null,
                        Parentesco_DNI101 = row["Parentesco_DNI101"] != DBNull.Value ? row["Parentesco_DNI101"].ToString() : null,
                        FechaRegistro_DNI101 = Convert.ToDateTime(row["FechaRegistro_DNI101"]),
                        DV = row["DV"] != DBNull.Value ? row["DV"].ToString() : null
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tutor por DNI: {ex.Message}");
                return null;
            }
        }

        public List<TutorBE_DNI101> ListarTodos()
        {
            var lista = new List<TutorBE_DNI101>();
            try
            {
                string query = @"SELECT * FROM Tutores_DNI101 ORDER BY Apellido_DNI101, Nombre_DNI101;";
                DataTable dt = _conexion.ExecuteReader(query);
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new TutorBE_DNI101
                    {
                        IdTutor_DNI101 = Convert.ToInt32(row["IdTutor_DNI101"]),
                        DniTutor_DNI101 = row["DniTutor_DNI101"].ToString() ?? string.Empty,
                        Nombre_DNI101 = row["Nombre_DNI101"].ToString() ?? string.Empty,
                        Apellido_DNI101 = row["Apellido_DNI101"].ToString() ?? string.Empty,
                        Telefono_DNI101 = row["Telefono_DNI101"] != DBNull.Value ? row["Telefono_DNI101"].ToString() : null,
                        Email_DNI101 = row["Email_DNI101"] != DBNull.Value ? row["Email_DNI101"].ToString() : null,
                        Parentesco_DNI101 = row["Parentesco_DNI101"] != DBNull.Value ? row["Parentesco_DNI101"].ToString() : null,
                        FechaRegistro_DNI101 = Convert.ToDateTime(row["FechaRegistro_DNI101"]),
                        DV = row["DV"] != DBNull.Value ? row["DV"].ToString() : null
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar tutores: {ex.Message}");
            }
            return lista;
        }
    }
}
