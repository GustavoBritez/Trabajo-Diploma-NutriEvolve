USE [ING]
GO

SET NOCOUNT ON;

-- ============================================================================
-- SCRIPT DE DATOS DE PRUEBA: 4 PACIENTES PEDIÁTRICOS CON HISTORIAL LARGO (PN2)
-- DNI FÁCILES DE PROBAR:
--  1111: Mateo Benítez (Lactante 0-24m, Crecimiento Normal / Eutrófico - 7 Controles)
--  2222: Sofía Rossi (Preescolar 2-5a, Curva de Sobrepeso / Obesidad - 6 Controles)
--  3333: Thiago Gómez (Lactante 0-24m, ⚠️ ALERTA CLÍNICA CRÍTICA: Talla Baja Severa - 6 Controles)
--  4444: Emma Martínez (Preescolar 2-5a, Percentil 50 Estable - 5 Controles)
-- ============================================================================

PRINT 'Insertando o actualizando Pacientes de prueba...';

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

-- Obtener IDs de Pacientes
DECLARE @IdP1 INT, @IdP2 INT, @IdP3 INT, @IdP4 INT;
SELECT @IdP1 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '1111';
SELECT @IdP2 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '2222';
SELECT @IdP3 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '3333';
SELECT @IdP4 = IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE DniNiño_DNI101 = '4444';

-- Limpiar consultas previas de prueba para re-insertar limpiamente si se ejecuta de nuevo
DELETE FROM Recordatorios24h_DNI101 WHERE IdConsulta_DNI101 IN (SELECT IdConsulta_DNI101 FROM ConsultasNutricionales_DNI101 WHERE IdPaciente_DNI101 IN (@IdP1, @IdP2, @IdP3, @IdP4));
DELETE FROM PlanesAlimentarios_DNI101 WHERE IdConsulta_DNI101 IN (SELECT IdConsulta_DNI101 FROM ConsultasNutricionales_DNI101 WHERE IdPaciente_DNI101 IN (@IdP1, @IdP2, @IdP3, @IdP4));
DELETE FROM AlertasClinicas_DNI101 WHERE IdConsulta_DNI101 IN (SELECT IdConsulta_DNI101 FROM ConsultasNutricionales_DNI101 WHERE IdPaciente_DNI101 IN (@IdP1, @IdP2, @IdP3, @IdP4));
DELETE FROM DiagnosticosNutricionales_DNI101 WHERE IdConsulta_DNI101 IN (SELECT IdConsulta_DNI101 FROM ConsultasNutricionales_DNI101 WHERE IdPaciente_DNI101 IN (@IdP1, @IdP2, @IdP3, @IdP4));
DELETE FROM ZScoresResultados_DNI101 WHERE IdConsulta_DNI101 IN (SELECT IdConsulta_DNI101 FROM ConsultasNutricionales_DNI101 WHERE IdPaciente_DNI101 IN (@IdP1, @IdP2, @IdP3, @IdP4));
DELETE FROM MedicionesAntropometricas_DNI101 WHERE IdConsulta_DNI101 IN (SELECT IdConsulta_DNI101 FROM ConsultasNutricionales_DNI101 WHERE IdPaciente_DNI101 IN (@IdP1, @IdP2, @IdP3, @IdP4));
DELETE FROM ConsultasNutricionales_DNI101 WHERE IdPaciente_DNI101 IN (@IdP1, @IdP2, @IdP3, @IdP4);

--------------------------------------------------------------------------------
-- HELPER STORED PROCEDURE O BLOQUE TEMPORAL PARA INSERTAR CONSULTA COMPLETA
--------------------------------------------------------------------------------
DECLARE @IdConsulta INT;

-- ============================================================================
-- HISTORIAL PACIENTE 1 (DNI 1111 - Mateo Benítez): 7 CONTROLES NORMALES OMS
-- ============================================================================

-- Control 1 (1 mes)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101)
VALUES (@IdP1, 11111111, '2024-02-15', 1, N'Materna Exclusiva', N'No', N'Primer control del lactante. Reflejos conservados.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 4.50, 54.50, 37.50, 0, 15.13);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.10, 0.20, 0.00, 0.15, 53.98, 57.93, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Lactante con desarrollo normopeso acorde a su edad.', 0);

-- Control 2 (3 meses)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101)
VALUES (@IdP1, 11111111, '2024-04-15', 3, N'Materna Exclusiva', N'No', N'Buen agarre y succión.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 6.40, 61.20, 40.50, 0, 17.09);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.20, 0.10, 0.20, 0.10, 57.93, 53.98, 57.93);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Crecimiento constante siguiendo la curva P50.', 0);

-- Control 3 (6 meses)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101)
VALUES (@IdP1, 11111111, '2024-07-15', 6, N'Predominante', N'Inicio de papillas y frutas', N'Comienza alimentación complementaria con excelente tolerancia.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 7.90, 67.60, 43.20, 0, 17.29);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.10, 0.00, 0.10, 0.05, 53.98, 50.00, 53.98);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Transición adecuada a sólidos.', 0);

-- Control 4 (9 meses)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101)
VALUES (@IdP1, 11111111, '2024-10-15', 9, N'Parcial / Mixta', N'Purés, carnes magras, vegetales', N'Crecimiento longitudinal muy armónico.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 8.90, 72.00, 45.00, 0, 17.17);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.00, -0.10, 0.10, 0.00, 50.00, 46.02, 53.98);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Estado nutricional óptimo.', 0);

-- Control 5 (12 meses)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101)
VALUES (@IdP1, 11111111, '2025-01-15', 12, N'Complementaria', N'Incorporado a la mesa familiar', N'Cumple 1 año. Marcha independiente iniciada.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 9.60, 75.70, 46.20, 0, 16.75);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -0.10, -0.10, -0.10, -0.05, 46.02, 46.02, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Desarrollo psicomotor y nutricional excelente.', 0);

-- Control 6 (15 meses)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101)
VALUES (@IdP1, 11111111, '2025-04-15', 15, N'Destetado', N'Dieta completa y variada', N'Controles pediátricos de rutina.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 10.30, 79.10, 47.10, 0, 16.46);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.00, 0.00, -0.10, 0.00, 50.00, 50.00, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Saludable.', 0);

-- Control 7 (20 meses)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Observaciones_DNI101)
VALUES (@IdP1, 11111111, '2025-09-15', 20, N'Destetado', N'Alimentación equilibrada', N'Último control de seguimiento normopeso.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 11.40, 84.50, 48.00, 0, 15.97);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.10, 0.10, 0.00, 0.05, 53.98, 53.98, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Estado nutricional excelente. Continuar pautas habituales.', 0);


-- ============================================================================
-- HISTORIAL PACIENTE 2 (DNI 2222 - Sofía Rossi): 6 CONTROLES CURVA SOBREPESO/OBESIDAD
-- ============================================================================

-- Control 1 (24m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, TipoLactancia_DNI101, Observaciones_DNI101)
VALUES (@IdP2, 11111111, '2023-05-10', 24, N'Destetado', N'Consulta por hábito alimentario con alto consumo de ultraprocesados.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 13.50, 86.00, 48.20, 50.00, 18.25);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 1.20, 0.30, 1.40, 0.40, 88.49, 61.79, 91.92);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Riesgo de Sobrepeso', N'Inicia leve ascenso percentilar de IMC.', 0);

-- Control 2 (30m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP2, 11111111, '2023-11-10', 30, N'Consumo de jugos azucarados frecuente.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 15.20, 91.50, 49.00, 52.50, 18.16);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 1.50, 0.40, 1.60, 0.50, 93.32, 65.54, 94.52);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Sobrepeso', N'Z-IMC mayor a +1.5. Se refuerzan pautas familiares.', 0);

-- Control 3 (36m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP2, 11111111, '2024-05-10', 36, N'Sedentarismo incrementado.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 17.00, 96.00, 49.60, 55.00, 18.44);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 1.70, 0.50, 1.80, 0.50, 95.54, 69.15, 96.41);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Sobrepeso', N'Mantiene curva de sobrepeso por encima de P95.', 0);

-- Control 4 (42m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP2, 11111111, '2024-11-10', 42, N'Aumento ponderal acelerado.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 19.10, 100.50, 50.10, 58.00, 18.91);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 2.00, 0.60, 2.10, 0.50, 97.72, 72.57, 98.21);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Obesidad Moderada', N'Z-IMC cruza la línea +2.0 SD (Obesidad OMS).', 0);

-- Control 5 (48m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP2, 11111111, '2025-05-10', 48, N'Se prescribe plan de alimentación estructurada.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 21.50, 104.50, 50.50, 61.00, 19.69);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 2.30, 0.70, 2.40, 0.60, 98.93, 75.80, 99.18);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Obesidad Severa', N'Tendencia sostenida en percentil > 99.', 0);

-- Control 6 (52m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP2, 11111111, '2025-09-10', 52, N'Último control. En tratamiento nutricional multifactorial.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 23.20, 107.00, 50.80, 63.00, 20.26);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 2.50, 0.70, 2.60, 0.60, 99.38, 75.80, 99.53);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Obesidad Severa Pediátrica', N'Seguimiento intensivo de pautas de actividad física y nutrición.', 0);


-- ============================================================================
-- HISTORIAL PACIENTE 3 (DNI 3333 - Thiago Gómez): ⚠️ ALERTA CLÍNICA ROJA
-- Desaceleración grave del crecimiento (Talla Baja Severa Z < -3.00)
-- ============================================================================

-- Control 1 (3m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP3, 11111111, '2023-09-20', 3, N'Lactante con leve hiporexia.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 5.80, 60.00, 39.50, 0, 16.11);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -0.60, -0.50, -0.40, -0.30, 27.43, 30.85, 34.46);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Monitoreo por ingesta limítrofe.', 0);

-- Control 2 (6m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP3, 11111111, '2023-12-20', 6, N'Dificultad en la incorporación de papillas.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 6.80, 63.50, 41.00, 0, 16.86);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -1.10, -1.20, -0.60, -0.50, 13.57, 11.51, 27.43);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Riesgo de Desnutrición', N'Pérdida de carril percentilar de talla y peso.', 0);

-- Control 3 (9m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP3, 11111111, '2024-03-20', 9, N'Astenia y palidez cutánea.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 7.20, 66.00, 42.10, 0, 16.53);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -1.80, -2.10, -0.90, -0.80, 3.59, 1.79, 18.41);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Desnutrición Moderada / Talla Baja', N'Caída de Z-Talla por debajo de -2.00 SD.', 0);

-- Control 4 (12m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP3, 11111111, '2024-06-20', 12, N'Escasa ganancia estatural en 6 meses.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 7.50, 68.00, 42.80, 0, 16.22);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -2.30, -2.70, -1.20, -1.00, 1.07, 0.35, 11.51);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Desnutrición Severa', N'Aplanamiento severo de la curva de longitud.', 0);

-- Control 5 (15m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP3, 11111111, '2024-09-20', 15, N'Derivado a Gastroenterología Pediátrica.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 7.80, 69.50, 43.30, 0, 16.15);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -2.60, -3.00, -1.40, -1.20, 0.47, 0.13, 8.08);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Talla Baja Severa', N'Z-Score Talla/Edad cruza el límite de -3.00 SD.', 1);
INSERT INTO AlertasClinicas_DNI101 (IdConsulta_DNI101, TipoAlerta_DNI101, Severidad_DNI101, MensajeAlerta_DNI101, FechaGeneracion_DNI101)
VALUES (@IdConsulta, N'Desnutrición / Talla Baja Severa', N'CRÍTICA', N'🚨 ALERTA ROJA: Z-Score Talla/Edad de -3.00 (Talla Baja Severa). Requiere intervención urgente.', '2024-09-20');

-- Control 6 (18m) -- CONTROL ACTUAL CON ALERTA CRÍTICA
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP3, 11111111, '2024-12-20', 18, N'Control de seguimiento en curso con soporte nutricional enteral.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 8.00, 71.00, 43.80, 0, 15.87);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -2.80, -3.20, -1.60, -1.30, 0.26, 0.07, 5.48);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Talla Baja Severa / Desnutrición Crónica', N'Desnutrición crónica reagudizada. Z-Talla de -3.20 SD. El paciente presenta un retardo severo del crecimiento.', 1);
INSERT INTO AlertasClinicas_DNI101 (IdConsulta_DNI101, TipoAlerta_DNI101, Severidad_DNI101, MensajeAlerta_DNI101, FechaGeneracion_DNI101)
VALUES (@IdConsulta, N'Desnutrición / Talla Baja Severa', N'CRÍTICA', N'🚨 ALERTA ROJA: Z-Score Talla/Edad de -3.20 (Talla Baja Severa / Desnutrición Crónica). Se requiere intervención nutricional urgente e interconsulta médica.', '2024-12-20');


-- ============================================================================
-- HISTORIAL PACIENTE 4 (DNI 4444 - Emma Martínez): 5 CONTROLES PERCENTIL 50 ESTABLE
-- ============================================================================

-- Control 1 (12m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP4, 11111111, '2023-03-01', 12, N'Control al año de vida.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 8.90, 74.00, 45.10, 0, 16.25);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -0.10, -0.10, -0.10, -0.05, 46.02, 46.02, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Desarrollo normal.', 0);

-- Control 2 (18m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP4, 11111111, '2023-09-01', 18, N'Control de rutina.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 10.20, 80.50, 46.40, 0, 15.74);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.00, 0.00, 0.00, 0.00, 50.00, 50.00, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Saludable.', 0);

-- Control 3 (24m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP4, 11111111, '2024-03-01', 24, N'Cumple 2 años. Curva impecable.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 11.50, 85.50, 47.50, 48.00, 15.73);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.10, 0.00, 0.10, 0.05, 53.98, 50.00, 53.98);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Excelente estado general.', 0);

-- Control 4 (30m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP4, 11111111, '2024-09-01', 30, N'Seguimiento semestral.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 12.70, 90.00, 48.30, 49.50, 15.68);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, 0.00, 0.00, 0.00, 0.00, 50.00, 50.00, 50.00);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Alimentación variada.', 0);

-- Control 5 (36m)
INSERT INTO ConsultasNutricionales_DNI101 (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101, Observaciones_DNI101)
VALUES (@IdP4, 11111111, '2025-03-01', 36, N'Control a los 3 años de vida.');
SET @IdConsulta = SCOPE_IDENTITY();
INSERT INTO MedicionesAntropometricas_DNI101 (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101)
VALUES (@IdConsulta, 13.90, 95.00, 49.00, 51.00, 15.40);
INSERT INTO ZScoresResultados_DNI101 (IdConsulta_DNI101, ZPesoEdad_DNI101, ZTallaEdad_DNI101, ZIMCEdad_DNI101, ZPCEdad_DNI101, PercentilPeso_DNI101, PercentilTalla_DNI101, PercentilIMC_DNI101)
VALUES (@IdConsulta, -0.10, 0.00, -0.10, 0.00, 46.02, 50.00, 46.02);
INSERT INTO DiagnosticosNutricionales_DNI101 (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101)
VALUES (@IdConsulta, N'Eutrófico (Normal)', N'Niña normopeso con desarrollo antropométrico óptimo.', 0);

GO

PRINT '¡Datos de prueba insertados con éxito!';
