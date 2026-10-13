using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using Services;

namespace UI.PN2
{
    public partial class frmSeguimientoNutricional_DNI101 : Form, IIdiomaObserver
    {
        private readonly PacienteBLL_DNI101 _pacienteBLL;
        private List<PacienteBE_DNI101> _listaPacientes = new List<PacienteBE_DNI101>();

        public frmSeguimientoNutricional_DNI101()
        {
            _pacienteBLL = new PacienteBLL_DNI101();

            InitializeComponent();

            this.Resize += (s, e) => AjustarDisenoResponsivo();
            this.Load += (s, e) => AjustarDisenoResponsivo();

            AjustarDisenoResponsivo();
            CargarPacientes();
        }

        public void AjustarDisenoResponsivo()
        {
            try
            {
                this.SuspendLayout();

                int w = this.ClientSize.Width;
                int h = this.ClientSize.Height;
                if (w < 100 || h < 100) return;

                int marginX = 15;
                int contentW = Math.Max(320, w - (marginX * 2));

                // 1. Header
                if (_pnlHeader != null)
                {
                    _pnlHeader.Location = new Point(marginX, 10);
                    _pnlHeader.Size = new Size(contentW, 55);
                }

                // 2. Filtros / Búsqueda
                if (_pnlFiltros != null)
                {
                    int topFiltros = _pnlHeader != null ? _pnlHeader.Bottom + 5 : 70;
                    _pnlFiltros.Location = new Point(marginX, topFiltros);
                    _pnlFiltros.Size = new Size(contentW, 58);

                    if (_txtBuscar != null && _btnRecargar != null && _lblLupa != null)
                    {
                        int btnW = _btnRecargar.Width;
                        _btnRecargar.Location = new Point(Math.Max(300, _pnlFiltros.ClientSize.Width - btnW - 15), 11);

                        int searchLeft = _lblLupa.Right + 12;
                        int availableW = _btnRecargar.Left - searchLeft - 20;
                        _txtBuscar.Location = new Point(searchLeft, 15);
                        _txtBuscar.Size = new Size(Math.Min(450, Math.Max(180, availableW)), 26);
                    }
                }

                // 3. Grilla de Pacientes
                if (_pnlGrilla != null)
                {
                    int topGrilla = _pnlFiltros != null ? _pnlFiltros.Bottom + 12 : 140;
                    _pnlGrilla.Location = new Point(marginX, topGrilla);
                    _pnlGrilla.Size = new Size(contentW, Math.Max(200, h - topGrilla - 20));
                }
            }
            catch
            {
                // Evitar excepciones transitorias durante renderizado
            }
            finally
            {
                this.ResumeLayout(true);
            }
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

            var colDni = new DataGridViewTextBoxColumn
            {
                Name = "DNI",
                HeaderText = "DNI del Niño",
                FillWeight = 85
            };
            var colNombre = new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                HeaderText = "Nombre Completo",
                FillWeight = 160
            };
            var colSexo = new DataGridViewTextBoxColumn
            {
                Name = "Sexo",
                HeaderText = "Sexo",
                FillWeight = 75
            };
            colSexo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSexo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colEdad = new DataGridViewTextBoxColumn
            {
                Name = "Edad",
                HeaderText = "Edad Pediátrica",
                FillWeight = 95
            };
            var colNac = new DataGridViewTextBoxColumn
            {
                Name = "Nacimiento",
                HeaderText = "Fecha de Nacimiento",
                FillWeight = 105
            };
            colNac.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNac.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colObra = new DataGridViewTextBoxColumn
            {
                Name = "ObraSocial",
                HeaderText = "Cobertura Médica",
                FillWeight = 115
            };
            var colTel = new DataGridViewTextBoxColumn
            {
                Name = "Telefono",
                HeaderText = "Teléfono de Contacto",
                FillWeight = 100
            };

            _dgvPacientes.Columns.AddRange(colDni, colNombre, colSexo, colEdad, colNac, colObra, colTel);

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

            _dgvPacientes.ClearSelection();
        }

        private void _txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            FiltrarPacientes();
        }

        private void _btnRecargar_Click(object? sender, EventArgs e)
        {
            CargarPacientes();
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

        public void ActualizarIdioma()
        {
            // Soporte para internacionalización
        }
    }
}
