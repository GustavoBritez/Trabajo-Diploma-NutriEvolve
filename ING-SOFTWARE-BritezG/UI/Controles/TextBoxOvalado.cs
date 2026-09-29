using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UI.Controles
{
    public class TextBoxOvalado : UserControl
    {
        private TextBox txtInterno = new TextBox();
        private bool isFocused = false;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorBordeNormal { get; set; } = Color.FromArgb(175, 198, 178);

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorBordeFocus { get; set; } = Color.FromArgb(78, 122, 84);

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorGlowFocus { get; set; } = Color.DarkSeaGreen;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorFondo { get; set; } = Color.White;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RadioBorde { get; set; } = 34;

        [Category("Apariencia Futurista")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => txtInterno?.Text ?? string.Empty;
            set
            {
                if (txtInterno != null)
                {
                    txtInterno.Text = value;
                }
            }
        }

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public char PasswordChar
        {
            get => txtInterno.PasswordChar;
            set => txtInterno.PasswordChar = value;
        }

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UseSystemPasswordChar
        {
            get => txtInterno.UseSystemPasswordChar;
            set => txtInterno.UseSystemPasswordChar = value;
        }

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string PlaceholderText
        {
            get => txtInterno.PlaceholderText;
            set => txtInterno.PlaceholderText = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBox BoxInterno => txtInterno;

        public TextBoxOvalado()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                          ControlStyles.UserPaint | 
                          ControlStyles.OptimizedDoubleBuffer | 
                          ControlStyles.ResizeRedraw |
                          ControlStyles.SupportsTransparentBackColor, true);

            this.Size = new Size(260, 42);
            this.BackColor = Color.Transparent;
            this.Padding = new Padding(16, 11, 16, 10);
            this.DoubleBuffered = true;

            txtInterno.BorderStyle = BorderStyle.None;
            txtInterno.Font = new Font("Segoe UI", 10.5F);
            txtInterno.BackColor = ColorFondo;
            txtInterno.ForeColor = Color.FromArgb(30, 48, 33);
            txtInterno.Dock = DockStyle.Fill;

            txtInterno.GotFocus += (s, e) => { isFocused = true; Invalidate(); };
            txtInterno.LostFocus += (s, e) => { isFocused = false; Invalidate(); };
            txtInterno.TextChanged += (s, e) => OnTextChanged(e);

            this.Controls.Add(txtInterno);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ActualizarRegion();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ActualizarRegion();
        }

        private void ActualizarRegion()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                int radio = Math.Min(RadioBorde, this.Height);
                using (GraphicsPath path = ObtenerRutaRedondeada(rect, radio))
                {
                    this.Region = new Region(path);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            int radio = Math.Min(RadioBorde, rect.Height);

            using (GraphicsPath path = ObtenerRutaRedondeada(rect, radio))
            {
                this.Region = new Region(path);

                // Fondo interior
                using (SolidBrush brush = new SolidBrush(ColorFondo))
                {
                    g.FillPath(brush, path);
                }

                // Borde y Glow en Focus
                if (isFocused)
                {
                    using (Pen glowPen = new Pen(Color.FromArgb(140, ColorGlowFocus), 2.8f))
                    {
                        g.DrawPath(glowPen, path);
                    }
                    using (Pen pen = new Pen(ColorBordeFocus, 1.8f))
                    {
                        g.DrawPath(pen, path);
                    }
                }
                else
                {
                    using (Pen pen = new Pen(ColorBordeNormal, 1.2f))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private GraphicsPath ObtenerRutaRedondeada(Rectangle rect, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radio;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
