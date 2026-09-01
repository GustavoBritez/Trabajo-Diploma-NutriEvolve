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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
#region Código generado por el Diseñador

        private void InitializeComponent()
        {
            panelIzquierdo = new Panel();
            cmbIdioma = new ComboBox();
            lblSubtitulo = new Label();
            lblTitulo = new Label();
            panelLogin = new Panel();
            btnCancelar = new Button();
            btnIngresar = new Button();
            txtPassword = new TextBox();
            txtUsuario = new TextBox();
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
            panelIzquierdo.Controls.Add(cmbIdioma);
            panelIzquierdo.Controls.Add(lblSubtitulo);
            panelIzquierdo.Controls.Add(lblTitulo);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Margin = new Padding(2);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new Size(233, 375);
            panelIzquierdo.TabIndex = 0;
            // 
            // cmbIdioma
            // 
            cmbIdioma.BackColor = Color.DarkSeaGreen;
            cmbIdioma.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbIdioma.FormattingEnabled = true;
            cmbIdioma.Items.AddRange(new object[] { "Español", "English", "Portugues" });
            cmbIdioma.Location = new Point(12, 12);
            cmbIdioma.Name = "cmbIdioma";
            cmbIdioma.Size = new Size(164, 29);
            cmbIdioma.TabIndex = 3;
            cmbIdioma.SelectedIndexChanged += cmbIdioma_SelectedIndexChanged;

            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 10F);
            lblSubtitulo.ForeColor = Color.WhiteSmoke;
            lblSubtitulo.Location = new Point(35, 142);
            lblSubtitulo.Margin = new Padding(2, 0, 2, 0);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(126, 19);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Sistema Nutricional";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(31, 90);
            lblTitulo.Margin = new Padding(2, 0, 2, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(184, 41);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "NutriEvolve";
            // 
            // panelLogin
            // 
            panelLogin.BackColor = Color.FromArgb(226, 234, 226);
            panelLogin.Controls.Add(btnCancelar);
            panelLogin.Controls.Add(btnIngresar);
            panelLogin.Controls.Add(txtPassword);
            panelLogin.Controls.Add(txtUsuario);
            panelLogin.Controls.Add(lblPassword);
            panelLogin.Controls.Add(lblUsuario);
            panelLogin.Controls.Add(lblLogin);
            panelLogin.Dock = DockStyle.Fill;
            panelLogin.Location = new Point(233, 0);
            panelLogin.Margin = new Padding(2);
            panelLogin.Name = "panelLogin";
            panelLogin.Size = new Size(389, 375);
            panelLogin.TabIndex = 1;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(78, 122, 84);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(202, 291);
            btnCancelar.Margin = new Padding(2);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(140, 38);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnIngresar
            // 
            btnIngresar.BackColor = Color.FromArgb(78, 122, 84);
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(58, 291);
            btnIngresar.Margin = new Padding(2);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(140, 38);
            btnIngresar.TabIndex = 5;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(58, 221);
            txtPassword.Margin = new Padding(2);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(257, 27);
            txtPassword.TabIndex = 4;
            // 
            // txtUsuario
            // 
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Segoe UI", 11F);
            txtUsuario.Location = new Point(58, 154);
            txtUsuario.Margin = new Padding(2);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(257, 27);
            txtUsuario.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F);
            lblPassword.ForeColor = Color.Black;
            lblPassword.Location = new Point(54, 195);
            lblPassword.Margin = new Padding(2, 0, 2, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(79, 19);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Contraseña";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 10F);
            lblUsuario.ForeColor = Color.Black;
            lblUsuario.Location = new Point(54, 128);
            lblUsuario.Margin = new Padding(2, 0, 2, 0);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(56, 19);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario";
            // 
            // lblLogin
            // 
            lblLogin.AutoSize = true;
            lblLogin.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblLogin.ForeColor = Color.FromArgb(78, 122, 84);
            lblLogin.Location = new Point(117, 52);
            lblLogin.Margin = new Padding(2, 0, 2, 0);
            lblLogin.Name = "lblLogin";
            lblLogin.Size = new Size(99, 37);
            lblLogin.TabIndex = 0;
            lblLogin.Text = "LOGIN";

            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(622, 375);
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
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnIngresar;
        private Button btnCancelar;
        private ComboBox cmbIdioma;

        private Button btnRecalcular;

    }
}
#endregion