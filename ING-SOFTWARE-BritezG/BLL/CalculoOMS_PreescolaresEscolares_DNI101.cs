using BE;
using System;

namespace BLL
{
    public class CalculoOMS_PreescolaresEscolares_DNI101 : IEstrategiaCalculoOMS_DNI101
    {
        public ZScoreResultadoBE_DNI101 CalcularZScores(MedicionAntropometricaBE_DNI101 medicion, int edadMeses, string sexo)
        {
            bool esMasculino = string.Equals(sexo, "M", StringComparison.OrdinalIgnoreCase) ||
                               string.Equals(sexo, "Masculino", StringComparison.OrdinalIgnoreCase);

            double edadAnios = edadMeses / 12.0;

            // Patrones de Referencia OMS (2 a 19 años)
            double tallaEsperada = (esMasculino ? 86.0 : 85.0) + ((edadAnios - 2.0) * 6.2);
            double imcEsperado = (esMasculino ? 15.5 : 15.3) + ((edadAnios - 2.0) * 0.25);
            double pesoEsperado = imcEsperado * Math.Pow(tallaEsperada / 100.0, 2);
            double pcEsperado = (esMasculino ? 49.0 : 48.0) + Math.Min(3.0, (edadAnios - 2.0) * 0.3);

            double sdTalla = 4.0 + ((edadAnios - 2.0) * 0.4);
            double sdIMC = 1.5 + ((edadAnios - 2.0) * 0.15);
            double sdPeso = 2.0 + ((edadAnios - 2.0) * 1.2);
            double sdPC = 1.3;

            double zPeso = Math.Round((medicion.PesoKg_DNI101 - pesoEsperado) / sdPeso, 2);
            double zTalla = Math.Round((medicion.TallaCm_DNI101 - tallaEsperada) / sdTalla, 2);
            double zIMC = Math.Round((medicion.IMC_DNI101 - imcEsperado) / sdIMC, 2);
            double zPC = Math.Round((medicion.PerimetroCefalicoCm_DNI101 - pcEsperado) / sdPC, 2);

            double pctPeso = Math.Round(ZScoreAPercentil(zPeso), 2);
            double pctTalla = Math.Round(ZScoreAPercentil(zTalla), 2);
            double pctIMC = Math.Round(ZScoreAPercentil(zIMC), 2);

            return new ZScoreResultadoBE_DNI101
            {
                ZPesoEdad_DNI101 = zPeso,
                ZTallaEdad_DNI101 = zTalla,
                ZIMCEdad_DNI101 = zIMC,
                ZPCEdad_DNI101 = zPC,
                PercentilPeso_DNI101 = pctPeso,
                PercentilTalla_DNI101 = pctTalla,
                PercentilIMC_DNI101 = pctIMC
            };
        }

        public DiagnosticoNutricionalBE_DNI101 EvaluarClasificacionOMS(ZScoreResultadoBE_DNI101 zscore)
        {
            string clasificacion;
            string detalles;
            bool requiereAlerta = false;

            if (zscore.ZIMCEdad_DNI101 > 2.0)
            {
                clasificacion = "Obesidad (OMS 2-19a)";
                detalles = "Z-IMC > +2.0 D.E. Exceso severo de adiposidad.";
                requiereAlerta = true;
            }
            else if (zscore.ZIMCEdad_DNI101 > 1.0)
            {
                clasificacion = "Sobrepeso (OMS 2-19a)";
                detalles = "Z-IMC entre +1.0 y +2.0 D.E. Se recomienda reestructuración nutricional.";
            }
            else if (zscore.ZIMCEdad_DNI101 < -3.0)
            {
                clasificacion = "Delgadez Severa (OMS 2-19a)";
                detalles = "Z-IMC < -3.0 D.E. Déficit ponderal crítico.";
                requiereAlerta = true;
            }
            else if (zscore.ZIMCEdad_DNI101 < -2.0)
            {
                clasificacion = "Delgadez (OMS 2-19a)";
                detalles = "Z-IMC entre -2.0 y -3.0 D.E.";
                requiereAlerta = true;
            }
            else if (zscore.ZTallaEdad_DNI101 < -2.0)
            {
                clasificacion = "Talla Baja (OMS)";
                detalles = "Z-Talla < -2.0 D.E. Retardo de crecimiento estatural.";
                requiereAlerta = true;
            }
            else
            {
                clasificacion = "Eutrófico / Normal (OMS 2-19a)";
                detalles = "Crecimiento e IMC dentro de límites estándar (+-2.0 D.E.).";
            }

            return new DiagnosticoNutricionalBE_DNI101
            {
                ClasificacionOMS_DNI101 = clasificacion,
                DetallesClinicos_DNI101 = detalles,
                RequiereAlerta_DNI101 = requiereAlerta
            };
        }

        private static double ZScoreAPercentil(double z)
        {
            double sign = (z < 0) ? -1.0 : 1.0;
            double absZ = Math.Abs(z) / Math.Sqrt(2.0);
            double t = 1.0 / (1.0 + 0.3275911 * absZ);
            double erf = 1.0 - (((((1.061405429 * t - 1.453152027) * t + 1.421413741) * t - 0.284496736) * t + 0.254829592) * t * Math.Exp(-absZ * absZ));
            double cdf = 0.5 * (1.0 + sign * erf);
            return cdf * 100.0;
        }
    }
}
