using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BE.PN2;
using BLL;
using BLL.PN2;
using Services;

namespace UI.PN2
{
    public partial class frmBitacoraCambiosConsultas : Form, IIdiomaObserver
    {
        private readonly BitacoraCambiosConsultaBLL_DNI101 _bll = new();
        private List<ConsultaCambioBE_DNI101> _listaConsultas = new();

        public frmBitacoraCambiosConsultas()
        {
            InitializeComponent();
            dtpFechaIni.MaxDate = DateTime.Today;
            dtpFechaFin.MaxDate = DateTime.Today;
            dgvCambios.AutoGenerateColumns = false;
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        private void frmBitacoraCambiosConsultas_Load(object? sender, EventArgs e)
        {
            CargarBitacora();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                gbGrilla.Text = "Registro Histórico de Cambios";
            }
        }

        public void CargarBitacora(int? idConsulta = null, string? nombre = null, DateTime? fechaIni = null, DateTime? fechaFin = null)
        {
            try
            {
                dgvCambios.DataSource = null;
                _listaConsultas = _bll.ListarCambios(idConsulta, nombre, fechaIni, fechaFin);
                dgvCambios.DataSource = _listaConsultas;
                btnActivar.Enabled = dgvCambios.CurrentRow != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la bitácora de cambios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_Click(object? sender, EventArgs e)
        {
            int? idConsulta = null;
            if (!string.IsNullOrWhiteSpace(txtCodConsulta.Text))
            {
                if (int.TryParse(txtCodConsulta.Text.Trim(), out int idVal))
                {
                    idConsulta = idVal;
                }
                else
                {
                    MessageBox.Show("El Código de Consulta (Cod_Cons) debe ser un número entero válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCodConsulta.Focus();
                    return;
                }
            }

            string? nombre = string.IsNullOrWhiteSpace(txtNombre.Text) ? null : txtNombre.Text.Trim();
            DateTime? fechaIni = dtpFechaIni.Checked ? dtpFechaIni.Value.Date : null;
            DateTime? fechaFin = dtpFechaFin.Checked ? dtpFechaFin.Value.Date : null;

            if (fechaIni.HasValue && fechaFin.HasValue && fechaIni.Value > fechaFin.Value)
            {
                MessageBox.Show("La fecha inicial no puede ser superior a la fecha final.", "Validación de Fechas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarBitacora(idConsulta, nombre, fechaIni, fechaFin);
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtCodConsulta.Clear();
            txtNombre.Clear();
            dtpFechaIni.Checked = false;
            dtpFechaFin.Checked = false;
            CargarBitacora();
        }

        private void btnActivar_Click(object? sender, EventArgs e)
        {
            if (dgvCambios.CurrentRow == null || dgvCambios.CurrentRow.DataBoundItem is not ConsultaCambioBE_DNI101 cambio)
            {
                MessageBox.Show("Seleccione un registro histórico de la grilla para activar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            EjecutarActivacion(cambio);
        }

        private void dgvCambios_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colBtnActivar.Index)
            {
                if (dgvCambios.Rows[e.RowIndex].DataBoundItem is ConsultaCambioBE_DNI101 cambio)
                {
                    EjecutarActivacion(cambio);
                }
            }
        }

        private void EjecutarActivacion(ConsultaCambioBE_DNI101 cambio)
        {
            if (cambio.Act)
            {
                MessageBox.Show(
                    $"Esta versión de la consulta #{cambio.IdConsulta} ya es la versión ACTIVA actualmente en el sistema (Act = 1).",
                    "Versión Ya Activa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var dr = MessageBox.Show(
                $"¿Confirma que desea ACTIVAR la versión histórica #{cambio.IdCambio} de la consulta #{cambio.IdConsulta}?\n\n" +
                $"Paciente: {cambio.NombrePaciente} (DNI: {cambio.DniPaciente})\n" +
                $"Fecha del registro: {cambio.FechaStr} {cambio.HoraStr}\n\n" +
                $"Esta acción actualizará la columna Act = 1 para esta versión, desmarcará las restantes versiones de esta consulta a Act = 0, y restaurará los datos en el sistema clínico activo.",
                "Confirmar Activación de Versión (CUN08)",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            try
            {
                UsuarioBE usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();
                int usuarioDni = usuarioActivo?._Dni ?? 11111111;

                bool resultado = _bll.ActivarVersionHistorica(cambio.IdCambio, usuarioDni);

                if (resultado)
                {
                    BitacoraBLL bitacoraBLL = new BitacoraBLL();
                    bitacoraBLL.RegistrarEvento(
                        2,
                        $"CUN08: Se activó la versión histórica #{cambio.IdCambio} de la Consulta #{cambio.IdConsulta} del paciente {cambio.NombrePaciente} (Act=1).",
                        usuarioDni,
                        "CUN08");

                    MessageBox.Show(
                        $"¡Versión #{cambio.IdCambio} activada con éxito!\n\n" +
                        $"La consulta #{cambio.IdConsulta} ha sido restaurada con la información de este registro histórico y se asignó Act = 1.\n" +
                        $"Los dígitos verificadores fueron recalculados correctamente.",
                        "Activación Exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    btnAplicar_Click(null, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("No se pudo completar la activación de la versión histórica.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la activación de la versión: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCambios_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCambios.Rows.Count)
            {
                if (dgvCambios.Rows[e.RowIndex].DataBoundItem is ConsultaCambioBE_DNI101 cambio)
                {
                    if (cambio.Act)
                    {
                        dgvCambios.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 248, 235);
                        dgvCambios.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(170, 215, 180);
                        dgvCambios.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;

                        if (e.ColumnIndex == colAct.Index)
                        {
                            e.CellStyle.Font = new Font(dgvCambios.Font, FontStyle.Bold);
                            e.CellStyle.ForeColor = Color.FromArgb(20, 120, 40);
                        }
                    }
                    else
                    {
                        dgvCambios.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        if (e.ColumnIndex == colAct.Index)
                        {
                            e.CellStyle.ForeColor = Color.Gray;
                        }
                    }
                }
            }
        }

        private void btnSalir_Click(object? sender, EventArgs e)
        {
            if (this.TopLevel)
            {
                this.Close();
            }
            else
            {
                this.Hide();
            }
        }

        private void btnAyudaContextual_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "MÓDULO DE BITÁCORA DE CAMBIOS (CUN08)\n\n" +
                "1. Persistencia Histórica:\n" +
                "   Cada inserción o modificación en las consultas nutricionales de los pacientes genera automáticamente un registro histórico en la bitácora mediante triggers de base de datos.\n\n" +
                "2. Columna Act (Estado Activo):\n" +
                "   Identifica la versión actualmente activa (1) en el sistema. Solamente un registro histórico por consulta mantiene el valor Act = 1.\n\n" +
                "3. Filtros:\n" +
                "   Puede filtrar por Código de Consulta (Cod_Cons), Nombre del Paciente y rango de fechas (Fecha Ini / Fecha Fin).\n\n" +
                "4. Botón ACTIVAR:\n" +
                "   Permite seleccionar cualquier versión histórica anterior y restaurarla en el sistema activo, asignándole Act = 1 y recalculando la integridad por Dígitos Verificadores.",
                "Ayuda - Bitácora de Cambios (CUN08)",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
