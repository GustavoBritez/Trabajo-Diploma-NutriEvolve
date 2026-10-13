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
            double p5 = curva.P5[idx];
            double p85 = curva.P85[idx];
            double p95 = curva.P95[idx];

            ResultadoEvaluacionOMS res = new ResultadoEvaluacionOMS();

            // Reglas oficiales CDC / OMS por percentiles:
            // 1. Bajo peso: Menor al percentil 5 (<5)
            // 2. Peso saludable (normal): Del percentil 5 al menos del 85 (5 a <85)
            // 3. Sobrepeso: Del percentil 85 al menos del 95 (85 a <95)
            // 4. Obesidad: Percentil 95 o mayor (≥ 95)
            if (val < p5)
            {
                res.Clasificacion = "Bajo peso";
                res.Detalles = $"IMC ({valorIMC:N2}) menor al percentil 5 (<P5: {p5:N2}). Diagnóstico: Bajo peso (<5).";
                res.PercentilAproximado = 3m;
                res.RequiereAlerta = true;
                res.SeveridadAlerta = "MODERADA";
                res.MensajeAlerta = $"⚠️ ALERTA: IMC de {valorIMC:N2} inferior al percentil 5 (<P5: {p5:N2}). Condición de Bajo peso.";
            }
            else if (val < p85)
            {
                res.Clasificacion = "Peso saludable (normal)";
                res.Detalles = $"IMC ({valorIMC:N2}) del percentil 5 al menos del 85 (P5: {p5:N2} a <P85: {p85:N2}). Condición: Peso saludable (normal).";
                res.PercentilAproximado = 50m;
                res.RequiereAlerta = false;
            }
            else if (val < p95)
            {
                res.Clasificacion = "Sobrepeso";
                res.Detalles = $"IMC ({valorIMC:N2}) del percentil 85 al menos del 95 (P85: {p85:N2} a <P95: {p95:N2}). Condición: Sobrepeso.";
                res.PercentilAproximado = 90m;
                res.RequiereAlerta = false;
            }
            else
            {
                res.Clasificacion = "Obesidad";
                res.Detalles = $"IMC ({valorIMC:N2}) en percentil 95 o mayor (≥P95: {p95:N2}). Condición: Obesidad (≥95).";
                res.PercentilAproximado = 97m;
                res.RequiereAlerta = true;
                res.SeveridadAlerta = "CRÍTICA";
                res.MensajeAlerta = $"🚨 ALERTA ROJA: IMC de {valorIMC:N2} en percentil 95 o mayor (≥P95: {p95:N2}). Diagnóstico: Obesidad.";
            }

            return res;
        }
    }
}
