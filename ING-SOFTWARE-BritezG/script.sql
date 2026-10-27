CREATE DATABASE ING
USE [ING]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Bitacora' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Bitacora](
	[Id_Event] [int] IDENTITY(1,1) NOT NULL,
	[Dni] [int] NOT NULL,
	[Modulo] [varchar](100) NOT NULL,
	[Criticidad] [int] NOT NULL,
	[Descripcion] [varchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED ([Id_Event] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
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
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Familia' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Familia](
	[ID_Familia] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([ID_Familia] ASC)
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Familia_Familia' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Familia_Familia](
	[ID_FamiliaPadre] [int] NOT NULL,
	[ID_FamiliaHija] [int] NOT NULL,
 CONSTRAINT [PK_Familia_Familia] PRIMARY KEY CLUSTERED ([ID_FamiliaPadre] ASC, [ID_FamiliaHija] ASC)
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Perfil' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Perfil](
	[ID_Perfil] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([ID_Perfil] ASC)
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Perfil_Familia' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Perfil_Familia](
	[ID_Perfil] [int] NOT NULL,
	[ID_Familia] [int] NOT NULL,
 CONSTRAINT [PK_Perfil_Familia] PRIMARY KEY CLUSTERED ([ID_Perfil] ASC, [ID_Familia] ASC)
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Familia_Perfil' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Familia_Perfil](
	[ID_Perfil] [int] NOT NULL,
	[ID_Familia] [int] NOT NULL,
 CONSTRAINT [PK_Familia_Perfil] PRIMARY KEY CLUSTERED ([ID_Perfil] ASC, [ID_Familia] ASC)
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Perfil_Permiso' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Perfil_Permiso](
	[ID_Perfil] [int] NOT NULL,
	[ID_Permiso] [int] NOT NULL,
 CONSTRAINT [PK_Perfil_Permiso] PRIMARY KEY CLUSTERED ([ID_Perfil] ASC, [ID_Permiso] ASC)
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permiso' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Permiso](
	[ID_Permiso] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([ID_Permiso] ASC)
) ON [PRIMARY]
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
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permiso_Familia' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Permiso_Familia](
	[ID_Permiso] [int] NOT NULL,
	[ID_Familia] [int] NOT NULL,
 CONSTRAINT [PK_Permiso_Familia] PRIMARY KEY CLUSTERED ([ID_Permiso] ASC, [ID_Familia] ASC)
) ON [PRIMARY]
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
) ON [PRIMARY]
END
GO

-- DATOS DE SEGURIDAD BASE
IF NOT EXISTS (SELECT * FROM [dbo].[Perfil] WHERE [ID_Perfil] = 1)
BEGIN
    SET IDENTITY_INSERT [dbo].[Perfil] ON;
    INSERT [dbo].[Perfil] ([ID_Perfil], [Nombre]) VALUES (1, N'Administrador');
    INSERT [dbo].[Perfil] ([ID_Perfil], [Nombre]) VALUES (2, N'Usuario');
    INSERT [dbo].[Perfil] ([ID_Perfil], [Nombre]) VALUES (3, N'Nutricionista');
    SET IDENTITY_INSERT [dbo].[Perfil] OFF;
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM [dbo].[Perfil] WHERE [ID_Perfil] = 3)
    BEGIN
        SET IDENTITY_INSERT [dbo].[Perfil] ON;
        INSERT [dbo].[Perfil] ([ID_Perfil], [Nombre]) VALUES (3, N'Nutricionista');
        SET IDENTITY_INSERT [dbo].[Perfil] OFF;
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Perfil] SET [Nombre] = N'Nutricionista' WHERE [ID_Perfil] = 3;
    END
END
GO

-- USUARIO ADMINISTRADOR (admin / 1234)
IF NOT EXISTS (SELECT * FROM [dbo].[Usuarios] WHERE [DNI] = 11111111)
BEGIN
    INSERT [dbo].[Usuarios] ([DNI], [NombreDeUsuario], [Nombre], [Apellido], [Contraseña], [Bloqueado], [Estado], [ID_Perfil], [Idioma], [DV]) 
    VALUES (11111111, N'admin', N'Administrador', N'General', N'$2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba', 0, 1, 1, N'Español', NULL);
END
ELSE
BEGIN
    UPDATE [dbo].[Usuarios]
    SET [NombreDeUsuario] = N'admin',
        [Contraseña] = N'$2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba',
        [Bloqueado] = 0,
        [Estado] = 1,
        [ID_Perfil] = 1
    WHERE [DNI] = 11111111;
END
GO

-- USUARIO NUTRICIONISTA (nutricionista / 1234)
IF NOT EXISTS (SELECT * FROM [dbo].[Usuarios] WHERE [DNI] = 22222222)
BEGIN
    INSERT [dbo].[Usuarios] ([DNI], [NombreDeUsuario], [Nombre], [Apellido], [Contraseña], [Bloqueado], [Estado], [ID_Perfil], [Idioma], [DV]) 
    VALUES (22222222, N'nutricionista', N'Nutricionista', N'Clinico', N'$2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba', 0, 1, 3, N'Español', NULL);
END
ELSE
BEGIN
    UPDATE [dbo].[Usuarios]
    SET [NombreDeUsuario] = N'nutricionista',
        [Contraseña] = N'$2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba',
        [Bloqueado] = 0,
        [Estado] = 1,
        [ID_Perfil] = 3
    WHERE [DNI] = 22222222;
END
GO

-- ALIAS USUARIO NUTRICIONISTA (nutri / 1234)
IF NOT EXISTS (SELECT * FROM [dbo].[Usuarios] WHERE [DNI] = 33333333)
BEGIN
    INSERT [dbo].[Usuarios] ([DNI], [NombreDeUsuario], [Nombre], [Apellido], [Contraseña], [Bloqueado], [Estado], [ID_Perfil], [Idioma], [DV]) 
    VALUES (33333333, N'nutri', N'Nutricionista', N'Staff', N'$2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba', 0, 1, 3, N'Español', NULL);
END
ELSE
BEGIN
    UPDATE [dbo].[Usuarios]
    SET [NombreDeUsuario] = N'nutri',
        [Contraseña] = N'$2a$11$xR8dUaYYFoG3.lY7WPJ.oeR.mlBDKCeUFyQ8zbHbtgAfGuUthDmba',
        [Bloqueado] = 0,
        [Estado] = 1,
        [ID_Perfil] = 3
    WHERE [DNI] = 33333333;
END
GO

-- POBLAR PATENTES (PERMISOS)
IF NOT EXISTS (SELECT * FROM [dbo].[Permiso] WHERE [ID_Permiso] = 20)
BEGIN
    SET IDENTITY_INSERT [dbo].[Permiso] ON;
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (20, N'Ver Usuarios');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (21, N'Ver Gestion Perfiles');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (22, N'Crear Usuario');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (23, N'Modificar');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (24, N'Desbloquear');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (25, N'ActDesac');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (26, N'Ver Bitacora');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (27, N'Ver Turnos');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (28, N'Ver Ayuda');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (29, N'Cambiar Contraseña');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (30, N'Exportar Bitacora');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (31, N'Ver Seguimientos');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (32, N'Respaldo BD');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (33, N'Ver Clientes');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (34, N'Ver Reporte Predictivo');
    INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (35, N'Ver Bitacora Cambios');
    SET IDENTITY_INSERT [dbo].[Permiso] OFF;
END
ELSE
BEGIN
    SET IDENTITY_INSERT [dbo].[Permiso] ON;
    IF NOT EXISTS (SELECT * FROM [dbo].[Permiso] WHERE [ID_Permiso] = 33)
        INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (33, N'Ver Clientes');
    IF NOT EXISTS (SELECT * FROM [dbo].[Permiso] WHERE [ID_Permiso] = 34)
        INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (34, N'Ver Reporte Predictivo');
    IF NOT EXISTS (SELECT * FROM [dbo].[Permiso] WHERE [ID_Permiso] = 35)
        INSERT [dbo].[Permiso] ([ID_Permiso], [Nombre]) VALUES (35, N'Ver Bitacora Cambios');
    SET IDENTITY_INSERT [dbo].[Permiso] OFF;
END
GO

-- POBLAR MAPEO DE BOTONES A PERMISOS
IF NOT EXISTS (SELECT * FROM [dbo].[Permiso_Boton] WHERE [ID_PermisoBoton] = 1)
BEGIN
    SET IDENTITY_INSERT [dbo].[Permiso_Boton] ON;
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (1, N'MenuPrincipal', N'btnUsuarios', N'Ver Usuarios');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (2, N'MenuPrincipal', N'btnGestionarPerfiles', N'Ver Gestion Perfiles');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (3, N'GestionUsuario', N'btnCrear', N'Crear Usuario');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (4, N'GestionUsuario', N'btnModificar', N'Modificar');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (5, N'GestionUsuario', N'btnEliminar', N'Desbloquear');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (6, N'GestionUsuario', N'btnActDesact', N'ActDesac');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (7, N'MenuPrincipal', N'btnReportes', N'Ver Bitacora');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (8, N'MenuPrincipal', N'btnTurnos', N'Ver Turnos');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (9, N'MenuPrincipal', N'btnAyuda', N'Ver Ayuda');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (10, N'MenuPrincipal', N'btnCambiarContrasena', N'Cambiar Contraseña');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (11, N'Bitacora', N'btnExportar', N'Exportar Bitacora');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (12, N'MenuPrincipal', N'btnSeguimiento', N'Ver Seguimientos');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (13, N'MenuPrincipal', N'btnRespaldo', N'Respaldo BD');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (14, N'MenuPrincipal', N'btnMaestroClientes', N'Ver Clientes');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (15, N'MenuPrincipal', N'btnSeguimientoNutricional', N'Ver Seguimientos');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (16, N'MenuPrincipal', N'btnReportePredictivo', N'Ver Reporte Predictivo');
    INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (17, N'MenuPrincipal', N'btnBitacoraCambios', N'Ver Bitacora Cambios');
    SET IDENTITY_INSERT [dbo].[Permiso_Boton] OFF;
END
ELSE
BEGIN
    SET IDENTITY_INSERT [dbo].[Permiso_Boton] ON;
    IF NOT EXISTS (SELECT * FROM [dbo].[Permiso_Boton] WHERE [ID_PermisoBoton] = 14)
        INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (14, N'MenuPrincipal', N'btnMaestroClientes', N'Ver Clientes');
    IF NOT EXISTS (SELECT * FROM [dbo].[Permiso_Boton] WHERE [ID_PermisoBoton] = 15)
        INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (15, N'MenuPrincipal', N'btnSeguimientoNutricional', N'Ver Seguimientos');
    IF NOT EXISTS (SELECT * FROM [dbo].[Permiso_Boton] WHERE [ID_PermisoBoton] = 16)
        INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (16, N'MenuPrincipal', N'btnReportePredictivo', N'Ver Reporte Predictivo');
    IF NOT EXISTS (SELECT * FROM [dbo].[Permiso_Boton] WHERE [ID_PermisoBoton] = 17)
        INSERT [dbo].[Permiso_Boton] ([ID_PermisoBoton], [NombreFormulario], [NombreControl], [NombrePatente]) VALUES (17, N'MenuPrincipal', N'btnBitacoraCambios', N'Ver Bitacora Cambios');
    SET IDENTITY_INSERT [dbo].[Permiso_Boton] OFF;
END
GO

-- ASIGNAR TODOS LOS PERMISOS AL PERFIL ADMINISTRADOR (ID_Perfil = 1)
-- El administrador es el unico que puede ver el sistema completo.
DELETE FROM [dbo].[Perfil_Permiso] WHERE [ID_Perfil] = 1;
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 20);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 21);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 22);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 23);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 24);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 25);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 26);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 27);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 28);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 29);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 30);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 31);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 32);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 33);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 34);
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (1, 35);
GO

-- ASIGNAR PERMISOS AL PERFIL NUTRICIONISTA (ID_Perfil = 3)
-- Un nutricionista unicamente debe ver los botones del PN1 (Turnos, Clientes) y PN2 (Seguimiento, Reporte Predictivo),
-- pero NO puede ver la bitacora de cambios, bitacora de eventos, usuarios, perfiles ni respaldo.
DELETE FROM [dbo].[Perfil_Permiso] WHERE [ID_Perfil] = 3;
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 27); -- Ver Turnos (PN1)
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 33); -- Ver Clientes (PN1)
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 31); -- Ver Seguimientos (PN2)
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 34); -- Ver Reporte Predictivo (PN2)
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 28); -- Ver Ayuda
INSERT [dbo].[Perfil_Permiso] ([ID_Perfil], [ID_Permiso]) VALUES (3, 29); -- Cambiar Contraseña
GO

-- TABLAS DE PN1: TURNERO NUTRICIONAL
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
        [DV] VARCHAR(255) NULL
    );
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

-- TABLAS DE PN2: SEGUIMIENTO NUTRICIONAL PEDIÁTRICO
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
END
GO

-- ============================================================================
-- SCRIPT DE DATOS DE PRUEBA: 4 PACIENTES PEDIÁTRICOS CON HISTORIAL LARGO (PN2)
-- DNI FÁCILES DE PROBAR:
--  1111: Mateo Benítez (Lactante 0-24m, Crecimiento Normal / Eutrófico - 7 Controles)
--  2222: Sofía Rossi (Preescolar 2-5a, Curva de Sobrepeso / Obesidad - 6 Controles)
--  3333: Thiago Gómez (Lactante 0-24m, ⚠️ ALERTA CLÍNICA CRÍTICA: Talla Baja Severa - 6 Controles)
--  4444: Emma Martínez (Preescolar 2-5a, Percentil 50 Estable - 5 Controles)
-- ============================================================================

-- PACIENTE 1: DNI 1111
IF NOT EXISTS (SELECT * FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '1111')
BEGIN
    INSERT INTO Pacientes_DNI101 (DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101)
    VALUES ('1111', N'Mateo', N'Benítez', '11-4455-6677', 'mateo.benitez@test.com', '2024-01-15', N'Masculino', N'OSDE 310');
END

-- PACIENTE 2: DNI 2222
IF NOT EXISTS (SELECT * FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '2222')
BEGIN
    INSERT INTO Pacientes_DNI101 (DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101)
    VALUES ('2222', N'Sofía', N'Rossi', '11-5566-7788', 'sofia.rossi@test.com', '2021-05-10', N'Femenino', N'Swiss Medical');
END

-- PACIENTE 3: DNI 3333 (ALERTA CLÍNICA)
IF NOT EXISTS (SELECT * FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '3333')
BEGIN
    INSERT INTO Pacientes_DNI101 (DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101)
    VALUES ('3333', N'Thiago', N'Gómez', '11-6677-8899', 'thiago.gomez@test.com', '2023-06-20', N'Masculino', N'Particular');
END

-- PACIENTE 4: DNI 4444
IF NOT EXISTS (SELECT * FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '4444')
BEGIN
    INSERT INTO Pacientes_DNI101 (DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101, Telefono_DNI101, Email_DNI101, FechaNacimiento_DNI101, Sexo_DNI101, ObraSocial_DNI101)
    VALUES ('4444', N'Emma', N'Martínez', '11-7788-9900', 'emma.martinez@test.com', '2022-03-01', N'Femenino', N'Galeno');
END
GO
IF NOT EXISTS (SELECT 1 FROM ConsultasNutricionales_DNI101)
BEGIN
    DECLARE @IdP1 INT, @IdP2 INT, @IdP3 INT, @IdP4 INT;
    SELECT @IdP1 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '1111';
    SELECT @IdP2 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '2222';
    SELECT @IdP3 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '3333';
    SELECT @IdP4 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '4444';

    DECLARE @IdConsulta INT;

-- HISTORIAL PACIENTE 1 (DNI 1111 - Mateo Benítez): 7 CONTROLES NORMALES OMS
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101) VALUES (@IdP1, 11111111, '2024-02-15', 1, N'Materna Exclusiva', N'No', N'Primer control del lactante. Reflejos conservados.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 4.50, 54.50, 37.50, 0, 15.13);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.10, 0.20, 0.00, 0.15, 53.98, 57.93, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Lactante con desarrollo normopeso acorde a su edad.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101) VALUES (@IdP1, 11111111, '2024-04-15', 3, N'Materna Exclusiva', N'No', N'Buen agarre y succión.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 6.40, 61.20, 40.50, 0, 17.09);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.20, 0.10, 0.20, 0.10, 57.93, 53.98, 57.93);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Crecimiento constante siguiendo la curva P50.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101) VALUES (@IdP1, 11111111, '2024-07-15', 6, N'Predominante', N'Inicio de papillas y frutas', N'Comienza alimentación complementaria con excelente tolerancia.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 7.90, 67.60, 43.20, 0, 17.29);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.10, 0.00, 0.10, 0.05, 53.98, 50.00, 53.98);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Transición adecuada a sólidos.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101) VALUES (@IdP1, 11111111, '2024-10-15', 9, N'Parcial / Mixta', N'Purés, carnes magras, vegetales', N'Crecimiento longitudinal muy armónico.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 8.90, 72.00, 45.00, 0, 17.17);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.00, -0.10, 0.10, 0.00, 50.00, 46.02, 53.98);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Estado nutricional óptimo.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101) VALUES (@IdP1, 11111111, '2025-01-15', 12, N'Complementaria', N'Incorporado a la mesa familiar', N'Cumple 1 año. Marcha independiente iniciada.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 9.60, 75.70, 46.20, 0, 16.75);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -0.10, -0.10, -0.10, -0.05, 46.02, 46.02, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Desarrollo psicomotor y nutricional excelente.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101) VALUES (@IdP1, 11111111, '2025-04-15', 15, N'Destetado', N'Dieta completa y variada', N'Controles pediátricos de rutina.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 10.30, 79.10, 47.10, 0, 16.46);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.00, 0.00, -0.10, 0.00, 50.00, 50.00, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Saludable.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101) VALUES (@IdP1, 11111111, '2025-09-15', 20, N'Destetado', N'Alimentación equilibrada', N'Último control de seguimiento normopeso.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 11.40, 84.50, 48.00, 0, 15.97);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.10, 0.10, 0.00, 0.05, 53.98, 53.98, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Estado nutricional excelente. Continuar pautas habituales.', 0);

-- HISTORIAL PACIENTE 2 (DNI 2222 - Sofía Rossi): 6 CONTROLES CURVA SOBREPESO/OBESIDAD
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP2, 11111111, '2023-05-10', 24, N'Consulta por hábito alimentario con alto consumo de ultraprocesados.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 13.50, 86.00, 48.20, 50.00, 18.25);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 1.20, 0.30, 1.40, 0.40, 88.49, 61.79, 91.92);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Riesgo de Sobrepeso', N'Inicia leve ascenso percentilar de IMC.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP2, 11111111, '2023-11-10', 30, N'Consumo de jugos azucarados frecuente.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 15.20, 91.50, 49.00, 52.50, 18.16);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 1.50, 0.40, 1.60, 0.50, 93.32, 65.54, 94.52);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Sobrepeso', N'Z-IMC mayor a +1.5. Se refuerzan pautas familiares.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP2, 11111111, '2024-05-10', 36, N'Sedentarismo incrementado.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 17.00, 96.00, 49.60, 55.00, 18.44);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 1.70, 0.50, 1.80, 0.50, 95.54, 69.15, 96.41);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Sobrepeso', N'Mantiene curva de sobrepeso por encima de P95.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP2, 11111111, '2024-11-10', 42, N'Aumento ponderal acelerado.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 19.10, 100.50, 50.10, 58.00, 18.91);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 2.00, 0.60, 2.10, 0.50, 97.72, 72.57, 98.21);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Obesidad Moderada', N'Z-IMC cruza la línea +2.0 SD (Obesidad OMS).', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP2, 11111111, '2025-05-10', 48, N'Se prescribe plan de alimentación estructurada.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 21.50, 104.50, 50.50, 61.00, 19.69);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 2.30, 0.70, 2.40, 0.60, 98.93, 75.80, 99.18);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Obesidad Severa', N'Tendencia sostenida en percentil > 99.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP2, 11111111, '2025-09-10', 52, N'Último control. En tratamiento nutricional multifactorial.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 23.20, 107.00, 50.80, 63.00, 20.26);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 2.50, 0.70, 2.60, 0.60, 99.38, 75.80, 99.53);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Obesidad Severa Pediátrica', N'Seguimiento intensivo de pautas de actividad física y nutrición.', 0);

-- HISTORIAL PACIENTE 3 (DNI 3333 - Thiago Gómez): ⚠️ ALERTA CLÍNICA ROJA
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP3, 11111111, '2023-09-20', 3, N'Lactante con leve hiporexia.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 5.80, 60.00, 39.50, 0, 16.11);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -0.60, -0.50, -0.40, -0.30, 27.43, 30.85, 34.46);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Monitoreo por ingesta limítrofe.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP3, 11111111, '2023-12-20', 6, N'Dificultad en la incorporación de papillas.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 6.80, 63.50, 41.00, 0, 16.86);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -1.10, -1.20, -0.60, -0.50, 13.57, 11.51, 27.43);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Riesgo de Desnutrición', N'Pérdida de carril percentilar de talla y peso.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP3, 11111111, '2024-03-20', 9, N'Astenia y palidez cutánea.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 7.20, 66.00, 42.10, 0, 16.53);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -1.80, -2.10, -0.90, -0.80, 3.59, 1.79, 18.41);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Desnutrición Moderada / Talla Baja', N'Caída de Z-Talla por debajo de -2.00 SD.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP3, 11111111, '2024-06-20', 12, N'Escasa ganancia estatural en 6 meses.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 7.50, 68.00, 42.80, 0, 16.22);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -2.30, -2.70, -1.20, -1.00, 1.07, 0.35, 11.51);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Desnutrición Severa', N'Aplanamiento severo de la curva de longitud.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP3, 11111111, '2024-09-20', 15, N'Derivado a Gastroenterología Pediátrica.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 7.80, 69.50, 43.30, 0, 16.15);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -2.60, -3.00, -1.40, -1.20, 0.47, 0.13, 8.08);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Talla Baja Severa', N'Z-Score Talla/Edad cruza el límite de -3.00 SD.', 1);
INSERT INTO AlertasClinicas_DNI101 (IdConsulta_DNI101, TipoAlerta_DNI101, Severidad_DNI101, MensajeAlerta_DNI101, FechaGeneracion_DNI101) VALUES (@IdConsulta, N'Desnutrición / Talla Baja Severa', N'CRÍTICA', N'🚨 ALERTA ROJA: Z-Score Talla/Edad de -3.00 (Talla Baja Severa). Requiere intervención urgente.', '2024-09-20');

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP3, 11111111, '2024-12-20', 18, N'Control de seguimiento en curso con soporte nutricional enteral.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 8.00, 71.00, 43.80, 0, 15.87);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -2.80, -3.20, -1.60, -1.30, 0.26, 0.07, 5.48);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Talla Baja Severa / Desnutrición Crónica', N'Desnutrición crónica reagudizada. Z-Talla de -3.20 SD. El paciente presenta un retardo severo del crecimiento.', 1);
INSERT INTO AlertasClinicas_DNI101 (IdConsulta_DNI101, TipoAlerta_DNI101, Severidad_DNI101, MensajeAlerta_DNI101, FechaGeneracion_DNI101) VALUES (@IdConsulta, N'Desnutrición / Talla Baja Severa', N'CRÍTICA', N'🚨 ALERTA ROJA: Z-Score Talla/Edad de -3.20 (Talla Baja Severa / Desnutrición Crónica). Se requiere intervención nutricional urgente e interconsulta médica.', '2024-12-20');

-- HISTORIAL PACIENTE 4 (DNI 4444 - Emma Martínez): 5 CONTROLES PERCENTIL 50 ESTABLE
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP4, 11111111, '2023-03-01', 12, N'Control al año de vida.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 8.90, 74.00, 45.10, 0, 16.25);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -0.10, -0.10, -0.10, -0.05, 46.02, 46.02, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Desarrollo normal.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP4, 11111111, '2023-09-01', 18, N'Control de rutina.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 10.20, 80.50, 46.40, 0, 15.74);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.00, 0.00, 0.00, 0.00, 50.00, 50.00, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Saludable.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP4, 11111111, '2024-03-01', 24, N'Cumple 2 años. Curva impecable.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 11.50, 85.50, 47.50, 48.00, 15.73);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.10, 0.00, 0.10, 0.05, 53.98, 50.00, 53.98);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Excelente estado general.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP4, 11111111, '2024-09-01', 30, N'Seguimiento semestral.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 12.70, 90.00, 48.30, 49.50, 15.68);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, 0.00, 0.00, 0.00, 0.00, 50.00, 50.00, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Alimentación variada.', 0);

INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101) VALUES (@IdP4, 11111111, '2025-03-01', 36, N'Control a los 3 años de vida.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101) VALUES (@IdConsulta, 13.90, 95.00, 49.00, 51.00, 15.40);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101) VALUES (@IdConsulta, -0.10, 0.00, -0.10, 0.00, 46.02, 50.00, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101) VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Niña normopeso con desarrollo antropométrico óptimo.', 0);
END
GO

--------------------------------------------------------------------------------
-- ACTUALIZACIÓN SCHEMA PACIENTES: DIRECCIÓN Y BORRADO LÓGICO
--------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pacientes_DNI101]') AND name = 'Direccion_DNI101')
BEGIN
    ALTER TABLE [dbo].[Pacientes_DNI101] ADD [Direccion_DNI101] VARCHAR(200) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Pacientes_DNI101]') AND name = 'Activo_DNI101')
BEGIN
    ALTER TABLE [dbo].[Pacientes_DNI101] ADD [Activo_DNI101] BIT NOT NULL CONSTRAINT DF_Pacientes_Activo DEFAULT 1;
END
GO

--------------------------------------------------------------------------------
-- TABLAS 1:N DE CONTACTOS DE PACIENTES / CLIENTES (EMAILS Y TELÉFONOS/CELULARES)
--------------------------------------------------------------------------------
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

--------------------------------------------------------------------------------
-- TABLA BITÁCORA DE CAMBIOS: CONSULTAS NUTRICIONALES (HISTORIAL PARA ROLLBACK)
--------------------------------------------------------------------------------
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
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ConsultasNutricionales_Cambios_DNI101]') AND name = 'Act')
    BEGIN
        ALTER TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101] ADD [Act] BIT NOT NULL CONSTRAINT DF_ConsultasCambios_Act DEFAULT 1;
    END
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ConsultasNutricionales_Cambios_DNI101]') AND name = 'PesoKg_DNI101')
    BEGIN
        ALTER TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101] ADD [PesoKg_DNI101] DECIMAL(5,2) NULL;
    END
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ConsultasNutricionales_Cambios_DNI101]') AND name = 'TallaCm_DNI101')
    BEGIN
        ALTER TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101] ADD [TallaCm_DNI101] DECIMAL(5,2) NULL;
    END
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ConsultasNutricionales_Cambios_DNI101]') AND name = 'PerimetroCefalicoCm_DNI101')
    BEGIN
        ALTER TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101] ADD [PerimetroCefalicoCm_DNI101] DECIMAL(5,2) NULL;
    END
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ConsultasNutricionales_Cambios_DNI101]') AND name = 'IMC_DNI101')
    BEGIN
        ALTER TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101] ADD [IMC_DNI101] DECIMAL(5,2) NULL;
    END
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ConsultasNutricionales_Cambios_DNI101]') AND name = 'ClasificacionOMS_DNI101')
    BEGIN
        ALTER TABLE [dbo].[ConsultasNutricionales_Cambios_DNI101] ADD [ClasificacionOMS_DNI101] VARCHAR(100) NULL;
    END
END
GO

--------------------------------------------------------------------------------
-- TRIGGER EN CONSULTAS NUTRICIONALES: BITÁCORA DE CAMBIOS CUN08 (VERSIONADO HISTÓRICO Y REVERSIÓN)
--------------------------------------------------------------------------------
CREATE OR ALTER TRIGGER [dbo].[trg_ConsultasNutricionales_BitacoraCambios]
ON [dbo].[ConsultasNutricionales_DNI101]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Si es una reversión o activación controlada desde la aplicación, no generar duplicados en la bitácora
    IF ISNULL(CAST(SESSION_CONTEXT(N'EsReversion') AS INT), 0) = 1
    BEGIN
        RETURN;
    END

    -- Ante INSERT (nueva consulta nutricional): registrar en la bitácora con Act = 1
    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
            [IdConsulta_DNI101],
            [IdPaciente_DNI101],
            [DniNutricionista_DNI101],
            [FechaControl_DNI101],
            [EdadMeses_DNI101],
            [TipoLactancia_DNI101],
            [AlimentacionComplementaria_DNI101],
            [Alergias_DNI101],
            [AntecedentesFamiliares_DNI101],
            [Observaciones_DNI101],
            [PesoKg_DNI101],
            [TallaCm_DNI101],
            [PerimetroCefalicoCm_DNI101],
            [IMC_DNI101],
            [ClasificacionOMS_DNI101],
            [FechaCambio],
            [UsuarioDni],
            [TipoOperacion],
            [Act]
        )
        SELECT 
            i.[IdConsulta_DNI101],
            i.[IdPaciente_DNI101],
            i.[DniNutricionista_DNI101],
            i.[FechaControl_DNI101],
            i.[EdadMeses_DNI101],
            i.[TipoLactancia_DNI101],
            i.[AlimentacionComplementaria_DNI101],
            i.[Alergias_DNI101],
            i.[AntecedentesFamiliares_DNI101],
            i.[Observaciones_DNI101],
            m.[PesoKg_DNI101],
            m.[TallaCm_DNI101],
            m.[PerimetroCefalicoCm_DNI101],
            m.[IMC_DNI101],
            d.[ClasificacionOMS_DNI101],
            SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'INSERT',
            1
        FROM inserted i
        LEFT JOIN [dbo].[MedicionesAntropometricas_DNI101] m ON i.IdConsulta_DNI101 = m.IdConsulta_DNI101
        LEFT JOIN [dbo].[DiagnosticosNutricionales_DNI101] d ON i.IdConsulta_DNI101 = d.IdConsulta_DNI101;
    END
    -- Ante UPDATE: marcar las versiones anteriores de esa consulta con Act = 0 y registrar la nueva versión activa con Act = 1
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        -- Si la modificación fue por recálculo de Dígitos Verificadores (DV) y no es modificación clínica explícita, omitir
        IF ISNULL(CAST(SESSION_CONTEXT(N'EsModificacionClinica') AS INT), 0) <> 1
        BEGIN
            IF NOT EXISTS (
                SELECT 1 
                FROM inserted i 
                JOIN deleted d ON i.IdConsulta_DNI101 = d.IdConsulta_DNI101
                WHERE 
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
            BEGIN
                RETURN;
            END
        END

        -- Solo modificar y registrar aquellas filas que realmente representan cambios clínicos
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
                    OR ISNULL(i.AlimentacionComplementaria_DNI101, '') <> ISNULL(del.AlimentacionComplementaria_DNI101, '')
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
            [IdConsulta_DNI101],
            [IdPaciente_DNI101],
            [DniNutricionista_DNI101],
            [FechaControl_DNI101],
            [EdadMeses_DNI101],
            [TipoLactancia_DNI101],
            [AlimentacionComplementaria_DNI101],
            [Alergias_DNI101],
            [AntecedentesFamiliares_DNI101],
            [Observaciones_DNI101],
            [PesoKg_DNI101],
            [TallaCm_DNI101],
            [PerimetroCefalicoCm_DNI101],
            [IMC_DNI101],
            [ClasificacionOMS_DNI101],
            [FechaCambio],
            [UsuarioDni],
            [TipoOperacion],
            [Act]
        )
        SELECT 
            i.[IdConsulta_DNI101],
            i.[IdPaciente_DNI101],
            i.[DniNutricionista_DNI101],
            i.[FechaControl_DNI101],
            i.[EdadMeses_DNI101],
            i.[TipoLactancia_DNI101],
            i.[AlimentacionComplementaria_DNI101],
            i.[Alergias_DNI101],
            i.[AntecedentesFamiliares_DNI101],
            i.[Observaciones_DNI101],
            m.[PesoKg_DNI101],
            m.[TallaCm_DNI101],
            m.[PerimetroCefalicoCm_DNI101],
            m.[IMC_DNI101],
            d.[ClasificacionOMS_DNI101],
            SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'UPDATE',
            1
        FROM inserted i
        JOIN deleted del ON i.IdConsulta_DNI101 = del.IdConsulta_DNI101
        LEFT JOIN [dbo].[MedicionesAntropometricas_DNI101] m ON i.IdConsulta_DNI101 = m.IdConsulta_DNI101
        LEFT JOIN [dbo].[DiagnosticosNutricionales_DNI101] d ON i.IdConsulta_DNI101 = d.IdConsulta_DNI101
        WHERE ISNULL(CAST(SESSION_CONTEXT(N'EsModificacionClinica') AS INT), 0) = 1
           OR (
                ISNULL(i.IdPaciente_DNI101, 0) <> ISNULL(del.IdPaciente_DNI101, 0)
                OR ISNULL(i.DniNutricionista_DNI101, 0) <> ISNULL(del.DniNutricionista_DNI101, 0)
                OR ISNULL(i.FechaControl_DNI101, '1900-01-01') <> ISNULL(del.FechaControl_DNI101, '1900-01-01')
                OR ISNULL(i.EdadMeses_DNI101, -1) <> ISNULL(del.EdadMeses_DNI101, -1)
                OR ISNULL(i.TipoLactancia_DNI101, '') <> ISNULL(del.TipoLactancia_DNI101, '')
                OR ISNULL(i.AlimentacionComplementaria_DNI101, '') <> ISNULL(del.AlimentacionComplementaria_DNI101, '')
                OR ISNULL(i.Alergias_DNI101, '') <> ISNULL(del.Alergias_DNI101, '')
                OR ISNULL(i.AntecedentesFamiliares_DNI101, '') <> ISNULL(del.AntecedentesFamiliares_DNI101, '')
                OR ISNULL(i.Observaciones_DNI101, '') <> ISNULL(del.Observaciones_DNI101, '')
           );
    END
    -- Ante DELETE: marcar en 0 las versiones de esa consulta y registrar el borrado con Act = 0
    ELSE IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE [dbo].[ConsultasNutricionales_Cambios_DNI101]
        SET [Act] = 0
        WHERE [IdConsulta_DNI101] IN (SELECT [IdConsulta_DNI101] FROM deleted);

        INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
            [IdConsulta_DNI101],
            [IdPaciente_DNI101],
            [DniNutricionista_DNI101],
            [FechaControl_DNI101],
            [EdadMeses_DNI101],
            [TipoLactancia_DNI101],
            [AlimentacionComplementaria_DNI101],
            [Alergias_DNI101],
            [AntecedentesFamiliares_DNI101],
            [Observaciones_DNI101],
            [PesoKg_DNI101],
            [TallaCm_DNI101],
            [PerimetroCefalicoCm_DNI101],
            [IMC_DNI101],
            [ClasificacionOMS_DNI101],
            [FechaCambio],
            [UsuarioDni],
            [TipoOperacion],
            [Act]
        )
        SELECT 
            d.[IdConsulta_DNI101],
            d.[IdPaciente_DNI101],
            d.[DniNutricionista_DNI101],
            d.[FechaControl_DNI101],
            d.[EdadMeses_DNI101],
            d.[TipoLactancia_DNI101],
            d.[AlimentacionComplementaria_DNI101],
            d.[Alergias_DNI101],
            d.[AntecedentesFamiliares_DNI101],
            d.[Observaciones_DNI101],
            NULL, NULL, NULL, NULL, NULL,
            SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'DELETE',
            0
        FROM deleted d;
    END
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConsultasNutricionales_Cambios_DNI101])
BEGIN
    INSERT INTO [dbo].[ConsultasNutricionales_Cambios_DNI101] (
        [IdConsulta_DNI101],
        [IdPaciente_DNI101],
        [DniNutricionista_DNI101],
        [FechaControl_DNI101],
        [EdadMeses_DNI101],
        [TipoLactancia_DNI101],
        [AlimentacionComplementaria_DNI101],
        [Alergias_DNI101],
        [AntecedentesFamiliares_DNI101],
        [Observaciones_DNI101],
        [FechaCambio],
        [UsuarioDni],
        [TipoOperacion],
        [Act]
    )
    SELECT 
        c.[IdConsulta_DNI101],
        c.[IdPaciente_DNI101],
        c.[DniNutricionista_DNI101],
        c.[FechaControl_DNI101],
        c.[EdadMeses_DNI101],
        c.[TipoLactancia_DNI101],
        c.[AlimentacionComplementaria_DNI101],
        c.[Alergias_DNI101],
        c.[AntecedentesFamiliares_DNI101],
        c.[Observaciones_DNI101],
        c.[FechaControl_DNI101],
        11111111,
        'INSERT',
        1
    FROM [dbo].[ConsultasNutricionales_DNI101] c;
END
GO

--------------------------------------------------------------------------------
-- POBLAR DIRECCIONES INICIALES PARA PACIENTES EXISTENTES
--------------------------------------------------------------------------------
UPDATE [dbo].[Pacientes_DNI101] SET [Direccion_DNI101] = 'Av. Santa Fe 3420, CABA' WHERE [DniNiño_DNI101] = '1111' AND ([Direccion_DNI101] IS NULL OR [Direccion_DNI101] = '');
UPDATE [dbo].[Pacientes_DNI101] SET [Direccion_DNI101] = 'Belgrano 1250, Quilmes' WHERE [DniNiño_DNI101] = '2222' AND ([Direccion_DNI101] IS NULL OR [Direccion_DNI101] = '');
UPDATE [dbo].[Pacientes_DNI101] SET [Direccion_DNI101] = 'Mitre 450, Avellaneda' WHERE [DniNiño_DNI101] = '3333' AND ([Direccion_DNI101] IS NULL OR [Direccion_DNI101] = '');
UPDATE [dbo].[Pacientes_DNI101] SET [Direccion_DNI101] = 'Rivadavia 8840, CABA' WHERE [DniNiño_DNI101] = '4444' AND ([Direccion_DNI101] IS NULL OR [Direccion_DNI101] = '');
GO

--------------------------------------------------------------------------------
-- POBLAR CONTACTOS 1:N INICIALES PARA PACIENTES EXISTENTES (SI NO EXISTEN)
--------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM [dbo].[Paciente_Emails_DNI101])
BEGIN
    -- 1. Insertar el email principal de cada paciente
    INSERT INTO [dbo].[Paciente_Emails_DNI101] ([IdPaciente_DNI101], [Email_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
    SELECT [IdPaciente_DNI101], [Email_DNI101], 1, 1 
    FROM [dbo].[Pacientes_DNI101] 
    WHERE [Email_DNI101] IS NOT NULL AND [Email_DNI101] <> '';

    -- 2. Insertar contactos de email secundarios familiares para demostrar multiplicidad 1:N
    DECLARE @IdPMateoE INT, @IdPSofiaE INT, @IdPThiagoE INT, @IdPEmmaE INT;
    SELECT @IdPMateoE = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '1111';
    SELECT @IdPSofiaE = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '2222';
    SELECT @IdPThiagoE = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '3333';
    SELECT @IdPEmmaE = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '4444';

    IF @IdPMateoE IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Emails_DNI101] ([IdPaciente_DNI101], [Email_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES 
            (@IdPMateoE, 'padre.benitez@empresa.com', 0, 1),
            (@IdPMateoE, 'abuela.rosa@hotmail.com', 0, 1);
    END

    IF @IdPSofiaE IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Emails_DNI101] ([IdPaciente_DNI101], [Email_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES (@IdPSofiaE, 'mariana.rossi@yahoo.com', 0, 1);
    END

    IF @IdPThiagoE IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Emails_DNI101] ([IdPaciente_DNI101], [Email_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES (@IdPThiagoE, 'gomez.lucas@outlook.com', 0, 1);
    END

    IF @IdPEmmaE IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Emails_DNI101] ([IdPaciente_DNI101], [Email_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES (@IdPEmmaE, 'dra.martinez@fibertel.com.ar', 0, 1);
    END
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Paciente_Telefonos_DNI101])
BEGIN
    -- 1. Insertar el teléfono principal de cada paciente
    INSERT INTO [dbo].[Paciente_Telefonos_DNI101] ([IdPaciente_DNI101], [Numero_DNI101], [Tipo_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
    SELECT [IdPaciente_DNI101], [Telefono_DNI101], 'Celular Madre', 1, 1 
    FROM [dbo].[Pacientes_DNI101] 
    WHERE [Telefono_DNI101] IS NOT NULL AND [Telefono_DNI101] <> '';

    -- 2. Insertar teléfonos secundarios familiares (celular padre, fijo hogar, laboral)
    DECLARE @IdPMateoT INT, @IdPSofiaT INT, @IdPThiagoT INT, @IdPEmmaT INT;
    SELECT @IdPMateoT = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '1111';
    SELECT @IdPSofiaT = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '2222';
    SELECT @IdPThiagoT = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '3333';
    SELECT @IdPEmmaT = IdPaciente_DNI101 FROM [dbo].[Pacientes_DNI101] WHERE DniNiño_DNI101 = '4444';

    IF @IdPMateoT IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Telefonos_DNI101] ([IdPaciente_DNI101], [Numero_DNI101], [Tipo_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES 
            (@IdPMateoT, '11-6677-8899', 'Celular Padre', 0, 1),
            (@IdPMateoT, '011-4780-1234', 'Fijo Hogar', 0, 1);
    END

    IF @IdPSofiaT IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Telefonos_DNI101] ([IdPaciente_DNI101], [Numero_DNI101], [Tipo_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES (@IdPSofiaT, '11-8899-0011', 'Celular Padre', 0, 1);
    END

    IF @IdPThiagoT IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Telefonos_DNI101] ([IdPaciente_DNI101], [Numero_DNI101], [Tipo_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES (@IdPThiagoT, '11-2233-4455', 'Fijo Abuelos', 0, 1);
    END

    IF @IdPEmmaT IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Paciente_Telefonos_DNI101] ([IdPaciente_DNI101], [Numero_DNI101], [Tipo_DNI101], [EsPrincipal_DNI101], [Activo_DNI101])
        VALUES (@IdPEmmaT, '011-4552-1122', 'Laboral Padre', 0, 1);
    END
END
GO

--------------------------------------------------------------------------------
-- TABLA DE AUDITORÍA: HISTÓRICO DE CAMBIOS DE PLANES ALIMENTARIOS
--------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlanesAlimentarios_Cambios_DNI101' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[PlanesAlimentarios_Cambios_DNI101](
    [IdCambio] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPlan_DNI101] INT NOT NULL,
    [IdConsulta_DNI101] INT NOT NULL,
    [RequerimientoCalorico_DNI101] DECIMAL(18,2) NOT NULL,
    [PctCarbohidratos_DNI101] DECIMAL(18,2) NOT NULL,
    [PctProteinas_DNI101] DECIMAL(18,2) NOT NULL,
    [PctGrasas_DNI101] DECIMAL(18,2) NOT NULL,
    [PautasFamiliares_DNI101] VARCHAR(MAX) NULL,
    [MetasSalud_DNI101] VARCHAR(MAX) NULL,
    [FechaCambio] DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    [UsuarioDni] INT NOT NULL DEFAULT 11111111,
    [TipoOperacion] VARCHAR(20) NOT NULL
) ON [PRIMARY];
END
GO

--------------------------------------------------------------------------------
-- TRIGGER EN PLANES ALIMENTARIOS: REGISTRA ESTADO PREVIO ANTE UPDATE O DELETE
--------------------------------------------------------------------------------
CREATE OR ALTER TRIGGER [dbo].[trg_PlanesAlimentarios_BitacoraCambios]
ON [dbo].[PlanesAlimentarios_DNI101]
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[PlanesAlimentarios_Cambios_DNI101] (
            [IdPlan_DNI101],
            [IdConsulta_DNI101],
            [RequerimientoCalorico_DNI101],
            [PctCarbohidratos_DNI101],
            [PctProteinas_DNI101],
            [PctGrasas_DNI101],
            [PautasFamiliares_DNI101],
            [MetasSalud_DNI101],
            [FechaCambio],
            [UsuarioDni],
            [TipoOperacion]
        )
        SELECT 
            d.[IdPlan_DNI101],
            d.[IdConsulta_DNI101],
            d.[RequerimientoCalorico_DNI101],
            d.[PctCarbohidratos_DNI101],
            d.[PctProteinas_DNI101],
            d.[PctGrasas_DNI101],
            d.[PautasFamiliares_DNI101],
            d.[MetasSalud_DNI101],
            SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'UPDATE'
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[PlanesAlimentarios_Cambios_DNI101] (
            [IdPlan_DNI101],
            [IdConsulta_DNI101],
            [RequerimientoCalorico_DNI101],
            [PctCarbohidratos_DNI101],
            [PctProteinas_DNI101],
            [PctGrasas_DNI101],
            [PautasFamiliares_DNI101],
            [MetasSalud_DNI101],
            [FechaCambio],
            [UsuarioDni],
            [TipoOperacion]
        )
        SELECT 
            d.[IdPlan_DNI101],
            d.[IdConsulta_DNI101],
            d.[RequerimientoCalorico_DNI101],
            d.[PctCarbohidratos_DNI101],
            d.[PctProteinas_DNI101],
            d.[PctGrasas_DNI101],
            d.[PautasFamiliares_DNI101],
            d.[MetasSalud_DNI101],
            SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'DELETE'
        FROM deleted d;
    END
END
GO

--------------------------------------------------------------------------------
-- TABLA DE AUDITORÍA: HISTÓRICO DE CAMBIOS DE PACIENTES
--------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pacientes_Cambios_DNI101' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Pacientes_Cambios_DNI101](
    [IdCambio] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPaciente_DNI101] INT NOT NULL,
    [DniNiño_DNI101] VARCHAR(20) NOT NULL,
    [Nombre_DNI101] VARCHAR(100) NOT NULL,
    [Apellido_DNI101] VARCHAR(100) NOT NULL,
    [Telefono_DNI101] VARCHAR(50) NULL,
    [Email_DNI101] VARCHAR(100) NULL,
    [Direccion_DNI101] VARCHAR(200) NULL,
    [Activo_DNI101] BIT NOT NULL DEFAULT 1,
    [FechaCambio] DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    [UsuarioDni] INT NOT NULL DEFAULT 11111111,
    [TipoOperacion] VARCHAR(20) NOT NULL
) ON [PRIMARY];
END
GO

--------------------------------------------------------------------------------
-- TRIGGER EN PACIENTES: REGISTRA ESTADO PREVIO ANTE UPDATE O DELETE
--------------------------------------------------------------------------------
CREATE OR ALTER TRIGGER [dbo].[trg_Pacientes_BitacoraCambios]
ON [dbo].[Pacientes_DNI101]
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[Pacientes_Cambios_DNI101] (
            [IdPaciente_DNI101],
            [DniNiño_DNI101],
            [Nombre_DNI101],
            [Apellido_DNI101],
            [Telefono_DNI101],
            [Email_DNI101],
            [Direccion_DNI101],
            [Activo_DNI101],
            [FechaCambio],
            [UsuarioDni],
            [TipoOperacion]
        )
        SELECT 
            d.[IdPaciente_DNI101],
            d.[DniNiño_DNI101],
            d.[Nombre_DNI101],
            d.[Apellido_DNI101],
            d.[Telefono_DNI101],
            d.[Email_DNI101],
            d.[Direccion_DNI101],
            d.[Activo_DNI101],
            SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'UPDATE'
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[Pacientes_Cambios_DNI101] (
            [IdPaciente_DNI101],
            [DniNiño_DNI101],
            [Nombre_DNI101],
            [Apellido_DNI101],
            [Telefono_DNI101],
            [Email_DNI101],
            [Direccion_DNI101],
            [Activo_DNI101],
            [FechaCambio],
            [UsuarioDni],
            [TipoOperacion]
        )
        SELECT 
            d.[IdPaciente_DNI101],
            d.[DniNiño_DNI101],
            d.[Nombre_DNI101],
            d.[Apellido_DNI101],
            d.[Telefono_DNI101],
            d.[Email_DNI101],
            d.[Direccion_DNI101],
            d.[Activo_DNI101],
            SYSDATETIME(),
            ISNULL(CAST(SESSION_CONTEXT(N'UsuarioDni') AS INT), 11111111),
            'DELETE'
        FROM deleted d;
    END
END
GO


