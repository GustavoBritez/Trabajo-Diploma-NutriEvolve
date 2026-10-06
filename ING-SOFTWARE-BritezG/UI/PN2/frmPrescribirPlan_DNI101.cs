using System;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BE.PN2;
using BLL.PN2;

namespace UI.PN2
{
    public class frmPrescribirPlan_DNI101 : Form
    {
        private readonly PacienteBE_DNI101 _paciente;
        private readonly PlanAlimentarioBLL_DNI101 _planBLL;
        private readonly ConsultaNutricionalBLL_DNI101 _consultaBLL;
        private ConsultaNutricionalBE_DNI101? _ultimaConsulta;

        private TextBox _txtKcal = null!;
        private TextBox _txtCarbos = null!;
        private TextBox _txtProte = null!;
        private TextBox _txtGrasas = null!;
        private Label _lblTotalPct = null!;
        private Label _lblGramosCarbos = null!;
        private Label _lblGramosProte = null!;
        private Label _lblGramosGrasas = null!;
        private TextBox _txtMetas = null!;
        private TextBox _txtPautas = null!;

        public frmPrescribirPlan_DNI101(PacienteBE_DNI101 paciente)
        {
            _paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
            _planBLL = new PlanAlimentarioBLL_DNI101();
            _consultaBLL = new ConsultaNutricionalBLL_DNI101();

            ConfigurarFormulario();
            ConstruirComponentes();
            CargarContextoClinico();
        }

        private void ConfigurarFormulario()
        {
            Text = $"CUN09: Prescribir Plan Alimentario - {_paciente.NombreCompleto}";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(680, 680);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 248, 245);
        }

        private void ConstruirComponentes()
        {
            // Header
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(123, 31, 162), // Púrpura profesional
                Padding = new Padding(15, 12, 15, 10)
            };

            Label lblTit = new Label
            {
                Text = "📋 PRESCRIBIR PLAN ALIMENTARIO PERSONALIZADO (CUN09)",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 12),
                AutoSize = true
            };

            Label lblSub = new Label
            {
                Text = $"Paciente: {_paciente.NombreCompleto}  |  DNI: {_paciente.DNINiño_DNI101}  |  Sexo: {_paciente.Sexo_DNI101 ?? "N/D"}",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(235, 220, 245),
                Location = new Point(17, 40),
                AutoSize = true
            };
            pnlTop.Controls.Add(lblTit);
            pnlTop.Controls.Add(lblSub);
            Controls.Add(pnlTop);

            // Body
            Panel pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25, 15, 25, 15),
                AutoScroll = true
            };

            int y = 10;

            // Grupo 1: Requerimiento Calórico y Macronutrientes
            GroupBox gbNutri = new GroupBox
            {
                Text = "Metas Energéticas y Distribución de Macronutrientes",
                Location = new Point(0, y),
                Size = new Size(610, 210),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(123, 31, 162)
            };

            Label lblKcal = new Label { Text = "Requerimiento Calórico Total (kcal/día):", Location = new Point(20, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtKcal = new TextBox { Location = new Point(20, 50), Size = new Size(160, 26), Font = new Font("Segoe UI", 11F, FontStyle.Bold), Text = "1400" };
            _txtKcal.TextChanged += (s, e) => RecalcularGramos();

            // Macronutrientes
            Label lblCarb = new Label { Text = "Carbohidratos (%):", Location = new Point(20, 90), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtCarbos = new TextBox { Location = new Point(20, 110), Size = new Size(110, 25), Font = new Font("Segoe UI", 10F), Text = "55" };
            _txtCarbos.TextChanged += (s, e) => RecalcularGramos();
            _lblGramosCarbos = new Label { Text = "= 192.5 g (4 kcal/g)", Location = new Point(20, 140), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(80, 95, 85) };

            Label lblProt = new Label { Text = "Proteínas (%):", Location = new Point(170, 90), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtProte = new TextBox { Location = new Point(170, 110), Size = new Size(110, 25), Font = new Font("Segoe UI", 10F), Text = "15" };
            _txtProte.TextChanged += (s, e) => RecalcularGramos();
            _lblGramosProte = new Label { Text = "= 52.5 g (4 kcal/g)", Location = new Point(170, 140), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(80, 95, 85) };

            Label lblGrasa = new Label { Text = "Lípidos / Grasas (%):", Location = new Point(320, 90), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtGrasas = new TextBox { Location = new Point(320, 110), Size = new Size(110, 25), Font = new Font("Segoe UI", 10F), Text = "30" };
            _txtGrasas.TextChanged += (s, e) => RecalcularGramos();
            _lblGramosGrasas = new Label { Text = "= 46.7 g (9 kcal/g)", Location = new Point(320, 140), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(80, 95, 85) };

            // Panel Suma Total
            Panel pnlTotal = new Panel
            {
                Location = new Point(450, 45),
                Size = new Size(140, 115),
                BackColor = Color.FromArgb(245, 240, 250)
            };
            pnlTotal.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(200, 180, 220), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlTotal.Width - 1, pnlTotal.Height - 1);
            };

            Label lblSumaTit = new Label { Text = "SUMA TOTAL", Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 80, 120) };
            _lblTotalPct = new Label { Text = "100.0%", Location = new Point(5, 35), Size = new Size(130, 32), Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = Color.FromArgb(46, 125, 50), TextAlign = ContentAlignment.MiddleCenter };
            Label lblReq = new Label { Text = "Debe sumar 100%", Location = new Point(5, 75), Size = new Size(130, 30), Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(110, 95, 125), TextAlign = ContentAlignment.TopCenter };

            pnlTotal.Controls.Add(lblSumaTit);
            pnlTotal.Controls.Add(_lblTotalPct);
            pnlTotal.Controls.Add(lblReq);

            gbNutri.Controls.Add(lblKcal);
            gbNutri.Controls.Add(_txtKcal);
            gbNutri.Controls.Add(lblCarb);
            gbNutri.Controls.Add(_txtCarbos);
            gbNutri.Controls.Add(_lblGramosCarbos);
            gbNutri.Controls.Add(lblProt);
            gbNutri.Controls.Add(_txtProte);
            gbNutri.Controls.Add(_lblGramosProte);
            gbNutri.Controls.Add(lblGrasa);
            gbNutri.Controls.Add(_txtGrasas);
            gbNutri.Controls.Add(_lblGramosGrasas);
            gbNutri.Controls.Add(pnlTotal);
            pnlBody.Controls.Add(gbNutri);

            y += 225;

            // Grupo 2: Pautas y Metas
            GroupBox gbPautas = new GroupBox
            {
                Text = "Objetivos y Recomendaciones Dietarias",
                Location = new Point(0, y),
                Size = new Size(610, 240),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 75, 60)
            };

            Label lblMetas = new Label { Text = "Metas del Tratamiento:", Location = new Point(15, 25), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtMetas = new TextBox { Location = new Point(15, 45), Size = new Size(580, 25), Font = new Font("Segoe UI", 9F), Text = "Mantener carril percentilar normopeso y fomentar hidratación." };

            Label lblPautas = new Label { Text = "Pautas Nutricionales y Familiares (Alimentos sugeridos y frecuencias):", Location = new Point(15, 80), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtPautas = new TextBox { Location = new Point(15, 100), Size = new Size(580, 120), Multiline = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Segoe UI", 9F), Text = "• Priorizar alimentos naturales, frutas y verduras variadas.\n• Evitar ultraprocesados, bebidas azucaradas y harinas refinadas.\n• Fomentar 60 minutos diarios de juego activo o actividad física." };

            gbPautas.Controls.Add(lblMetas);
            gbPautas.Controls.Add(_txtMetas);
            gbPautas.Controls.Add(lblPautas);
            gbPautas.Controls.Add(_txtPautas);
            pnlBody.Controls.Add(gbPautas);

            y += 255;

            // Botones inferiores
            Button btnGuardar = new Button
            {
                Text = "💾  Prescribir y Guardar Plan",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(123, 31, 162),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(260, 42),
                Location = new Point(180, y),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(80, 95, 85),
                BackColor = Color.FromArgb(235, 238, 235),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 42),
                Location = new Point(460, y),
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(200, 210, 200);
            btnCancelar.Click += (s, e) => Close();

            pnlBody.Controls.Add(btnGuardar);
            pnlBody.Controls.Add(btnCancelar);

            Controls.Add(pnlBody);
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
            }
            else
            {
                _lblTotalPct.ForeColor = Color.FromArgb(198, 40, 40); // Rojo alerta
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
    }
}
