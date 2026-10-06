using System;

namespace BE.PN2
{
    public class PlanAlimentarioBE_DNI101
    {
        public int IdPlan_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public decimal RequerimientoCalorico_DNI101 { get; set; }
        public decimal PctCarbohidratos_DNI101 { get; set; }
        public decimal PctProteinas_DNI101 { get; set; }
        public decimal PctGrasas_DNI101 { get; set; }
        public string PautasFamiliares_DNI101 { get; set; } = string.Empty;
        public string MetasSalud_DNI101 { get; set; } = string.Empty;
        public string? DV { get; set; }

        public decimal GramosCarbohidratos => RequerimientoCalorico_DNI101 > 0 ? Math.Round((RequerimientoCalorico_DNI101 * (PctCarbohidratos_DNI101 / 100m)) / 4m, 1) : 0;
        public decimal GramosProteinas => RequerimientoCalorico_DNI101 > 0 ? Math.Round((RequerimientoCalorico_DNI101 * (PctProteinas_DNI101 / 100m)) / 4m, 1) : 0;
        public decimal GramosGrasas => RequerimientoCalorico_DNI101 > 0 ? Math.Round((RequerimientoCalorico_DNI101 * (PctGrasas_DNI101 / 100m)) / 9m, 1) : 0;

        public bool EsValido()
        {
            return (PctCarbohidratos_DNI101 + PctProteinas_DNI101 + PctGrasas_DNI101) == 100m && RequerimientoCalorico_DNI101 > 0;
        }
    }
}
