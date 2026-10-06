using System;

namespace BLL.PN2.Strategy
{
    public class EvaluacionTallaEdadStrategy : IEvaluadorNutricionalStrategy_DNI101
    {
        public string NombreIndicador => "Talla para la Edad";

        public ResultadoEvaluacionOMS Evaluar(decimal valorTalla, int edadMeses, string sexo)
        {
            var curva = CurvasOMSData_DNI101.ObtenerCurva("TALLA", sexo);
            
            int idx = 0;
            double minDist = double.MaxValue;
            for (int i = 0; i < curva.Meses.Length; i++)
            {
                double dist = Math.Abs(curva.Meses[i] - edadMeses);
                if (dist < minDist)
                {
                    minDist = dist;
                    idx = i;
                }
            }

            double val = (double)valorTalla;
            double p3 = curva.P3[idx];
            double p15 = curva.P15[idx];
            double p50 = curva.P50[idx];
            double p85 = curva.P85[idx];
            double p97 = curva.P97[idx];

            ResultadoEvaluacionOMS res = new ResultadoEvaluacionOMS();

            if (val < p3)
            {
                res.Clasificacion = "Talla Baja Severa / Retardo del Crecimiento";
                res.Detalles = $"Talla ({valorTalla:N2} cm) por debajo de P3 ({p3:N2} cm). Posible desnutrición crónica.";
                res.PercentilAproximado = 2m;
                res.RequiereAlerta = true;
                res.SeveridadAlerta = "CRÍTICA";
                res.MensajeAlerta = $"🚨 ALERTA ROJA: Talla de {valorTalla:N2} cm por debajo de P3. Riesgo de retraso estatural severo.";
            }
            else if (val < p15)
            {
                res.Clasificacion = "Riesgo de Talla Baja";
                res.Detalles = $"Talla ({valorTalla:N2} cm) entre P3 y P15 ({p3:N2} - {p15:N2} cm).";
                res.PercentilAproximado = 10m;
                res.RequiereAlerta = false;
            }
            else if (val <= p85)
            {
                res.Clasificacion = "Talla Adecuada";
                res.Detalles = $"Talla ({valorTalla:N2} cm) en rango P15-P85 ({p15:N2} - {p85:N2} cm).";
                res.PercentilAproximado = 50m;
                res.RequiereAlerta = false;
            }
            else
            {
                res.Clasificacion = "Talla Alta";
                res.Detalles = $"Talla ({valorTalla:N2} cm) superior a P85 ({p85:N2} cm).";
                res.PercentilAproximado = 95m;
                res.RequiereAlerta = false;
            }

            return res;
        }
    }
}
