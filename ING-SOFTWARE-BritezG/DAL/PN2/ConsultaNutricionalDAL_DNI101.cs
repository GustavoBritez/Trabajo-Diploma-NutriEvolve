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
    }
}
