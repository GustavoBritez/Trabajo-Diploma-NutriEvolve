using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;

namespace UI.PN2
{
    public class frmSeguimientoNutricional_DNI101 : Form
    {
        private readonly PacienteBLL_DNI101 _pacienteBLL;
        private List<PacienteBE_DNI101> _listaPacientes = new List<PacienteBE_DNI101>();

        private DataGridView _dgvPacientes = null!;
        private TextBox _txtBuscar = null!;

        public frmSeguimientoNutricional_DNI101()
        {
            _pacienteBLL = new PacienteBLL_DNI101();
            ConfigurarFormulario();
            ConstruirComponentes();
            CargarPacientes();
        }

        private void ConfigurarFormulario()
        {
            Text = "Seguimiento Nutricional Pediátrico";
            BackColor = Color.FromArgb(245, 248, 245);
            Dock = DockStyle.Fill;
            FormBorderStyle = FormBorderStyle.None;
        }

        private void ConstruirComponentes()
        {
            // Panel Superior / Barra de Búsqueda
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = Color.White,
                Padding = new Padding(25, 15, 25, 10)
            };
            pnlTop.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 230, 222), 1))
                    e.Graphics.DrawLine(pen, 0, pnlTop.Height - 1, pnlTop.Width, pnlTop.Height - 1);
            };

            Label lblTit = new Label
            {
                Text = "🥗 MÓDULO DE SEGUIMIENTO NUTRICIONAL PEDIÁTRICO (PN2)",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 125, 50),
                Location = new Point(25, 12),
                AutoSize = true
            };

            Label lblSub = new Label
            {
                Text = "CUN06: Haga clic sobre un paciente en la grilla para acceder a Graficar Diagnóstico, Registrar Consulta o Prescribir Plan.",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(80, 95, 85),
                Location = new Point(27, 40),
                AutoSize = true
            };

            Label lblLupa = new Label
            {
                Text = "🔍 Buscar Paciente (DNI, Nombre o Apellido):",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 75, 65),
                Location = new Point(27, 72),
                AutoSize = true
            };

            _txtBuscar = new TextBox
            {
                Location = new Point(310, 68),
                Size = new Size(320, 26),
                Font = new Font("Segoe UI", 10F)
            };
            _txtBuscar.TextChanged += (s, e) => FiltrarPacientes();

            Button btnRecargar = new Button
            {
                Text = "🔄 Actualizar",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 80, 70),
                BackColor = Color.FromArgb(235, 242, 237),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 28),
                Location = new Point(640, 67),
                Cursor = Cursors.Hand
            };
            btnRecargar.FlatAppearance.BorderColor = Color.FromArgb(200, 215, 205);
            btnRecargar.Click += (s, e) => CargarPacientes();

            pnlTop.Controls.Add(lblTit);
            pnlTop.Controls.Add(lblSub);
            pnlTop.Controls.Add(lblLupa);
            pnlTop.Controls.Add(_txtBuscar);
            pnlTop.Controls.Add(btnRecargar);
            Controls.Add(pnlTop);

            // Grilla de Pacientes
            Panel pnlGrilla = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25, 20, 25, 20)
            };

            _dgvPacientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 42 },
                Cursor = Cursors.Hand,
                GridColor = Color.FromArgb(230, 238, 232)
            };

            _dgvPacientes.EnableHeadersVisualStyles = false;
            _dgvPacientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(76, 124, 89);
            _dgvPacientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _dgvPacientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _dgvPacientes.ColumnHeadersHeight = 38;

            _dgvPacientes.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            _dgvPacientes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 208);
            _dgvPacientes.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 40, 25);
            _dgvPacientes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 252, 249);

            _dgvPacientes.CellClick += DgvPacientes_CellClick;

            pnlGrilla.Controls.Add(_dgvPacientes);
            Controls.Add(pnlGrilla);
        }

        public void CargarPacientes()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _listaPacientes = _pacienteBLL.ListarPacientes();
                PoblarGrilla(_listaPacientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el listado de pacientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void PoblarGrilla(List<PacienteBE_DNI101> pacientes)
        {
            _dgvPacientes.Columns.Clear();
            _dgvPacientes.Columns.Add("DNI", "DNI del Niño");
            _dgvPacientes.Columns.Add("Nombre", "Nombre Completo");
            _dgvPacientes.Columns.Add("Sexo", "Sexo");
            _dgvPacientes.Columns.Add("Edad", "Edad Pediátrica");
            _dgvPacientes.Columns.Add("Nacimiento", "Fecha de Nacimiento");
            _dgvPacientes.Columns.Add("ObraSocial", "Cobertura Médica");
            _dgvPacientes.Columns.Add("Telefono", "Teléfono de Contacto");

            foreach (var p in pacientes)
            {
                int meses = p.ObtenerEdadMeses();
                string edad = meses < 24 ? $"{meses} meses" : $"{meses / 12} años ({meses}m)";

                int rowIdx = _dgvPacientes.Rows.Add(
                    p.DNINiño_DNI101,
                    p.NombreCompleto,
                    p.Sexo_DNI101 ?? "Masculino",
                    edad,
                    p.FechaNacimiento_DNI101.ToString("dd/MM/yyyy"),
                    p.ObraSocial_DNI101 ?? "Particular",
                    p.Telefono_DNI101 ?? "-"
                );
                _dgvPacientes.Rows[rowIdx].Tag = p;
            }
        }

        private void FiltrarPacientes()
        {
            string q = _txtBuscar.Text.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(q))
            {
                PoblarGrilla(_listaPacientes);
                return;
            }

            var filtrados = _listaPacientes.Where(p =>
                (p.DNINiño_DNI101?.ToLowerInvariant().Contains(q) ?? false) ||
                (p.NombreCompleto.ToLowerInvariant().Contains(q)) ||
                (p.ObraSocial_DNI101?.ToLowerInvariant().Contains(q) ?? false)
            ).ToList();

            PoblarGrilla(filtrados);
        }

        private void DgvPacientes_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _dgvPacientes.Rows.Count) return;

            var fila = _dgvPacientes.Rows[e.RowIndex];
            if (fila.Tag is PacienteBE_DNI101 paciente)
            {
                // Disparar el MessageBox totalmente personalizado con la estética del sistema
                using (var customBox = new CustomMessageBoxPN2(paciente))
                {
                    if (customBox.ShowDialog(this) == DialogResult.OK)
                    {
                        switch (customBox.AccionElegida)
                        {
                            case AccionSeleccionadaPN2.GraficarDiagnostico:
                                using (var frmGraf = new frmGraficarDiagnostico_DNI101(paciente))
                                {
                                    frmGraf.ShowDialog(this);
                                }
                                break;

                            case AccionSeleccionadaPN2.RegistrarConsulta:
                                using (var frmReg = new frmRegistrarConsulta_DNI101(paciente))
                                {
                                    frmReg.ShowDialog(this);
                                }
                                break;

                            case AccionSeleccionadaPN2.PrescribirPlan:
                                using (var frmPlan = new frmPrescribirPlan_DNI101(paciente))
                                {
                                    frmPlan.ShowDialog(this);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
