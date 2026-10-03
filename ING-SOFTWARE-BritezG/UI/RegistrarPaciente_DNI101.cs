using BE;
using BLL;
using Services;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class RegistrarPaciente_DNI101 : Form, IIdiomaObserver
    {
        private readonly PacienteBLL_DNI101 _pacienteBLL = new();
        private readonly IdiomaBLL _idiomaBLL = new();

        public string DniNiñoRegistrado => txtDniNiño.Text.Trim();

        public RegistrarPaciente_DNI101(string dniNiñoInicial = "")
        {
            InitializeComponent();
            cmbObraSocial.SelectedIndex = 0;

            if (!string.IsNullOrWhiteSpace(dniNiñoInicial))
            {
                txtDniNiño.Text = dniNiñoInicial.Trim();
                txtNombre.Focus();
            }

            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirFormulario(this, _idiomaBLL);
            }
        }

        private void btnRegistrarPaciente_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string dniNiño = txtDniNiño.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string email = txtEmail.Text.Trim();
            string? obraSocial = cmbObraSocial.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                _idiomaBLL.MostrarMensaje("msg_ingrese_nombre_paciente", "titulo_campo_requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                _idiomaBLL.MostrarMensaje("msg_ingrese_apellido_paciente", "titulo_campo_requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(dniNiño))
            {
                _idiomaBLL.MostrarMensaje("msg_ingrese_dni_paciente", "titulo_campo_requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDniNiño.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(obraSocial))
            {
                _idiomaBLL.MostrarMensaje("msg_seleccione_obra_social", "titulo_campo_requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbObraSocial.Focus();
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                int idPaciente = _pacienteBLL.RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial);

                if (idPaciente > 0)
                {
                    _idiomaBLL.MostrarMensaje(
                        "msg_paciente_registrado_det",
                        "titulo_paciente_registrado_ok",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        apellido,
                        nombre,
                        dniNiño,
                        obraSocial ?? "");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    _idiomaBLL.MostrarMensaje("msg_no_pudo_registrar_paciente", "titulo_error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _idiomaBLL.MostrarMensaje("msg_error_registrar_paciente", "titulo_error_registrar_paciente", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelarPaciente_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
