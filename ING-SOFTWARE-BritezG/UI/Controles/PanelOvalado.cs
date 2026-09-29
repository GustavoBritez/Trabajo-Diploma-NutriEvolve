using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UI.Controles
{
    public class PanelOvalado : Panel
    {
        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RadioBorde { get; set; } = 30;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorBorde { get; set; } = Color.FromArgb(200, 220, 205);

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DibujarBorde { get; set; } = false;

        public PanelOvalado()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(226, 234, 226);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (GraphicsPath path = ObtenerRutaRedondeada(rect, RadioBorde))
            {
                this.Region = new Region(path);

                if (DibujarBorde)
                {
                    using (Pen pen = new Pen(ColorBorde, 1.5f))
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

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }
    }
}
