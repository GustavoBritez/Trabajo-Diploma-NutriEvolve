using System.Drawing;
using System.Windows.Forms;
using UI.Controles;

namespace UI.PN2
{
    partial class frmSeguimientoNutricional_DNI101
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
            _pnlHeader = new Panel();
            _lblTit = new Label();
            _lblSub = new Label();
            _pnlFiltros = new PanelOvalado();
            _lblLupa = new Label();
            _txtBuscar = new TextBox();
            _btnRecargar = new BotonFuturista();
            _pnlGrilla = new PanelOvalado();
            _dgvPacientes = new DataGridView();

            _pnlHeader.SuspendLayout();
            _pnlFiltros.SuspendLayout();
            _pnlGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvPacientes).BeginInit();
            SuspendLayout();

            // 
            // _pnlHeader
            // 
            _pnlHeader.BackColor = Color.Transparent;
            _pnlHeader.Controls.Add(_lblTit);
            _pnlHeader.Controls.Add(_lblSub);
            _pnlHeader.Height = 55;
            _pnlHeader.Location = new Point(15, 10);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Size = new Size(920, 55);
            _pnlHeader.TabIndex = 0;

            // 
            // _lblTit
            // 
            _lblTit.AutoSize = true;
            _lblTit.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            _lblTit.ForeColor = Color.FromArgb(30, 75, 40);
            _lblTit.Location = new Point(0, 4);
            _lblTit.Name = "_lblTit";
            _lblTit.Size = new Size(580, 25);
            _lblTit.TabIndex = 0;
            _lblTit.Text = "🥗 MÓDULO DE SEGUIMIENTO NUTRICIONAL PEDIÁTRICO (PN2)";

            // 
            // _lblSub
            // 
            _lblSub.AutoSize = true;
            _lblSub.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            _lblSub.ForeColor = Color.FromArgb(70, 95, 75);
            _lblSub.Location = new Point(2, 32);
            _lblSub.Name = "_lblSub";
            _lblSub.Size = new Size(720, 17);
            _lblSub.TabIndex = 1;
            _lblSub.Text = "CUN06: Haga clic sobre un paciente en la grilla para acceder a Graficar Diagnóstico, Registrar Consulta o Prescribir Plan.";

            // 
            // _pnlFiltros
            // 
            _pnlFiltros.BackColor = Color.White;
            _pnlFiltros.ColorBorde = Color.FromArgb(190, 215, 195);
            _pnlFiltros.Controls.Add(_lblLupa);
            _pnlFiltros.Controls.Add(_txtBuscar);
            _pnlFiltros.Controls.Add(_btnRecargar);
            _pnlFiltros.DibujarBorde = true;
            _pnlFiltros.Height = 58;
            _pnlFiltros.Location = new Point(15, 70);
            _pnlFiltros.Name = "_pnlFiltros";
            _pnlFiltros.RadioBorde = 24;
            _pnlFiltros.Size = new Size(920, 58);
            _pnlFiltros.TabIndex = 1;

            // 
            // _lblLupa
            // 
            _lblLupa.AutoSize = true;
            _lblLupa.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            _lblLupa.ForeColor = Color.FromArgb(40, 70, 45);
            _lblLupa.Location = new Point(18, 18);
            _lblLupa.Name = "_lblLupa";
            _lblLupa.Size = new Size(295, 17);
            _lblLupa.TabIndex = 0;
            _lblLupa.Text = "🔍 Buscar Paciente (DNI, Nombre o Apellido):";

            // 
            // _txtBuscar
            // 
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.Font = new Font("Segoe UI", 10F);
            _txtBuscar.Location = new Point(325, 15);
            _txtBuscar.Name = "_txtBuscar";
            _txtBuscar.Size = new Size(320, 25);
            _txtBuscar.TabIndex = 1;
            _txtBuscar.TextChanged += _txtBuscar_TextChanged;

            // 
            // _btnRecargar
            // 
            _btnRecargar.ColorGlow = Color.DarkSeaGreen;
            _btnRecargar.ColorPrincipal = Color.FromArgb(76, 124, 89);
            _btnRecargar.ColorSecundario = Color.FromArgb(50, 85, 60);
            _btnRecargar.Cursor = Cursors.Hand;
            _btnRecargar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnRecargar.ForeColor = Color.White;
            _btnRecargar.Location = new Point(765, 11);
            _btnRecargar.Name = "_btnRecargar";
            _btnRecargar.RadioBorde = 24;
            _btnRecargar.Size = new Size(135, 35);
            _btnRecargar.TabIndex = 2;
            _btnRecargar.Text = "🔄 Actualizar";
            _btnRecargar.Click += _btnRecargar_Click;

            // 
            // _pnlGrilla
            // 
            _pnlGrilla.BackColor = Color.White;
            _pnlGrilla.ColorBorde = Color.FromArgb(190, 215, 195);
            _pnlGrilla.Controls.Add(_dgvPacientes);
            _pnlGrilla.DibujarBorde = true;
            _pnlGrilla.Location = new Point(15, 140);
            _pnlGrilla.Name = "_pnlGrilla";
            _pnlGrilla.Padding = new Padding(12);
            _pnlGrilla.RadioBorde = 24;
            _pnlGrilla.Size = new Size(920, 480);
            _pnlGrilla.TabIndex = 2;

            // 
            // _dgvPacientes
            // 
            _dgvPacientes.AllowUserToAddRows = false;
            _dgvPacientes.AllowUserToDeleteRows = false;
            _dgvPacientes.AllowUserToResizeRows = false;
            _dgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _dgvPacientes.BackgroundColor = Color.White;
            _dgvPacientes.BorderStyle = BorderStyle.None;
            _dgvPacientes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _dgvPacientes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            _dgvPacientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(76, 124, 89);
            _dgvPacientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            _dgvPacientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _dgvPacientes.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(76, 124, 89);
            _dgvPacientes.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            _dgvPacientes.ColumnHeadersHeight = 36;
            _dgvPacientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dgvPacientes.Cursor = Cursors.Hand;
            _dgvPacientes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            _dgvPacientes.DefaultCellStyle.BackColor = Color.White;
            _dgvPacientes.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            _dgvPacientes.DefaultCellStyle.ForeColor = Color.FromArgb(40, 50, 45);
            _dgvPacientes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 210);
            _dgvPacientes.DefaultCellStyle.SelectionForeColor = Color.Black;
            _dgvPacientes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 252, 249);
            _dgvPacientes.Dock = DockStyle.Fill;
            _dgvPacientes.EnableHeadersVisualStyles = false;
            _dgvPacientes.GridColor = Color.FromArgb(232, 240, 234);
            _dgvPacientes.Location = new Point(12, 12);
            _dgvPacientes.MultiSelect = false;
            _dgvPacientes.Name = "_dgvPacientes";
            _dgvPacientes.ReadOnly = true;
            _dgvPacientes.RowHeadersVisible = false;
            _dgvPacientes.RowTemplate.Height = 34;
            _dgvPacientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgvPacientes.Size = new Size(896, 456);
            _dgvPacientes.TabIndex = 0;
            _dgvPacientes.CellClick += DgvPacientes_CellClick;

            // 
            // frmSeguimientoNutricional_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(225, 240, 228);
            ClientSize = new Size(950, 640);
            Controls.Add(_pnlGrilla);
            Controls.Add(_pnlFiltros);
            Controls.Add(_pnlHeader);
            Dock = DockStyle.Fill;
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmSeguimientoNutricional_DNI101";
            Text = "Seguimiento Nutricional Pediátrico";

            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlFiltros.ResumeLayout(false);
            _pnlFiltros.PerformLayout();
            _pnlGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvPacientes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel _pnlHeader;
        private Label _lblTit;
        private Label _lblSub;
        private PanelOvalado _pnlFiltros;
        private Label _lblLupa;
        private TextBox _txtBuscar;
        private BotonFuturista _btnRecargar;
        private PanelOvalado _pnlGrilla;
        private DataGridView _dgvPacientes;
    }
}
