using System;
using System.Data;
using BE.PN2;
using DAL.PN2;
using Services;

namespace BLL.PN2
{
    public class PlanAlimentarioBLL_DNI101
    {
        private readonly PlanAlimentarioDAL_DNI101 _dal;
        private readonly BitacoraBLL _bitacoraBLL;
        private readonly DigitoVerificadorBLL _dvBLL;

        public PlanAlimentarioBLL_DNI101()
        {
            _dal = new PlanAlimentarioDAL_DNI101();
            _bitacoraBLL = new BitacoraBLL();
            _dvBLL = new DigitoVerificadorBLL();
        }

        /// <summary>
        /// Prescribe y persiste un plan alimentario estructurado (CUN09) en una sola llamada a base de datos.
        /// </summary>
        public int PrescribirPlan(
            int idConsulta,
            decimal requerimientoCalorico,
            decimal pctCarbohidratos,
            decimal pctProteinas,
            decimal pctGrasas,
            string pautasFamiliares,
            string metasSalud)
        {
            if (idConsulta <= 0)
                throw new ArgumentException("Debe asociar el plan a una consulta clínica válida.");

            if (requerimientoCalorico <= 0 || requerimientoCalorico > 8000)
                throw new ArgumentException("El requerimiento calórico debe ser un valor válido (mayor a 0 y menor a 8000 kcal).");

            decimal totalPct = pctCarbohidratos + pctProteinas + pctGrasas;
            if (Math.Round(totalPct, 1) != 100m)
                throw new ArgumentException($"La suma de macronutrientes debe ser exactamente 100% (Suma actual: {totalPct:N1}%).");

            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            if (dniActual == 0) dniActual = 11111111;

            string dvDummy = $"{idConsulta}-{requerimientoCalorico}-{totalPct}";

            // 1 hit to DB
            int idPlan = _dal.GuardarPlanAlimentario(
                idConsulta: idConsulta,
                requerimientoCalorico: requerimientoCalorico,
                pctCarbohidratos: pctCarbohidratos,
                pctProteinas: pctProteinas,
                pctGrasas: pctGrasas,
                pautasFamiliares: pautasFamiliares,
                metasSalud: metasSalud,
                dv: dvDummy
            );

            // Transversales
            try
            {
                _bitacoraBLL.RegistrarBitacora(2, $"CUN09: Plan Alimentario #{idPlan} prescrito para consulta #{idConsulta}. Calorías: {requerimientoCalorico} kcal ({pctCarbohidratos}% C, {pctProteinas}% P, {pctGrasas}% G)", dniActual, "SeguimientoNutricional");
                _dvBLL.RecalcularYPersistir();
            }
            catch { }

            return idPlan;
        }

        public PlanAlimentarioBE_DNI101? ObtenerPlanPorConsulta(int idConsulta)
        {
            DataTable dt = _dal.ObtenerPlanPorConsulta(idConsulta);
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new PlanAlimentarioBE_DNI101
            {
                IdPlan_DNI101 = Convert.ToInt32(row["IdPlan_DNI101"]),
                IdConsulta_DNI101 = Convert.ToInt32(row["IdConsulta_DNI101"]),
                RequerimientoCalorico_DNI101 = Convert.ToDecimal(row["RequerimientoCalorico_DNI101"]),
                PctCarbohidratos_DNI101 = Convert.ToDecimal(row["PctCarbohidratos_DNI101"]),
                PctProteinas_DNI101 = Convert.ToDecimal(row["PctProteinas_DNI101"]),
                PctGrasas_DNI101 = Convert.ToDecimal(row["PctGrasas_DNI101"]),
                PautasFamiliares_DNI101 = row["PautasFamiliares_DNI101"]?.ToString() ?? "",
                MetasSalud_DNI101 = row["MetasSalud_DNI101"]?.ToString() ?? "",
                DV = row["DV"]?.ToString()
            };
        }

        public PlanAlimentarioBE_DNI101? ObtenerUltimoPlanPorPaciente(int idPaciente)
        {
            DataTable dt = _dal.ObtenerUltimoPlanPorPaciente(idPaciente);
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new PlanAlimentarioBE_DNI101
            {
                IdPlan_DNI101 = Convert.ToInt32(row["IdPlan_DNI101"]),
                IdConsulta_DNI101 = Convert.ToInt32(row["IdConsulta_DNI101"]),
                RequerimientoCalorico_DNI101 = Convert.ToDecimal(row["RequerimientoCalorico_DNI101"]),
                PctCarbohidratos_DNI101 = Convert.ToDecimal(row["PctCarbohidratos_DNI101"]),
                PctProteinas_DNI101 = Convert.ToDecimal(row["PctProteinas_DNI101"]),
                PctGrasas_DNI101 = Convert.ToDecimal(row["PctGrasas_DNI101"]),
                PautasFamiliares_DNI101 = row["PautasFamiliares_DNI101"]?.ToString() ?? "",
                MetasSalud_DNI101 = row["MetasSalud_DNI101"]?.ToString() ?? "",
                DV = row["DV"]?.ToString()
            };
        }
    }
}
