using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using BE.PN2;
using DAL;

namespace BLL.PN2
{
    public class ReporteInteligenteBLL
    {
        private readonly Conexion _conexion;

        public ReporteInteligenteBLL()
        {
            _conexion = new Conexion();
        }

        public List<ReporteInteligenteDTO> GenerarReporteGlobal()
        {
            var reportes = new List<ReporteInteligenteDTO>();

            string sqlPacientes = "SELECT IdPaciente_DNI101 FROM Pacientes_DNI101 WHERE Activo_DNI101 = 1 ORDER BY IdPaciente_DNI101";
            var ids = new List<int>();

            using (var cn = _conexion.CrearConexion())
            using (var cmd = new SqlCommand(sqlPacientes, cn))
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ids.Add(dr.GetInt32(0));
                    }
                }
            }

            foreach (int id in ids)
            {
                var rep = GenerarReportePaciente(id);
                if (rep != null)
                {
                    reportes.Add(rep);
                }
            }

            return reportes;
        }

        public ReporteInteligenteDTO? GenerarReportePaciente(int idPaciente)
        {
            string sql = @"
                SELECT 
                    p.IdPaciente_DNI101, p.Nombre_DNI101, p.Apellido_DNI101, p.DniNiño_DNI101, p.Sexo_DNI101,
                    c.IdConsulta_DNI101, c.FechaControl_DNI101, c.EdadMeses_DNI101,
                    m.PesoKg_DNI101, m.TallaCm_DNI101, m.IMC_DNI101,
                    z.ZIMCEdad_DNI101, z.PercentilIMC_DNI101,
                    d.ClasificacionOMS_DNI101
                FROM Pacientes_DNI101 p
                INNER JOIN ConsultasNutricionales_DNI101 c ON p.IdPaciente_DNI101 = c.IdPaciente_DNI101
                INNER JOIN MedicionesAntropometricas_DNI101 m ON c.IdConsulta_DNI101 = m.IdConsulta_DNI101
                LEFT JOIN ZScoresResultados_DNI101 z ON c.IdConsulta_DNI101 = z.IdConsulta_DNI101
                LEFT JOIN DiagnosticosNutricionales_DNI101 d ON c.IdConsulta_DNI101 = d.IdConsulta_DNI101
                WHERE p.IdPaciente_DNI101 = @IdPac
                ORDER BY c.FechaControl_DNI101 ASC, c.IdConsulta_DNI101 ASC";

            var mediciones = new List<(DateTime fecha, int edadMeses, decimal peso, decimal talla, decimal imc, decimal zImc, decimal percentil, string clasif)>();
            string nombre = string.Empty;
            string apellido = string.Empty;
            string dni = string.Empty;
            string sexo = string.Empty;

            using (var cn = _conexion.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdPac", idPaciente);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        nombre = dr.GetString(1);
                        apellido = dr.GetString(2);
                        dni = dr.GetString(3);
                        sexo = dr.GetString(4);

                        mediciones.Add((
                            dr.GetDateTime(6),
                            dr.GetInt32(7),
                            dr.GetDecimal(8),
                            dr.GetDecimal(9),
                            dr.GetDecimal(10),
                            dr.IsDBNull(11) ? 0m : dr.GetDecimal(11),
                            dr.IsDBNull(12) ? 50m : dr.GetDecimal(12),
                            dr.IsDBNull(13) ? "Sin diagnóstico" : dr.GetString(13)
                        ));
                    }
                }
            }

            if (mediciones.Count == 0) return null;

            var dto = new ReporteInteligenteDTO
            {
                IdPaciente = idPaciente,
                NombreCompleto = $"{nombre} {apellido}",
                Dni = dni,
                Sexo = sexo,
                TotalConsultas = mediciones.Count
            };

            var ult = mediciones[^1];
            dto.EdadMesesActual = ult.edadMeses;
            dto.UltimoPesoKg = ult.peso;
            dto.UltimaTallaCm = ult.talla;
            dto.UltimoIMC = ult.imc;
            dto.UltimoZScoreIMC = ult.zImc;
            dto.UltimoPercentilIMC = ult.percentil;
            dto.DiagnosticoOMSActual = ult.clasif;

            // 1. Cálculo de Velocidad Ponderal y Deriva de Z-Score
            if (mediciones.Count >= 2)
            {
                var penult = mediciones[^2];
                decimal deltaMeses = (decimal)(ult.fecha - penult.fecha).TotalDays / 30.417m;
                if (deltaMeses <= 0) deltaMeses = Math.Max(1, ult.edadMeses - penult.edadMeses);

                dto.VelocidadPesoKgMes = Math.Round((ult.peso - penult.peso) / deltaMeses, 3);
                dto.VelocidadTallaCmMes = Math.Round((ult.talla - penult.talla) / deltaMeses, 3);
                dto.DeltaZScoreIMC = Math.Round(ult.zImc - penult.zImc, 2);
            }
            else
            {
                dto.VelocidadPesoKgMes = 0.50m;
                dto.VelocidadTallaCmMes = 1.20m;
                dto.DeltaZScoreIMC = 0.00m;
            }

            // 2. Proyección predictiva para los próximos 90 días (3 meses)
            dto.PesoProyectado90Dias = Math.Round(dto.UltimoPesoKg + (dto.VelocidadPesoKgMes * 3.0m), 2);
            dto.TallaProyectada90Dias = Math.Round(dto.UltimaTallaCm + (dto.VelocidadTallaCmMes * 3.0m), 2);

            // 3. Motor de Inferencia y Clasificación Predictiva de Riesgo Nutricional
            if (dto.UltimoZScoreIMC < -2.0m || dto.VelocidadPesoKgMes < 0.10m || dto.DeltaZScoreIMC < -0.80m)
            {
                dto.NivelRiesgo = "Crítico";
                dto.ClasificacionRiesgoFuturo = "Riesgo Severo de Malnutrición por Desaceleración Ponderal Aguda";
                dto.RecomendacionFrecuenciaControl = "Adelantar control urgente a 15 días (vigilancia estrecha)";
                dto.RecomendacionDietoterapica = "Prescribir Plan Hipercalórico enriquecido con módulo calórico y evaluación de deglución.";
                dto.SuplementacionSugerida = "Módulo Calórico de Maltodextrina / Complejo Polivitamínico en Gotas";
                dto.PautaAlimentariaEspecial = "Fraccionar tomas cada 2.5 a 3 horas; densificar papillas con aceite vegetal crudo y carnes procesadas.";
            }
            else if (dto.UltimoZScoreIMC > 2.0m || (dto.VelocidadPesoKgMes > 1.20m && dto.DeltaZScoreIMC > 0.60m))
            {
                dto.NivelRiesgo = "Alto";
                dto.ClasificacionRiesgoFuturo = "Riesgo de Sobrepeso Infantil y Obesidad Acelerada";
                dto.RecomendacionFrecuenciaControl = "Reevaluación antropométrica en 30 días";
                dto.RecomendacionDietoterapica = "Ajustar densidad calórica de tomas, promover lactancia materna y pautas familiares de movimiento activo.";
                dto.SuplementacionSugerida = "Sin suplementación calórica requerida; control periódico de micronutrientes";
                dto.PautaAlimentariaEspecial = "Evitar azúcares añadidos, jugos industriales y ultraprocesados; fomentar autorregulación del apetito infantil.";
            }
            else if (dto.UltimoZScoreIMC < -1.0m || dto.VelocidadPesoKgMes < 0.25m)
            {
                dto.NivelRiesgo = "Moderado";
                dto.ClasificacionRiesgoFuturo = "Alerta de Riesgo Nutricional Leve / Aplanamiento de Curva";
                dto.RecomendacionFrecuenciaControl = "Control preventivo a los 30 días";
                dto.RecomendacionDietoterapica = "Fortificar alimentación complementaria e incorporar suplementación vitamínico-mineral.";
                dto.SuplementacionSugerida = "Suplemento Polivitamínico Pediátrico en Gotas y Sulfato Ferroso (dosis profiláctica)";
                dto.PautaAlimentariaEspecial = "Aumentar frecuencia de alimentos ricos en hierro biodisponible y vegetales coloridos.";
            }
            else
            {
                dto.NivelRiesgo = "Bajo";
                dto.ClasificacionRiesgoFuturo = "Trayectoria de Crecimiento Eutrófica y Armónica (OMS P50)";
                dto.RecomendacionFrecuenciaControl = "Control periódico habitual cada 60 o 90 días";
                dto.RecomendacionDietoterapica = "Continuar pautas alimentarias saludables y lactancia materna acordes a la edad.";
                dto.SuplementacionSugerida = "Profilaxis habitual con Sulfato Ferroso y Vitamina D3 según normativa pediátrica nacional.";
                dto.PautaAlimentariaEspecial = "Mantener mesa familiar compartida y variedad de texturas y alimentos naturales.";
            }

            return dto;
        }
    }
}
