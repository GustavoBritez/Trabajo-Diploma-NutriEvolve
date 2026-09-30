using BE;

namespace BLL
{
    public class EvaluadorNutricionalContext_DNI101
    {
        private IEstrategiaCalculoOMS_DNI101 _estrategia;

        public EvaluadorNutricionalContext_DNI101(int edadMeses)
        {
            if (edadMeses <= 24)
            {
                _estrategia = new CalculoOMS_Lactantes_DNI101();
            }
            else
            {
                _estrategia = new CalculoOMS_PreescolaresEscolares_DNI101();
            }
        }

        public void SetEstrategia(IEstrategiaCalculoOMS_DNI101 estrategia)
        {
            _estrategia = estrategia;
        }

        public ZScoreResultadoBE_DNI101 EjecutarCalculoZScores(MedicionAntropometricaBE_DNI101 medicion, int edadMeses, string sexo)
        {
            return _estrategia.CalcularZScores(medicion, edadMeses, sexo);
        }

        public DiagnosticoNutricionalBE_DNI101 EjecutarEvaluacionDiagnostico(ZScoreResultadoBE_DNI101 zscore)
        {
            return _estrategia.EvaluarClasificacionOMS(zscore);
        }
    }
}
