namespace UI
{
    partial class FrmSeleccionarFamilia
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
            btnCancelarF = new Button();
            btnVincular = new Button();
            cmbOrigen = new ComboBox();
            cmbDestino = new ComboBox();
            lblTituloFamiliaFamilia = new Label();
            labelOrigen = new Label();
            labelDestino = new Label();
            labelFlecha = new Label();
            btnDesvincular = new Button();
            SuspendLayout();
            // 
            // btnCancelarF
            // 
            btnCancelarF.BackColor = Color.FromArgb(225, 225, 225);
            btnCancelarF.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelarF.Location = new Point(12, 127);
            btnCancelarF.Name = "btnCancelarF";
            btnCancelarF.Size = new Size(82, 32);
            btnCancelarF.TabIndex = 2;
            btnCancelarF.Text = "Cancelar";
            btnCancelarF.UseVisualStyleBackColor = false;
            btnCancelarF.Click += btnCancelar_Click;
            // 
            // btnVincular
            // 
            btnVincular.BackColor = Color.FromArgb(225, 225, 225);
            btnVincular.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnVincular.Location = new Point(292, 127);
            btnVincular.Name = "btnVincular";
            btnVincular.Size = new Size(97, 32);
            btnVincular.TabIndex = 3;
            btnVincular.Text = "Vincular";
            btnVincular.UseVisualStyleBackColor = false;
            btnVincular.Click += btnVincular_Click;
            // 
            // cmbOrigen
            // 
            cmbOrigen.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrigen.Location = new Point(128, 45);
            cmbOrigen.Name = "cmbOrigen";
            cmbOrigen.Size = new Size(158, 23);
            cmbOrigen.TabIndex = 8;
            // 
            // cmbDestino
            // 
            cmbDestino.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDestino.Location = new Point(124, 98);
            cmbDestino.Name = "cmbDestino";
            cmbDestino.Size = new Size(162, 23);
            cmbDestino.TabIndex = 9;
            // 
            // lblTituloFamiliaFamilia
            // 
            lblTituloFamiliaFamilia.BackColor = Color.FromArgb(143, 188, 153);
            lblTituloFamiliaFamilia.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTituloFamiliaFamilia.ForeColor = Color.FromArgb(46, 94, 67);
            lblTituloFamiliaFamilia.Location = new Point(-23, 2);
            lblTituloFamiliaFamilia.Name = "lblTituloFamiliaFamilia";
            lblTituloFamiliaFamilia.Size = new Size(448, 40);
            lblTituloFamiliaFamilia.TabIndex = 10;
            lblTituloFamiliaFamilia.Text = "Relacion Familia a Familia";
            lblTituloFamiliaFamilia.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelOrigen
            // 
            labelOrigen.BackColor = Color.FromArgb(218, 237, 223);
            labelOrigen.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelOrigen.ForeColor = Color.FromArgb(46, 94, 67);
            labelOrigen.Location = new Point(76, 46);
            labelOrigen.Name = "labelOrigen";
            labelOrigen.Size = new Size(55, 24);
            labelOrigen.TabIndex = 11;
            labelOrigen.Text = "Origen";
            labelOrigen.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelDestino
            // 
            labelDestino.BackColor = Color.FromArgb(218, 237, 223);
            labelDestino.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDestino.ForeColor = Color.FromArgb(46, 94, 67);
            labelDestino.Location = new Point(67, 100);
            labelDestino.Name = "labelDestino";
            labelDestino.Size = new Size(64, 24);
            labelDestino.TabIndex = 12;
            labelDestino.Text = "Destino";
            labelDestino.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelFlecha
            // 
            labelFlecha.BackColor = Color.FromArgb(218, 237, 223);
            labelFlecha.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFlecha.ForeColor = Color.FromArgb(46, 94, 67);
            labelFlecha.Location = new Point(178, 71);
            labelFlecha.Name = "labelFlecha";
            labelFlecha.Size = new Size(47, 48);
            labelFlecha.TabIndex = 13;
            labelFlecha.Text = "🡫";
            labelFlecha.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnDesvincular
            // 
            btnDesvincular.BackColor = Color.FromArgb(225, 225, 225);
            btnDesvincular.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDesvincular.Location = new Point(178, 127);
            btnDesvincular.Name = "btnDesvincular";
            btnDesvincular.Size = new Size(108, 32);
            btnDesvincular.TabIndex = 14;
            btnDesvincular.Text = "Desvincular";
            btnDesvincular.UseVisualStyleBackColor = false;
            btnDesvincular.Click += btnDesvincular_Click;
            // 
            // FrmSeleccionarFamilia
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(218, 237, 223);
            ClientSize = new Size(401, 166);
            Controls.Add(btnDesvincular);
            Controls.Add(lblTituloFamiliaFamilia);
            Controls.Add(cmbDestino);
            Controls.Add(cmbOrigen);
            Controls.Add(btnVincular);
            Controls.Add(btnCancelarF);
            Controls.Add(labelFlecha);
            Controls.Add(labelDestino);
            Controls.Add(labelOrigen);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmSeleccionarFamilia";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Insertar Familia a Familia";
            Load += FrmSeleccionarFamilia_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnCancelarF;
        private Button btnAceptar;
        private ComboBox cmbOrigen;
        private ComboBox cmbDestino;
        private Label lblTituloFamiliaFamilia;
        private Label labelOrigen;
        private Label labelDestino;
        private Label labelFlecha;
        private Button btnVincular;
        private Button btnDesvincular;
    }
}