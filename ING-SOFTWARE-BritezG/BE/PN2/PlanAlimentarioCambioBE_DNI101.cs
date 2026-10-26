using System;

namespace BE.PN2
{
    public class PlanAlimentarioCambioBE_DNI101
    {
        public int IdCambio { get; set; }
        public int IdPlan { get; set; }
        public int IdConsulta { get; set; }
        public decimal RequerimientoCalorico { get; set; }
        public decimal PctCarbohidratos { get; set; }
        public decimal PctProteinas { get; set; }
        public decimal PctGrasas { get; set; }
        public string? PautasFamiliares { get; set; }
        public string? MetasSalud { get; set; }
        public DateTime FechaCambio { get; set; }
        public int UsuarioDni { get; set; }
        public string TipoOperacion { get; set; } = string.Empty;
    }
}
