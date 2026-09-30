using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class ConsultaNutricionalDAL_DNI101
    {
        private readonly Conexion _conexion = new();

        public ConsultaNutricionalDAL_DNI101()
        {
            SeguimientoNutricionalDatabaseInitializer.AsegurarTablas();
        }

        public int GuardarConsultaCompleta(ConsultaNutricionalBE_DNI101 consulta)
        {
            // 1. Guardar Cabecera de Consulta
            string sqlConsulta = @"
INSERT INTO [dbo].[ConsultasNutricionales_DNI101]
([IdPaciente_DNI101], [DniNutricionista_DNI101], [IdTurno_DNI101], [FechaControl_DNI101], [EdadMeses_DNI101], 
 [TipoLactancia_DNI101], [AlimentacionComplementaria_DNI101], [Alergias_DNI101], [AntecedentesFamiliares_DNI101], [Observaciones_DNI101], [DV])
VALUES
(@IdPaciente, @DniNutricionista, @IdTurno, @FechaControl, @EdadMeses, 
 @TipoLactancia, @AlimentacionComplementaria, @Alergias, @AntecedentesFamiliares, @Observaciones, @DV);
SELECT CAST(SCOPE_IDENTITY() as int);";

            DataTable dtConsulta = _conexion.ExecuteReader(sqlConsulta,
                new SqlParameter("@IdPaciente", consulta.IdPaciente_DNI101),
                new SqlParameter("@DniNutricionista", consulta.DniNutricionista_DNI101),
                new SqlParameter("@IdTurno", (object?)consulta.IdTurno_DNI101 ?? DBNull.Value),
                new SqlParameter("@FechaControl", consulta.FechaControl_DNI101),
                new SqlParameter("@EdadMeses", consulta.EdadMeses_DNI101),
                new SqlParameter("@TipoLactancia", (object?)consulta.TipoLactancia_DNI101 ?? DBNull.Value),
                new SqlParameter("@AlimentacionComplementaria", (object?)consulta.AlimentacionComplementaria_DNI101 ?? DBNull.Value),
                new SqlParameter("@Alergias", (object?)consulta.Alergias_DNI101 ?? DBNull.Value),
                new SqlParameter("@AntecedentesFamiliares", (object?)consulta.AntecedentesFamiliares_DNI101 ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object?)consulta.Observaciones_DNI101 ?? DBNull.Value),
                new SqlParameter("@DV", (object?)consulta.DV ?? DBNull.Value)
            );

            if (dtConsulta.Rows.Count > 0 && dtConsulta.Rows[0][0] != DBNull.Value)
            {
                consulta.IdConsulta_DNI101 = Convert.ToInt32(dtConsulta.Rows[0][0]);
            }

            // 2. Guardar Mediciones Antropométricas
            if (consulta.Medicion_DNI101 != null)
            {
                consulta.Medicion_DNI101.IdConsulta_DNI101 = consulta.IdConsulta_DNI101;
                string sqlMedicion = @"
INSERT INTO [dbo].[MedicionesAntropometricas_DNI101]
([IdConsulta_DNI101], [PesoKg_DNI101], [TallaCm_DNI101], [PerimetroCefalicoCm_DNI101], [CircunferenciaCinturaCm_DNI101], [IMC_DNI101], [DV])
VALUES
(@IdConsulta, @PesoKg, @TallaCm, @PerimetroCefalicoCm, @CircunferenciaCinturaCm, @IMC, @DV);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dtMed = _conexion.ExecuteReader(sqlMedicion,
                    new SqlParameter("@IdConsulta", consulta.IdConsulta_DNI101),
                    new SqlParameter("@PesoKg", consulta.Medicion_DNI101.PesoKg_DNI101),
                    new SqlParameter("@TallaCm", consulta.Medicion_DNI101.TallaCm_DNI101),
                    new SqlParameter("@PerimetroCefalicoCm", consulta.Medicion_DNI101.PerimetroCefalicoCm_DNI101),
                    new SqlParameter("@CircunferenciaCinturaCm", (object?)consulta.Medicion_DNI101.CircunferenciaCinturaCm_DNI101 ?? DBNull.Value),
                    new SqlParameter("@IMC", consulta.Medicion_DNI101.IMC_DNI101),
                    new SqlParameter("@DV", (object?)consulta.Medicion_DNI101.DV ?? DBNull.Value)
                );

                if (dtMed.Rows.Count > 0 && dtMed.Rows[0][0] != DBNull.Value)
                {
                    consulta.Medicion_DNI101.IdMedicion_DNI101 = Convert.ToInt32(dtMed.Rows[0][0]);
                }
            }

            // 3. Guardar Z-Scores y Percentiles
            if (consulta.ZScores_DNI101 != null)
            {
                consulta.ZScores_DNI101.IdConsulta_DNI101 = consulta.IdConsulta_DNI101;
                string sqlZScore = @"
INSERT INTO [dbo].[ZScoresResultados_DNI101]
([IdConsulta_DNI101], [ZPesoEdad_DNI101], [ZTallaEdad_DNI101], [ZIMCEdad_DNI101], [ZPCEdad_DNI101], [PercentilPeso_DNI101], [PercentilTalla_DNI101], [PercentilIMC_DNI101], [DV])
VALUES
(@IdConsulta, @ZPeso, @ZTalla, @ZIMC, @ZPC, @PercentilPeso, @PercentilTalla, @PercentilIMC, @DV);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dtZ = _conexion.ExecuteReader(sqlZScore,
                    new SqlParameter("@IdConsulta", consulta.IdConsulta_DNI101),
                    new SqlParameter("@ZPeso", consulta.ZScores_DNI101.ZPesoEdad_DNI101),
                    new SqlParameter("@ZTalla", consulta.ZScores_DNI101.ZTallaEdad_DNI101),
                    new SqlParameter("@ZIMC", consulta.ZScores_DNI101.ZIMCEdad_DNI101),
                    new SqlParameter("@ZPC", consulta.ZScores_DNI101.ZPCEdad_DNI101),
                    new SqlParameter("@PercentilPeso", consulta.ZScores_DNI101.PercentilPeso_DNI101),
                    new SqlParameter("@PercentilTalla", consulta.ZScores_DNI101.PercentilTalla_DNI101),
                    new SqlParameter("@PercentilIMC", consulta.ZScores_DNI101.PercentilIMC_DNI101),
                    new SqlParameter("@DV", (object?)consulta.ZScores_DNI101.DV ?? DBNull.Value)
                );

                if (dtZ.Rows.Count > 0 && dtZ.Rows[0][0] != DBNull.Value)
                {
                    consulta.ZScores_DNI101.IdZScore_DNI101 = Convert.ToInt32(dtZ.Rows[0][0]);
                }
            }

            // 4. Guardar Diagnóstico Nutricional
            if (consulta.Diagnostico_DNI101 != null)
            {
                consulta.Diagnostico_DNI101.IdConsulta_DNI101 = consulta.IdConsulta_DNI101;
                string sqlDiag = @"
INSERT INTO [dbo].[DiagnosticosNutricionales_DNI101]
([IdConsulta_DNI101], [ClasificacionOMS_DNI101], [DetallesClinicos_DNI101], [RequiereAlerta_DNI101], [DV])
VALUES
(@IdConsulta, @Clasificacion, @Detalles, @RequiereAlerta, @DV);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dtDiag = _conexion.ExecuteReader(sqlDiag,
                    new SqlParameter("@IdConsulta", consulta.IdConsulta_DNI101),
                    new SqlParameter("@Clasificacion", consulta.Diagnostico_DNI101.ClasificacionOMS_DNI101),
                    new SqlParameter("@Detalles", consulta.Diagnostico_DNI101.DetallesClinicos_DNI101),
                    new SqlParameter("@RequiereAlerta", consulta.Diagnostico_DNI101.RequiereAlerta_DNI101),
                    new SqlParameter("@DV", (object?)consulta.Diagnostico_DNI101.DV ?? DBNull.Value)
                );

                if (dtDiag.Rows.Count > 0 && dtDiag.Rows[0][0] != DBNull.Value)
                {
                    consulta.Diagnostico_DNI101.IdDiagnostico_DNI101 = Convert.ToInt32(dtDiag.Rows[0][0]);
                }
            }

            // 5. Guardar Alerta Clínica (si existe)
            if (consulta.Alerta_DNI101 != null)
            {
                consulta.Alerta_DNI101.IdConsulta_DNI101 = consulta.IdConsulta_DNI101;
                string sqlAlerta = @"
INSERT INTO [dbo].[AlertasClinicas_DNI101]
([IdConsulta_DNI101], [TipoAlerta_DNI101], [Severidad_DNI101], [MensajeAlerta_DNI101], [FechaGeneracion_DNI101], [DV])
VALUES
(@IdConsulta, @Tipo, @Severidad, @Mensaje, @Fecha, @DV);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dtAlerta = _conexion.ExecuteReader(sqlAlerta,
                    new SqlParameter("@IdConsulta", consulta.IdConsulta_DNI101),
                    new SqlParameter("@Tipo", consulta.Alerta_DNI101.TipoAlerta_DNI101),
                    new SqlParameter("@Severidad", consulta.Alerta_DNI101.Severidad_DNI101),
                    new SqlParameter("@Mensaje", consulta.Alerta_DNI101.MensajeAlerta_DNI101),
                    new SqlParameter("@Fecha", consulta.Alerta_DNI101.FechaGeneracion_DNI101),
                    new SqlParameter("@DV", (object?)consulta.Alerta_DNI101.DV ?? DBNull.Value)
                );

                if (dtAlerta.Rows.Count > 0 && dtAlerta.Rows[0][0] != DBNull.Value)
                {
                    consulta.Alerta_DNI101.IdAlerta_DNI101 = Convert.ToInt32(dtAlerta.Rows[0][0]);
                }
            }

            // 6. Guardar Plan Alimentario (si existe)
            if (consulta.PlanAlimentario_DNI101 != null)
            {
                consulta.PlanAlimentario_DNI101.IdConsulta_DNI101 = consulta.IdConsulta_DNI101;
                string sqlPlan = @"
INSERT INTO [dbo].[PlanesAlimentarios_DNI101]
([IdConsulta_DNI101], [RequerimientoCalorico_DNI101], [PctCarbohidratos_DNI101], [PctProteinas_DNI101], [PctGrasas_DNI101], [PautasFamiliares_DNI101], [MetasSalud_DNI101], [DV])
VALUES
(@IdConsulta, @Kcal, @PctCHO, @PctPROT, @PctGrasas, @Pautas, @Metas, @DV);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dtPlan = _conexion.ExecuteReader(sqlPlan,
                    new SqlParameter("@IdConsulta", consulta.IdConsulta_DNI101),
                    new SqlParameter("@Kcal", consulta.PlanAlimentario_DNI101.RequerimientoCalorico_DNI101),
                    new SqlParameter("@PctCHO", consulta.PlanAlimentario_DNI101.PctCarbohidratos_DNI101),
                    new SqlParameter("@PctPROT", consulta.PlanAlimentario_DNI101.PctProteinas_DNI101),
                    new SqlParameter("@PctGrasas", consulta.PlanAlimentario_DNI101.PctGrasas_DNI101),
                    new SqlParameter("@Pautas", consulta.PlanAlimentario_DNI101.PautasFamiliares_DNI101),
                    new SqlParameter("@Metas", consulta.PlanAlimentario_DNI101.MetasSalud_DNI101),
                    new SqlParameter("@DV", (object?)consulta.PlanAlimentario_DNI101.DV ?? DBNull.Value)
                );

                if (dtPlan.Rows.Count > 0 && dtPlan.Rows[0][0] != DBNull.Value)
                {
                    consulta.PlanAlimentario_DNI101.IdPlan_DNI101 = Convert.ToInt32(dtPlan.Rows[0][0]);
                }
            }

            // 7. Guardar Recordatorio 24h (si existe)
            if (consulta.Recordatorio_DNI101 != null)
            {
                consulta.Recordatorio_DNI101.IdConsulta_DNI101 = consulta.IdConsulta_DNI101;
                string sqlRec = @"
INSERT INTO [dbo].[Recordatorios24h_DNI101]
([IdConsulta_DNI101], [Desayuno_DNI101], [Almuerzo_DNI101], [Merienda_DNI101], [Cena_DNI101], [Colaciones_DNI101], [FrecuenciaAlimentos_DNI101], [DV])
VALUES
(@IdConsulta, @Desayuno, @Almuerzo, @Merienda, @Cena, @Colaciones, @Frecuencia, @DV);
SELECT CAST(SCOPE_IDENTITY() as int);";

                DataTable dtRec = _conexion.ExecuteReader(sqlRec,
                    new SqlParameter("@IdConsulta", consulta.IdConsulta_DNI101),
                    new SqlParameter("@Desayuno", consulta.Recordatorio_DNI101.Desayuno_DNI101),
                    new SqlParameter("@Almuerzo", consulta.Recordatorio_DNI101.Almuerzo_DNI101),
                    new SqlParameter("@Merienda", consulta.Recordatorio_DNI101.Merienda_DNI101),
                    new SqlParameter("@Cena", consulta.Recordatorio_DNI101.Cena_DNI101),
                    new SqlParameter("@Colaciones", (object?)consulta.Recordatorio_DNI101.Colaciones_DNI101 ?? DBNull.Value),
                    new SqlParameter("@Frecuencia", (object?)consulta.Recordatorio_DNI101.FrecuenciaAlimentos_DNI101 ?? DBNull.Value),
                    new SqlParameter("@DV", (object?)consulta.Recordatorio_DNI101.DV ?? DBNull.Value)
                );

                if (dtRec.Rows.Count > 0 && dtRec.Rows[0][0] != DBNull.Value)
                {
                    consulta.Recordatorio_DNI101.IdRecordatorio_DNI101 = Convert.ToInt32(dtRec.Rows[0][0]);
                }
            }

            return consulta.IdConsulta_DNI101;
        }

        public List<ConsultaNutricionalBE_DNI101> ListarHistorialPorPaciente(int idPaciente)
        {
            List<ConsultaNutricionalBE_DNI101> lista = new();
            string sql = @"
SELECT c.*, 
       m.PesoKg_DNI101, m.TallaCm_DNI101, m.PerimetroCefalicoCm_DNI101, m.IMC_DNI101,
       z.ZPesoEdad_DNI101, z.ZTallaEdad_DNI101, z.ZIMCEdad_DNI101, z.PercentilIMC_DNI101,
       d.ClasificacionOMS_DNI101, d.DetallesClinicos_DNI101
FROM [dbo].[ConsultasNutricionales_DNI101] c
LEFT JOIN [dbo].[MedicionesAntropometricas_DNI101] m ON c.IdConsulta_DNI101 = m.IdConsulta_DNI101
LEFT JOIN [dbo].[ZScoresResultados_DNI101] z ON c.IdConsulta_DNI101 = z.IdConsulta_DNI101
LEFT JOIN [dbo].[DiagnosticosNutricionales_DNI101] d ON c.IdConsulta_DNI101 = d.IdConsulta_DNI101
WHERE c.IdPaciente_DNI101 = @IdPaciente
ORDER BY c.FechaControl_DNI101 DESC;";

            DataTable dt = _conexion.ExecuteReader(sql, new SqlParameter("@IdPaciente", idPaciente));

            foreach (DataRow row in dt.Rows)
            {
                var consulta = new ConsultaNutricionalBE_DNI101
                {
                    IdConsulta_DNI101 = Convert.ToInt32(row["IdConsulta_DNI101"]),
                    IdPaciente_DNI101 = Convert.ToInt32(row["IdPaciente_DNI101"]),
                    DniNutricionista_DNI101 = Convert.ToInt32(row["DniNutricionista_DNI101"]),
                    FechaControl_DNI101 = Convert.ToDateTime(row["FechaControl_DNI101"]),
                    EdadMeses_DNI101 = Convert.ToInt32(row["EdadMeses_DNI101"]),
                    TipoLactancia_DNI101 = row["TipoLactancia_DNI101"]?.ToString(),
                    Observaciones_DNI101 = row["Observaciones_DNI101"]?.ToString()
                };

                if (row["PesoKg_DNI101"] != DBNull.Value)
                {
                    consulta.Medicion_DNI101 = new MedicionAntropometricaBE_DNI101
                    {
                        IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                        PesoKg_DNI101 = Convert.ToDouble(row["PesoKg_DNI101"]),
                        TallaCm_DNI101 = Convert.ToDouble(row["TallaCm_DNI101"]),
                        PerimetroCefalicoCm_DNI101 = Convert.ToDouble(row["PerimetroCefalicoCm_DNI101"]),
                        IMC_DNI101 = Convert.ToDouble(row["IMC_DNI101"])
                    };
                }

                if (row["ZIMCEdad_DNI101"] != DBNull.Value)
                {
                    consulta.ZScores_DNI101 = new ZScoreResultadoBE_DNI101
                    {
                        IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                        ZPesoEdad_DNI101 = Convert.ToDouble(row["ZPesoEdad_DNI101"]),
                        ZTallaEdad_DNI101 = Convert.ToDouble(row["ZTallaEdad_DNI101"]),
                        ZIMCEdad_DNI101 = Convert.ToDouble(row["ZIMCEdad_DNI101"]),
                        PercentilIMC_DNI101 = Convert.ToDouble(row["PercentilIMC_DNI101"])
                    };
                }

                if (row["ClasificacionOMS_DNI101"] != DBNull.Value)
                {
                    consulta.Diagnostico_DNI101 = new DiagnosticoNutricionalBE_DNI101
                    {
                        IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                        ClasificacionOMS_DNI101 = row["ClasificacionOMS_DNI101"].ToString() ?? "",
                        DetallesClinicos_DNI101 = row["DetallesClinicos_DNI101"].ToString() ?? ""
                    };
                }

                lista.Add(consulta);
            }

            return lista;
        }

        public ConsultaNutricionalBE_DNI101? ObtenerUltimaConsulta(int idPaciente)
        {
            var historial = ListarHistorialPorPaciente(idPaciente);
            return historial.Count > 0 ? historial[0] : null;
        }
    }
}
