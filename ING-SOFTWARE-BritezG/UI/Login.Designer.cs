namespace UI
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        private void InitializeComponent()
        {
            panelIzquierdo = new Panel();
            lblVersion = new Label();
            cmbIdioma = new ComboBox();
            lblTitulo = new Label();
            panelLogin = new Panel();
            btnCerrarForm = new Button();
            lblLoginSub = new Label();
            btnCancelar = new UI.Controles.BotonFuturista();
            btnIngresar = new UI.Controles.BotonFuturista();
            txtPassword = new UI.Controles.TextBoxOvalado();
            txtUsuario = new UI.Controles.TextBoxOvalado();
            lblPassword = new Label();
            lblUsuario = new Label();
            lblLogin = new Label();
            panelIzquierdo.SuspendLayout();
            panelLogin.SuspendLayout();
            SuspendLayout();
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = Color.FromArgb(78, 122, 84);
            panelIzquierdo.Controls.Add(lblVersion);
            panelIzquierdo.Controls.Add(cmbIdioma);
            panelIzquierdo.Controls.Add(lblTitulo);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Margin = new Padding(2);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new Size(260, 420);
            panelIzquierdo.TabIndex = 0;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.FromArgb(195, 225, 200);
            lblVersion.Location = new Point(24, 385);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(144, 13);
            lblVersion.TabIndex = 8;
            lblVersion.Text = "v1.0.0 — Edición Pediátrica";
            // 
            // cmbIdioma
            // 
            cmbIdioma.BackColor = Color.DarkSeaGreen;
            cmbIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdioma.FlatStyle = FlatStyle.Flat;
            cmbIdioma.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            cmbIdioma.ForeColor = Color.FromArgb(25, 45, 28);
            cmbIdioma.FormattingEnabled = true;
            cmbIdioma.Items.AddRange(new object[] { "Español", "English", "Portugues" });
            cmbIdioma.Location = new Point(24, 22);
            cmbIdioma.Name = "cmbIdioma";
            cmbIdioma.Size = new Size(140, 25);
            cmbIdioma.TabIndex = 3;
            cmbIdioma.SelectedIndexChanged += cmbIdioma_SelectedIndexChanged;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(18, 100);
            lblTitulo.Margin = new Padding(2, 0, 2, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(195, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "NutriEvolve";
            // 
            // panelLogin
            // 
            panelLogin.BackColor = Color.FromArgb(226, 234, 226);
            panelLogin.Controls.Add(btnCerrarForm);
            panelLogin.Controls.Add(lblLoginSub);
            panelLogin.Controls.Add(btnCancelar);
            panelLogin.Controls.Add(btnIngresar);
            panelLogin.Controls.Add(txtPassword);
            panelLogin.Controls.Add(txtUsuario);
            panelLogin.Controls.Add(lblPassword);
            panelLogin.Controls.Add(lblUsuario);
            panelLogin.Controls.Add(lblLogin);
            panelLogin.Dock = DockStyle.Fill;
            panelLogin.Location = new Point(260, 0);
            panelLogin.Margin = new Padding(2);
            panelLogin.Name = "panelLogin";
            panelLogin.Size = new Size(420, 420);
            panelLogin.TabIndex = 1;
            // 
            // btnCerrarForm
            // 
            btnCerrarForm.BackColor = Color.Transparent;
            btnCerrarForm.Cursor = Cursors.Hand;
            btnCerrarForm.FlatAppearance.BorderSize = 0;
            btnCerrarForm.FlatAppearance.MouseDownBackColor = Color.FromArgb(210, 170, 170);
            btnCerrarForm.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 195, 195);
            btnCerrarForm.FlatStyle = FlatStyle.Flat;
            btnCerrarForm.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCerrarForm.ForeColor = Color.FromArgb(120, 145, 125);
            btnCerrarForm.Location = new Point(375, 12);
            btnCerrarForm.Name = "btnCerrarForm";
            btnCerrarForm.Size = new Size(32, 32);
            btnCerrarForm.TabIndex = 9;
            btnCerrarForm.Text = "✕";
            btnCerrarForm.UseVisualStyleBackColor = false;
            btnCerrarForm.Click += btnCerrarForm_Click;
            // 
            // lblLoginSub
            // 
            lblLoginSub.AutoSize = true;
            lblLoginSub.Font = new Font("Segoe UI", 9.5F);
            lblLoginSub.ForeColor = Color.FromArgb(95, 125, 100);
            lblLoginSub.Location = new Point(50, 78);
            lblLoginSub.Name = "lblLoginSub";
            lblLoginSub.Size = new Size(213, 17);
            lblLoginSub.TabIndex = 7;
            lblLoginSub.Text = "Ingrese sus credenciales de acceso";
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Transparent;
            btnCancelar.ColorGlow = Color.DarkSeaGreen;
            btnCancelar.ColorPrincipal = Color.FromArgb(78, 122, 84);
            btnCancelar.ColorSecundario = Color.FromArgb(50, 82, 55);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.EsBotonSecundario = true;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(215, 290);
            btnCancelar.Margin = new Padding(2);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.RadioBorde = 44;
            btnCancelar.Size = new Size(155, 44);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnIngresar
            // 
            btnIngresar.BackColor = Color.Transparent;
            btnIngresar.ColorGlow = Color.DarkSeaGreen;
            btnIngresar.ColorPrincipal = Color.FromArgb(78, 122, 84);
            btnIngresar.ColorSecundario = Color.FromArgb(50, 82, 55);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.EsBotonSecundario = false;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(50, 290);
            btnIngresar.Margin = new Padding(2);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.RadioBorde = 44;
            btnIngresar.Size = new Size(155, 44);
            btnIngresar.TabIndex = 5;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.Transparent;
            txtPassword.ColorBordeFocus = Color.FromArgb(78, 122, 84);
            txtPassword.ColorBordeNormal = Color.FromArgb(175, 198, 178);
            txtPassword.ColorFondo = Color.White;
            txtPassword.ColorGlowFocus = Color.DarkSeaGreen;
            txtPassword.Location = new Point(50, 218);
            txtPassword.Margin = new Padding(2);
            txtPassword.Name = "txtPassword";
            txtPassword.Padding = new Padding(16, 11, 16, 10);
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderText = "••••••••";
            txtPassword.RadioBorde = 34;
            txtPassword.Size = new Size(320, 42);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = false;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.Transparent;
            txtUsuario.ColorBordeFocus = Color.FromArgb(78, 122, 84);
            txtUsuario.ColorBordeNormal = Color.FromArgb(175, 198, 178);
            txtUsuario.ColorFondo = Color.White;
            txtUsuario.ColorGlowFocus = Color.DarkSeaGreen;
            txtUsuario.Location = new Point(50, 138);
            txtUsuario.Margin = new Padding(2);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Padding = new Padding(16, 11, 16, 10);
            txtUsuario.PasswordChar = '\0';
            txtUsuario.PlaceholderText = "Nombre de usuario";
            txtUsuario.RadioBorde = 34;
            txtUsuario.Size = new Size(320, 42);
            txtUsuario.TabIndex = 3;
            txtUsuario.UseSystemPasswordChar = false;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(40, 68, 45);
            lblPassword.Location = new Point(52, 196);
            lblPassword.Margin = new Padding(2, 0, 2, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(77, 17);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Contraseña";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblUsuario.ForeColor = Color.FromArgb(40, 68, 45);
            lblUsuario.Location = new Point(52, 116);
            lblUsuario.Margin = new Padding(2, 0, 2, 0);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(54, 17);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario";
            // 
            // lblLogin
            // 
            lblLogin.AutoSize = true;
            lblLogin.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblLogin.ForeColor = Color.FromArgb(45, 80, 52);
            lblLogin.Location = new Point(48, 42);
            lblLogin.Margin = new Padding(2, 0, 2, 0);
            lblLogin.Name = "lblLogin";
            lblLogin.Size = new Size(196, 32);
            lblLogin.TabIndex = 0;
            lblLogin.Text = "INICIAR SESIÓN";
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(226, 234, 226);
            ClientSize = new Size(680, 420);
            Controls.Add(panelLogin);
            Controls.Add(panelIzquierdo);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema NutriEvolve";
            Load += Login_Load;
            panelIzquierdo.ResumeLayout(false);
            panelIzquierdo.PerformLayout();
            panelLogin.ResumeLayout(false);
            panelLogin.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblLoginSub;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnCerrarForm;
        private UI.Controles.TextBoxOvalado txtUsuario;
        private UI.Controles.TextBoxOvalado txtPassword;
        private UI.Controles.BotonFuturista btnIngresar;
        private UI.Controles.BotonFuturista btnCancelar;
        private ComboBox cmbIdioma;
    }
}