using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.PN2
{
    public class ConsultaNutricionalDAL_DNI101
    {
        private readonly Conexion _conexion;

        public ConsultaNutricionalDAL_DNI101()
        {
            _conexion = new Conexion();
        }

        /// <summary>
        /// Registra la consulta médica, medición antropométrica, diagnóstico y alerta clínica en una única transacción atómica.
        /// Un solo acceso a base de datos para todo el CUN08. Totalmente desacoplada de entidades BE.
        /// </summary>
        public int GuardarConsultaCompleta(
            int idPaciente,
            int dniNutricionista,
            DateTime fechaControl,
            int edadMeses,
            string? tipoLactancia,
            string? alimentacionComp,
            string? alergias,
            string? antFamiliares,
            string? observaciones,
            decimal pesoKg,
            decimal tallaCm,
            decimal perimetroCefalico,
            decimal imc,
            string clasificacionOMS,
            string detallesClinicos,
            bool requiereAlerta,
            string? tipoAlerta,
            string? severidadAlerta,
            string? mensajeAlerta,
            string? dv)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                // 1. Insertar Consulta
                string sqlConsulta = @"
                    INSERT INTO ConsultasNutricionales_DNI101
                    (IdPaciente_DNI101, DniNutricionista_DNI101, FechaControl_DNI101, EdadMeses_DNI101,
                     TipoLactancia_DNI101, AlimentacionComplementaria_DNI101, Alergias_DNI101,
                     AntecedentesFamiliares_DNI101, Observaciones_DNI101, DV)
                    VALUES
                    (@IdPaciente, @DniNutri, @Fecha, @EdadMeses,
                     @TipoLact, @AlimComp, @Alergias,
                     @AntFam, @Obs, @DV);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                int idConsulta;
                using (SqlCommand cmdConsulta = new SqlCommand(sqlConsulta, tx.Connection, tx))
                {
                    cmdConsulta.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    cmdConsulta.Parameters.AddWithValue("@DniNutri", dniNutricionista);
                    cmdConsulta.Parameters.AddWithValue("@Fecha", fechaControl);
                    cmdConsulta.Parameters.AddWithValue("@EdadMeses", edadMeses);
                    cmdConsulta.Parameters.AddWithValue("@TipoLact", (object?)tipoLactancia ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@AlimComp", (object?)alimentacionComp ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@Alergias", (object?)alergias ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@AntFam", (object?)antFamiliares ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@Obs", (object?)observaciones ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);

                    idConsulta = (int)cmdConsulta.ExecuteScalar()!;
                }

                // 2. Insertar Medicion
                string sqlMedicion = @"
                    INSERT INTO MedicionesAntropometricas_DNI101
                    (IdConsulta_DNI101, PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, CircunferenciaCinturaCm_DNI101, IMC_DNI101, DV)
                    VALUES
                    (@IdConsulta, @Peso, @Talla, @Perimetro, 0, @IMC, @DV);";

                using (SqlCommand cmdMedicion = new SqlCommand(sqlMedicion, tx.Connection, tx))
                {
                    cmdMedicion.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdMedicion.Parameters.AddWithValue("@Peso", pesoKg);
                    cmdMedicion.Parameters.AddWithValue("@Talla", tallaCm);
                    cmdMedicion.Parameters.AddWithValue("@Perimetro", perimetroCefalico);
                    cmdMedicion.Parameters.AddWithValue("@IMC", imc);
                    cmdMedicion.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);
                    cmdMedicion.ExecuteNonQuery();
                }

                // 3. Insertar Diagnostico
                string sqlDiag = @"
                    INSERT INTO DiagnosticosNutricionales_DNI101
                    (IdConsulta_DNI101, ClasificacionOMS_DNI101, DetallesClinicos_DNI101, RequiereAlerta_DNI101, DV)
                    VALUES
                    (@IdConsulta, @Clasif, @Detalles, @Alerta, @DV);";

                using (SqlCommand cmdDiag = new SqlCommand(sqlDiag, tx.Connection, tx))
                {
                    cmdDiag.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdDiag.Parameters.AddWithValue("@Clasif", clasificacionOMS);
                    cmdDiag.Parameters.AddWithValue("@Detalles", detallesClinicos);
                    cmdDiag.Parameters.AddWithValue("@Alerta", requiereAlerta ? 1 : 0);
                    cmdDiag.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);
                    cmdDiag.ExecuteNonQuery();
                }

                // 4. Si requiere alerta, insertar AlertaClinica
                if (requiereAlerta && !string.IsNullOrWhiteSpace(mensajeAlerta))
                {
                    string sqlAlerta = @"
                        INSERT INTO AlertasClinicas_DNI101
                        (IdConsulta_DNI101, TipoAlerta_DNI101, Severidad_DNI101, MensajeAlerta_DNI101, FechaGeneracion_DNI101, DV)
                        VALUES
                        (@IdConsulta, @Tipo, @Sev, @Msg, @FechaGen, @DV);";

                    using (SqlCommand cmdAlerta = new SqlCommand(sqlAlerta, tx.Connection, tx))
                    {
                        cmdAlerta.Parameters.AddWithValue("@IdConsulta", idConsulta);
                        cmdAlerta.Parameters.AddWithValue("@Tipo", tipoAlerta ?? "Alerta Nutricional");
                        cmdAlerta.Parameters.AddWithValue("@Sev", severidadAlerta ?? "MODERADA");
                        cmdAlerta.Parameters.AddWithValue("@Msg", mensajeAlerta);
                        cmdAlerta.Parameters.AddWithValue("@FechaGen", DateTime.Now);
                        cmdAlerta.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);
                        cmdAlerta.ExecuteNonQuery();
                    }
                }

                // 5. Asegurar datos antropométricos y diagnóstico en el registro histórico de la bitácora
                string sqlSnap = @"
                    UPDATE ConsultasNutricionales_Cambios_DNI101
                    SET PesoKg_DNI101 = @Peso,
                        TallaCm_DNI101 = @Talla,
                        PerimetroCefalicoCm_DNI101 = @Perimetro,
                        IMC_DNI101 = @IMC,
                        ClasificacionOMS_DNI101 = @Clasif
                    WHERE IdConsulta_DNI101 = @IdConsulta AND Act = 1;";

                using (SqlCommand cmdSnap = new SqlCommand(sqlSnap, tx.Connection, tx))
                {
                    cmdSnap.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdSnap.Parameters.AddWithValue("@Peso", pesoKg);
                    cmdSnap.Parameters.AddWithValue("@Talla", tallaCm);
                    cmdSnap.Parameters.AddWithValue("@Perimetro", perimetroCefalico);
                    cmdSnap.Parameters.AddWithValue("@IMC", imc);
                    cmdSnap.Parameters.AddWithValue("@Clasif", clasificacionOMS);
                    cmdSnap.ExecuteNonQuery();
                }

                return idConsulta;
            });
        }

        public DataTable ObtenerHistorialPorPaciente(int idPaciente)
        {
            string sql = @"
                SELECT 
                    c.IdConsulta_DNI101,
                    c.IdPaciente_DNI101,
                    c.DniNutricionista_DNI101,
                    c.FechaControl_DNI101,
                    c.EdadMeses_DNI101,
                    c.TipoLactancia_DNI101,
                    c.AlimentacionComplementaria_DNI101,
                    c.Alergias_DNI101,
                    c.AntecedentesFamiliares_DNI101,
                    c.Observaciones_DNI101,
                    m.IdMedicion_DNI101,
                    m.PesoKg_DNI101,
                    m.TallaCm_DNI101,
                    m.PerimetroCefalicoCm_DNI101,
                    m.IMC_DNI101,
                    d.IdDiagnostico_DNI101,
                    d.ClasificacionOMS_DNI101,
                    d.DetallesClinicos_DNI101,
                    d.RequiereAlerta_DNI101,
                    a.IdAlerta_DNI101,
                    a.TipoAlerta_DNI101,
                    a.Severidad_DNI101,
                    a.MensajeAlerta_DNI101
                FROM ConsultasNutricionales_DNI101 c
                LEFT JOIN MedicionesAntropometricas_DNI101 m ON c.IdConsulta_DNI101 = m.IdConsulta_DNI101
                LEFT JOIN DiagnosticosNutricionales_DNI101 d ON c.IdConsulta_DNI101 = d.IdConsulta_DNI101
                LEFT JOIN AlertasClinicas_DNI101 a ON c.IdConsulta_DNI101 = a.IdConsulta_DNI101
                WHERE c.IdPaciente_DNI101 = @IdPaciente
                ORDER BY c.FechaControl_DNI101 ASC;";

            DataTable dt = new DataTable();
            using (SqlConnection cn = _conexion.CrearConexion())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable ObtenerUltimaConsultaPorPaciente(int idPaciente)
        {
            string sql = @"
                SELECT TOP 1
                    c.IdConsulta_DNI101,
                    c.IdPaciente_DNI101,
                    c.DniNutricionista_DNI101,
                    c.FechaControl_DNI101,
                    c.EdadMeses_DNI101,
                    c.Observaciones_DNI101,
                    m.PesoKg_DNI101,
                    m.TallaCm_DNI101,
                    m.PerimetroCefalicoCm_DNI101,
                    m.IMC_DNI101,
                    d.ClasificacionOMS_DNI101,
                    d.DetallesClinicos_DNI101,
                    d.RequiereAlerta_DNI101
                FROM ConsultasNutricionales_DNI101 c
                LEFT JOIN MedicionesAntropometricas_DNI101 m ON c.IdConsulta_DNI101 = m.IdConsulta_DNI101
                LEFT JOIN DiagnosticosNutricionales_DNI101 d ON c.IdConsulta_DNI101 = d.IdConsulta_DNI101
                WHERE c.IdPaciente_DNI101 = @IdPaciente
                ORDER BY c.FechaControl_DNI101 DESC;";

            DataTable dt = new DataTable();
            using (SqlConnection cn = _conexion.CrearConexion())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public bool ModificarConsultaCompleta(
            int idConsulta,
            int dniNutricionista,
            DateTime fechaControl,
            int edadMeses,
            string? tipoLactancia,
            string? alimentacionComp,
            string? alergias,
            string? antFamiliares,
            string? observaciones,
            decimal pesoKg,
            decimal tallaCm,
            decimal perimetroCefalico,
            decimal imc,
            string clasificacionOMS,
            string detallesClinicos,
            bool requiereAlerta,
            string? tipoAlerta,
            string? severidadAlerta,
            string? mensajeAlerta,
            string? dv)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                // 1. Establecer Session Context con el DNI del usuario activo y marca de modificación clínica para auditoría
                using (var cmdCtx = new SqlCommand(@"
                    EXEC sp_set_session_context @key=N'UsuarioDni', @value=@val;
                    EXEC sp_set_session_context @key=N'EsModificacionClinica', @value=1;", tx.Connection, tx))
                {
                    cmdCtx.Parameters.AddWithValue("@val", dniNutricionista);
                    cmdCtx.ExecuteNonQuery();
                }

                // 2. Actualizar ConsultasNutricionales_DNI101 (dispara trigger de bitácora médica)
                string sqlConsulta = @"
                    UPDATE ConsultasNutricionales_DNI101
                    SET DniNutricionista_DNI101 = @DniNutri,
                        FechaControl_DNI101 = @Fecha,
                        EdadMeses_DNI101 = @EdadMeses,
                        TipoLactancia_DNI101 = @TipoLact,
                        AlimentacionComplementaria_DNI101 = @AlimComp,
                        Alergias_DNI101 = @Alergias,
                        AntecedentesFamiliares_DNI101 = @AntFam,
                        Observaciones_DNI101 = @Obs,
                        DV = @DV
                    WHERE IdConsulta_DNI101 = @IdConsulta;";

                using (var cmdConsulta = new SqlCommand(sqlConsulta, tx.Connection, tx))
                {
                    cmdConsulta.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdConsulta.Parameters.AddWithValue("@DniNutri", dniNutricionista);
                    cmdConsulta.Parameters.AddWithValue("@Fecha", fechaControl);
                    cmdConsulta.Parameters.AddWithValue("@EdadMeses", edadMeses);
                    cmdConsulta.Parameters.AddWithValue("@TipoLact", (object?)tipoLactancia ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@AlimComp", (object?)alimentacionComp ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@Alergias", (object?)alergias ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@AntFam", (object?)antFamiliares ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@Obs", (object?)observaciones ?? DBNull.Value);
                    cmdConsulta.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);
                    cmdConsulta.ExecuteNonQuery();
                }

                // 3. Actualizar MedicionesAntropometricas_DNI101
                string sqlMedicion = @"
                    UPDATE MedicionesAntropometricas_DNI101
                    SET PesoKg_DNI101 = @Peso,
                        TallaCm_DNI101 = @Talla,
                        PerimetroCefalicoCm_DNI101 = @Perimetro,
                        IMC_DNI101 = @IMC,
                        DV = @DV
                    WHERE IdConsulta_DNI101 = @IdConsulta;";

                using (var cmdMedicion = new SqlCommand(sqlMedicion, tx.Connection, tx))
                {
                    cmdMedicion.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdMedicion.Parameters.AddWithValue("@Peso", pesoKg);
                    cmdMedicion.Parameters.AddWithValue("@Talla", tallaCm);
                    cmdMedicion.Parameters.AddWithValue("@Perimetro", perimetroCefalico);
                    cmdMedicion.Parameters.AddWithValue("@IMC", imc);
                    cmdMedicion.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);
                    cmdMedicion.ExecuteNonQuery();
                }

                // 4. Actualizar DiagnosticosNutricionales_DNI101
                string sqlDiag = @"
                    UPDATE DiagnosticosNutricionales_DNI101
                    SET ClasificacionOMS_DNI101 = @Clasif,
                        DetallesClinicos_DNI101 = @Detalles,
                        RequiereAlerta_DNI101 = @Alerta,
                        DV = @DV
                    WHERE IdConsulta_DNI101 = @IdConsulta;";

                using (var cmdDiag = new SqlCommand(sqlDiag, tx.Connection, tx))
                {
                    cmdDiag.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdDiag.Parameters.AddWithValue("@Clasif", clasificacionOMS);
                    cmdDiag.Parameters.AddWithValue("@Detalles", detallesClinicos);
                    cmdDiag.Parameters.AddWithValue("@Alerta", requiereAlerta ? 1 : 0);
                    cmdDiag.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);
                    cmdDiag.ExecuteNonQuery();
                }

                // 5. Alertas Clínicas
                if (requiereAlerta && !string.IsNullOrWhiteSpace(mensajeAlerta))
                {
                    string sqlAlerta = @"
                        IF EXISTS (SELECT 1 FROM AlertasClinicas_DNI101 WHERE IdConsulta_DNI101 = @IdConsulta)
                        BEGIN
                            UPDATE AlertasClinicas_DNI101
                            SET TipoAlerta_DNI101 = @Tipo,
                                Severidad_DNI101 = @Sev,
                                MensajeAlerta_DNI101 = @Msg,
                                DV = @DV
                            WHERE IdConsulta_DNI101 = @IdConsulta;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO AlertasClinicas_DNI101
                            (IdConsulta_DNI101, TipoAlerta_DNI101, Severidad_DNI101, MensajeAlerta_DNI101, FechaGeneracion_DNI101, DV)
                            VALUES
                            (@IdConsulta, @Tipo, @Sev, @Msg, GETDATE(), @DV);
                        END";

                    using (var cmdAlerta = new SqlCommand(sqlAlerta, tx.Connection, tx))
                    {
                        cmdAlerta.Parameters.AddWithValue("@IdConsulta", idConsulta);
                        cmdAlerta.Parameters.AddWithValue("@Tipo", tipoAlerta ?? "Alerta Nutricional");
                        cmdAlerta.Parameters.AddWithValue("@Sev", severidadAlerta ?? "MODERADA");
                        cmdAlerta.Parameters.AddWithValue("@Msg", mensajeAlerta);
                        cmdAlerta.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);
                        cmdAlerta.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sqlDelAlerta = "DELETE FROM AlertasClinicas_DNI101 WHERE IdConsulta_DNI101 = @IdConsulta;";
                    using (var cmdDelAlerta = new SqlCommand(sqlDelAlerta, tx.Connection, tx))
                    {
                        cmdDelAlerta.Parameters.AddWithValue("@IdConsulta", idConsulta);
                        cmdDelAlerta.ExecuteNonQuery();
                    }
                }

                // 6. Actualizar mediciones antropométricas y diagnóstico en la versión activa de la bitácora
                string sqlSnap = @"
                    UPDATE ConsultasNutricionales_Cambios_DNI101
                    SET PesoKg_DNI101 = @Peso,
                        TallaCm_DNI101 = @Talla,
                        PerimetroCefalicoCm_DNI101 = @Perimetro,
                        IMC_DNI101 = @IMC,
                        ClasificacionOMS_DNI101 = @Clasif
                    WHERE IdConsulta_DNI101 = @IdConsulta AND Act = 1;";

                using (var cmdSnap = new SqlCommand(sqlSnap, tx.Connection, tx))
                {
                    cmdSnap.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdSnap.Parameters.AddWithValue("@Peso", pesoKg);
                    cmdSnap.Parameters.AddWithValue("@Talla", tallaCm);
                    cmdSnap.Parameters.AddWithValue("@Perimetro", perimetroCefalico);
                    cmdSnap.Parameters.AddWithValue("@IMC", imc);
                    cmdSnap.Parameters.AddWithValue("@Clasif", clasificacionOMS);
                    cmdSnap.ExecuteNonQuery();
                }

                // 7. Restablecer contexto
                using (var cmdClear = new SqlCommand("EXEC sp_set_session_context @key=N'EsModificacionClinica', @value=0;", tx.Connection, tx))
                {
                    cmdClear.ExecuteNonQuery();
                }

                return true;
            });
        }
    }
}
