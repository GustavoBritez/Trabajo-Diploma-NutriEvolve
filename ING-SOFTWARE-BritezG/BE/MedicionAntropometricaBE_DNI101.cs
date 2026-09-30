using System;

namespace BE
{
    public class MedicionAntropometricaBE_DNI101
    {
        public int IdMedicion_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public double PesoKg_DNI101 { get; set; }
        public double TallaCm_DNI101 { get; set; }
        public double PerimetroCefalicoCm_DNI101 { get; set; }
        public double? CircunferenciaCinturaCm_DNI101 { get; set; }
        public double IMC_DNI101 { get; set; }
        public string? DV { get; set; }
    }
}
