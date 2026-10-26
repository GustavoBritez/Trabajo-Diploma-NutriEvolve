using System;
using System.Drawing;
using System.Windows.Forms;
using UI.Controles;

namespace UI.PN1
{
    public class frmConfirmarAltaCliente_DNI101 : Form
    {
        private Panel pnlHeader = null!;
        private Label lblTitulo = null!;
        private Label lblSubtitulo = null!;

        private Panel pnlContenedor = null!;
        private TableLayoutPanel tblDatos = null!;

        private FlowLayoutPanel pnlBotones = null!;
        private BotonFuturista btnContinuar = null!;
        private BotonFuturista btnCancelar = null!;

        public frmConfirmarAltaCliente_DNI101(string dni, string apellidos, string nombres, string email, string celular, string direccion)
        {
            InicializarComponentes(dni, apellidos, nombres, email, celular, direccion);
        }

        private void InicializarComponentes(string dni, string apellidos, string nombres, string email, string celular, string direccion)
        {
            this.Text = "Añadir Cliente";
            this.Size = new Size(460, 390);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(245, 248, 245);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // --- Panel Header ---
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(50, 82, 55),
                Padding = new Padding(18, 10, 18, 10)
            };

            lblTitulo = new Label
            {
                Text = "Añadir Cliente",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 24
            };

            lblSubtitulo = new Label
            {
                Text = "¿Desea registrar al nuevo cliente con los siguientes datos?",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(215, 240, 220),
                Dock = DockStyle.Fill
            };

            pnlHeader.Controls.Add(lblSubtitulo);
            pnlHeader.Controls.Add(lblTitulo);

            // --- Panel Contenedor de Datos ---
            pnlContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(22, 16, 22, 10),
                BackColor = Color.White
            };

            tblDatos = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            tblDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tblDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));

            for (int i = 0; i < 6; i++)
            {
                tblDatos.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66F));
            }

            AgregarFilaDato(tblDatos, 0, "DNI:", dni);
            AgregarFilaDato(tblDatos, 1, "Apellidos:", apellidos);
            AgregarFilaDato(tblDatos, 2, "Nombres:", nombres);
            AgregarFilaDato(tblDatos, 3, "Email:", string.IsNullOrWhiteSpace(email) ? "(Sin email)" : email);
            AgregarFilaDato(tblDatos, 4, "Celular:", string.IsNullOrWhiteSpace(celular) ? "(Sin celular)" : celular);
            AgregarFilaDato(tblDatos, 5, "Dirección:", string.IsNullOrWhiteSpace(direccion) ? "(Sin dirección)" : direccion);

            pnlContenedor.Controls.Add(tblDatos);

            // --- Panel Botones Footer ---
            pnlBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 62,
                BackColor = Color.FromArgb(240, 244, 240),
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(16, 12, 16, 12)
            };

            btnCancelar = new BotonFuturista
            {
                Text = "Cancelar",
                DialogResult = DialogResult.Cancel,
                Size = new Size(115, 36),
                ColorPrincipal = Color.FromArgb(170, 75, 75),
                ColorSecundario = Color.FromArgb(135, 45, 45),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            btnContinuar = new BotonFuturista
            {
                Text = "Continuar",
                DialogResult = DialogResult.OK,
                Size = new Size(125, 36),
                ColorPrincipal = Color.FromArgb(78, 122, 84),
                ColorSecundario = Color.FromArgb(50, 82, 55),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            pnlBotones.Controls.Add(btnCancelar);
            pnlBotones.Controls.Add(btnContinuar);

            this.Controls.Add(pnlContenedor);
            this.Controls.Add(pnlBotones);
            this.Controls.Add(pnlHeader);

            this.AcceptButton = btnContinuar;
            this.CancelButton = btnCancelar;
        }

        private void AgregarFilaDato(TableLayoutPanel table, int row, string etiqueta, string valor)
        {
            var lblTag = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 70, 48),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblVal = new Label
            {
                Text = valor,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(25, 25, 25),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            table.Controls.Add(lblTag, 0, row);
            table.Controls.Add(lblVal, 1, row);
        }
    }
}
