using System;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class SeguimientoNutricionalDatabaseInitializer
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
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ConsultasNutricionales_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[ConsultasNutricionales_DNI101](
        [IdConsulta_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdPaciente_DNI101] INT NOT NULL,
        [DniNutricionista_DNI101] INT NOT NULL,
        [IdTurno_DNI101] INT NULL,
        [FechaControl_DNI101] DATETIME2(7) NOT NULL,
        [EdadMeses_DNI101] INT NOT NULL,
        [TipoLactancia_DNI101] VARCHAR(50) NULL,
        [AlimentacionComplementaria_DNI101] VARCHAR(255) NULL,
        [Alergias_DNI101] VARCHAR(255) NULL,
        [AntecedentesFamiliares_DNI101] VARCHAR(255) NULL,
        [Observaciones_DNI101] VARCHAR(500) NULL,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MedicionesAntropometricas_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[MedicionesAntropometricas_DNI101](
        [IdMedicion_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdConsulta_DNI101] INT NOT NULL,
        [PesoKg_DNI101] DECIMAL(5,2) NOT NULL,
        [TallaCm_DNI101] DECIMAL(5,2) NOT NULL,
        [PerimetroCefalicoCm_DNI101] DECIMAL(5,2) NOT NULL,
        [CircunferenciaCinturaCm_DNI101] DECIMAL(5,2) NULL,
        [IMC_DNI101] DECIMAL(5,2) NOT NULL,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ZScoresResultados_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[ZScoresResultados_DNI101](
        [IdZScore_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdConsulta_DNI101] INT NOT NULL,
        [ZPesoEdad_DNI101] DECIMAL(4,2) NOT NULL,
        [ZTallaEdad_DNI101] DECIMAL(4,2) NOT NULL,
        [ZIMCEdad_DNI101] DECIMAL(4,2) NOT NULL,
        [ZPCEdad_DNI101] DECIMAL(4,2) NOT NULL,
        [PercentilPeso_DNI101] DECIMAL(5,2) NOT NULL,
        [PercentilTalla_DNI101] DECIMAL(5,2) NOT NULL,
        [PercentilIMC_DNI101] DECIMAL(5,2) NOT NULL,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DiagnosticosNutricionales_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[DiagnosticosNutricionales_DNI101](
        [IdDiagnostico_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdConsulta_DNI101] INT NOT NULL,
        [ClasificacionOMS_DNI101] VARCHAR(100) NOT NULL,
        [DetallesClinicos_DNI101] VARCHAR(500) NOT NULL,
        [RequiereAlerta_DNI101] BIT NOT NULL DEFAULT 0,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AlertasClinicas_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[AlertasClinicas_DNI101](
        [IdAlerta_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdConsulta_DNI101] INT NOT NULL,
        [TipoAlerta_DNI101] VARCHAR(50) NOT NULL,
        [Severidad_DNI101] VARCHAR(20) NOT NULL,
        [MensajeAlerta_DNI101] VARCHAR(255) NOT NULL,
        [FechaGeneracion_DNI101] DATETIME2(7) NOT NULL,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlanesAlimentarios_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[PlanesAlimentarios_DNI101](
        [IdPlan_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdConsulta_DNI101] INT NOT NULL,
        [RequerimientoCalorico_DNI101] DECIMAL(6,2) NOT NULL,
        [PctCarbohidratos_DNI101] DECIMAL(4,2) NOT NULL,
        [PctProteinas_DNI101] DECIMAL(4,2) NOT NULL,
        [PctGrasas_DNI101] DECIMAL(4,2) NOT NULL,
        [PautasFamiliares_DNI101] VARCHAR(1000) NOT NULL,
        [MetasSalud_DNI101] VARCHAR(500) NOT NULL,
        [DV] VARCHAR(255) NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Recordatorios24h_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Recordatorios24h_DNI101](
        [IdRecordatorio_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [IdConsulta_DNI101] INT NOT NULL,
        [Desayuno_DNI101] VARCHAR(255) NOT NULL,
        [Almuerzo_DNI101] VARCHAR(255) NOT NULL,
        [Merienda_DNI101] VARCHAR(255) NOT NULL,
        [Cena_DNI101] VARCHAR(255) NOT NULL,
        [Colaciones_DNI101] VARCHAR(255) NULL,
        [FrecuenciaAlimentos_DNI101] VARCHAR(500) NULL,
        [DV] VARCHAR(255) NULL
    );
END;
";
                    conexion.ExecuteNonQuery(ddl);
                    _initialized = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al asegurar tablas de seguimiento nutricional: {ex.Message}");
                }
            }
        }
    }
}
