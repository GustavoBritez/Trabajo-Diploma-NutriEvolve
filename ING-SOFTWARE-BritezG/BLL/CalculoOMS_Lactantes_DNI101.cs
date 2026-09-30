using BE;
using System;

namespace BLL
{
    public class CalculoOMS_Lactantes_DNI101 : IEstrategiaCalculoOMS_DNI101
    {
        public ZScoreResultadoBE_DNI101 CalcularZScores(MedicionAntropometricaBE_DNI101 medicion, int edadMeses, string sexo)
        {
            bool esMasculino = string.Equals(sexo, "M", StringComparison.OrdinalIgnoreCase) ||
                               string.Equals(sexo, "Masculino", StringComparison.OrdinalIgnoreCase);

            // Valores de referencia base (OMS 0-24m)
            double pesoEsperado = (esMasculino ? 3.3 : 3.2) + (edadMeses * 0.55);
            double tallaEsperada = (esMasculino ? 50.0 : 49.0) + (edadMeses * 1.8);
            double imcEsperado = pesoEsperado / Math.Pow(tallaEsperada / 100.0, 2);
            double pcEsperado = (esMasculino ? 34.5 : 34.0) + (edadMeses * 0.6);

            // Desviaciones estándar estimadas OMS (Lactantes)
            double sdPeso = 0.8 + (edadMeses * 0.08);
            double sdTalla = 2.2 + (edadMeses * 0.1);
            double sdIMC = 1.2 + (edadMeses * 0.03);
            double sdPC = 1.2 + (edadMeses * 0.04);

            double zPeso = Math.Round((medicion.PesoKg_DNI101 - pesoEsperado) / sdPeso, 2);
            double zTalla = Math.Round((medicion.TallaCm_DNI101 - tallaEsperada) / sdTalla, 2);
            double zIMC = Math.Round((medicion.IMC_DNI101 - imcEsperado) / sdIMC, 2);
            double zPC = Math.Round((medicion.PerimetroCefalicoCm_DNI101 - pcEsperado) / sdPC, 2);

            // Conversión de Z-Score a Percentil (Distribución Normal estándar)
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

            if (zscore.ZIMCEdad_DNI101 > 3.0)
            {
                clasificacion = "Obesidad Severa (OMS)";
                detalles = "Z-IMC > +3.0. Riesgo metabólico y cardiovascular elevado.";
                requiereAlerta = true;
            }
            else if (zscore.ZIMCEdad_DNI101 > 2.0)
            {
                clasificacion = "Obesidad (OMS)";
                detalles = "Z-IMC entre +2.0 y +3.0. Exceso significativo de masa corporal.";
                requiereAlerta = true;
            }
            else if (zscore.ZIMCEdad_DNI101 > 1.0)
            {
                clasificacion = "Riesgo de Sobrepeso (OMS)";
                detalles = "Z-IMC entre +1.0 y +2.0. Requiere pautas de alimentación familiar activa.";
            }
            else if (zscore.ZIMCEdad_DNI101 < -3.0)
            {
                clasificacion = "Desnutrición Aguda Severa (OMS)";
                detalles = "Z-IMC < -3.0. Déficit crítico de masa corporal. Intervención prioritaria.";
                requiereAlerta = true;
            }
            else if (zscore.ZIMCEdad_DNI101 < -2.0)
            {
                clasificacion = "Desnutrición Aguda Moderada (OMS)";
                detalles = "Z-IMC entre -2.0 y -3.0. Requiere suplementación e intensificación de controles.";
                requiereAlerta = true;
            }
            else if (zscore.ZTallaEdad_DNI101 < -2.0)
            {
                clasificacion = "Talla Baja para la Edad (OMS)";
                detalles = "Z-Talla < -2.0. Posible desnutrición crónica o retardo estatural.";
                requiereAlerta = true;
            }
            else
            {
                clasificacion = "Eutrófico / Normal (OMS)";
                detalles = "Variables antropométricas y Z-Scores dentro de rangos normales de crecimiento OMS (-2 a +1 D.E.).";
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
            // Aproximación polinomial de la CDF Normal Estándar
            double sign = (z < 0) ? -1.0 : 1.0;
            double absZ = Math.Abs(z) / Math.Sqrt(2.0);
            double t = 1.0 / (1.0 + 0.3275911 * absZ);
            double erf = 1.0 - (((((1.061405429 * t - 1.453152027) * t + 1.421413741) * t - 0.284496736) * t + 0.254829592) * t * Math.Exp(-absZ * absZ));
            double cdf = 0.5 * (1.0 + sign * erf);
            return cdf * 100.0;
        }
    }
}
