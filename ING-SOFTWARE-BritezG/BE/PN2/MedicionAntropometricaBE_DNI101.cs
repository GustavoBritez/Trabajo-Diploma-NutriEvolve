using System;

namespace BE.PN2
{
    public class MedicionAntropometricaBE_DNI101
    {
        public int IdMedicion_DNI101 { get; set; }
        public int IdConsulta_DNI101 { get; set; }
        public decimal PesoKg_DNI101 { get; set; }
        public decimal TallaCm_DNI101 { get; set; }
        public decimal PerimetroCefalicoCm_DNI101 { get; set; }
        public decimal? CircunferenciaCinturaCm_DNI101 { get; set; }
        public decimal IMC_DNI101 { get; set; }
        public string? DV { get; set; }

        public MedicionAntropometricaBE_DNI101() { }

        public MedicionAntropometricaBE_DNI101(decimal peso, decimal talla, decimal perimetro = 0, decimal? cintura = null)
        {
            PesoKg_DNI101 = peso;
            TallaCm_DNI101 = talla;
            PerimetroCefalicoCm_DNI101 = perimetro;
            CircunferenciaCinturaCm_DNI101 = cintura;
            if (talla > 0)
            {
                decimal tallaMetros = talla / 100m;
                IMC_DNI101 = Math.Round(peso / (tallaMetros * tallaMetros), 2);
            }
        }
    }
}
