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
        private UI.Controles.BotonFuturista btnVerificarDVMenu;
        private UI.Controles.BotonFuturista btnRecalcularDVMenu;

        public MenuPrincipal()
        {
            InitializeComponent();
            InicializarPanelDV();

            if (panelContenedor != null)
            {
                panelContenedor.AutoScroll = true;
            }

            this.VisibleChanged += (s, e) => Form1_VisibleChanged();

            TraductorUI.SuscribirFormulario(this, this);
            TraductorUI.ConfigurarComboIdiomas(cmbIdioma, idiomaBLL);
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
            btnVerificarDVMenu = new UI.Controles.BotonFuturista();
            btnRecalcularDVMenu = new UI.Controles.BotonFuturista();

            gbDV.Name = "gbDV";
            gbDV.Text = "Dígito Verificador (Integridad de Base de Datos)";
            gbDV.Visible = false;
            gbDV.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            gbDV.Location = new Point(25, 25);
            gbDV.Size = new Size(Math.Max(400, panelContenedor.ClientSize.Width - 50), 125);
            gbDV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbDV.BackColor = Color.FromArgb(225, 240, 228);

            lblDVEtiquetaMenu.Name = "lblDVEtiquetaMenu";
            lblDVEtiquetaMenu.AutoSize = true;
            lblDVEtiquetaMenu.Font = new Font("Segoe UI", 10F);
            lblDVEtiquetaMenu.Location = new Point(25, 33);
            lblDVEtiquetaMenu.Text = "Verifique la consistencia o fuerce el recálculo de todos los DV.";

            btnVerificarDVMenu.Name = "btnVerificarDV";
            btnVerificarDVMenu.ColorPrincipal = Color.FromArgb(40, 120, 60);
            btnVerificarDVMenu.ColorSecundario = Color.FromArgb(30, 85, 45);
            btnVerificarDVMenu.ColorGlow = Color.DarkSeaGreen;
            btnVerificarDVMenu.RadioBorde = 38;
            btnVerificarDVMenu.Location = new Point(25, 65);
            btnVerificarDVMenu.Size = new Size(170, 38);
            btnVerificarDVMenu.Text = "Verificar DV";
            btnVerificarDVMenu.Click += btnVerificarDVMenu_Click;

            btnRecalcularDVMenu.Name = "btnRecalcularDV";
            btnRecalcularDVMenu.ColorPrincipal = Color.FromArgb(46, 94, 67);
            btnRecalcularDVMenu.ColorSecundario = Color.FromArgb(32, 68, 48);
            btnRecalcularDVMenu.ColorGlow = Color.DarkSeaGreen;
            btnRecalcularDVMenu.RadioBorde = 38;
            btnRecalcularDVMenu.Location = new Point(210, 65);
            btnRecalcularDVMenu.Size = new Size(190, 38);
            btnRecalcularDVMenu.Text = "Recalcular DV";
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

                var alteradas = digitoVerificadorBLL.ObtenerTablasAlteradas();

                if (alteradas.Count == 0)
                {
                    MessageBox.Show(
                        "Los DV de la base de datos son consistentes. El sistema se encuentra íntegro.",
                        "Verificación DV",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Hubo Cambios en la Tabla {string.Join(", ", alteradas)}\n\nPor favor, utilice la opción 'Recalcular DV' para restaurar el sistema.",
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
            TraductorUI.SincronizarComboIdioma(cmbIdioma);
        }

        private void ActualizarUsuario()
        {
            var usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();

            if (usuarioActivo != null)
            {
                string nombrePerfil = "";
                switch (usuarioActivo._IdPerfil)
                {
                    case 1:
                        nombrePerfil = idiomaBLL.Traducir("rol_admin");
                        break;
                    case 2:
                        nombrePerfil = idiomaBLL.Traducir("rol_user");
                        break;
                    case 3:
                        nombrePerfil = idiomaBLL.Traducir("rol_medico");
                        break;
                    default:
                        nombrePerfil = $"Perfil {usuarioActivo._IdPerfil}";
                        break;
                }

                this.label6.Text = $"{usuarioActivo._NombreDeUsuario} || {nombrePerfil}";
            }
            else
            {
                this.label6.Text = "";
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
        private void panelContenedor_Resize(object? sender, EventArgs e)
        {
            CentrarPanelCambioPass();
            if (gbDV != null && panelContenedor != null)
            {
                gbDV.Width = Math.Max(400, panelContenedor.ClientSize.Width - 50);
            }
        }

        private void CentrarPanelCambioPass()
        {
            if (ChangePassPanel != null && panelContenedor != null)
            {
                ChangePassPanel.Location = new Point(
                    Math.Max(20, (panelContenedor.ClientSize.Width - ChangePassPanel.Width) / 2),
                    Math.Max(20, (panelContenedor.ClientSize.Height - ChangePassPanel.Height) / 2)
                );
            }
        }

        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            ChangePassPanel.Visible = !ChangePassPanel.Visible;
            if (ChangePassPanel.Visible)
            {
                CentrarPanelCambioPass();
                ChangePassPanel.BringToFront();
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNewPass.Text = "";
            txtRepPass.Text = "";
            ChangePassPanel.Visible = false;
        }

        #region Idioma
        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirFormulario(this, idiomaBLL);

                if (gbDV != null)
                {
                    if (idiomaBLL.ExisteTraduccion("gbDV")) gbDV.Text = idiomaBLL.Traducir("gbDV");
                    if (lblDVEtiquetaMenu != null && idiomaBLL.ExisteTraduccion("lblDVEtiquetaMenu")) lblDVEtiquetaMenu.Text = idiomaBLL.Traducir("lblDVEtiquetaMenu");
                    if (btnVerificarDVMenu != null && idiomaBLL.ExisteTraduccion("btnVerificarDVMenu")) btnVerificarDVMenu.Text = idiomaBLL.Traducir("btnVerificarDVMenu");
                    if (btnRecalcularDVMenu != null && idiomaBLL.ExisteTraduccion("btnRecalcularDVMenu")) btnRecalcularDVMenu.Text = idiomaBLL.Traducir("btnRecalcularDVMenu");
                }

                ActualizarUsuario();
                TraductorUI.SincronizarComboIdioma(cmbIdioma);

                if (_frmTurneroContenido != null && !_frmTurneroContenido.IsDisposed && _frmTurneroContenido is IIdiomaObserver obsTurnero)
                {
                    obsTurnero.ActualizarIdioma();
                }
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

        private frmTurnero_DNI101? _frmTurneroContenido;

        private void btnTurnos_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBE usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();
                if (usuarioActivo == null)
                {
                    idiomaBLL.MostrarMensaje("msg_error_nosesion", "titulo_error_nosesion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MostrarTurneroEnContenedor();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la gestión de turnos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void MostrarTurneroEnContenedor()
        {
            if (_frmTurneroContenido == null || _frmTurneroContenido.IsDisposed)
            {
                _frmTurneroContenido = new frmTurnero_DNI101();
            }

            if (ChangePassPanel != null)
            {
                ChangePassPanel.Visible = false;
            }

            panelContenedor.Controls.Clear();
            _frmTurneroContenido.TopLevel = false;
            _frmTurneroContenido.FormBorderStyle = FormBorderStyle.None;
            _frmTurneroContenido.Dock = DockStyle.Fill;
            _frmTurneroContenido.AutoScroll = true;
            panelContenedor.Controls.Add(_frmTurneroContenido);
            _frmTurneroContenido.Show();
            _frmTurneroContenido.BringToFront();
            _frmTurneroContenido.CargarTurnos();
        }
    }

}
