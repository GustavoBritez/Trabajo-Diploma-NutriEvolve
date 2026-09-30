using BE;
using DAL;
using Services;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class SeguimientoNutricionalBLL_DNI101
    {
        private readonly ConsultaNutricionalDAL_DNI101 _consultaDAL = new();
        private readonly PacienteDAL_DNI101 _pacienteDAL = new();
        private readonly TurnoBLL_DNI101 _turnoBLL = new();
        private readonly EventoBLL _bitacoraBLL = new();
        private readonly EvaluadorCrecimientoSubject_DNI101 _sujetoAlertas = new();

        public SeguimientoNutricionalBLL_DNI101()
        {
            // Suscribir observador de bitácora al sujeto de alertas
            _sujetoAlertas.Suscribir(new BitacoraAlertaObserver_DNI101());
        }

        public PacienteBE_DNI101? ObtenerPacientePorDNI(string dniNiño)
        {
            if (string.IsNullOrWhiteSpace(dniNiño)) throw new ArgumentException("El DNI del niño es requerido.", nameof(dniNiño));
            return _pacienteDAL.ObtenerPacientePorDNI(dniNiño.Trim());
        }

        public List<PacienteBE_DNI101> ListarPacientes()
        {
            return _pacienteDAL.ListarTodos();
        }

        public List<ConsultaNutricionalBE_DNI101> ObtenerHistoriaClinica(int idPaciente)
        {
            if (idPaciente <= 0) return new List<ConsultaNutricionalBE_DNI101>();
            return _consultaDAL.ListarHistorialPorPaciente(idPaciente);
        }

        public (MedicionAntropometricaBE_DNI101 medicion, ZScoreResultadoBE_DNI101 zscores, DiagnosticoNutricionalBE_DNI101 diagnostico) CalcularZScoresEnVivo(
            double pesoKg, double tallaCm, double perimetroCefalicoCm, double? circunferenciaCinturaCm, int edadMeses, string sexo)
        {
            if (pesoKg <= 0) throw new ArgumentException("El peso debe ser mayor a 0 kg.", nameof(pesoKg));
            if (tallaCm <= 0) throw new ArgumentException("La talla debe ser mayor a 0 cm.", nameof(tallaCm));
            if (perimetroCefalicoCm <= 0) throw new ArgumentException("El perímetro cefálico debe ser mayor a 0 cm.", nameof(perimetroCefalicoCm));

            double imc = Math.Round(pesoKg / Math.Pow(tallaCm / 100.0, 2), 2);

            var medicion = new MedicionAntropometricaBE_DNI101
            {
                PesoKg_DNI101 = pesoKg,
                TallaCm_DNI101 = tallaCm,
                PerimetroCefalicoCm_DNI101 = perimetroCefalicoCm,
                CircunferenciaCinturaCm_DNI101 = circunferenciaCinturaCm,
                IMC_DNI101 = imc
            };

            // Aplicación del Patrón Strategy para Z-Scores OMS
            var contextoStrategy = new EvaluadorNutricionalContext_DNI101(edadMeses);
            var zscores = contextoStrategy.EjecutarCalculoZScores(medicion, edadMeses, sexo);
            var diagnostico = contextoStrategy.EjecutarEvaluacionDiagnostico(zscores);

            return (medicion, zscores, diagnostico);
        }

        public ConsultaNutricionalBE_DNI101 RegistrarConsulta(
            ConsultaNutricionalBE_DNI101 consulta,
            Action<AlertaClinicaBE_DNI101>? callbackAlertaUI = null)
        {
            if (consulta == null) throw new ArgumentNullException(nameof(consulta));
            if (consulta.IdPaciente_DNI101 <= 0) throw new InvalidOperationException("Debe seleccionar un paciente pediátrico válido.");

            // Si hay un observador visual de UI, lo suscribimos temporalmente
            IObservadorAlertaClinica_DNI101? obsUI = null;
            if (callbackAlertaUI != null)
            {
                obsUI = new AlertaVisualUIObserver_DNI101(callbackAlertaUI);
                _sujetoAlertas.Suscribir(obsUI);
            }

            try
            {
                // 1. Obtener Nutricionista DNI activo
                int dniNutricionista = 0;
                try
                {
                    dniNutricionista = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                }
                catch { }

                consulta.DniNutricionista_DNI101 = dniNutricionista;
                consulta.FechaControl_DNI101 = DateTime.Now;

                // 2. Evaluar Alerta Clínica previa mediante el Patrón Observer
                var ultimaConsulta = _consultaDAL.ObtenerUltimaConsulta(consulta.IdPaciente_DNI101);
                ZScoreResultadoBE_DNI101? zscorePrevio = ultimaConsulta?.ZScores_DNI101;

                if (consulta.ZScores_DNI101 != null)
                {
                    consulta.Alerta_DNI101 = _sujetoAlertas.EvaluarAlertaCrecimiento(consulta.ZScores_DNI101, zscorePrevio);
                }

                // 3. Persistencia Atómica en Base de Datos vía DAL
                int idConsultaGenerada = _consultaDAL.GuardarConsultaCompleta(consulta);
                consulta.IdConsulta_DNI101 = idConsultaGenerada;

                // 4. Si la consulta proviene de un Turno agendado, se marca como Asistió
                if (consulta.IdTurno_DNI101.HasValue && consulta.IdTurno_DNI101.Value > 0)
                {
                    try
                    {
                        _turnoBLL.MarcarAsistencia(consulta.IdTurno_DNI101.Value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Nota: No se pudo actualizar el estado del turno: {ex.Message}");
                    }
                }

                // 5. Actualización de Integridad por Dígito Verificador (DV)
                try
                {
                    new DigitoVerificadorBLL().RecalcularYPersistir();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al recalcular DV: {ex.Message}");
                }

                // 6. Registro del evento en Bitácora de Auditoría
                _bitacoraBLL.RegistrarEvento(
                    2,
                    $"Consulta Nutricional #{idConsultaGenerada} registrada exitosamente (CUN11) para el Paciente #{consulta.IdPaciente_DNI101}.",
                    dniNutricionista,
                    "SeguimientoNutricional"
                );

                return consulta;
            }
            finally
            {
                if (obsUI != null)
                {
                    _sujetoAlertas.Desuscribir(obsUI);
                }
            }
        }
    }
}
