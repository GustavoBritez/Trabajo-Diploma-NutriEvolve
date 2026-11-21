namespace UI
{
    partial class Perfiles
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
            TS_Gestion = new ToolStrip();
            toolStripLabel1 = new ToolStripDropDownButton();
            MenuItem_Familia_A_Perfil = new ToolStripMenuItem();
            Agregar_Permiso_A_Perfil = new ToolStripMenuItem();
            MenuItem_Permiso_A_Familia = new ToolStripMenuItem();
            familiaToolStripMenuItem = new ToolStripMenuItem();
            perfilToolStripMenuItem = new ToolStripMenuItem();
            permisoToolStripMenuItem = new ToolStripMenuItem();
            toolStripLabel2 = new ToolStripDropDownButton();
            E_MenuItem_Familia_A_Perfil = new ToolStripMenuItem();
            E_MenuItem_Permiso_A_Perfil = new ToolStripMenuItem();
            E_MenuItem_Permiso_A_Familia = new ToolStripMenuItem();
            perfilToolStripMenuItem1 = new ToolStripMenuItem();
            familiaToolStripMenuItem1 = new ToolStripMenuItem();
            permisoToolStripMenuItem1 = new ToolStripMenuItem();
            RelacionFamilia = new ToolStripButton();

            dgvPermisos = new DataGridView();
            panel3 = new Panel();
            labelPermiso = new Label();

            dgvFamilias = new DataGridView();
            panel2 = new Panel();
            labelFamilia = new Label();

            dgvPerfiles = new DataGridView();
            panel1 = new Panel();
            labelPerfil = new Label();

            Vista_Familia = new TreeView();
            panel4 = new Panel();
            labelArbol = new Label();

            btnSalir = new UI.Controles.BotonFuturista();

            TS_Gestion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerfiles).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFamilias).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPermisos).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();

            // 
            // TS_Gestion (Barra superior unificada con la paleta de MenuPrincipal)
            // 
            TS_Gestion.AutoSize = false;
            TS_Gestion.BackColor = Color.FromArgb(46, 94, 67);
            TS_Gestion.ForeColor = Color.White;
            TS_Gestion.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            TS_Gestion.GripStyle = ToolStripGripStyle.Hidden;
            TS_Gestion.Padding = new Padding(12, 4, 12, 4);
            TS_Gestion.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripLabel2, RelacionFamilia });
            TS_Gestion.Location = new Point(0, 0);
            TS_Gestion.Name = "TS_Gestion";
            TS_Gestion.Size = new Size(1120, 42);
            TS_Gestion.TabIndex = 6;
            TS_Gestion.Text = "TS_Gestion";

            // 
            // toolStripLabel1 (Agregar)
            // 
            toolStripLabel1.BackColor = Color.FromArgb(76, 124, 89);
            toolStripLabel1.ForeColor = Color.White;
            toolStripLabel1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            toolStripLabel1.Margin = new Padding(4, 2, 8, 2);
            toolStripLabel1.Padding = new Padding(10, 4, 10, 4);
            toolStripLabel1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripLabel1.DropDownItems.AddRange(new ToolStripItem[] { MenuItem_Familia_A_Perfil, Agregar_Permiso_A_Perfil, MenuItem_Permiso_A_Familia, familiaToolStripMenuItem, perfilToolStripMenuItem, permisoToolStripMenuItem });
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(95, 30);
            toolStripLabel1.Text = "➕ Agregar";

            // 
            // MenuItem_Familia_A_Perfil
            // 
            MenuItem_Familia_A_Perfil.Name = "MenuItem_Familia_A_Perfil";
            MenuItem_Familia_A_Perfil.Size = new Size(190, 26);
            MenuItem_Familia_A_Perfil.Text = "Familia a Perfil";
            MenuItem_Familia_A_Perfil.Click += Agregar_Familia_A_Perfil;

            // 
            // Agregar_Permiso_A_Perfil
            // 
            Agregar_Permiso_A_Perfil.Name = "Agregar_Permiso_A_Perfil";
            Agregar_Permiso_A_Perfil.Size = new Size(190, 26);
            Agregar_Permiso_A_Perfil.Text = "Permiso a Perfil";
            Agregar_Permiso_A_Perfil.Click += Agregar_Permiso_A_Perfil_Click;

            // 
            // MenuItem_Permiso_A_Familia
            // 
            MenuItem_Permiso_A_Familia.Name = "MenuItem_Permiso_A_Familia";
            MenuItem_Permiso_A_Familia.Size = new Size(190, 26);
            MenuItem_Permiso_A_Familia.Text = "Permiso a Familia";
            MenuItem_Permiso_A_Familia.Click += Agregar_Permiso_A_Familia;

            // 
            // familiaToolStripMenuItem
            // 
            familiaToolStripMenuItem.Name = "familiaToolStripMenuItem";
            familiaToolStripMenuItem.Size = new Size(190, 26);
            familiaToolStripMenuItem.Text = "Familia";
            familiaToolStripMenuItem.Click += Agregar_Familia;

            // 
            // perfilToolStripMenuItem
            // 
            perfilToolStripMenuItem.Name = "perfilToolStripMenuItem";
            perfilToolStripMenuItem.Size = new Size(190, 26);
            perfilToolStripMenuItem.Text = "Perfil";
            perfilToolStripMenuItem.Click += Agregar_Perfil;

            // 
            // permisoToolStripMenuItem
            // 
            permisoToolStripMenuItem.Name = "permisoToolStripMenuItem";
            permisoToolStripMenuItem.Size = new Size(190, 26);
            permisoToolStripMenuItem.Text = "Permiso";
            permisoToolStripMenuItem.Click += permisoToolStripMenuItem_Click;

            // 
            // toolStripLabel2 (Eliminar)
            // 
            toolStripLabel2.BackColor = Color.FromArgb(155, 65, 65);
            toolStripLabel2.ForeColor = Color.White;
            toolStripLabel2.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            toolStripLabel2.Margin = new Padding(4, 2, 8, 2);
            toolStripLabel2.Padding = new Padding(10, 4, 10, 4);
            toolStripLabel2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripLabel2.DropDownItems.AddRange(new ToolStripItem[] { E_MenuItem_Familia_A_Perfil, E_MenuItem_Permiso_A_Perfil, E_MenuItem_Permiso_A_Familia, perfilToolStripMenuItem1, familiaToolStripMenuItem1, permisoToolStripMenuItem1 });
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(95, 30);
            toolStripLabel2.Text = "🗑 Eliminar";

            // 
            // E_MenuItem_Familia_A_Perfil
            // 
            E_MenuItem_Familia_A_Perfil.Name = "E_MenuItem_Familia_A_Perfil";
            E_MenuItem_Familia_A_Perfil.Size = new Size(190, 26);
            E_MenuItem_Familia_A_Perfil.Text = "Familia a Perfil";
            E_MenuItem_Familia_A_Perfil.Click += Eliminar_Familia_A_Perfil_Click;

            // 
            // E_MenuItem_Permiso_A_Perfil
            // 
            E_MenuItem_Permiso_A_Perfil.Name = "E_MenuItem_Permiso_A_Perfil";
            E_MenuItem_Permiso_A_Perfil.Size = new Size(190, 26);
            E_MenuItem_Permiso_A_Perfil.Text = "Permiso a Perfil";
            E_MenuItem_Permiso_A_Perfil.Click += E_MenuItem_Permiso_A_Perfil_Click;

            // 
            // E_MenuItem_Permiso_A_Familia
            // 
            E_MenuItem_Permiso_A_Familia.Name = "E_MenuItem_Permiso_A_Familia";
            E_MenuItem_Permiso_A_Familia.Size = new Size(190, 26);
            E_MenuItem_Permiso_A_Familia.Text = "Permiso a Familia";
            E_MenuItem_Permiso_A_Familia.Click += Eliminar_Permiso_A_Familia_Click;

            // 
            // perfilToolStripMenuItem1
            // 
            perfilToolStripMenuItem1.Name = "perfilToolStripMenuItem1";
            perfilToolStripMenuItem1.Size = new Size(190, 26);
            perfilToolStripMenuItem1.Text = "Perfil";
            perfilToolStripMenuItem1.Click += Eliminar_Perfil_Click;

            // 
            // familiaToolStripMenuItem1
            // 
            familiaToolStripMenuItem1.Name = "familiaToolStripMenuItem1";
            familiaToolStripMenuItem1.Size = new Size(190, 26);
            familiaToolStripMenuItem1.Text = "Familia";
            familiaToolStripMenuItem1.Click += Eliminar_Familia_Click;

            // 
            // permisoToolStripMenuItem1
            // 
            permisoToolStripMenuItem1.Name = "permisoToolStripMenuItem1";
            permisoToolStripMenuItem1.Size = new Size(190, 26);
            permisoToolStripMenuItem1.Text = "Permiso";
            permisoToolStripMenuItem1.Click += permisoToolStripMenuItem1_Click;

            // 
            // RelacionFamilia
            // 
            RelacionFamilia.BackColor = Color.FromArgb(76, 124, 89);
            RelacionFamilia.ForeColor = Color.White;
            RelacionFamilia.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            RelacionFamilia.Margin = new Padding(4, 2, 8, 2);
            RelacionFamilia.Padding = new Padding(12, 4, 12, 4);
            RelacionFamilia.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RelacionFamilia.Name = "RelacionFamilia";
            RelacionFamilia.Size = new Size(140, 30);
            RelacionFamilia.Text = "🔗 Relación Familia";
            RelacionFamilia.Click += RelacionFamilia_Click;

            // 
            // COLUMNA 1: PERMISOS / PATENTES
            // 
            panel3.BackColor = Color.FromArgb(76, 124, 89);
            panel3.Controls.Add(labelPermiso);
            panel3.Location = new Point(16, 56);
            panel3.Name = "panel3";
            panel3.Size = new Size(250, 34);
            panel3.TabIndex = 16;

            labelPermiso.Dock = DockStyle.Fill;
            labelPermiso.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            labelPermiso.ForeColor = Color.White;
            labelPermiso.Location = new Point(0, 0);
            labelPermiso.Name = "labelPermiso";
            labelPermiso.Size = new Size(250, 34);
            labelPermiso.TabIndex = 14;
            labelPermiso.Text = "Permisos (Patentes)";
            labelPermiso.TextAlign = ContentAlignment.MiddleCenter;

            dgvPermisos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPermisos.Location = new Point(16, 92);
            dgvPermisos.Name = "dgvPermisos";
            dgvPermisos.Size = new Size(250, 310);
            dgvPermisos.TabIndex = 13;

            // 
            // COLUMNA 2: FAMILIAS EXISTENTES
            // 
            panel2.BackColor = Color.FromArgb(76, 124, 89);
            panel2.Controls.Add(labelFamilia);
            panel2.Location = new Point(282, 56);
            panel2.Name = "panel2";
            panel2.Size = new Size(250, 34);
            panel2.TabIndex = 10;

            labelFamilia.Dock = DockStyle.Fill;
            labelFamilia.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            labelFamilia.ForeColor = Color.White;
            labelFamilia.Location = new Point(0, 0);
            labelFamilia.Name = "labelFamilia";
            labelFamilia.Size = new Size(250, 34);
            labelFamilia.TabIndex = 14;
            labelFamilia.Text = "Familias Existentes";
            labelFamilia.TextAlign = ContentAlignment.MiddleCenter;

            dgvFamilias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvFamilias.Location = new Point(282, 92);
            dgvFamilias.Name = "dgvFamilias";
            dgvFamilias.Size = new Size(250, 310);
            dgvFamilias.TabIndex = 9;
            dgvFamilias.SelectionChanged += dgvFamilias_SelectionChanged;

            // 
            // COLUMNA 3: PERFILES EXISTENTES
            // 
            panel1.BackColor = Color.FromArgb(76, 124, 89);
            panel1.Controls.Add(labelPerfil);
            panel1.Location = new Point(548, 56);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 34);
            panel1.TabIndex = 8;

            labelPerfil.Dock = DockStyle.Fill;
            labelPerfil.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            labelPerfil.ForeColor = Color.White;
            labelPerfil.Location = new Point(0, 0);
            labelPerfil.Name = "labelPerfil";
            labelPerfil.Size = new Size(250, 34);
            labelPerfil.TabIndex = 14;
            labelPerfil.Text = "Perfiles Existentes";
            labelPerfil.TextAlign = ContentAlignment.MiddleCenter;

            dgvPerfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPerfiles.Location = new Point(548, 92);
            dgvPerfiles.Name = "dgvPerfiles";
            dgvPerfiles.Size = new Size(250, 310);
            dgvPerfiles.TabIndex = 7;
            dgvPerfiles.SelectionChanged += dgvPerfiles_SelectionChanged;

            // 
            // COLUMNA 4: ÁRBOL COMPOSITE DE PERMISOS Y FAMILIAS
            // 
            panel4.BackColor = Color.FromArgb(46, 94, 67);
            panel4.Controls.Add(labelArbol);
            panel4.Location = new Point(814, 56);
            panel4.Name = "panel4";
            panel4.Size = new Size(290, 34);
            panel4.TabIndex = 17;

            labelArbol.Dock = DockStyle.Fill;
            labelArbol.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            labelArbol.ForeColor = Color.White;
            labelArbol.Location = new Point(0, 0);
            labelArbol.Name = "labelArbol";
            labelArbol.Size = new Size(290, 34);
            labelArbol.TabIndex = 0;
            labelArbol.Text = "Jerarquía Composite";
            labelArbol.TextAlign = ContentAlignment.MiddleCenter;

            Vista_Familia.BackColor = Color.White;
            Vista_Familia.BorderStyle = BorderStyle.FixedSingle;
            Vista_Familia.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            Vista_Familia.ForeColor = Color.FromArgb(30, 50, 35);
            Vista_Familia.Indent = 20;
            Vista_Familia.ItemHeight = 24;
            Vista_Familia.Location = new Point(814, 92);
            Vista_Familia.Name = "Vista_Familia";
            Vista_Familia.Size = new Size(290, 310);
            Vista_Familia.TabIndex = 12;

            // 
            // btnSalir (BotonFuturista alineado a la paleta del MenuPrincipal)
            // 
            btnSalir.ColorPrincipal = Color.FromArgb(180, 70, 70);
            btnSalir.ColorSecundario = Color.FromArgb(140, 45, 45);
            btnSalir.ColorGlow = Color.FromArgb(220, 120, 120);
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSalir.ForeColor = Color.White;
            btnSalir.Location = new Point(16, 415);
            btnSalir.Name = "btnSalir";
            btnSalir.RadioBorde = 20;
            btnSalir.Size = new Size(135, 38);
            btnSalir.TabIndex = 11;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;

            // 
            // Perfiles
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 245, 238);
            ClientSize = new Size(1120, 468);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(dgvPermisos);
            Controls.Add(Vista_Familia);
            Controls.Add(btnSalir);
            Controls.Add(dgvPerfiles);
            Controls.Add(panel2);
            Controls.Add(TS_Gestion);
            Controls.Add(panel1);
            Controls.Add(dgvFamilias);
            Font = new Font("Segoe UI", 9F);
            Name = "Perfiles";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Perfiles, Familias y Permisos (RBAC)";
            Load += Perfiles_Load;
            TS_Gestion.ResumeLayout(false);
            TS_Gestion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerfiles).EndInit();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvFamilias).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPermisos).EndInit();
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ToolStrip TS_Gestion;
        private ToolStripDropDownButton toolStripLabel1;
        private ToolStripMenuItem MenuItem_Familia_A_Perfil;
        private ToolStripMenuItem MenuItem_Permiso_A_Familia;
        private ToolStripDropDownButton toolStripLabel2;
        private ToolStripMenuItem E_MenuItem_Familia_A_Perfil;
        private ToolStripMenuItem E_MenuItem_Permiso_A_Familia;
        private DataGridView dgvPerfiles;
        private Panel panel2;
        private Label labelFamilia;
        private DataGridView dgvFamilias;
        private Panel panel1;
        private Label labelPerfil;
        private UI.Controles.BotonFuturista btnSalir;
        private TreeView Vista_Familia;
        private DataGridView dgvPermisos;
        private Panel panel3;
        private Label labelPermiso;
        private Panel panel4;
        private Label labelArbol;
        private ToolStripMenuItem perfilToolStripMenuItem;
        private ToolStripMenuItem familiaToolStripMenuItem;
        private ToolStripMenuItem perfilToolStripMenuItem1;
        private ToolStripMenuItem familiaToolStripMenuItem1;
        private ToolStripButton RelacionFamilia;
        private ToolStripMenuItem Agregar_Permiso_A_Perfil;
        private ToolStripMenuItem E_MenuItem_Permiso_A_Perfil;
        private ToolStripMenuItem permisoToolStripMenuItem;
        private ToolStripMenuItem permisoToolStripMenuItem1;
    }
}
