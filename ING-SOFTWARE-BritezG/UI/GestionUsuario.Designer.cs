using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class GestionUsuario
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
        private void InitializeComponent()
        {
            panelLateral = new Panel();
            btnActDesact = new Button();
            btnEliminar = new Button();
            btnModificar = new Button();
            btnCrear = new Button();
            dgvUsuarios = new DataGridView();
            lblTitulo = new Label();
            gbFiltrar = new GroupBox();
            rbMostrarTodos = new RadioButton();
            rbMostrarInactivos = new RadioButton();
            rbMostrarActivos = new RadioButton();
            gbDetalles = new GroupBox();
            btnCancelarG = new Button();
            btnAceptarG = new Button();
            CKB_Desactivar = new CheckBox();
            CKB_Activar = new CheckBox();
            txtNombreUsuario = new TextBox();
            lblNombreUsuario = new Label();
            cmbRol = new ComboBox();
            lblRol = new Label();
            txtApellido = new TextBox();
            lblApellido = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            txtDni = new TextBox();
            lblDni = new Label();
            btnSalirG = new Button();
            buttonActualizar = new Button();
            cmbIdioma = new ComboBox();
            panelLateral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            gbFiltrar.SuspendLayout();
            gbDetalles.SuspendLayout();
            SuspendLayout();
            // 
            // panelLateral
            // 
            panelLateral.BackColor = Color.FromArgb(143, 188, 153);
            panelLateral.BorderStyle = BorderStyle.FixedSingle;
            panelLateral.Controls.Add(btnActDesact);
            panelLateral.Controls.Add(btnEliminar);
            panelLateral.Controls.Add(btnModificar);
            panelLateral.Controls.Add(btnCrear);
            panelLateral.Location = new Point(12, 52);
            panelLateral.Name = "panelLateral";
            panelLateral.Size = new Size(160, 297);
            panelLateral.TabIndex = 4;
            // 
            // btnActDesact
            // 
            btnActDesact.BackColor = Color.FromArgb(225, 225, 225);
            btnActDesact.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnActDesact.Location = new Point(15, 180);
            btnActDesact.Name = "btnActDesact";
            btnActDesact.Size = new Size(128, 40);
            btnActDesact.TabIndex = 0;
            btnActDesact.Text = "Act/Desact";
            btnActDesact.UseVisualStyleBackColor = false;
            btnActDesact.Click += btnActDesact_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(225, 225, 225);
            btnEliminar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEliminar.Location = new Point(15, 125);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(128, 40);
            btnEliminar.TabIndex = 1;
            btnEliminar.Text = "Desbloquear";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnDesbloquear_Click;
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.FromArgb(225, 225, 225);
            btnModificar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnModificar.Location = new Point(15, 70);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(128, 40);
            btnModificar.TabIndex = 2;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnCrear
            // 
            btnCrear.BackColor = Color.FromArgb(225, 225, 225);
            btnCrear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCrear.Location = new Point(15, 15);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(128, 40);
            btnCrear.TabIndex = 3;
            btnCrear.Text = "Crear";
            btnCrear.UseVisualStyleBackColor = false;
            btnCrear.Click += btnCrear_Click;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.Location = new Point(185, 52);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.Size = new Size(1047, 343);
            dgvUsuarios.TabIndex = 3;
            dgvUsuarios.CellFormatting += dgvUsuarios_CellFormatting;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(46, 94, 67);
            lblTitulo.Location = new Point(12, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(926, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "👤 Gestión de Usuarios";
            lblTitulo.TextAlign = ContentAlignment.TopCenter;
            // 
            // gbFiltrar
            // 
            gbFiltrar.Controls.Add(rbMostrarTodos);
            gbFiltrar.Controls.Add(rbMostrarInactivos);
            gbFiltrar.Controls.Add(rbMostrarActivos);
            gbFiltrar.FlatStyle = FlatStyle.Flat;
            gbFiltrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            gbFiltrar.Location = new Point(185, 410);
            gbFiltrar.Name = "gbFiltrar";
            gbFiltrar.Size = new Size(200, 114);
            gbFiltrar.TabIndex = 2;
            gbFiltrar.TabStop = false;
            gbFiltrar.Text = "Filtrar Usuarios";
            // 
            // rbMostrarTodos
            // 
            rbMostrarTodos.ForeColor = Color.Red;
            rbMostrarTodos.Location = new Point(15, 75);
            rbMostrarTodos.Name = "rbMostrarTodos";
            rbMostrarTodos.Size = new Size(150, 20);
            rbMostrarTodos.TabIndex = 5;
            rbMostrarTodos.Text = "Mostrar Todos";
            // 
            // rbMostrarInactivos
            // 
            rbMostrarInactivos.Location = new Point(15, 50);
            rbMostrarInactivos.Name = "rbMostrarInactivos";
            rbMostrarInactivos.Size = new Size(150, 20);
            rbMostrarInactivos.TabIndex = 0;
            rbMostrarInactivos.Text = "Mostrar Inactivos";
            // 
            // rbMostrarActivos
            // 
            rbMostrarActivos.Checked = true;
            rbMostrarActivos.Location = new Point(15, 25);
            rbMostrarActivos.Name = "rbMostrarActivos";
            rbMostrarActivos.Size = new Size(150, 20);
            rbMostrarActivos.TabIndex = 1;
            rbMostrarActivos.TabStop = true;
            rbMostrarActivos.Text = "Mostrar Activos";
            // 
            // gbDetalles
            // 
            gbDetalles.BackColor = Color.FromArgb(180, 180, 180);
            gbDetalles.Controls.Add(btnCancelarG);
            gbDetalles.Controls.Add(btnAceptarG);
            gbDetalles.Controls.Add(CKB_Desactivar);
            gbDetalles.Controls.Add(CKB_Activar);
            gbDetalles.Controls.Add(txtNombreUsuario);
            gbDetalles.Controls.Add(lblNombreUsuario);
            gbDetalles.Controls.Add(cmbRol);
            gbDetalles.Controls.Add(lblRol);
            gbDetalles.Controls.Add(txtApellido);
            gbDetalles.Controls.Add(lblApellido);
            gbDetalles.Controls.Add(txtNombre);
            gbDetalles.Controls.Add(lblNombre);
            gbDetalles.Controls.Add(txtDni);
            gbDetalles.Controls.Add(lblDni);
            gbDetalles.FlatStyle = FlatStyle.Flat;
            gbDetalles.Location = new Point(391, 410);
            gbDetalles.Name = "gbDetalles";
            gbDetalles.Size = new Size(841, 135);
            gbDetalles.TabIndex = 1;
            gbDetalles.TabStop = false;
            // 
            // btnCancelarG
            // 
            btnCancelarG.BackColor = Color.FromArgb(200, 100, 100);
            btnCancelarG.Enabled = false;
            btnCancelarG.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancelarG.Location = new Point(333, 102);
            btnCancelarG.Name = "btnCancelarG";
            btnCancelarG.Size = new Size(80, 25);
            btnCancelarG.TabIndex = 14;
            btnCancelarG.Text = "Cancelar";
            btnCancelarG.UseVisualStyleBackColor = false;
            btnCancelarG.Visible = false;
            btnCancelarG.Click += btnCancelar_Click;
            // 
            // btnAceptarG
            // 
            btnAceptarG.BackColor = Color.FromArgb(100, 200, 100);
            btnAceptarG.Enabled = false;
            btnAceptarG.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAceptarG.Location = new Point(245, 102);
            btnAceptarG.Name = "btnAceptarG";
            btnAceptarG.Size = new Size(80, 25);
            btnAceptarG.TabIndex = 13;
            btnAceptarG.Text = "Aceptar";
            btnAceptarG.UseVisualStyleBackColor = false;
            btnAceptarG.Visible = false;
            btnAceptarG.Click += btnAceptar_Click;
            // 
            // CKB_Desactivar
            // 
            CKB_Desactivar.AutoSize = true;
            CKB_Desactivar.Location = new Point(245, 75);
            CKB_Desactivar.Name = "CKB_Desactivar";
            CKB_Desactivar.Size = new Size(80, 19);
            CKB_Desactivar.TabIndex = 12;
            CKB_Desactivar.Text = "Desactivar";
            CKB_Desactivar.UseVisualStyleBackColor = true;
            // 
            // CKB_Activar
            // 
            CKB_Activar.AutoSize = true;
            CKB_Activar.Location = new Point(333, 75);
            CKB_Activar.Name = "CKB_Activar";
            CKB_Activar.Size = new Size(63, 19);
            CKB_Activar.TabIndex = 11;
            CKB_Activar.Text = "Activar";
            CKB_Activar.UseVisualStyleBackColor = true;
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Location = new Point(370, 46);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(165, 23);
            txtNombreUsuario.TabIndex = 1;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombreUsuario.Location = new Point(220, 46);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(115, 20);
            lblNombreUsuario.TabIndex = 2;
            lblNombreUsuario.Text = "NombreUsuario";
            lblNombreUsuario.TextAlign = ContentAlignment.TopRight;
            // 
            // cmbRol
            // 
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Location = new Point(370, 15);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(115, 23);
            cmbRol.TabIndex = 3;
            // 
            // lblRol
            // 
            lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRol.Location = new Point(230, 15);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(105, 20);
            lblRol.TabIndex = 4;
            lblRol.Text = "Rol";
            lblRol.TextAlign = ContentAlignment.TopRight;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(75, 72);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(130, 23);
            txtApellido.TabIndex = 5;
            // 
            // lblApellido
            // 
            lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblApellido.Location = new Point(10, 75);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(60, 20);
            lblApellido.TabIndex = 6;
            lblApellido.Text = "Apellido";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(75, 42);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(130, 23);
            txtNombre.TabIndex = 7;
            // 
            // lblNombre
            // 
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombre.Location = new Point(10, 45);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(60, 20);
            lblNombre.TabIndex = 8;
            lblNombre.Text = "Nombre";
            // 
            // txtDni
            // 
            txtDni.Location = new Point(75, 12);
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(130, 23);
            txtDni.TabIndex = 9;
            // 
            // lblDni
            // 
            lblDni.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDni.Location = new Point(10, 15);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(60, 20);
            lblDni.TabIndex = 10;
            lblDni.Text = "DNI";
            // 
            // btnSalirG
            // 
            btnSalirG.BackColor = Color.FromArgb(255, 120, 120);
            btnSalirG.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalirG.ForeColor = Color.Black;
            btnSalirG.Location = new Point(12, 505);
            btnSalirG.Name = "btnSalirG";
            btnSalirG.Size = new Size(115, 40);
            btnSalirG.TabIndex = 0;
            btnSalirG.Text = "Salir";
            btnSalirG.UseVisualStyleBackColor = false;
            btnSalirG.Click += btnSalir_Click;
            // 
            // buttonActualizar
            // 
            buttonActualizar.BackColor = Color.FromArgb(225, 225, 225);
            buttonActualizar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonActualizar.Location = new Point(28, 355);
            buttonActualizar.Name = "buttonActualizar";
            buttonActualizar.Size = new Size(128, 40);
            buttonActualizar.TabIndex = 4;
            buttonActualizar.Text = "Actualizar";
            buttonActualizar.UseVisualStyleBackColor = false;
            buttonActualizar.Click += buttonActualizar_Click;
            // 
            // cmbIdioma
            // 
            cmbIdioma.BackColor = Color.DarkSeaGreen;
            cmbIdioma.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbIdioma.FormattingEnabled = true;
            cmbIdioma.Items.AddRange(new object[] { "Español", "English", "Portugues" });
            cmbIdioma.Location = new Point(1068, 9);
            cmbIdioma.Name = "cmbIdioma";
            cmbIdioma.Size = new Size(164, 29);
            cmbIdioma.TabIndex = 6;
            cmbIdioma.SelectedIndexChanged += cmbIdioma_SelectedIndexChanged;
            // 
            // GestionUsuario
            // 
            BackColor = Color.FromArgb(218, 237, 223);
            ClientSize = new Size(1244, 560);
            Controls.Add(cmbIdioma);
            Controls.Add(buttonActualizar);
            Controls.Add(btnSalirG);
            Controls.Add(gbDetalles);
            Controls.Add(gbFiltrar);
            Controls.Add(panelLateral);
            Controls.Add(lblTitulo);
            Controls.Add(dgvUsuarios);
            Font = new Font("Segoe UI", 9F);
            Name = "GestionUsuario";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Usuarios";
            Load += GestionUsuario_Load;
            panelLateral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            gbFiltrar.ResumeLayout(false);
            gbDetalles.ResumeLayout(false);
            gbDetalles.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelLateral;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnActDesact;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbFiltrar;
        private System.Windows.Forms.RadioButton rbMostrarActivos;
        private System.Windows.Forms.RadioButton rbMostrarInactivos;
        private System.Windows.Forms.GroupBox gbDetalles;
        private System.Windows.Forms.Label lblDni;
        private System.Windows.Forms.TextBox txtDni;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Label lblNombreUsuario;
        private System.Windows.Forms.TextBox txtNombreUsuario;
        private System.Windows.Forms.Button btnSalirG;
        private System.Windows.Forms.CheckBox CKB_Desactivar;
        private System.Windows.Forms.CheckBox CKB_Activar;
        private System.Windows.Forms.Button btnAceptarG;
        private System.Windows.Forms.Button btnCancelarG;
        private RadioButton rbMostrarTodos;
        private Button buttonActualizar;
        private ComboBox cmbIdioma;
    }
}