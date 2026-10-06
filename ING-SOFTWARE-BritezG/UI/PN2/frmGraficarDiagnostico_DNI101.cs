using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;
using BE;
using BE.PN2;
using BLL.PN2;
using ScottPlot.WinForms;
using SPColor = ScottPlot.Color;
using SPAlignment = ScottPlot.Alignment;

namespace UI.PN2
{
    public class frmGraficarDiagnostico_DNI101 : Form
    {
        private readonly PacienteBE_DNI101 _paciente;
        private readonly ConsultaNutricionalBLL_DNI101 _consultaBLL;
        private List<ConsultaNutricionalBE_DNI101> _historialConsultas = new List<ConsultaNutricionalBE_DNI101>();

        private FormsPlot _formsPlot = null!;
        private ComboBox _cmbIndicador = null!;
        private Label _lblDiagnosticoBadge = null!;
        private Label _lblDetallesClinicos = null!;
        private Panel _pnlAlertaCritica = null!;
        private Label _lblTextoAlerta = null!;
        private DataGridView _dgvHistorial = null!;

        public frmGraficarDiagnostico_DNI101(PacienteBE_DNI101 paciente)
        {
            _paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
            _consultaBLL = new ConsultaNutricionalBLL_DNI101();

            ConfigurarFormulario();
            ConstruirComponentes();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            Text = $"CUN07: Graficar Diagnóstico OMS - {_paciente.NombreCompleto}";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1150, 720);
            MinimumSize = new Size(950, 600);
            BackColor = System.Drawing.Color.FromArgb(245, 248, 245);
        }

        private void ConstruirComponentes()
        {
            // Panel Superior: Información del paciente y selector de curva
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = System.Drawing.Color.FromArgb(76, 124, 89),
                Padding = new Padding(15, 10, 15, 10)
            };

            Label lblTitulo = new Label
            {
                Text = $"📈 CURVAS DE CRECIMIENTO INFANTIL (OMS) — {_paciente.NombreCompleto}",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new Point(15, 12),
                AutoSize = true
            };

            int meses = _paciente.ObtenerEdadMeses();
            string edadStr = meses < 24 ? $"{meses} meses" : $"{meses / 12} años ({meses} meses)";
            Label lblSub = new Label
            {
                Text = $"DNI: {_paciente.DNINiño_DNI101}  |  Sexo: {_paciente.Sexo_DNI101 ?? "N/D"}  |  Nacimiento: {_paciente.FechaNacimiento_DNI101:dd/MM/yyyy} ({edadStr})  |  Cobertura: {_paciente.ObraSocial_DNI101 ?? "Particular"}",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = System.Drawing.Color.FromArgb(220, 240, 225),
                Location = new Point(17, 42),
                AutoSize = true
            };

            Label lblSel = new Label
            {
                Text = "Indicador:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new Point(pnlTop.Width - 360, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true
            };

            _cmbIndicador = new ComboBox
            {
                Location = new Point(pnlTop.Width - 280, 22),
                Size = new Size(260, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _cmbIndicador.Items.AddRange(new object[]
            {
                "📊  IMC para la Edad (kg/m²)",
                "⚖️  Peso para la Edad (kg)",
                "📏  Talla para la Edad (cm)"
            });
            _cmbIndicador.SelectedIndex = 0;
            _cmbIndicador.SelectedIndexChanged += (s, e) => ActualizarGrafico();

            pnlTop.Controls.Add(lblTitulo);
            pnlTop.Controls.Add(lblSub);
            pnlTop.Controls.Add(lblSel);
            pnlTop.Controls.Add(_cmbIndicador);
            Controls.Add(pnlTop);

            // Contenedor Principal: Splitter Horizontal (Izquierda Gráfico, Derecha Panel Clínico)
            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 720,
                SplitterWidth = 6,
                BackColor = System.Drawing.Color.FromArgb(225, 235, 228)
            };

            // Izquierda: Control ScottPlot
            _formsPlot = new FormsPlot
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };
            split.Panel1.Controls.Add(_formsPlot);

            // Derecha: Panel Clínico y Grilla de Historial
            Panel pnlClinico = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White,
                Padding = new Padding(15)
            };

            Label lblTitPanel = new Label
            {
                Text = "DIAGNÓSTICO CLÍNICO EVOLUTIVO",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(60, 80, 65),
                Dock = DockStyle.Top,
                Height = 25
            };

            _lblDiagnosticoBadge = new Label
            {
                Text = "Cargando estado...",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.FromArgb(46, 125, 50),
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter
            };

            _lblDetallesClinicos = new Label
            {
                Text = "Evaluando parámetros antropométricos...",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = System.Drawing.Color.FromArgb(50, 65, 55),
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(0, 5, 0, 5)
            };

            _pnlAlertaCritica = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = System.Drawing.Color.FromArgb(255, 235, 238),
                Visible = false,
                Padding = new Padding(8)
            };
            _pnlAlertaCritica.Paint += (s, e) =>
            {
                using (var pen = new Pen(System.Drawing.Color.FromArgb(229, 57, 53), 1.5f))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, _pnlAlertaCritica.Width - 1, _pnlAlertaCritica.Height - 1);
                }
            };
            _lblTextoAlerta = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(198, 40, 40),
                Dock = DockStyle.Fill
            };
            _pnlAlertaCritica.Controls.Add(_lblTextoAlerta);

            Label lblTitGrilla = new Label
            {
                Text = "Historial de Mediciones Antropométricas:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(70, 85, 75),
                Dock = DockStyle.Top,
                Height = 28,
                Padding = new Padding(0, 8, 0, 0)
            };

            _dgvHistorial = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = System.Drawing.Color.FromArgb(250, 252, 250),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Botones inferiores del panel clínico
            Panel pnlBotones = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(0, 8, 0, 0)
            };

            Button btnNuevaConsulta = new Button
            {
                Text = "➕ Nueva Consulta",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.FromArgb(21, 101, 192),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 35),
                Location = new Point(0, 8),
                Cursor = Cursors.Hand
            };
            btnNuevaConsulta.FlatAppearance.BorderSize = 0;
            btnNuevaConsulta.Click += (s, e) =>
            {
                using (var frm = new frmRegistrarConsulta_DNI101(_paciente))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        CargarDatos();
                    }
                }
            };

            Button btnPlan = new Button
            {
                Text = "📋 Prescribir Plan",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.FromArgb(123, 31, 162),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 35),
                Location = new Point(140, 8),
                Cursor = Cursors.Hand
            };
            btnPlan.FlatAppearance.BorderSize = 0;
            btnPlan.Click += (s, e) =>
            {
                using (var frm = new frmPrescribirPlan_DNI101(_paciente))
                {
                    frm.ShowDialog(this);
                }
            };

            pnlBotones.Controls.Add(btnNuevaConsulta);
            pnlBotones.Controls.Add(btnPlan);

            pnlClinico.Controls.Add(_dgvHistorial);
            pnlClinico.Controls.Add(lblTitGrilla);
            pnlClinico.Controls.Add(_pnlAlertaCritica);
            pnlClinico.Controls.Add(_lblDetallesClinicos);
            pnlClinico.Controls.Add(_lblDiagnosticoBadge);
            pnlClinico.Controls.Add(lblTitPanel);
            pnlClinico.Controls.Add(pnlBotones);

            split.Panel2.Controls.Add(pnlClinico);
            Controls.Add(split);
        }

        private void CargarDatos()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _historialConsultas = _consultaBLL.ObtenerHistorialPorPaciente(_paciente.IdPaciente_DNI101);

                // Poblar grilla
                _dgvHistorial.Columns.Clear();
                _dgvHistorial.Columns.Add("Fecha", "Fecha");
                _dgvHistorial.Columns.Add("Edad", "Edad");
                _dgvHistorial.Columns.Add("Peso", "Peso (kg)");
                _dgvHistorial.Columns.Add("Talla", "Talla (cm)");
                _dgvHistorial.Columns.Add("IMC", "IMC");
                _dgvHistorial.Columns.Add("Estado", "Diagnóstico");

                foreach (var c in _historialConsultas)
                {
                    _dgvHistorial.Rows.Add(
                        c.FechaControl_DNI101.ToString("dd/MM/yyyy"),
                        $"{c.EdadMeses_DNI101} m",
                        c.Medicion?.PesoKg_DNI101.ToString("N2") ?? "-",
                        c.Medicion?.TallaCm_DNI101.ToString("N2") ?? "-",
                        c.Medicion?.IMC_DNI101.ToString("N2") ?? "-",
                        c.Diagnostico?.ClasificacionOMS_DNI101 ?? "Eutrófico"
                    );
                }

                ActualizarPanelDiagnostico();
                ActualizarGrafico();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el historial del paciente: {ex.Message}", "Error Clínico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ActualizarPanelDiagnostico()
        {
            if (_historialConsultas.Count == 0)
            {
                _lblDiagnosticoBadge.Text = "Sin Consultas Previas";
                _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(120, 144, 156);
                _lblDetallesClinicos.Text = "El paciente no tiene mediciones registradas. Haga clic en '➕ Nueva Consulta' para iniciar el seguimiento.";
                _pnlAlertaCritica.Visible = false;
                return;
            }

            var ultima = _historialConsultas.Last();
            string clasif = ultima.Diagnostico?.ClasificacionOMS_DNI101 ?? "Eutrófico (Normal)";
            _lblDiagnosticoBadge.Text = $"Estado: {clasif}";

            // Colores semafóricos según clasificación OMS
            if (clasif.Contains("Desnutrición") || clasif.Contains("Severa") || clasif.Contains("Baja"))
            {
                _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(198, 40, 40); // Rojo intenso
            }
            else if (clasif.Contains("Obesidad"))
            {
                _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(216, 67, 21); // Naranja oscuro
            }
            else if (clasif.Contains("Sobrepeso") || clasif.Contains("Riesgo"))
            {
                _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(245, 124, 0); // Ámbar
            }
            else
            {
                _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(46, 125, 50); // Verde normal
            }

            decimal imc = ultima.Medicion?.IMC_DNI101 ?? 0;
            decimal peso = ultima.Medicion?.PesoKg_DNI101 ?? 0;
            decimal talla = ultima.Medicion?.TallaCm_DNI101 ?? 0;
            _lblDetallesClinicos.Text = $"Último Control ({ultima.FechaControl_DNI101:dd/MM/yyyy}): Peso {peso:N2} kg, Talla {talla:N2} cm, IMC {imc:N2}. {ultima.Diagnostico?.DetallesClinicos_DNI101}";

            // Alerta activa
            var alerta = ultima.Alertas.FirstOrDefault();
            if (alerta != null || (ultima.Diagnostico?.RequiereAlerta_DNI101 ?? false))
            {
                _pnlAlertaCritica.Visible = true;
                _lblTextoAlerta.Text = alerta?.MensajeAlerta_DNI101 ?? $"🚨 ALERTA: Condición nutricional atípica detectada ({clasif}).";
            }
            else
            {
                _pnlAlertaCritica.Visible = false;
            }
        }

        private void ActualizarGrafico()
        {
            _formsPlot.Plot.Clear();

            string sexo = _paciente.Sexo_DNI101 ?? "Masculino";
            string tipoIndicador = "IMC";
            string unidad = "kg/m²";
            string tituloEjeY = "Índice de Masa Corporal (IMC)";

            if (_cmbIndicador.SelectedIndex == 1)
            {
                tipoIndicador = "PESO";
                unidad = "kg";
                tituloEjeY = "Peso (kg)";
            }
            else if (_cmbIndicador.SelectedIndex == 2)
            {
                tipoIndicador = "TALLA";
                unidad = "cm";
                tituloEjeY = "Talla / Longitud (cm)";
            }

            var curva = CurvasOMSData_DNI101.ObtenerCurva(tipoIndicador, sexo);

            // 1. Agregar Curvas de Percentiles de Referencia OMS (Spline / Líneas de fondo)
            var c97 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P97);
            c97.Color = new SPColor(229, 57, 53, 180); // Rojo
            c97.LineWidth = 2f;
            c97.LegendText = "Percentil 97 (Límite Superior)";

            var c85 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P85);
            c85.Color = new SPColor(245, 124, 0, 180); // Naranja
            c85.LineWidth = 1.5f;
            c85.LegendText = "Percentil 85 (Sobrepeso / Alerta)";

            var c50 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P50);
            c50.Color = new SPColor(46, 125, 50, 240); // Verde Mediana OMS
            c50.LineWidth = 2.5f;
            c50.LegendText = "Percentil 50 (Mediana OMS)";

            var c15 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P15);
            c15.Color = new SPColor(245, 124, 0, 180); // Naranja
            c15.LineWidth = 1.5f;
            c15.LegendText = "Percentil 15 (Bajo Peso / Alerta)";

            var c3 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P3);
            c3.Color = new SPColor(229, 57, 53, 180); // Rojo
            c3.LineWidth = 2f;
            c3.LegendText = "Percentil 3 (Límite Inferior)";

            // 2. Extraer y Graficar las Mediciones Reales del Paciente
            List<double> pacMeses = new List<double>();
            List<double> pacValores = new List<double>();

            foreach (var c in _historialConsultas)
            {
                if (c.Medicion != null)
                {
                    double val = 0;
                    if (tipoIndicador == "PESO") val = (double)c.Medicion.PesoKg_DNI101;
                    else if (tipoIndicador == "TALLA") val = (double)c.Medicion.TallaCm_DNI101;
                    else val = (double)c.Medicion.IMC_DNI101;

                    if (val > 0)
                    {
                        pacMeses.Add(c.EdadMeses_DNI101);
                        pacValores.Add(val);
                    }
                }
            }

            if (pacMeses.Count > 0)
            {
                var pacSerie = _formsPlot.Plot.Add.Scatter(pacMeses.ToArray(), pacValores.ToArray());
                pacSerie.Color = new SPColor(25, 118, 210, 255); // Azul Rey profesional
                pacSerie.LineWidth = 3f;
                pacSerie.MarkerSize = 10f;
                pacSerie.LegendText = $"Evolución: {_paciente.Nombre_DNI101} ({pacMeses.Count} controles)";
            }

            // Configurar Ejes y Títulos
            _formsPlot.Plot.Axes.Title.Label.Text = $"{_cmbIndicador.Text} — Estándar OMS ({sexo})";
            _formsPlot.Plot.Axes.Title.Label.FontSize = 14;
            _formsPlot.Plot.Axes.Title.Label.Bold = true;

            _formsPlot.Plot.Axes.Bottom.Label.Text = "Edad del Paciente (en meses cumplidos)";
            _formsPlot.Plot.Axes.Bottom.Label.FontSize = 11;

            _formsPlot.Plot.Axes.Left.Label.Text = $"{tituloEjeY} ({unidad})";
            _formsPlot.Plot.Axes.Left.Label.FontSize = 11;

            _formsPlot.Plot.Axes.SetLimitsX(0, 60);

            _formsPlot.Plot.ShowLegend(SPAlignment.UpperLeft);
            _formsPlot.Refresh();
        }
    }
}
