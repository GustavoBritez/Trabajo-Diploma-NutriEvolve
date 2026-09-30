using System;

namespace BE
{
    public class Recordatorio24hBE_DNI101
    {
        public int IdRecordatorio_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public string Desayuno_DNI101 { get; set; } = string.Empty;
        public string Almuerzo_DNI101 { get; set; } = string.Empty;
        public string Merienda_DNI101 { get; set; } = string.Empty;
        public string Cena_DNI101 { get; set; } = string.Empty;
        public string? Colaciones_DNI101 { get; set; }
        public string? FrecuenciaAlimentos_DNI101 { get; set; }
        public string? DV { get; set; }
    }
}
