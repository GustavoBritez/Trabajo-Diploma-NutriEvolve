using System;

namespace BE
{
    public class PlanAlimentarioBE_DNI101
    {
        public int IdPlan_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public double RequerimientoCalorico_DNI101 { get; set; }
        public double PctCarbohidratos_DNI101 { get; set; }
        public double PctProteinas_DNI101 { get; set; }
        public double PctGrasas_DNI101 { get; set; }
        public string PautasFamiliares_DNI101 { get; set; } = string.Empty;
        public string MetasSalud_DNI101 { get; set; } = string.Empty;
        public string? DV { get; set; }
    }
}
