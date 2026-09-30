using System;

namespace BE
{
    public class AlertaClinicaBE_DNI101
    {
        public int IdAlerta_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public string TipoAlerta_DNI101 { get; set; } = string.Empty;
        public string Severidad_DNI101 { get; set; } = "Baja"; // Baja, Media, Alta, Crítica
        public string MensajeAlerta_DNI101 { get; set; } = string.Empty;
        public DateTime FechaGeneracion_DNI101 { get; set; } = DateTime.Now;
        public string? DV { get; set; }
    }
}
