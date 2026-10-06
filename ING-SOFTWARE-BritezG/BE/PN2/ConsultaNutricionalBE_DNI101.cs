using System;
using System.Collections.Generic;
using BE;

namespace BE.PN2
{
    public class ConsultaNutricionalBE_DNI101
    {
        public int IdConsulta_DNI101 { get; set; }
        public int IdPaciente_DNI101 { get; set; }
        public int DniNutricionista_DNI101 { get; set; }
        public int? IdTurno_DNI101 { get; set; }
        public DateTime FechaControl_DNI101 { get; set; }
        public int EdadMeses_DNI101 { get; set; }
        public string? TipoLactancia_DNI101 { get; set; }
        public string? AlimentacionComplementaria_DNI101 { get; set; }
        public string? Alergias_DNI101 { get; set; }
        public string? AntecedentesFamiliares_DNI101 { get; set; }
        public string? Observaciones_DNI101 { get; set; }
        public string? DV { get; set; }

        // Navegación clínica
        public PacienteBE_DNI101? Paciente { get; set; }
        public MedicionAntropometricaBE_DNI101? Medicion { get; set; }
        public DiagnosticoNutricionalBE_DNI101? Diagnostico { get; set; }
        public PlanAlimentarioBE_DNI101? PlanAlimentario { get; set; }
        public List<AlertaClinicaBE_DNI101> Alertas { get; set; } = new List<AlertaClinicaBE_DNI101>();

        public ConsultaNutricionalBE_DNI101()
        {
            FechaControl_DNI101 = DateTime.Now;
        }
    }
}
