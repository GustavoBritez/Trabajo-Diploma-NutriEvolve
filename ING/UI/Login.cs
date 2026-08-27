using BE;
using BLL;
using BLL.Perfiles;
using DAL;
using Services;
using Services.Perfiles;
using System.Data;


namespace UI
{
    public partial class Login : Form, IIdiomaObserver
    {
        UsuarioBLL usuarioBLL = new();
        private IdiomaBLL idiomaBLL = new IdiomaBLL();
        DigitoVerificadorBLL digitoVerificadorBLL = new();
        public Login()
        {
            InitializeComponent();
            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            ServicesSessionManager.Instancia.Suscribir(this);
            ActualizarIdioma();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
        }

        private void btnIngresar_Click(object sender, EventArgs e)
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

                string nombre = txtUsuario.Text?.Trim();
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
                DigitoVerificadorBLL digitoVerificadorBLL = new DigitoVerificadorBLL();
                bool baseDatosIntegra = digitoVerificadorBLL.VerificarBaseDatos();

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
                        // NO PONEMOS RETURN. Dejamos que el flujo continúe hacia el login.
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
                    // REGLA DE ORO: Solo recalculamos automáticamente por el ingreso a la bitácora 
                    // SI la base de datos estaba sana. Si estaba corrupta, no tocamos nada para que 
                    // el Admin pueda ver el error en el panel.
                    if (baseDatosIntegra)
                    {
                        digitoVerificadorBLL.RecalcularYPersistir();
                        ServicesSessionManager.Instancia.RegistrarEstadoIntegridad(false);
                    }

                    PatenteBLL patenteBLL = new PatenteBLL();
                    List<PatenteServices> listaPatentes = patenteBLL.ObtenerPermisosDePerfil(usuario._IdPerfil);
                    List<string> nombresPermisos = listaPatentes.Select(p => p.Nombre).ToList();
                    ServicesSessionManager.Instancia.CargarPermisosDelUsuario(nombresPermisos);

                    List<Idioma> idiomas = idiomaBLL.ObtenerIdiomas();
                    Idioma idioma = idiomas.Find(i => i.Nombre == usuario._Idioma.ToString());
                    ServicesSessionManager.Instancia.CambiarIdioma(idioma);

                   

                    //MessageBox.Show("Inicio de sesión exitoso.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    idiomaBLL.MostrarMensaje("msg_inicio_sesion","titulo_inicio_sesion",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());

                }
                else
                {
                    // (Tu lógica de intentos fallidos igual que siempre)
                    UsuarioBE usuarioDespues = usuarioBLL.BuscarUsuario(nombre);
                    if (usuarioDespues != null && usuarioDespues._Bloqueado)
                    {
                        idiomaBLL.MostrarMensaje("msg_bloquear_cuenta", "titulo_bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        int intentos = usuarioBLL.ObtenerIntentosFallidos(nombre);
                        int intentosRestantes = Math.Max(0, 3 - intentos);

                        idiomaBLL.MostrarMensaje("msg_intentos_incorrectos","titulo_intento_fallido",MessageBoxButtons.OK, MessageBoxIcon.Warning, intentos, intentosRestantes);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_login_error", "titulo_login_error", MessageBoxButtons.OK, MessageBoxIcon.Error, ex);
             
            }
        }

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

        // Solo para logearme ma rapido
        private void Login_Load(object sender, EventArgs e)
        {
            cmbIdioma.SelectedIndex = 0;
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



        private void button1_Click(object sender, EventArgs e)
        {
            DigitoVerificadorBLL dvBll = new DigitoVerificadorBLL();
            dvBll.RecalcularYPersistir();
            MessageBox.Show("Dígitos recalculados. Ya puedes iniciar sesión normalmente.");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Cambiamos el cursor al de espera por si la base de datos procesa muchos registros
                Cursor = Cursors.WaitCursor;

                // Instanciamos tu clase de lógica
                DigitoVerificadorBLL dvBll = new DigitoVerificadorBLL();

                // 1° PASO: Llenamos las columnas "DV" individuales de cada fila en la tabla Usuarios
                dvBll.ActualizarDVIndividualesUsuarios();

                // 2° PASO: Llenamos la tabla central "DV" con los totales maestros
                dvBll.RecalcularYPersistir();

                // Volvemos el cursor a la normalidad
                Cursor = Cursors.Default;

                MessageBox.Show(
                    "¡Carga Inicial Completada!\n\n" +
                    "1. Se han rellenado las columnas DV de cada usuario individual.\n" +
                    "2. Se han calculado las firmas globales en la tabla central.\n\n" +
                    "Ya puedes ir a SQL Server, modificar un registro a mano y probar si el login te bloquea.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Ocurrió un error al cargar los dígitos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
