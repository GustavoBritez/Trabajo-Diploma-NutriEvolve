using BE;
using BLL;
using Services;
using System.Windows.Forms;

namespace UI
{
    public partial class MenuPrincipal : Form, IIdiomaObserver
    {
        private readonly UsuarioBLL usuarioBLL = new UsuarioBLL();
        private readonly ServicioBcrypt servicioB = new();

        private IdiomaBLL idiomaBLL = new IdiomaBLL();
        private GroupBox gbDV;
        private Label lblDVEtiquetaMenu;
        private Button btnVerificarDVMenu;
        private Button btnRecalcularDVMenu;

        public MenuPrincipal()
        {
            InitializeComponent();
            InicializarPanelDV();

            //this.Load += (s, e) => Form1_Load();
            //this.Shown += (s, e) => Form1_Shown();
            this.VisibleChanged += (s, e) => Form1_VisibleChanged();

            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;

            ServicesSessionManager.Instancia.Suscribir(this);
            ActualizarIdioma();
            ActualizarPanelDV();
        }

        private void ActualizarDisponibilidadBotones()
        {
            try
            {
                UsuarioBE usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();
                bool tieneSession = usuarioActivo != null;
                bool baseCorrupta = ServicesSessionManager.Instancia.BaseDatosCorruptaDetectada;

                if (baseCorrupta)
                {
                    btnTurnos.Enabled = false;
                    btnSeguimiento.Enabled = false;
                    btnReportes.Enabled = false;
                    btnUsuarios.Enabled = false;
                    btnAyuda.Enabled = false;
                    btnCambiarContrasena.Enabled = false;
                    btnGestionarPerfiles.Enabled = false;

                    btnRespaldo.Enabled = true;

                    btnLogin.Enabled = true;
                    btnLogout.Enabled = true;
                }
                else
                {
                    if (tieneSession)
                    {
                        btnTurnos.Enabled = true;
                        btnSeguimiento.Enabled = true;
                        btnReportes.Enabled = true;
                        btnUsuarios.Enabled = true;
                        btnAyuda.Enabled = true;
                        btnCambiarContrasena.Enabled = true;
                        btnRespaldo.Enabled = true;
                        btnGestionarPerfiles.Enabled = true;

                        btnLogout.Visible = true;
                        btnLogout.Enabled = true;
                    }
                    else
                    {
                        btnTurnos.Enabled = false;
                        btnSeguimiento.Enabled = false;
                        btnReportes.Enabled = false;
                        btnUsuarios.Enabled = false;
                        btnAyuda.Enabled = false;
                        btnCambiarContrasena.Enabled = false;
                        btnRespaldo.Enabled = false;
                        btnGestionarPerfiles.Enabled = false;

                        btnLogout.Visible = false;
                    }

                    btnLogin.Visible = true;
                    btnLogin.Enabled = true;
                }
            }
            catch
            {
                // Manejo de error si es necesario
            }
        }

        private void Form1_VisibleChanged()
        {
            ApuntarComboBox();
            ActualizarDisponibilidadBotones();
            ActualizarUsuario();
            ActualizarPanelDV();
        }

        private void InicializarPanelDV()
        {
            gbDV = new GroupBox();
            lblDVEtiquetaMenu = new Label();
            btnVerificarDVMenu = new Button();
            btnRecalcularDVMenu = new Button();

            gbDV.Name = "gbDV";
            gbDV.Text = "Digito Verificador";
            gbDV.Visible = false;
            gbDV.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            gbDV.Size = new Size(860, 120);
            gbDV.Location = new Point(25, 120);
            gbDV.BackColor = Color.FromArgb(225, 240, 228);

            lblDVEtiquetaMenu.Name = "lblDVEtiquetaMenu";
            lblDVEtiquetaMenu.AutoSize = true;
            lblDVEtiquetaMenu.Font = new Font("Segoe UI", 10F);
            lblDVEtiquetaMenu.Location = new Point(25, 33);
            lblDVEtiquetaMenu.Text = "Verifique la consistencia o fuerce el recálculo de todos los DV.";

            btnVerificarDVMenu.Name = "btnVerificarDV";
            btnVerificarDVMenu.BackColor = Color.FromArgb(40, 120, 60);
            btnVerificarDVMenu.FlatStyle = FlatStyle.Flat;
            btnVerificarDVMenu.ForeColor = Color.White;
            btnVerificarDVMenu.Location = new Point(25, 65);
            btnVerificarDVMenu.Size = new Size(170, 38);
            btnVerificarDVMenu.Text = "Verificar DV";
            btnVerificarDVMenu.UseVisualStyleBackColor = false;
            btnVerificarDVMenu.Click += btnVerificarDVMenu_Click;

            btnRecalcularDVMenu.Name = "btnRecalcularDV";
            btnRecalcularDVMenu.BackColor = Color.FromArgb(46, 94, 67);
            btnRecalcularDVMenu.FlatStyle = FlatStyle.Flat;
            btnRecalcularDVMenu.ForeColor = Color.White;
            btnRecalcularDVMenu.Location = new Point(210, 65);
            btnRecalcularDVMenu.Size = new Size(190, 38);
            btnRecalcularDVMenu.Text = "Recalcular DV";
            btnRecalcularDVMenu.UseVisualStyleBackColor = false;
            btnRecalcularDVMenu.Click += btnRecalcularDVMenu_Click;

            gbDV.Controls.Add(lblDVEtiquetaMenu);
            gbDV.Controls.Add(btnVerificarDVMenu);
            gbDV.Controls.Add(btnRecalcularDVMenu);
            panelContenedor.Controls.Add(gbDV);
            gbDV.BringToFront();
        }

        private void ActualizarPanelDV()
        {
            bool mostrarDV = ServicesSessionManager.Instancia.EsAdministrador()
                && ServicesSessionManager.Instancia.BaseDatosCorruptaDetectada;

            gbDV.Visible = mostrarDV;
            if (mostrarDV)
            {
                gbDV.BringToFront();
            }
        }

        private void btnVerificarDVMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DigitoVerificadorBLL digitoVerificadorBLL = new DigitoVerificadorBLL();

                bool consistente = digitoVerificadorBLL.VerificarBaseDatos();

                if (consistente)
                {
                    MessageBox.Show(
                        "Los DV de la base de datos son consistentes. El sistema se encuentra íntegro.",
                        "Verificación DV",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    List<string> corruptos = digitoVerificadorBLL.ObtenerUsuariosCorruptos();
                    string detalleCorruptos = corruptos.Count > 0
                        ? $"\n\nRegistros alterados detectados en la tabla Usuarios:\n- {string.Join("\n- ", corruptos)}"
                        : "\n\nSe detectaron alteraciones en otras tablas del sistema.";

                    MessageBox.Show(
                        $"Se detectaron inconsistencias en la base de datos.{detalleCorruptos}\n\nPor favor, utilice la opción 'Recalcular DV' para restaurar el sistema.",
                        "Alerta de Seguridad",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnRecalcularDVMenu_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "ATENCIÓN: Se recalcularán y persistirán todos los DV (Individuales y Globales) de la base de datos. ¿Desea continuar?",
                "Confirmación de Recálculo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado != DialogResult.Yes)
            {
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                DigitoVerificadorBLL digitoVerificadorBLL = new DigitoVerificadorBLL();

                digitoVerificadorBLL.ActualizarDVIndividualesUsuarios();
                digitoVerificadorBLL.RecalcularYPersistir();

                ServicesSessionManager.Instancia.RegistrarEstadoIntegridad(false);

                MessageBox.Show(
                    "Los DV fueron recalculados correctamente. La integridad ha sido restaurada.",
                    "Recalcular DV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ServicesSessionManager.Instancia.Logout();
                FormManager.Navegar(this, FormManager.ObtenerLogin());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
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

        private void ActualizarUsuario()
        {
            // Guardamos el usuario en una variable para no llamar a la Instancia tantas veces
            var usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();

            if (usuarioActivo != null)
            {
                // Traducimos el ID numérico a un texto legible para la interfaz
                string nombrePerfil = "";
                switch (usuarioActivo._IdPerfil)
                {
                    case 1:
                        nombrePerfil = "Administrador";
                        break;
                    case 2:
                        nombrePerfil = "Usuario";
                        break;
                    case 3:
                        nombrePerfil = "Médico";
                        break;
                    default:
                        nombrePerfil = $"Perfil {usuarioActivo._IdPerfil}";
                        break;
                }

                this.label6.Text = $"{usuarioActivo._NombreDeUsuario} || {nombrePerfil}";
            }
            else
            {
                this.label6.Text = ""; // O string.Empty
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBE usuarioActual = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();

                Idioma id = ServicesSessionManager.Instancia.ObtenerIdioma();
                usuarioActual._Idioma = id.Nombre;

                usuarioBLL.CambioDeIdiomaUser(usuarioActual);

                // 1. Modificaciones en la base de datos
                usuarioBLL.CambioDeIdiomaUser(usuarioActual);
                usuarioBLL.LogOut(usuarioActual);


                // =========================================================
                // 2. ACTUALIZACIÓN DEL DÍGITO VERIFICADOR (El parche clave)
                // =========================================================
                DigitoVerificadorBLL dvBll = new DigitoVerificadorBLL();
                dvBll.ActualizarDVIndividualesUsuarios(); // Actualiza la fila del usuario modificado
                dvBll.RecalcularYPersistir();             // Actualiza las firmas globales
                                                          // =========================================================

             


                idiomaBLL.MostrarMensaje("msg_cerrar_sesion", "titulo_cerrar_sesion", MessageBoxButtons.OK, MessageBoxIcon.Information);
               

                ActualizarUsuario();

                List<Idioma> idiomas = idiomaBLL.ObtenerIdiomas();
                Idioma español = idiomas.First(i => i.Codigo == "es");
                ServicesSessionManager.Instancia.CambiarIdioma(español);

                FormManager.Navegar(this, FormManager.ObtenerLogin());
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_cerrar_sesion", "titulo_error_cerrar_sesion", MessageBoxButtons.OK, MessageBoxIcon.Information, ex.Message);
            }
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            UsuarioBE usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();
            if (usuarioActivo == null)
            {
                idiomaBLL.MostrarMensaje("msg_error_nosesion", "titulo_error_nosesion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormManager.Navegar(this, FormManager.ObtenerGestionUsuario());
        }

        private void btnBitacora_Click(object sender, EventArgs e)
        {
            FormManager.Navegar(this, FormManager.ObtenerBitacora());
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FormManager.Navegar(this, FormManager.ObtenerLogin());
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string nuevaPass = txtNewPass.Text;
                string repPass = txtRepPass.Text;
                string actualPass = txtActualPass.Text;

                if (string.IsNullOrEmpty(txtNewPass.Text) || string.IsNullOrEmpty(txtRepPass.Text) || string.IsNullOrEmpty(txtActualPass.Text))
                {
                    
                    idiomaBLL.MostrarMensaje("msg_campos_vacios", "titulo_campos_vacios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // Validar que las contraseñas coincidan
                if (nuevaPass != repPass)
                {
                   
                    idiomaBLL.MostrarMensaje("msg_contra_distinta", "titulo_contra_distinta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                UsuarioBE usuario = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();

                bool boleano = servicioB.ValidarContraseña(actualPass, usuario._Contraseña);

                bool boleano2 = servicioB.ValidarContraseña(nuevaPass, usuario._Contraseña);

                // Validar que no sea igual a la contraseña anterior
                if (boleano && boleano2)
                {
                    
                    idiomaBLL.MostrarMensaje("msg_samecontra", "titulo_samecontra", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string hashnuevaPass = servicioB.HashearContraseña(nuevaPass);

                usuario._Contraseña = hashnuevaPass;
                usuarioBLL.CambiarContraseña(usuario);

                idiomaBLL.MostrarMensaje("msg_contra_cambiada", "titulo_contra_cambiada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_cambio", "titulo_error_cambio", MessageBoxButtons.OK, MessageBoxIcon.Information,ex.Message);
            }
            finally
            {
                txtNewPass.Text = "";
                txtRepPass.Text = "";
                txtActualPass.Text = "";
                ChangePassPanel.Visible = false;
            }
        }
        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            ChangePassPanel.Visible = !ChangePassPanel.Visible;
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNewPass.Text = "";
            txtRepPass.Text = "";
            ChangePassPanel.Visible = false;
        }

        #region Idioma No tocar
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
        private void cmdIdioma_SelectedIndexChanged(object sender, EventArgs e)
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
        #endregion

        private void btnRespaldo_Click(object sender, EventArgs e)
        {
            FormManager.Navegar(this, FormManager.ObtenerRespaldo());
        }


        private void btnGestionarPerfiles_Click(object sender, EventArgs e)
        {
            FormManager.Navegar(this, new Perfiles());
        }

        /*
        private void btnGestionarPerfiles_Click(object sender, EventArgs e)
        {
            FormManager.Navegar(this, new Perfiles());
        }
        */
    }

}
