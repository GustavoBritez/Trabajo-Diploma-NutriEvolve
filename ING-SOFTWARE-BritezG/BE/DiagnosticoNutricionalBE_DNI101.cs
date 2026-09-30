using System;

namespace BE
{
    public class DiagnosticoNutricionalBE_DNI101
    {
        public int IdDiagnostico_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public string ClasificacionOMS_DNI101 { get; set; } = string.Empty;
        public string DetallesClinicos_DNI101 { get; set; } = string.Empty;
        public bool RequiereAlerta_DNI101 { get; set; }
        public string? DV { get; set; }
    }
}
