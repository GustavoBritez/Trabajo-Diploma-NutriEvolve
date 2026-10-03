namespace UI
{
    partial class MenuPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelMenu = new Panel();
            btnAyuda = new UI.Controles.BotonFuturista();
            btnRespaldo = new UI.Controles.BotonFuturista();
            btnGestionarPerfiles = new UI.Controles.BotonFuturista();
            btnUsuarios = new UI.Controles.BotonFuturista();
            btnReportes = new UI.Controles.BotonFuturista();
            btnTurnos = new UI.Controles.BotonFuturista();
            lblModulo = new Label();
            btnCambiarContrasena = new UI.Controles.BotonFuturista();
            btnLogin = new UI.Controles.BotonFuturista();
            btnLogout = new UI.Controles.BotonFuturista();
            panelTop = new Panel();
            cmbIdioma = new ComboBox();
            label6 = new Label();
            label4 = new Label();
            lblTitulo = new Label();
            panelContenedor = new Panel();
            label1 = new Label();
            ChangePassPanel = new Panel();
            label5 = new Label();
            txtActualPass = new TextBox();
            btnCancelarMP = new UI.Controles.BotonFuturista();
            btnAceptar = new UI.Controles.BotonFuturista();
            txtRepPass = new TextBox();
            label3 = new Label();
            label2 = new Label();
            txtNewPass = new TextBox();
            sqlCommandBuilder1 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            panelMenu.SuspendLayout();
            panelTop.SuspendLayout();
            panelContenedor.SuspendLayout();
            ChangePassPanel.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(76, 124, 89);
            panelMenu.Controls.Add(btnAyuda);
            panelMenu.Controls.Add(btnRespaldo);
            panelMenu.Controls.Add(btnGestionarPerfiles);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(btnReportes);
            panelMenu.Controls.Add(btnTurnos);
            panelMenu.Controls.Add(lblModulo);
            panelMenu.Controls.Add(btnCambiarContrasena);
            panelMenu.Controls.Add(btnLogin);
            panelMenu.Controls.Add(btnLogout);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(260, 700);
            panelMenu.TabIndex = 0;
            // 
            // lblModulo
            // 
            lblModulo.AutoSize = true;
            lblModulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblModulo.ForeColor = Color.White;
            lblModulo.Location = new Point(18, 20);
            lblModulo.Name = "lblModulo";
            lblModulo.Size = new Size(182, 21);
            lblModulo.TabIndex = 0;
            lblModulo.Text = "MÓDULOS DEL SISTEMA";
            // 
            // btnTurnos
            // 
            btnTurnos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnTurnos.BackColor = Color.Transparent;
            btnTurnos.Cursor = Cursors.Hand;
            btnTurnos.EsMenuLateral = true;
            btnTurnos.FlatAppearance.BorderSize = 0;
            btnTurnos.FlatStyle = FlatStyle.Flat;
            btnTurnos.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnTurnos.ForeColor = Color.White;
            btnTurnos.Location = new Point(12, 56);
            btnTurnos.Name = "btnTurnos";
            btnTurnos.PaddingIzquierdo = 18;
            btnTurnos.RadioBorde = 24;
            btnTurnos.Size = new Size(236, 44);
            btnTurnos.TabIndex = 1;
            btnTurnos.Text = "📅  Gestión de Turnos";
            btnTurnos.TextAlign = ContentAlignment.MiddleLeft;
            btnTurnos.UseVisualStyleBackColor = false;
            btnTurnos.Click += btnTurnos_Click;

            // btnReportes
            // 
            btnReportes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnReportes.BackColor = Color.Transparent;
            btnReportes.Cursor = Cursors.Hand;
            btnReportes.EsMenuLateral = true;
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(12, 156);
            btnReportes.Name = "btnReportes";
            btnReportes.PaddingIzquierdo = 18;
            btnReportes.RadioBorde = 24;
            btnReportes.Size = new Size(236, 44);
            btnReportes.TabIndex = 3;
            btnReportes.Text = "📒  Bitacora";
            btnReportes.TextAlign = ContentAlignment.MiddleLeft;
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Click += btnBitacora_Click;
            // 
            // btnUsuarios
            // 
            btnUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnUsuarios.BackColor = Color.Transparent;
            btnUsuarios.Cursor = Cursors.Hand;
            btnUsuarios.EsMenuLateral = true;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Location = new Point(12, 206);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.PaddingIzquierdo = 18;
            btnUsuarios.RadioBorde = 24;
            btnUsuarios.Size = new Size(236, 44);
            btnUsuarios.TabIndex = 4;
            btnUsuarios.Text = "👤  Gestión de Usuarios";
            btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.UseVisualStyleBackColor = false;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnGestionarPerfiles
            // 
            btnGestionarPerfiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnGestionarPerfiles.BackColor = Color.Transparent;
            btnGestionarPerfiles.Cursor = Cursors.Hand;
            btnGestionarPerfiles.EsMenuLateral = true;
            btnGestionarPerfiles.FlatAppearance.BorderSize = 0;
            btnGestionarPerfiles.FlatStyle = FlatStyle.Flat;
            btnGestionarPerfiles.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnGestionarPerfiles.ForeColor = Color.White;
            btnGestionarPerfiles.Location = new Point(12, 256);
            btnGestionarPerfiles.Name = "btnGestionarPerfiles";
            btnGestionarPerfiles.PaddingIzquierdo = 18;
            btnGestionarPerfiles.RadioBorde = 24;
            btnGestionarPerfiles.Size = new Size(236, 44);
            btnGestionarPerfiles.TabIndex = 10;
            btnGestionarPerfiles.Text = "🔑  Perfiles";
            btnGestionarPerfiles.TextAlign = ContentAlignment.MiddleLeft;
            btnGestionarPerfiles.UseVisualStyleBackColor = false;
            btnGestionarPerfiles.Click += btnGestionarPerfiles_Click;
            // 
            // btnRespaldo
            // 
            btnRespaldo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnRespaldo.BackColor = Color.Transparent;
            btnRespaldo.Cursor = Cursors.Hand;
            btnRespaldo.EsMenuLateral = true;
            btnRespaldo.FlatAppearance.BorderSize = 0;
            btnRespaldo.FlatStyle = FlatStyle.Flat;
            btnRespaldo.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRespaldo.ForeColor = Color.White;
            btnRespaldo.Location = new Point(12, 306);
            btnRespaldo.Name = "btnRespaldo";
            btnRespaldo.PaddingIzquierdo = 18;
            btnRespaldo.RadioBorde = 24;
            btnRespaldo.Size = new Size(236, 44);
            btnRespaldo.TabIndex = 9;
            btnRespaldo.Text = "🔒  Respaldo";
            btnRespaldo.TextAlign = ContentAlignment.MiddleLeft;
            btnRespaldo.UseVisualStyleBackColor = false;
            btnRespaldo.Click += btnRespaldo_Click;
            // 
            // btnAyuda
            // 
            btnAyuda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnAyuda.BackColor = Color.Transparent;
            btnAyuda.Cursor = Cursors.Hand;
            btnAyuda.EsMenuLateral = true;
            btnAyuda.FlatAppearance.BorderSize = 0;
            btnAyuda.FlatStyle = FlatStyle.Flat;
            btnAyuda.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAyuda.ForeColor = Color.White;
            btnAyuda.Location = new Point(12, 356);
            btnAyuda.Name = "btnAyuda";
            btnAyuda.PaddingIzquierdo = 18;
            btnAyuda.RadioBorde = 24;
            btnAyuda.Size = new Size(236, 44);
            btnAyuda.TabIndex = 5;
            btnAyuda.Text = "❓  Ayuda";
            btnAyuda.TextAlign = ContentAlignment.MiddleLeft;
            btnAyuda.UseVisualStyleBackColor = false;
            // 
            // btnCambiarContrasena
            // 
            btnCambiarContrasena.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCambiarContrasena.BackColor = Color.Transparent;
            btnCambiarContrasena.Cursor = Cursors.Hand;
            btnCambiarContrasena.EsBotonSecundario = true;
            btnCambiarContrasena.FlatAppearance.BorderSize = 0;
            btnCambiarContrasena.FlatStyle = FlatStyle.Flat;
            btnCambiarContrasena.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCambiarContrasena.ForeColor = Color.White;
            btnCambiarContrasena.Location = new Point(12, 558);
            btnCambiarContrasena.Margin = new Padding(2);
            btnCambiarContrasena.Name = "btnCambiarContrasena";
            btnCambiarContrasena.RadioBorde = 38;
            btnCambiarContrasena.Size = new Size(236, 38);
            btnCambiarContrasena.TabIndex = 8;
            btnCambiarContrasena.Text = "Cambiar Contraseña";
            btnCambiarContrasena.UseVisualStyleBackColor = false;
            btnCambiarContrasena.Click += btnCambiarContrasena_Click;
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnLogin.BackColor = Color.Transparent;
            btnLogin.ColorGlow = Color.DarkSeaGreen;
            btnLogin.ColorPrincipal = Color.FromArgb(78, 122, 84);
            btnLogin.ColorSecundario = Color.FromArgb(50, 82, 55);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.EsBotonSecundario = false;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(12, 602);
            btnLogin.Margin = new Padding(2);
            btnLogin.Name = "btnLogin";
            btnLogin.RadioBorde = 38;
            btnLogin.Size = new Size(236, 38);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "Iniciar Sesión";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnLogout.BackColor = Color.Transparent;
            btnLogout.ColorGlow = Color.FromArgb(240, 160, 160);
            btnLogout.ColorPrincipal = Color.FromArgb(160, 70, 70);
            btnLogout.ColorSecundario = Color.FromArgb(120, 45, 45);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.EsBotonSecundario = false;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(12, 646);
            btnLogout.Name = "btnLogout";
            btnLogout.RadioBorde = 38;
            btnLogout.Size = new Size(236, 38);
            btnLogout.TabIndex = 6;
            btnLogout.Text = "🚪 Cerrar Sesión";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(92, 145, 104);
            panelTop.Controls.Add(cmbIdioma);
            panelTop.Controls.Add(label6);
            panelTop.Controls.Add(label4);
            panelTop.Controls.Add(lblTitulo);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(260, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(950, 60);
            panelTop.TabIndex = 1;
            // 
            // cmbIdioma
            // 
            cmbIdioma.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbIdioma.BackColor = Color.DarkSeaGreen;
            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdioma.FlatStyle = FlatStyle.Flat;
            cmbIdioma.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            cmbIdioma.FormattingEnabled = true;
            cmbIdioma.Items.AddRange(new object[] { "Español", "English", "Portugues" });
            cmbIdioma.Location = new Point(780, 16);
            cmbIdioma.Name = "cmbIdioma";
            cmbIdioma.Size = new Size(150, 27);
            cmbIdioma.TabIndex = 2;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoEllipsis = true;
            label6.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(240, 252, 242);
            label6.Location = new Point(480, 18);
            label6.Name = "label6";
            label6.Size = new Size(290, 25);
            label6.TabIndex = 10;
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(220, 240, 222);
            label4.Location = new Point(410, 20);
            label4.Name = "label4";
            label4.Size = new Size(69, 20);
            label4.TabIndex = 9;
            label4.Text = "Usuario: ";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(25, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(137, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "NutriEvolve";
            // 
            // panelContenedor
            // 
            panelContenedor.AutoScroll = true;
            panelContenedor.BackColor = Color.FromArgb(225, 240, 228);
            panelContenedor.Controls.Add(label1);
            panelContenedor.Controls.Add(ChangePassPanel);
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(260, 60);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(950, 640);
            panelContenedor.TabIndex = 2;
            panelContenedor.Resize += panelContenedor_Resize;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.ForeColor = Color.FromArgb(120, 150, 125);
            label1.Location = new Point(670, 610);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 1;
            // 
            // ChangePassPanel
            // 
            ChangePassPanel.BackColor = Color.FromArgb(76, 124, 89);
            ChangePassPanel.Controls.Add(label5);
            ChangePassPanel.Controls.Add(txtActualPass);
            ChangePassPanel.Controls.Add(btnCancelarMP);
            ChangePassPanel.Controls.Add(btnAceptar);
            ChangePassPanel.Controls.Add(txtRepPass);
            ChangePassPanel.Controls.Add(label3);
            ChangePassPanel.Controls.Add(label2);
            ChangePassPanel.Controls.Add(txtNewPass);
            ChangePassPanel.Location = new Point(280, 200);
            ChangePassPanel.Name = "ChangePassPanel";
            ChangePassPanel.Size = new Size(388, 220);
            ChangePassPanel.TabIndex = 0;
            ChangePassPanel.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(155, 22);
            label5.Name = "label5";
            label5.Size = new Size(135, 20);
            label5.TabIndex = 13;
            label5.Text = "Contraseña Actual";
            // 
            // txtActualPass
            // 
            txtActualPass.Font = new Font("Segoe UI", 10F);
            txtActualPass.Location = new Point(25, 20);
            txtActualPass.Name = "txtActualPass";
            txtActualPass.PasswordChar = '●';
            txtActualPass.Size = new Size(120, 25);
            txtActualPass.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(155, 62);
            label2.Name = "label2";
            label2.Size = new Size(135, 20);
            label2.TabIndex = 3;
            label2.Text = "Nueva Contraseña";
            // 
            // txtNewPass
            // 
            txtNewPass.Font = new Font("Segoe UI", 10F);
            txtNewPass.Location = new Point(25, 60);
            txtNewPass.Name = "txtNewPass";
            txtNewPass.PasswordChar = '●';
            txtNewPass.Size = new Size(120, 25);
            txtNewPass.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(155, 102);
            label3.Name = "label3";
            label3.Size = new Size(141, 20);
            label3.TabIndex = 4;
            label3.Text = "Repetir Contraseña";
            // 
            // txtRepPass
            // 
            txtRepPass.Font = new Font("Segoe UI", 10F);
            txtRepPass.Location = new Point(25, 100);
            txtRepPass.Name = "txtRepPass";
            txtRepPass.PasswordChar = '●';
            txtRepPass.Size = new Size(120, 25);
            txtRepPass.TabIndex = 5;
            // 
            // btnAceptar
            // 
            btnAceptar.BackColor = Color.Transparent;
            btnAceptar.ColorGlow = Color.DarkSeaGreen;
            btnAceptar.ColorPrincipal = Color.FromArgb(78, 122, 84);
            btnAceptar.ColorSecundario = Color.FromArgb(50, 82, 55);
            btnAceptar.Cursor = Cursors.Hand;
            btnAceptar.EsBotonSecundario = false;
            btnAceptar.FlatAppearance.BorderSize = 0;
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAceptar.ForeColor = Color.White;
            btnAceptar.Location = new Point(25, 155);
            btnAceptar.Margin = new Padding(2);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.RadioBorde = 36;
            btnAceptar.Size = new Size(150, 40);
            btnAceptar.TabIndex = 9;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelarMP
            // 
            btnCancelarMP.BackColor = Color.Transparent;
            btnCancelarMP.Cursor = Cursors.Hand;
            btnCancelarMP.EsBotonSecundario = true;
            btnCancelarMP.FlatAppearance.BorderSize = 0;
            btnCancelarMP.FlatStyle = FlatStyle.Flat;
            btnCancelarMP.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelarMP.ForeColor = Color.White;
            btnCancelarMP.Location = new Point(210, 155);
            btnCancelarMP.Margin = new Padding(2);
            btnCancelarMP.Name = "btnCancelarMP";
            btnCancelarMP.RadioBorde = 36;
            btnCancelarMP.Size = new Size(150, 40);
            btnCancelarMP.TabIndex = 10;
            btnCancelarMP.Text = "Cancelar";
            btnCancelarMP.UseVisualStyleBackColor = false;
            btnCancelarMP.Click += btnCancelar_Click;
            // 
            // MenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(225, 240, 228);
            ClientSize = new Size(1210, 700);
            Controls.Add(panelContenedor);
            Controls.Add(panelTop);
            Controls.Add(panelMenu);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1000, 650);
            Name = "MenuPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema NutriEvolve";
            panelMenu.ResumeLayout(false);
            panelMenu.PerformLayout();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelContenedor.ResumeLayout(false);
            panelContenedor.PerformLayout();
            ChangePassPanel.ResumeLayout(false);
            ChangePassPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelContenedor;

        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.Label lblTitulo;

        private UI.Controles.BotonFuturista btnTurnos;
        private UI.Controles.BotonFuturista btnReportes;
        private UI.Controles.BotonFuturista btnUsuarios;
        private UI.Controles.BotonFuturista btnAyuda;
        private UI.Controles.BotonFuturista btnLogout;
        private UI.Controles.BotonFuturista btnCambiarContrasena;
        private UI.Controles.BotonFuturista btnLogin;
        private Panel ChangePassPanel;
        private UI.Controles.BotonFuturista btnCancelarMP;
        private UI.Controles.BotonFuturista btnAceptar;
        private TextBox txtRepPass;
        private Label label3;
        private Label label2;
        private TextBox txtNewPass;
        private Label label1;
        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder1;
        private TextBox txtActualPass;
        private Label label5;
        private Label label6;
        private Label label4;
        private ComboBox cmbIdioma;
        private UI.Controles.BotonFuturista btnRespaldo;
        private UI.Controles.BotonFuturista btnGestionarPerfiles;
    }
}
