using System;

namespace BE
{
    public class ConsultaNutricionalBE_DNI101
    {
        public int IdConsulta_DNI101 { get; set; }
        public int IdPaciente_DNI101 { get; set; }
        public int DniNutricionista_DNI101 { get; set; }
        public int? IdTurno_DNI101 { get; set; }
        public DateTime FechaControl_DNI101 { get; set; } = DateTime.Now;
        public int EdadMeses_DNI101 { get; set; }
        public string? TipoLactancia_DNI101 { get; set; }
        public string? AlimentacionComplementaria_DNI101 { get; set; }
        public string? Alergias_DNI101 { get; set; }
        public string? AntecedentesFamiliares_DNI101 { get; set; }
        public string? Observaciones_DNI101 { get; set; }
        public string? DV { get; set; }

        // Entidades Navegacionales / Asoc.
        public PacienteBE_DNI101? Paciente_DNI101 { get; set; }
        public MedicionAntropometricaBE_DNI101? Medicion_DNI101 { get; set; }
        public ZScoreResultadoBE_DNI101? ZScores_DNI101 { get; set; }
        public DiagnosticoNutricionalBE_DNI101? Diagnostico_DNI101 { get; set; }
        public AlertaClinicaBE_DNI101? Alerta_DNI101 { get; set; }
        public PlanAlimentarioBE_DNI101? PlanAlimentario_DNI101 { get; set; }
        public Recordatorio24hBE_DNI101? Recordatorio_DNI101 { get; set; }
    }
}
