using BLL;
using BLL.Perfiles;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace UI
{
    public partial class FrmCrearPermiso : Form, IIdiomaObserver
    {
        public enum ModoFormulario
        {
            Permiso,
            Perfil,
            Familia
        }

        public string NombrePermiso { get; private set; }
        public string NombreFormulario { get; private set; }
        public string NombreBoton { get; private set; }
        public bool TieneBotonAsignado { get; private set; }

        private IdiomaBLL idiomaBLL = new IdiomaBLL();
        private ModoFormulario _modoActual;

        public FrmCrearPermiso(ModoFormulario modo)
        {
            InitializeComponent();

            _modoActual = modo; // Guardamos el modo

            // 1. PRIMERO traducimos los textos base de la pantalla
            ServicesSessionManager.Instancia.Suscribir(this);
            ActualizarIdioma();

            // 2. DESPUÉS adaptamos los títulos para que el idioma no los pise
            ConfigurarVisualmente();

            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void ConfigurarVisualmente()
        {
            switch (_modoActual)
            {
                case ModoFormulario.Permiso:

                    lblTitulo.Text = idiomaBLL.Traducir("lblTituloPermiso");
                    lblNombrePermiso.Text = idiomaBLL.Traducir("lblNombrePermisoPermiso");
                    lblFormulario.Text = idiomaBLL.Traducir("lblFormularioPermiso");

                    break;

                case ModoFormulario.Perfil:

                    lblTitulo.Text = idiomaBLL.Traducir("lblTituloPerfil");
                    lblNombrePermiso.Text = idiomaBLL.Traducir("lblNombrePermisoPerfil");

                    OcultarCombosParaPerfilesYFamilias();

                    break;

                case ModoFormulario.Familia:

                    lblTitulo.Text = idiomaBLL.Traducir("lblTituloFamilia");
                    lblNombrePermiso.Text = idiomaBLL.Traducir("lblNombrePermisoFamilia");

                    OcultarCombosParaPerfilesYFamilias();

                    break;
            }
        }

        private void OcultarCombosParaPerfilesYFamilias()
        {
            // Ocultamos los combos y sus etiquetas porque no aplican a Perfiles/Familias
            lblFormulario.Visible = false;
            cmbFormularios.Visible = false;
            lblBoton.Visible = false;
            cmbBotones.Visible = false;

            // Subimos los botones de Aceptar y Cancelar para que queden prolijos
            btnCancelar.Location = new Point(btnCancelar.Location.X, 185);
            btnAceptar.Location = new Point(btnAceptar.Location.X, 185);
        }

        private void FrmCrearPermiso_Load(object sender, EventArgs e)
        {
            try
            {
                // Solo cargamos los formularios de Reflection si estamos en modo Permiso
                if (_modoActual == ModoFormulario.Permiso)
                {
                    List<string> formularios = FormManager.ObtenerFormulariosDelSistema();
                    cmbFormularios.DataSource = formularios;
                    cmbFormularios.SelectedIndex = -1; // Empezar vacío
                    cmbBotones.Enabled = false;        // Deshabilitado hasta que elija un Form
                }
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_cargar_formularios", "msg_error_cargar_formularios", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        // Cada vez que cambie la pantalla seleccionada, cargamos sus botones específicos
        private void cmbFormularios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFormularios.SelectedIndex != -1)
            {
                string formSeleccionado = cmbFormularios.SelectedItem.ToString();
                List<string> botones = FormManager.ObtenerBotonesDeFormulario(formSeleccionado);

                cmbBotones.DataSource = botones;
                cmbBotones.SelectedIndex = -1;
                cmbBotones.Enabled = true;
            }
            else
            {
                cmbBotones.DataSource = null;
                cmbBotones.Enabled = false;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // 1. Validamos que el nombre no esté vacío sin importar el modo
            if (string.IsNullOrWhiteSpace(txtNombrePermiso.Text))
            {
                idiomaBLL.MostrarMensaje("msg_nombre_vacio", "titulo_nombre_vacio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Si es modo Permiso, LOS COMBOS SON OBLIGATORIOS
            if (_modoActual == ModoFormulario.Permiso)
            {
                if (cmbFormularios.SelectedIndex == -1 || cmbBotones.SelectedIndex == -1)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_pantalla_boton", "titulo_sel_pantalla_boton", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                NombreFormulario = cmbFormularios.SelectedItem.ToString();
                NombreBoton = cmbBotones.SelectedItem.ToString();
                TieneBotonAsignado = true;
            }
            else
            {
                // Si es perfil o familia, ignoramos los combos y le avisamos al padre que no hay botón
                TieneBotonAsignado = false;
            }

            // 3. Todo correcto: guardamos y cerramos
            NombrePermiso = txtNombrePermiso.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                Traducir(this.Controls);
                ConfigurarVisualmente();
            }
        }

        private void Traducir(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (!string.IsNullOrEmpty(control.Name))
                {
                    string traduccion = idiomaBLL.Traducir(control.Name);

                    if (traduccion != control.Name)
                        control.Text = traduccion;
                }

                if (control.HasChildren)
                    Traducir(control.Controls);
            }
        }
    }
}