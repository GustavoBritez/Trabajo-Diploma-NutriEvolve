using System;
using System.Collections.Generic;

namespace BLL.PN2.Strategy
{
    public class EvaluadorNutricionalContext_DNI101
    {
        private IEvaluadorNutricionalStrategy_DNI101 _estrategia;

        public EvaluadorNutricionalContext_DNI101(IEvaluadorNutricionalStrategy_DNI101 estrategia)
        {
            _estrategia = estrategia;
        }

        public void SetEstrategia(IEvaluadorNutricionalStrategy_DNI101 estrategia)
        {
            _estrategia = estrategia;
        }

        public ResultadoEvaluacionOMS EjecutarEvaluacion(decimal valor, int edadMeses, string sexo)
        {
            return _estrategia.Evaluar(valor, edadMeses, sexo);
        }
    }
}
