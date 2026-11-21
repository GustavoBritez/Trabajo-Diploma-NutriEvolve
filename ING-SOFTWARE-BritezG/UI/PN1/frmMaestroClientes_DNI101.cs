using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using BLL;
using Services;

namespace UI.PN1
{
    [Serializable]
    [XmlRoot("Clientes")]
    public class ClientesXmlContainer
    {
        [XmlElement("Cliente")]
        public List<ClienteXmlItem> Clientes { get; set; } = new();
    }

    [Serializable]
    public class ClienteXmlItem
    {
        public string DNI { get; set; } = string.Empty;
        public string APELLIDOS { get; set; } = string.Empty;
        public string NOMBRES { get; set; } = string.Empty;
        public string FECHA_NACIMIENTO { get; set; } = string.Empty;
        public string SEXO { get; set; } = "Masculino";
        public string COBERTURA { get; set; } = "Particular";
        public string EMAIL { get; set; } = string.Empty;
        public string CELULAR { get; set; } = string.Empty;
        public string DIRECCION { get; set; } = string.Empty;
    }

    public partial class frmMaestroClientes_DNI101 : Form, IIdiomaObserver
    {
        private enum ModoOperacion
        {
            Inicio,
            Alta,
            Modificacion
        }

        private ModoOperacion _modoActual = ModoOperacion.Inicio;
        private readonly PacienteBLL_DNI101 _pacienteBLL = new();
        private int _idPacienteSeleccionado = 0;
        private bool _procesandoAltaDialogo = false;
        private bool _dniCifradoActivo = false;

        public frmMaestroClientes_DNI101()
        {
            InitializeComponent();
            dtpFechaNacimiento.MaxDate = DateTime.Today;
            if (cmbSexo.Items.Count > 0) cmbSexo.SelectedIndex = 0;
            dtpFechaNacimiento.ValueChanged += (_, _) => ActualizarEtiquetaEdad();
            ConfigurarEventosTextBoxesAlta();
            ConfigurarEstado(ModoOperacion.Inicio);
            CargarGrillaClientes();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
            AgregarMensaje("Sistema iniciado. Matriz de clientes lista.");
        }

        private void ActualizarEtiquetaEdad()
        {
            DateTime fn = dtpFechaNacimiento.Value.Date;
            DateTime hoy = DateTime.Today;
            int meses = (hoy.Year - fn.Year) * 12 + (hoy.Month - fn.Month);
            if (hoy.Day < fn.Day) meses--;
            meses = Math.Max(0, meses);

            lblEdadActual.Text = meses >= 12
                ? $"{meses / 12}a ({meses}m)"
                : $"{meses}m";
        }

        private void ConfigurarEventosTextBoxesAlta()
        {
            txtDni.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; txtApellidos.Focus(); } };
            txtApellidos.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; txtNombres.Focus(); } };
            txtNombres.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; dtpFechaNacimiento.Focus(); } };
            dtpFechaNacimiento.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; txtCobertura.Focus(); } };
            txtCobertura.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; txtEmail.Focus(); } };
            txtEmail.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; txtCelular.Focus(); } };
            txtCelular.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; txtDireccion.Focus(); } };
            txtDireccion.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && _modoActual == ModoOperacion.Alta) { e.SuppressKeyPress = true; EjecutarFlujoAltaCliente(); } };
            txtDireccion.Leave += TxtDireccion_Leave;
        }

        private void TxtDireccion_Leave(object? sender, EventArgs e)
        {
            if (_modoActual != ModoOperacion.Alta || _procesandoAltaDialogo) return;

            this.BeginInvoke(new Action(() =>
            {
                if (_modoActual != ModoOperacion.Alta || _procesandoAltaDialogo) return;
                if (this.ActiveControl == btnCancelar) return;

                if (!string.IsNullOrWhiteSpace(txtDni.Text) &&
                    !string.IsNullOrWhiteSpace(txtApellidos.Text) &&
                    !string.IsNullOrWhiteSpace(txtNombres.Text))
                {
                    EjecutarFlujoAltaCliente();
                }
            }));
        }

        private void EjecutarFlujoAltaCliente()
        {
            if (_modoActual != ModoOperacion.Alta || _procesandoAltaDialogo) return;

            string dni = txtDni.Text.Trim();
            string apellidos = txtApellidos.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string email = txtEmail.Text.Trim();
            string celular = txtCelular.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            DateTime fechaNacimiento = dtpFechaNacimiento.Value.Date;
            string sexo = cmbSexo.SelectedItem?.ToString() ?? "Masculino";
            string cobertura = string.IsNullOrWhiteSpace(txtCobertura.Text) ? "Particular" : txtCobertura.Text.Trim();

            if (string.IsNullOrWhiteSpace(dni))
            {
                MessageBox.Show("El campo DNI es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(apellidos))
            {
                MessageBox.Show("El campo Apellidos es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellidos.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(nombres))
            {
                MessageBox.Show("El campo Nombres es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombres.Focus();
                return;
            }

            _procesandoAltaDialogo = true;
            try
            {
                using var dlg = new frmConfirmarAltaCliente_DNI101(dni, apellidos, nombres, email, celular, direccion, fechaNacimiento, sexo, cobertura);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _pacienteBLL.GuardarMaestro(dni, apellidos, nombres, email, celular, direccion, fechaNacimiento, sexo, cobertura);
                    AgregarMensaje($"Alta exitosa: Cliente DNI {dni} ({apellidos}, {nombres}) registrado.");
                    MessageBox.Show("Cliente agregado exitosamente con sus tablas 1:N sincronizadas.", "Alta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ConfigurarEstado(ModoOperacion.Inicio);
                    CargarGrillaClientes();
                }
                else
                {
                    ConfigurarEstado(ModoOperacion.Inicio);
                    CargarCamposDesdeGrilla();
                    AgregarMensaje("Operación de alta cancelada por el usuario. Controles liberados.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar cliente: {ex.Message}", "Error en Alta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AgregarMensaje($"Error en alta: {ex.Message}");
            }
            finally
            {
                _procesandoAltaDialogo = false;
            }
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                TraductorUI.TraducirDataGridView(dgvClientes, idiomaBLL, this.Name);
            }
        }

        public void CargarGrillaClientes()
        {
            try
            {
                DataTable dt = _pacienteBLL.ListarMaestro();
                dgvClientes.DataSource = dt;

                if (dgvClientes.Columns["IdPaciente_DNI101"] != null)
                {
                    dgvClientes.Columns["IdPaciente_DNI101"].Visible = false;
                }

                if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
                {
                    TraductorUI.TraducirDataGridView(dgvClientes, new IdiomaBLL(), this.Name);
                }

                _dniCifradoActivo = false;
                ActualizarEstadoBotonRevertir();
                if (dgvClientes.Columns["colDni"] != null)
                {
                    dgvClientes.Columns["colDni"].Width = 75;
                }
                lblDni.Text = " DNI:";

                if (dgvClientes.Rows.Count > 0)
                {
                    dgvClientes.Rows[0].Selected = true;
                    CargarCamposDesdeGrilla();
                }
                else
                {
                    LimpiarCampos();
                }

                ActualizarBotonesSegunSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro de clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AgregarMensaje($"Error al cargar datos: {ex.Message}");
            }
        }

        private void ConfigurarEstado(ModoOperacion modo)
        {
            _modoActual = modo;

            switch (modo)
            {
                case ModoOperacion.Inicio:
                    dgvClientes.Enabled = true;

                    txtDni.ReadOnly = true;
                    txtDni.BackColor = Color.WhiteSmoke;
                    cmbSexo.Enabled = false;
                    txtApellidos.ReadOnly = true;
                    txtApellidos.BackColor = Color.WhiteSmoke;
                    txtNombres.ReadOnly = true;
                    txtNombres.BackColor = Color.WhiteSmoke;
                    dtpFechaNacimiento.Enabled = false;
                    txtCobertura.ReadOnly = true;
                    txtCobertura.BackColor = Color.WhiteSmoke;
                    txtEmail.ReadOnly = true;
                    txtEmail.BackColor = Color.WhiteSmoke;
                    txtCelular.ReadOnly = true;
                    txtCelular.BackColor = Color.WhiteSmoke;
                    txtDireccion.ReadOnly = true;
                    txtDireccion.BackColor = Color.WhiteSmoke;

                    btnActualizar.Enabled = true;
                    btnAnadir.Enabled = true;
                    bool hayFila = dgvClientes.CurrentRow != null && _idPacienteSeleccionado > 0;
                    btnModificar.Enabled = hayFila;
                    btnEliminar.Enabled = hayFila;

                    btnAplicar.Enabled = false;
                    btnCancelar.Enabled = false;
                    btnSalir.Enabled = true;

                    btnGestionEmail1N.Enabled = hayFila;
                    btnGestionCelular1N.Enabled = hayFila;

                    btnSerializar.Enabled = true;
                    btnDesSerializar.Enabled = true;
                    btnExaminarSerializar.Enabled = true;
                    btnExaminarDesSerializar.Enabled = true;
                    txtRutaSerializar.Enabled = true;
                    txtRutaDesSerializar.Enabled = true;
                    btnLimpiar.Enabled = true;
                    btnAyudaContextual.Enabled = true;
                    btnRevertirDni.Enabled = true;
                    break;

                case ModoOperacion.Alta:
                    dgvClientes.Enabled = false;

                    LimpiarCampos();

                    txtDni.ReadOnly = false;
                    txtDni.BackColor = Color.White;
                    cmbSexo.Enabled = true;
                    txtApellidos.ReadOnly = false;
                    txtApellidos.BackColor = Color.White;
                    txtNombres.ReadOnly = false;
                    txtNombres.BackColor = Color.White;
                    dtpFechaNacimiento.Enabled = true;
                    txtCobertura.ReadOnly = false;
                    txtCobertura.BackColor = Color.White;
                    txtEmail.ReadOnly = false;
                    txtEmail.BackColor = Color.White;
                    txtCelular.ReadOnly = false;
                    txtCelular.BackColor = Color.White;
                    txtDireccion.ReadOnly = false;
                    txtDireccion.BackColor = Color.White;

                    btnActualizar.Enabled = false;
                    btnAnadir.Enabled = false;
                    btnModificar.Enabled = false;
                    btnEliminar.Enabled = false;

                    btnAplicar.Enabled = false;
                    btnCancelar.Enabled = true;
                    btnSalir.Enabled = false;
                    btnAyudaContextual.Enabled = false;
                    btnLimpiar.Enabled = false;

                    btnGestionEmail1N.Enabled = false;
                    btnGestionCelular1N.Enabled = false;

                    btnSerializar.Enabled = false;
                    btnDesSerializar.Enabled = false;
                    btnExaminarSerializar.Enabled = false;
                    btnExaminarDesSerializar.Enabled = false;
                    txtRutaSerializar.Enabled = false;
                    txtRutaDesSerializar.Enabled = false;
                    btnRevertirDni.Enabled = false;

                    txtDni.Focus();
                    AgregarMensaje("Modo AÑADIR activado. Ingrese los datos del nuevo cliente.");
                    break;

                case ModoOperacion.Modificacion:
                    dgvClientes.Enabled = false;

                    txtDni.ReadOnly = true;
                    txtDni.BackColor = Color.FromArgb(235, 235, 235);
                    cmbSexo.Enabled = true;
                    txtApellidos.ReadOnly = false;
                    txtApellidos.BackColor = Color.White;
                    txtNombres.ReadOnly = false;
                    txtNombres.BackColor = Color.White;
                    dtpFechaNacimiento.Enabled = true;
                    txtCobertura.ReadOnly = false;
                    txtCobertura.BackColor = Color.White;
                    txtEmail.ReadOnly = false;
                    txtEmail.BackColor = Color.White;
                    txtCelular.ReadOnly = false;
                    txtCelular.BackColor = Color.White;
                    txtDireccion.ReadOnly = false;
                    txtDireccion.BackColor = Color.White;

                    btnActualizar.Enabled = false;
                    btnAnadir.Enabled = false;
                    btnModificar.Enabled = false;
                    btnEliminar.Enabled = false;

                    btnAplicar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnSalir.Enabled = false;
                    btnAyudaContextual.Enabled = false;
                    btnLimpiar.Enabled = false;

                    btnGestionEmail1N.Enabled = false;
                    btnGestionCelular1N.Enabled = false;

                    btnSerializar.Enabled = false;
                    btnDesSerializar.Enabled = false;
                    btnExaminarSerializar.Enabled = false;
                    btnExaminarDesSerializar.Enabled = false;
                    txtRutaSerializar.Enabled = false;
                    txtRutaDesSerializar.Enabled = false;
                    btnRevertirDni.Enabled = false;

                    txtApellidos.Focus();
                    AgregarMensaje($"Modo MODIFICAR activado para cliente DNI {txtDni.Text}.");
                    break;
            }
        }

        private void LimpiarCampos()
        {
            txtDni.Clear();
            if (cmbSexo.Items.Count > 0) cmbSexo.SelectedIndex = 0;
            txtApellidos.Clear();
            txtNombres.Clear();
            dtpFechaNacimiento.Value = DateTime.Today;
            txtCobertura.Text = "Particular";
            txtEmail.Clear();
            txtCelular.Clear();
            txtDireccion.Clear();
            _idPacienteSeleccionado = 0;
            ActualizarEtiquetaEdad();
        }

        private void CargarCamposDesdeGrilla()
        {
            if (dgvClientes.CurrentRow == null || _modoActual != ModoOperacion.Inicio) return;

            try
            {
                var row = dgvClientes.CurrentRow;
                if (dgvClientes.Columns.Contains("IdPaciente_DNI101") && row.Cells["IdPaciente_DNI101"].Value != null && int.TryParse(row.Cells["IdPaciente_DNI101"].Value.ToString(), out int id))
                {
                    _idPacienteSeleccionado = id;
                }
                else
                {
                    _idPacienteSeleccionado = 0;
                }

                txtDni.Text = ObtenerValorCelda(row, "colDni", "DNI");
                txtApellidos.Text = ObtenerValorCelda(row, "colApellidos", "APELLIDOS");
                txtNombres.Text = ObtenerValorCelda(row, "colNombres", "NOMBRES");
                txtEmail.Text = ObtenerValorCelda(row, "colEmail", "EMAIL");
                txtCelular.Text = ObtenerValorCelda(row, "colCelular", "CELULAR");
                txtDireccion.Text = ObtenerValorCelda(row, "colDireccion", "DIRECCION");

                string sexoVal = ObtenerValorCelda(row, "colSexo", "SEXO");
                if (string.Equals(sexoVal, "Femenino", StringComparison.OrdinalIgnoreCase))
                    cmbSexo.SelectedItem = "Femenino";
                else
                    cmbSexo.SelectedItem = "Masculino";

                string cobVal = ObtenerValorCelda(row, "colCobertura", "COBERTURA");
                txtCobertura.Text = string.IsNullOrWhiteSpace(cobVal) ? "Particular" : cobVal;

                string fnVal = ObtenerValorCelda(row, "colFechaNacimiento", "FECHA_NACIMIENTO");
                if (DateTime.TryParse(fnVal, out DateTime parsedFn) && parsedFn >= dtpFechaNacimiento.MinDate && parsedFn <= dtpFechaNacimiento.MaxDate)
                {
                    dtpFechaNacimiento.Value = parsedFn.Date;
                }
                else
                {
                    dtpFechaNacimiento.Value = DateTime.Today;
                }
                ActualizarEtiquetaEdad();
            }
            catch { }
        }

        private string ObtenerValorCelda(DataGridViewRow row, string colName1, string colName2)
        {
            if (dgvClientes.Columns.Contains(colName1) && row.Cells[colName1].Value != null)
                return row.Cells[colName1].Value.ToString() ?? "";
            if (dgvClientes.Columns.Contains(colName2) && row.Cells[colName2].Value != null)
                return row.Cells[colName2].Value.ToString() ?? "";
            return "";
        }

        private void ActualizarBotonesSegunSeleccion()
        {
            if (_modoActual == ModoOperacion.Inicio)
            {
                bool hayFila = dgvClientes.CurrentRow != null && _idPacienteSeleccionado > 0;
                btnModificar.Enabled = hayFila;
                btnEliminar.Enabled = hayFila;
                btnGestionEmail1N.Enabled = hayFila;
                btnGestionCelular1N.Enabled = hayFila;
            }
        }

        private void dgvClientes_SelectionChanged(object? sender, EventArgs e)
        {
            if (_modoActual == ModoOperacion.Inicio)
            {
                CargarCamposDesdeGrilla();
                ActualizarBotonesSegunSeleccion();
            }
        }

        private void AgregarMensaje(string texto)
        {
            try
            {
                string linea = $"[{DateTime.Now:HH:mm:ss}] {texto}\r\n";
                txtMensaje.AppendText(linea);
                txtMensaje.SelectionStart = txtMensaje.TextLength;
                txtMensaje.ScrollToCaret();
            }
            catch { }
        }

        #region Eventos de Botones ABM

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarGrillaClientes();
            AgregarMensaje($"Matriz de clientes actualizada desde la base de datos ({dgvClientes.Rows.Count} registros).");
        }

        private void btnAnadir_Click(object? sender, EventArgs e)
        {
            if (_dniCifradoActivo)
            {
                RevertirDnisAPlano();
            }
            ConfigurarEstado(ModoOperacion.Alta);
        }

        private void btnModificar_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null)
            {
                MessageBox.Show("Por favor seleccione un cliente de la grilla para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_dniCifradoActivo)
            {
                RevertirDnisAPlano();
            }
            ConfigurarEstado(ModoOperacion.Modificacion);
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null || _idPacienteSeleccionado <= 0)
            {
                MessageBox.Show("Por favor seleccione un cliente registrado en la base de datos para bloquear / dar de baja.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cliente = $"{txtApellidos.Text}, {txtNombres.Text}".Trim();
            string dniMostrar = txtDni.Text.Trim();
            if (_dniCifradoActivo || ServicioCriptografiaAES.EsTextoCifrado(dniMostrar))
            {
                dniMostrar = _pacienteBLL.DescifrarDniAES256(dniMostrar);
            }

            string msg = $"¿Confirma el bloqueo / baja lógica del cliente '{cliente}' (DNI: {dniMostrar})?\n\n" +
                         "Nota: El cliente no se elimina físicamente de la base de datos; la operación se registrará como 'BLOQUEAR' en la Bitácora de Cambios, conservando sus registros históricos y desactivando sus contactos asociados.";

            if (MessageBox.Show(msg, "Confirmar Bloqueo (Baja Lógica)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _pacienteBLL.EliminarLogico(_idPacienteSeleccionado);
                    AgregarMensaje($"Bloqueo lógico realizado para cliente DNI {dniMostrar} ({cliente}). Operación registrada en Bitácora de Cambios como BLOQUEAR.");
                    MessageBox.Show("Cliente bloqueado (baja lógica) correctamente.\nOperación registrada en la Bitácora de Cambios como BLOQUEAR.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ConfigurarEstado(ModoOperacion.Inicio);
                    CargarGrillaClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al bloquear cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AgregarMensaje($"Error al bloquear: {ex.Message}");
                }
            }
        }

        #region Criptografía Reversible (AES-256)

        private void btnRevertirDni_Click(object? sender, EventArgs e)
        {
            if (_modoActual != ModoOperacion.Inicio)
            {
                MessageBox.Show("La función de Criptografía Reversible (AES-256) solo está disponible en modo de consulta de clientes.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvClientes.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros de clientes en la grilla para aplicar criptografía reversible.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _dniCifradoActivo = !_dniCifradoActivo;

                if (dt.Columns.Contains("DNI"))
                {
                    dt.Columns["DNI"].ReadOnly = false;
                    dt.Columns["DNI"].MaxLength = -1;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;
                        string valorActual = row["DNI"]?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(valorActual)) continue;

                        if (_dniCifradoActivo)
                        {
                            row["DNI"] = _pacienteBLL.CifrarDniAES256(valorActual);
                        }
                        else
                        {
                            row["DNI"] = _pacienteBLL.DescifrarDniAES256(valorActual);
                        }
                    }
                }

                if (dgvClientes.Columns["colDni"] != null)
                {
                    dgvClientes.Columns["colDni"].Width = _dniCifradoActivo ? 190 : 75;
                }

                lblDni.Text = _dniCifradoActivo ? " DNI (AES):" : " DNI:";
                ActualizarEstadoBotonRevertir();
                CargarCamposDesdeGrilla();

                if (_dniCifradoActivo)
                {
                    AgregarMensaje("CRIPTOGRAFÍA REVERSIBLE ACTIVADA: Números de DNI cifrados con AES-256 (Base64). Confidencialidad activa.");
                }
                else
                {
                    AgregarMensaje("CRIPTOGRAFÍA REVERSIBLE DESREVERTIDA: Números de DNI descifrados a su valor numérico original en texto plano.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar criptografía reversible: {ex.Message}", "Error de Criptografía", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AgregarMensaje($"Error en criptografía AES-256: {ex.Message}");
            }
        }

        private void ActualizarEstadoBotonRevertir()
        {
            if (_dniCifradoActivo)
            {
                btnRevertirDni.Text = "Desrevertir DNI";
                toolTipGeneral.SetToolTip(btnRevertirDni, "Criptografía Reversible (AES-256 Activa): Presione para descifrar y volver a ver el número de DNI original en texto plano.");
            }
            else
            {
                btnRevertirDni.Text = "Revertir/Desrevertir";
                toolTipGeneral.SetToolTip(btnRevertirDni, "Criptografía Reversible (AES-256): Presione para cifrar con AES-256 los números de DNI y ver su representación en Base64.");
            }
        }

        private void RevertirDnisAPlano()
        {
            if (!_dniCifradoActivo) return;

            try
            {
                if (dgvClientes.DataSource is DataTable dt && dt.Columns.Contains("DNI"))
                {
                    dt.Columns["DNI"].ReadOnly = false;
                    dt.Columns["DNI"].MaxLength = -1;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;
                        string valorActual = row["DNI"]?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(valorActual)) continue;
                        row["DNI"] = _pacienteBLL.DescifrarDniAES256(valorActual);
                    }
                }

                _dniCifradoActivo = false;
                if (dgvClientes.Columns["colDni"] != null)
                {
                    dgvClientes.Columns["colDni"].Width = 75;
                }
                lblDni.Text = " DNI:";
                ActualizarEstadoBotonRevertir();
                CargarCamposDesdeGrilla();
            }
            catch { }
        }

        #endregion

        private void btnAplicar_Click(object? sender, EventArgs e)
        {
            if (_modoActual != ModoOperacion.Modificacion)
            {
                return;
            }

            string dni = txtDni.Text.Trim();
            string apellidos = txtApellidos.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string email = txtEmail.Text.Trim();
            string celular = txtCelular.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            DateTime fechaNacimiento = dtpFechaNacimiento.Value.Date;
            string sexo = cmbSexo.SelectedItem?.ToString() ?? "Masculino";
            string cobertura = string.IsNullOrWhiteSpace(txtCobertura.Text) ? "Particular" : txtCobertura.Text.Trim();

            if (_idPacienteSeleccionado <= 0)
            {
                MessageBox.Show("Por favor seleccione un cliente registrado en la base de datos para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(apellidos))
            {
                MessageBox.Show("El campo Apellidos es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellidos.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(nombres))
            {
                MessageBox.Show("El campo Nombres es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombres.Focus();
                return;
            }

            try
            {
                _pacienteBLL.ModificarMaestro(_idPacienteSeleccionado, dni, apellidos, nombres, email, celular, direccion, fechaNacimiento, sexo, cobertura);
                AgregarMensaje($"Modificación exitosa: Cliente DNI {dni} actualizado.");
                MessageBox.Show("Cliente modificado exitosamente con repercusión en sus contactos 1:N.", "Modificación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ConfigurarEstado(ModoOperacion.Inicio);
                CargarGrillaClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar los cambios: {ex.Message}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AgregarMensaje($"Error en operación: {ex.Message}");
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            ConfigurarEstado(ModoOperacion.Inicio);
            CargarCamposDesdeGrilla();
            AgregarMensaje("Operación cancelada. Se restauraron los campos.");
        }

        private void btnSalir_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void btnGestionContactos1N_Click(object? sender, EventArgs e)
        {
            if (_idPacienteSeleccionado <= 0)
            {
                MessageBox.Show("Seleccione un cliente registrado para gestionar sus múltiples emails o teléfonos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombre = $"{txtApellidos.Text}, {txtNombres.Text}".Trim();
            using var frm1N = new frmContactos1N_DNI101(_idPacienteSeleccionado, nombre);
            frm1N.ShowDialog(this);
            CargarGrillaClientes();
        }

        private void btnAyudaContextual_Click(object? sender, EventArgs e)
        {
            try
            {
                using var frmAyuda = new UI.PN2.frmManualAyuda(2);
                frmAyuda.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda contextual: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Serialización y Des-serialización XML (Directa en Grilla, Sin Tocar BD ni Dígito Verificador)

        private void btnExaminarSerializar_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Title = "Seleccionar ubicación y nombre para el archivo XML",
                Filter = "Archivos XML (*.xml)|*.xml|Todos los archivos (*.*)|*.*",
                FileName = $"Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.xml",
                DefaultExt = "xml"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                txtRutaSerializar.Text = sfd.FileName;
                AgregarMensaje($"Ubicación para serializar seleccionada: {Path.GetFileName(sfd.FileName)}");
            }
        }

        private void btnSerializar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaSerializar.Text))
            {
                btnExaminarSerializar_Click(sender, e);
                if (string.IsNullOrWhiteSpace(txtRutaSerializar.Text))
                {
                    MessageBox.Show("Para SERIALIZAR primero se debe seleccionar la ubicación (📁) en el Disco Duro donde se va a almacenar el Archivo XML.", "Ubicación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (dgvClientes.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos en la matriz de clientes para serializar.", "Matriz Vacía", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string ruta = txtRutaSerializar.Text.Trim();
                var container = new ClientesXmlContainer();

                foreach (DataGridViewRow row in dgvClientes.Rows)
                {
                    if (row.IsNewRow) continue;

                    string dniVal = ObtenerValorCelda(row, "colDni", "DNI");
                    if (_dniCifradoActivo || ServicioCriptografiaAES.EsTextoCifrado(dniVal))
                    {
                        dniVal = _pacienteBLL.DescifrarDniAES256(dniVal);
                    }

                    string fnRaw = ObtenerValorCelda(row, "colFechaNacimiento", "FECHA_NACIMIENTO");
                    string fnStr = DateTime.TryParse(fnRaw, out DateTime fnDate)
                        ? fnDate.ToString("yyyy-MM-dd")
                        : DateTime.Today.ToString("yyyy-MM-dd");

                    container.Clientes.Add(new ClienteXmlItem
                    {
                        DNI = dniVal,
                        APELLIDOS = ObtenerValorCelda(row, "colApellidos", "APELLIDOS"),
                        NOMBRES = ObtenerValorCelda(row, "colNombres", "NOMBRES"),
                        FECHA_NACIMIENTO = fnStr,
                        SEXO = string.IsNullOrWhiteSpace(ObtenerValorCelda(row, "colSexo", "SEXO")) ? "Masculino" : ObtenerValorCelda(row, "colSexo", "SEXO"),
                        COBERTURA = string.IsNullOrWhiteSpace(ObtenerValorCelda(row, "colCobertura", "COBERTURA")) ? "Particular" : ObtenerValorCelda(row, "colCobertura", "COBERTURA"),
                        EMAIL = ObtenerValorCelda(row, "colEmail", "EMAIL"),
                        CELULAR = ObtenerValorCelda(row, "colCelular", "CELULAR"),
                        DIRECCION = ObtenerValorCelda(row, "colDireccion", "DIRECCION")
                    });
                }

                var serializer = new XmlSerializer(typeof(ClientesXmlContainer));
                using (var writer = new StreamWriter(ruta, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, container);
                }

                AgregarMensaje($"SERIALIZACIÓN EXITOSA: {container.Clientes.Count} clientes serializados en '{Path.GetFileName(ruta)}'.");
                MessageBox.Show(
                    $"La matriz de datos con los {container.Clientes.Count} clientes se ha serializado exitosamente en el archivo XML:\n\n{ruta}",
                    "Serialización XML Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la serialización XML: {ex.Message}", "Error de Serialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AgregarMensaje($"Error en serialización: {ex.Message}");
            }
        }

        private void btnExaminarDesSerializar_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Seleccionar archivo XML para des-serializar",
                Filter = "Archivos XML (*.xml)|*.xml|Todos los archivos (*.*)|*.*",
                CheckFileExists = true
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtRutaDesSerializar.Text = ofd.FileName;
                AgregarMensaje($"Archivo XML seleccionado para des-serializar: {Path.GetFileName(ofd.FileName)}");
            }
        }

        private void btnDesSerializar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaDesSerializar.Text) || !File.Exists(txtRutaDesSerializar.Text))
            {
                btnExaminarDesSerializar_Click(sender, e);
                if (string.IsNullOrWhiteSpace(txtRutaDesSerializar.Text) || !File.Exists(txtRutaDesSerializar.Text))
                {
                    MessageBox.Show("Para DES-SERIALIZAR un archivo XML este se debe seleccionar (📁) desde el Disco Duro.", "Archivo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                string ruta = txtRutaDesSerializar.Text.Trim();
                var serializer = new XmlSerializer(typeof(ClientesXmlContainer));

                ClientesXmlContainer? container = null;
                using (var reader = new StreamReader(ruta, Encoding.UTF8))
                {
                    container = (ClientesXmlContainer?)serializer.Deserialize(reader);
                }

                if (container == null || container.Clientes == null || container.Clientes.Count == 0)
                {
                    MessageBox.Show("El archivo XML no contiene registros de clientes válidos.", "XML Vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Cargar directamente en la grilla en memoria (sin golpear la base de datos ni tocar el Dígito Verificador)
                var dt = new DataTable();
                dt.Columns.Add("IdPaciente_DNI101", typeof(int));
                dt.Columns.Add("DNI", typeof(string));
                dt.Columns.Add("APELLIDOS", typeof(string));
                dt.Columns.Add("NOMBRES", typeof(string));
                dt.Columns.Add("FECHA_NACIMIENTO", typeof(DateTime));
                dt.Columns.Add("SEXO", typeof(string));
                dt.Columns.Add("COBERTURA", typeof(string));
                dt.Columns.Add("EMAIL", typeof(string));
                dt.Columns.Add("CELULAR", typeof(string));
                dt.Columns.Add("DIRECCION", typeof(string));

                int idx = 1;
                foreach (var item in container.Clientes)
                {
                    string dniLimpio = item.DNI?.Trim() ?? string.Empty;
                    if (ServicioCriptografiaAES.EsTextoCifrado(dniLimpio))
                    {
                        dniLimpio = _pacienteBLL.DescifrarDniAES256(dniLimpio);
                    }

                    DateTime fn = DateTime.Today;
                    if (!string.IsNullOrWhiteSpace(item.FECHA_NACIMIENTO) && DateTime.TryParse(item.FECHA_NACIMIENTO, out DateTime parsedFn))
                    {
                        fn = parsedFn.Date;
                    }

                    string sexo = string.IsNullOrWhiteSpace(item.SEXO) ? "Masculino" : item.SEXO.Trim();
                    string cobertura = string.IsNullOrWhiteSpace(item.COBERTURA) ? "Particular" : item.COBERTURA.Trim();

                    dt.Rows.Add(
                        idx++,
                        dniLimpio,
                        item.APELLIDOS ?? string.Empty,
                        item.NOMBRES ?? string.Empty,
                        fn,
                        sexo,
                        cobertura,
                        item.EMAIL ?? string.Empty,
                        item.CELULAR ?? string.Empty,
                        item.DIRECCION ?? string.Empty
                    );
                }

                dgvClientes.DataSource = dt;
                if (dgvClientes.Columns["IdPaciente_DNI101"] != null)
                {
                    dgvClientes.Columns["IdPaciente_DNI101"].Visible = false;
                }

                _dniCifradoActivo = false;
                ActualizarEstadoBotonRevertir();
                if (dgvClientes.Columns["colDni"] != null)
                {
                    dgvClientes.Columns["colDni"].Width = 75;
                }
                lblDni.Text = " DNI:";

                if (dgvClientes.Rows.Count > 0)
                {
                    dgvClientes.Rows[0].Selected = true;
                    CargarCamposDesdeGrilla();
                }

                AgregarMensaje($"DES-SERIALIZACIÓN EXITOSA: {container.Clientes.Count} clientes cargados en la grilla desde '{Path.GetFileName(ruta)}'.");
                MessageBox.Show(
                    $"El archivo XML se ha des-serializado correctamente.\nSe cargaron {container.Clientes.Count} registros en la matriz visual de clientes.",
                    "Des-serialización XML Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la des-serialización XML: {ex.Message}", "Error de Des-serialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AgregarMensaje($"Error en des-serialización: {ex.Message}");
            }
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            if (_dniCifradoActivo)
            {
                RevertirDnisAPlano();
            }
            LimpiarCampos();
            txtRutaSerializar.Clear();
            txtRutaDesSerializar.Clear();
            txtMensaje.Clear();
            AgregarMensaje("Formulario y rutas limpiados por el usuario.");
        }

        #endregion
    }
}
