using System;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BE.PN2;
using BLL.PN2;

namespace UI.PN2
{
    public partial class frmPrescribirPlan_DNI101 : Form
    {
        private readonly PacienteBE_DNI101 _paciente;
        private readonly PlanAlimentarioBLL_DNI101 _planBLL;
        private readonly ConsultaNutricionalBLL_DNI101 _consultaBLL;
        private ConsultaNutricionalBE_DNI101? _ultimaConsulta;

        public frmPrescribirPlan_DNI101(PacienteBE_DNI101 paciente)
        {
            _paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
            _planBLL = new PlanAlimentarioBLL_DNI101();
            _consultaBLL = new ConsultaNutricionalBLL_DNI101();

            InitializeComponent();
            ConfigurarTextosPaciente();
            CargarContextoClinico();
        }

        private void ConfigurarTextosPaciente()
        {
            Text = $"CUN09: Prescribir Plan Alimentario - {_paciente.NombreCompleto}";

            int meses = _paciente.ObtenerEdadMeses();
            string edadStr = meses < 24 ? $"{meses} meses" : $"{meses / 12} años ({meses}m)";
            lblSub.Text = $"Paciente: {_paciente.NombreCompleto}  |  DNI: {_paciente.DNINiño_DNI101}  |  Sexo: {_paciente.Sexo_DNI101 ?? "Masculino"}  |  Edad: {edadStr}";
        }

        private void CargarContextoClinico()
        {
            try
            {
                _ultimaConsulta = _consultaBLL.ObtenerUltimaConsultaPorPaciente(_paciente.IdPaciente_DNI101);
                if (_ultimaConsulta != null)
                {
                    // Si existe un plan previo para esta consulta o paciente, cargarlo
                    var planPrevio = _planBLL.ObtenerUltimoPlanPorPaciente(_paciente.IdPaciente_DNI101);
                    if (planPrevio != null)
                    {
                        _txtKcal.Text = planPrevio.RequerimientoCalorico_DNI101.ToString("N0");
                        _txtCarbos.Text = planPrevio.PctCarbohidratos_DNI101.ToString("N0");
                        _txtProte.Text = planPrevio.PctProteinas_DNI101.ToString("N0");
                        _txtGrasas.Text = planPrevio.PctGrasas_DNI101.ToString("N0");
                        _txtMetas.Text = planPrevio.MetasSalud_DNI101;
                        _txtPautas.Text = planPrevio.PautasFamiliares_DNI101;
                    }
                }
                RecalcularGramos();
            }
            catch { }
        }

        private void _txtKcal_TextChanged(object? sender, EventArgs e)
        {
            RecalcularGramos();
        }

        private void _txtNutri_TextChanged(object? sender, EventArgs e)
        {
            RecalcularGramos();
        }

        private void RecalcularGramos()
        {
            decimal.TryParse(_txtKcal.Text.Trim(), out decimal kcal);
            decimal.TryParse(_txtCarbos.Text.Trim(), out decimal carbos);
            decimal.TryParse(_txtProte.Text.Trim(), out decimal prote);
            decimal.TryParse(_txtGrasas.Text.Trim(), out decimal grasas);

            decimal total = carbos + prote + grasas;
            _lblTotalPct.Text = $"{total:N1}%";

            if (Math.Round(total, 1) == 100m)
            {
                _lblTotalPct.ForeColor = Color.FromArgb(46, 125, 50); // Verde
                _lblTotalStatus.Text = "✓ Distribución válida (100%)";
                _lblTotalStatus.ForeColor = Color.FromArgb(46, 125, 50);
            }
            else
            {
                _lblTotalPct.ForeColor = Color.FromArgb(198, 40, 40); // Rojo alerta
                _lblTotalStatus.Text = "⚠️ Debe sumar 100.0%";
                _lblTotalStatus.ForeColor = Color.FromArgb(198, 40, 40);
            }

            if (kcal > 0)
            {
                decimal gC = (kcal * (carbos / 100m)) / 4m;
                decimal gP = (kcal * (prote / 100m)) / 4m;
                decimal gG = (kcal * (grasas / 100m)) / 9m;

                _lblGramosCarbos.Text = $"= {gC:N1} g (4 kcal/g)";
                _lblGramosProte.Text = $"= {gP:N1} g (4 kcal/g)";
                _lblGramosGrasas.Text = $"= {gG:N1} g (9 kcal/g)";
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_ultimaConsulta == null)
                {
                    MessageBox.Show("El paciente debe tener al menos una consulta médica registrada para prescribir un plan alimentario.", "Atención Clínica", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(_txtKcal.Text.Trim(), out decimal kcal) || kcal <= 0)
                {
                    MessageBox.Show("Ingrese un requerimiento calórico válido.", "Dato Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtKcal.Focus();
                    return;
                }

                decimal.TryParse(_txtCarbos.Text.Trim(), out decimal carbos);
                decimal.TryParse(_txtProte.Text.Trim(), out decimal prote);
                decimal.TryParse(_txtGrasas.Text.Trim(), out decimal grasas);

                if (Math.Round(carbos + prote + grasas, 1) != 100m)
                {
                    MessageBox.Show("La suma de los porcentajes de macronutrientes debe ser exactamente 100%.", "Suma Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cursor = Cursors.WaitCursor;

                // ÚNICO HIT A BASE DE DATOS
                int idPlan = _planBLL.PrescribirPlan(
                    idConsulta: _ultimaConsulta.IdConsulta_DNI101,
                    requerimientoCalorico: kcal,
                    pctCarbohidratos: carbos,
                    pctProteinas: prote,
                    pctGrasas: grasas,
                    pautasFamiliares: _txtPautas.Text.Trim(),
                    metasSalud: _txtMetas.Text.Trim()
                );

                MessageBox.Show(
                    $"¡Plan Alimentario #{idPlan} prescrito exitosamente!\n\nCalorías: {kcal:N0} kcal\nCarbohidratos: {carbos}%\nProteínas: {prote}%\nGrasas: {grasas}%",
                    "Plan Prescrito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al prescribir el plan: {ex.Message}", "Error Clínico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
