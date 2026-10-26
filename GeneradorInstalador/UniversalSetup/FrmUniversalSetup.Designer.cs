#nullable enable
#pragma warning disable CS8618
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UniversalSetup
{
    partial class FrmUniversalSetup
    {
        private System.ComponentModel.IContainer? components = null;

        // Banner Superior
        private Panel pnlBanner;
        private Label lblBannerTitle;
        private Label lblBannerSubtitle;
        private Label lblBadgeModo;

        // Barra Inferior de Navegación
        private Panel pnlBottom;
        private Button btnCancelar;
        private Button btnAnterior;
        private Button btnSiguiente;

        // Contenedor Central de Pasos
        private Panel pnlContent;

        // ===== Paso 1: Bienvenida =====
        private Panel pnlPaso1_Bienvenida;
        private Label lblTituloP1;
        private Label lblDescP1;
        private Label lblAvisoModoP1;
        private GroupBox grpInfoP1;
        private Label lblInfoApp;
        private Label lblInfoBD;
        private Label lblInfoAutor;
        private Label lblInfoRequisitos;

        // ===== Paso 2: Directorio de Instalación =====
        private Panel pnlPaso2_Ruta;
        private Label lblTituloP2;
        private Label lblDescP2;
        private Label lblRuta;
        private TextBox txtRutaInstalacion;
        private Button btnExaminarRuta;
        private CheckBox chkCrearAccesoDirecto;
        private Label lblAvisoInstalacionPreviaP2;

        // ===== Paso 3: Base de Datos y Servidor SQL =====
        private Panel pnlPaso3_BaseDatos;
        private Label lblTituloP3;
        private Label lblDescP3;
        private Label lblServidor;
        private ComboBox cboServidor;
        private Button btnProbarConexion;
        private RadioButton rdoAuthWindows;
        private RadioButton rdoAuthSql;
        private Label lblUsuarioSql;
        private TextBox txtUsuarioSql;
        private Label lblClaveSql;
        private TextBox txtClaveSql;
        private CheckBox chkInstalarBD;
        private GroupBox grpModoBDReinstalacion;
        private RadioButton rdoConservarBD;
        private RadioButton rdoRecrearBD;
        private CheckBox chkSellarDV;

        // ===== Paso 4: Progreso =====
        private Panel pnlPaso4_Progreso;
        private Label lblTituloP4;
        private Label lblEstadoP4;
        private ProgressBar progressBar;
        private TextBox txtLog;

        // ===== Paso 5: Finalizado =====
        private Panel pnlPaso5_Finalizado;
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
            Text = "Instalador / Reinstalador Universal .NET";
            Size = new Size(740, 590);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            BackColor = Color.FromArgb(248, 250, 252);

            // ==================== Banner Superior ====================
            pnlBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = Color.FromArgb(30, 58, 138)
            };

            lblBannerTitle = new Label
            {
                Text = "Asistente de Instalación / Reinstalación",
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(24, 15),
                AutoSize = true
            };

            lblBannerSubtitle = new Label
            {
                Text = "Despliegue automático de aplicación y base de datos SQL Server",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(219, 234, 254),
                Location = new Point(25, 45),
                AutoSize = true
            };

            lblBadgeModo = new Label
            {
                Text = "NUEVA INSTALACIÓN",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(16, 185, 129),
                Location = new Point(540, 25),
                Size = new Size(160, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlBanner.Controls.Add(lblBannerTitle);
            pnlBanner.Controls.Add(lblBannerSubtitle);
            pnlBanner.Controls.Add(lblBadgeModo);

            // ==================== Barra Inferior ====================
            pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 65,
                BackColor = Color.FromArgb(241, 245, 249)
            };

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(100, 36),
                Location = new Point(25, 15),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnCancelar.Click += BtnCancelar_Click;

            btnAnterior = new Button
            {
                Text = "< Anterior",
                Size = new Size(110, 36),
                Location = new Point(460, 15),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnAnterior.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnAnterior.Click += BtnAnterior_Click;

            btnSiguiente = new Button
            {
                Text = "Siguiente >",
                Size = new Size(130, 36),
                Location = new Point(580, 15),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSiguiente.FlatAppearance.BorderSize = 0;
            btnSiguiente.Click += BtnSiguiente_Click;

            pnlBottom.Controls.Add(btnCancelar);
            pnlBottom.Controls.Add(btnAnterior);
            pnlBottom.Controls.Add(btnSiguiente);

            // ==================== Contenedor Central ====================
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(25)
            };

            // ==================== Paso 1: Bienvenida ====================
            pnlPaso1_Bienvenida = new Panel { Dock = DockStyle.Fill, Visible = true };

            lblTituloP1 = new Label
            {
                Text = "Bienvenido al Instalador de la Aplicación",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblDescP1 = new Label
            {
                Text = "Este asistente configurará automáticamente la solución .NET, publicará los componentes en su equipo\ny preparará la base de datos SQL Server correspondiente.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(12, 45),
                AutoSize = true
            };

            lblAvisoModoP1 = new Label
            {
                Text = "Modo de Instalación inicial listo para comenzar.",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 185, 129),
                Location = new Point(12, 95),
                AutoSize = true
            };

            grpInfoP1 = new GroupBox
            {
                Text = " Ficha Técnica de la Solución ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, 130),
                Size = new Size(660, 180)
            };

            lblInfoApp = new Label
            {
                Text = "Aplicación Principal: Cargando...",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(20, 30),
                AutoSize = true
            };

            lblInfoBD = new Label
            {
                Text = "Base de Datos SQL: Cargando...",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(20, 65),
                AutoSize = true
            };

            lblInfoAutor = new Label
            {
                Text = "Autor / Proyecto: Alumno / Universidad",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(20, 100),
                AutoSize = true
            };

            lblInfoRequisitos = new Label
            {
                Text = "Requisitos: Windows 10/11, SQL Server (Local, Express o LocalDB) y .NET Runtime 9.0.",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 138),
                AutoSize = true
            };

            grpInfoP1.Controls.Add(lblInfoApp);
            grpInfoP1.Controls.Add(lblInfoBD);
            grpInfoP1.Controls.Add(lblInfoAutor);
            grpInfoP1.Controls.Add(lblInfoRequisitos);

            pnlPaso1_Bienvenida.Controls.Add(lblTituloP1);
            pnlPaso1_Bienvenida.Controls.Add(lblDescP1);
            pnlPaso1_Bienvenida.Controls.Add(lblAvisoModoP1);
            pnlPaso1_Bienvenida.Controls.Add(grpInfoP1);

            // ==================== Paso 2: Ruta ====================
            pnlPaso2_Ruta = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP2 = new Label
            {
                Text = "Directorio de Instalación",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblDescP2 = new Label
            {
                Text = "Seleccione la carpeta donde se copiarán los ejecutables, librerías y componentes del sistema.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(12, 45),
                AutoSize = true
            };

            lblRuta = new Label
            {
                Text = "Ruta de Destino:",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(12, 90),
                AutoSize = true
            };

            txtRutaInstalacion = new TextBox
            {
                Location = new Point(15, 115),
                Size = new Size(530, 29),
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.White
            };
            txtRutaInstalacion.TextChanged += TxtRutaInstalacion_TextChanged;

            btnExaminarRuta = new Button
            {
                Text = "Examinar...",
                Size = new Size(115, 30),
                Location = new Point(555, 114),
                Font = new Font("Segoe UI", 9.5f),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExaminarRuta.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnExaminarRuta.Click += BtnExaminarRuta_Click;

            chkCrearAccesoDirecto = new CheckBox
            {
                Text = "Crear acceso directo en el Escritorio",
                Checked = true,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 165),
                AutoSize = true
            };

            lblAvisoInstalacionPreviaP2 = new Label
            {
                Text = "Se detectó una instalación existente en esta carpeta. Se procederá con la REINSTALACIÓN.",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(194, 65, 12),
                BackColor = Color.FromArgb(254, 243, 199),
                Padding = new Padding(10, 8, 10, 8),
                Location = new Point(15, 210),
                Size = new Size(655, 40),
                Visible = false
            };

            pnlPaso2_Ruta.Controls.Add(lblTituloP2);
            pnlPaso2_Ruta.Controls.Add(lblDescP2);
            pnlPaso2_Ruta.Controls.Add(lblRuta);
            pnlPaso2_Ruta.Controls.Add(txtRutaInstalacion);
            pnlPaso2_Ruta.Controls.Add(btnExaminarRuta);
            pnlPaso2_Ruta.Controls.Add(chkCrearAccesoDirecto);
            pnlPaso2_Ruta.Controls.Add(lblAvisoInstalacionPreviaP2);

            // ==================== Paso 3: Base de Datos ====================
            pnlPaso3_BaseDatos = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP3 = new Label
            {
                Text = "Paso 3 de 5: Configuración de Base de datos",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblDescP3 = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(12, 40),
                AutoSize = true,
                Visible = false
            };

            lblServidor = new Label
            {
                Text = "Instancia de SQL Server:",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(12, 75),
                AutoSize = true
            };

            cboServidor = new ComboBox
            {
                Location = new Point(15, 100),
                Size = new Size(390, 29),
                Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cboServidor.Items.AddRange(new object[] { ".", @".\SQLEXPRESS", "localhost", @"(LocalDB)\MSSQLLocalDB" });
            cboServidor.SelectedIndex = 0;

            btnProbarConexion = new Button
            {
                Text = "Probar Conexión",
                Size = new Size(160, 30),
                Location = new Point(415, 99),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(30, 58, 138),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnProbarConexion.FlatAppearance.BorderColor = Color.FromArgb(147, 197, 253);
            btnProbarConexion.Click += BtnProbarConexion_Click;

            rdoAuthWindows = new RadioButton
            {
                Text = "Autenticación Integrada de Windows (Recomendada)",
                Checked = true,
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(15, 140),
                AutoSize = true
            };
            rdoAuthWindows.CheckedChanged += RdoAuth_CheckedChanged;

            rdoAuthSql = new RadioButton
            {
                Text = "Autenticación SQL Server (sa / contraseña)",
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(370, 140),
                AutoSize = true
            };
            rdoAuthSql.CheckedChanged += RdoAuth_CheckedChanged;

            lblUsuarioSql = new Label
            {
                Text = "Usuario:",
                Location = new Point(370, 175),
                AutoSize = true,
                Enabled = false
            };
            txtUsuarioSql = new TextBox
            {
                Text = "sa",
                Location = new Point(435, 172),
                Size = new Size(100, 25),
                Enabled = false
            };

            lblClaveSql = new Label
            {
                Text = "Clave:",
                Location = new Point(545, 175),
                AutoSize = true,
                Enabled = false
            };
            txtClaveSql = new TextBox
            {
                PasswordChar = '●',
                Location = new Point(590, 172),
                Size = new Size(95, 25),
                Enabled = false
            };

            chkInstalarBD = new CheckBox
            {
                Text = "Ejecutar script SQL maestro para inicializar el catálogo de datos",
                Checked = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 215),
                AutoSize = true
            };

            // Selector de modo de reinstalación de base de datos
            grpModoBDReinstalacion = new GroupBox
            {
                Text = " SE DETECTO UNA BASE DE DATOS EXISTENTE EN ESTE SERVIDOR ",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(194, 65, 12),
                Location = new Point(15, 245),
                Size = new Size(670, 85),
                Visible = false
            };

            rdoConservarBD = new RadioButton
            {
                Text = "Conservar datos existentes y reparar/sellar integridad",
                Checked = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(15, 25),
                AutoSize = true
            };

            rdoRecrearBD = new RadioButton
            {
                Text = "Recrear base de datos desde cero",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(185, 28, 28),
                Location = new Point(15, 52),
                AutoSize = true
            };

            grpModoBDReinstalacion.Controls.Add(rdoConservarBD);
            grpModoBDReinstalacion.Controls.Add(rdoRecrearBD);

            chkSellarDV = new CheckBox
            {
                Text = "Calcular y sellar Dígitos Verificadores de integridad (si la solución lo implementa)",
                Checked = true,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 335),
                AutoSize = true
            };

            pnlPaso3_BaseDatos.Controls.Add(lblTituloP3);
            pnlPaso3_BaseDatos.Controls.Add(lblDescP3);
            pnlPaso3_BaseDatos.Controls.Add(lblServidor);
            pnlPaso3_BaseDatos.Controls.Add(cboServidor);
            pnlPaso3_BaseDatos.Controls.Add(btnProbarConexion);
            pnlPaso3_BaseDatos.Controls.Add(rdoAuthWindows);
            pnlPaso3_BaseDatos.Controls.Add(rdoAuthSql);
            pnlPaso3_BaseDatos.Controls.Add(lblUsuarioSql);
            pnlPaso3_BaseDatos.Controls.Add(txtUsuarioSql);
            pnlPaso3_BaseDatos.Controls.Add(lblClaveSql);
            pnlPaso3_BaseDatos.Controls.Add(txtClaveSql);
            pnlPaso3_BaseDatos.Controls.Add(chkInstalarBD);
            pnlPaso3_BaseDatos.Controls.Add(grpModoBDReinstalacion);
            pnlPaso3_BaseDatos.Controls.Add(chkSellarDV);

            // ==================== Paso 4: Progreso ====================
            pnlPaso4_Progreso = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP4 = new Label
            {
                Text = "Instalando / Actualizando Componentes...",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblEstadoP4 = new Label
            {
                Text = "Preparando archivos...",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(12, 45),
                AutoSize = true
            };

            progressBar = new ProgressBar
            {
                Location = new Point(15, 75),
                Size = new Size(660, 24),
                Style = ProgressBarStyle.Continuous
            };

            txtLog = new TextBox
            {
                Location = new Point(15, 115),
                Size = new Size(660, 250),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Consolas", 8.8f)
            };

            pnlPaso4_Progreso.Controls.Add(lblTituloP4);
            pnlPaso4_Progreso.Controls.Add(lblEstadoP4);
            pnlPaso4_Progreso.Controls.Add(progressBar);
            pnlPaso4_Progreso.Controls.Add(txtLog);

            // ==================== Paso 5: Finalizado ====================
            pnlPaso5_Finalizado = new Panel { Dock = DockStyle.Fill, Visible = false };

            lblTituloP5 = new Label
            {
                Text = "¡Instalación Finalizada con Éxito!",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 185, 129),
                Location = new Point(10, 20),
                AutoSize = true
            };

            lblDescP5 = new Label
            {
                Text = "El sistema ha sido instalado y configurado correctamente en su computadora.\nLa base de datos fue conectada y verificada de manera satisfactoria.",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(12, 65),
                AutoSize = true
            };

            chkIniciarApp = new CheckBox
            {
                Text = "Iniciar la aplicación ahora",
                Checked = true,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 130),
                AutoSize = true
            };

            pnlPaso5_Finalizado.Controls.Add(lblTituloP5);
            pnlPaso5_Finalizado.Controls.Add(lblDescP5);
            pnlPaso5_Finalizado.Controls.Add(chkIniciarApp);

            // ==================== Agregar Pasos al Contenedor ====================
            pnlContent.Controls.Add(pnlPaso1_Bienvenida);
            pnlContent.Controls.Add(pnlPaso2_Ruta);
            pnlContent.Controls.Add(pnlPaso3_BaseDatos);
            pnlContent.Controls.Add(pnlPaso4_Progreso);
            pnlContent.Controls.Add(pnlPaso5_Finalizado);

            // ==================== Estructura del Form ====================
            Controls.Add(pnlContent);
            Controls.Add(pnlBottom);
            Controls.Add(pnlBanner);
        }
    }
}
