using BE;
using BLL;
using BLL.Perfiles;
using DAL;
using Services;
using Services.Perfiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UI
{
    public partial class Login : Form, IIdiomaObserver
    {
        UsuarioBLL usuarioBLL = new();
        private IdiomaBLL idiomaBLL = new IdiomaBLL();
        DigitoVerificadorBLL digitoVerificadorBLL = new();

        // --- MÉTODOS NATIVOS DE WINDOWS PARA BORDES REDONDEADOS Y ARRASTRE ---
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public Login()
        {
            InitializeComponent();
            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            ServicesSessionManager.Instancia.Suscribir(this);
            ActualizarIdioma();

            // Habilitamos arrastre de ventana desde los paneles
            panelIzquierdo.MouseDown += MoverVentana_MouseDown;
            panelLogin.MouseDown += MoverVentana_MouseDown;
            lblTitulo.MouseDown += MoverVentana_MouseDown;
            lblLogin.MouseDown += MoverVentana_MouseDown;
        }

        private void MoverVentana_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Aplica bordes redondeados al formulario completo
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 36, 36));
        }

        private void btnCerrarForm_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            this.Close();
            FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
        }

        private void btnIngresar_Click(object? sender, EventArgs e)
        {
            try
            {
                // 1. VALIDACIONES BÁSICAS Y DE SESIÓN
                if (ServicesSessionManager.Instancia.ObtenerUsuarioActivo() != null)
                {
                    idiomaBLL.MostrarMensaje("msg_sesion_activa", "titulo_sesion_activa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
                    return;
                }

                string nombre = txtUsuario.Text?.Trim() ?? string.Empty;
                string contraseña = txtPassword.Text ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(contraseña))
                {  
                    idiomaBLL.MostrarMensaje("msg_falta_uscon", "titulo_falta_uscon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. BUSCAMOS AL USUARIO
                UsuarioBE usuario = usuarioBLL.BuscarUsuario(nombre);

                if (usuario == null)
                {
                    idiomaBLL.MostrarMensaje("msg_inexistente_usuario", "titulo_no_usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (usuario._Bloqueado)
                {
                    idiomaBLL.MostrarMensaje("msg_cuentabloqueada", "titulo_cuentabloqueada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // 3. ESCUDO DE INTEGRIDAD 
                DigitoVerificadorBLL dvBLL = new DigitoVerificadorBLL();
                bool baseDatosIntegra = dvBLL.VerificarBaseDatos();

                if (!baseDatosIntegra)
                {
                    Services.ServicioBcrypt servicioB = new Services.ServicioBcrypt();
                    bool contraseñaCorrecta = servicioB.ValidarContraseña(contraseña, usuario._Contraseña);

                    if (usuario._IdPerfil == 1 && contraseñaCorrecta)
                    {
                        ServicesSessionManager.Instancia.RegistrarEstadoIntegridad(true);
                        MessageBox.Show(
                            "¡ALERTA! La base de datos está corrupta, pero tienes permisos de Administrador.\n\n" +
                            "Se te permitirá el ingreso. Por favor, dirígete al panel de seguridad para verificar y recalcular los dígitos verificadores.",
                            "Modo Rescate - Acceso Autorizado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Dejamos que el flujo continúe hacia el login.
                    }
                    else
                    {
                        MessageBox.Show("¡ALERTA CRÍTICA! Se ha detectado una alteración externa en la base de datos.\n" +
                                        "Por razones de seguridad, el sistema ha sido bloqueado.",
                                        "Error de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                // 4. LOGIN OFICIAL
                bool loginOK = usuarioBLL.Login(nombre, contraseña);

                if (loginOK)
                {
                    if (baseDatosIntegra)
                    {
                        dvBLL.RecalcularYPersistir();
                        ServicesSessionManager.Instancia.RegistrarEstadoIntegridad(false);
                    }

                    PatenteBLL patenteBLL = new PatenteBLL();
                    List<PatenteServices> listaPatentes = patenteBLL.ObtenerPermisosDePerfil(usuario._IdPerfil);
                    List<string> nombresPermisos = listaPatentes.Select(p => p.Nombre).ToList();
                    ServicesSessionManager.Instancia.CargarPermisosDelUsuario(nombresPermisos);

                    List<Idioma> idiomas = idiomaBLL.ObtenerIdiomas();
                    Idioma idioma = idiomas.Find(i => i.Nombre == usuario._Idioma.ToString());
                    if (idioma != null)
                    {
                        ServicesSessionManager.Instancia.CambiarIdioma(idioma);
                    }

                    idiomaBLL.MostrarMensaje("msg_inicio_sesion", "titulo_inicio_sesion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
                }
                else
                {
                    UsuarioBE usuarioDespues = usuarioBLL.BuscarUsuario(nombre);
                    if (usuarioDespues != null && usuarioDespues._Bloqueado)
                    {
                        idiomaBLL.MostrarMensaje("msg_bloquear_cuenta", "titulo_bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        int intentos = usuarioBLL.ObtenerIntentosFallidos(nombre);
                        int intentosRestantes = Math.Max(0, 3 - intentos);
                        idiomaBLL.MostrarMensaje("msg_intentos_incorrectos", "titulo_intento_fallido", MessageBoxButtons.OK, MessageBoxIcon.Warning, intentos, intentosRestantes);
                    }
                }
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_login_error", "titulo_login_error", MessageBoxButtons.OK, MessageBoxIcon.Error, ex);
            }
        }

        #region Idioma
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

        private void Login_Load(object? sender, EventArgs e)
        {
            cmbIdioma.SelectedIndex = 0;
        }

        private void cmbIdioma_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbIdioma.SelectedItem == null) return;

            List<Idioma> idiomas = idiomaBLL.ObtenerIdiomas();

            if (cmbIdioma.SelectedItem.ToString() == "Español")
            {
                Idioma español = idiomas.FirstOrDefault(i => i.Codigo == "es");
                if (español != null) ServicesSessionManager.Instancia.CambiarIdioma(español);
            }
            else if (cmbIdioma.SelectedItem.ToString() == "English")
            {
                Idioma ingles = idiomas.FirstOrDefault(i => i.Codigo == "en");
                if (ingles != null) ServicesSessionManager.Instancia.CambiarIdioma(ingles);
            }
            else if (cmbIdioma.SelectedItem.ToString() == "Portugues")
            {
                Idioma portugues = idiomas.FirstOrDefault(i => i.Codigo == "po");
                if (portugues != null) ServicesSessionManager.Instancia.CambiarIdioma(portugues);
            }
        }
    }
}
