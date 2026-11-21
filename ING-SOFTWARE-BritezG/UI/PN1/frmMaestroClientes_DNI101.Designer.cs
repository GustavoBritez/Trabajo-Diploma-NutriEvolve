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

            lblTituloMaestro = new System.Windows.Forms.Label();

            dgvClientes = new System.Windows.Forms.DataGridView();
            colDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colApellidos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNombres = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colFechaNacimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSexo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCobertura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCelular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDireccion = new System.Windows.Forms.DataGridViewTextBoxColumn();

            panelCampos = new System.Windows.Forms.Panel();
            lblDni = new System.Windows.Forms.Label();
            txtDni = new System.Windows.Forms.TextBox();
            lblSexo = new System.Windows.Forms.Label();
            cmbSexo = new System.Windows.Forms.ComboBox();
            lblApellidos = new System.Windows.Forms.Label();
            txtApellidos = new System.Windows.Forms.TextBox();
            lblNombres = new System.Windows.Forms.Label();
            txtNombres = new System.Windows.Forms.TextBox();
            lblFechaNacimiento = new System.Windows.Forms.Label();
            dtpFechaNacimiento = new System.Windows.Forms.DateTimePicker();
            lblEdadActual = new System.Windows.Forms.Label();
            lblCobertura = new System.Windows.Forms.Label();
            txtCobertura = new System.Windows.Forms.TextBox();
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
            btnRevertirDni = new UI.Controles.BotonFuturista();

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
            // lblTituloMaestro
            // 
            lblTituloMaestro.AutoSize = true;
            lblTituloMaestro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTituloMaestro.ForeColor = System.Drawing.Color.Black;
            lblTituloMaestro.Location = new System.Drawing.Point(20, 14);
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
            dgvStyleHeader.BackColor = System.Drawing.Color.FromArgb(0, 220, 240);
            dgvStyleHeader.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            dgvStyleHeader.ForeColor = System.Drawing.Color.Black;
            dgvClientes.ColumnHeadersDefaultCellStyle = dgvStyleHeader;
            dgvClientes.ColumnHeadersHeight = 28;
            dgvClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                colDni,
                colApellidos,
                colNombres,
                colFechaNacimiento,
                colSexo,
                colCobertura,
                colEmail,
                colCelular,
                colDireccion
            });
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.Location = new System.Drawing.Point(20, 42);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvStyleRows.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            dgvClientes.RowsDefaultCellStyle = dgvStyleRows;
            dgvClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new System.Drawing.Size(760, 185);
            dgvClientes.TabIndex = 2;
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;

            // 
            // colDni
            // 
            colDni.DataPropertyName = "DNI";
            colDni.HeaderText = "DNI";
            colDni.Name = "colDni";
            colDni.ReadOnly = true;
            colDni.Width = 75;

            // 
            // colApellidos
            // 
            colApellidos.DataPropertyName = "APELLIDOS";
            colApellidos.HeaderText = "APELLIDOS";
            colApellidos.Name = "colApellidos";
            colApellidos.ReadOnly = true;
            colApellidos.Width = 95;

            // 
            // colNombres
            // 
            colNombres.DataPropertyName = "NOMBRES";
            colNombres.HeaderText = "NOMBRES";
            colNombres.Name = "colNombres";
            colNombres.ReadOnly = true;
            colNombres.Width = 95;

            // 
            // colFechaNacimiento
            // 
            colFechaNacimiento.DataPropertyName = "FECHA_NACIMIENTO";
            colFechaNacimiento.HeaderText = "FECHA NAC.";
            colFechaNacimiento.Name = "colFechaNacimiento";
            colFechaNacimiento.ReadOnly = true;
            colFechaNacimiento.Width = 85;
            colFechaNacimiento.DefaultCellStyle.Format = "dd/MM/yyyy";

            // 
            // colSexo
            // 
            colSexo.DataPropertyName = "SEXO";
            colSexo.HeaderText = "SEXO";
            colSexo.Name = "colSexo";
            colSexo.ReadOnly = true;
            colSexo.Width = 75;

            // 
            // colCobertura
            // 
            colCobertura.DataPropertyName = "COBERTURA";
            colCobertura.HeaderText = "COBERTURA";
            colCobertura.Name = "colCobertura";
            colCobertura.ReadOnly = true;
            colCobertura.Width = 90;

            // 
            // colEmail
            // 
            colEmail.DataPropertyName = "EMAIL";
            colEmail.HeaderText = "EMAIL";
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            colEmail.Width = 110;

            // 
            // colCelular
            // 
            colCelular.DataPropertyName = "CELULAR";
            colCelular.HeaderText = "CELULAR";
            colCelular.Name = "colCelular";
            colCelular.ReadOnly = true;
            colCelular.Width = 85;

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
            btnAyudaContextual.Location = new System.Drawing.Point(800, 12);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(130, 26);
            btnAyudaContextual.TabIndex = 10;
            btnAyudaContextual.Text = "? Ayuda";
            toolTipGeneral.SetToolTip(btnAyudaContextual, "Ayuda: Despliega el manual de usuario del Maestro de Clientes (Capítulo 2).");
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // 
            // btnActualizar
            // 
            btnActualizar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnActualizar.Location = new System.Drawing.Point(800, 46);
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
            btnAnadir.Location = new System.Drawing.Point(800, 80);
            btnAnadir.Name = "btnAnadir";
            btnAnadir.Size = new System.Drawing.Size(130, 28);
            btnAnadir.TabIndex = 4;
            btnAnadir.Text = "Añadir";
            toolTipGeneral.SetToolTip(btnAnadir, "Añadir: Habilita el formulario para ingresar y registrar un nuevo cliente en el sistema.");
            btnAnadir.Click += btnAnadir_Click;

            // 
            // btnModificar
            // 
            btnModificar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnModificar.Location = new System.Drawing.Point(800, 114);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new System.Drawing.Size(130, 28);
            btnModificar.TabIndex = 5;
            btnModificar.Text = "Modificar";
            toolTipGeneral.SetToolTip(btnModificar, "Modificar: Habilita la edición de los datos del cliente seleccionado en la grilla.");
            btnModificar.Click += btnModificar_Click;

            // 
            // btnEliminar
            // 
            btnEliminar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnEliminar.Location = new System.Drawing.Point(800, 148);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new System.Drawing.Size(130, 28);
            btnEliminar.TabIndex = 6;
            btnEliminar.Text = "Eliminar";
            toolTipGeneral.SetToolTip(btnEliminar, "Bloquear / Baja Lógica: Desactiva lógicamente al cliente seleccionado.");
            btnEliminar.Click += btnEliminar_Click;

            // 
            // btnRevertirDni
            // 
            btnRevertirDni.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            btnRevertirDni.Location = new System.Drawing.Point(800, 184);
            btnRevertirDni.Name = "btnRevertirDni";
            btnRevertirDni.Size = new System.Drawing.Size(130, 36);
            btnRevertirDni.TabIndex = 6;
            btnRevertirDni.Text = "Revertir/Desrevertir";
            toolTipGeneral.SetToolTip(btnRevertirDni, "Criptografía Reversible (AES-256): Permite alternar entre cifrar los números de DNI en AES-256 (Base64) y descifrarlos.");
            btnRevertirDni.Click += btnRevertirDni_Click;

            // 
            // btnAplicar
            // 
            btnAplicar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnAplicar.Location = new System.Drawing.Point(800, 240);
            btnAplicar.Name = "btnAplicar";
            btnAplicar.Size = new System.Drawing.Size(130, 28);
            btnAplicar.TabIndex = 7;
            btnAplicar.Text = "Aplicar";
            toolTipGeneral.SetToolTip(btnAplicar, "Aplicar: Valida y confirma la modificación del cliente seleccionado.");
            btnAplicar.Click += btnAplicar_Click;

            // 
            // btnCancelar
            // 
            btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnCancelar.Location = new System.Drawing.Point(800, 274);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new System.Drawing.Size(130, 28);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            toolTipGeneral.SetToolTip(btnCancelar, "Cancelar: Descarta la operación actual de alta o modificación.");
            btnCancelar.Click += btnCancelar_Click;

            // 
            // btnSalir
            // 
            btnSalir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnSalir.Location = new System.Drawing.Point(800, 308);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(130, 28);
            btnSalir.TabIndex = 9;
            btnSalir.Text = "Salir";
            toolTipGeneral.SetToolTip(btnSalir, "Salir: Cierra el módulo de Maestro de Clientes y regresa a la pantalla principal.");
            btnSalir.Click += btnSalir_Click;

            // 
            // panelCampos
            // 
            panelCampos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelCampos.Controls.Add(lblDni);
            panelCampos.Controls.Add(txtDni);
            panelCampos.Controls.Add(lblSexo);
            panelCampos.Controls.Add(cmbSexo);
            panelCampos.Controls.Add(lblApellidos);
            panelCampos.Controls.Add(txtApellidos);
            panelCampos.Controls.Add(lblNombres);
            panelCampos.Controls.Add(txtNombres);
            panelCampos.Controls.Add(lblFechaNacimiento);
            panelCampos.Controls.Add(dtpFechaNacimiento);
            panelCampos.Controls.Add(lblEdadActual);
            panelCampos.Controls.Add(lblCobertura);
            panelCampos.Controls.Add(txtCobertura);
            panelCampos.Controls.Add(lblEmail);
            panelCampos.Controls.Add(txtEmail);
            panelCampos.Controls.Add(btnGestionEmail1N);
            panelCampos.Controls.Add(lblCelular);
            panelCampos.Controls.Add(txtCelular);
            panelCampos.Controls.Add(btnGestionCelular1N);
            panelCampos.Controls.Add(lblDireccion);
            panelCampos.Controls.Add(txtDireccion);
            panelCampos.Location = new System.Drawing.Point(20, 232);
            panelCampos.Name = "panelCampos";
            panelCampos.Size = new System.Drawing.Size(455, 190);
            panelCampos.TabIndex = 11;

            // Row 1: DNI + Sexo
            lblDni.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
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
            txtDni.Size = new System.Drawing.Size(180, 23);
            txtDni.TabIndex = 1;

            lblSexo.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblSexo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblSexo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblSexo.Location = new System.Drawing.Point(272, 0);
            lblSexo.Name = "lblSexo";
            lblSexo.Size = new System.Drawing.Size(55, 26);
            lblSexo.TabIndex = 2;
            lblSexo.Text = " Sexo:";
            lblSexo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            cmbSexo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbSexo.Font = new System.Drawing.Font("Segoe UI", 9F);
            cmbSexo.Items.AddRange(new object[] { "Masculino", "Femenino" });
            cmbSexo.Location = new System.Drawing.Point(328, 1);
            cmbSexo.Name = "cmbSexo";
            cmbSexo.Size = new System.Drawing.Size(124, 23);
            cmbSexo.TabIndex = 3;

            // Row 2: Apellidos
            lblApellidos.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblApellidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblApellidos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblApellidos.Location = new System.Drawing.Point(0, 27);
            lblApellidos.Name = "lblApellidos";
            lblApellidos.Size = new System.Drawing.Size(90, 26);
            lblApellidos.TabIndex = 4;
            lblApellidos.Text = " Apellidos:";
            lblApellidos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtApellidos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtApellidos.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtApellidos.Location = new System.Drawing.Point(90, 27);
            txtApellidos.Name = "txtApellidos";
            txtApellidos.Size = new System.Drawing.Size(362, 23);
            txtApellidos.TabIndex = 5;

            // Row 3: Nombres
            lblNombres.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblNombres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblNombres.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblNombres.Location = new System.Drawing.Point(0, 54);
            lblNombres.Name = "lblNombres";
            lblNombres.Size = new System.Drawing.Size(90, 26);
            lblNombres.TabIndex = 6;
            lblNombres.Text = " Nombres:";
            lblNombres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtNombres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtNombres.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtNombres.Location = new System.Drawing.Point(90, 54);
            txtNombres.Name = "txtNombres";
            txtNombres.Size = new System.Drawing.Size(362, 23);
            txtNombres.TabIndex = 7;

            // Row 4: Fecha Nacimiento + Edad + Cobertura Médica
            lblFechaNacimiento.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblFechaNacimiento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblFechaNacimiento.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblFechaNacimiento.Location = new System.Drawing.Point(0, 81);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new System.Drawing.Size(90, 26);
            lblFechaNacimiento.TabIndex = 8;
            lblFechaNacimiento.Text = " Fecha Nac.:";
            lblFechaNacimiento.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            dtpFechaNacimiento.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            dtpFechaNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFechaNacimiento.Location = new System.Drawing.Point(90, 82);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.Size = new System.Drawing.Size(98, 23);
            dtpFechaNacimiento.TabIndex = 9;

            lblEdadActual.BackColor = System.Drawing.Color.FromArgb(235, 248, 235);
            lblEdadActual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblEdadActual.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            lblEdadActual.ForeColor = System.Drawing.Color.FromArgb(30, 80, 40);
            lblEdadActual.Location = new System.Drawing.Point(189, 81);
            lblEdadActual.Name = "lblEdadActual";
            lblEdadActual.Size = new System.Drawing.Size(65, 26);
            lblEdadActual.TabIndex = 10;
            lblEdadActual.Text = "0m";
            lblEdadActual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblCobertura.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblCobertura.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblCobertura.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblCobertura.Location = new System.Drawing.Point(255, 81);
            lblCobertura.Name = "lblCobertura";
            lblCobertura.Size = new System.Drawing.Size(73, 26);
            lblCobertura.TabIndex = 11;
            lblCobertura.Text = " Cobertura:";
            lblCobertura.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtCobertura.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCobertura.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtCobertura.Location = new System.Drawing.Point(328, 81);
            txtCobertura.Name = "txtCobertura";
            txtCobertura.Size = new System.Drawing.Size(124, 23);
            txtCobertura.TabIndex = 12;

            // Row 5: Email
            lblEmail.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEmail.Location = new System.Drawing.Point(0, 108);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new System.Drawing.Size(90, 26);
            lblEmail.TabIndex = 13;
            lblEmail.Text = " Email:";
            lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtEmail.Location = new System.Drawing.Point(90, 108);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new System.Drawing.Size(284, 23);
            txtEmail.TabIndex = 14;

            btnGestionEmail1N.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            btnGestionEmail1N.Location = new System.Drawing.Point(376, 107);
            btnGestionEmail1N.Name = "btnGestionEmail1N";
            btnGestionEmail1N.Size = new System.Drawing.Size(76, 25);
            btnGestionEmail1N.TabIndex = 15;
            btnGestionEmail1N.Text = "1:N";
            toolTipGeneral.SetToolTip(btnGestionEmail1N, "Gestionar múltiples emails para este cliente (Relación 1:N)");
            btnGestionEmail1N.UseVisualStyleBackColor = true;
            btnGestionEmail1N.Click += btnGestionContactos1N_Click;

            // Row 6: Celular
            lblCelular.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblCelular.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCelular.Location = new System.Drawing.Point(0, 135);
            lblCelular.Name = "lblCelular";
            lblCelular.Size = new System.Drawing.Size(90, 26);
            lblCelular.TabIndex = 16;
            lblCelular.Text = " Celular:";
            lblCelular.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCelular.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtCelular.Location = new System.Drawing.Point(90, 135);
            txtCelular.Name = "txtCelular";
            txtCelular.Size = new System.Drawing.Size(284, 23);
            txtCelular.TabIndex = 17;

            btnGestionCelular1N.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            btnGestionCelular1N.Location = new System.Drawing.Point(376, 134);
            btnGestionCelular1N.Name = "btnGestionCelular1N";
            btnGestionCelular1N.Size = new System.Drawing.Size(76, 25);
            btnGestionCelular1N.TabIndex = 18;
            btnGestionCelular1N.Text = "1:N";
            toolTipGeneral.SetToolTip(btnGestionCelular1N, "Gestionar múltiples teléfonos/celulares para este cliente (Relación 1:N)");
            btnGestionCelular1N.UseVisualStyleBackColor = true;
            btnGestionCelular1N.Click += btnGestionContactos1N_Click;

            // Row 7: Dirección
            lblDireccion.BackColor = System.Drawing.Color.FromArgb(0, 230, 0);
            lblDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblDireccion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDireccion.Location = new System.Drawing.Point(0, 162);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new System.Drawing.Size(90, 26);
            lblDireccion.TabIndex = 19;
            lblDireccion.Text = " Dirección:";
            lblDireccion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDireccion.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtDireccion.Location = new System.Drawing.Point(90, 162);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new System.Drawing.Size(362, 23);
            txtDireccion.TabIndex = 20;

            // 
            // panelMensaje
            // 
            panelMensaje.Controls.Add(lblMensaje);
            panelMensaje.Controls.Add(txtMensaje);
            panelMensaje.Location = new System.Drawing.Point(485, 232);
            panelMensaje.Name = "panelMensaje";
            panelMensaje.Size = new System.Drawing.Size(295, 190);
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
            txtMensaje.Size = new System.Drawing.Size(295, 168);
            txtMensaje.TabIndex = 1;

            // 
            // panelSeparador
            // 
            panelSeparador.BackColor = System.Drawing.Color.FromArgb(190, 210, 195);
            panelSeparador.Location = new System.Drawing.Point(20, 427);
            panelSeparador.Name = "panelSeparador";
            panelSeparador.Size = new System.Drawing.Size(910, 1);
            panelSeparador.TabIndex = 13;

            // 
            // btnSerializar
            // 
            btnSerializar.BackColor = System.Drawing.Color.FromArgb(245, 130, 32);
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
            btnDesSerializar.BackColor = System.Drawing.Color.FromArgb(245, 130, 32);
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
            Controls.Add(btnRevertirDni);
            Controls.Add(btnModificar);
            Controls.Add(btnAnadir);
            Controls.Add(btnActualizar);
            Controls.Add(btnAyudaContextual);
            Controls.Add(dgvClientes);
            Controls.Add(lblTituloMaestro);
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
        private System.Windows.Forms.Label lblTituloMaestro;

        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApellidos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombres;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFechaNacimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSexo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCobertura;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCelular;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDireccion;

        private System.Windows.Forms.Panel panelCampos;
        private System.Windows.Forms.Label lblDni;
        private System.Windows.Forms.TextBox txtDni;
        private System.Windows.Forms.Label lblSexo;
        private System.Windows.Forms.ComboBox cmbSexo;
        private System.Windows.Forms.Label lblApellidos;
        private System.Windows.Forms.TextBox txtApellidos;
        private System.Windows.Forms.Label lblNombres;
        private System.Windows.Forms.TextBox txtNombres;
        private System.Windows.Forms.Label lblFechaNacimiento;
        private System.Windows.Forms.DateTimePicker dtpFechaNacimiento;
        private System.Windows.Forms.Label lblEdadActual;
        private System.Windows.Forms.Label lblCobertura;
        private System.Windows.Forms.TextBox txtCobertura;
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
        private UI.Controles.BotonFuturista btnRevertirDni;

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
