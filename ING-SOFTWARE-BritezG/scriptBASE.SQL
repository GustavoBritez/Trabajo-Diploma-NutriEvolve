-- ============================================================================
-- SCRIPT BASE OFICIAL DE BASE DE DATOS (NutriEvolve - ING)
-- 
-- Contenido:
-- 1. Estructura completa de tablas DDL y Triggers de Bitácora de Cambios.
-- 2. Catálogo completo de Permisos (Patentes) actuales del sistema (IDs 20 al 35).
-- 3. Mapeo de controles visuales / botones de formularios a Permisos (Permiso_Boton).
-- 4. Perfiles del sistema:
--      - ID 1: Administrador (acceso a todos los permisos del sistema).
--      - ID 3: Nutricionista (acceso exclusivo a los botones de PN1 y PN2).
-- 5. Cuenta única de usuario: Administrador General (admin / 1234).
-- 6. Todas las tablas de dominio (pacientes, consultas, turnos, bitácoras) limpias sin datos de prueba.
-- ============================================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ING')
BEGIN
    CREATE DATABASE [ING];
END
GO

USE [ING];
GO

-- ============================================================================
-- 1. TABLAS DEL SISTEMA DE SEGURIDAD Y CONTROL DE INTEGRIDAD
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Bitacora' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Bitacora](
	[Id_Bitacora] [int] IDENTITY(1,1) NOT NULL,
	[Dni] [int] NOT NULL,
	[Modulo] [varchar](100) NOT NULL,
	[Criticidad] [int] NOT NULL,
	[Descripcion] [varchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED ([Id_Bitacora] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DV' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[DV](
	[NombreTabla] [nvarchar](128) NOT NULL,
	[DVH] [nvarchar](128) NOT NULL,
	[DVV] [nvarchar](128) NOT NULL,
	[FechaCalculo] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DV] PRIMARY KEY CLUSTERED ([NombreTabla] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Familia' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Familia](
	[ID_Familia] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([ID_Familia] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Familia_Familia' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Familia_Familia](
	[ID_FamiliaPadre] [int] NOT NULL,
	[ID_FamiliaHija] [int] NOT NULL,
 CONSTRAINT [PK_Familia_Familia] PRIMARY KEY CLUSTERED ([ID_FamiliaPadre] ASC, [ID_FamiliaHija] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Perfil' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Perfil](
	[ID_Perfil] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([ID_Perfil] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Familia_Perfil' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Familia_Perfil](
	[ID_Perfil] [int] NOT NULL,
	[ID_Familia] [int] NOT NULL,
 CONSTRAINT [PK_Familia_Perfil] PRIMARY KEY CLUSTERED ([ID_Perfil] ASC, [ID_Familia] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Perfil_Permiso' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Perfil_Permiso](
	[ID_Perfil] [int] NOT NULL,
	[ID_Permiso] [int] NOT NULL,
 CONSTRAINT [PK_Perfil_Permiso] PRIMARY KEY CLUSTERED ([ID_Perfil] ASC, [ID_Permiso] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permiso' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Permiso](
	[ID_Permiso] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([ID_Permiso] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permiso_Boton' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Permiso_Boton](
	[ID_PermisoBoton] [int] IDENTITY(1,1) NOT NULL,
	[NombreFormulario] [varchar](100) NOT NULL,
	[NombreControl] [varchar](100) NOT NULL,
	[NombrePatente] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Permiso_Boton] PRIMARY KEY CLUSTERED ([ID_PermisoBoton] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permiso_Familia' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Permiso_Familia](
	[ID_Permiso] [int] NOT NULL,
	[ID_Familia] [int] NOT NULL,
 CONSTRAINT [PK_Permiso_Familia] PRIMARY KEY CLUSTERED ([ID_Permiso] ASC, [ID_Familia] ASC)
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuarios' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Usuarios](
	[DNI] [int] NOT NULL,
	[NombreDeUsuario] [varchar](50) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Contraseña] [varchar](255) NOT NULL,
	[Bloqueado] [bit] NOT NULL DEFAULT 0,
	[Estado] [bit] NOT NULL DEFAULT 1,
	[ID_Perfil] [int] NOT NULL,
	[Idioma] [varchar](25) NOT NULL DEFAULT 'Español',
	[DV] [varchar](255) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([DNI] ASC)
) ON [PRIMARY];
END
GO

-- ============================================================================
-- 2. TABLAS DE DOMINIO - PRIMER NIVEL (PN1): MAESTRO CLIENTES Y TURNERO
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pacientes_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Pacientes_DNI101](
        [IdPaciente_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [DniNiño_DNI101] VARCHAR(100) NOT NULL UNIQUE,
        [Nombre_DNI101] VARCHAR(50) NOT NULL,
        [Apellido_DNI101] VARCHAR(50) NOT NULL,
        [Telefono_DNI101] VARCHAR(50) NULL,
        [Email_DNI101] VARCHAR(100) NULL,
        [Direccion_DNI101] VARCHAR(200) NULL,
        [FechaNacimiento_DNI101] DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
        [Sexo_DNI101] VARCHAR(10) NULL,
        [ObraSocial_DNI101] VARCHAR(100) NULL,
        [Activo_DNI101] BIT NOT NULL CONSTRAINT DF_Pacientes_Activo DEFAULT 1,
        [DV] VARCHAR(255) NULL
    );
END
ELSE
BEGIN
    -- Asegurar ancho para cifrado AES-256 (Base64)
    ALTER TABLE [dbo].[Pacientes_DNI101] ALTER COLUMN [DniNiño_DNI101] VARCHAR(100) NOT NULL;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pacientes_DNI101]') AND name = 'Direccion_DNI101')
        ALTER TABLE [dbo].[Pacientes_DNI101] ADD [Direccion_DNI101] VARCHAR(200) NULL;
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pacientes_DNI101]') AND name = 'Activo_DNI101')
        ALTER TABLE [dbo].[Pacientes_DNI101] ADD [Activo_DNI101] BIT NOT NULL CONSTRAINT DF_Pacientes_Activo DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Paciente_Emails_DNI101' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Paciente_Emails_DNI101](
    [IdEmail_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPaciente_DNI101] INT NOT NULL,
    [Email_DNI101] VARCHAR(150) NOT NULL,
    [EsPrincipal_DNI101] BIT NOT NULL DEFAULT 0,
    [Activo_DNI101] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_Paciente_Emails_Pacientes] FOREIGN KEY ([IdPaciente_DNI101]) 
        REFERENCES [dbo].[Pacientes_DNI101] ([IdPaciente_DNI101]) ON DELETE CASCADE
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Paciente_Telefonos_DNI101' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Paciente_Telefonos_DNI101](
    [IdTelefono_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPaciente_DNI101] INT NOT NULL,
    [Numero_DNI101] VARCHAR(50) NOT NULL,
    [Tipo_DNI101] VARCHAR(30) NOT NULL DEFAULT 'Celular',
    [EsPrincipal_DNI101] BIT NOT NULL DEFAULT 0,
    [Activo_DNI101] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_Paciente_Telefonos_Pacientes] FOREIGN KEY ([IdPaciente_DNI101]) 
        REFERENCES [dbo].[Pacientes_DNI101] ([IdPaciente_DNI101]) ON DELETE CASCADE
) ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AgendasMedicas_DNI101' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[AgendasMedicas_DNI101](
        [IdAgenda_DNI101] INT IDENTITY(1,1) PRIMARY KEY,
        [Fecha_DNI101] DATE NOT NULL,
        [EstadoAgenda_DNI101] VARCHAR(50) NOT NULL DEFAULT 'Abierta',
        [DniNutricionista_DNI101] INT NOT NULL,
        [DV] VARCHAR(255) NULL
    );
END
GO

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
END
GO

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
END
GO

-- ============================================================================
-- 3. TABLAS DE DOMINIO - SEGUNDO NIVEL (PN2): SEGUIMIENTO NUTRICIONAL
-- ============================================================================

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
END
GO

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
END
GO

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
END
GO

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
END
GO

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
END
GO

-- ============================================================================
-- 4. TABLAS DE AUDITORÍA HISTÓRICA (CONTROL DE CAMBIOS Y ROLLBACK)
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pacientes_Cambios_DNI101' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Pacientes_Cambios_DNI101](
    [IdCambio] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPaciente_DNI101] INT NOT NULL,
    [DniNiño_DNI101] VARCHAR(100) NOT NULL,
    [Nombre_DNI101] VARCHAR(100) NOT NULL,
    [Apellido_DNI101] VARCHAR(100) NOT NULL,
    [Telefono_DNI101] VARCHAR(50) NULL,
    [Email_DNI101] VARCHAR(100) NULL,
    [Direccion_DNI101] VARCHAR(200) NULL,
    [Activo_DNI101] BIT NOT NULL DEFAULT 1,
    [FechaCambio] DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    [UsuarioDni] INT NOT NULL DEFAULT 11111111,
    [TipoOperacion] VARCHAR(20) NOT NULL,
    [Act] BIT NOT NULL CONSTRAINT DF_PacientesCambios_Act DEFAULT 1
) ON [PRIMARY];
END
ELSE
BEGIN
    ALTER TABLE [dbo].[Pacientes_Cambios_DNI101] ALTER COLUMN [DniNiño_DNI101] VARCHAR(100) NOT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ConsultasNutricionales_Cambios_DNI101' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101](
    [IdCambio] INT IDENTITY(1,1) PRIMARY KEY,
    [IdConsulta_DNI101] INT NOT NULL,
    [IdPaciente_DNI101] INT NOT NULL,
    [DniNutricionista_DNI101] INT NOT NULL,
    [FechaControl_DNI101] DATETIME2(7) NOT NULL,
    [EdadMeses_DNI101] INT NOT NULL,
    [TipoLactancia_DNI101] VARCHAR(50) NULL,
    [AlimentacionComplementaria_DNI101] VARCHAR(255) NULL,
    [Alergias_DNI101] VARCHAR(255) NULL,
    [AntecedentesFamiliares_DNI101] VARCHAR(255) NULL,
    [Observaciones_DNI101] VARCHAR(MAX) NULL,
    [PesoKg_DNI101] DECIMAL(5,2) NULL,
    [TallaCm_DNI101] DECIMAL(5,2) NULL,
    [PerimetroCefalicoCm_DNI101] DECIMAL(5,2) NULL,
    [IMC_DNI101] DECIMAL(5,2) NULL,
    [ClasificacionOMS_DNI101] VARCHAR(100) NULL,
    [FechaCambio] DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    [UsuarioDni] INT NOT NULL DEFAULT 11111111,
    [TipoOperacion] VARCHAR(20) NOT NULL,
    [Act] BIT NOT NULL CONSTRAINT DF_ConsultasCambios_Act DEFAULT 1
) ON [PRIMARY];
END
GO

-- ============================================================================
-- 5. TRIGGERS DE CONTROL DE CAMBIOS Y TRAZABILIDAD (SESSION_CONTEXT)
-- ============================================================================

CREATE OR ALTER TRIGGER [dbo].[trg_Pacientes_BitacoraCambios]
ON [dbo].[Pacientes_DNI101]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(CAST(SESSION_CONTEXT(N'EsReversion') AS INT), 0) = 1
    BEGIN
        RETURN;
    END

    -- 1. ALTA (INSERT)
    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[Pacientes_Cambios_DNI101] (
            [IdPaciente_DNI101], [DniNiño_DNI101], [Nombre_DNI101], [Apellido_DNI101],
            [Telefono_DNI101], [Email_DNI101], [Direccion_DNI101], [Activo_DNI101],
            [FechaCambio], [UsuarioDni], [TipoOperacion], [Act]
        )
        SELECT 
            i.[IdPaciente_DNI101], i.[DniNiño_DNI101], i.[Nombre_DNI101], i.[Apellido_DNI101],
            i.[Telefono_DNI101], i.[Email_DNI101], i.[Direccion_DNI101], ISNULL(i.[Activo_DNI101], 1),
            SYSDATETIME(), ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'ALTA', 1
        FROM inserted i;
    END
    -- 2. MODIFICACION / BLOQUEAR / DESBLOQUEAR (UPDATE)
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        -- Ignorar actualizaciones puramente de Dígito Verificador (DV)
        IF NOT EXISTS (
            SELECT 1 
            FROM inserted i 
            JOIN deleted d ON i.[IdPaciente_DNI101] = d.[IdPaciente_DNI101]
            WHERE ISNULL(i.[DniNiño_DNI101], '') <> ISNULL(d.[DniNiño_DNI101], '')
               OR ISNULL(i.[Nombre_DNI101], '') <> ISNULL(d.[Nombre_DNI101], '')
               OR ISNULL(i.[Apellido_DNI101], '') <> ISNULL(d.[Apellido_DNI101], '')
               OR ISNULL(i.[Telefono_DNI101], '') <> ISNULL(d.[Telefono_DNI101], '')
               OR ISNULL(i.[Email_DNI101], '') <> ISNULL(d.[Email_DNI101], '')
               OR ISNULL(i.[Direccion_DNI101], '') <> ISNULL(d.[Direccion_DNI101], '')
               OR ISNULL(i.[Activo_DNI101], 1) <> ISNULL(d.[Activo_DNI101], 1)
        )
        BEGIN
            RETURN;
        END

        UPDATE c
        SET c.[Act] = 0
        FROM [dbo].[Pacientes_Cambios_DNI101] c
        INNER JOIN inserted i ON c.[IdPaciente_DNI101] = i.[IdPaciente_DNI101]
        INNER JOIN deleted d ON i.[IdPaciente_DNI101] = d.[IdPaciente_DNI101]
        WHERE ISNULL(i.[DniNiño_DNI101], '') <> ISNULL(d.[DniNiño_DNI101], '')
           OR ISNULL(i.[Nombre_DNI101], '') <> ISNULL(d.[Nombre_DNI101], '')
           OR ISNULL(i.[Apellido_DNI101], '') <> ISNULL(d.[Apellido_DNI101], '')
           OR ISNULL(i.[Telefono_DNI101], '') <> ISNULL(d.[Telefono_DNI101], '')
           OR ISNULL(i.[Email_DNI101], '') <> ISNULL(d.[Email_DNI101], '')
           OR ISNULL(i.[Direccion_DNI101], '') <> ISNULL(d.[Direccion_DNI101], '')
           OR ISNULL(i.[Activo_DNI101], 1) <> ISNULL(d.[Activo_DNI101], 1);

        INSERT INTO [dbo].[Pacientes_Cambios_DNI101] (
            [IdPaciente_DNI101], [DniNiño_DNI101], [Nombre_DNI101], [Apellido_DNI101],
            [Telefono_DNI101], [Email_DNI101], [Direccion_DNI101], [Activo_DNI101],
            [FechaCambio], [UsuarioDni], [TipoOperacion], [Act]
        )
        SELECT 
            i.[IdPaciente_DNI101], i.[DniNiño_DNI101], i.[Nombre_DNI101], i.[Apellido_DNI101],
            i.[Telefono_DNI101], i.[Email_DNI101], i.[Direccion_DNI101], ISNULL(i.[Activo_DNI101], 1),
            SYSDATETIME(), ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            CASE 
                WHEN ISNULL(d.[Activo_DNI101], 1) = 1 AND ISNULL(i.[Activo_DNI101], 1) = 0 THEN 'BLOQUEAR'
                WHEN ISNULL(d.[Activo_DNI101], 1) = 0 AND ISNULL(i.[Activo_DNI101], 1) = 1 THEN 'DESBLOQUEAR'
                ELSE 'MODIFICACIÓN'
            END,
            1
        FROM inserted i
        JOIN deleted d ON i.[IdPaciente_DNI101] = d.[IdPaciente_DNI101]
        WHERE ISNULL(i.[DniNiño_DNI101], '') <> ISNULL(d.[DniNiño_DNI101], '')
           OR ISNULL(i.[Nombre_DNI101], '') <> ISNULL(d.[Nombre_DNI101], '')
           OR ISNULL(i.[Apellido_DNI101], '') <> ISNULL(d.[Apellido_DNI101], '')
           OR ISNULL(i.[Telefono_DNI101], '') <> ISNULL(d.[Telefono_DNI101], '')
           OR ISNULL(i.[Email_DNI101], '') <> ISNULL(d.[Email_DNI101], '')
           OR ISNULL(i.[Direccion_DNI101], '') <> ISNULL(d.[Direccion_DNI101], '')
           OR ISNULL(i.[Activo_DNI101], 1) <> ISNULL(d.[Activo_DNI101], 1);
    END
    -- 3. DELETE físico (registrado como BLOQUEAR con Act = 0)
    ELSE IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE c
        SET c.[Act] = 0
        FROM [dbo].[Pacientes_Cambios_DNI101] c
        INNER JOIN deleted d ON c.[IdPaciente_DNI101] = d.[IdPaciente_DNI101];

        INSERT INTO [dbo].[Pacientes_Cambios_DNI101] (
            [IdPaciente_DNI101], [DniNiño_DNI101], [Nombre_DNI101], [Apellido_DNI101],
            [Telefono_DNI101], [Email_DNI101], [Direccion_DNI101], [Activo_DNI101],
            [FechaCambio], [UsuarioDni], [TipoOperacion], [Act]
        )
        SELECT 
            d.[IdPaciente_DNI101], d.[DniNiño_DNI101], d.[Nombre_DNI101], d.[Apellido_DNI101],
            d.[Telefono_DNI101], d.[Email_DNI101], d.[Direccion_DNI101], 0,
            SYSDATETIME(), ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'BLOQUEAR', 0
        FROM deleted d;
    END
END
GO

CREATE OR ALTER TRIGGER [dbo].[trg_ConsultasNutricionales_BitacoraCambios]
ON [dbo].[ConsultasNutricionales_DNI101]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(CAST(SESSION_CONTEXT(N'EsReversion') AS INT), 0) = 1
    BEGIN
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
            [IdConsulta_DNI101], [IdPaciente_DNI101], [DniNutricionista_DNI101], [FechaControl_DNI101],
            [EdadMeses_DNI101], [TipoLactancia_DNI101], [AlimentacionComplementaria_DNI101], [Alergias_DNI101],
            [AntecedentesFamiliares_DNI101], [Observaciones_DNI101], [PesoKg_DNI101], [TallaCm_DNI101],
            [PerimetroCefalicoCm_DNI101], [IMC_DNI101], [ClasificacionOMS_DNI101], [FechaCambio],
            [UsuarioDni], [TipoOperacion], [Act]
        )
        SELECT 
            i.[IdConsulta_DNI101], i.[IdPaciente_DNI101], i.[DniNutricionista_DNI101], i.[FechaControl_DNI101],
            i.[EdadMeses_DNI101], i.[TipoLactancia_DNI101], i.[AlimentacionComplementaria_DNI101], i.[Alergias_DNI101],
            i.[AntecedentesFamiliares_DNI101], i.[Observaciones_DNI101], m.[PesoKg_DNI101], m.[TallaCm_DNI101],
            m.[PerimetroCefalicoCm_DNI101], m.[IMC_DNI101], d.[ClasificacionOMS_DNI101], SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111), 'INSERT', 1
        FROM inserted i
        LEFT JOIN [dbo].[MedicionesAntropometricas_DNI101] m ON i.IdConsulta_DNI101 = m.IdConsulta_DNI101
        LEFT JOIN [dbo].[DiagnosticosNutricionales_DNI101] d ON i.IdConsulta_DNI101 = d.IdConsulta_DNI101;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        IF ISNULL(CAST(SESSION_CONTEXT(N'EsModificacionClinica') AS INT), 0) <> 1
        BEGIN
            IF NOT EXISTS (
                SELECT 1 
                FROM inserted i 
                JOIN deleted d ON i.IdConsulta_DNI101 = d.IdConsulta_DNI101
                WHERE ISNULL(i.IdPaciente_DNI101, 0) <> ISNULL(d.IdPaciente_DNI101, 0)
                    OR ISNULL(i.DniNutricionista_DNI101, 0) <> ISNULL(d.DniNutricionista_DNI101, 0)
                    OR ISNULL(i.FechaControl_DNI101, '1900-01-01') <> ISNULL(d.FechaControl_DNI101, '1900-01-01')
                    OR ISNULL(i.EdadMeses_DNI101, -1) <> ISNULL(d.EdadMeses_DNI101, -1)
                    OR ISNULL(i.TipoLactancia_DNI101, '') <> ISNULL(d.TipoLactancia_DNI101, '')
                    OR ISNULL(i.AlimentacionComplementaria_DNI101, '') <> ISNULL(d.AlimentacionComplementaria_DNI101, '')
                    OR ISNULL(i.Alergias_DNI101, '') <> ISNULL(d.Alergias_DNI101, '')
                    OR ISNULL(i.AntecedentesFamiliares_DNI101, '') <> ISNULL(d.AntecedentesFamiliares_DNI101, '')
                    OR ISNULL(i.Observaciones_DNI101, '') <> ISNULL(d.Observaciones_DNI101, '')
            )
            BEGIN
                RETURN;
            END
        END

        ;WITH Modificadas AS (
            SELECT i.IdConsulta_DNI101
            FROM inserted i
            JOIN deleted d ON i.IdConsulta_DNI101 = d.IdConsulta_DNI101
            WHERE ISNULL(CAST(SESSION_CONTEXT(N'EsModificacionClinica') AS INT), 0) = 1
               OR (
                    ISNULL(i.IdPaciente_DNI101, 0) <> ISNULL(d.IdPaciente_DNI101, 0)
                    OR ISNULL(i.DniNutricionista_DNI101, 0) <> ISNULL(d.DniNutricionista_DNI101, 0)
                    OR ISNULL(i.FechaControl_DNI101, '1900-01-01') <> ISNULL(d.FechaControl_DNI101, '1900-01-01')
                    OR ISNULL(i.EdadMeses_DNI101, -1) <> ISNULL(d.EdadMeses_DNI101, -1)
                    OR ISNULL(i.TipoLactancia_DNI101, '') <> ISNULL(d.TipoLactancia_DNI101, '')
                    OR ISNULL(i.AlimentacionComplementaria_DNI101, '') <> ISNULL(d.AlimentacionComplementaria_DNI101, '')
                    OR ISNULL(i.Alergias_DNI101, '') <> ISNULL(d.Alergias_DNI101, '')
                    OR ISNULL(i.AntecedentesFamiliares_DNI101, '') <> ISNULL(d.AntecedentesFamiliares_DNI101, '')
                    OR ISNULL(i.Observaciones_DNI101, '') <> ISNULL(d.Observaciones_DNI101, '')
               )
        )
        UPDATE c
        SET c.[Act] = 0
        FROM [dbo].[ConsultasNutricionales_Cambios_DNI101] c
        INNER JOIN Modificadas m ON c.[IdConsulta_DNI101] = m.[IdConsulta_DNI101];

        INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
            [IdConsulta_DNI101], [IdPaciente_DNI101], [DniNutricionista_DNI101], [FechaControl_DNI101],
            [EdadMeses_DNI101], [TipoLactancia_DNI101], [AlimentacionComplementaria_DNI101], [Alergias_DNI101],
            [AntecedentesFamiliares_DNI101], [Observaciones_DNI101], [PesoKg_DNI101], [TallaCm_DNI101],
            [PerimetroCefalicoCm_DNI101], [IMC_DNI101], [ClasificacionOMS_DNI101], [FechaCambio],
            [UsuarioDni], [TipoOperacion], [Act]
        )
        SELECT 
            i.[IdConsulta_DNI101], i.[IdPaciente_DNI101], i.[DniNutricionista_DNI101], i.[FechaControl_DNI101],
            i.[EdadMeses_DNI101], i.[TipoLactancia_DNI101], i.[AlimentacionComplementaria_DNI101], i.[Alergias_DNI101],
            i.[AntecedentesFamiliares_DNI101], i.[Observaciones_DNI101], m.[PesoKg_DNI101], m.[TallaCm_DNI101],
            m.[PerimetroCefalicoCm_DNI101], m.[IMC_DNI101], d.[ClasificacionOMS_DNI101], SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111), 'UPDATE', 1
        FROM inserted i
        JOIN deleted del ON i.IdConsulta_DNI101 = del.IdConsulta_DNI101
        LEFT JOIN [dbo].[MedicionesAntropometricas_DNI101] m ON i.IdConsulta_DNI101 = m.IdConsulta_DNI101
        LEFT JOIN [dbo].[DiagnosticosNutricionales_DNI101] d ON i.IdConsulta_DNI101 = d.IdConsulta_DNI101;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE [dbo].[ConsultasNutricionales_Cambios_DNI101]
        SET [Act] = 0
        WHERE [IdConsulta_DNI101] IN (SELECT [IdConsulta_DNI101] FROM deleted);

        INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
            [IdConsulta_DNI101], [IdPaciente_DNI101], [DniNutricionista_DNI101], [FechaControl_DNI101],
            [EdadMeses_DNI101], [TipoLactancia_DNI101], [AlimentacionComplementaria_DNI101], [Alergias_DNI101],
            [AntecedentesFamiliares_DNI101], [Observaciones_DNI101], [PesoKg_DNI101], [TallaCm_DNI101],
            [PerimetroCefalicoCm_DNI101], [IMC_DNI101], [ClasificacionOMS_DNI101], [FechaCambio],
            [UsuarioDni], [TipoOperacion], [Act]
        )
        SELECT 
            d.[IdConsulta_DNI101], d.[IdPaciente_DNI101], d.[DniNutricionista_DNI101], d.[FechaControl_DNI101],
            d.[EdadMeses_DNI101], d.[TipoLactancia_DNI101], d.[AlimentacionComplementaria_DNI101], d.[Alergias_DNI101],
            d.[AntecedentesFamiliares_DNI101], d.[Observaciones_DNI101], NULL, NULL, NULL, NULL, NULL,
            SYSDATETIME(), ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111), 'DELETE', 0
        FROM deleted d;
    END
END
GO

-- ============================================================================
-- 6. LIMPIEZA DE DATOS PREEXISTENTES (SOLO QUEDARÁN SEGURIDAD BASE Y ADMIN)
-- ============================================================================

DELETE FROM [dbo].[Bitacora];
DELETE FROM [dbo].[DV];

DELETE FROM [dbo].[ConsultasNutricionales_Cambios_DNI101];
DELETE FROM [dbo].[Pacientes_Cambios_DNI101];

DELETE FROM [dbo].[AlertasClinicas_DNI101];
DELETE FROM [dbo].[DiagnosticosNutricionales_DNI101];
DELETE FROM [dbo].[MedicionesAntropometricas_DNI101];
DELETE FROM [dbo].[PlanesAlimentarios_DNI101];
DELETE FROM [dbo].[ConsultasNutricionales_DNI101];

DELETE FROM [dbo].[Turnos_DNI101];
DELETE FROM [dbo].[BloquesHorarios_DNI101];
DELETE FROM [dbo].[AgendasMedicas_DNI101];

DELETE FROM [dbo].[Paciente_Emails_DNI101];
DELETE FROM [dbo].[Paciente_Telefonos_DNI101];
DELETE FROM [dbo].[Pacientes_DNI101];

DELETE FROM [dbo].[Usuarios];
DELETE FROM [dbo].[Perfil_Permiso];
DELETE FROM [dbo].[Permiso_Boton];
DELETE FROM [dbo].[Permiso];
DELETE FROM [dbo].[Perfil];
DELETE FROM [dbo].[Familia_Perfil];
DELETE FROM [dbo].[Permiso_Familia];
DELETE FROM [dbo].[Familia_Familia];
DELETE FROM [dbo].[Familia];
GO

-- ============================================================================
-- 7. CARGA DE PERFILES OFICIALES (ADMINISTRADOR Y NUTRICIONISTA ÚNICAMENTE)
-- ============================================================================

SET IDENTITY_INSERT [dbo].[Perfil] ON;
INSERT INTO [dbo].[Perfil] ([ID_Perfil], [Nombre]) VALUES (1, N'Administrador');
INSERT INTO [dbo].[Perfil] ([ID_Perfil], [Nombre]) VALUES (3, N'Nutricionista');
SET IDENTITY_INSERT [dbo].[Perfil] OFF;
GO

-- ============================================================================
-- 8. CARGA DE CATÁLOGO COMPLETO DE PERMISOS (PATENTES) DEL SISTEMA
-- ============================================================================

SET IDENTITY_INSERT [dbo].[Permiso] ON;
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (20, N'Ver Usuarios');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (21, N'Ver Gestion Perfiles');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (22, N'Crear Usuario');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (23, N'Modificar');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (24, N'Desbloquear');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (25, N'ActDesac');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (26, N'Ver Bitacora');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (27, N'Ver Turnos');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (28, N'Ver Ayuda');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (29, N'Cambiar Contraseña');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (30, N'Exportar Bitacora');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (31, N'Ver Seguimientos');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (32, N'Respaldo BD');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (33, N'Ver Clientes');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (34, N'Ver Reporte Predictivo');
INSERT INTO [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (35, N'Ver Bitacora Cambios');
SET IDENTITY_INSERT [dbo].[Permiso] OFF;
GO

-- ============================================================================
-- 9. MAPEO DE CONTROLES VISUALES (BOTONES) A PERMISOS
-- ============================================================================

SET IDENTITY_INSERT [dbo].[Permiso_Boton] ON;
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (1, N'MenuPrincipal', N'btnUsuarios', N'Ver Usuarios');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (2, N'MenuPrincipal', N'btnGestionarPerfiles', N'Ver Gestion Perfiles');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (3, N'GestionUsuario', N'btnCrear', N'Crear Usuario');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (4, N'GestionUsuario', N'btnModificar', N'Modificar');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (5, N'GestionUsuario', N'btnEliminar', N'Desbloquear');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (6, N'GestionUsuario', N'btnActDesact', N'ActDesac');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (7, N'MenuPrincipal', N'btnReportes', N'Ver Bitacora');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (8, N'MenuPrincipal', N'btnTurnos', N'Ver Turnos');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (9, N'MenuPrincipal', N'btnAyuda', N'Ver Ayuda');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (10, N'MenuPrincipal', N'btnCambiarContrasena', N'Cambiar Contraseña');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (11, N'Bitacora', N'btnExportar', N'Exportar Bitacora');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (12, N'MenuPrincipal', N'btnSeguimiento', N'Ver Seguimientos');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (13, N'MenuPrincipal', N'btnRespaldo', N'Respaldo BD');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (14, N'MenuPrincipal', N'btnMaestroClientes', N'Ver Clientes');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (15, N'MenuPrincipal', N'btnSeguimientoNutricional', N'Ver Seguimientos');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (16, N'MenuPrincipal', N'btnReportePredictivo', N'Ver Reporte Predictivo');
INSERT INTO [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (17, N'MenuPrincipal', N'btnBitacoraCambios', N'Ver Bitacora Cambios');
SET IDENTITY_INSERT [dbo].[Permiso_Boton] OFF;
GO

-- ============================================================================
-- 10. ASIGNACIÓN DE PERMISOS POR PERFIL
-- ============================================================================

-- A. PERFIL ADMINISTRADOR (ID_Perfil = 1): Acceso Total
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 20); -- Ver Usuarios
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 21); -- Ver Gestion Perfiles
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 22); -- Crear Usuario
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 23); -- Modificar
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 24); -- Desbloquear
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 25); -- ActDesac
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 26); -- Ver Bitacora (Eventos)
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 27); -- Ver Turnos (PN1)
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 28); -- Ver Ayuda
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 29); -- Cambiar Contraseña
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 30); -- Exportar Bitacora
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 31); -- Ver Seguimientos (PN2)
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 32); -- Respaldo BD
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 33); -- Ver Clientes (PN1)
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 34); -- Ver Reporte Predictivo (PN2)
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 35); -- Ver Bitacora Cambios
GO

-- B. PERFIL NUTRICIONISTA (ID_Perfil = 3): ÚNICAMENTE BOTONES DE PN1 Y PN2
-- Botones PN1: btnTurnos (27) y btnMaestroClientes (33)
-- Botones PN2: btnSeguimiento / btnSeguimientoNutricional (31) y btnReportePredictivo (34)
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 27); -- PN1: btnTurnos
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 33); -- PN1: btnMaestroClientes
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 31); -- PN2: btnSeguimientoNutricional
INSERT INTO [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 34); -- PN2: btnReportePredictivo
GO

-- ============================================================================
-- 11. ÚNICA CUENTA DE USUARIO: ADMINISTRADOR GENERAL
-- Credenciales: admin / 1234
-- Hash BCrypt de '1234': $2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba
-- ============================================================================

INSERT INTO [dbo].[Usuarios] (
    [DNI], 
    [NombreDeUsuario], 
    [Nombre], 
    [Apellido], 
    [Contraseña], 
    [Bloqueado], 
    [Estado], 
    [ID_Perfil], 
    [Idioma], 
    [DV]
) 
VALUES (
    11111111, 
    N'admin', 
    N'Administrador', 
    N'General', 
    N'$2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba', 
    0, 
    1, 
    1, 
    N'Español', 
    NULL
);
GO

PRINT '>>> SCRIPT BASE EJECUTADO CORRECTAMENTE: SEGURIDAD, PERFILES (ADMIN / NUTRICIONISTA) Y ADMIN CREADOS SIN DATOS RESIDUALES <<<';
GO
