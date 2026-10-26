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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleHeader = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleRows = new System.Windows.Forms.DataGridViewCellStyle();

            toolTipGeneral = new System.Windows.Forms.ToolTip(components);

            lblTituloSuperior = new System.Windows.Forms.Label();
            lblTituloMaestro = new System.Windows.Forms.Label();

            dgvClientes = new System.Windows.Forms.DataGridView();
            colDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colApellidos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNombres = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCelular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDireccion = new System.Windows.Forms.DataGridViewTextBoxColumn();

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

            panelMensaje = new System.Windows.Forms.Panel();
            lblMensaje = new System.Windows.Forms.Label();
            txtMensaje = new System.Windows.Forms.TextBox();

            btnActualizar = new UI.Controles.BotonFuturista();
            btnAnadir = new UI.Controles.BotonFuturista();
            btnModificar = new UI.Controles.BotonFuturista();
            btnEliminar = new UI.Controles.BotonFuturista();
            btnAplicar = new UI.Controles.BotonFuturista();
            btnCancelar = new UI.Controles.BotonFuturista();
            btnSalir = new UI.Controles.BotonFuturista();
            btnAyudaContextual = new UI.Controles.BotonFuturista();

            panelSeparador = new System.Windows.Forms.Panel();
            btnSerializar = new System.Windows.Forms.Button();
            txtRutaSerializar = new System.Windows.Forms.TextBox();
            btnExaminarSerializar = new System.Windows.Forms.Button();

            btnDesSerializar = new System.Windows.Forms.Button();
            txtRutaDesSerializar = new System.Windows.Forms.TextBox();
            btnExaminarDesSerializar = new System.Windows.Forms.Button();

            btnLimpiar = new UI.Controles.BotonFuturista();

            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            panelCampos.SuspendLayout();
            panelMensaje.SuspendLayout();
            SuspendLayout();

            // 
            // lblTituloSuperior
            // 
            lblTituloSuperior.AutoSize = true;
            lblTituloSuperior.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblTituloSuperior.ForeColor = System.Drawing.Color.FromArgb(30, 60, 40);
            lblTituloSuperior.Location = new System.Drawing.Point(20, 8);
            lblTituloSuperior.Name = "lblTituloSuperior";
            lblTituloSuperior.Size = new System.Drawing.Size(510, 17);
            lblTituloSuperior.TabIndex = 0;
            lblTituloSuperior.Text = "Modelo de GUI para SERIALIZACIÓN sobre una operación de ABM-C";

            // 
            // lblTituloMaestro
            // 
            lblTituloMaestro.AutoSize = true;
            lblTituloMaestro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTituloMaestro.ForeColor = System.Drawing.Color.Black;
            lblTituloMaestro.Location = new System.Drawing.Point(20, 30);
            lblTituloMaestro.Name = "lblTituloMaestro";
            lblTituloMaestro.Size = new System.Drawing.Size(260, 21);
            lblTituloMaestro.TabIndex = 1;
            lblTituloMaestro.Text = "MAESTRO DE CLIENTES - ABM";

            // 
            // dgvClientes
            // 
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            dgvClientes.BackgroundColor = System.Drawing.Color.White;
            dgvClientes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            dgvStyleHeader.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgvStyleHeader.BackColor = System.Drawing.Color.FromArgb(0, 220, 240); // Cyan header matching excel
            dgvStyleHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvStyleHeader.ForeColor = System.Drawing.Color.Black;
            dgvClientes.ColumnHeadersDefaultCellStyle = dgvStyleHeader;
            dgvClientes.ColumnHeadersHeight = 28;
            dgvClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                colDni,
                colApellidos,
                colNombres,
                colEmail,
                colCelular,
                colDireccion
            });
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.Location = new System.Drawing.Point(20, 58);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvStyleRows.Font = new System.Drawing.Font("Segoe UI", 9F);
            dgvClientes.RowsDefaultCellStyle = dgvStyleRows;
            dgvClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new System.Drawing.Size(760, 175);
            dgvClientes.TabIndex = 2;
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;

            // 
            // colDni
            // 
            colDni.DataPropertyName = "DNI";
            colDni.HeaderText = "DNI";
            colDni.Name = "colDni";
            colDni.ReadOnly = true;
            colDni.Width = 90;

            // 
            // colApellidos
            // 
            colApellidos.DataPropertyName = "APELLIDOS";
            colApellidos.HeaderText = "APELLIDOS";
            colApellidos.Name = "colApellidos";
            colApellidos.ReadOnly = true;
            colApellidos.Width = 130;

            // 
            // colNombres
            // 
            colNombres.DataPropertyName = "NOMBRES";
            colNombres.HeaderText = "NOMBRES";
            colNombres.Name = "colNombres";
            colNombres.ReadOnly = true;
            colNombres.Width = 130;

            // 
            // colEmail
            // 
            colEmail.DataPropertyName = "EMAIL";
            colEmail.HeaderText = "EMAIL";
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            colEmail.Width = 140;

            // 
            // colCelular
            // 
            colCelular.DataPropertyName = "CELULAR";
            colCelular.HeaderText = "CELULAR";
            colCelular.Name = "colCelular";
            colCelular.ReadOnly = true;
            colCelular.Width = 110;

            // 
            // colDireccion
            // 
            colDireccion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colDireccion.DataPropertyName = "DIRECCION";
            colDireccion.HeaderText = "DIRECCION";
            colDireccion.Name = "colDireccion";
            colDireccion.ReadOnly = true;

            // 
            // btnAyudaContextual
            // 
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(800, 18);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(130, 26);
            btnAyudaContextual.TabIndex = 10;
            btnAyudaContextual.Text = "? Ayuda";
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // 
            // btnActualizar
            // 
            btnActualizar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnActualizar.Location = new System.Drawing.Point(800, 58);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new System.Drawing.Size(130, 28);
            btnActualizar.TabIndex = 3;
            btnActualizar.Text = "Actualizar";
            toolTipGeneral.SetToolTip(btnActualizar, "Actualizar: Recarga la matriz de datos de los clientes desde la base de datos.");
            btnActualizar.Click += btnActualizar_Click;

            // 
            // btnAnadir
            // 
            btnAnadir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnAnadir.Location = new System.Drawing.Point(800, 92);
            btnAnadir.Name = "btnAnadir";
            btnAnadir.Size = new System.Drawing.Size(130, 28);
            btnAnadir.TabIndex = 4;
            btnAnadir.Text = "Añadir";
            btnAnadir.Click += btnAnadir_Click;

            // 
            // btnModificar
            // 
            btnModificar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnModificar.Location = new System.Drawing.Point(800, 126);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new System.Drawing.Size(130, 28);
            btnModificar.TabIndex = 5;
            btnModificar.Text = "Modificar";
            btnModificar.Click += btnModificar_Click;

            // 
            // btnEliminar
            // 
            btnEliminar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnEliminar.EsBotonSecundario = true;
            btnEliminar.Location = new System.Drawing.Point(800, 160);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new System.Drawing.Size(130, 28);
            btnEliminar.TabIndex = 6;
            btnEliminar.Text = "Eliminar";
            btnEliminar.Click += btnEliminar_Click;

            // 
            // btnAplicar
            // 
            btnAplicar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnAplicar.Location = new System.Drawing.Point(800, 255);
            btnAplicar.Name = "btnAplicar";
            btnAplicar.Size = new System.Drawing.Size(130, 28);
            btnAplicar.TabIndex = 7;
            btnAplicar.Text = "Aplicar";
            btnAplicar.Click += btnAplicar_Click;

            // 
            // btnCancelar
            // 
            btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnCancelar.Location = new System.Drawing.Point(800, 289);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new System.Drawing.Size(130, 28);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;

            // 
            // btnSalir
            // 
            btnSalir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnSalir.Location = new System.Drawing.Point(800, 323);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(130, 28);
            btnSalir.TabIndex = 9;
            btnSalir.Text = "Salir";
            btnSalir.Click += btnSalir_Click;

            // 
            // panelCampos (Lower input boxes with bright green label badges matching Excel)
            // 
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
            panelCampos.Location = new System.Drawing.Point(20, 245);
            panelCampos.Name = "panelCampos";
            panelCampos.Size = new System.Drawing.Size(420, 162);
            panelCampos.TabIndex = 11;

            // Row 1: DNI
            lblDni.BackColor = System.Drawing.Color.FromArgb(0, 230, 0); // Bright green matching Excel
            lblDni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblDni.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDni.Location = new System.Drawing.Point(0, 0);
            lblDni.Name = "lblDni";
            lblDni.Size = new System.Drawing.Size(90, 26);
            lblDni.TabIndex = 0;
            lblDni.Text = " DNI:";
            lblDni.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtDni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDni.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtDni.Location = new System.Drawing.Point(90, 0);
            txtDni.Name = "txtDni";
            txtDni.Size = new System.Drawing.Size(328, 23);
            txtDni.TabIndex = 1;

            // Row 2: Apellidos
            lblApellidos.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblApellidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblApellidos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblApellidos.Location = new System.Drawing.Point(0, 27);
            lblApellidos.Name = "lblApellidos";
            lblApellidos.Size = new System.Drawing.Size(90, 26);
            lblApellidos.TabIndex = 2;
            lblApellidos.Text = " Apellidos:";
            lblApellidos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtApellidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtApellidos.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtApellidos.Location = new System.Drawing.Point(90, 27);
            txtApellidos.Name = "txtApellidos";
            txtApellidos.Size = new System.Drawing.Size(328, 23);
            txtApellidos.TabIndex = 3;

            // Row 3: Nombres
            lblNombres.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblNombres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblNombres.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblNombres.Location = new System.Drawing.Point(0, 54);
            lblNombres.Name = "lblNombres";
            lblNombres.Size = new System.Drawing.Size(90, 26);
            lblNombres.TabIndex = 4;
            lblNombres.Text = " Nombres:";
            lblNombres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtNombres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtNombres.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtNombres.Location = new System.Drawing.Point(90, 54);
            txtNombres.Name = "txtNombres";
            txtNombres.Size = new System.Drawing.Size(328, 23);
            txtNombres.TabIndex = 5;

            // Row 4: Email
            lblEmail.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEmail.Location = new System.Drawing.Point(0, 81);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new System.Drawing.Size(90, 26);
            lblEmail.TabIndex = 6;
            lblEmail.Text = " Email:";
            lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtEmail.Location = new System.Drawing.Point(90, 81);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new System.Drawing.Size(250, 23);
            txtEmail.TabIndex = 7;

            btnGestionEmail1N.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            btnGestionEmail1N.Location = new System.Drawing.Point(342, 80);
            btnGestionEmail1N.Name = "btnGestionEmail1N";
            btnGestionEmail1N.Size = new System.Drawing.Size(76, 25);
            btnGestionEmail1N.TabIndex = 8;
            btnGestionEmail1N.Text = "1:N";
            toolTipGeneral.SetToolTip(btnGestionEmail1N, "Gestionar múltiples emails para este cliente (Relación 1:N)");
            btnGestionEmail1N.UseVisualStyleBackColor = true;
            btnGestionEmail1N.Click += btnGestionContactos1N_Click;

            // Row 5: Celular
            lblCelular.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblCelular.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCelular.Location = new System.Drawing.Point(0, 108);
            lblCelular.Name = "lblCelular";
            lblCelular.Size = new System.Drawing.Size(90, 26);
            lblCelular.TabIndex = 9;
            lblCelular.Text = " Celular:";
            lblCelular.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCelular.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtCelular.Location = new System.Drawing.Point(90, 108);
            txtCelular.Name = "txtCelular";
            txtCelular.Size = new System.Drawing.Size(250, 23);
            txtCelular.TabIndex = 10;

            btnGestionCelular1N.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            btnGestionCelular1N.Location = new System.Drawing.Point(342, 107);
            btnGestionCelular1N.Name = "btnGestionCelular1N";
            btnGestionCelular1N.Size = new System.Drawing.Size(76, 25);
            btnGestionCelular1N.TabIndex = 11;
            btnGestionCelular1N.Text = "1:N";
            toolTipGeneral.SetToolTip(btnGestionCelular1N, "Gestionar múltiples teléfonos/celulares para este cliente (Relación 1:N)");
            btnGestionCelular1N.UseVisualStyleBackColor = true;
            btnGestionCelular1N.Click += btnGestionContactos1N_Click;

            // Row 6: Dirección
            lblDireccion.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblDireccion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDireccion.Location = new System.Drawing.Point(0, 135);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new System.Drawing.Size(90, 26);
            lblDireccion.TabIndex = 12;
            lblDireccion.Text = " Dirección:";
            lblDireccion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDireccion.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtDireccion.Location = new System.Drawing.Point(90, 135);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new System.Drawing.Size(328, 23);
            txtDireccion.TabIndex = 13;

            // 
            // panelMensaje (Cuadro Mensaje: al centro, matching Excel)
            // 
            panelMensaje.Controls.Add(lblMensaje);
            panelMensaje.Controls.Add(txtMensaje);
            panelMensaje.Location = new System.Drawing.Point(450, 245);
            panelMensaje.Name = "panelMensaje";
            panelMensaje.Size = new System.Drawing.Size(330, 162);
            panelMensaje.TabIndex = 12;

            // 
            // lblMensaje
            // 
            lblMensaje.AutoSize = true;
            lblMensaje.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblMensaje.ForeColor = System.Drawing.Color.Black;
            lblMensaje.Location = new System.Drawing.Point(0, 2);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new System.Drawing.Size(63, 17);
            lblMensaje.TabIndex = 0;
            lblMensaje.Text = "Mensaje:";

            // 
            // txtMensaje
            // 
            txtMensaje.BackColor = System.Drawing.Color.White;
            txtMensaje.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtMensaje.Font = new System.Drawing.Font("Consolas", 8.5F);
            txtMensaje.Location = new System.Drawing.Point(0, 22);
            txtMensaje.Multiline = true;
            txtMensaje.Name = "txtMensaje";
            txtMensaje.ReadOnly = true;
            txtMensaje.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtMensaje.Size = new System.Drawing.Size(330, 140);
            txtMensaje.TabIndex = 1;

            // 
            // panelSeparador
            // 
            panelSeparador.BackColor = System.Drawing.Color.FromArgb(190, 210, 195);
            panelSeparador.Location = new System.Drawing.Point(20, 420);
            panelSeparador.Name = "panelSeparador";
            panelSeparador.Size = new System.Drawing.Size(910, 1);
            panelSeparador.TabIndex = 13;

            // 
            // btnSerializar
            // 
            btnSerializar.BackColor = System.Drawing.Color.FromArgb(245, 130, 32); // Orange button matching Excel
            btnSerializar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSerializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSerializar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSerializar.ForeColor = System.Drawing.Color.White;
            btnSerializar.Location = new System.Drawing.Point(60, 435);
            btnSerializar.Name = "btnSerializar";
            btnSerializar.Size = new System.Drawing.Size(220, 34);
            btnSerializar.TabIndex = 14;
            btnSerializar.Text = "SERIALIZAR";
            btnSerializar.UseVisualStyleBackColor = false;
            toolTipGeneral.SetToolTip(btnSerializar, "Serializar: Exporta la matriz visual de clientes completa a un archivo XML en disco.");
            btnSerializar.Click += btnSerializar_Click;

            // 
            // txtRutaSerializar
            // 
            txtRutaSerializar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtRutaSerializar.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtRutaSerializar.Location = new System.Drawing.Point(30, 478);
            txtRutaSerializar.Name = "txtRutaSerializar";
            txtRutaSerializar.PlaceholderText = "Ruta archivo XML a exportar...";
            txtRutaSerializar.Size = new System.Drawing.Size(245, 23);
            txtRutaSerializar.TabIndex = 15;

            // 
            // btnExaminarSerializar
            // 
            btnExaminarSerializar.BackColor = System.Drawing.Color.WhiteSmoke;
            btnExaminarSerializar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnExaminarSerializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExaminarSerializar.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnExaminarSerializar.Location = new System.Drawing.Point(280, 477);
            btnExaminarSerializar.Name = "btnExaminarSerializar";
            btnExaminarSerializar.Size = new System.Drawing.Size(32, 26);
            btnExaminarSerializar.TabIndex = 16;
            btnExaminarSerializar.Text = "📁";
            btnExaminarSerializar.UseVisualStyleBackColor = true;
            toolTipGeneral.SetToolTip(btnExaminarSerializar, "Seleccionar ubicación y nombre para guardar el archivo XML.");
            btnExaminarSerializar.Click += btnExaminarSerializar_Click;

            // 
            // btnDesSerializar
            // 
            btnDesSerializar.BackColor = System.Drawing.Color.FromArgb(245, 130, 32); // Orange button matching Excel
            btnDesSerializar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnDesSerializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDesSerializar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnDesSerializar.ForeColor = System.Drawing.Color.White;
            btnDesSerializar.Location = new System.Drawing.Point(420, 435);
            btnDesSerializar.Name = "btnDesSerializar";
            btnDesSerializar.Size = new System.Drawing.Size(220, 34);
            btnDesSerializar.TabIndex = 17;
            btnDesSerializar.Text = "DES-SERIALIZAR";
            btnDesSerializar.UseVisualStyleBackColor = false;
            toolTipGeneral.SetToolTip(btnDesSerializar, "Des-serializar: Carga un archivo XML desde disco y traslada los datos a la matriz visual de clientes.");
            btnDesSerializar.Click += btnDesSerializar_Click;

            // 
            // txtRutaDesSerializar
            // 
            txtRutaDesSerializar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtRutaDesSerializar.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtRutaDesSerializar.Location = new System.Drawing.Point(390, 478);
            txtRutaDesSerializar.Name = "txtRutaDesSerializar";
            txtRutaDesSerializar.PlaceholderText = "Ruta archivo XML a importar...";
            txtRutaDesSerializar.Size = new System.Drawing.Size(245, 23);
            txtRutaDesSerializar.TabIndex = 18;

            // 
            // btnExaminarDesSerializar
            // 
            btnExaminarDesSerializar.BackColor = System.Drawing.Color.WhiteSmoke;
            btnExaminarDesSerializar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnExaminarDesSerializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExaminarDesSerializar.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnExaminarDesSerializar.Location = new System.Drawing.Point(640, 477);
            btnExaminarDesSerializar.Name = "btnExaminarDesSerializar";
            btnExaminarDesSerializar.Size = new System.Drawing.Size(32, 26);
            btnExaminarDesSerializar.TabIndex = 19;
            btnExaminarDesSerializar.Text = "📁";
            btnExaminarDesSerializar.UseVisualStyleBackColor = true;
            toolTipGeneral.SetToolTip(btnExaminarDesSerializar, "Seleccionar archivo XML existente para des-serializar.");
            btnExaminarDesSerializar.Click += btnExaminarDesSerializar_Click;

            // 
            // btnLimpiar
            // 
            btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnLimpiar.Location = new System.Drawing.Point(800, 455);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new System.Drawing.Size(130, 40);
            btnLimpiar.TabIndex = 20;
            btnLimpiar.Text = "LIMPIAR";
            toolTipGeneral.SetToolTip(btnLimpiar, "Limpiar: Restablece los campos de edición, rutas de serialización y mensajes del sistema.");
            btnLimpiar.Click += btnLimpiar_Click;

            // 
            // frmMaestroClientes_DNI101
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(960, 530);
            Controls.Add(btnLimpiar);
            Controls.Add(btnExaminarDesSerializar);
            Controls.Add(txtRutaDesSerializar);
            Controls.Add(btnDesSerializar);
            Controls.Add(btnExaminarSerializar);
            Controls.Add(txtRutaSerializar);
            Controls.Add(btnSerializar);
            Controls.Add(panelSeparador);
            Controls.Add(panelMensaje);
            Controls.Add(panelCampos);
            Controls.Add(btnSalir);
            Controls.Add(btnCancelar);
            Controls.Add(btnAplicar);
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(btnAnadir);
            Controls.Add(btnActualizar);
            Controls.Add(btnAyudaContextual);
            Controls.Add(dgvClientes);
            Controls.Add(lblTituloMaestro);
            Controls.Add(lblTituloSuperior);
            Name = "frmMaestroClientes_DNI101";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Maestro de Clientes - ABM con Serialización XML";
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            panelCampos.ResumeLayout(false);
            panelCampos.PerformLayout();
            panelMensaje.ResumeLayout(false);
            panelMensaje.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ToolTip toolTipGeneral;

        private System.Windows.Forms.Label lblTituloSuperior;
        private System.Windows.Forms.Label lblTituloMaestro;

        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApellidos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombres;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCelular;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDireccion;

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

        private System.Windows.Forms.Panel panelMensaje;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.TextBox txtMensaje;

        private UI.Controles.BotonFuturista btnActualizar;
        private UI.Controles.BotonFuturista btnAnadir;
        private UI.Controles.BotonFuturista btnModificar;
        private UI.Controles.BotonFuturista btnEliminar;
        private UI.Controles.BotonFuturista btnAplicar;
        private UI.Controles.BotonFuturista btnCancelar;
        private UI.Controles.BotonFuturista btnSalir;
        private UI.Controles.BotonFuturista btnAyudaContextual;

        private System.Windows.Forms.Panel panelSeparador;
        private System.Windows.Forms.Button btnSerializar;
        private System.Windows.Forms.TextBox txtRutaSerializar;
        private System.Windows.Forms.Button btnExaminarSerializar;

        private System.Windows.Forms.Button btnDesSerializar;
        private System.Windows.Forms.TextBox txtRutaDesSerializar;
        private System.Windows.Forms.Button btnExaminarDesSerializar;

        private UI.Controles.BotonFuturista btnLimpiar;
    }
}
