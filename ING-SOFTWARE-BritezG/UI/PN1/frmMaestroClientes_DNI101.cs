using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using Services;

namespace UI.PN1
{
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

        public frmMaestroClientes_DNI101()
        {
            InitializeComponent();
            ConfigurarEstado(ModoOperacion.Inicio);
            CargarGrillaClientes();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
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
                    txtApellidos.ReadOnly = true;
                    txtApellidos.BackColor = Color.WhiteSmoke;
                    txtNombres.ReadOnly = true;
                    txtNombres.BackColor = Color.WhiteSmoke;
                    txtEmail.ReadOnly = true;
                    txtEmail.BackColor = Color.WhiteSmoke;
                    txtCelular.ReadOnly = true;
                    txtCelular.BackColor = Color.WhiteSmoke;
                    txtDireccion.ReadOnly = true;
                    txtDireccion.BackColor = Color.WhiteSmoke;

                    btnAnadir.Enabled = true;
                    btnModificar.Enabled = dgvClientes.CurrentRow != null;
                    btnEliminar.Enabled = dgvClientes.CurrentRow != null;

                    btnAplicar.Enabled = false;
                    btnCancelar.Enabled = false;
                    btnSalir.Enabled = true;

                    btnGestionEmail1N.Enabled = dgvClientes.CurrentRow != null;
                    btnGestionCelular1N.Enabled = dgvClientes.CurrentRow != null;
                    break;

                case ModoOperacion.Alta:
                    dgvClientes.Enabled = false;

                    LimpiarCampos();

                    txtDni.ReadOnly = false;
                    txtDni.BackColor = Color.White;
                    txtApellidos.ReadOnly = false;
                    txtApellidos.BackColor = Color.White;
                    txtNombres.ReadOnly = false;
                    txtNombres.BackColor = Color.White;
                    txtEmail.ReadOnly = false;
                    txtEmail.BackColor = Color.White;
                    txtCelular.ReadOnly = false;
                    txtCelular.BackColor = Color.White;
                    txtDireccion.ReadOnly = false;
                    txtDireccion.BackColor = Color.White;

                    btnAnadir.Enabled = false;
                    btnModificar.Enabled = false;
                    btnEliminar.Enabled = false;

                    btnAplicar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnSalir.Enabled = false;

                    btnGestionEmail1N.Enabled = false;
                    btnGestionCelular1N.Enabled = false;

                    txtDni.Focus();
                    break;

                case ModoOperacion.Modificacion:
                    dgvClientes.Enabled = false;

                    // En modificación la clave primaria DNI no se altera
                    txtDni.ReadOnly = true;
                    txtDni.BackColor = Color.FromArgb(235, 235, 235);
                    txtApellidos.ReadOnly = false;
                    txtApellidos.BackColor = Color.White;
                    txtNombres.ReadOnly = false;
                    txtNombres.BackColor = Color.White;
                    txtEmail.ReadOnly = false;
                    txtEmail.BackColor = Color.White;
                    txtCelular.ReadOnly = false;
                    txtCelular.BackColor = Color.White;
                    txtDireccion.ReadOnly = false;
                    txtDireccion.BackColor = Color.White;

                    btnAnadir.Enabled = false;
                    btnModificar.Enabled = false;
                    btnEliminar.Enabled = false;

                    btnAplicar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnSalir.Enabled = false;

                    btnGestionEmail1N.Enabled = true;
                    btnGestionCelular1N.Enabled = true;

                    txtApellidos.Focus();
                    break;
            }
        }

        private void LimpiarCampos()
        {
            txtDni.Clear();
            txtApellidos.Clear();
            txtNombres.Clear();
            txtEmail.Clear();
            txtCelular.Clear();
            txtDireccion.Clear();
            _idPacienteSeleccionado = 0;
        }

        private void CargarCamposDesdeGrilla()
        {
            if (dgvClientes.CurrentRow == null || _modoActual != ModoOperacion.Inicio) return;

            try
            {
                var row = dgvClientes.CurrentRow;
                _idPacienteSeleccionado = Convert.ToInt32(row.Cells["IdPaciente_DNI101"].Value);
                txtDni.Text = row.Cells["DNI"].Value?.ToString() ?? "";
                txtApellidos.Text = row.Cells["APELLIDOS"].Value?.ToString() ?? "";
                txtNombres.Text = row.Cells["NOMBRES"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["EMAIL"].Value?.ToString() ?? "";
                txtCelular.Text = row.Cells["CELULAR"].Value?.ToString() ?? "";
                txtDireccion.Text = row.Cells["DIRECCION"].Value?.ToString() ?? "";
            }
            catch { }
        }

        private void ActualizarBotonesSegunSeleccion()
        {
            if (_modoActual == ModoOperacion.Inicio)
            {
                bool hayFila = dgvClientes.CurrentRow != null;
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

        #region Eventos de Botones

        private void btnAnadir_Click(object? sender, EventArgs e)
        {
            ConfigurarEstado(ModoOperacion.Alta);
        }

        private void btnModificar_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null || _idPacienteSeleccionado <= 0)
            {
                MessageBox.Show("Por favor seleccione un cliente de la grilla para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ConfigurarEstado(ModoOperacion.Modificacion);
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null || _idPacienteSeleccionado <= 0)
            {
                MessageBox.Show("Por favor seleccione un cliente de la grilla para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cliente = $"{txtApellidos.Text}, {txtNombres.Text}".Trim();
            string msg = $"¿Confirma la eliminación lógica del cliente '{cliente}' (DNI: {txtDni.Text})?\n\n" +
                         "Nota de Repercusión 1:N: Se conservarán los registros históricos y se marcarán como inactivos sus contactos asociados.";

            if (MessageBox.Show(msg, "Confirmar Baja Lógica", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _pacienteBLL.EliminarLogico(_idPacienteSeleccionado);
                    MessageBox.Show("Cliente dado de baja lógica correctamente.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ConfigurarEstado(ModoOperacion.Inicio);
                    CargarGrillaClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAplicar_Click(object? sender, EventArgs e)
        {
            string dni = txtDni.Text.Trim();
            string apellidos = txtApellidos.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string email = txtEmail.Text.Trim();
            string celular = txtCelular.Text.Trim();
            string direccion = txtDireccion.Text.Trim();

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

            try
            {
                if (_modoActual == ModoOperacion.Alta)
                {
                    _pacienteBLL.GuardarMaestro(dni, apellidos, nombres, email, celular, direccion);
                    MessageBox.Show("Cliente agregado exitosamente con sus tablas 1:N sincronizadas.", "Alta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_modoActual == ModoOperacion.Modificacion)
                {
                    _pacienteBLL.ModificarMaestro(_idPacienteSeleccionado, dni, apellidos, nombres, email, celular, direccion);
                    MessageBox.Show("Cliente modificado exitosamente con repercusión en sus contactos 1:N.", "Modificación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ConfigurarEstado(ModoOperacion.Inicio);
                CargarGrillaClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar los cambios: {ex.Message}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            ConfigurarEstado(ModoOperacion.Inicio);
            CargarCamposDesdeGrilla();
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
    }
}
