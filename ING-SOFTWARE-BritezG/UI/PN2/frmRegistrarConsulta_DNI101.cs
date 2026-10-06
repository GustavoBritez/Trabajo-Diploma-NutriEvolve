using System;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BLL.PN2;
using BLL.PN2.Strategy;

namespace UI.PN2
{
    public class frmRegistrarConsulta_DNI101 : Form
    {
        private readonly PacienteBE_DNI101 _paciente;
        private readonly ConsultaNutricionalBLL_DNI101 _consultaBLL;

        private DateTimePicker _dtpFecha = null!;
        private TextBox _txtPeso = null!;
        private TextBox _txtTalla = null!;
        private TextBox _txtPerimetro = null!;
        private Label _lblIMCValor = null!;
        private Label _lblCategoriaPreview = null!;
        private ComboBox _cmbLactancia = null!;
        private TextBox _txtAlimentacionComp = null!;
        private TextBox _txtAlergias = null!;
        private TextBox _txtAntFam = null!;
        private TextBox _txtObservaciones = null!;

        public frmRegistrarConsulta_DNI101(PacienteBE_DNI101 paciente)
        {
            _paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
            _consultaBLL = new ConsultaNutricionalBLL_DNI101();

            ConfigurarFormulario();
            ConstruirComponentes();
        }

        private void ConfigurarFormulario()
        {
            Text = $"CUN08: Registrar Consulta Nutricional - {_paciente.NombreCompleto}";
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
                BackColor = Color.FromArgb(76, 124, 89),
                Padding = new Padding(15, 12, 15, 10)
            };

            Label lblTit = new Label
            {
                Text = "📝 NUEVA CONSULTA NUTRICIONAL (CUN08)",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 12),
                AutoSize = true
            };

            int meses = _paciente.ObtenerEdadMeses();
            string edadStr = meses < 24 ? $"{meses} meses" : $"{meses / 12} años ({meses}m)";
            Label lblSub = new Label
            {
                Text = $"Paciente: {_paciente.NombreCompleto}  |  DNI: {_paciente.DNINiño_DNI101}  |  Sexo: {_paciente.Sexo_DNI101 ?? "N/D"}  |  Edad: {edadStr}",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 240, 225),
                Location = new Point(17, 40),
                AutoSize = true
            };
            pnlTop.Controls.Add(lblTit);
            pnlTop.Controls.Add(lblSub);
            Controls.Add(pnlTop);

            // Panel Contenedor
            Panel pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25, 15, 25, 15),
                AutoScroll = true
            };

            int y = 10;

            // Fecha
            Label lblFecha = new Label { Text = "Fecha del Control Clínico:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _dtpFecha = new DateTimePicker { Location = new Point(0, y + 22), Size = new Size(220, 26), Format = DateTimePickerFormat.Short, Value = DateTime.Today };
            pnlBody.Controls.Add(lblFecha);
            pnlBody.Controls.Add(_dtpFecha);

            y += 60;

            // Grupo 1: Mediciones Antropométricas
            GroupBox gbMed = new GroupBox
            {
                Text = "Mediciones Antropométricas",
                Location = new Point(0, y),
                Size = new Size(610, 150),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 125, 50)
            };

            Label lblPeso = new Label { Text = "Peso (kg):", Location = new Point(15, 25), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtPeso = new TextBox { Location = new Point(15, 45), Size = new Size(110, 25), Font = new Font("Segoe UI", 10F) };
            _txtPeso.TextChanged += (s, e) => RecalcularIMC();

            Label lblTalla = new Label { Text = "Talla/Long. (cm):", Location = new Point(145, 25), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtTalla = new TextBox { Location = new Point(145, 45), Size = new Size(110, 25), Font = new Font("Segoe UI", 10F) };
            _txtTalla.TextChanged += (s, e) => RecalcularIMC();

            Label lblPerim = new Label { Text = "Perím. Cefálico (cm):", Location = new Point(275, 25), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtPerimetro = new TextBox { Location = new Point(275, 45), Size = new Size(110, 25), Font = new Font("Segoe UI", 10F), Text = "0" };

            // Panel IMC Calculado
            Panel pnlIMC = new Panel
            {
                Location = new Point(410, 20),
                Size = new Size(185, 115),
                BackColor = Color.FromArgb(240, 246, 242)
            };
            pnlIMC.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(180, 210, 190), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlIMC.Width - 1, pnlIMC.Height - 1);
            };

            Label lblTitIMC = new Label { Text = "IMC CALCULADO", Location = new Point(10, 8), AutoSize = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(70, 90, 75) };
            _lblIMCValor = new Label { Text = "0.00", Location = new Point(10, 28), Size = new Size(165, 32), Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(46, 125, 50), TextAlign = ContentAlignment.MiddleCenter };
            _lblCategoriaPreview = new Label { Text = "Ingrese valores", Location = new Point(5, 68), Size = new Size(175, 40), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(90, 105, 95), TextAlign = ContentAlignment.TopCenter };

            pnlIMC.Controls.Add(lblTitIMC);
            pnlIMC.Controls.Add(_lblIMCValor);
            pnlIMC.Controls.Add(_lblCategoriaPreview);

            gbMed.Controls.Add(lblPeso);
            gbMed.Controls.Add(_txtPeso);
            gbMed.Controls.Add(lblTalla);
            gbMed.Controls.Add(_txtTalla);
            gbMed.Controls.Add(lblPerim);
            gbMed.Controls.Add(_txtPerimetro);
            gbMed.Controls.Add(pnlIMC);
            pnlBody.Controls.Add(gbMed);

            y += 165;

            // Grupo 2: Antecedentes Clínicos y Dietarios
            GroupBox gbClinico = new GroupBox
            {
                Text = "Evaluación Clínica y Hábitos Alimentarios",
                Location = new Point(0, y),
                Size = new Size(610, 260),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 101, 192)
            };

            Label lblLact = new Label { Text = "Lactancia:", Location = new Point(15, 25), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _cmbLactancia = new ComboBox { Location = new Point(15, 45), Size = new Size(180, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            _cmbLactancia.Items.AddRange(new object[] { "Materna Exclusiva", "Predominante / Parcial", "Fórmula Infantil", "Destetado / Sin lactancia" });
            _cmbLactancia.SelectedIndex = 0;

            Label lblAlim = new Label { Text = "Alimentación Complementaria:", Location = new Point(215, 25), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtAlimentacionComp = new TextBox { Location = new Point(215, 45), Size = new Size(380, 25), Font = new Font("Segoe UI", 9F) };

            Label lblAlerg = new Label { Text = "Alergias / Intolerancias:", Location = new Point(15, 80), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtAlergias = new TextBox { Location = new Point(15, 100), Size = new Size(270, 25), Font = new Font("Segoe UI", 9F) };

            Label lblAnt = new Label { Text = "Antecedentes Familiares:", Location = new Point(305, 80), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtAntFam = new TextBox { Location = new Point(305, 100), Size = new Size(290, 25), Font = new Font("Segoe UI", 9F) };

            Label lblObs = new Label { Text = "Observaciones Médicas y Evolución:", Location = new Point(15, 135), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtObservaciones = new TextBox { Location = new Point(15, 155), Size = new Size(580, 85), Multiline = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Segoe UI", 9F) };

            gbClinico.Controls.Add(lblLact);
            gbClinico.Controls.Add(_cmbLactancia);
            gbClinico.Controls.Add(lblAlim);
            gbClinico.Controls.Add(_txtAlimentacionComp);
            gbClinico.Controls.Add(lblAlerg);
            gbClinico.Controls.Add(_txtAlergias);
            gbClinico.Controls.Add(lblAnt);
            gbClinico.Controls.Add(_txtAntFam);
            gbClinico.Controls.Add(lblObs);
            gbClinico.Controls.Add(_txtObservaciones);
            pnlBody.Controls.Add(gbClinico);

            y += 275;

            // Botones de acción al pie
            Button btnGuardar = new Button
            {
                Text = "💾  Guardar Consulta Nutricional",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 125, 50),
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

        private void RecalcularIMC()
        {
            if (decimal.TryParse(_txtPeso.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal peso) &&
                decimal.TryParse(_txtTalla.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal talla) &&
                peso > 0 && talla > 0)
            {
                decimal imc = _consultaBLL.CalcularIMC(peso, talla);
                _lblIMCValor.Text = imc.ToString("N2");

                int edadMeses = _paciente.ObtenerEdadMeses();
                string sexo = _paciente.Sexo_DNI101 ?? "Masculino";
                ResultadoEvaluacionOMS eval = _consultaBLL.EvaluarEstadoNutricional(imc, edadMeses, sexo);

                _lblCategoriaPreview.Text = eval.Clasificacion;

                if (eval.RequiereAlerta)
                {
                    _lblCategoriaPreview.ForeColor = Color.FromArgb(198, 40, 40);
                    _lblIMCValor.ForeColor = Color.FromArgb(198, 40, 40);
                }
                else if (eval.Clasificacion.Contains("Sobrepeso"))
                {
                    _lblCategoriaPreview.ForeColor = Color.FromArgb(245, 124, 0);
                    _lblIMCValor.ForeColor = Color.FromArgb(245, 124, 0);
                }
                else
                {
                    _lblCategoriaPreview.ForeColor = Color.FromArgb(46, 125, 50);
                    _lblIMCValor.ForeColor = Color.FromArgb(46, 125, 50);
                }
            }
            else
            {
                _lblIMCValor.Text = "0.00";
                _lblIMCValor.ForeColor = Color.FromArgb(90, 105, 95);
                _lblCategoriaPreview.Text = "Ingrese peso y talla";
                _lblCategoriaPreview.ForeColor = Color.FromArgb(90, 105, 95);
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(_txtPeso.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal peso) || peso <= 0)
                {
                    MessageBox.Show("Por favor, ingrese un peso válido en kilogramos.", "Dato Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtPeso.Focus();
                    return;
                }

                if (!decimal.TryParse(_txtTalla.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal talla) || talla <= 20)
                {
                    MessageBox.Show("Por favor, ingrese una talla/longitud válida en centímetros.", "Dato Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtTalla.Focus();
                    return;
                }

                decimal.TryParse(_txtPerimetro.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal perimetro);

                Cursor = Cursors.WaitCursor;

                int edadMeses = _paciente.ObtenerEdadMeses();
                string sexo = _paciente.Sexo_DNI101 ?? "Masculino";

                // ÚNICO HIT A BASE DE DATOS
                int idConsulta = _consultaBLL.RegistrarConsulta(
                    idPaciente: _paciente.IdPaciente_DNI101,
                    edadMeses: edadMeses,
                    sexo: sexo,
                    pesoKg: peso,
                    tallaCm: talla,
                    perimetroCefalico: perimetro,
                    fechaControl: _dtpFecha.Value,
                    tipoLactancia: _cmbLactancia.SelectedItem?.ToString(),
                    alimentacionComp: _txtAlimentacionComp.Text.Trim(),
                    alergias: _txtAlergias.Text.Trim(),
                    antFamiliares: _txtAntFam.Text.Trim(),
                    observaciones: _txtObservaciones.Text.Trim()
                );

                MessageBox.Show(
                    $"¡Consulta #{idConsulta} registrada con éxito!\n\nPeso: {peso:N2} kg | Talla: {talla:N2} cm | IMC: {_lblIMCValor.Text}\nDiagnóstico: {_lblCategoriaPreview.Text}",
                    "Consulta Nutricional Registrada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la consulta nutricional: {ex.Message}", "Error Clínico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
