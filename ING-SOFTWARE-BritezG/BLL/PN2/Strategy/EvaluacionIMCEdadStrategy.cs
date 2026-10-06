using System;

namespace BLL.PN2.Strategy
{
    public class EvaluacionIMCEdadStrategy : IEvaluadorNutricionalStrategy_DNI101
    {
        public string NombreIndicador => "IMC para la Edad";

        public ResultadoEvaluacionOMS Evaluar(decimal valorIMC, int edadMeses, string sexo)
        {
            var curva = CurvasOMSData_DNI101.ObtenerCurva("IMC", sexo);
            
            // Buscar índice más cercano en meses
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

            double val = (double)valorIMC;
            double p3 = curva.P3[idx];
            double p15 = curva.P15[idx];
            double p50 = curva.P50[idx];
            double p85 = curva.P85[idx];
            double p97 = curva.P97[idx];

            ResultadoEvaluacionOMS res = new ResultadoEvaluacionOMS();

            if (val < p3)
            {
                res.Clasificacion = "Desnutrición Aguda / Bajo Peso Severo";
                res.Detalles = $"IMC ({valorIMC:N2}) por debajo del percentil 3 ({p3:N2}). Riesgo de desnutrición aguda.";
                res.PercentilAproximado = 2m;
                res.RequiereAlerta = true;
                res.SeveridadAlerta = "CRÍTICA";
                res.MensajeAlerta = $"🚨 ALERTA ROJA: IMC de {valorIMC:N2} por debajo de P3. Retardo ponderal crítico.";
            }
            else if (val < p15)
            {
                res.Clasificacion = "Riesgo de Bajo Peso";
                res.Detalles = $"IMC ({valorIMC:N2}) en franja P3-P15 ({p3:N2} - {p15:N2}). Monitoreo nutricional cercano.";
                res.PercentilAproximado = 10m;
                res.RequiereAlerta = false;
            }
            else if (val <= p85)
            {
                res.Clasificacion = "Eutrófico (Normal)";
                res.Detalles = $"IMC ({valorIMC:N2}) en rango normopeso P15-P85 ({p15:N2} - {p85:N2}). Crecimiento saludable.";
                res.PercentilAproximado = 50m;
                res.RequiereAlerta = false;
            }
            else if (val <= p97)
            {
                res.Clasificacion = "Sobrepeso";
                res.Detalles = $"IMC ({valorIMC:N2}) en franja P85-P97 ({p85:N2} - {p97:N2}). Riesgo de progresión a obesidad.";
                res.PercentilAproximado = 90m;
                res.RequiereAlerta = false;
            }
            else
            {
                res.Clasificacion = "Obesidad Pediátrica";
                res.Detalles = $"IMC ({valorIMC:N2}) por encima del percentil 97 ({p97:N2}). Obesidad según estándar OMS.";
                res.PercentilAproximado = 98m;
                res.RequiereAlerta = true;
                res.SeveridadAlerta = "MODERADA";
                res.MensajeAlerta = $"⚠️ ALERTA AMARILLA: IMC de {valorIMC:N2} supera el percentil 97 (Obesidad). Requiere ajuste de pauta alimentaria.";
            }

            return res;
        }
    }
}
