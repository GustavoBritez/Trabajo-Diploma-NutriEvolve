using BE;

namespace BLL
{
    public interface IEstrategiaCalculoOMS_DNI101
    {
        ZScoreResultadoBE_DNI101 CalcularZScores(MedicionAntropometricaBE_DNI101 medicion, int edadMeses, string sexo);
        DiagnosticoNutricionalBE_DNI101 EvaluarClasificacionOMS(ZScoreResultadoBE_DNI101 zscore);
    }
}
