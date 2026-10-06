namespace BLL.PN2.Strategy
{
    public interface IEvaluadorNutricionalStrategy_DNI101
    {
        string NombreIndicador { get; }
        ResultadoEvaluacionOMS Evaluar(decimal valor, int edadMeses, string sexo);
    }
}
