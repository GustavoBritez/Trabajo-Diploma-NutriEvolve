using System;
using System.Collections.Generic;

namespace BLL.PN2
{
    public class CurvaPercentilesOMS
    {
        public double[] Meses { get; set; } = Array.Empty<double>();
        public double[] P3 { get; set; } = Array.Empty<double>();
        public double[] P15 { get; set; } = Array.Empty<double>();
        public double[] P50 { get; set; } = Array.Empty<double>();
        public double[] P85 { get; set; } = Array.Empty<double>();
        public double[] P97 { get; set; } = Array.Empty<double>();
    }

    /// <summary>
    /// Tablas oficiales de estándares de crecimiento infantil de la Organización Mundial de la Salud (OMS / WHO)
    /// extraídas fielmente de curvas_oms.pdf para percentiles P3, P15, P50, P85 y P97.
    /// </summary>
    public static class CurvasOMSData_DNI101
    {
        // Puntos de edad en meses: 0 a 60 meses
        public static readonly double[] MesesEstandar = new double[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
            14, 16, 18, 20, 22, 24, 28, 32, 36, 40, 44, 48, 52, 56, 60
        };

        #region PESO PARA LA EDAD (0 a 5 años - kg)
        // Niños (Male)
        private static readonly double[] PesoNinosP3  = { 2.5, 3.4, 4.3, 5.0, 5.6, 6.0, 6.4, 6.7, 6.9, 7.1, 7.4, 7.6, 7.7, 8.1, 8.4, 8.8, 9.1, 9.4, 9.7, 10.2, 10.7, 11.3, 11.8, 12.3, 12.7, 13.2, 13.7, 14.1 };
        private static readonly double[] PesoNinosP15 = { 2.9, 3.9, 4.9, 5.7, 6.2, 6.7, 7.1, 7.4, 7.7, 8.0, 8.2, 8.4, 8.6, 9.0, 9.4, 9.8, 10.1, 10.5, 10.8, 11.4, 12.0, 12.7, 13.3, 13.9, 14.4, 15.0, 15.6, 16.0 };
        private static readonly double[] PesoNinosP50 = { 3.3, 4.5, 5.6, 6.4, 7.0, 7.5, 7.9, 8.3, 8.6, 8.9, 9.2, 9.4, 9.6, 10.1, 10.5, 10.9, 11.3, 11.8, 12.2, 12.9, 13.7, 14.3, 15.0, 15.7, 16.3, 17.0, 17.7, 18.3 };
        private static readonly double[] PesoNinosP85 = { 3.9, 5.1, 6.3, 7.2, 7.8, 8.4, 8.8, 9.2, 9.6, 9.9, 10.2, 10.5, 10.8, 11.3, 11.7, 12.2, 12.7, 13.2, 13.6, 14.5, 15.4, 16.2, 17.0, 17.8, 18.6, 19.4, 20.2, 21.0 };
        private static readonly double[] PesoNinosP97 = { 4.4, 5.8, 7.1, 8.0, 8.7, 9.3, 9.8, 10.3, 10.7, 11.0, 11.4, 11.7, 12.0, 12.6, 13.1, 13.7, 14.2, 14.7, 15.3, 16.3, 17.3, 18.3, 19.3, 20.2, 21.2, 22.2, 23.1, 24.1 };

        // Niñas (Female)
        private static readonly double[] PesoNinasP3  = { 2.4, 3.2, 3.9, 4.5, 5.0, 5.4, 5.7, 6.0, 6.3, 6.5, 6.7, 6.9, 7.0, 7.4, 7.7, 8.1, 8.4, 8.7, 9.0, 9.5, 10.1, 10.8, 11.4, 11.9, 12.3, 12.8, 13.4, 13.9 };
        private static readonly double[] PesoNinasP15 = { 2.8, 3.6, 4.5, 5.2, 5.7, 6.1, 6.5, 6.8, 7.0, 7.3, 7.5, 7.7, 7.9, 8.3, 8.7, 9.0, 9.4, 9.8, 10.1, 10.8, 11.5, 12.2, 12.9, 13.5, 14.0, 14.7, 15.3, 15.8 };
        private static readonly double[] PesoNinasP50 = { 3.2, 4.2, 5.1, 5.8, 6.4, 6.9, 7.3, 7.6, 7.9, 8.2, 8.5, 8.7, 8.9, 9.4, 9.8, 10.2, 10.6, 11.1, 11.5, 12.3, 13.1, 13.9, 14.8, 15.5, 16.1, 16.8, 17.6, 18.2 };
        private static readonly double[] PesoNinasP85 = { 3.7, 4.8, 5.8, 6.6, 7.3, 7.8, 8.2, 8.6, 8.9, 9.3, 9.6, 9.9, 10.1, 10.7, 11.1, 11.6, 12.1, 12.6, 13.0, 14.0, 15.0, 16.0, 16.9, 17.8, 18.6, 19.5, 20.4, 21.2 };
        private static readonly double[] PesoNinasP97 = { 4.2, 5.5, 6.6, 7.5, 8.2, 8.8, 9.3, 9.8, 10.2, 10.5, 10.9, 11.2, 11.5, 12.1, 12.6, 13.2, 13.7, 14.3, 14.8, 16.0, 17.1, 18.3, 19.4, 20.4, 21.5, 22.6, 23.6, 24.6 };
        #endregion

        #region TALLA / LONGITUD PARA LA EDAD (0 a 5 años - cm)
        // Niños (Male)
        private static readonly double[] TallaNinosP3  = { 46.1, 50.8, 54.4, 57.3, 59.7, 61.7, 63.3, 64.8, 66.2, 67.5, 68.7, 69.9, 71.0, 73.1, 75.0, 76.9, 78.6, 80.2, 81.7, 84.6, 87.3, 89.9, 92.4, 94.7, 96.9, 99.1, 101.2, 103.3 };
        private static readonly double[] TallaNinosP15 = { 47.9, 52.8, 56.4, 59.4, 61.8, 63.8, 65.5, 67.0, 68.4, 69.7, 71.0, 72.2, 73.4, 75.5, 77.5, 79.4, 81.2, 82.8, 84.4, 87.4, 90.2, 92.9, 95.4, 97.9, 100.2, 102.4, 104.6, 106.8 };
        private static readonly double[] TallaNinosP50 = { 49.9, 54.7, 58.4, 61.4, 63.9, 65.9, 67.6, 69.2, 70.6, 72.0, 73.3, 74.5, 75.7, 78.0, 80.2, 82.3, 84.2, 86.0, 87.8, 91.0, 93.9, 96.7, 99.3, 101.9, 104.3, 106.7, 108.9, 111.1 };
        private static readonly double[] TallaNinosP85 = { 51.8, 56.7, 60.4, 63.5, 66.0, 68.0, 69.8, 71.3, 72.8, 74.2, 75.6, 76.9, 78.1, 80.5, 82.8, 85.0, 87.0, 89.0, 90.8, 94.2, 97.3, 100.2, 103.0, 105.7, 108.2, 110.7, 113.1, 115.4 };
        private static readonly double[] TallaNinosP97 = { 53.7, 58.6, 62.4, 65.5, 68.0, 70.1, 71.9, 73.5, 75.0, 76.5, 77.9, 79.2, 80.5, 83.0, 85.4, 87.7, 89.8, 91.8, 93.8, 97.4, 100.7, 103.8, 106.7, 109.5, 112.2, 114.8, 117.2, 119.6 };

        // Niñas (Female)
        private static readonly double[] TallaNinasP3  = { 45.4, 49.8, 53.0, 55.6, 57.8, 59.6, 61.2, 62.7, 64.0, 65.3, 66.5, 67.7, 68.9, 71.0, 73.0, 74.9, 76.7, 78.4, 80.0, 83.0, 85.9, 88.6, 91.1, 93.5, 95.8, 98.1, 100.2, 102.3 };
        private static readonly double[] TallaNinasP15 = { 47.3, 51.7, 55.0, 57.7, 59.9, 61.8, 63.5, 64.9, 66.4, 67.7, 69.0, 70.2, 71.4, 73.6, 75.7, 77.6, 79.5, 81.3, 83.0, 86.1, 89.0, 91.7, 94.3, 96.8, 99.2, 101.5, 103.7, 105.9 };
        private static readonly double[] TallaNinasP50 = { 49.1, 53.7, 57.1, 59.8, 62.1, 64.0, 65.7, 67.3, 68.7, 70.1, 71.5, 72.8, 74.0, 76.4, 78.6, 80.7, 82.7, 84.6, 86.4, 89.8, 92.8, 95.7, 98.4, 101.0, 103.5, 105.9, 108.2, 110.4 };
        private static readonly double[] TallaNinasP85 = { 51.0, 55.6, 59.1, 61.9, 64.2, 66.2, 68.0, 69.6, 71.1, 72.5, 73.9, 75.3, 76.6, 79.1, 81.4, 83.7, 85.8, 87.8, 89.7, 93.3, 96.5, 99.5, 102.3, 105.1, 107.6, 110.1, 112.5, 114.8 };
        private static readonly double[] TallaNinasP97 = { 52.9, 57.6, 61.1, 64.0, 66.4, 68.5, 70.3, 71.9, 73.5, 74.9, 76.4, 77.8, 79.2, 81.8, 84.2, 86.6, 88.8, 90.9, 92.9, 96.7, 100.1, 103.2, 106.2, 109.0, 111.7, 114.3, 116.8, 119.2 };
        #endregion

        #region IMC PARA LA EDAD (0 a 5 años - kg/m²)
        // Niños (Male)
        private static readonly double[] IMCNinosP3  = { 11.5, 12.9, 14.1, 14.7, 14.9, 14.9, 14.8, 14.7, 14.5, 14.3, 14.2, 14.0, 13.9, 13.7, 13.5, 13.4, 13.3, 13.2, 13.1, 13.0, 12.8, 12.7, 12.7, 12.6, 12.5, 12.5, 12.4, 12.4 };
        private static readonly double[] IMCNinosP15 = { 12.4, 13.9, 15.0, 15.6, 15.8, 15.8, 15.7, 15.6, 15.4, 15.2, 15.0, 14.9, 14.7, 14.5, 14.3, 14.2, 14.1, 14.0, 13.9, 13.7, 13.6, 13.5, 13.4, 13.3, 13.3, 13.2, 13.2, 13.1 };
        private static readonly double[] IMCNinosP50 = { 13.4, 14.9, 16.3, 16.8, 16.9, 16.9, 16.8, 16.6, 16.4, 16.2, 16.0, 15.8, 15.7, 15.4, 15.2, 15.1, 14.9, 14.8, 14.7, 14.5, 14.4, 14.3, 14.2, 14.1, 14.0, 14.0, 13.9, 13.9 };
        private static readonly double[] IMCNinosP85 = { 14.5, 16.1, 17.5, 18.0, 18.1, 18.0, 17.9, 17.7, 17.4, 17.2, 17.0, 16.8, 16.6, 16.4, 16.2, 16.0, 15.9, 15.8, 15.6, 15.5, 15.3, 15.2, 15.1, 15.0, 14.9, 14.9, 14.9, 14.8 };
        private static readonly double[] IMCNinosP97 = { 15.5, 17.2, 18.7, 19.2, 19.3, 19.2, 19.0, 18.7, 18.4, 18.2, 17.9, 17.7, 17.5, 17.2, 17.0, 16.8, 16.6, 16.5, 16.4, 16.2, 16.1, 16.0, 15.9, 15.8, 15.8, 15.7, 15.7, 15.7 };

        // Niñas (Female)
        private static readonly double[] IMCNinasP3  = { 11.4, 12.7, 13.7, 14.2, 14.4, 14.4, 14.3, 14.1, 14.0, 13.8, 13.7, 13.5, 13.4, 13.2, 13.1, 13.0, 12.9, 12.8, 12.7, 12.5, 12.4, 12.3, 12.2, 12.1, 12.0, 12.0, 11.9, 11.9 };
        private static readonly double[] IMCNinasP15 = { 12.3, 13.7, 14.7, 15.2, 15.4, 15.4, 15.2, 15.0, 14.9, 14.7, 14.5, 14.4, 14.2, 14.0, 13.9, 13.8, 13.7, 13.6, 13.5, 13.3, 13.2, 13.1, 13.0, 12.9, 12.8, 12.8, 12.7, 12.7 };
        private static readonly double[] IMCNinasP50 = { 13.3, 14.9, 15.9, 16.4, 16.6, 16.5, 16.4, 16.2, 16.0, 15.8, 15.6, 15.4, 15.3, 15.0, 14.8, 14.7, 14.6, 14.4, 14.3, 14.1, 14.0, 13.9, 13.8, 13.7, 13.6, 13.5, 13.5, 13.4 };
        private static readonly double[] IMCNinasP85 = { 14.4, 16.2, 17.3, 17.8, 17.9, 17.8, 17.6, 17.4, 17.1, 16.9, 16.7, 16.5, 16.3, 16.0, 15.8, 15.6, 15.5, 15.3, 15.2, 15.0, 14.9, 14.8, 14.7, 14.6, 14.6, 14.5, 14.5, 14.4 };
        private static readonly double[] IMCNinasP97 = { 15.5, 17.3, 18.5, 19.0, 19.1, 19.0, 18.8, 18.5, 18.2, 17.9, 17.7, 17.4, 17.2, 16.9, 16.7, 16.5, 16.3, 16.2, 16.0, 15.8, 15.7, 15.6, 15.5, 15.4, 15.4, 15.3, 15.3, 15.3 };
        #endregion

        public static CurvaPercentilesOMS ObtenerCurva(string tipoIndicador, string sexo)
        {
            bool esFemenino = sexo.Trim().Equals("Femenino", StringComparison.OrdinalIgnoreCase)
                           || sexo.Trim().Equals("F", StringComparison.OrdinalIgnoreCase);

            string ind = tipoIndicador.ToUpperInvariant();

            if (ind.Contains("PESO"))
            {
                return new CurvaPercentilesOMS
                {
                    Meses = MesesEstandar,
                    P3  = esFemenino ? PesoNinasP3  : PesoNinosP3,
                    P15 = esFemenino ? PesoNinasP15 : PesoNinosP15,
                    P50 = esFemenino ? PesoNinasP50 : PesoNinosP50,
                    P85 = esFemenino ? PesoNinasP85 : PesoNinosP85,
                    P97 = esFemenino ? PesoNinasP97 : PesoNinosP97,
                };
            }
            else if (ind.Contains("TALLA") || ind.Contains("LONGITUD") || ind.Contains("ESTATURA"))
            {
                return new CurvaPercentilesOMS
                {
                    Meses = MesesEstandar,
                    P3  = esFemenino ? TallaNinasP3  : TallaNinosP3,
                    P15 = esFemenino ? TallaNinasP15 : TallaNinosP15,
                    P50 = esFemenino ? TallaNinasP50 : TallaNinosP50,
                    P85 = esFemenino ? TallaNinasP85 : TallaNinosP85,
                    P97 = esFemenino ? TallaNinasP97 : TallaNinosP97,
                };
            }
            else // Default: IMC
            {
                return new CurvaPercentilesOMS
                {
                    Meses = MesesEstandar,
                    P3  = esFemenino ? IMCNinasP3  : IMCNinosP3,
                    P15 = esFemenino ? IMCNinasP15 : IMCNinosP15,
                    P50 = esFemenino ? IMCNinasP50 : IMCNinosP50,
                    P85 = esFemenino ? IMCNinasP85 : IMCNinosP85,
                    P97 = esFemenino ? IMCNinasP97 : IMCNinosP97,
                };
            }
        }
    }
}
