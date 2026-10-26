using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

        public frmMaestroClientes_DNI101()
        {
            InitializeComponent();
            ConfigurarEstado(ModoOperacion.Inicio);
            CargarGrillaClientes();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
            AgregarMensaje("Sistema iniciado. Matriz de clientes lista.");
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

                    btnActualizar.Enabled = true;
                    btnAnadir.Enabled = true;
                    btnModificar.Enabled = dgvClientes.CurrentRow != null;
                    btnEliminar.Enabled = dgvClientes.CurrentRow != null;

                    btnAplicar.Enabled = false;
                    btnCancelar.Enabled = false;
                    btnSalir.Enabled = true;

                    btnGestionEmail1N.Enabled = dgvClientes.CurrentRow != null;
                    btnGestionCelular1N.Enabled = dgvClientes.CurrentRow != null;

                    btnSerializar.Enabled = true;
                    btnDesSerializar.Enabled = true;
                    btnLimpiar.Enabled = true;
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

                    btnActualizar.Enabled = false;
                    btnAnadir.Enabled = false;
                    btnModificar.Enabled = false;
                    btnEliminar.Enabled = false;

                    btnAplicar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnSalir.Enabled = false;

                    btnGestionEmail1N.Enabled = false;
                    btnGestionCelular1N.Enabled = false;

                    txtDni.Focus();
                    AgregarMensaje("Modo AÑADIR activado. Ingrese los datos del nuevo cliente.");
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

                    btnActualizar.Enabled = false;
                    btnAnadir.Enabled = false;
                    btnModificar.Enabled = false;
                    btnEliminar.Enabled = false;

                    btnAplicar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnSalir.Enabled = false;

                    btnGestionEmail1N.Enabled = true;
                    btnGestionCelular1N.Enabled = true;

                    txtApellidos.Focus();
                    AgregarMensaje($"Modo MODIFICAR activado para cliente DNI {txtDni.Text}.");
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
                if (row.Cells["IdPaciente_DNI101"] != null && row.Cells["IdPaciente_DNI101"].Value != null && int.TryParse(row.Cells["IdPaciente_DNI101"].Value.ToString(), out int id))
                {
                    _idPacienteSeleccionado = id;
                }
                else
                {
                    _idPacienteSeleccionado = 0;
                }

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
            ConfigurarEstado(ModoOperacion.Alta);
        }

        private void btnModificar_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null)
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
                MessageBox.Show("Por favor seleccione un cliente registrado en la base de datos para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    AgregarMensaje($"Baja lógica realizada para cliente DNI {txtDni.Text} ({cliente}).");
                    MessageBox.Show("Cliente dado de baja lógica correctamente.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ConfigurarEstado(ModoOperacion.Inicio);
                    CargarGrillaClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AgregarMensaje($"Error al eliminar: {ex.Message}");
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
                    AgregarMensaje($"Alta exitosa: Cliente DNI {dni} ({apellidos}, {nombres}) registrado.");
                    MessageBox.Show("Cliente agregado exitosamente con sus tablas 1:N sincronizadas.", "Alta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_modoActual == ModoOperacion.Modificacion)
                {
                    _pacienteBLL.ModificarMaestro(_idPacienteSeleccionado, dni, apellidos, nombres, email, celular, direccion);
                    AgregarMensaje($"Modificación exitosa: Cliente DNI {dni} actualizado.");
                    MessageBox.Show("Cliente modificado exitosamente con repercusión en sus contactos 1:N.", "Modificación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

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

        #region Serialización y Des-serialización XML

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

                    container.Clientes.Add(new ClienteXmlItem
                    {
                        DNI = row.Cells["colDni"].Value?.ToString() ?? "",
                        APELLIDOS = row.Cells["colApellidos"].Value?.ToString() ?? "",
                        NOMBRES = row.Cells["colNombres"].Value?.ToString() ?? "",
                        EMAIL = row.Cells["colEmail"].Value?.ToString() ?? "",
                        CELULAR = row.Cells["colCelular"].Value?.ToString() ?? "",
                        DIRECCION = row.Cells["colDireccion"].Value?.ToString() ?? ""
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

                // Subir a la matriz de datos del Cliente presente en la pantalla
                DataTable dt = new DataTable();
                dt.Columns.Add("IdPaciente_DNI101", typeof(int));
                dt.Columns.Add("DNI", typeof(string));
                dt.Columns.Add("APELLIDOS", typeof(string));
                dt.Columns.Add("NOMBRES", typeof(string));
                dt.Columns.Add("EMAIL", typeof(string));
                dt.Columns.Add("CELULAR", typeof(string));
                dt.Columns.Add("DIRECCION", typeof(string));

                foreach (var item in container.Clientes)
                {
                    dt.Rows.Add(0, item.DNI, item.APELLIDOS, item.NOMBRES, item.EMAIL, item.CELULAR, item.DIRECCION);
                }

                dgvClientes.DataSource = dt;
                if (dgvClientes.Columns["IdPaciente_DNI101"] != null)
                {
                    dgvClientes.Columns["IdPaciente_DNI101"].Visible = false;
                }

                if (dgvClientes.Rows.Count > 0)
                {
                    dgvClientes.Rows[0].Selected = true;
                    CargarCamposDesdeGrilla();
                }

                ActualizarBotonesSegunSeleccion();

                AgregarMensaje($"DES-SERIALIZACIÓN EXITOSA: {container.Clientes.Count} clientes cargados desde '{Path.GetFileName(ruta)}' a la matriz de datos.");
                MessageBox.Show(
                    $"El archivo XML se ha des-serializado correctamente.\nSe subieron {container.Clientes.Count} registros a la matriz de datos de la pantalla.",
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
            LimpiarCampos();
            txtRutaSerializar.Clear();
            txtRutaDesSerializar.Clear();
            txtMensaje.Clear();
            AgregarMensaje("Formulario y rutas limpiados por el usuario.");
        }

        #endregion
    }
}
