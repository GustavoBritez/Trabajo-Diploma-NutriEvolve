using BE;
using BLL;
using BLL.Perfiles;
using Microsoft.VisualBasic;
using Services;
using Services.Perfiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class GestionUsuario : Form, IIdiomaObserver
    {
        private UsuarioBLL usuarioBLL = new UsuarioBLL();
        private string _modoActual = "";
        private UsuarioBE _usuarioEnModificacion = null;
        private PerfilBLL perfilBLL = new();
        private IdiomaBLL idiomaBLL = new IdiomaBLL();
        /// Atributo nuevo 
        private List<Perfil> _listaTodosLosPerfiles = new List<Perfil>();

        public GestionUsuario()
        {
            InitializeComponent();

            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            ServicesSessionManager.Instancia.Suscribir(this);
            ActualizarIdioma();

            this.VisibleChanged += (s, e) =>
            {
                if (this.Visible)
                {
                    CargarPerfiles();
                    ApuntarComboBox();
                }
            };

            GestionUsuarios_Load(null, null);
            rbMostrarActivos.CheckedChanged += RbMostrar_CheckedChanged;
            rbMostrarInactivos.CheckedChanged += RbMostrar_CheckedChanged;

            dgvUsuarios.CellFormatting += dgvUsuarios_CellFormatting;
        }
        private void CargarPerfiles()
        {
            cmbRol.Items.Clear();
            _listaTodosLosPerfiles.Clear();

            foreach (Perfil pe in perfilBLL.ObtenerPerfiles())
            {
                cmbRol.Items.Add(pe.Nombre);
                _listaTodosLosPerfiles.Add(pe);
            }
        }
        private void RbMostrar_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFiltroEstado();
        }

        private void AplicarFiltroEstado()
        {
            try
            {
                List<UsuarioBE> todosusuarios = usuarioBLL.ListarUsuarios();
                List<UsuarioBE> usuariosFiltrados;

                if (rbMostrarActivos.Checked)
                {
                    usuariosFiltrados = todosusuarios.Where(u => u._Estado == true).ToList();
                }
                else if (rbMostrarInactivos.Checked)
                {
                    usuariosFiltrados = todosusuarios.Where(u => u._Estado == false).ToList();
                }
                else
                {
                    usuariosFiltrados = todosusuarios;
                }

                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = usuariosFiltrados;

                ConfigurarColumnasDataGridView();
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_mostrar", "titulo_error_mostrar", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 1)
            {
                UsuarioBE usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as UsuarioBE;
                if (usuarioSeleccionado != null)
                {
                    CargarCamposDelUsuario(usuarioSeleccionado); // Cheken que atualizo los txt


                    if (_modoActual == "CambiarContrasena")
                    {
                        CambiarContrasenaDelUsuario(usuarioSeleccionado);
                    }
                }
            }
        }

        private void CargarCamposDelUsuario(UsuarioBE usuario)
        {
            txtDni.Text = usuario._Dni.ToString();
            txtNombre.Text = usuario._Nombre;
            txtApellido.Text = usuario._Apellido;
            txtNombreUsuario.Text = usuario._NombreDeUsuario;
            CKB_Desactivar.Checked = !usuario._Estado;
            CKB_Activar.Checked = usuario._Estado;

            var perfilEncontrado = _listaTodosLosPerfiles.FirstOrDefault(p => p.Id == usuario._IdPerfil);

            if (perfilEncontrado != null)
            {
                cmbRol.SelectedItem = perfilEncontrado.Nombre;
            }
            else
            {
                cmbRol.SelectedIndex = -1;
            }
        }

        private void CambiarContrasenaDelUsuario(UsuarioBE usuario)
        {
            try
            {
                string nuevaContraseña = Interaction.InputBox(
                    "Ingrese la nueva contraseña (mínimo 3 caracteres):",
                    "Cambiar Contraseña"
                );

                if (string.IsNullOrWhiteSpace(nuevaContraseña))
                {
                    
                    idiomaBLL.MostrarMensaje("msg_op_cancelada", "titulo_op_cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RestablecerModoCambiarContrasena();
                    return;
                }

                if (nuevaContraseña.Length < 3)
                {
                    idiomaBLL.MostrarMensaje("msg_faltan_caract", "titulo_error_mostrar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RestablecerModoCambiarContrasena();
                    return;
                }

                usuario._Contraseña = nuevaContraseña;
                usuarioBLL.ModificarUsuario(usuario);
                idiomaBLL.MostrarMensaje("msg_contra_cambiada", "titulo_contra_cambiada", MessageBoxButtons.OK, MessageBoxIcon.Information, usuario._NombreDeUsuario);
                RestablecerModoCambiarContrasena();
                GestionUsuarios_Load(null, null);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_mostrar", "titulo_error_mostrar", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
                RestablecerModoCambiarContrasena();
            }
        }

        private void RestablecerModoCambiarContrasena()
        {
            _modoActual = "";
            dgvUsuarios.ClearSelection();
            LimpiarCampos();


            btnCrear.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnActDesact.Enabled = true;
        }

        private void HabilitarModoCrear()
        {

            dgvUsuarios.ClearSelection();


            LimpiarCampos();


            txtDni.Enabled = true;
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            cmbRol.Enabled = true;
            txtNombreUsuario.Enabled = true;


            CKB_Desactivar.Enabled = false;
            CKB_Activar.Enabled = false;


            btnAceptarG.Visible = true;
            btnAceptarG.Enabled = true;
            btnCancelarG.Visible = true;
            btnCancelarG.Enabled = true;


            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnActDesact.Enabled = false;


            txtDni.Focus();
        }

        private void CrearUsuario()
        {
            try
            {
                if (!ValidarCamposCrear())
                {
                    return;
                }

                string _dni = txtDni.Text;
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();
                string nombreDeUsuario = txtNombreUsuario.Text.Trim();

                if (!int.TryParse(_dni, out int dni))
                {
         
                    idiomaBLL.MostrarMensaje("msg_error_dni", "titulo_error_dni", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (dni < 10000000)
                {
                    
                    idiomaBLL.MostrarMensaje("msg_error_dni_dg", "titulo_error_dni", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string contraseña = $"{nombre}{_dni}";

                string nombreRolSeleccionado = cmbRol.SelectedItem?.ToString() ?? "Usuario";

                int idPerfilReal = perfilBLL.ObtenerIdPerfilPorNombre(nombreRolSeleccionado);

                UsuarioBE nuevoUsuario = new UsuarioBE(
                    nombre: nombre,
                    apellido: apellido,
                    dni: dni,
                    nombreDeUsuario: nombreDeUsuario,
                    contraseña: contraseña,
                    idPerfil: idPerfilReal,
                    bloqueado: true,
                    estado: true,
                    idioma: "Español"
                );

                usuarioBLL.CrearUsuario(nuevoUsuario);

               
                idiomaBLL.MostrarMensaje("msg_usuario_creado", "titulo_usuario_creado", MessageBoxButtons.OK, MessageBoxIcon.Information,nombreDeUsuario,contraseña);
                CancelarOperacion();
                GestionUsuarios_Load(null, null);
            }
            catch (Exception ex)
            {
               
                idiomaBLL.MostrarMensaje("msg_error_mostrar", "titulo_error_mostrar", MessageBoxButtons.OK, MessageBoxIcon.Information, ex.Message);
            }
        }

        private bool ValidarCamposCrear()
        {
            if (string.IsNullOrWhiteSpace(txtDni.Text))
            {
                
                idiomaBLL.MostrarMensaje("msg_pido_dni", "titulo_pido_dni", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                
                idiomaBLL.MostrarMensaje("msg_pido_nombre", "titulo_pido_nombre", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                idiomaBLL.MostrarMensaje("msg_pido_apellido", "titulo_pido_apellido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
            {
                idiomaBLL.MostrarMensaje("msg_pido_nomuser", "titulo_pido_nomuser", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return false;
            }

            if (cmbRol.SelectedItem == null)
            {
                idiomaBLL.MostrarMensaje("msg_pido_rol", "titulo_pido_rol", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return false;
            }

            return true;
        }

        private void CancelarOperacion()
        {
            _modoActual = "";
            _usuarioEnModificacion = null;


            LimpiarCampos();


            btnAceptarG.Visible = false;
            btnAceptarG.Enabled = false;
            btnCancelarG.Visible = false;
            btnCancelarG.Enabled = false;

            btnCrear.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnActDesact.Enabled = true;


            txtDni.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            cmbRol.Enabled = false;
            txtNombreUsuario.Enabled = false;
            CKB_Desactivar.Enabled = false;
            CKB_Activar.Enabled = false;
        }

        private void LimpiarCampos()
        {
            txtDni.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtNombreUsuario.Clear();
            cmbRol.SelectedIndex = -1;
            CKB_Desactivar.Checked = false;
            CKB_Activar.Checked = false;
        }

        private void GestionUsuario_Load(object sender, EventArgs e)
        {
            txtDni.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            cmbRol.Enabled = false;
            txtNombreUsuario.Enabled = false;
            CKB_Desactivar.Enabled = false;
            CKB_Activar.Enabled = false;


            btnAceptarG.Visible = false;
            btnAceptarG.Enabled = false;
            btnCancelarG.Visible = false;
            btnCancelarG.Enabled = false;

            GestionUsuarios_Load(sender, e);

        }

        public void GestionUsuarios_Load(object sender, EventArgs e)
        {
            AplicarFiltroEstado();
            
        }

        private void ConfigurarColumnasDataGridView()
        {
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvUsuarios.Columns.Contains("_Contraseña"))
            {
                dgvUsuarios.Columns["_Contraseña"].Visible = false;
            }

            if (dgvUsuarios.Columns.Contains("NombrePerfil"))
            {
                dgvUsuarios.Columns["NombrePerfil"].Visible = false;
            }

            if (dgvUsuarios.Columns.Contains("_Dni"))
            {
                dgvUsuarios.Columns["_Dni"].HeaderText = "DNI";
            }

            if (dgvUsuarios.Columns.Contains("_Nombre"))
            {
                dgvUsuarios.Columns["_Nombre"].HeaderText = "Nombre";
            }

            if (dgvUsuarios.Columns.Contains("_Apellido"))
            {
                dgvUsuarios.Columns["_Apellido"].HeaderText = "Apellido";
            }

            if (dgvUsuarios.Columns.Contains("_NombreDeUsuario"))
            {
                dgvUsuarios.Columns["_NombreDeUsuario"].HeaderText = "Nombre de Usuario";
            }

            if (dgvUsuarios.Columns.Contains("_IdPerfil"))
            {
                dgvUsuarios.Columns["_IdPerfil"].HeaderText = "Perfil"; // Podés ponerle "Rol" si preferís
            }

            if (dgvUsuarios.Columns.Contains("_Bloqueado"))
            {
                dgvUsuarios.Columns["_Bloqueado"].HeaderText = "Bloqueado";
            }

            if (dgvUsuarios.Columns.Contains("_Estado"))
            {
                dgvUsuarios.Columns["_Estado"].HeaderText = "Estado";
            }
        }

        private void HabilitarModoModificar()
        {
            txtDni.Enabled = false;
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            cmbRol.Enabled = true;
            txtNombreUsuario.Enabled = true;


            CKB_Desactivar.Enabled = false;
            CKB_Activar.Enabled = false;


            btnAceptarG.Visible = true;
            btnAceptarG.Enabled = true;
            btnCancelarG.Visible = true;
            btnCancelarG.Enabled = true;


            btnCrear.Enabled = false;
            btnEliminar.Enabled = false;
            btnActDesact.Enabled = false;
        }

        private void ModificarUsuario()
        {
            try
            {
                if (_usuarioEnModificacion is null)
                {
                    idiomaBLL.MostrarMensaje("msg_error_usermod", "titulo_error_mostrar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string cambios = "";

                if (!string.IsNullOrWhiteSpace(txtNombre.Text) && _usuarioEnModificacion._Nombre != txtNombre.Text.Trim())
                {
                    cambios += $"Nombre: {_usuarioEnModificacion._Nombre} -> {txtNombre.Text.Trim()}; ";
                    _usuarioEnModificacion._Nombre = txtNombre.Text.Trim();
                }

                if (!string.IsNullOrWhiteSpace(txtApellido.Text) && _usuarioEnModificacion._Apellido != txtApellido.Text.Trim())
                {
                    cambios += $"Apellido: {_usuarioEnModificacion._Apellido} -> {txtApellido.Text.Trim()}; ";
                    _usuarioEnModificacion._Apellido = txtApellido.Text.Trim();
                }

                // Validación de Nombre de Usuario corregida
                if (!string.IsNullOrWhiteSpace(txtNombreUsuario.Text) && _usuarioEnModificacion._NombreDeUsuario != txtNombreUsuario.Text.Trim())
                {
                    // Agregamos la condición para que busque si el nombre existe en OTRO usuario (distinto DNI)
                    var u = usuarioBLL.ListarUsuarios().Find(x => x._NombreDeUsuario == txtNombreUsuario.Text.Trim() && x._Dni != _usuarioEnModificacion._Dni);
                    if (u != null)
                    {
                        throw new Exception($"Ya existe otro usuario ocupando el nombre de Usuario: {txtNombreUsuario.Text.Trim()}");
                    }

                    cambios += $"NombreUsuario: {_usuarioEnModificacion._NombreDeUsuario} -> {txtNombreUsuario.Text.Trim()}; ";
                    _usuarioEnModificacion._NombreDeUsuario = txtNombreUsuario.Text.Trim();
                }

                // CORRECCIÓN DEL ROL (De string a int)
                if (cmbRol.SelectedItem != null)
                {
                    // 1. Agarramos el texto (Ej: "Administrador")
                    string nombreRolSeleccionado = cmbRol.SelectedItem.ToString();

                    // 2. Lo traducimos a número
                    int idPerfilSeleccionado = perfilBLL.ObtenerIdPerfilPorNombre(nombreRolSeleccionado);

                    // 3. Comparamos los números enteros
                    if (_usuarioEnModificacion._IdPerfil != idPerfilSeleccionado)
                    {
                        cambios += $"IdPerfil: {_usuarioEnModificacion._IdPerfil} -> {idPerfilSeleccionado}; ";
                        _usuarioEnModificacion._IdPerfil = idPerfilSeleccionado;
                    }
                }

                usuarioBLL.ModificarUsuario(_usuarioEnModificacion);

                idiomaBLL.MostrarMensaje("msg_usuario_mod", "titulo_usuario_mod", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CancelarOperacion();
                GestionUsuarios_Load(null, null);
            }
            catch (Exception ex)
            {
              
                idiomaBLL.MostrarMensaje("msg_error_mostrar", "titulo_error_mostrar", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        #region Botones

        private void btnSalir_Click(object sender, EventArgs e)
        {
            UsuarioBE usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();

            usuarioActivo = usuarioBLL.BuscarUsuario(usuarioActivo._NombreDeUsuario);

            if (usuarioActivo != null)
            {
                PatenteBLL patenteBLL = new PatenteBLL();

                List<PatenteServices> listaPatentesActualizada = patenteBLL.ObtenerPermisosDePerfil(usuarioActivo._IdPerfil);

                List<string> nombresPermisosActualizados = listaPatentesActualizada.Select(p => p.Nombre).ToList();

                ServicesSessionManager.Instancia.CargarPermisosDelUsuario(nombresPermisosActualizados);
            }
            FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CancelarOperacion();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            _modoActual = "Crear";
            HabilitarModoCrear();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (_modoActual == "Crear")
            {
                CrearUsuario();
            }
            else if (_modoActual == "Modificar")
            {
                ModificarUsuario();
            }
        }

        private void btnGestionarPerfiles_Click(object sender, EventArgs e)
        {
            FormManager.Navegar(this, new Perfiles());
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count != 1)
                {
                    MessageBox.Show("Error: Seleccione una fila para Modificar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioBE usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as UsuarioBE;

                if (usuarioSeleccionado is null)
                {
                    MessageBox.Show("Error: No se pudo seleccionar un usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _modoActual = "Modificar";
                _usuarioEnModificacion = usuarioSeleccionado;
                HabilitarModoModificar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count != 1)
                {
                    MessageBox.Show("Error: Seleccione una fila para desbloquear/bloquear", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioBE usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as UsuarioBE;

                if (usuarioSeleccionado is null)
                {
                    MessageBox.Show("Error: No se pudo seleccionar un usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (usuarioSeleccionado._Bloqueado == false)
                {
                    MessageBox.Show("Error: El Usuario no esta bloqueado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                usuarioSeleccionado._Bloqueado = !usuarioSeleccionado._Bloqueado;
                usuarioBLL.Desbloquear(usuarioSeleccionado);

                string bloqueado = usuarioSeleccionado._Bloqueado ? "bloqueado" : "desbloqueado";
                MessageBox.Show($"Usuario '{usuarioSeleccionado._NombreDeUsuario}' {bloqueado} correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GestionUsuarios_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: No se pudo cambiar el estado del usuario. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActDesact_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count != 1)
                {
                    MessageBox.Show("Error: Seleccione una fila para Modificar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioBE usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as UsuarioBE;

                if (usuarioSeleccionado is null)
                {
                    MessageBox.Show("Error: No se pudo seleccionar un usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool estabaActivo = usuarioSeleccionado._Estado;
                usuarioBLL.CambiarEstado(usuarioSeleccionado);
                MessageBox.Show($"Usuario '{usuarioSeleccionado._NombreDeUsuario}' cambio correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: No se pudo cambiar el estado del usuario. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GestionUsuarios_Load(sender, e);
            }
        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            AplicarFiltroEstado();
        }

        #endregion

        #region
        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                Traducir(this.Controls);
            }
        }

        private void Traducir(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (!string.IsNullOrEmpty(control.Name))
                {
                    string traduccion = idiomaBLL.Traducir(control.Name);

                    if (traduccion != control.Name) // evita reemplazar si no existe la clave
                        control.Text = traduccion;
                }

                if (control.HasChildren)
                    Traducir(control.Controls);
            }
        }
        #endregion



        private void dgvUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "_IdPerfil" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int idPerfilBuscado))
                {
                    var perfilEncontrado = _listaTodosLosPerfiles.FirstOrDefault(p => p.Id == idPerfilBuscado);

                    if (perfilEncontrado != null)
                    {
                        e.Value = perfilEncontrado.Nombre;
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Idioma> idiomas = idiomaBLL.ObtenerIdiomas();

            if (cmbIdioma.SelectedItem.ToString() == "Español")
            {
                Idioma español = idiomas.First(i => i.Codigo == "es");
                ServicesSessionManager.Instancia.CambiarIdioma(español);
            }
            else if (cmbIdioma.SelectedItem.ToString() == "English")
            {
                Idioma ingles = idiomas.First(i => i.Codigo == "en");
                ServicesSessionManager.Instancia.CambiarIdioma(ingles);
            }
            else if (cmbIdioma.SelectedItem.ToString() == "Portugues")
            {
                Idioma portugues = idiomas.First(i => i.Codigo == "po");
                ServicesSessionManager.Instancia.CambiarIdioma(portugues);
            }
        }

        private void ApuntarComboBox()
        {
            string idioma = ServicesSessionManager.Instancia.ObtenerIdioma().Nombre;

            if (idioma == "Español")
            {
                cmbIdioma.SelectedIndex = 0;
            }
            else if (idioma == "English")
            {
                cmbIdioma.SelectedIndex = 1;
            }
            else if (idioma == "Portugues")
            {
                cmbIdioma.SelectedIndex = 2;
            }
        }
    }
}
