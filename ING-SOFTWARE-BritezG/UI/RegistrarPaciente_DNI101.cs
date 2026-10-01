using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class RegistrarPaciente_DNI101 : Form
    {
        private readonly PacienteBLL_DNI101 _pacienteBLL = new();

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
                MessageBox.Show("Por favor, ingrese el nombre del paciente.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show("Por favor, ingrese el apellido del paciente.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(dniNiño))
            {
                MessageBox.Show("Por favor, ingrese el DNI del niño/a.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDniNiño.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(obraSocial))
            {
                MessageBox.Show("Por favor, seleccione una Obra Social (Medicus, OSPEP o SAO).", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbObraSocial.Focus();
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                int idPaciente = _pacienteBLL.RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial);

                if (idPaciente > 0)
                {
                    MessageBox.Show(
                        $"El paciente '{apellido}, {nombre}' con DNI {dniNiño} y Obra Social {obraSocial} fue registrado exitosamente.",
                        "Paciente Registrado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo completar el registro del paciente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar el paciente:\n{ex.Message}", "Error al Registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
