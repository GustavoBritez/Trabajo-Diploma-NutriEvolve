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
            RelacionFamilia = new ToolStripLabel();
            dgvPerfiles = new DataGridView();
            panel1 = new Panel();
            labelPerfil = new Label();
            dgvFamilias = new DataGridView();
            panel2 = new Panel();
            labelFamilia = new Label();
            btnSalir = new Button();
            Vista_Familia = new TreeView();
            dgvPermisos = new DataGridView();
            panel3 = new Panel();
            labelPermiso = new Label();
            permisoToolStripMenuItem1 = new ToolStripMenuItem();
            TS_Gestion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerfiles).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFamilias).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPermisos).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // TS_Gestion
            // 
            TS_Gestion.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripLabel2, RelacionFamilia });
            TS_Gestion.Location = new Point(0, 0);
            TS_Gestion.Name = "TS_Gestion";
            TS_Gestion.Size = new Size(1074, 25);
            TS_Gestion.TabIndex = 6;
            TS_Gestion.Text = "TS_Gestion";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripLabel1.DropDownItems.AddRange(new ToolStripItem[] { MenuItem_Familia_A_Perfil, Agregar_Permiso_A_Perfil, MenuItem_Permiso_A_Familia, familiaToolStripMenuItem, perfilToolStripMenuItem, permisoToolStripMenuItem });
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(62, 22);
            toolStripLabel1.Text = "Agregar";
            // 
            // MenuItem_Familia_A_Perfil
            // 
            MenuItem_Familia_A_Perfil.Name = "MenuItem_Familia_A_Perfil";
            MenuItem_Familia_A_Perfil.Size = new Size(180, 22);
            MenuItem_Familia_A_Perfil.Text = "Familia a Perfil";
            MenuItem_Familia_A_Perfil.Click += Agregar_Familia_A_Perfil;
            // 
            // Agregar_Permiso_A_Perfil
            // 
            Agregar_Permiso_A_Perfil.Name = "Agregar_Permiso_A_Perfil";
            Agregar_Permiso_A_Perfil.Size = new Size(180, 22);
            Agregar_Permiso_A_Perfil.Text = "Permiso a Perfil";
            Agregar_Permiso_A_Perfil.Click += Agregar_Permiso_A_Perfil_Click;
            // 
            // MenuItem_Permiso_A_Familia
            // 
            MenuItem_Permiso_A_Familia.Name = "MenuItem_Permiso_A_Familia";
            MenuItem_Permiso_A_Familia.Size = new Size(180, 22);
            MenuItem_Permiso_A_Familia.Text = "Permiso a Familia";
            MenuItem_Permiso_A_Familia.Click += Agregar_Permiso_A_Familia;
            // 
            // familiaToolStripMenuItem
            // 
            familiaToolStripMenuItem.Name = "familiaToolStripMenuItem";
            familiaToolStripMenuItem.Size = new Size(180, 22);
            familiaToolStripMenuItem.Text = "Familia";
            familiaToolStripMenuItem.Click += Agregar_Familia;
            // 
            // perfilToolStripMenuItem
            // 
            perfilToolStripMenuItem.Name = "perfilToolStripMenuItem";
            perfilToolStripMenuItem.Size = new Size(180, 22);
            perfilToolStripMenuItem.Text = "Perfil";
            perfilToolStripMenuItem.Click += Agregar_Perfil;
            // 
            // permisoToolStripMenuItem
            // 
            permisoToolStripMenuItem.Name = "permisoToolStripMenuItem";
            permisoToolStripMenuItem.Size = new Size(180, 22);
            permisoToolStripMenuItem.Text = "Permiso";
            permisoToolStripMenuItem.Click += permisoToolStripMenuItem_Click;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.DropDownItems.AddRange(new ToolStripItem[] { E_MenuItem_Familia_A_Perfil, E_MenuItem_Permiso_A_Perfil, E_MenuItem_Permiso_A_Familia, perfilToolStripMenuItem1, familiaToolStripMenuItem1, permisoToolStripMenuItem1 });
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(63, 22);
            toolStripLabel2.Text = "Eliminar";
            // 
            // E_MenuItem_Familia_A_Perfil
            // 
            E_MenuItem_Familia_A_Perfil.Name = "E_MenuItem_Familia_A_Perfil";
            E_MenuItem_Familia_A_Perfil.Size = new Size(180, 22);
            E_MenuItem_Familia_A_Perfil.Text = "Familia a Perfil";
            E_MenuItem_Familia_A_Perfil.Click += Eliminar_Familia_A_Perfil_Click;
            // 
            // E_MenuItem_Permiso_A_Perfil
            // 
            E_MenuItem_Permiso_A_Perfil.Name = "E_MenuItem_Permiso_A_Perfil";
            E_MenuItem_Permiso_A_Perfil.Size = new Size(180, 22);
            E_MenuItem_Permiso_A_Perfil.Text = "Permiso a Perfil";
            E_MenuItem_Permiso_A_Perfil.Click += E_MenuItem_Permiso_A_Perfil_Click;
            // 
            // E_MenuItem_Permiso_A_Familia
            // 
            E_MenuItem_Permiso_A_Familia.Name = "E_MenuItem_Permiso_A_Familia";
            E_MenuItem_Permiso_A_Familia.Size = new Size(180, 22);
            E_MenuItem_Permiso_A_Familia.Text = "Permiso a Familia";
            E_MenuItem_Permiso_A_Familia.Click += Eliminar_Permiso_A_Familia_Click;
            // 
            // perfilToolStripMenuItem1
            // 
            perfilToolStripMenuItem1.Name = "perfilToolStripMenuItem1";
            perfilToolStripMenuItem1.Size = new Size(180, 22);
            perfilToolStripMenuItem1.Text = "Perfil";
            perfilToolStripMenuItem1.Click += Eliminar_Perfil_Click;
            // 
            // familiaToolStripMenuItem1
            // 
            familiaToolStripMenuItem1.Name = "familiaToolStripMenuItem1";
            familiaToolStripMenuItem1.Size = new Size(180, 22);
            familiaToolStripMenuItem1.Text = "Familia";
            familiaToolStripMenuItem1.Click += Eliminar_Familia_Click;
            // 
            // RelacionFamilia
            // 
            RelacionFamilia.BackColor = Color.Lime;
            RelacionFamilia.Name = "RelacionFamilia";
            RelacionFamilia.Size = new Size(93, 22);
            RelacionFamilia.Text = "Relacion Familia";
            RelacionFamilia.Click += RelacionFamilia_Click;
            RelacionFamilia.MouseLeave += RelacionFamilia_MouseLeave;
            RelacionFamilia.MouseHover += RelacionFamilia_MouseHover;
            // 
            // dgvPerfiles
            // 
            dgvPerfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPerfiles.Location = new Point(470, 72);
            dgvPerfiles.Name = "dgvPerfiles";
            dgvPerfiles.Size = new Size(293, 271);
            dgvPerfiles.TabIndex = 7;
            dgvPerfiles.SelectionChanged += dgvPerfiles_SelectionChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(143, 188, 153);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(labelPerfil);
            panel1.Location = new Point(470, 38);
            panel1.Name = "panel1";
            panel1.Size = new Size(588, 28);
            panel1.TabIndex = 8;
            // 
            // labelPerfil
            // 
            labelPerfil.AutoSize = true;
            labelPerfil.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            labelPerfil.ForeColor = Color.White;
            labelPerfil.Location = new Point(192, 0);
            labelPerfil.Name = "labelPerfil";
            labelPerfil.Size = new Size(184, 28);
            labelPerfil.TabIndex = 14;
            labelPerfil.Text = "Perfiles Existentes";
            // 
            // dgvFamilias
            // 
            dgvFamilias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFamilias.Location = new Point(255, 72);
            dgvFamilias.Name = "dgvFamilias";
            dgvFamilias.Size = new Size(195, 271);
            dgvFamilias.TabIndex = 9;
            dgvFamilias.SelectionChanged += dgvFamilias_SelectionChanged;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(143, 188, 153);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(labelFamilia);
            panel2.Location = new Point(255, 38);
            panel2.Name = "panel2";
            panel2.Size = new Size(195, 28);
            panel2.TabIndex = 10;
            // 
            // labelFamilia
            // 
            labelFamilia.AutoSize = true;
            labelFamilia.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            labelFamilia.ForeColor = Color.White;
            labelFamilia.Location = new Point(3, -1);
            labelFamilia.Name = "labelFamilia";
            labelFamilia.Size = new Size(190, 28);
            labelFamilia.TabIndex = 14;
            labelFamilia.Text = "Familias Existentes";
            // 
            // btnSalir
            // 
            btnSalir.BackColor = Color.FromArgb(255, 120, 120);
            btnSalir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalir.ForeColor = Color.Black;
            btnSalir.Location = new Point(12, 345);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(115, 40);
            btnSalir.TabIndex = 11;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // Vista_Familia
            // 
            Vista_Familia.Location = new Point(769, 70);
            Vista_Familia.Name = "Vista_Familia";
            Vista_Familia.Size = new Size(289, 277);
            Vista_Familia.TabIndex = 12;
            // 
            // dgvPermisos
            // 
            dgvPermisos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPermisos.Location = new Point(12, 72);
            dgvPermisos.Name = "dgvPermisos";
            dgvPermisos.Size = new Size(227, 271);
            dgvPermisos.TabIndex = 13;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(143, 188, 153);
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(labelPermiso);
            panel3.Location = new Point(12, 41);
            panel3.Name = "panel3";
            panel3.Size = new Size(227, 28);
            panel3.TabIndex = 16;
            // 
            // labelPermiso
            // 
            labelPermiso.AutoSize = true;
            labelPermiso.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            labelPermiso.ForeColor = Color.White;
            labelPermiso.Location = new Point(66, -2);
            labelPermiso.Name = "labelPermiso";
            labelPermiso.Size = new Size(87, 28);
            labelPermiso.TabIndex = 14;
            labelPermiso.Text = "Permiso";
            // 
            // permisoToolStripMenuItem1
            // 
            permisoToolStripMenuItem1.Name = "permisoToolStripMenuItem1";
            permisoToolStripMenuItem1.Size = new Size(180, 22);
            permisoToolStripMenuItem1.Text = "Permiso";
            permisoToolStripMenuItem1.Click += permisoToolStripMenuItem1_Click;
            // 
            // Perfiles
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(218, 237, 223);
            ClientSize = new Size(1074, 391);
            Controls.Add(panel3);
            Controls.Add(dgvPermisos);
            Controls.Add(Vista_Familia);
            Controls.Add(btnSalir);
            Controls.Add(dgvPerfiles);
            Controls.Add(panel2);
            Controls.Add(TS_Gestion);
            Controls.Add(panel1);
            Controls.Add(dgvFamilias);
            Name = "Perfiles";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Perfiles";
            Load += Perfiles_Load;
            TS_Gestion.ResumeLayout(false);
            TS_Gestion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerfiles).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFamilias).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPermisos).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Button btnSalir;
        private TreeView Vista_Familia;
        private DataGridView dgvPermisos;
        private Panel panel3;
        private Label labelPermiso;
        private ToolStripMenuItem perfilToolStripMenuItem;
        private ToolStripMenuItem familiaToolStripMenuItem;
        private ToolStripMenuItem perfilToolStripMenuItem1;
        private ToolStripMenuItem familiaToolStripMenuItem1;
        private ToolStripLabel RelacionFamilia;
        private ToolStripMenuItem Agregar_Permiso_A_Perfil;
        private ToolStripMenuItem E_MenuItem_Permiso_A_Perfil;
        private ToolStripMenuItem permisoToolStripMenuItem;
        private ToolStripMenuItem permisoToolStripMenuItem1;
    }
}