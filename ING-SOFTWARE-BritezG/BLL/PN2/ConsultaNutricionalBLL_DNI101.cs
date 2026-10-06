using System;
using System.Collections.Generic;
using System.Data;
using BE.PN2;
using BLL.PN2.Strategy;
using DAL.PN2;
using Services;

namespace BLL.PN2
{
    public class ConsultaNutricionalBLL_DNI101
    {
        private readonly ConsultaNutricionalDAL_DNI101 _dal;
        private readonly BitacoraBLL _bitacoraBLL;
        private readonly DigitoVerificadorBLL _dvBLL;
        private readonly EvaluadorNutricionalContext_DNI101 _evaluadorContext;

        public ConsultaNutricionalBLL_DNI101()
        {
            _dal = new ConsultaNutricionalDAL_DNI101();
            _bitacoraBLL = new BitacoraBLL();
            _dvBLL = new DigitoVerificadorBLL();
            _evaluadorContext = new EvaluadorNutricionalContext_DNI101(new EvaluacionIMCEdadStrategy());
        }

        public decimal CalcularIMC(decimal pesoKg, decimal tallaCm)
        {
            if (tallaCm <= 0) return 0;
            decimal tallaM = tallaCm / 100m;
            return Math.Round(pesoKg / (tallaM * tallaM), 2);
        }

        public ResultadoEvaluacionOMS EvaluarEstadoNutricional(decimal imc, int edadMeses, string sexo)
        {
            _evaluadorContext.SetEstrategia(new EvaluacionIMCEdadStrategy());
            return _evaluadorContext.EjecutarEvaluacion(imc, edadMeses, sexo);
        }

        public ResultadoEvaluacionOMS EvaluarPeso(decimal pesoKg, int edadMeses, string sexo)
        {
            _evaluadorContext.SetEstrategia(new EvaluacionPesoEdadStrategy());
            return _evaluadorContext.EjecutarEvaluacion(pesoKg, edadMeses, sexo);
        }

        public ResultadoEvaluacionOMS EvaluarTalla(decimal tallaCm, int edadMeses, string sexo)
        {
            _evaluadorContext.SetEstrategia(new EvaluacionTallaEdadStrategy());
            return _evaluadorContext.EjecutarEvaluacion(tallaCm, edadMeses, sexo);
        }

        /// <summary>
        /// Registra una consulta nutricional pediátrica completa (CUN08) en un único acceso a la base de datos.
        /// </summary>
        public int RegistrarConsulta(
            int idPaciente,
            int edadMeses,
            string sexo,
            decimal pesoKg,
            decimal tallaCm,
            decimal perimetroCefalico,
            DateTime fechaControl,
            string? tipoLactancia,
            string? alimentacionComp,
            string? alergias,
            string? antFamiliares,
            string? observaciones)
        {
            if (idPaciente <= 0)
                throw new ArgumentException("Debe seleccionar un paciente válido.");
            if (pesoKg <= 0 || pesoKg > 150)
                throw new ArgumentException("El peso debe ser un valor biológico válido (mayor a 0 kg).");
            if (tallaCm <= 20 || tallaCm > 220)
                throw new ArgumentException("La talla/longitud debe ser un valor biológico válido (entre 20 y 220 cm).");

            decimal imc = CalcularIMC(pesoKg, tallaCm);
            ResultadoEvaluacionOMS eval = EvaluarEstadoNutricional(imc, edadMeses, sexo);

            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            if (dniActual == 0) dniActual = 11111111;

            string dvDummy = $"{idPaciente}-{pesoKg}-{tallaCm}-{fechaControl:yyyyMMdd}";

            // Único acceso a la base de datos
            int idConsulta = _dal.GuardarConsultaCompleta(
                idPaciente: idPaciente,
                dniNutricionista: dniActual,
                fechaControl: fechaControl,
                edadMeses: edadMeses,
                tipoLactancia: tipoLactancia,
                alimentacionComp: alimentacionComp,
                alergias: alergias,
                antFamiliares: antFamiliares,
                observaciones: observaciones,
                pesoKg: pesoKg,
                tallaCm: tallaCm,
                perimetroCefalico: perimetroCefalico,
                imc: imc,
                clasificacionOMS: eval.Clasificacion,
                detallesClinicos: eval.Detalles,
                requiereAlerta: eval.RequiereAlerta,
                tipoAlerta: eval.RequiereAlerta ? "Alerta Nutricional" : null,
                severidadAlerta: eval.SeveridadAlerta,
                mensajeAlerta: eval.MensajeAlerta,
                dv: dvDummy
            );

            // Transversales: Bitácora y Dígito Verificador
            try
            {
                _bitacoraBLL.RegistrarBitacora(2, $"CUN08: Registro de control nutricional #{idConsulta} para paciente ID {idPaciente}. Diagnóstico: {eval.Clasificacion}", dniActual, "SeguimientoNutricional");
                _dvBLL.RecalcularYPersistir();
            }
            catch { }

            return idConsulta;
        }

        public List<ConsultaNutricionalBE_DNI101> ObtenerHistorialPorPaciente(int idPaciente)
        {
            DataTable dt = _dal.ObtenerHistorialPorPaciente(idPaciente);
            List<ConsultaNutricionalBE_DNI101> lista = new List<ConsultaNutricionalBE_DNI101>();

            foreach (DataRow row in dt.Rows)
            {
                var consulta = new ConsultaNutricionalBE_DNI101
                {
                    IdConsulta_DNI101 = Convert.ToInt32(row["IdConsulta_DNI101"]),
                    IdPaciente_DNI101 = Convert.ToInt32(row["IdPaciente_DNI101"]),
                    DniNutricionista_DNI101 = Convert.ToInt32(row["DniNutricionista_DNI101"]),
                    FechaControl_DNI101 = Convert.ToDateTime(row["FechaControl_DNI101"]),
                    EdadMeses_DNI101 = Convert.ToInt32(row["EdadMeses_DNI101"]),
                    TipoLactancia_DNI101 = row["TipoLactancia_DNI101"] != DBNull.Value ? row["TipoLactancia_DNI101"].ToString() : null,
                    AlimentacionComplementaria_DNI101 = row["AlimentacionComplementaria_DNI101"] != DBNull.Value ? row["AlimentacionComplementaria_DNI101"].ToString() : null,
                    Alergias_DNI101 = row["Alergias_DNI101"] != DBNull.Value ? row["Alergias_DNI101"].ToString() : null,
                    AntecedentesFamiliares_DNI101 = row["AntecedentesFamiliares_DNI101"] != DBNull.Value ? row["AntecedentesFamiliares_DNI101"].ToString() : null,
                    Observaciones_DNI101 = row["Observaciones_DNI101"] != DBNull.Value ? row["Observaciones_DNI101"].ToString() : null,
                };

                if (row["IdMedicion_DNI101"] != DBNull.Value)
                {
                    consulta.Medicion = new MedicionAntropometricaBE_DNI101
                    {
                        IdMedicion_DNI101 = Convert.ToInt32(row["IdMedicion_DNI101"]),
                        IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                        PesoKg_DNI101 = Convert.ToDecimal(row["PesoKg_DNI101"]),
                        TallaCm_DNI101 = Convert.ToDecimal(row["TallaCm_DNI101"]),
                        PerimetroCefalicoCm_DNI101 = Convert.ToDecimal(row["PerimetroCefalicoCm_DNI101"]),
                        IMC_DNI101 = Convert.ToDecimal(row["IMC_DNI101"])
                    };
                }

                if (row["IdDiagnostico_DNI101"] != DBNull.Value)
                {
                    consulta.Diagnostico = new DiagnosticoNutricionalBE_DNI101
                    {
                        IdDiagnostico_DNI101 = Convert.ToInt32(row["IdDiagnostico_DNI101"]),
                        IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                        ClasificacionOMS_DNI101 = row["ClasificacionOMS_DNI101"]?.ToString() ?? "",
                        DetallesClinicos_DNI101 = row["DetallesClinicos_DNI101"]?.ToString() ?? "",
                        RequiereAlerta_DNI101 = row["RequiereAlerta_DNI101"] != DBNull.Value && Convert.ToBoolean(row["RequiereAlerta_DNI101"])
                    };
                }

                if (row["IdAlerta_DNI101"] != DBNull.Value)
                {
                    consulta.Alertas.Add(new AlertaClinicaBE_DNI101
                    {
                        IdAlerta_DNI101 = Convert.ToInt32(row["IdAlerta_DNI101"]),
                        IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                        TipoAlerta_DNI101 = row["TipoAlerta_DNI101"]?.ToString() ?? "",
                        Severidad_DNI101 = row["Severidad_DNI101"]?.ToString() ?? "",
                        MensajeAlerta_DNI101 = row["MensajeAlerta_DNI101"]?.ToString() ?? "",
                        FechaGeneracion_DNI101 = consulta.FechaControl_DNI101
                    });
                }

                lista.Add(consulta);
            }

            return lista;
        }

        public ConsultaNutricionalBE_DNI101? ObtenerUltimaConsultaPorPaciente(int idPaciente)
        {
            DataTable dt = _dal.ObtenerUltimaConsultaPorPaciente(idPaciente);
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            var consulta = new ConsultaNutricionalBE_DNI101
            {
                IdConsulta_DNI101 = Convert.ToInt32(row["IdConsulta_DNI101"]),
                IdPaciente_DNI101 = Convert.ToInt32(row["IdPaciente_DNI101"]),
                DniNutricionista_DNI101 = Convert.ToInt32(row["DniNutricionista_DNI101"]),
                FechaControl_DNI101 = Convert.ToDateTime(row["FechaControl_DNI101"]),
                EdadMeses_DNI101 = Convert.ToInt32(row["EdadMeses_DNI101"]),
                Observaciones_DNI101 = row["Observaciones_DNI101"] != DBNull.Value ? row["Observaciones_DNI101"].ToString() : null,
            };

            if (row["PesoKg_DNI101"] != DBNull.Value)
            {
                consulta.Medicion = new MedicionAntropometricaBE_DNI101
                {
                    IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                    PesoKg_DNI101 = Convert.ToDecimal(row["PesoKg_DNI101"]),
                    TallaCm_DNI101 = Convert.ToDecimal(row["TallaCm_DNI101"]),
                    PerimetroCefalicoCm_DNI101 = Convert.ToDecimal(row["PerimetroCefalicoCm_DNI101"]),
                    IMC_DNI101 = Convert.ToDecimal(row["IMC_DNI101"])
                };
            }

            if (row["ClasificacionOMS_DNI101"] != DBNull.Value)
            {
                consulta.Diagnostico = new DiagnosticoNutricionalBE_DNI101
                {
                    IdConsulta_DNI101 = consulta.IdConsulta_DNI101,
                    ClasificacionOMS_DNI101 = row["ClasificacionOMS_DNI101"]?.ToString() ?? "",
                    DetallesClinicos_DNI101 = row["DetallesClinicos_DNI101"]?.ToString() ?? "",
                    RequiereAlerta_DNI101 = row["RequiereAlerta_DNI101"] != DBNull.Value && Convert.ToBoolean(row["RequiereAlerta_DNI101"])
                };
            }

            return consulta;
        }
    }
}
