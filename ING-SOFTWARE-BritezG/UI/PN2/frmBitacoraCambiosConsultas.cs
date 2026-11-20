using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BE.PN1;
using BE.PN2;
using BLL;
using BLL.PN2;
using Services;

namespace UI.PN2
{
    public partial class frmBitacoraCambiosConsultas : Form, IIdiomaObserver
    {
        private readonly BitacoraCambiosConsultaBLL_DNI101 _bll = new();
        private List<PacienteCambioBE_DNI101> _listaPacientes = new();
        private List<ConsultaCambioBE_DNI101> _listaConsultas = new();

        public frmBitacoraCambiosConsultas()
        {
            InitializeComponent();
            dtpFechaIni.MaxDate = DateTime.Today;
            dtpFechaFin.MaxDate = DateTime.Today;
            dgvCambiosPacientes.AutoGenerateColumns = false;
            dgvCambios.AutoGenerateColumns = false;

            if (cmbModulo.Items.Count > 0)
            {
                cmbModulo.SelectedIndex = 0; // Por defecto: Maestro de Clientes (ABM)
            }
            if (cmbOperacion.Items.Count > 0)
            {
                cmbOperacion.SelectedIndex = 0; // Por defecto: Todas
            }

            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public frmBitacoraCambiosConsultas(string? dniFiltro) : this()
        {
            if (!string.IsNullOrWhiteSpace(dniFiltro))
            {
                txtCodConsulta.Text = dniFiltro;
                cmbModulo.SelectedIndex = 0; // Maestro de Clientes (ABM)
                CargarBitacoraPacientes(dni: dniFiltro);
            }
        }

        private void frmBitacoraCambiosConsultas_Load(object? sender, EventArgs e)
        {
            ConfigurarVistaSegunModulo();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                RefrescarDatos();
            }
        }

        public void RefrescarDatos()
        {
            txtCodConsulta.Clear();
            txtNombre.Clear();
            dtpFechaIni.Checked = false;
            dtpFechaFin.Checked = false;

            if (cmbOperacion.Items.Count > 0)
            {
                cmbOperacion.SelectedIndex = 0; // Todas
            }

            if (cmbModulo.SelectedIndex == 0)
            {
                CargarBitacoraPacientes();
            }
            else
            {
                CargarBitacoraConsultas();
            }
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                if (cmbModulo.SelectedIndex == 0)
                {
                    gbGrilla.Text = "Registro Histórico de Cambios - Maestro de Clientes";
                }
                else
                {
                    gbGrilla.Text = "Registro Histórico de Cambios - Consultas Nutricionales";
                }
            }
        }

        private void cmbModulo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ConfigurarVistaSegunModulo();
        }

        private void ConfigurarVistaSegunModulo()
        {
            if (cmbModulo.SelectedIndex == 0) // Maestro de Clientes (ABM)
            {
                lblTitulo.Text = "BITÁCORA DE CAMBIOS - MAESTRO DE CLIENTES (ABM)";
                lblSubtitulo.Text = "Persistencia histórica de cambios, bajas lógicas y activación de versiones de clientes";
                gbGrilla.Text = "Registro Histórico de Cambios - Maestro de Clientes";

                lblCodProd.Text = "DNI Cliente:";
                txtCodConsulta.PlaceholderText = "DNI Cliente";

                lblNombre.Text = "Nombre / Apellido:";
                txtNombre.PlaceholderText = "Buscar por cliente";

                lblOperacion.Visible = true;
                cmbOperacion.Visible = true;

                dgvCambiosPacientes.Visible = true;
                dgvCambiosPacientes.BringToFront();
                dgvCambios.Visible = false;

                CargarBitacoraPacientes();
            }
            else // Consultas Nutricionales (CUN08)
            {
                lblTitulo.Text = "BITÁCORA DE CAMBIOS - CONSULTAS NUTRICIONALES (CUN08)";
                lblSubtitulo.Text = "Persistencia histórica de cambios en consultas de pacientes y activación de versiones";
                gbGrilla.Text = "Registro Histórico de Cambios - Consultas Nutricionales";

                lblCodProd.Text = "Cod_Cons.:";
                txtCodConsulta.PlaceholderText = "ID Consulta";

                lblNombre.Text = "Nombre:";
                txtNombre.PlaceholderText = "Buscar por paciente";

                lblOperacion.Visible = false;
                cmbOperacion.Visible = false;

                dgvCambiosPacientes.Visible = false;
                dgvCambios.Visible = true;
                dgvCambios.BringToFront();

                CargarBitacoraConsultas();
            }
        }

        #region Carga y Filtrado - Maestro de Clientes

        public void CargarBitacoraPacientes(string? dni = null, string? nombre = null, DateTime? fechaIni = null, DateTime? fechaFin = null, string? tipoOperacion = null)
        {
            try
            {
                dgvCambiosPacientes.DataSource = null;
                _listaPacientes = _bll.ListarCambiosPacientes(
                    idPaciente: null,
                    dni: dni,
                    nombre: nombre,
                    fechaIni: fechaIni,
                    fechaFin: fechaFin,
                    tipoOperacion: tipoOperacion);

                dgvCambiosPacientes.DataSource = _listaPacientes;
                btnActivar.Enabled = dgvCambiosPacientes.CurrentRow != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la bitácora de cambios de clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Carga y Filtrado - Consultas Nutricionales

        public void CargarBitacoraConsultas(int? idConsulta = null, string? nombre = null, DateTime? fechaIni = null, DateTime? fechaFin = null)
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
                MessageBox.Show($"Error al cargar la bitácora de cambios de consultas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Acciones de Botones

        private void btnAplicar_Click(object? sender, EventArgs e)
        {
            string? filtroTexto = string.IsNullOrWhiteSpace(txtCodConsulta.Text) ? null : txtCodConsulta.Text.Trim();
            string? nombre = string.IsNullOrWhiteSpace(txtNombre.Text) ? null : txtNombre.Text.Trim();
            DateTime? fechaIni = dtpFechaIni.Checked ? dtpFechaIni.Value.Date : null;
            DateTime? fechaFin = dtpFechaFin.Checked ? dtpFechaFin.Value.Date : null;

            if (fechaIni.HasValue && fechaFin.HasValue && fechaIni.Value > fechaFin.Value)
            {
                MessageBox.Show("La fecha inicial no puede ser superior a la fecha final.", "Validación de Fechas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbModulo.SelectedIndex == 0) // Maestro Clientes
            {
                string? opSeleccionada = cmbOperacion.SelectedItem?.ToString();
                if (opSeleccionada == "Todas") opSeleccionada = null;

                CargarBitacoraPacientes(
                    dni: filtroTexto,
                    nombre: nombre,
                    fechaIni: fechaIni,
                    fechaFin: fechaFin,
                    tipoOperacion: opSeleccionada);
            }
            else // Consultas Nutricionales
            {
                int? idConsulta = null;
                if (!string.IsNullOrWhiteSpace(filtroTexto))
                {
                    if (int.TryParse(filtroTexto, out int idVal))
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

                CargarBitacoraConsultas(idConsulta, nombre, fechaIni, fechaFin);
            }
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            RefrescarDatos();
        }

        private void btnActivar_Click(object? sender, EventArgs e)
        {
            if (cmbModulo.SelectedIndex == 0) // Maestro de Clientes
            {
                if (dgvCambiosPacientes.CurrentRow == null || dgvCambiosPacientes.CurrentRow.DataBoundItem is not PacienteCambioBE_DNI101 cambioPac)
                {
                    MessageBox.Show("Seleccione un registro histórico de cliente para activar / revertir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                EjecutarActivacionPaciente(cambioPac);
            }
            else // Consultas Nutricionales
            {
                if (dgvCambios.CurrentRow == null || dgvCambios.CurrentRow.DataBoundItem is not ConsultaCambioBE_DNI101 cambioCons)
                {
                    MessageBox.Show("Seleccione un registro histórico de consulta para activar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                EjecutarActivacionConsulta(cambioCons);
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
            try
            {
                using var frmAyuda = new frmManualAyuda(5);
                frmAyuda.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda contextual: {ex.Message}", "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Activación / Rollback de Clientes

        private void dgvCambiosPacientes_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colPacBtnActivar.Index)
            {
                if (dgvCambiosPacientes.Rows[e.RowIndex].DataBoundItem is PacienteCambioBE_DNI101 cambio)
                {
                    EjecutarActivacionPaciente(cambio);
                }
            }
        }

        private void EjecutarActivacionPaciente(PacienteCambioBE_DNI101 cambio)
        {
            if (cambio.Act)
            {
                MessageBox.Show(
                    $"Esta versión del cliente DNI {cambio.Dni} ({cambio.Cliente}) ya es la versión ACTIVA actualmente en el sistema (Act = 1).",
                    "Versión Ya Activa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var dr = MessageBox.Show(
                $"¿Confirma que desea ACTIVAR / REVERTIR la versión histórica #{cambio.IdCambio} del cliente '{cambio.Cliente}' (DNI: {cambio.Dni})?\n\n" +
                $"Fecha del registro: {cambio.FechaStr} {cambio.HoraStr}\n" +
                $"Operación original: {cambio.OperacionStr}\n" +
                $"Estado en esa versión: {cambio.EstadoStr}\n" +
                $"Email: {cambio.Email}\n" +
                $"Celular: {cambio.Telefono}\n" +
                $"Dirección: {cambio.Direccion}\n\n" +
                $"Esta acción restaurará los datos del cliente en la base de datos activa (Pacientes_DNI101), marcará esta versión como Act = 1, y recalculará la integridad del sistema.",
                "Confirmar Activación de Versión de Cliente",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            try
            {
                UsuarioBE usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();
                int usuarioDni = usuarioActivo?._Dni ?? 11111111;

                bool resultado = _bll.RevertirCambioPaciente(cambio.IdCambio, usuarioDni);

                if (resultado)
                {
                    BitacoraBLL bitacoraBLL = new BitacoraBLL();
                    bitacoraBLL.RegistrarEvento(
                        2,
                        $"Bitácora de Cambios: Se activó / restauró la versión histórica #{cambio.IdCambio} del cliente {cambio.Cliente} (DNI: {cambio.Dni}).",
                        usuarioDni,
                        "BitacoraCambios");

                    MessageBox.Show(
                        $"¡Versión #{cambio.IdCambio} activada con éxito!\n\n" +
                        $"El cliente '{cambio.Cliente}' (DNI: {cambio.Dni}) ha sido restaurado en el sistema activo con la información de este registro histórico.\n" +
                        $"Los dígitos verificadores fueron recalculados correctamente.",
                        "Activación Exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    btnAplicar_Click(null, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("No se pudo completar la activación de la versión histórica del cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la activación de la versión: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCambiosPacientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCambiosPacientes.Rows.Count)
            {
                if (dgvCambiosPacientes.Rows[e.RowIndex].DataBoundItem is PacienteCambioBE_DNI101 cambio)
                {
                    if (cambio.Act)
                    {
                        dgvCambiosPacientes.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 248, 235);
                        dgvCambiosPacientes.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(170, 215, 180);
                        dgvCambiosPacientes.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;

                        if (e.ColumnIndex == colPacAct.Index)
                        {
                            e.CellStyle.Font = new Font(dgvCambiosPacientes.Font, FontStyle.Bold);
                            e.CellStyle.ForeColor = Color.FromArgb(20, 120, 40);
                        }
                    }
                    else
                    {
                        dgvCambiosPacientes.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        if (e.ColumnIndex == colPacAct.Index)
                        {
                            e.CellStyle.ForeColor = Color.Gray;
                        }
                    }

                    if (e.ColumnIndex == colPacOperacion.Index)
                    {
                        if (cambio.OperacionStr == "BLOQUEAR")
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(180, 40, 40);
                            e.CellStyle.Font = new Font(dgvCambiosPacientes.Font, FontStyle.Bold);
                        }
                        else if (cambio.OperacionStr == "DESBLOQUEAR")
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(30, 120, 160);
                            e.CellStyle.Font = new Font(dgvCambiosPacientes.Font, FontStyle.Bold);
                        }
                        else if (cambio.OperacionStr == "ALTA")
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(20, 130, 40);
                            e.CellStyle.Font = new Font(dgvCambiosPacientes.Font, FontStyle.Bold);
                        }
                    }
                }
            }
        }

        #endregion

        #region Activación / Rollback de Consultas

        private void dgvCambios_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colBtnActivar.Index)
            {
                if (dgvCambios.Rows[e.RowIndex].DataBoundItem is ConsultaCambioBE_DNI101 cambio)
                {
                    EjecutarActivacionConsulta(cambio);
                }
            }
        }

        private void EjecutarActivacionConsulta(ConsultaCambioBE_DNI101 cambio)
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

        #endregion
    }
}
