namespace UniversalSetup
{
    partial class FrmUniversalSetup
    {
        private System.ComponentModel.IContainer components = null;

        // Banner Superior
        private System.Windows.Forms.Panel pnlBanner;
        private System.Windows.Forms.Label lblBannerTitle;
        private System.Windows.Forms.Label lblBannerSubtitle;
        private System.Windows.Forms.Label lblBadgeModo;

        // Barra Inferior de Navegación
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnSiguiente;

        // Contenedor Central de Pasos
        private System.Windows.Forms.Panel pnlContent;

        // ===== Paso 1: Bienvenida =====
        private System.Windows.Forms.Panel pnlPaso1_Bienvenida;
        private System.Windows.Forms.Label lblTituloP1;
        private System.Windows.Forms.Label lblDescP1;
        private System.Windows.Forms.Label lblAvisoModoP1;
        private System.Windows.Forms.GroupBox grpInfoP1;
        private System.Windows.Forms.Label lblInfoApp;
        private System.Windows.Forms.Label lblInfoBD;

        // ===== Paso 2: Directorio de Instalación =====
        private System.Windows.Forms.Panel pnlPaso2_Ruta;
        private System.Windows.Forms.Label lblTituloP2;
        private System.Windows.Forms.Label lblDescP2;
        private System.Windows.Forms.Label lblRuta;
        private System.Windows.Forms.TextBox txtRutaInstalacion;
        private System.Windows.Forms.Button btnExaminarRuta;
        private System.Windows.Forms.CheckBox chkCrearAccesoDirecto;
        private System.Windows.Forms.Label lblAvisoInstalacionPreviaP2;

        // ===== Paso 3: Base de Datos y Servidor SQL =====
        private System.Windows.Forms.Panel pnlPaso3_BaseDatos;
        private System.Windows.Forms.Label lblTituloP3;
        private System.Windows.Forms.Label lblDescP3;
        private System.Windows.Forms.Label lblServidor;
        private System.Windows.Forms.ComboBox cboServidor;
        private System.Windows.Forms.Button btnProbarConexion;
        private System.Windows.Forms.RadioButton rdoAuthWindows;
        private System.Windows.Forms.RadioButton rdoAuthSql;
        private System.Windows.Forms.Label lblUsuarioSql;
        private System.Windows.Forms.TextBox txtUsuarioSql;
        private System.Windows.Forms.Label lblClaveSql;
        private System.Windows.Forms.TextBox txtClaveSql;
        private System.Windows.Forms.CheckBox chkInstalarBD;
        private System.Windows.Forms.GroupBox grpModoBDReinstalacion;
        private System.Windows.Forms.RadioButton rdoConservarBD;
        private System.Windows.Forms.RadioButton rdoRecrearBD;
        private System.Windows.Forms.CheckBox chkSellarDV;

        // ===== Paso 4: Progreso =====
        private System.Windows.Forms.Panel pnlPaso4_Progreso;
        private System.Windows.Forms.Label lblTituloP4;
        private System.Windows.Forms.Label lblEstadoP4;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtLog;

        // ===== Paso 5: Finalizado =====
        private System.Windows.Forms.Panel pnlPaso5_Finalizado;
        private System.Windows.Forms.Label lblTituloP5;
        private System.Windows.Forms.Label lblDescP5;
        private System.Windows.Forms.CheckBox chkIniciarApp;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlBanner = new Panel();
            lblBadgeModo = new Label();
            lblBannerSubtitle = new Label();
            lblBannerTitle = new Label();
            pnlBottom = new Panel();
            btnSiguiente = new Button();
            btnAnterior = new Button();
            btnCancelar = new Button();
            pnlContent = new Panel();
            pnlPaso1_Bienvenida = new Panel();
            grpInfoP1 = new GroupBox();
            lblInfoBD = new Label();
            lblInfoApp = new Label();
            lblAvisoModoP1 = new Label();
            lblDescP1 = new Label();
            lblTituloP1 = new Label();
            pnlPaso2_Ruta = new Panel();
            lblAvisoInstalacionPreviaP2 = new Label();
            chkCrearAccesoDirecto = new CheckBox();
            btnExaminarRuta = new Button();
            txtRutaInstalacion = new TextBox();
            lblRuta = new Label();
            lblDescP2 = new Label();
            lblTituloP2 = new Label();
            pnlPaso3_BaseDatos = new Panel();
            chkSellarDV = new CheckBox();
            grpModoBDReinstalacion = new GroupBox();
            rdoRecrearBD = new RadioButton();
            rdoConservarBD = new RadioButton();
            chkInstalarBD = new CheckBox();
            txtClaveSql = new TextBox();
            lblClaveSql = new Label();
            txtUsuarioSql = new TextBox();
            lblUsuarioSql = new Label();
            rdoAuthSql = new RadioButton();
            rdoAuthWindows = new RadioButton();
            btnProbarConexion = new Button();
            cboServidor = new ComboBox();
            lblServidor = new Label();
            lblDescP3 = new Label();
            lblTituloP3 = new Label();
            pnlPaso4_Progreso = new Panel();
            txtLog = new TextBox();
            progressBar = new ProgressBar();
            lblEstadoP4 = new Label();
            lblTituloP4 = new Label();
            pnlPaso5_Finalizado = new Panel();
            chkIniciarApp = new CheckBox();
            lblDescP5 = new Label();
            lblTituloP5 = new Label();
            pnlBanner.SuspendLayout();
            pnlBottom.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlPaso1_Bienvenida.SuspendLayout();
            grpInfoP1.SuspendLayout();
            pnlPaso2_Ruta.SuspendLayout();
            pnlPaso3_BaseDatos.SuspendLayout();
            grpModoBDReinstalacion.SuspendLayout();
            pnlPaso4_Progreso.SuspendLayout();
            pnlPaso5_Finalizado.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBanner
            // 
            pnlBanner.BorderStyle = BorderStyle.FixedSingle;
            pnlBanner.Controls.Add(lblBadgeModo);
            pnlBanner.Controls.Add(lblBannerSubtitle);
            pnlBanner.Controls.Add(lblBannerTitle);
            pnlBanner.Dock = DockStyle.Top;
            pnlBanner.Location = new Point(0, 0);
            pnlBanner.Name = "pnlBanner";
            pnlBanner.Size = new Size(734, 75);
            pnlBanner.TabIndex = 0;
            // 
            // lblBadgeModo
            // 
            lblBadgeModo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBadgeModo.BorderStyle = BorderStyle.FixedSingle;
            lblBadgeModo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblBadgeModo.Location = new Point(530, 18);
            lblBadgeModo.Name = "lblBadgeModo";
            lblBadgeModo.Size = new Size(185, 30);
            lblBadgeModo.TabIndex = 2;
            lblBadgeModo.Text = "[ NUEVA INSTALACION ]";
            lblBadgeModo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBannerSubtitle
            // 
            lblBannerSubtitle.AutoSize = true;
            lblBannerSubtitle.Font = new Font("Segoe UI", 9F);
            lblBannerSubtitle.Location = new Point(17, 38);
            lblBannerSubtitle.Name = "lblBannerSubtitle";
            lblBannerSubtitle.Size = new Size(345, 15);
            lblBannerSubtitle.TabIndex = 1;
            lblBannerSubtitle.Text = "Despliegue automático de aplicación y base de datos SQL Server";
            // 
            // lblBannerTitle
            // 
            lblBannerTitle.AutoSize = true;
            lblBannerTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblBannerTitle.Location = new Point(15, 12);
            lblBannerTitle.Name = "lblBannerTitle";
            lblBannerTitle.Size = new Size(311, 21);
            lblBannerTitle.TabIndex = 0;
            lblBannerTitle.Text = "Asistente de Instalacion y Reinstalacion";
            // 
            // pnlBottom
            // 
            pnlBottom.BorderStyle = BorderStyle.FixedSingle;
            pnlBottom.Controls.Add(btnSiguiente);
            pnlBottom.Controls.Add(btnAnterior);
            pnlBottom.Controls.Add(btnCancelar);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 496);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(734, 55);
            pnlBottom.TabIndex = 1;
            // 
            // btnSiguiente
            // 
            btnSiguiente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSiguiente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSiguiente.Location = new Point(606, 10);
            btnSiguiente.Name = "btnSiguiente";
            btnSiguiente.Size = new Size(110, 32);
            btnSiguiente.TabIndex = 2;
            btnSiguiente.Text = "Siguiente >";
            btnSiguiente.UseVisualStyleBackColor = true;
            btnSiguiente.Click += BtnSiguiente_Click;
            // 
            // btnAnterior
            // 
            btnAnterior.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAnterior.Enabled = false;
            btnAnterior.Location = new Point(495, 10);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(105, 32);
            btnAnterior.TabIndex = 1;
            btnAnterior.Text = "< Anterior";
            btnAnterior.UseVisualStyleBackColor = true;
            btnAnterior.Click += BtnAnterior_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(20, 10);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 32);
            btnCancelar.TabIndex = 0;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(pnlPaso1_Bienvenida);
            pnlContent.Controls.Add(pnlPaso2_Ruta);
            pnlContent.Controls.Add(pnlPaso3_BaseDatos);
            pnlContent.Controls.Add(pnlPaso4_Progreso);
            pnlContent.Controls.Add(pnlPaso5_Finalizado);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 75);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(15);
            pnlContent.Size = new Size(734, 421);
            pnlContent.TabIndex = 2;
            // 
            // pnlPaso1_Bienvenida
            // 
            pnlPaso1_Bienvenida.Controls.Add(grpInfoP1);
            pnlPaso1_Bienvenida.Controls.Add(lblAvisoModoP1);
            pnlPaso1_Bienvenida.Controls.Add(lblDescP1);
            pnlPaso1_Bienvenida.Controls.Add(lblTituloP1);
            pnlPaso1_Bienvenida.Dock = DockStyle.Fill;
            pnlPaso1_Bienvenida.Location = new Point(15, 15);
            pnlPaso1_Bienvenida.Name = "pnlPaso1_Bienvenida";
            pnlPaso1_Bienvenida.Size = new Size(704, 391);
            pnlPaso1_Bienvenida.TabIndex = 0;
            // 
            // grpInfoP1
            // 
            grpInfoP1.Controls.Add(lblInfoBD);
            grpInfoP1.Controls.Add(lblInfoApp);
            grpInfoP1.Location = new Point(12, 115);
            grpInfoP1.Name = "grpInfoP1";
            grpInfoP1.Size = new Size(680, 200);
            grpInfoP1.TabIndex = 3;
            grpInfoP1.TabStop = false;
            grpInfoP1.Text = "Solución";
            // 
            // lblInfoBD
            // 
            lblInfoBD.AutoSize = true;
            lblInfoBD.Location = new Point(20, 65);
            lblInfoBD.Name = "lblInfoBD";
            lblInfoBD.Size = new Size(147, 15);
            lblInfoBD.TabIndex = 1;
            lblInfoBD.Text = "Base de Datos: Cargando...";
            // 
            // lblInfoApp
            // 
            lblInfoApp.AutoSize = true;
            lblInfoApp.Location = new Point(20, 30);
            lblInfoApp.Name = "lblInfoApp";
            lblInfoApp.Size = new Size(179, 15);
            lblInfoApp.TabIndex = 0;
            lblInfoApp.Text = "Aplicacion Principal: Cargando...";
            // 
            // lblAvisoModoP1
            // 
            lblAvisoModoP1.AutoSize = true;
            lblAvisoModoP1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAvisoModoP1.Location = new Point(12, 85);
            lblAvisoModoP1.Name = "lblAvisoModoP1";
            lblAvisoModoP1.Size = new Size(262, 15);
            lblAvisoModoP1.TabIndex = 2;
            lblAvisoModoP1.Text = "Modo de Instalacioninicial listo para comenzar.";
            // 
            // lblDescP1
            // 
            lblDescP1.AutoSize = true;
            lblDescP1.Location = new Point(12, 40);
            lblDescP1.Name = "lblDescP1";
            lblDescP1.Size = new Size(284, 15);
            lblDescP1.TabIndex = 1;
            lblDescP1.Text = "Este asistente intalara los componentes en su equipo";
            lblDescP1.Click += lblDescP1_Click;
            // 
            // lblTituloP1
            // 
            lblTituloP1.AutoSize = true;
            lblTituloP1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloP1.Location = new Point(10, 10);
            lblTituloP1.Name = "lblTituloP1";
            lblTituloP1.Size = new Size(322, 21);
            lblTituloP1.TabIndex = 0;
            lblTituloP1.Text = "Bienvenido al Instalador de la Aplicación";
            // 
            // pnlPaso2_Ruta
            // 
            pnlPaso2_Ruta.Controls.Add(lblAvisoInstalacionPreviaP2);
            pnlPaso2_Ruta.Controls.Add(chkCrearAccesoDirecto);
            pnlPaso2_Ruta.Controls.Add(btnExaminarRuta);
            pnlPaso2_Ruta.Controls.Add(txtRutaInstalacion);
            pnlPaso2_Ruta.Controls.Add(lblRuta);
            pnlPaso2_Ruta.Controls.Add(lblDescP2);
            pnlPaso2_Ruta.Controls.Add(lblTituloP2);
            pnlPaso2_Ruta.Dock = DockStyle.Fill;
            pnlPaso2_Ruta.Location = new Point(15, 15);
            pnlPaso2_Ruta.Name = "pnlPaso2_Ruta";
            pnlPaso2_Ruta.Size = new Size(704, 391);
            pnlPaso2_Ruta.TabIndex = 1;
            pnlPaso2_Ruta.Visible = false;
            // 
            // lblAvisoInstalacionPreviaP2
            // 
            lblAvisoInstalacionPreviaP2.AutoSize = true;
            lblAvisoInstalacionPreviaP2.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblAvisoInstalacionPreviaP2.Location = new Point(12, 175);
            lblAvisoInstalacionPreviaP2.Name = "lblAvisoInstalacionPreviaP2";
            lblAvisoInstalacionPreviaP2.Size = new Size(424, 15);
            lblAvisoInstalacionPreviaP2.TabIndex = 6;
            lblAvisoInstalacionPreviaP2.Text = "(Se detectó una versión previa en esta carpeta. Los binarios serán actualizados).";
            lblAvisoInstalacionPreviaP2.Visible = false;
            // 
            // chkCrearAccesoDirecto
            // 
            chkCrearAccesoDirecto.AutoSize = true;
            chkCrearAccesoDirecto.Checked = true;
            chkCrearAccesoDirecto.CheckState = CheckState.Checked;
            chkCrearAccesoDirecto.Location = new Point(15, 140);
            chkCrearAccesoDirecto.Name = "chkCrearAccesoDirecto";
            chkCrearAccesoDirecto.Size = new Size(213, 19);
            chkCrearAccesoDirecto.TabIndex = 5;
            chkCrearAccesoDirecto.Text = "Crear acceso directo en el Escritorio";
            chkCrearAccesoDirecto.UseVisualStyleBackColor = true;
            // 
            // btnExaminarRuta
            // 
            btnExaminarRuta.Location = new Point(585, 93);
            btnExaminarRuta.Name = "btnExaminarRuta";
            btnExaminarRuta.Size = new Size(100, 26);
            btnExaminarRuta.TabIndex = 4;
            btnExaminarRuta.Text = "Examinar...";
            btnExaminarRuta.UseVisualStyleBackColor = true;
            btnExaminarRuta.Click += BtnExaminarRuta_Click;
            // 
            // txtRutaInstalacion
            // 
            txtRutaInstalacion.Location = new Point(15, 95);
            txtRutaInstalacion.Name = "txtRutaInstalacion";
            txtRutaInstalacion.Size = new Size(560, 23);
            txtRutaInstalacion.TabIndex = 3;
            txtRutaInstalacion.TextChanged += TxtRutaInstalacion_TextChanged;
            // 
            // lblRuta
            // 
            lblRuta.AutoSize = true;
            lblRuta.Location = new Point(12, 75);
            lblRuta.Name = "lblRuta";
            lblRuta.Size = new Size(110, 15);
            lblRuta.TabIndex = 2;
            lblRuta.Text = "Ruta de Instalación:";
            // 
            // lblDescP2
            // 
            lblDescP2.AutoSize = true;
            lblDescP2.Location = new Point(12, 40);
            lblDescP2.Name = "lblDescP2";
            lblDescP2.Size = new Size(421, 15);
            lblDescP2.TabIndex = 1;
            lblDescP2.Text = "Seleccione el directorio donde se copiarán los archivos ejecutables del sistema:";
            // 
            // lblTituloP2
            // 
            lblTituloP2.AutoSize = true;
            lblTituloP2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloP2.Location = new Point(10, 10);
            lblTituloP2.Name = "lblTituloP2";
            lblTituloP2.Size = new Size(254, 21);
            lblTituloP2.TabIndex = 0;
            lblTituloP2.Text = "Carpeta de Destino en el Equipo";
            // 
            // pnlPaso3_BaseDatos
            // 
            pnlPaso3_BaseDatos.Controls.Add(chkSellarDV);
            pnlPaso3_BaseDatos.Controls.Add(grpModoBDReinstalacion);
            pnlPaso3_BaseDatos.Controls.Add(chkInstalarBD);
            pnlPaso3_BaseDatos.Controls.Add(txtClaveSql);
            pnlPaso3_BaseDatos.Controls.Add(lblClaveSql);
            pnlPaso3_BaseDatos.Controls.Add(txtUsuarioSql);
            pnlPaso3_BaseDatos.Controls.Add(lblUsuarioSql);
            pnlPaso3_BaseDatos.Controls.Add(rdoAuthSql);
            pnlPaso3_BaseDatos.Controls.Add(rdoAuthWindows);
            pnlPaso3_BaseDatos.Controls.Add(btnProbarConexion);
            pnlPaso3_BaseDatos.Controls.Add(cboServidor);
            pnlPaso3_BaseDatos.Controls.Add(lblServidor);
            pnlPaso3_BaseDatos.Controls.Add(lblDescP3);
            pnlPaso3_BaseDatos.Controls.Add(lblTituloP3);
            pnlPaso3_BaseDatos.Dock = DockStyle.Fill;
            pnlPaso3_BaseDatos.Location = new Point(15, 15);
            pnlPaso3_BaseDatos.Name = "pnlPaso3_BaseDatos";
            pnlPaso3_BaseDatos.Size = new Size(704, 391);
            pnlPaso3_BaseDatos.TabIndex = 2;
            pnlPaso3_BaseDatos.Visible = false;
            // 
            // chkSellarDV
            // 
            chkSellarDV.AutoSize = true;
            chkSellarDV.Checked = true;
            chkSellarDV.CheckState = CheckState.Checked;
            chkSellarDV.Location = new Point(15, 330);
            chkSellarDV.Name = "chkSellarDV";
            chkSellarDV.Size = new Size(353, 19);
            chkSellarDV.TabIndex = 13;
            chkSellarDV.Text = "Recalcular y sellar Dígitos Verificadores (DVH / DVV) al finalizar";
            chkSellarDV.UseVisualStyleBackColor = true;
            // 
            // grpModoBDReinstalacion
            // 
            grpModoBDReinstalacion.Controls.Add(rdoRecrearBD);
            grpModoBDReinstalacion.Controls.Add(rdoConservarBD);
            grpModoBDReinstalacion.Location = new Point(15, 235);
            grpModoBDReinstalacion.Name = "grpModoBDReinstalacion";
            grpModoBDReinstalacion.Size = new Size(660, 80);
            grpModoBDReinstalacion.TabIndex = 12;
            grpModoBDReinstalacion.TabStop = false;
            grpModoBDReinstalacion.Text = "Comportamiento ante Base de Datos Existente (Reinstalación)";
            grpModoBDReinstalacion.Visible = false;
            // 
            // rdoRecrearBD
            // 
            rdoRecrearBD.AutoSize = true;
            rdoRecrearBD.Location = new Point(15, 50);
            rdoRecrearBD.Name = "rdoRecrearBD";
            rdoRecrearBD.Size = new Size(449, 19);
            rdoRecrearBD.TabIndex = 1;
            rdoRecrearBD.Text = "Restablecimiento de fábrica: eliminar y volver a crear desde cero la base de datos";
            rdoRecrearBD.UseVisualStyleBackColor = true;
            // 
            // rdoConservarBD
            // 
            rdoConservarBD.AutoSize = true;
            rdoConservarBD.Checked = true;
            rdoConservarBD.Location = new Point(15, 25);
            rdoConservarBD.Name = "rdoConservarBD";
            rdoConservarBD.Size = new Size(428, 19);
            rdoConservarBD.TabIndex = 0;
            rdoConservarBD.TabStop = true;
            rdoConservarBD.Text = "Conservar datos existentes (solo actualiza estructuras sin borrar información)";
            rdoConservarBD.UseVisualStyleBackColor = true;
            // 
            // chkInstalarBD
            // 
            chkInstalarBD.AutoSize = true;
            chkInstalarBD.Checked = true;
            chkInstalarBD.CheckState = CheckState.Checked;
            chkInstalarBD.Location = new Point(15, 205);
            chkInstalarBD.Name = "chkInstalarBD";
            chkInstalarBD.Size = new Size(359, 19);
            chkInstalarBD.TabIndex = 11;
            chkInstalarBD.Text = "Crear o actualizar la base de datos automáticamente (script.sql)";
            chkInstalarBD.UseVisualStyleBackColor = true;
            // 
            // txtClaveSql
            // 
            txtClaveSql.Enabled = false;
            txtClaveSql.Location = new Point(310, 165);
            txtClaveSql.Name = "txtClaveSql";
            txtClaveSql.PasswordChar = '*';
            txtClaveSql.Size = new Size(150, 23);
            txtClaveSql.TabIndex = 10;
            // 
            // lblClaveSql
            // 
            lblClaveSql.AutoSize = true;
            lblClaveSql.Enabled = false;
            lblClaveSql.Location = new Point(265, 168);
            lblClaveSql.Name = "lblClaveSql";
            lblClaveSql.Size = new Size(39, 15);
            lblClaveSql.TabIndex = 9;
            lblClaveSql.Text = "Clave:";
            // 
            // txtUsuarioSql
            // 
            txtUsuarioSql.Enabled = false;
            txtUsuarioSql.Location = new Point(95, 165);
            txtUsuarioSql.Name = "txtUsuarioSql";
            txtUsuarioSql.Size = new Size(150, 23);
            txtUsuarioSql.TabIndex = 8;
            txtUsuarioSql.Text = "sa";
            // 
            // lblUsuarioSql
            // 
            lblUsuarioSql.AutoSize = true;
            lblUsuarioSql.Enabled = false;
            lblUsuarioSql.Location = new Point(35, 168);
            lblUsuarioSql.Name = "lblUsuarioSql";
            lblUsuarioSql.Size = new Size(50, 15);
            lblUsuarioSql.TabIndex = 7;
            lblUsuarioSql.Text = "Usuario:";
            // 
            // rdoAuthSql
            // 
            rdoAuthSql.AutoSize = true;
            rdoAuthSql.Location = new Point(15, 140);
            rdoAuthSql.Name = "rdoAuthSql";
            rdoAuthSql.Size = new Size(174, 19);
            rdoAuthSql.TabIndex = 6;
            rdoAuthSql.Text = "Autenticación de SQL Server";
            rdoAuthSql.UseVisualStyleBackColor = true;
            rdoAuthSql.CheckedChanged += RdoAuth_CheckedChanged;
            // 
            // rdoAuthWindows
            // 
            rdoAuthWindows.AutoSize = true;
            rdoAuthWindows.Checked = true;
            rdoAuthWindows.Location = new Point(15, 115);
            rdoAuthWindows.Name = "rdoAuthWindows";
            rdoAuthWindows.Size = new Size(254, 19);
            rdoAuthWindows.TabIndex = 5;
            rdoAuthWindows.TabStop = true;
            rdoAuthWindows.Text = "Autenticación de Windows (Recomendada)";
            rdoAuthWindows.UseVisualStyleBackColor = true;
            rdoAuthWindows.CheckedChanged += RdoAuth_CheckedChanged;
            // 
            // btnProbarConexion
            // 
            btnProbarConexion.Location = new Point(345, 78);
            btnProbarConexion.Name = "btnProbarConexion";
            btnProbarConexion.Size = new Size(125, 26);
            btnProbarConexion.TabIndex = 4;
            btnProbarConexion.Text = "Probar Conexión";
            btnProbarConexion.UseVisualStyleBackColor = true;
            btnProbarConexion.Click += BtnProbarConexion_Click;
            // 
            // cboServidor
            // 
            cboServidor.FormattingEnabled = true;
            cboServidor.Items.AddRange(new object[] { ".", ".\\SQLEXPRESS", "localhost", "(LocalDB)\\MSSQLLocalDB" });
            cboServidor.Location = new Point(15, 80);
            cboServidor.Name = "cboServidor";
            cboServidor.Size = new Size(320, 23);
            cboServidor.TabIndex = 3;
            cboServidor.SelectedIndexChanged += CboServidor_SelectedIndexChanged;
            cboServidor.TextChanged += CboServidor_TextChanged;
            // 
            // lblServidor
            // 
            lblServidor.AutoSize = true;
            lblServidor.Location = new Point(12, 60);
            lblServidor.Name = "lblServidor";
            lblServidor.Size = new Size(116, 15);
            lblServidor.TabIndex = 2;
            lblServidor.Text = "Instancia SQL Server:";
            // 
            // lblDescP3
            // 
            lblDescP3.AutoSize = true;
            lblDescP3.Location = new Point(12, 35);
            lblDescP3.Name = "lblDescP3";
            lblDescP3.Size = new Size(429, 15);
            lblDescP3.TabIndex = 1;
            lblDescP3.Text = "Indique la instancia de SQL Server donde se creará o actualizará la base de datos:";
            // 
            // lblTituloP3
            // 
            lblTituloP3.AutoSize = true;
            lblTituloP3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloP3.Location = new Point(10, 10);
            lblTituloP3.Name = "lblTituloP3";
            lblTituloP3.Size = new Size(302, 21);
            lblTituloP3.TabIndex = 0;
            lblTituloP3.Text = "Configuración del Servidor SQL Server";
            // 
            // pnlPaso4_Progreso
            // 
            pnlPaso4_Progreso.Controls.Add(txtLog);
            pnlPaso4_Progreso.Controls.Add(progressBar);
            pnlPaso4_Progreso.Controls.Add(lblEstadoP4);
            pnlPaso4_Progreso.Controls.Add(lblTituloP4);
            pnlPaso4_Progreso.Dock = DockStyle.Fill;
            pnlPaso4_Progreso.Location = new Point(15, 15);
            pnlPaso4_Progreso.Name = "pnlPaso4_Progreso";
            pnlPaso4_Progreso.Size = new Size(704, 391);
            pnlPaso4_Progreso.TabIndex = 3;
            pnlPaso4_Progreso.Visible = false;
            // 
            // txtLog
            // 
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.Location = new Point(15, 100);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(670, 260);
            txtLog.TabIndex = 3;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(15, 65);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(670, 23);
            progressBar.TabIndex = 2;
            // 
            // lblEstadoP4
            // 
            lblEstadoP4.AutoSize = true;
            lblEstadoP4.Location = new Point(12, 40);
            lblEstadoP4.Name = "lblEstadoP4";
            lblEstadoP4.Size = new Size(124, 15);
            lblEstadoP4.TabIndex = 1;
            lblEstadoP4.Text = "Preparando archivos...";
            // 
            // lblTituloP4
            // 
            lblTituloP4.AutoSize = true;
            lblTituloP4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloP4.Location = new Point(10, 10);
            lblTituloP4.Name = "lblTituloP4";
            lblTituloP4.Size = new Size(226, 21);
            lblTituloP4.TabIndex = 0;
            lblTituloP4.Text = "Instalando y Configurando...";
            // 
            // pnlPaso5_Finalizado
            // 
            pnlPaso5_Finalizado.Controls.Add(chkIniciarApp);
            pnlPaso5_Finalizado.Controls.Add(lblDescP5);
            pnlPaso5_Finalizado.Controls.Add(lblTituloP5);
            pnlPaso5_Finalizado.Dock = DockStyle.Fill;
            pnlPaso5_Finalizado.Location = new Point(15, 15);
            pnlPaso5_Finalizado.Name = "pnlPaso5_Finalizado";
            pnlPaso5_Finalizado.Size = new Size(704, 391);
            pnlPaso5_Finalizado.TabIndex = 4;
            pnlPaso5_Finalizado.Visible = false;
            // 
            // chkIniciarApp
            // 
            chkIniciarApp.AutoSize = true;
            chkIniciarApp.Checked = true;
            chkIniciarApp.CheckState = CheckState.Checked;
            chkIniciarApp.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            chkIniciarApp.Location = new Point(15, 110);
            chkIniciarApp.Name = "chkIniciarApp";
            chkIniciarApp.Size = new Size(185, 21);
            chkIniciarApp.TabIndex = 2;
            chkIniciarApp.Text = "Iniciar la aplicación ahora";
            chkIniciarApp.UseVisualStyleBackColor = true;
            // 
            // lblDescP5
            // 
            lblDescP5.AutoSize = true;
            lblDescP5.Location = new Point(12, 55);
            lblDescP5.Name = "lblDescP5";
            lblDescP5.Size = new Size(417, 30);
            lblDescP5.TabIndex = 1;
            lblDescP5.Text = "El sistema ha sido instalado y configurado correctamente en su computadora.\r\nLa base de datos fue conectada y verificada de manera satisfactoria.";
            // 
            // lblTituloP5
            // 
            lblTituloP5.AutoSize = true;
            lblTituloP5.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTituloP5.Location = new Point(10, 15);
            lblTituloP5.Name = "lblTituloP5";
            lblTituloP5.Size = new Size(291, 25);
            lblTituloP5.TabIndex = 0;
            lblTituloP5.Text = "¡Instalación Finalizada con Éxito!";
            // 
            // FrmUniversalSetup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 551);
            Controls.Add(pnlContent);
            Controls.Add(pnlBottom);
            Controls.Add(pnlBanner);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FrmUniversalSetup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Instalador / Reinstalador Universal .NET";
            pnlBanner.ResumeLayout(false);
            pnlBanner.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            pnlPaso1_Bienvenida.ResumeLayout(false);
            pnlPaso1_Bienvenida.PerformLayout();
            grpInfoP1.ResumeLayout(false);
            grpInfoP1.PerformLayout();
            pnlPaso2_Ruta.ResumeLayout(false);
            pnlPaso2_Ruta.PerformLayout();
            pnlPaso3_BaseDatos.ResumeLayout(false);
            pnlPaso3_BaseDatos.PerformLayout();
            grpModoBDReinstalacion.ResumeLayout(false);
            grpModoBDReinstalacion.PerformLayout();
            pnlPaso4_Progreso.ResumeLayout(false);
            pnlPaso4_Progreso.PerformLayout();
            pnlPaso5_Finalizado.ResumeLayout(false);
            pnlPaso5_Finalizado.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
    }
}
