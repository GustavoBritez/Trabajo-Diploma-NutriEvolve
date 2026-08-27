using BLL.Perfiles;
using Services;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace UI
{
    public static class FormManager
    {
        private static Login _Login;
        private static MenuPrincipal _MenuPrincipal;
        private static GestionUsuario _gestionUsuario;
        private static Bitacora _bitacora;
        private static Perfiles _perfiles;
        private static Respaldo _respaldo;
        private static FormTurnero_DNI101 _formTurnero_DNI101;

        public static FormTurnero_DNI101 ObtenerFormTurnero_DNI101()
        {
            if (_formTurnero_DNI101 == null || _formTurnero_DNI101.IsDisposed)
            {
                _formTurnero_DNI101 = new FormTurnero_DNI101();
            }
            AplicarSeguridad(_formTurnero_DNI101);
            return _formTurnero_DNI101;
        }

        public static Login ObtenerLogin()
        {
            if (_Login == null || _Login.IsDisposed)
            {
                _Login = new Login();
            }
            return _Login;
        }

        public static Perfiles ObtenerPerfiles()
        {
            if (_perfiles == null || _perfiles.IsDisposed)
            {
                _perfiles = new Perfiles();
            }
            AplicarSeguridad(_perfiles);
            return _perfiles;
        }

        public static MenuPrincipal ObtenerMenuPrincipal()
        {
            if (_MenuPrincipal == null || _MenuPrincipal.IsDisposed)
            {
                _MenuPrincipal = new MenuPrincipal();
            }
            AplicarSeguridad(_MenuPrincipal);
            return _MenuPrincipal;
        }

        public static GestionUsuario ObtenerGestionUsuario()
        {
            if (_gestionUsuario == null || _gestionUsuario.IsDisposed)
            {
                _gestionUsuario = new GestionUsuario();

            }
            AplicarSeguridad(_gestionUsuario);
            return _gestionUsuario;
        }

        public static Bitacora ObtenerBitacora()
        {
            if (_bitacora == null || _bitacora.IsDisposed)
            {
                _bitacora = new Bitacora();
            }
            AplicarSeguridad(_bitacora);
            return _bitacora;
        }

        public static Respaldo ObtenerRespaldo()
        {
            if (_respaldo == null || _respaldo.IsDisposed)

            {
                _respaldo = new Respaldo();
            }
            AplicarSeguridad(_respaldo);
            return _respaldo;
        }

        public static void Navegar(Form formularioActual, Form formularioDestino)
        {
            try
            {
                if (formularioActual != null && !formularioActual.IsDisposed)
                {
                    formularioActual.Hide();
                }

                if (formularioDestino != null && !formularioDestino.IsDisposed)
                {
                    formularioDestino.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al navegar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void LimpiarInstancias()
        {
            if (_Login != null && !_Login.IsDisposed)
            {
                _Login.Dispose();
            }
            if (_MenuPrincipal != null && !_MenuPrincipal.IsDisposed)
            {
                _MenuPrincipal.Dispose();
            }
            if (_gestionUsuario != null && !_gestionUsuario.IsDisposed)
            {
                _gestionUsuario.Dispose();
            }

            _Login = null;
            _MenuPrincipal = null;
            _gestionUsuario = null;
        }

        // 1. El m�todo principal que llama tu pantalla
        public static void AplicarSeguridad(Form formulario)
        {
            PatenteBLL patenteBLL = new PatenteBLL();
            Dictionary<string, string> controlesRestringidos = patenteBLL.ObtenerControlesRestringidos(formulario.Name);

            // Llamamos al esc�ner profundo
            AplicarSeguridadRecursiva(formulario.Controls, controlesRestringidos);
        }

        /*private static void AplicarSeguridadRecursiva(Control.ControlCollection controles, Dictionary<string, string> controlesRestringidos)
        {
            foreach (Control control in controles)
            {
                if (controlesRestringidos.ContainsKey(control.Name))
                {
                    string permisoRequerido = controlesRestringidos[control.Name];
                    if (!ServicesSessionManager.Instancia.TienePermiso(permisoRequerido))
                    {
                        control.Visible = false;
                    }
                }
                if (control.HasChildren)
                {
                    AplicarSeguridadRecursiva(control.Controls, controlesRestringidos);
                }
            }
        }*/
        private static void AplicarSeguridadRecursiva(Control.ControlCollection controles, Dictionary<string, string> controlesRestringidos)
        {
            foreach (Control control in controles)
            {

                if (EsControlGestionadoPorPermisos(control.Name))
                {
                    // Excepción de Seguridad (Modo Rescate): El administrador siempre tiene acceso al botón de respaldo,
                    // incluso si la base de datos fue comprometida o eliminada. Para el resto de controles, se aplican permisos.
                    if (ServicesSessionManager.Instancia.EsAdministrador() && string.Equals(control.Name, "btnRespaldo", StringComparison.OrdinalIgnoreCase))
                    {
                        control.Visible = true;
                    }
                    else
                    {
                        if (controlesRestringidos.ContainsKey(control.Name))
                        {
                            string permisoRequerido = controlesRestringidos[control.Name];

                            // Evaluamos si tiene permiso (devuelve true o false)
                            bool tienePermiso = ServicesSessionManager.Instancia.TienePermiso(permisoRequerido);

                            // Si tiene permiso, lo muestra. Si no, lo oculta.
                            control.Visible = tienePermiso;
                        }
                        else
                        {
                            // Si el control debería estar gestionado por permisos pero no tiene mapeo,
                            // se oculta para no dejar accesos huérfanos cuando la tabla fue alterada.
                            control.Visible = false;
                        }
                    }

                }

                if (control.HasChildren)
                {
                    AplicarSeguridadRecursiva(control.Controls, controlesRestringidos);
                }
            }
        }

        private static bool EsControlGestionadoPorPermisos(string nombreControl)
        {
            return string.Equals(nombreControl, "btnRespaldo", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnGestionarPerfiles", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnUsuarios", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnReportes", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnSeguimiento", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnTurnos", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnAyuda", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnCambiarContrasena", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnCrear", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnEliminar", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnModificar", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnActDesact", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombreControl, "btnExportar", StringComparison.OrdinalIgnoreCase);
        }
        
        #region "Gesti�n de Permisos Din�micos (Reflection)"

        public static List<string> ObtenerFormulariosDelSistema()
        {
            List<string> nombresFormularios = new List<string>();

            PropertyInfo[] propiedades = typeof(FormManager).GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (PropertyInfo prop in propiedades)
            {
                if (typeof(Form).IsAssignableFrom(prop.PropertyType))
                {
                    nombresFormularios.Add(prop.PropertyType.Name);
                }
            }

            MethodInfo[] metodos = typeof(FormManager).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo metodo in metodos)
            {
                if (metodo.Name.StartsWith("Obtener") && typeof(Form).IsAssignableFrom(metodo.ReturnType))
                {
                    string nombreForm = metodo.ReturnType.Name;
                    if (!nombresFormularios.Contains(nombreForm))
                    {
                        nombresFormularios.Add(nombreForm);
                    }
                }
            }

            return nombresFormularios;
        }

        public static List<string> ObtenerBotonesDeFormulario(string nombreFormulario)
        {
            List<string> nombresBotones = new List<string>();

            try
            {
                Type tipoFormulario = Type.GetType($"UI.{nombreFormulario}");
                if (tipoFormulario == null) return nombresBotones;

                FieldInfo[] campos = tipoFormulario.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                foreach (FieldInfo campo in campos)
                {
                    if (typeof(Button).IsAssignableFrom(campo.FieldType) ||
                        campo.FieldType.Name == "ButtonActive" ||
                        typeof(ToolStripItem).IsAssignableFrom(campo.FieldType))
                    {
                        nombresBotones.Add(campo.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al escanear botones: " + ex.Message);
            }

            return nombresBotones;
        }

        #endregion

        #region "Graficos Botones"

        public class ButtonActive : Button
        {
            private Color _colorFondo = Color.FromArgb(0, 191, 143);
            private Color _colorTexto = Color.Black;

            public ButtonActive()
            {
                this.FlatStyle = FlatStyle.Flat;
                this.FlatAppearance.BorderSize = 0;
                this.Size = new Size(150, 45);
                this.BackColor = _colorFondo;
                this.ForeColor = _colorTexto;
                this.Cursor = Cursors.Hand;
                this.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            }

            private GraphicsPath GetCapsulePath(RectangleF rect, float radius)
            {
                GraphicsPath path = new GraphicsPath();
                float diameter = radius * 2;

                path.StartFigure();
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Width - diameter + rect.X, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Width - diameter + rect.X, rect.Height - diameter + rect.Y, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Height - diameter + rect.Y, diameter, diameter, 90, 90);
                path.CloseFigure();

                return path;
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                base.OnPaint(pevent);

                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                RectangleF rectSuperficie = new RectangleF(0, 0, this.Width, this.Height);

                float raddioBorde = this.Height / 2F;

                using (GraphicsPath pathSuperficie = GetCapsulePath(rectSuperficie, raddioBorde))
                using (Brush brushFondo = new SolidBrush(this.BackColor))
                {
                    this.Region = new Region(pathSuperficie);

                    pevent.Graphics.FillPath(brushFondo, pathSuperficie);
                }

                TextRenderer.DrawText(
                    pevent.Graphics,
                    this.Text,
                    this.Font,
                    this.ClientRectangle,
                    this.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                this.Invalidate();
            }
        }
        #endregion
    }
}