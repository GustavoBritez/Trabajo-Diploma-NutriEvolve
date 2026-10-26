using System;
using System.Data;
using System.Windows.Forms;
using BLL;
using Services;

namespace UI.PN1
{
    public partial class frmContactos1N_DNI101 : Form, IIdiomaObserver
    {
        private readonly int _idPaciente;
        private readonly string _nombrePaciente;
        private readonly PacienteBLL_DNI101 _pacienteBLL = new();

        public frmContactos1N_DNI101(int idPaciente, string nombrePaciente)
        {
            InitializeComponent();
            _idPaciente = idPaciente;
            _nombrePaciente = nombrePaciente;
            lblPaciente.Text = $"Cliente / Paciente: {_nombrePaciente} (ID: {_idPaciente})";
            cmbTipoTel.SelectedIndex = 0;
            CargarGrillas();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                TraductorUI.TraducirDataGridView(dgvEmails, idiomaBLL, this.Name);
                TraductorUI.TraducirDataGridView(dgvTelefonos, idiomaBLL, this.Name);
            }
        }

        private void CargarGrillas()
        {
            try
            {
                DataTable dtEmails = _pacienteBLL.ObtenerEmails1N(_idPaciente);
                dgvEmails.DataSource = dtEmails;
                if (dgvEmails.Columns["IdEmail_DNI101"] != null)
                {
                    dgvEmails.Columns["IdEmail_DNI101"].Visible = false;
                }
                if (dgvEmails.Columns["Email_DNI101"] != null)
                {
                    dgvEmails.Columns["Email_DNI101"].HeaderText = "Correo Electrónico";
                }
                if (dgvEmails.Columns["EsPrincipal_DNI101"] != null)
                {
                    dgvEmails.Columns["EsPrincipal_DNI101"].HeaderText = "¿Es Principal?";
                }

                DataTable dtTels = _pacienteBLL.ObtenerTelefonos1N(_idPaciente);
                dgvTelefonos.DataSource = dtTels;
                if (dgvTelefonos.Columns["IdTelefono_DNI101"] != null)
                {
                    dgvTelefonos.Columns["IdTelefono_DNI101"].Visible = false;
                }
                if (dgvTelefonos.Columns["Numero_DNI101"] != null)
                {
                    dgvTelefonos.Columns["Numero_DNI101"].HeaderText = "Número";
                }
                if (dgvTelefonos.Columns["Tipo_DNI101"] != null)
                {
                    dgvTelefonos.Columns["Tipo_DNI101"].HeaderText = "Tipo";
                }
                if (dgvTelefonos.Columns["EsPrincipal_DNI101"] != null)
                {
                    dgvTelefonos.Columns["EsPrincipal_DNI101"].HeaderText = "¿Es Principal?";
                }

                if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
                {
                    var idiomaBLL = new IdiomaBLL();
                    TraductorUI.TraducirDataGridView(dgvEmails, idiomaBLL, this.Name);
                    TraductorUI.TraducirDataGridView(dgvTelefonos, idiomaBLL, this.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar contactos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarEmail_Click(object? sender, EventArgs e)
        {
            string email = txtNuevoEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Por favor ingrese una dirección de correo válida.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _pacienteBLL.AgregarEmail1N(_idPaciente, email, chkEmailPrincipal.Checked);
                txtNuevoEmail.Clear();
                chkEmailPrincipal.Checked = false;
                CargarGrillas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarEmail_Click(object? sender, EventArgs e)
        {
            if (dgvEmails.CurrentRow == null)
            {
                MessageBox.Show("Seleccione el correo que desea dar de baja lógica.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idEmail = Convert.ToInt32(dgvEmails.CurrentRow.Cells["IdEmail_DNI101"].Value);
            string emailTexto = dgvEmails.CurrentRow.Cells["Email_DNI101"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Confirma la baja lógica del correo '{emailTexto}'?", "Baja Lógica 1:N", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _pacienteBLL.EliminarEmailLogico1N(idEmail);
                    CargarGrillas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAgregarTel_Click(object? sender, EventArgs e)
        {
            string num = txtNuevoTelefono.Text.Trim();
            if (string.IsNullOrWhiteSpace(num))
            {
                MessageBox.Show("Por favor ingrese un número telefónico válido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipo = cmbTipoTel.SelectedItem?.ToString() ?? "Celular";

            try
            {
                _pacienteBLL.AgregarTelefono1N(_idPaciente, num, tipo, chkTelPrincipal.Checked);
                txtNuevoTelefono.Clear();
                chkTelPrincipal.Checked = false;
                CargarGrillas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar teléfono: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarTel_Click(object? sender, EventArgs e)
        {
            if (dgvTelefonos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione el teléfono que desea dar de baja lógica.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idTel = Convert.ToInt32(dgvTelefonos.CurrentRow.Cells["IdTelefono_DNI101"].Value);
            string numTexto = dgvTelefonos.CurrentRow.Cells["Numero_DNI101"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Confirma la baja lógica del teléfono '{numTexto}'?", "Baja Lógica 1:N", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _pacienteBLL.EliminarTelefonoLogico1N(idTel);
                    CargarGrillas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar teléfono: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void btnAyudaContextual_Click(object? sender, EventArgs e)
        {
            try
            {
                using var frmAyuda = new UI.PN2.frmManualAyuda(3);
                frmAyuda.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda contextual: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
