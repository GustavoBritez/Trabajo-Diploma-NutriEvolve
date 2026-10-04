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

        public int Guardar(string dniNiño, string nombre, string apellido, string? telefono, string? email, DateTime fechaNacimiento, string? sexo, string? obraSocial, string? dv)
        {
            try
            {
                string query = @"
INSERT INTO Pacientes_DNI101 (DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101, DV)
VALUES (@dni, @nombre, @apellido, @telefono, @email, @fechaNac, @sexo, @obraSocial, @dv);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dt = _conexion.ExecuteReader(query,
                    new SqlParameter("@dni", dniNiño),
                    new SqlParameter("@nombre", nombre),
                    new SqlParameter("@apellido", apellido),
                    new SqlParameter("@telefono", (object?)telefono ?? DBNull.Value),
                    new SqlParameter("@email", (object?)email ?? DBNull.Value),
                    new SqlParameter("@fechaNac", fechaNacimiento),
                    new SqlParameter("@sexo", (object?)sexo ?? DBNull.Value),
                    new SqlParameter("@obraSocial", (object?)obraSocial ?? DBNull.Value),
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
                Console.WriteLine($"Error al guardar paciente: {ex.Message}");
                throw;
            }
        }

        public bool Modificar(int idPaciente, string dniNiño, string nombre, string apellido, string? telefono, string? email, DateTime fechaNacimiento, string? sexo, string? obraSocial, string? dv)
        {
            try
            {
                string query = @"
UPDATE Pacientes_DNI101
SET DniNiño_DNI101 = @dni,
    Nombre_DNI101 = @nombre,
    Apellido_DNI101 = @apellido,
    Telefono_DNI101 = @telefono,
    Email_DNI101 = @email,
    FechaNacimiento_DNI101 = @fechaNac,
    Sexo_DNI101 = @sexo,
    ObraSocial_DNI101 = @obraSocial,
    DV = @dv
WHERE IdPaciente_DNI101 = @idPaciente;";

                _conexion.ExecuteNonQuery(query,
                    new SqlParameter("@idPaciente", idPaciente),
                    new SqlParameter("@dni", dniNiño),
                    new SqlParameter("@nombre", nombre),
                    new SqlParameter("@apellido", apellido),
                    new SqlParameter("@telefono", (object?)telefono ?? DBNull.Value),
                    new SqlParameter("@email", (object?)email ?? DBNull.Value),
                    new SqlParameter("@fechaNac", fechaNacimiento),
                    new SqlParameter("@sexo", (object?)sexo ?? DBNull.Value),
                    new SqlParameter("@obraSocial", (object?)obraSocial ?? DBNull.Value),
                    new SqlParameter("@dv", (object?)dv ?? DBNull.Value)
                );

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar paciente: {ex.Message}");
                throw;
            }
        }

        public DataTable ObtenerPacientePorDNI(string dniNiño)
        {
            try
            {
                string query = @"
SELECT p.*
FROM Pacientes_DNI101 p
WHERE p.DniNiño_DNI101 = @dni;";

                return _conexion.ExecuteReader(query, new SqlParameter("@dni", dniNiño.Trim()));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente por DNI: {ex.Message}");
                return new DataTable();
            }
        }

        public DataTable ObtenerPorId(int idPaciente)
        {
            try
            {
                string query = @"
SELECT p.*
FROM Pacientes_DNI101 p
WHERE p.IdPaciente_DNI101 = @id;";

                return _conexion.ExecuteReader(query, new SqlParameter("@id", idPaciente));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente por ID: {ex.Message}");
                return new DataTable();
            }
        }

        public DataTable ListarTodos()
        {
            try
            {
                string query = @"
SELECT p.*
FROM Pacientes_DNI101 p
ORDER BY p.Apellido_DNI101, p.Nombre_DNI101;";

                return _conexion.ExecuteReader(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar pacientes: {ex.Message}");
                return new DataTable();
            }
        }
    }
}
