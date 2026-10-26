using System.Drawing;
using System.Windows.Forms;

namespace UI.PN2
{
  partial class CustomMessageBoxPN2
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
      pnlHeader = new Panel();
      lblTitulo = new Label();
      btnCerrarCruz = new Button();
      pnlPaciente = new Panel();
      lblPacNombre = new Label();
      lblPacDetalles = new Label();
      lblSubtitulo = new Label();
      btnGraficar = new Button();
      btnRegistrar = new Button();
      btnPrescribir = new Button();
      btnCancelar = new Button();

      pnlHeader.SuspendLayout();
      pnlPaciente.SuspendLayout();
      SuspendLayout();

      // 
      // pnlHeader
      // 
      pnlHeader.BackColor = Color.FromArgb(76, 124, 89);
      pnlHeader.Controls.Add(lblTitulo);
      pnlHeader.Controls.Add(btnCerrarCruz);
      pnlHeader.Dock = DockStyle.Top;
      pnlHeader.Height = 65;
      pnlHeader.Location = new Point(0, 0);
      pnlHeader.Name = "pnlHeader";
      pnlHeader.Size = new Size(540, 65);
      pnlHeader.TabIndex = 0;

      // 
      // lblTitulo
      // 
      lblTitulo.AutoSize = true;
      lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      lblTitulo.ForeColor = Color.White;
      lblTitulo.Location = new Point(20, 20);
      lblTitulo.Name = "lblTitulo";
      lblTitulo.Size = new Size(290, 21);
      lblTitulo.TabIndex = 0;
      lblTitulo.Text = " SELECCIONAR ACCIÓN CLÍNICA";

      // 
      // btnCerrarCruz
      // 
      btnCerrarCruz.BackColor = Color.Transparent;
      btnCerrarCruz.Cursor = Cursors.Hand;
      btnCerrarCruz.FlatAppearance.BorderSize = 0;
      btnCerrarCruz.FlatStyle = FlatStyle.Flat;
      btnCerrarCruz.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      btnCerrarCruz.ForeColor = Color.White;
      btnCerrarCruz.Location = new Point(495, 15);
      btnCerrarCruz.Name = "btnCerrarCruz";
      btnCerrarCruz.Size = new Size(35, 35);
      btnCerrarCruz.TabIndex = 1;
      btnCerrarCruz.Text = "";
      btnCerrarCruz.UseVisualStyleBackColor = false;
      btnCerrarCruz.Click += BtnCerrar_Click;

      // 
      // pnlPaciente
      // 
      pnlPaciente.BackColor = Color.White;
      pnlPaciente.Controls.Add(lblPacNombre);
      pnlPaciente.Controls.Add(lblPacDetalles);
      pnlPaciente.Location = new Point(20, 80);
      pnlPaciente.Name = "pnlPaciente";
      pnlPaciente.Size = new Size(500, 75);
      pnlPaciente.TabIndex = 1;
      pnlPaciente.Paint += PnlPaciente_Paint;

      // 
      // lblPacNombre
      // 
      lblPacNombre.AutoSize = true;
      lblPacNombre.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
      lblPacNombre.ForeColor = Color.FromArgb(30, 45, 35);
      lblPacNombre.Location = new Point(12, 10);
      lblPacNombre.Name = "lblPacNombre";
      lblPacNombre.Size = new Size(150, 20);
      lblPacNombre.TabIndex = 0;
      lblPacNombre.Text = " Paciente";

      // 
      // lblPacDetalles
      // 
      lblPacDetalles.AutoSize = true;
      lblPacDetalles.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
      lblPacDetalles.ForeColor = Color.FromArgb(90, 105, 95);
      lblPacDetalles.Location = new Point(14, 40);
      lblPacDetalles.Name = "lblPacDetalles";
      lblPacDetalles.Size = new Size(300, 15);
      lblPacDetalles.TabIndex = 1;
      lblPacDetalles.Text = "DNI: - | Sexo: - | Edad: - | Cobertura: -";

      // 
      // lblSubtitulo
      // 
      lblSubtitulo.AutoSize = true;
      lblSubtitulo.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
      lblSubtitulo.ForeColor = Color.FromArgb(70, 85, 75);
      lblSubtitulo.Location = new Point(22, 165);
      lblSubtitulo.Name = "lblSubtitulo";
      lblSubtitulo.Size = new Size(330, 17);
      lblSubtitulo.TabIndex = 2;
      lblSubtitulo.Text = "Seleccione la operación a realizar para este paciente:";

      // 
      // btnGraficar
      // 
      btnGraficar.Location = new Point(20, 195);
      btnGraficar.Name = "btnGraficar";
      btnGraficar.Size = new Size(500, 68);
      btnGraficar.TabIndex = 3;
      btnGraficar.Click += BtnGraficar_Click;

      // 
      // btnRegistrar
      // 
      btnRegistrar.Location = new Point(20, 275);
      btnRegistrar.Name = "btnRegistrar";
      btnRegistrar.Size = new Size(500, 68);
      btnRegistrar.TabIndex = 4;
      btnRegistrar.Click += BtnRegistrar_Click;

      // 
      // btnPrescribir
      // 
      btnPrescribir.Location = new Point(20, 355);
      btnPrescribir.Name = "btnPrescribir";
      btnPrescribir.Size = new Size(500, 68);
      btnPrescribir.TabIndex = 5;
      btnPrescribir.Click += BtnPrescribir_Click;

      // 
      // btnCancelar
      // 
      btnCancelar.BackColor = Color.FromArgb(235, 238, 235);
      btnCancelar.Cursor = Cursors.Hand;
      btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(200, 210, 200);
      btnCancelar.FlatStyle = FlatStyle.Flat;
      btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
      btnCancelar.ForeColor = Color.FromArgb(90, 100, 95);
      btnCancelar.Location = new Point(210, 435);
      btnCancelar.Name = "btnCancelar";
      btnCancelar.Size = new Size(120, 32);
      btnCancelar.TabIndex = 6;
      btnCancelar.Text = "Cancelar";
      btnCancelar.UseVisualStyleBackColor = false;
      btnCancelar.Click += BtnCerrar_Click;

      // 
      // CustomMessageBoxPN2
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.FromArgb(245, 248, 245);
      ClientSize = new Size(540, 480);
      Controls.Add(btnCancelar);
      Controls.Add(btnPrescribir);
      Controls.Add(btnRegistrar);
      Controls.Add(btnGraficar);
      Controls.Add(lblSubtitulo);
      Controls.Add(pnlPaciente);
      Controls.Add(pnlHeader);
      DoubleBuffered = true;
      FormBorderStyle = FormBorderStyle.None;
      Name = "CustomMessageBoxPN2";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Acciones Clínicas - Seguimiento Nutricional";

      pnlHeader.ResumeLayout(false);
      pnlHeader.PerformLayout();
      pnlPaciente.ResumeLayout(false);
      pnlPaciente.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblTitulo;
    private Button btnCerrarCruz;
    private Panel pnlPaciente;
    private Label lblPacNombre;
    private Label lblPacDetalles;
    private Label lblSubtitulo;
    private Button btnGraficar;
    private Button btnRegistrar;
    private Button btnPrescribir;
    private Button btnCancelar;
  }
}
