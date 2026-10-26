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
WHERE ISNULL(p.Activo_DNI101, 1) = 1
ORDER BY p.Apellido_DNI101, p.Nombre_DNI101;";

                return _conexion.ExecuteReader(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar pacientes: {ex.Message}");
                return new DataTable();
            }
        }

        public DataTable ListarMaestro()
        {
            try
            {
                string query = @"
SELECT 
    IdPaciente_DNI101,
    DniNiño_DNI101 AS DNI,
    Apellido_DNI101 AS APELLIDOS,
    Nombre_DNI101 AS NOMBRES,
    ISNULL(Email_DNI101, '') AS EMAIL,
    ISNULL(Telefono_DNI101, '') AS CELULAR,
    ISNULL(Direccion_DNI101, '') AS DIRECCION
FROM Pacientes_DNI101
WHERE ISNULL(Activo_DNI101, 1) = 1
ORDER BY Apellido_DNI101, Nombre_DNI101;";

                return _conexion.ExecuteReader(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarMaestro: {ex.Message}");
                return new DataTable();
            }
        }

        public int GuardarMaestro(string dni, string apellidos, string nombres, string? email, string? celular, string? direccion)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                string sqlPac = @"
INSERT INTO Pacientes_DNI101 
(DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, Direccion_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101, Activo_DNI101)
VALUES 
(@dni, @nombres, @apellidos, @celular, @email, @direccion, DATEADD(YEAR, -5, SYSDATETIME()), 'Otro', 'Particular', 1);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int idPaciente;
                using (var cmd = new SqlCommand(sqlPac, tx.Connection, tx))
                {
                    cmd.Parameters.AddWithValue("@dni", dni.Trim());
                    cmd.Parameters.AddWithValue("@nombres", nombres.Trim());
                    cmd.Parameters.AddWithValue("@apellidos", apellidos.Trim());
                    cmd.Parameters.AddWithValue("@celular", (object?)celular?.Trim() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", (object?)email?.Trim() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", (object?)direccion?.Trim() ?? DBNull.Value);
                    idPaciente = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Guardar en tabla 1:N de emails si se especificó
                if (!string.IsNullOrWhiteSpace(email))
                {
                    string sqlEmail = "INSERT INTO Paciente_Emails_DNI101 (IdPaciente_DNI101, Email_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @email, 1, 1);";
                    using (var cmdEm = new SqlCommand(sqlEmail, tx.Connection, tx))
                    {
                        cmdEm.Parameters.AddWithValue("@id", idPaciente);
                        cmdEm.Parameters.AddWithValue("@email", email.Trim());
                        cmdEm.ExecuteNonQuery();
                    }
                }

                // Guardar en tabla 1:N de teléfonos si se especificó
                if (!string.IsNullOrWhiteSpace(celular))
                {
                    string sqlTel = "INSERT INTO Paciente_Telefonos_DNI101 (IdPaciente_DNI101, Numero_DNI101, Tipo_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @num, 'Celular', 1, 1);";
                    using (var cmdTel = new SqlCommand(sqlTel, tx.Connection, tx))
                    {
                        cmdTel.Parameters.AddWithValue("@id", idPaciente);
                        cmdTel.Parameters.AddWithValue("@num", celular.Trim());
                        cmdTel.ExecuteNonQuery();
                    }
                }

                return idPaciente;
            });
        }

        public bool ModificarMaestro(int idPaciente, string dni, string apellidos, string nombres, string? email, string? celular, string? direccion)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                string sqlUpd = @"
UPDATE Pacientes_DNI101
SET DniNiño_DNI101 = @dni,
    Nombre_DNI101 = @nombres,
    Apellido_DNI101 = @apellidos,
    Telefono_DNI101 = @celular,
    Email_DNI101 = @email,
    Direccion_DNI101 = @direccion
WHERE IdPaciente_DNI101 = @id;";

                using (var cmd = new SqlCommand(sqlUpd, tx.Connection, tx))
                {
                    cmd.Parameters.AddWithValue("@id", idPaciente);
                    cmd.Parameters.AddWithValue("@dni", dni.Trim());
                    cmd.Parameters.AddWithValue("@nombres", nombres.Trim());
                    cmd.Parameters.AddWithValue("@apellidos", apellidos.Trim());
                    cmd.Parameters.AddWithValue("@celular", (object?)celular?.Trim() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", (object?)email?.Trim() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", (object?)direccion?.Trim() ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }

                // Sincronizar email principal en 1:N
                if (!string.IsNullOrWhiteSpace(email))
                {
                    string sqlEm = @"
IF EXISTS (SELECT 1 FROM Paciente_Emails_DNI101 WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1)
    UPDATE Paciente_Emails_DNI101 SET Email_DNI101 = @email WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1;
ELSE
    INSERT INTO Paciente_Emails_DNI101 (IdPaciente_DNI101, Email_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @email, 1, 1);";

                    using (var cmdEm = new SqlCommand(sqlEm, tx.Connection, tx))
                    {
                        cmdEm.Parameters.AddWithValue("@id", idPaciente);
                        cmdEm.Parameters.AddWithValue("@email", email.Trim());
                        cmdEm.ExecuteNonQuery();
                    }
                }

                // Sincronizar celular principal en 1:N
                if (!string.IsNullOrWhiteSpace(celular))
                {
                    string sqlTel = @"
IF EXISTS (SELECT 1 FROM Paciente_Telefonos_DNI101 WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1)
    UPDATE Paciente_Telefonos_DNI101 SET Numero_DNI101 = @cel WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1;
ELSE
    INSERT INTO Paciente_Telefonos_DNI101 (IdPaciente_DNI101, Numero_DNI101, Tipo_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @cel, 'Celular', 1, 1);";

                    using (var cmdTel = new SqlCommand(sqlTel, tx.Connection, tx))
                    {
                        cmdTel.Parameters.AddWithValue("@id", idPaciente);
                        cmdTel.Parameters.AddWithValue("@cel", celular.Trim());
                        cmdTel.ExecuteNonQuery();
                    }
                }

                return true;
            });
        }

        public bool EliminarLogico(int idPaciente)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                // 1. Borrado lógico de Paciente (Activo = 0)
                string sqlPac = "UPDATE Pacientes_DNI101 SET Activo_DNI101 = 0 WHERE IdPaciente_DNI101 = @id;";
                using (var cmd = new SqlCommand(sqlPac, tx.Connection, tx))
                {
                    cmd.Parameters.AddWithValue("@id", idPaciente);
                    cmd.ExecuteNonQuery();
                }

                // 2. Repercusión en cascada lógica sobre las tablas 1:n
                string sql1N = @"
UPDATE Paciente_Emails_DNI101 SET Activo_DNI101 = 0 WHERE IdPaciente_DNI101 = @id;
UPDATE Paciente_Telefonos_DNI101 SET Activo_DNI101 = 0 WHERE IdPaciente_DNI101 = @id;";

                using (var cmd1N = new SqlCommand(sql1N, tx.Connection, tx))
                {
                    cmd1N.Parameters.AddWithValue("@id", idPaciente);
                    cmd1N.ExecuteNonQuery();
                }

                return true;
            });
        }

        public DataTable ObtenerEmails1N(int idPaciente)
        {
            string sql = "SELECT IdEmail_DNI101, Email_DNI101, EsPrincipal_DNI101 FROM Paciente_Emails_DNI101 WHERE IdPaciente_DNI101 = @id AND Activo_DNI101 = 1;";
            return _conexion.ExecuteReader(sql, new SqlParameter("@id", idPaciente));
        }

        public void AgregarEmail1N(int idPaciente, string email, bool esPrincipal)
        {
            _conexion.ExecuteTransaction(tx =>
            {
                if (esPrincipal)
                {
                    using var cmdReset = new SqlCommand("UPDATE Paciente_Emails_DNI101 SET EsPrincipal_DNI101 = 0 WHERE IdPaciente_DNI101 = @id;", tx.Connection, tx);
                    cmdReset.Parameters.AddWithValue("@id", idPaciente);
                    cmdReset.ExecuteNonQuery();
                }

                string sql = "INSERT INTO Paciente_Emails_DNI101 (IdPaciente_DNI101, Email_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @email, @princ, 1);";
                using var cmd = new SqlCommand(sql, tx.Connection, tx);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                cmd.Parameters.AddWithValue("@email", email.Trim());
                cmd.Parameters.AddWithValue("@princ", esPrincipal);
                cmd.ExecuteNonQuery();
            });
        }

        public void EliminarEmailLogico1N(int idEmail)
        {
            string sql = "UPDATE Paciente_Emails_DNI101 SET Activo_DNI101 = 0 WHERE IdEmail_DNI101 = @id;";
            _conexion.ExecuteNonQuery(sql, new SqlParameter("@id", idEmail));
        }

        public DataTable ObtenerTelefonos1N(int idPaciente)
        {
            string sql = "SELECT IdTelefono_DNI101, Numero_DNI101, Tipo_DNI101, EsPrincipal_DNI101 FROM Paciente_Telefonos_DNI101 WHERE IdPaciente_DNI101 = @id AND Activo_DNI101 = 1;";
            return _conexion.ExecuteReader(sql, new SqlParameter("@id", idPaciente));
        }

        public void AgregarTelefono1N(int idPaciente, string numero, string tipo, bool esPrincipal)
        {
            _conexion.ExecuteTransaction(tx =>
            {
                if (esPrincipal)
                {
                    using var cmdReset = new SqlCommand("UPDATE Paciente_Telefonos_DNI101 SET EsPrincipal_DNI101 = 0 WHERE IdPaciente_DNI101 = @id;", tx.Connection, tx);
                    cmdReset.Parameters.AddWithValue("@id", idPaciente);
                    cmdReset.ExecuteNonQuery();
                }

                string sql = "INSERT INTO Paciente_Telefonos_DNI101 (IdPaciente_DNI101, Numero_DNI101, Tipo_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @num, @tipo, @princ, 1);";
                using var cmd = new SqlCommand(sql, tx.Connection, tx);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                cmd.Parameters.AddWithValue("@num", numero.Trim());
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@princ", esPrincipal);
                cmd.ExecuteNonQuery();
            });
        }

        public void EliminarTelefonoLogico1N(int idTelefono)
        {
            string sql = "UPDATE Paciente_Telefonos_DNI101 SET Activo_DNI101 = 0 WHERE IdTelefono_DNI101 = @id;";
            _conexion.ExecuteNonQuery(sql, new SqlParameter("@id", idTelefono));
        }

        public void ImportarClientesDesdeXml(List<(string dni, string apellidos, string nombres, string email, string celular, string direccion)> clientes)
        {
            if (clientes == null || clientes.Count == 0) return;

            _conexion.ExecuteTransaction(tx =>
            {
                foreach (var c in clientes)
                {
                    string dni = c.dni.Trim();
                    string apellidos = c.apellidos.Trim();
                    string nombres = c.nombres.Trim();
                    string email = c.email?.Trim() ?? "";
                    string celular = c.celular?.Trim() ?? "";
                    string direccion = c.direccion?.Trim() ?? "";

                    if (string.IsNullOrWhiteSpace(dni)) continue;

                    // 1. Verificar si ya existe por DNI (activo o inactivo)
                    string sqlCheck = "SELECT IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = @dni;";
                    int idPaciente = 0;
                    using (var cmd = new SqlCommand(sqlCheck, tx.Connection, tx))
                    {
                        cmd.Parameters.AddWithValue("@dni", dni);
                        object? obj = cmd.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value)
                        {
                            idPaciente = Convert.ToInt32(obj);
                        }
                    }

                    if (idPaciente > 0)
                    {
                        // Reactivar y actualizar datos
                        string sqlUpd = @"
UPDATE Pacientes_DNI101
SET Nombre_DNI101 = @nombres,
    Apellido_DNI101 = @apellidos,
    Telefono_DNI101 = @celular,
    Email_DNI101 = @email,
    Direccion_DNI101 = @direccion,
    Activo_DNI101 = 1
WHERE IdPaciente_DNI101 = @id;";
                        using (var cmdUpd = new SqlCommand(sqlUpd, tx.Connection, tx))
                        {
                            cmdUpd.Parameters.AddWithValue("@id", idPaciente);
                            cmdUpd.Parameters.AddWithValue("@nombres", nombres);
                            cmdUpd.Parameters.AddWithValue("@apellidos", apellidos);
                            cmdUpd.Parameters.AddWithValue("@celular", string.IsNullOrWhiteSpace(celular) ? (object)DBNull.Value : celular);
                            cmdUpd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email);
                            cmdUpd.Parameters.AddWithValue("@direccion", string.IsNullOrWhiteSpace(direccion) ? (object)DBNull.Value : direccion);
                            cmdUpd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insertar nuevo registro
                        string sqlIns = @"
INSERT INTO Pacientes_DNI101 
(DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, Direccion_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101, Activo_DNI101)
VALUES 
(@dni, @nombres, @apellidos, @celular, @email, @direccion, DATEADD(YEAR, -5, SYSDATETIME()), 'Otro', 'Particular', 1);
SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        using (var cmdIns = new SqlCommand(sqlIns, tx.Connection, tx))
                        {
                            cmdIns.Parameters.AddWithValue("@dni", dni);
                            cmdIns.Parameters.AddWithValue("@nombres", nombres);
                            cmdIns.Parameters.AddWithValue("@apellidos", apellidos);
                            cmdIns.Parameters.AddWithValue("@celular", string.IsNullOrWhiteSpace(celular) ? (object)DBNull.Value : celular);
                            cmdIns.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email);
                            cmdIns.Parameters.AddWithValue("@direccion", string.IsNullOrWhiteSpace(direccion) ? (object)DBNull.Value : direccion);
                            idPaciente = Convert.ToInt32(cmdIns.ExecuteScalar());
                        }
                    }

                    // Sincronizar en tabla 1:N de emails
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        string sqlEmail = @"
IF EXISTS (SELECT 1 FROM Paciente_Emails_DNI101 WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1)
    UPDATE Paciente_Emails_DNI101 SET Email_DNI101 = @email, Activo_DNI101 = 1 WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1;
ELSE
    INSERT INTO Paciente_Emails_DNI101 (IdPaciente_DNI101, Email_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @email, 1, 1);";
                        using (var cmdEm = new SqlCommand(sqlEmail, tx.Connection, tx))
                        {
                            cmdEm.Parameters.AddWithValue("@id", idPaciente);
                            cmdEm.Parameters.AddWithValue("@email", email);
                            cmdEm.ExecuteNonQuery();
                        }
                    }

                    // Sincronizar en tabla 1:N de teléfonos
                    if (!string.IsNullOrWhiteSpace(celular))
                    {
                        string sqlTel = @"
IF EXISTS (SELECT 1 FROM Paciente_Telefonos_DNI101 WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1)
    UPDATE Paciente_Telefonos_DNI101 SET Numero_DNI101 = @cel, Tipo_DNI101 = 'Celular', Activo_DNI101 = 1 WHERE IdPaciente_DNI101 = @id AND EsPrincipal_DNI101 = 1;
ELSE
    INSERT INTO Paciente_Telefonos_DNI101 (IdPaciente_DNI101, Numero_DNI101, Tipo_DNI101, EsPrincipal_DNI101, Activo_DNI101) VALUES (@id, @cel, 'Celular', 1, 1);";
                        using (var cmdTel = new SqlCommand(sqlTel, tx.Connection, tx))
                        {
                            cmdTel.Parameters.AddWithValue("@id", idPaciente);
                            cmdTel.Parameters.AddWithValue("@cel", celular);
                            cmdTel.ExecuteNonQuery();
                        }
                    }
                }
            });
        }
    }
}
