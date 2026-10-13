using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SetupApp
{
    partial class FrmSetup
    {
        private System.ComponentModel.IContainer components = null;

        // Controles principales del wizard
        private Panel pnlBanner;
        private Label lblTituloBanner;
        private Label lblSubtituloBanner;
        private Panel pnlContenedor;
        private Panel pnlPie;
        private Button btnAtras;
        private Button btnSiguiente;
        private Button btnCancelar;

        // Paso 1: Bienvenida
        private Panel pnlPaso1;
        private Label lblBienvenidaP1;
        private Label lblDescP1;

        // Paso 2: Directorio
        private Panel pnlPaso2;
        private Label lblTituloP2;
        private Label lblDescP2;
        private TextBox txtRutaDestino;
        private Button btnExaminar;
        private CheckBox chkAccesoDirecto;

        // Paso 3: SQL Server y BD
        private Panel pnlPaso3;
        private Label lblTituloP3;
        private Label lblDescP3;
        private Label lblServidorP3;
        private ComboBox cmbServidorSql;
        private Button btnBuscarInstancias;
        private GroupBox grpAuth;
        private RadioButton rdoAuthWindows;
        private RadioButton rdoAuthSql;
        private Label lblSqlUsuario;
        private TextBox txtSqlUsuario;
        private Label lblSqlPassword;
        private TextBox txtSqlPassword;
        private Label lblBDP3;
        private TextBox txtNombreBD;
        private Button btnProbarConexion;
        private Label lblEstadoConexion;
        private CheckBox chkCrearBD;
        private CheckBox chkSellarDV;

        // Paso 4: Progreso
        private Panel pnlPaso4;
        private Label lblTituloP4;
        private Label lblEstadoProgreso;
        private ProgressBar prgProgreso;
        private RichTextBox rtbLog;

        // Paso 5: Finalización
        private Panel pnlPaso5;
        private Label lblTituloP5;
        private Label lblDescP5;
        private CheckBox chkIniciarApp;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Text = "Instalador de NutriEvolve - Sistema de Gestión Nutricional";
            Size = new Size(720, 560);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            BackColor = Color.FromArgb(248, 250, 252);

            // ==================== Banner Superior ====================
            pnlBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(30, 58, 138)
            };

            lblTituloBanner = new Label
            {
                Text = "Instalación de NutriEvolve",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Location = new Point(25, 14),
                AutoSize = true
            };

            lblSubtituloBanner = new Label
            {
                Text = "Asistente de configuración de la aplicación y la base de datos",
                ForeColor = Color.FromArgb(219, 234, 254),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                Location = new Point(26, 44),
                AutoSize = true
            };

            pnlBanner.Controls.Add(lblTituloBanner);
            pnlBanner.Controls.Add(lblSubtituloBanner);

            // ==================== Pie de página ====================
            pnlPie = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 65,
                BackColor = Color.FromArgb(241, 245, 249)
            };

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(100, 34),
                Location = new Point(25, 16),
                BackColor = Color.White,
                FlatStyle = FlatStyle.System
            };
            btnCancelar.Click += BtnCancelar_Click;

            btnAtras = new Button
            {
                Text = "< Atrás",
                Size = new Size(100, 34),
                Location = new Point(480, 16),
                BackColor = Color.White,
                FlatStyle = FlatStyle.System
            };
            btnAtras.Click += BtnAtras_Click;

            btnSiguiente = new Button
            {
                Text = "Siguiente >",
                Size = new Size(110, 34),
                Location = new Point(590, 16),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.System
            };
            btnSiguiente.Click += BtnSiguiente_Click;

            pnlPie.Controls.Add(btnCancelar);
            pnlPie.Controls.Add(btnAtras);
            pnlPie.Controls.Add(btnSiguiente);

            // ==================== Contenedor Central ====================
            pnlContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 20, 30, 20)
            };

            ConstruirPaso1Designer();
            ConstruirPaso2Designer();
            ConstruirPaso3Designer();
            ConstruirPaso4Designer();
            ConstruirPaso5Designer();

            Controls.Add(pnlContenedor);
            Controls.Add(pnlPie);
            Controls.Add(pnlBanner);
        }

        private void ConstruirPaso1Designer()
        {
            pnlPaso1 = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblBienvenidaP1 = new Label
            {
                Text = "Bienvenido al Asistente de Instalación de NutriEvolve",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 15),
                AutoSize = true
            };

            lblDescP1 = new Label
            {
                Text = "Este instalador configurará en su computadora:\n\n" +
                       "  • La aplicación de escritorio NutriEvolve (Gestión Nutricional, Turnos y Seguimiento Clínico).\n" +
                       "  • La Base de Datos SQL Server ('ING') con estructura completa, tablas, vistas y procedimientos.\n" +
                       "  • Datos de prueba iniciales (evaluaciones pediátricas, curvas de percentiles OMS y usuarios).\n" +
                       "  • Dígitos Verificadores (DVH y DVV) para garantizar la integridad referencial del sistema.\n" +
                       "  • Configuración automática de la cadena de conexión personalizada para esta máquina.\n\n" +
                       "Requisitos previos requeridos:\n" +
                       "  ✓ Windows 10 / 11 (64-bit)\n" +
                       "  ✓ Microsoft SQL Server (Local, Express o LocalDB)\n" +
                       "  ✓ .NET Desktop Runtime 9.0 o superior\n\n" +
                       "Haga clic en 'Siguiente' para comenzar con la configuración.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(10, 55),
                Size = new Size(640, 290)
            };

            pnlPaso1.Controls.Add(lblBienvenidaP1);
            pnlPaso1.Controls.Add(lblDescP1);
            pnlContenedor.Controls.Add(pnlPaso1);
        }

        private void ConstruirPaso2Designer()
        {
            pnlPaso2 = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP2 = new Label
            {
                Text = "Seleccione la Carpeta de Destino",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 15),
                AutoSize = true
            };

            lblDescP2 = new Label
            {
                Text = "NutriEvolve se instalará en la siguiente carpeta. Si desea elegir otra ubicación, presione 'Examinar'.",
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(10, 50),
                AutoSize = true
            };

            string defaultPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "NutriEvolve");
            txtRutaDestino = new TextBox
            {
                Text = defaultPath,
                Location = new Point(10, 95),
                Size = new Size(510, 28)
            };

            btnExaminar = new Button
            {
                Text = "Examinar...",
                Location = new Point(530, 93),
                Size = new Size(110, 32),
                FlatStyle = FlatStyle.System
            };
            btnExaminar.Click += BtnExaminar_Click;

            chkAccesoDirecto = new CheckBox
            {
                Text = "Crear un acceso directo en el Escritorio (Desktop)",
                Checked = true,
                Location = new Point(10, 160),
                AutoSize = true
            };

            pnlPaso2.Controls.Add(lblTituloP2);
            pnlPaso2.Controls.Add(lblDescP2);
            pnlPaso2.Controls.Add(txtRutaDestino);
            pnlPaso2.Controls.Add(btnExaminar);
            pnlPaso2.Controls.Add(chkAccesoDirecto);
            pnlContenedor.Controls.Add(pnlPaso2);
        }

        private void ConstruirPaso3Designer()
        {
            pnlPaso3 = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP3 = new Label
            {
                Text = "Configuración del Servidor SQL Server y Base de Datos",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblDescP3 = new Label
            {
                Text = "Indique la instancia de SQL Server instalada en esta computadora. El instalador creará e inicializará\nla base de datos y configurará la aplicación para que se comunique directamente con ella.",
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(10, 38),
                AutoSize = true
            };

            lblServidorP3 = new Label
            {
                Text = "Servidor / Instancia SQL Server (Data Source):",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(10, 85),
                AutoSize = true
            };

            cmbServidorSql = new ComboBox
            {
                Location = new Point(10, 108),
                Size = new Size(400, 28),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cmbServidorSql.Items.AddRange(new object[] { ".", @".\SQLEXPRESS", @"localhost", @"(LocalDB)\MSSQLLocalDB" });
            cmbServidorSql.SelectedIndex = 0;

            btnBuscarInstancias = new Button
            {
                Text = "🔍 Auto-detectar",
                Location = new Point(420, 106),
                Size = new Size(130, 32),
                FlatStyle = FlatStyle.System
            };
            btnBuscarInstancias.Click += BtnBuscarInstancias_Click;

            grpAuth = new GroupBox
            {
                Text = "Tipo de Autenticación",
                Location = new Point(10, 150),
                Size = new Size(630, 95)
            };

            rdoAuthWindows = new RadioButton
            {
                Text = "Autenticación de Windows (Integrated Security - Recomendado)",
                Checked = true,
                Location = new Point(15, 24),
                AutoSize = true
            };
            rdoAuthWindows.CheckedChanged += RdoAuthWindows_CheckedChanged;

            rdoAuthSql = new RadioButton
            {
                Text = "Autenticación de SQL Server (Usuario y Contraseña)",
                Location = new Point(15, 54),
                AutoSize = true
            };
            rdoAuthSql.CheckedChanged += RdoAuthSql_CheckedChanged;

            lblSqlUsuario = new Label { Text = "Usuario:", Location = new Point(350, 56), AutoSize = true, Visible = false };
            txtSqlUsuario = new TextBox { Text = "sa", Location = new Point(410, 53), Size = new Size(80, 26), Visible = false };

            lblSqlPassword = new Label { Text = "Clave:", Location = new Point(500, 56), AutoSize = true, Visible = false };
            txtSqlPassword = new TextBox { Text = "", PasswordChar = '•', Location = new Point(545, 53), Size = new Size(75, 26), Visible = false };

            grpAuth.Controls.Add(rdoAuthWindows);
            grpAuth.Controls.Add(rdoAuthSql);
            grpAuth.Controls.Add(lblSqlUsuario);
            grpAuth.Controls.Add(txtSqlUsuario);
            grpAuth.Controls.Add(lblSqlPassword);
            grpAuth.Controls.Add(txtSqlPassword);

            lblBDP3 = new Label { Text = "Nombre de Base de Datos:", Location = new Point(10, 258), AutoSize = true };
            txtNombreBD = new TextBox { Text = "ING", Location = new Point(185, 255), Size = new Size(120, 26) };

            btnProbarConexion = new Button
            {
                Text = "⚡ Probar Conexión",
                Location = new Point(320, 252),
                Size = new Size(150, 32),
                FlatStyle = FlatStyle.System
            };
            btnProbarConexion.Click += BtnProbarConexion_Click;

            lblEstadoConexion = new Label
            {
                Text = "Haga clic en 'Probar Conexión' para validar el servidor.",
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(10, 292),
                Size = new Size(630, 22)
            };

            chkCrearBD = new CheckBox
            {
                Text = "Crear e inicializar la base de datos 'ING' ejecutando el script maestro con esquema y datos",
                Checked = true,
                Location = new Point(10, 320),
                AutoSize = true
            };

            chkSellarDV = new CheckBox
            {
                Text = "Calcular y sellar Dígitos Verificadores de integridad (DVH, DVV y global) al finalizar",
                Checked = true,
                Location = new Point(10, 345),
                AutoSize = true
            };

            pnlPaso3.Controls.Add(lblTituloP3);
            pnlPaso3.Controls.Add(lblDescP3);
            pnlPaso3.Controls.Add(lblServidorP3);
            pnlPaso3.Controls.Add(cmbServidorSql);
            pnlPaso3.Controls.Add(btnBuscarInstancias);
            pnlPaso3.Controls.Add(grpAuth);
            pnlPaso3.Controls.Add(lblBDP3);
            pnlPaso3.Controls.Add(txtNombreBD);
            pnlPaso3.Controls.Add(btnProbarConexion);
            pnlPaso3.Controls.Add(lblEstadoConexion);
            pnlPaso3.Controls.Add(chkCrearBD);
            pnlPaso3.Controls.Add(chkSellarDV);
            pnlContenedor.Controls.Add(pnlPaso3);
        }

        private void ConstruirPaso4Designer()
        {
            pnlPaso4 = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP4 = new Label
            {
                Text = "Instalando NutriEvolve...",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblEstadoProgreso = new Label
            {
                Text = "Iniciando instalación...",
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(10, 40),
                Size = new Size(630, 24)
            };

            prgProgreso = new ProgressBar
            {
                Location = new Point(10, 70),
                Size = new Size(630, 24),
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            rtbLog = new RichTextBox
            {
                Location = new Point(10, 105),
                Size = new Size(630, 260),
                ReadOnly = true,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Consolas", 9f),
                BorderStyle = BorderStyle.None
            };

            pnlPaso4.Controls.Add(lblTituloP4);
            pnlPaso4.Controls.Add(lblEstadoProgreso);
            pnlPaso4.Controls.Add(prgProgreso);
            pnlPaso4.Controls.Add(rtbLog);
            pnlContenedor.Controls.Add(pnlPaso4);
        }

        private void ConstruirPaso5Designer()
        {
            pnlPaso5 = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP5 = new Label
            {
                Text = "¡Instalación Finalizada con Éxito!",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 101, 52),
                Location = new Point(10, 15),
                AutoSize = true
            };

            lblDescP5 = new Label
            {
                Text = "NutriEvolve y la base de datos se han instalado y configurado correctamente en su equipo.\n\n" +
                       "Información de Cuentas de Acceso Predefinidas:\n\n" +
                       "  👑 Perfil Administrador:\n" +
                       "      • Usuario: admin\n" +
                       "      • Contraseña: admin123\n\n" +
                       "  🥗 Perfil Nutricionista:\n" +
                       "      • Usuario: nutri\n" +
                       "      • Contraseña: nutri123\n\n" +
                       "La cadena de conexión ha sido sellada y vinculada a la instancia de SQL Server configurada.",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(10, 60),
                Size = new Size(630, 230)
            };

            chkIniciarApp = new CheckBox
            {
                Text = "Iniciar NutriEvolve ahora",
                Checked = true,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 58, 138),
                Location = new Point(10, 310),
                AutoSize = true
            };

            pnlPaso5.Controls.Add(lblTituloP5);
            pnlPaso5.Controls.Add(lblDescP5);
            pnlPaso5.Controls.Add(chkIniciarApp);
            pnlContenedor.Controls.Add(pnlPaso5);
        }
    }
}
