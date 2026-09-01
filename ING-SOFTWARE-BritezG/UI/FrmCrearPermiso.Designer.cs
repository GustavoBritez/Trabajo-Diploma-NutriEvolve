namespace UI
{
    partial class FrmCrearPermiso
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            panelCentral = new Panel();
            cmbBotones = new ComboBox();
            lblBoton = new Label();
            cmbFormularios = new ComboBox();
            lblFormulario = new Label();
            txtNombrePermiso = new TextBox();
            lblNombrePermiso = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            panelCentral.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(46, 94, 67);
            lblTitulo.Location = new Point(-92, 30);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(491, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "🛡️ Crear Nuevo Permiso";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelCentral
            // 
            panelCentral.BackColor = Color.FromArgb(180, 180, 180);
            panelCentral.BorderStyle = BorderStyle.FixedSingle;
            panelCentral.Controls.Add(cmbBotones);
            panelCentral.Controls.Add(lblBoton);
            panelCentral.Controls.Add(cmbFormularios);
            panelCentral.Controls.Add(lblFormulario);
            panelCentral.Controls.Add(txtNombrePermiso);
            panelCentral.Controls.Add(lblNombrePermiso);
            panelCentral.Location = new Point(34, 73);
            panelCentral.Name = "panelCentral";
            panelCentral.Size = new Size(365, 200);
            panelCentral.TabIndex = 1;
            // 
            // cmbBotones
            // 
            cmbBotones.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBotones.Font = new Font("Segoe UI", 10F);
            cmbBotones.Location = new Point(23, 158);
            cmbBotones.Name = "cmbBotones";
            cmbBotones.Size = new Size(317, 25);
            cmbBotones.TabIndex = 0;
            // 
            // lblBoton
            // 
            lblBoton.AutoSize = true;
            lblBoton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBoton.ForeColor = Color.Black;
            lblBoton.Location = new Point(19, 135);
            lblBoton.Name = "lblBoton";
            lblBoton.Size = new Size(120, 19);
            lblBoton.TabIndex = 1;
            lblBoton.Text = "Asignar a Botón:";
            // 
            // cmbFormularios
            // 
            cmbFormularios.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFormularios.Font = new Font("Segoe UI", 10F);
            cmbFormularios.Location = new Point(23, 98);
            cmbFormularios.Name = "cmbFormularios";
            cmbFormularios.Size = new Size(317, 25);
            cmbFormularios.TabIndex = 2;
            cmbFormularios.SelectedIndexChanged += cmbFormularios_SelectedIndexChanged;
            // 
            // lblFormulario
            // 
            lblFormulario.AutoSize = true;
            lblFormulario.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFormulario.ForeColor = Color.Black;
            lblFormulario.Location = new Point(19, 75);
            lblFormulario.Name = "lblFormulario";
            lblFormulario.Size = new Size(208, 19);
            lblFormulario.TabIndex = 3;
            lblFormulario.Text = "Asignar a Pantalla (Opcional):";
            // 
            // txtNombrePermiso
            // 
            txtNombrePermiso.Font = new Font("Segoe UI", 10F);
            txtNombrePermiso.Location = new Point(23, 40);
            txtNombrePermiso.Name = "txtNombrePermiso";
            txtNombrePermiso.Size = new Size(317, 25);
            txtNombrePermiso.TabIndex = 1;
            // 
            // lblNombrePermiso
            // 
            lblNombrePermiso.AutoSize = true;
            lblNombrePermiso.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNombrePermiso.ForeColor = Color.Black;
            lblNombrePermiso.Location = new Point(19, 15);
            lblNombrePermiso.Name = "lblNombrePermiso";
            lblNombrePermiso.Size = new Size(212, 19);
            lblNombrePermiso.TabIndex = 0;
            lblNombrePermiso.Text = "Nombre del Permiso (Acción):";
            // 
            // btnAceptar
            // 
            btnAceptar.BackColor = Color.FromArgb(225, 225, 225);
            btnAceptar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAceptar.Location = new Point(271, 290);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(128, 40);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(255, 120, 120);
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.Location = new Point(34, 290);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(128, 40);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmCrearPermiso
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(218, 237, 223);
            ClientSize = new Size(434, 350);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(panelCentral);
            Controls.Add(lblTitulo);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCrearPermiso";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gestión de Permisos";
            Load += FrmCrearPermiso_Load;
            panelCentral.ResumeLayout(false);
            panelCentral.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.TextBox txtNombrePermiso;
        private System.Windows.Forms.Label lblNombrePermiso;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;

        // Declaraciones de los nuevos controles agregados
        private System.Windows.Forms.Label lblFormulario;
        private System.Windows.Forms.ComboBox cmbFormularios;
        private System.Windows.Forms.Label lblBoton;
        private System.Windows.Forms.ComboBox cmbBotones;
    }
}