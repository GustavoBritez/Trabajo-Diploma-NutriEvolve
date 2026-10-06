using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.PN2
{
    public class PlanAlimentarioDAL_DNI101
    {
        private readonly Conexion _conexion;

        public PlanAlimentarioDAL_DNI101()
        {
            _conexion = new Conexion();
        }

        /// <summary>
        /// Guarda el plan alimentario en una única llamada a base de datos para el CUN09.
        /// Totalmente desacoplada de clases BE.
        /// </summary>
        public int GuardarPlanAlimentario(
            int idConsulta,
            decimal requerimientoCalorico,
            decimal pctCarbohidratos,
            decimal pctProteinas,
            decimal pctGrasas,
            string pautasFamiliares,
            string metasSalud,
            string? dv)
        {
            string sql = @"
                INSERT INTO PlanesAlimentarios_DNI101
                (IdConsulta_DNI101, RequerimientoCalorico_DNI101, PctCarbohidratos_DNI101,
                 PctProteinas_DNI101, PctGrasas_DNI101, PautasFamiliares_DNI101, MetasSalud_DNI101, DV)
                VALUES
                (@IdConsulta, @Kcal, @Carbos, @Prote, @Grasas, @Pautas, @Metas, @DV);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using (SqlConnection cn = _conexion.CrearConexion())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmd.Parameters.AddWithValue("@Kcal", requerimientoCalorico);
                    cmd.Parameters.AddWithValue("@Carbos", pctCarbohidratos);
                    cmd.Parameters.AddWithValue("@Prote", pctProteinas);
                    cmd.Parameters.AddWithValue("@Grasas", pctGrasas);
                    cmd.Parameters.AddWithValue("@Pautas", (object?)pautasFamiliares ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Metas", (object?)metasSalud ?? string.Empty);
                    cmd.Parameters.AddWithValue("@DV", (object?)dv ?? DBNull.Value);

                    cn.Open();
                    object res = cmd.ExecuteScalar()!;
                    return (int)res;
                }
            }
        }

        public DataTable ObtenerPlanPorConsulta(int idConsulta)
        {
            string sql = @"
                SELECT TOP 1
                    IdPlan_DNI101,
                    IdConsulta_DNI101,
                    RequerimientoCalorico_DNI101,
                    PctCarbohidratos_DNI101,
                    PctProteinas_DNI101,
                    PctGrasas_DNI101,
                    PautasFamiliares_DNI101,
                    MetasSalud_DNI101,
                    DV
                FROM PlanesAlimentarios_DNI101
                WHERE IdConsulta_DNI101 = @IdConsulta;";

            DataTable dt = new DataTable();
            using (SqlConnection cn = _conexion.CrearConexion())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable ObtenerUltimoPlanPorPaciente(int idPaciente)
        {
            string sql = @"
                SELECT TOP 1
                    p.IdPlan_DNI101,
                    p.IdConsulta_DNI101,
                    p.RequerimientoCalorico_DNI101,
                    p.PctCarbohidratos_DNI101,
                    p.PctProteinas_DNI101,
                    p.PctGrasas_DNI101,
                    p.PautasFamiliares_DNI101,
                    p.MetasSalud_DNI101,
                    p.DV,
                    c.FechaControl_DNI101
                FROM PlanesAlimentarios_DNI101 p
                INNER JOIN ConsultasNutricionales_DNI101 c ON p.IdConsulta_DNI101 = c.IdConsulta_DNI101
                WHERE c.IdPaciente_DNI101 = @IdPaciente
                ORDER BY c.FechaControl_DNI101 DESC;";

            DataTable dt = new DataTable();
            using (SqlConnection cn = _conexion.CrearConexion())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
