using System;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class TurnosDatabaseInitializer
    {
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        public static void AsegurarTablas()
        {
            if (_initialized) return;

            lock (_lock)
            {
                if (_initialized) return;

                try
                {
                    Conexion conexion = new Conexion();
                    string ddl = @"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tutores_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Tutores_DNI101](
        [IdTutor_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [DniTutor_DNI101] VARCHAR(20) NOT NULL UNIQUE,
        [Nombre_DNI101] VARCHAR(50) NOT NULL,
        [Apellido_DNI101] VARCHAR(50) NOT NULL,
        [Telefono_DNI101] VARCHAR(50) NULL,
        [Email_DNI101] VARCHAR(100) NULL,
        [Parentesco_DNI101] VARCHAR(50) NULL,
        [FechaRegistro_DNI101] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pacientes_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Pacientes_DNI101](
        [IdPaciente_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [DniNiño_DNI101] VARCHAR(20) NOT NULL UNIQUE,
        [Nombre_DNI101] VARCHAR(50) NOT NULL,
        [Apellido_DNI101] VARCHAR(50) NOT NULL,
        [Telefono_DNI101] VARCHAR(50) NULL,
        [Email_DNI101] VARCHAR(100) NULL,
        [FechaNacimiento_DNI101] DATETIME2(7) NOT NULL,
        [Sexo_DNI101] VARCHAR(10) NULL,
        [ObraSocial_DNI101] VARCHAR(100) NULL,
        [IdTutor_DNI101] INT NULL,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AgendasMedicas_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[AgendasMedicas_DNI101](
        [IdAgenda_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [Fecha_DNI101] DATE NOT NULL,
        [EstadoAgenda_DNI101] VARCHAR(50) NOT NULL DEFAULT 'Abierta',
        [DniNutricionista_DNI101] INT NOT NULL,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BloquesHorarios_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[BloquesHorarios_DNI101](
        [IdBloque_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdAgenda_DNI101] INT NOT NULL,
        [HoraInicio_DNI101] TIME(7) NOT NULL,
        [HoraFin_DNI101] TIME(7) NOT NULL,
        [EstadoBloque_DNI101] VARCHAR(50) NOT NULL DEFAULT 'Disponible',
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Turnos_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Turnos_DNI101](
        [IdTurno_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [CodigoTurno_DNI101] VARCHAR(50) NOT NULL UNIQUE,
        [FechaTurno_DNI101] DATETIME2(7) NOT NULL,
        [HoraTurno_DNI101] TIME(7) NOT NULL,
        [MotivoConsulta_DNI101] VARCHAR(255) NOT NULL,
        [EstadoTurno_DNI101] VARCHAR(50) NOT NULL DEFAULT 'Solicitado',
        [IdPaciente_DNI101] INT NOT NULL,
        [DniNutricionista_DNI101] INT NOT NULL,
        [IdBloque_DNI101] INT NULL,
        [DV] VARCHAR(255) NULL
    );
END;
";
                    conexion.ExecuteNonQuery(ddl);
                    _initialized = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al asegurar tablas de turnos: {ex.Message}");
                }
            }
        }
    }
}
