namespace UI.PN1
{
    partial class frmContactos1N_DNI101
    {
        private System.ComponentModel.IContainer components = null;

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
            panelHeader = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblPaciente = new System.Windows.Forms.Label();
            
            gbEmails = new System.Windows.Forms.GroupBox();
            dgvEmails = new System.Windows.Forms.DataGridView();
            txtNuevoEmail = new System.Windows.Forms.TextBox();
            chkEmailPrincipal = new System.Windows.Forms.CheckBox();
            btnAgregarEmail = new UI.Controles.BotonFuturista();
            btnEliminarEmail = new UI.Controles.BotonFuturista();

            gbTelefonos = new System.Windows.Forms.GroupBox();
            dgvTelefonos = new System.Windows.Forms.DataGridView();
            txtNuevoTelefono = new System.Windows.Forms.TextBox();
            cmbTipoTel = new System.Windows.Forms.ComboBox();
            chkTelPrincipal = new System.Windows.Forms.CheckBox();
            btnAgregarTel = new UI.Controles.BotonFuturista();
            btnEliminarTel = new UI.Controles.BotonFuturista();

            btnCerrar = new UI.Controles.BotonFuturista();

            panelHeader.SuspendLayout();
            gbEmails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmails).BeginInit();
            gbTelefonos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTelefonos).BeginInit();
            SuspendLayout();

            btnAyudaContextual = new UI.Controles.BotonFuturista();

            // panelHeader
            panelHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblPaciente);
            panelHeader.Controls.Add(btnAyudaContextual);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(720, 65);
            panelHeader.TabIndex = 0;

            // btnAyudaContextual
            btnAyudaContextual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(610, 15);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(95, 35);
            btnAyudaContextual.TabIndex = 2;
            btnAyudaContextual.Text = "? Ayuda";
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // lblTitulo
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(18, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(460, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Contactos 1:N (Múltiples Emails y Celulares)";

            // lblPaciente
            lblPaciente.AutoSize = true;
            lblPaciente.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblPaciente.ForeColor = System.Drawing.Color.FromArgb(235, 245, 235);
            lblPaciente.Location = new System.Drawing.Point(20, 36);
            lblPaciente.Name = "lblPaciente";
            lblPaciente.Size = new System.Drawing.Size(120, 17);
            lblPaciente.TabIndex = 1;
            lblPaciente.Text = "Cliente / Paciente: ";

            // gbEmails
            gbEmails.Controls.Add(dgvEmails);
            gbEmails.Controls.Add(txtNuevoEmail);
            gbEmails.Controls.Add(chkEmailPrincipal);
            gbEmails.Controls.Add(btnAgregarEmail);
            gbEmails.Controls.Add(btnEliminarEmail);
            gbEmails.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbEmails.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbEmails.Location = new System.Drawing.Point(18, 75);
            gbEmails.Name = "gbEmails";
            gbEmails.Size = new System.Drawing.Size(744, 185);
            gbEmails.TabIndex = 1;
            gbEmails.TabStop = false;
            gbEmails.Text = "Correos Electrónicos (Tabla 1:N)";

            // dgvEmails
            dgvEmails.AllowUserToAddRows = false;
            dgvEmails.AllowUserToDeleteRows = false;
            dgvEmails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmails.BackgroundColor = System.Drawing.Color.White;
            dgvEmails.ColumnHeadersHeight = 28;
            dgvEmails.Location = new System.Drawing.Point(15, 25);
            dgvEmails.MultiSelect = false;
            dgvEmails.Name = "dgvEmails";
            dgvEmails.ReadOnly = true;
            dgvEmails.RowHeadersVisible = false;
            dgvEmails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvEmails.Size = new System.Drawing.Size(714, 105);
            dgvEmails.TabIndex = 0;

            // txtNuevoEmail
            txtNuevoEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtNuevoEmail.Location = new System.Drawing.Point(15, 142);
            txtNuevoEmail.Name = "txtNuevoEmail";
            txtNuevoEmail.PlaceholderText = "Ingrese nuevo email (ej: paciente@correo.com)";
            txtNuevoEmail.Size = new System.Drawing.Size(265, 24);
            txtNuevoEmail.TabIndex = 1;

            // chkEmailPrincipal
            chkEmailPrincipal.AutoSize = true;
            chkEmailPrincipal.Location = new System.Drawing.Point(290, 144);
            chkEmailPrincipal.Name = "chkEmailPrincipal";
            chkEmailPrincipal.Size = new System.Drawing.Size(175, 21);
            chkEmailPrincipal.TabIndex = 2;
            chkEmailPrincipal.Text = "Principal";

            // btnAgregarEmail
            btnAgregarEmail.Location = new System.Drawing.Point(475, 138);
            btnAgregarEmail.Name = "btnAgregarEmail";
            btnAgregarEmail.Size = new System.Drawing.Size(120, 32);
            btnAgregarEmail.TabIndex = 3;
            btnAgregarEmail.Text = "Agregar Email";
            btnAgregarEmail.Click += btnAgregarEmail_Click;

            // btnEliminarEmail
            btnEliminarEmail.EsBotonSecundario = true;
            btnEliminarEmail.Location = new System.Drawing.Point(605, 138);
            btnEliminarEmail.Name = "btnEliminarEmail";
            btnEliminarEmail.Size = new System.Drawing.Size(120, 32);
            btnEliminarEmail.TabIndex = 4;
            btnEliminarEmail.Text = "Baja Lógica";
            btnEliminarEmail.Click += btnEliminarEmail_Click;

            // gbTelefonos
            gbTelefonos.Controls.Add(dgvTelefonos);
            gbTelefonos.Controls.Add(txtNuevoTelefono);
            gbTelefonos.Controls.Add(cmbTipoTel);
            gbTelefonos.Controls.Add(chkTelPrincipal);
            gbTelefonos.Controls.Add(btnAgregarTel);
            gbTelefonos.Controls.Add(btnEliminarTel);
            gbTelefonos.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbTelefonos.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbTelefonos.Location = new System.Drawing.Point(18, 270);
            gbTelefonos.Name = "gbTelefonos";
            gbTelefonos.Size = new System.Drawing.Size(744, 185);
            gbTelefonos.TabIndex = 2;
            gbTelefonos.TabStop = false;
            gbTelefonos.Text = "Números Telefónicos / Celulares (Tabla 1:N)";

            // dgvTelefonos
            dgvTelefonos.AllowUserToAddRows = false;
            dgvTelefonos.AllowUserToDeleteRows = false;
            dgvTelefonos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvTelefonos.BackgroundColor = System.Drawing.Color.White;
            dgvTelefonos.ColumnHeadersHeight = 28;
            dgvTelefonos.Location = new System.Drawing.Point(15, 25);
            dgvTelefonos.MultiSelect = false;
            dgvTelefonos.Name = "dgvTelefonos";
            dgvTelefonos.ReadOnly = true;
            dgvTelefonos.RowHeadersVisible = false;
            dgvTelefonos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvTelefonos.Size = new System.Drawing.Size(714, 105);
            dgvTelefonos.TabIndex = 0;

            // txtNuevoTelefono
            txtNuevoTelefono.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtNuevoTelefono.Location = new System.Drawing.Point(15, 142);
            txtNuevoTelefono.Name = "txtNuevoTelefono";
            txtNuevoTelefono.PlaceholderText = "Número (ej: 11-4455-6677)";
            txtNuevoTelefono.Size = new System.Drawing.Size(160, 24);
            txtNuevoTelefono.TabIndex = 1;

            // cmbTipoTel
            cmbTipoTel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbTipoTel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cmbTipoTel.FormattingEnabled = true;
            cmbTipoTel.Items.AddRange(new object[] { "Celular", "Fijo Particular", "Laboral / Urgencias" });
            cmbTipoTel.Location = new System.Drawing.Point(185, 142);
            cmbTipoTel.Name = "cmbTipoTel";
            cmbTipoTel.Size = new System.Drawing.Size(95, 24);
            cmbTipoTel.TabIndex = 2;

            // chkTelPrincipal
            chkTelPrincipal.AutoSize = true;
            chkTelPrincipal.Location = new System.Drawing.Point(290, 144);
            chkTelPrincipal.Name = "chkTelPrincipal";
            chkTelPrincipal.Size = new System.Drawing.Size(175, 21);
            chkTelPrincipal.TabIndex = 3;
            chkTelPrincipal.Text = "Principal";

            // btnAgregarTel
            btnAgregarTel.Location = new System.Drawing.Point(475, 138);
            btnAgregarTel.Name = "btnAgregarTel";
            btnAgregarTel.Size = new System.Drawing.Size(120, 32);
            btnAgregarTel.TabIndex = 4;
            btnAgregarTel.Text = "Agregar Tel.";
            btnAgregarTel.Click += btnAgregarTel_Click;

            // btnEliminarTel
            btnEliminarTel.EsBotonSecundario = true;
            btnEliminarTel.Location = new System.Drawing.Point(605, 138);
            btnEliminarTel.Name = "btnEliminarTel";
            btnEliminarTel.Size = new System.Drawing.Size(120, 32);
            btnEliminarTel.TabIndex = 5;
            btnEliminarTel.Text = "Baja Lógica";
            btnEliminarTel.Click += btnEliminarTel_Click;

            // btnCerrar
            btnCerrar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnCerrar.Location = new System.Drawing.Point(642, 465);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new System.Drawing.Size(120, 36);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 248, 245);
            ClientSize = new System.Drawing.Size(780, 515);
            Controls.Add(btnCerrar);
            Controls.Add(gbTelefonos);
            Controls.Add(gbEmails);
            Controls.Add(panelHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmContactos1N_DNI101";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Contactos del Cliente (Tablas 1:N)";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            gbEmails.ResumeLayout(false);
            gbEmails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmails).EndInit();
            gbTelefonos.ResumeLayout(false);
            gbTelefonos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTelefonos).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblPaciente;
        private System.Windows.Forms.GroupBox gbEmails;
        private System.Windows.Forms.DataGridView dgvEmails;
        private System.Windows.Forms.TextBox txtNuevoEmail;
        private System.Windows.Forms.CheckBox chkEmailPrincipal;
        private UI.Controles.BotonFuturista btnAgregarEmail;
        private UI.Controles.BotonFuturista btnEliminarEmail;
        private System.Windows.Forms.GroupBox gbTelefonos;
        private System.Windows.Forms.DataGridView dgvTelefonos;
        private System.Windows.Forms.TextBox txtNuevoTelefono;
        private System.Windows.Forms.ComboBox cmbTipoTel;
        private System.Windows.Forms.CheckBox chkTelPrincipal;
        private UI.Controles.BotonFuturista btnAgregarTel;
        private UI.Controles.BotonFuturista btnEliminarTel;
        private UI.Controles.BotonFuturista btnCerrar;
        private UI.Controles.BotonFuturista btnAyudaContextual;
    }
}
