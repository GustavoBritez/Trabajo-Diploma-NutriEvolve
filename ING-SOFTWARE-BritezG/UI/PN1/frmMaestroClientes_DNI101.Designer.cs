namespace UI.PN1
{
    partial class frmMaestroClientes_DNI101
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
            System.Windows.Forms.DataGridViewCellStyle dgvStyleHeader = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleRows = new System.Windows.Forms.DataGridViewCellStyle();

            lblTituloSuperior = new System.Windows.Forms.Label();
            lblTituloMaestro = new System.Windows.Forms.Label();

            dgvClientes = new System.Windows.Forms.DataGridView();

            panelCampos = new System.Windows.Forms.Panel();
            lblDni = new System.Windows.Forms.Label();
            txtDni = new System.Windows.Forms.TextBox();
            lblApellidos = new System.Windows.Forms.Label();
            txtApellidos = new System.Windows.Forms.TextBox();
            lblNombres = new System.Windows.Forms.Label();
            txtNombres = new System.Windows.Forms.TextBox();
            lblEmail = new System.Windows.Forms.Label();
            txtEmail = new System.Windows.Forms.TextBox();
            btnGestionEmail1N = new System.Windows.Forms.Button();
            lblCelular = new System.Windows.Forms.Label();
            txtCelular = new System.Windows.Forms.TextBox();
            btnGestionCelular1N = new System.Windows.Forms.Button();
            lblDireccion = new System.Windows.Forms.Label();
            txtDireccion = new System.Windows.Forms.TextBox();

            btnAnadir = new UI.Controles.BotonFuturista();
            btnModificar = new UI.Controles.BotonFuturista();
            btnEliminar = new UI.Controles.BotonFuturista();
            btnAplicar = new UI.Controles.BotonFuturista();
            btnCancelar = new UI.Controles.BotonFuturista();
            btnSalir = new UI.Controles.BotonFuturista();
            btnAyudaContextual = new UI.Controles.BotonFuturista();

            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            panelCampos.SuspendLayout();
            SuspendLayout();

            // lblTituloSuperior
            lblTituloSuperior.AutoSize = true;
            lblTituloSuperior.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblTituloSuperior.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            lblTituloSuperior.Location = new System.Drawing.Point(20, 12);
            lblTituloSuperior.Name = "lblTituloSuperior";
            lblTituloSuperior.Size = new System.Drawing.Size(460, 17);
            lblTituloSuperior.TabIndex = 0;
            lblTituloSuperior.Text = "Modelo de GUI ABM-C clasico para el mantenimiento de Entidades tipo Maestro.";

            // lblTituloMaestro
            lblTituloMaestro.AutoSize = true;
            lblTituloMaestro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTituloMaestro.ForeColor = System.Drawing.Color.Black;
            lblTituloMaestro.Location = new System.Drawing.Point(20, 40);
            lblTituloMaestro.Name = "lblTituloMaestro";
            lblTituloMaestro.Size = new System.Drawing.Size(260, 21);
            lblTituloMaestro.TabIndex = 1;
            lblTituloMaestro.Text = "MAESTRO DE CLIENTES - ABM";

            // dgvClientes
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            dgvClientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.BackgroundColor = System.Drawing.Color.White;
            dgvClientes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            dgvStyleHeader.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgvStyleHeader.BackColor = System.Drawing.Color.FromArgb(0, 220, 240); // Cyan header matching excel
            dgvStyleHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvStyleHeader.ForeColor = System.Drawing.Color.Black;
            dgvClientes.ColumnHeadersDefaultCellStyle = dgvStyleHeader;
            dgvClientes.ColumnHeadersHeight = 28;
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.Location = new System.Drawing.Point(20, 68);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvStyleRows.Font = new System.Drawing.Font("Segoe UI", 9F);
            dgvClientes.RowsDefaultCellStyle = dgvStyleRows;
            dgvClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new System.Drawing.Size(650, 175);
            dgvClientes.TabIndex = 2;
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;

            // btnAyudaContextual
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(685, 25);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(120, 32);
            btnAyudaContextual.TabIndex = 10;
            btnAyudaContextual.Text = "? Ayuda";
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // Botones grupo superior (Añadir, Modificar, Eliminar)
            btnAnadir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnAnadir.Location = new System.Drawing.Point(685, 75);
            btnAnadir.Name = "btnAnadir";
            btnAnadir.Size = new System.Drawing.Size(120, 32);
            btnAnadir.TabIndex = 3;
            btnAnadir.Text = "Añadir";
            btnAnadir.Click += btnAnadir_Click;

            btnModificar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnModificar.Location = new System.Drawing.Point(685, 115);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new System.Drawing.Size(120, 32);
            btnModificar.TabIndex = 4;
            btnModificar.Text = "Modificar";
            btnModificar.Click += btnModificar_Click;

            btnEliminar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnEliminar.EsBotonSecundario = true;
            btnEliminar.Location = new System.Drawing.Point(685, 155);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new System.Drawing.Size(120, 32);
            btnEliminar.TabIndex = 5;
            btnEliminar.Text = "Eliminar";
            btnEliminar.Click += btnEliminar_Click;

            // Botones grupo inferior (Aplicar, Cancelar, Salir)
            btnAplicar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnAplicar.Location = new System.Drawing.Point(685, 270);
            btnAplicar.Name = "btnAplicar";
            btnAplicar.Size = new System.Drawing.Size(120, 32);
            btnAplicar.TabIndex = 6;
            btnAplicar.Text = "Aplicar";
            btnAplicar.Click += btnAplicar_Click;

            btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnCancelar.Location = new System.Drawing.Point(685, 310);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new System.Drawing.Size(120, 32);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;

            btnSalir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnSalir.Location = new System.Drawing.Point(685, 360);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(120, 32);
            btnSalir.TabIndex = 8;
            btnSalir.Text = "Salir";
            btnSalir.Click += btnSalir_Click;

            // panelCampos (Lower input boxes with green label badges matching Excel)
            panelCampos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelCampos.Controls.Add(lblDni);
            panelCampos.Controls.Add(txtDni);
            panelCampos.Controls.Add(lblApellidos);
            panelCampos.Controls.Add(txtApellidos);
            panelCampos.Controls.Add(lblNombres);
            panelCampos.Controls.Add(txtNombres);
            panelCampos.Controls.Add(lblEmail);
            panelCampos.Controls.Add(txtEmail);
            panelCampos.Controls.Add(btnGestionEmail1N);
            panelCampos.Controls.Add(lblCelular);
            panelCampos.Controls.Add(txtCelular);
            panelCampos.Controls.Add(btnGestionCelular1N);
            panelCampos.Controls.Add(lblDireccion);
            panelCampos.Controls.Add(txtDireccion);
            panelCampos.Location = new System.Drawing.Point(20, 260);
            panelCampos.Name = "panelCampos";
            panelCampos.Size = new System.Drawing.Size(650, 160);
            panelCampos.TabIndex = 9;

            // Row 1: DNI
            lblDni.BackColor = System.Drawing.Color.FromArgb(0, 230, 0); // Green label matching Excel
            lblDni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblDni.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDni.Location = new System.Drawing.Point(0, 0);
            lblDni.Name = "lblDni";
            lblDni.Size = new System.Drawing.Size(130, 26);
            lblDni.TabIndex = 0;
            lblDni.Text = " DNI:";
            lblDni.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtDni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDni.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtDni.Location = new System.Drawing.Point(130, 0);
            txtDni.Name = "txtDni";
            txtDni.Size = new System.Drawing.Size(518, 23);
            txtDni.TabIndex = 1;

            // Row 2: Apellidos
            lblApellidos.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblApellidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblApellidos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblApellidos.Location = new System.Drawing.Point(0, 26);
            lblApellidos.Name = "lblApellidos";
            lblApellidos.Size = new System.Drawing.Size(130, 26);
            lblApellidos.TabIndex = 2;
            lblApellidos.Text = " Apellidos:";
            lblApellidos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtApellidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtApellidos.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtApellidos.Location = new System.Drawing.Point(130, 26);
            txtApellidos.Name = "txtApellidos";
            txtApellidos.Size = new System.Drawing.Size(518, 23);
            txtApellidos.TabIndex = 3;

            // Row 3: Nombres
            lblNombres.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblNombres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblNombres.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblNombres.Location = new System.Drawing.Point(0, 52);
            lblNombres.Name = "lblNombres";
            lblNombres.Size = new System.Drawing.Size(130, 26);
            lblNombres.TabIndex = 4;
            lblNombres.Text = " Nombres:";
            lblNombres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtNombres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtNombres.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtNombres.Location = new System.Drawing.Point(130, 52);
            txtNombres.Name = "txtNombres";
            txtNombres.Size = new System.Drawing.Size(518, 23);
            txtNombres.TabIndex = 5;

            // Row 4: Email
            lblEmail.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEmail.Location = new System.Drawing.Point(0, 78);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new System.Drawing.Size(130, 26);
            lblEmail.TabIndex = 6;
            lblEmail.Text = " Email:";
            lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtEmail.Location = new System.Drawing.Point(130, 78);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new System.Drawing.Size(438, 23);
            txtEmail.TabIndex = 7;

            btnGestionEmail1N.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            btnGestionEmail1N.Location = new System.Drawing.Point(568, 77);
            btnGestionEmail1N.Name = "btnGestionEmail1N";
            btnGestionEmail1N.Size = new System.Drawing.Size(80, 25);
            btnGestionEmail1N.TabIndex = 8;
            btnGestionEmail1N.Text = "Emails 1:N";
            btnGestionEmail1N.UseVisualStyleBackColor = true;
            btnGestionEmail1N.Click += btnGestionContactos1N_Click;

            // Row 5: Celular
            lblCelular.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblCelular.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCelular.Location = new System.Drawing.Point(0, 104);
            lblCelular.Name = "lblCelular";
            lblCelular.Size = new System.Drawing.Size(130, 26);
            lblCelular.TabIndex = 9;
            lblCelular.Text = " Celular:";
            lblCelular.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCelular.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtCelular.Location = new System.Drawing.Point(130, 104);
            txtCelular.Name = "txtCelular";
            txtCelular.Size = new System.Drawing.Size(438, 23);
            txtCelular.TabIndex = 10;

            btnGestionCelular1N.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            btnGestionCelular1N.Location = new System.Drawing.Point(568, 103);
            btnGestionCelular1N.Name = "btnGestionCelular1N";
            btnGestionCelular1N.Size = new System.Drawing.Size(80, 25);
            btnGestionCelular1N.TabIndex = 11;
            btnGestionCelular1N.Text = "Tels 1:N";
            btnGestionCelular1N.UseVisualStyleBackColor = true;
            btnGestionCelular1N.Click += btnGestionContactos1N_Click;

            // Row 6: Dirección
            lblDireccion.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblDireccion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDireccion.Location = new System.Drawing.Point(0, 130);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new System.Drawing.Size(130, 28);
            lblDireccion.TabIndex = 12;
            lblDireccion.Text = " Dirección:";
            lblDireccion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDireccion.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtDireccion.Location = new System.Drawing.Point(130, 130);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new System.Drawing.Size(518, 23);
            txtDireccion.TabIndex = 13;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(830, 440);
            Controls.Add(panelCampos);
            Controls.Add(btnSalir);
            Controls.Add(btnCancelar);
            Controls.Add(btnAplicar);
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(btnAnadir);
            Controls.Add(btnAyudaContextual);
            Controls.Add(dgvClientes);
            Controls.Add(lblTituloMaestro);
            Controls.Add(lblTituloSuperior);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmMaestroClientes_DNI101";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Maestro de Clientes - ABM";
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            panelCampos.ResumeLayout(false);
            panelCampos.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTituloSuperior;
        private System.Windows.Forms.Label lblTituloMaestro;
        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.Panel panelCampos;
        private System.Windows.Forms.Label lblDni;
        private System.Windows.Forms.TextBox txtDni;
        private System.Windows.Forms.Label lblApellidos;
        private System.Windows.Forms.TextBox txtApellidos;
        private System.Windows.Forms.Label lblNombres;
        private System.Windows.Forms.TextBox txtNombres;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnGestionEmail1N;
        private System.Windows.Forms.Label lblCelular;
        private System.Windows.Forms.TextBox txtCelular;
        private System.Windows.Forms.Button btnGestionCelular1N;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;

        private UI.Controles.BotonFuturista btnAnadir;
        private UI.Controles.BotonFuturista btnModificar;
        private UI.Controles.BotonFuturista btnEliminar;
        private UI.Controles.BotonFuturista btnAplicar;
        private UI.Controles.BotonFuturista btnCancelar;
        private UI.Controles.BotonFuturista btnSalir;
        private UI.Controles.BotonFuturista btnAyudaContextual;
    }
}
