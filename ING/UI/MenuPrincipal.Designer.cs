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
            btnGestionarPerfiles = new Button();
            btnRespaldo = new Button();
            btnCambiarContrasena = new Button();
            btnLogin = new Button();
            btnLogout = new Button();
            btnAyuda = new Button();
            btnUsuarios = new Button();
            btnReportes = new Button();
            btnSeguimiento = new Button();
            btnTurnos = new Button();
            lblModulo = new Label();
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
            btnCancelarMP = new Button();
            btnAceptar = new Button();
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
            panelMenu.Controls.Add(btnGestionarPerfiles);
            panelMenu.Controls.Add(btnRespaldo);
            panelMenu.Controls.Add(btnCambiarContrasena);
            panelMenu.Controls.Add(btnLogin);
            panelMenu.Controls.Add(btnLogout);
            panelMenu.Controls.Add(btnAyuda);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(btnReportes);
            panelMenu.Controls.Add(btnSeguimiento);
            panelMenu.Controls.Add(btnTurnos);
            panelMenu.Controls.Add(lblModulo);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(259, 700);
            panelMenu.TabIndex = 0;
            // 
            // btnGestionarPerfiles
            // 
            btnGestionarPerfiles.BackColor = Color.FromArgb(76, 124, 89);
            btnGestionarPerfiles.FlatAppearance.BorderSize = 0;
            btnGestionarPerfiles.FlatStyle = FlatStyle.Flat;
            btnGestionarPerfiles.Font = new Font("Segoe UI", 10F);
            btnGestionarPerfiles.ForeColor = Color.White;
            btnGestionarPerfiles.Location = new Point(10, 285);
            btnGestionarPerfiles.Name = "btnGestionarPerfiles";
            btnGestionarPerfiles.Size = new Size(239, 45);
            btnGestionarPerfiles.TabIndex = 10;
            btnGestionarPerfiles.Text = "🔑 Perfiles";
            btnGestionarPerfiles.TextAlign = ContentAlignment.MiddleLeft;
            btnGestionarPerfiles.UseVisualStyleBackColor = false;
            btnGestionarPerfiles.Click += btnGestionarPerfiles_Click;
            // 
            // btnRespaldo
            // 
            btnRespaldo.BackColor = Color.FromArgb(76, 124, 89);
            btnRespaldo.FlatAppearance.BorderSize = 0;
            btnRespaldo.FlatStyle = FlatStyle.Flat;
            btnRespaldo.Font = new Font("Segoe UI", 10F);
            btnRespaldo.ForeColor = Color.White;
            btnRespaldo.Location = new Point(10, 413);
            btnRespaldo.Name = "btnRespaldo";
            btnRespaldo.Size = new Size(239, 45);
            btnRespaldo.TabIndex = 9;
            btnRespaldo.Text = "🔒 Respaldo";
            btnRespaldo.TextAlign = ContentAlignment.MiddleLeft;
            btnRespaldo.UseVisualStyleBackColor = false;
            btnRespaldo.Click += btnRespaldo_Click;
            // 
            // btnCambiarContrasena
            // 
            btnCambiarContrasena.BackColor = Color.FromArgb(78, 122, 84);
            btnCambiarContrasena.FlatStyle = FlatStyle.Flat;
            btnCambiarContrasena.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCambiarContrasena.ForeColor = Color.Transparent;
            btnCambiarContrasena.Location = new Point(11, 564);
            btnCambiarContrasena.Margin = new Padding(2);
            btnCambiarContrasena.Name = "btnCambiarContrasena";
            btnCambiarContrasena.Size = new Size(240, 38);
            btnCambiarContrasena.TabIndex = 8;
            btnCambiarContrasena.Text = "Cambiar Contraseña";
            btnCambiarContrasena.UseVisualStyleBackColor = false;
            btnCambiarContrasena.Click += btnCambiarContrasena_Click;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(78, 122, 84);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(11, 606);
            btnLogin.Margin = new Padding(2);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(240, 38);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "Iniciar Sesión";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(76, 124, 89);
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10F);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(24, 649);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(215, 45);
            btnLogout.TabIndex = 6;
            btnLogout.Text = "🚪 Cerrar Sesión";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnAyuda
            // 
            btnAyuda.BackColor = Color.FromArgb(76, 124, 89);
            btnAyuda.FlatAppearance.BorderSize = 0;
            btnAyuda.FlatStyle = FlatStyle.Flat;
            btnAyuda.Font = new Font("Segoe UI", 10F);
            btnAyuda.ForeColor = Color.White;
            btnAyuda.Location = new Point(9, 464);
            btnAyuda.Name = "btnAyuda";
            btnAyuda.Size = new Size(239, 45);
            btnAyuda.TabIndex = 5;
            btnAyuda.Text = "❓ Ayuda";
            btnAyuda.TextAlign = ContentAlignment.MiddleLeft;
            btnAyuda.UseVisualStyleBackColor = false;
            // 
            // btnUsuarios
            // 
            btnUsuarios.BackColor = Color.FromArgb(76, 124, 89);
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Segoe UI", 10F);
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Location = new Point(10, 225);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(239, 45);
            btnUsuarios.TabIndex = 4;
            btnUsuarios.Text = "👤 Gestión de Usuarios";
            btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.UseVisualStyleBackColor = false;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(76, 124, 89);
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 10F);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(10, 170);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(239, 45);
            btnReportes.TabIndex = 3;
            btnReportes.Text = "📒 Bitacora";
            btnReportes.TextAlign = ContentAlignment.MiddleLeft;
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Click += btnBitacora_Click;
            // 
            // btnSeguimiento
            // 
            btnSeguimiento.BackColor = Color.FromArgb(76, 124, 89);
            btnSeguimiento.FlatAppearance.BorderSize = 0;
            btnSeguimiento.FlatStyle = FlatStyle.Flat;
            btnSeguimiento.Font = new Font("Segoe UI", 10F);
            btnSeguimiento.ForeColor = Color.White;
            btnSeguimiento.Location = new Point(10, 111);
            btnSeguimiento.Name = "btnSeguimiento";
            btnSeguimiento.Size = new Size(241, 45);
            btnSeguimiento.TabIndex = 2;
            btnSeguimiento.Text = "\U0001f957 Seguimiento Nutricional";
            btnSeguimiento.TextAlign = ContentAlignment.MiddleLeft;
            btnSeguimiento.UseVisualStyleBackColor = false;
            // 
            // btnTurnos
            // 
            btnTurnos.BackColor = Color.FromArgb(76, 124, 89);
            btnTurnos.FlatAppearance.BorderSize = 0;
            btnTurnos.FlatStyle = FlatStyle.Flat;
            btnTurnos.Font = new Font("Segoe UI", 10F);
            btnTurnos.ForeColor = Color.White;
            btnTurnos.Location = new Point(10, 60);
            btnTurnos.Name = "btnTurnos";
            btnTurnos.Size = new Size(239, 45);
            btnTurnos.TabIndex = 1;
            btnTurnos.Text = "📅 Gestión de Turnos";
            btnTurnos.TextAlign = ContentAlignment.MiddleLeft;
            btnTurnos.UseVisualStyleBackColor = false;
            btnTurnos.Click += btnTurnos_Click;
            // 
            // lblModulo
            // 
            lblModulo.AutoSize = true;
            lblModulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblModulo.ForeColor = Color.White;
            lblModulo.Location = new Point(24, 25);
            lblModulo.Name = "lblModulo";
            lblModulo.Size = new Size(86, 25);
            lblModulo.TabIndex = 0;
            lblModulo.Text = "Módulos";
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(92, 145, 104);
            panelTop.Controls.Add(cmbIdioma);
            panelTop.Controls.Add(label6);
            panelTop.Controls.Add(label4);
            panelTop.Controls.Add(lblTitulo);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(259, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(951, 60);
            panelTop.TabIndex = 1;
            // 
            // cmbIdioma
            // 
            cmbIdioma.BackColor = Color.DarkSeaGreen;
            cmbIdioma.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbIdioma.FormattingEnabled = true;
            cmbIdioma.Items.AddRange(new object[] { "Español", "English", "Portugues" });
            cmbIdioma.Location = new Point(784, 16);
            cmbIdioma.Name = "cmbIdioma";
            cmbIdioma.Size = new Size(164, 29);
            cmbIdioma.TabIndex = 2;
            cmbIdioma.SelectedIndexChanged += cmdIdioma_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            label6.ForeColor = Color.MistyRose;
            label6.Location = new Point(285, 20);
            label6.Name = "label6";
            label6.Size = new Size(0, 25);
            label6.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            label4.ForeColor = Color.MistyRose;
            label4.Location = new Point(192, 20);
            label4.Name = "label4";
            label4.Size = new Size(87, 25);
            label4.TabIndex = 9;
            label4.Text = "Usuario: ";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(25, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(124, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "NutriEvolve";
            // 
            // panelContenedor
            // 
            panelContenedor.BackColor = Color.FromArgb(225, 240, 228);
            panelContenedor.Controls.Add(label1);
            panelContenedor.Controls.Add(ChangePassPanel);
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(259, 60);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(951, 640);
            panelContenedor.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(675, 512);
            label1.Name = "label1";
            label1.Size = new Size(0, 19);
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
            ChangePassPanel.Location = new Point(25, 412);
            ChangePassPanel.Name = "ChangePassPanel";
            ChangePassPanel.Size = new Size(388, 216);
            ChangePassPanel.TabIndex = 0;
            ChangePassPanel.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(157, 20);
            label5.Name = "label5";
            label5.Size = new Size(185, 28);
            label5.TabIndex = 13;
            label5.Text = "Contraseña Actual";
            // 
            // txtActualPass
            // 
            txtActualPass.Location = new Point(39, 20);
            txtActualPass.Name = "txtActualPass";
            txtActualPass.Size = new Size(100, 23);
            txtActualPass.TabIndex = 11;
            // 
            // btnCancelarMP
            // 
            btnCancelarMP.BackColor = Color.Red;
            btnCancelarMP.FlatStyle = FlatStyle.Flat;
            btnCancelarMP.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancelarMP.ForeColor = Color.Transparent;
            btnCancelarMP.Location = new Point(208, 157);
            btnCancelarMP.Margin = new Padding(2);
            btnCancelarMP.Name = "btnCancelarMP";
            btnCancelarMP.Size = new Size(123, 38);
            btnCancelarMP.TabIndex = 10;
            btnCancelarMP.Text = "Cancelar";
            btnCancelarMP.UseVisualStyleBackColor = false;
            btnCancelarMP.Click += btnCancelar_Click;
            // 
            // btnAceptar
            // 
            btnAceptar.BackColor = Color.FromArgb(76, 124, 99);
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnAceptar.ForeColor = Color.Transparent;
            btnAceptar.Location = new Point(39, 157);
            btnAceptar.Margin = new Padding(2);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(123, 38);
            btnAceptar.TabIndex = 9;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // txtRepPass
            // 
            txtRepPass.Location = new Point(39, 107);
            txtRepPass.Name = "txtRepPass";
            txtRepPass.Size = new Size(100, 23);
            txtRepPass.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(157, 99);
            label3.Name = "label3";
            label3.Size = new Size(193, 28);
            label3.TabIndex = 4;
            label3.Text = "Repetir Contraseña";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(157, 57);
            label2.Name = "label2";
            label2.Size = new Size(185, 28);
            label2.TabIndex = 3;
            label2.Text = "Nueva Contraseña";
            // 
            // txtNewPass
            // 
            txtNewPass.Location = new Point(39, 65);
            txtNewPass.Name = "txtNewPass";
            txtNewPass.Size = new Size(100, 23);
            txtNewPass.TabIndex = 1;
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
            FormBorderStyle = FormBorderStyle.FixedSingle;
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
            //
            // btnActive
            //

        }

        #endregion
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelContenedor;

        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.Label lblTitulo;

        // Botones creados manualmente 
        private FormManager.ButtonActive btnActive;

        private System.Windows.Forms.Button btnTurnos;
        private System.Windows.Forms.Button btnSeguimiento;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnAyuda;
        private System.Windows.Forms.Button btnLogout;
        private Button btnCambiarContrasena;
        private Button btnLogin;
        private Panel ChangePassPanel;
        private Button btnCancelarMP;
        private Button btnAceptar;
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
        private Button btnRespaldo;
        private Button btnGestionarPerfiles;
    }
}
