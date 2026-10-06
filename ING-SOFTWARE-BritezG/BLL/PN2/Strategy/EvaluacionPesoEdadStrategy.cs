using System;

namespace BLL.PN2.Strategy
{
    public class EvaluacionPesoEdadStrategy : IEvaluadorNutricionalStrategy_DNI101
    {
        public string NombreIndicador => "Peso para la Edad";

        public ResultadoEvaluacionOMS Evaluar(decimal valorPeso, int edadMeses, string sexo)
        {
            var curva = CurvasOMSData_DNI101.ObtenerCurva("PESO", sexo);
            
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

            double val = (double)valorPeso;
            double p3 = curva.P3[idx];
            double p15 = curva.P15[idx];
            double p50 = curva.P50[idx];
            double p85 = curva.P85[idx];
            double p97 = curva.P97[idx];

            ResultadoEvaluacionOMS res = new ResultadoEvaluacionOMS();

            if (val < p3)
            {
                res.Clasificacion = "Bajo Peso Severo";
                res.Detalles = $"Peso ({valorPeso:N2} kg) < P3 ({p3:N2} kg). Déficit ponderal marcado.";
                res.PercentilAproximado = 2m;
                res.RequiereAlerta = true;
                res.SeveridadAlerta = "CRÍTICA";
                res.MensajeAlerta = $"🚨 ALERTA ROJA: Peso de {valorPeso:N2} kg debajo de P3.";
            }
            else if (val < p15)
            {
                res.Clasificacion = "Bajo Peso Moderado";
                res.Detalles = $"Peso ({valorPeso:N2} kg) entre P3 y P15 ({p3:N2} - {p15:N2} kg).";
                res.PercentilAproximado = 10m;
                res.RequiereAlerta = false;
            }
            else if (val <= p85)
            {
                res.Clasificacion = "Peso Adecuado (Eutrófico)";
                res.Detalles = $"Peso ({valorPeso:N2} kg) en rango P15-P85 ({p15:N2} - {p85:N2} kg).";
                res.PercentilAproximado = 50m;
                res.RequiereAlerta = false;
            }
            else
            {
                res.Clasificacion = "Peso Elevado";
                res.Detalles = $"Peso ({valorPeso:N2} kg) superior a P85 ({p85:N2} kg).";
                res.PercentilAproximado = 95m;
                res.RequiereAlerta = false;
            }

            return res;
        }
    }
}
