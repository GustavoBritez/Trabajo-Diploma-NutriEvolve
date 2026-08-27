namespace UI
{
    partial class Respaldo
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

        private Panel pnlTitulo;

        private Label lblTituloR;
        private Label lblDescripcionR;

        private GroupBox gbBackup;
        private Label lblRutaBackup;
        private TextBox txtRutaBackup;
        private Button btnBuscarBackup;
        private Button btnRealizarBackup;

        private GroupBox gbRestore;
        private Label lblRutaRestore;
        private TextBox txtRutaRestore;
        private Button btnBuscarRestore;
        private Button btnRealizarRestore;

        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;

        private void InitializeComponent()
        {
            pnlTitulo = new Panel();
            lblTituloR = new Label();
            lblDescripcionR = new Label();
            gbBackup = new GroupBox();
            lblRutaBackup = new Label();
            txtRutaBackup = new TextBox();
            btnBuscarBackup = new Button();
            btnRealizarBackup = new Button();
            gbRestore = new GroupBox();
            lblRutaRestore = new Label();
            txtRutaRestore = new TextBox();
            btnBuscarRestore = new Button();
            btnRealizarRestore = new Button();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
            btnSalirR = new Button();
            pnlTitulo.SuspendLayout();
            gbBackup.SuspendLayout();
            gbRestore.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTitulo
            // 
            pnlTitulo.BorderStyle = BorderStyle.FixedSingle;
            pnlTitulo.Controls.Add(lblTituloR);
            pnlTitulo.Controls.Add(lblDescripcionR);
            pnlTitulo.Location = new Point(15, 15);
            pnlTitulo.Name = "pnlTitulo";
            pnlTitulo.Size = new Size(860, 80);
            pnlTitulo.TabIndex = 0;
            // 
            // lblTituloR
            // 
            lblTituloR.AutoSize = true;
            lblTituloR.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTituloR.ForeColor = Color.FromArgb(40, 120, 60);
            lblTituloR.Location = new Point(20, 10);
            lblTituloR.Name = "lblTituloR";
            lblTituloR.Size = new Size(308, 30);
            lblTituloR.TabIndex = 0;
            lblTituloR.Text = "Gestión de Backup y Restore";
            // 
            // lblDescripcionR
            // 
            lblDescripcionR.AutoSize = true;
            lblDescripcionR.Font = new Font("Segoe UI", 10F);
            lblDescripcionR.Location = new Point(22, 45);
            lblDescripcionR.Name = "lblDescripcionR";
            lblDescripcionR.Size = new Size(369, 19);
            lblDescripcionR.TabIndex = 1;
            lblDescripcionR.Text = "Administración de copias de seguridad de la base de datos.";
            // 
            // gbBackup
            // 
            gbBackup.Controls.Add(lblRutaBackup);
            gbBackup.Controls.Add(txtRutaBackup);
            gbBackup.Controls.Add(btnBuscarBackup);
            gbBackup.Controls.Add(btnRealizarBackup);
            gbBackup.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            gbBackup.Location = new Point(15, 110);
            gbBackup.Name = "gbBackup";
            gbBackup.Size = new Size(860, 180);
            gbBackup.TabIndex = 1;
            gbBackup.TabStop = false;
            gbBackup.Text = "BackUp";
            // 
            // lblRutaBackup
            // 
            lblRutaBackup.AutoSize = true;
            lblRutaBackup.Location = new Point(25, 40);
            lblRutaBackup.Name = "lblRutaBackup";
            lblRutaBackup.Size = new Size(42, 20);
            lblRutaBackup.TabIndex = 0;
            lblRutaBackup.Text = "Ruta";
            // 
            // txtRutaBackup
            // 
            txtRutaBackup.Location = new Point(25, 65);
            txtRutaBackup.Name = "txtRutaBackup";
            txtRutaBackup.Size = new Size(680, 27);
            txtRutaBackup.TabIndex = 1;
            // 
            // btnBuscarBackup
            // 
            btnBuscarBackup.Location = new Point(720, 63);
            btnBuscarBackup.Name = "btnBuscarBackup";
            btnBuscarBackup.Size = new Size(70, 34);
            btnBuscarBackup.TabIndex = 2;
            btnBuscarBackup.Text = "...";
            btnBuscarBackup.Click += btnBuscarBackup_Click;
            // 
            // btnRealizarBackup
            // 
            btnRealizarBackup.BackColor = Color.ForestGreen;
            btnRealizarBackup.FlatStyle = FlatStyle.Flat;
            btnRealizarBackup.ForeColor = Color.White;
            btnRealizarBackup.Location = new Point(25, 120);
            btnRealizarBackup.Name = "btnRealizarBackup";
            btnRealizarBackup.Size = new Size(180, 40);
            btnRealizarBackup.TabIndex = 3;
            btnRealizarBackup.Text = "Realizar Backup";
            btnRealizarBackup.UseVisualStyleBackColor = false;
            btnRealizarBackup.Click += btnRealizarBackup_Click;
            // 
            // gbRestore
            // 
            gbRestore.Controls.Add(lblRutaRestore);
            gbRestore.Controls.Add(txtRutaRestore);
            gbRestore.Controls.Add(btnBuscarRestore);
            gbRestore.Controls.Add(btnRealizarRestore);
            gbRestore.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            gbRestore.Location = new Point(15, 310);
            gbRestore.Name = "gbRestore";
            gbRestore.Size = new Size(860, 180);
            gbRestore.TabIndex = 2;
            gbRestore.TabStop = false;
            gbRestore.Text = "Restore";
            // 
            // lblRutaRestore
            // 
            lblRutaRestore.AutoSize = true;
            lblRutaRestore.Location = new Point(25, 40);
            lblRutaRestore.Name = "lblRutaRestore";
            lblRutaRestore.Size = new Size(63, 20);
            lblRutaRestore.TabIndex = 0;
            lblRutaRestore.Text = "Archivo";
            // 
            // txtRutaRestore
            // 
            txtRutaRestore.Location = new Point(25, 65);
            txtRutaRestore.Name = "txtRutaRestore";
            txtRutaRestore.Size = new Size(680, 27);
            txtRutaRestore.TabIndex = 1;
            // 
            // btnBuscarRestore
            // 
            btnBuscarRestore.Location = new Point(720, 63);
            btnBuscarRestore.Name = "btnBuscarRestore";
            btnBuscarRestore.Size = new Size(70, 34);
            btnBuscarRestore.TabIndex = 2;
            btnBuscarRestore.Text = "...";
            btnBuscarRestore.Click += btnBuscarRestore_Click;
            // 
            // btnRealizarRestore
            // 
            btnRealizarRestore.BackColor = Color.ForestGreen;
            btnRealizarRestore.FlatStyle = FlatStyle.Flat;
            btnRealizarRestore.ForeColor = Color.White;
            btnRealizarRestore.Location = new Point(25, 120);
            btnRealizarRestore.Name = "btnRealizarRestore";
            btnRealizarRestore.Size = new Size(180, 40);
            btnRealizarRestore.TabIndex = 3;
            btnRealizarRestore.Text = "Realizar Restore";
            btnRealizarRestore.UseVisualStyleBackColor = false;
            btnRealizarRestore.Click += btnRealizarRestore_Click;
            // 
            // btnSalirR
            // 
            btnSalirR.BackColor = Color.FromArgb(255, 120, 120);
            btnSalirR.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalirR.ForeColor = Color.Black;
            btnSalirR.Location = new Point(12, 646);

            btnSalirR.Name = "btnSalirR";
            btnSalirR.Size = new Size(115, 40);
            btnSalirR.TabIndex = 3;
            btnSalirR.Text = "Salir";
            btnSalirR.UseVisualStyleBackColor = false;
            btnSalirR.Click += btnSalirR_Click;
            // 
            // Respaldo
            // 
            BackColor = Color.FloralWhite;
            ClientSize = new Size(885, 706);

            Controls.Add(btnSalirR);
            Controls.Add(pnlTitulo);
            Controls.Add(gbBackup);
            Controls.Add(gbRestore);
            Name = "Respaldo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Backup y Restore";
            pnlTitulo.ResumeLayout(false);
            pnlTitulo.PerformLayout();
            gbBackup.ResumeLayout(false);
            gbBackup.PerformLayout();
            gbRestore.ResumeLayout(false);
            gbRestore.PerformLayout();
            ResumeLayout(false);
        }
        private Button btnSalirR;
    }

    #endregion
}
