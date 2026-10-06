using System;

namespace BE.PN2
{
    public class AlertaClinicaBE_DNI101
    {
        public int IdAlerta_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public string TipoAlerta_DNI101 { get; set; } = string.Empty;
        public string Severidad_DNI101 { get; set; } = string.Empty; // CRÍTICA, MODERADA, LEVE
        public string MensajeAlerta_DNI101 { get; set; } = string.Empty;
        public DateTime FechaGeneracion_DNI101 { get; set; }
        public string? DV { get; set; }

        public AlertaClinicaBE_DNI101() { }

        public AlertaClinicaBE_DNI101(string tipo, string severidad, string mensaje)
        {
            TipoAlerta_DNI101 = tipo;
            Severidad_DNI101 = severidad;
            MensajeAlerta_DNI101 = mensaje;
            FechaGeneracion_DNI101 = DateTime.Now;
        }
    }
}
