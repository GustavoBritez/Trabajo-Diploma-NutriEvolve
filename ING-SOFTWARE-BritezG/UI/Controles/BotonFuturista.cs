using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UI.Controles
{
    public class BotonFuturista : Button
    {
        private bool isHovered = false;
        private bool isPressed = false;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorPrincipal { get; set; } = Color.FromArgb(78, 122, 84);

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorSecundario { get; set; } = Color.FromArgb(50, 82, 55);

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorGlow { get; set; } = Color.DarkSeaGreen;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RadioBorde { get; set; } = 40;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EsBotonSecundario { get; set; } = false;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EsMenuLateral { get; set; } = false;

        [Category("Apariencia Futurista")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int PaddingIzquierdo { get; set; } = 18;

        public BotonFuturista()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                          ControlStyles.UserPaint | 
                          ControlStyles.OptimizedDoubleBuffer | 
                          ControlStyles.ResizeRedraw |
                          ControlStyles.SupportsTransparentBackColor, true);

            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.CheckedBackColor = Color.Transparent;
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
            this.Size = new Size(150, 44);
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

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isPressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            int radio = Math.Min(RadioBorde, rect.Height);

            using (GraphicsPath path = ObtenerRutaRedondeada(rect, radio))
            {
                // Aseguramos el recorte de esquinas rectangulares
                this.Region = new Region(path);

                if (EsMenuLateral)
                {
                    // --- MODO MENÚ LATERAL FUTURISTA ---
                    if (isHovered)
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(98, 152, 106), Color.FromArgb(70, 115, 80), LinearGradientMode.Vertical))
                        {
                            g.FillPath(brush, path);
                        }
                        using (Pen glowPen = new Pen(Color.FromArgb(210, ColorGlow), 1.8f))
                        {
                            g.DrawPath(glowPen, path);
                        }
                    }
                    else if (isPressed)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(52, 90, 60)))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    else
                    {
                        // Reposo elegante en panel lateral
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(64, 108, 75)))
                        {
                            g.FillPath(brush, path);
                        }
                        using (Pen pen = new Pen(Color.FromArgb(60, 255, 255, 255), 1f))
                        {
                            g.DrawPath(pen, path);
                        }
                    }

                    DibujarTexto(g, rect);
                }
                else if (EsBotonSecundario)
                {
                    // --- MODO BOTÓN SECUNDARIO (OUTLINE ARMONIOSO) ---
                    Color fondoSec = isHovered ? Color.FromArgb(208, 226, 212) : (isPressed ? Color.FromArgb(192, 214, 196) : Color.FromArgb(242, 248, 243));
                    using (SolidBrush brush = new SolidBrush(fondoSec))
                    {
                        g.FillPath(brush, path);
                    }

                    Color bordeSec = isHovered ? ColorPrincipal : Color.FromArgb(135, 170, 140);
                    using (Pen pen = new Pen(bordeSec, isHovered ? 2f : 1.3f))
                    {
                        g.DrawPath(pen, path);
                    }

                    Color textoColor = Color.FromArgb(40, 68, 45);
                    DibujarTexto(g, rect, textoColor);
                }
                else
                {
                    // --- MODO BOTÓN PRINCIPAL FUTURISTA (DEGRADADO + GLOW) ---
                    Color col1 = isHovered ? Color.FromArgb(92, 142, 98) : ColorPrincipal;
                    Color col2 = isPressed ? Color.FromArgb(40, 68, 45) : ColorSecundario;

                    using (LinearGradientBrush brush = new LinearGradientBrush(rect, col1, col2, LinearGradientMode.Vertical))
                    {
                        g.FillPath(brush, path);
                    }

                    if (isHovered)
                    {
                        using (Pen glowPen = new Pen(Color.FromArgb(200, ColorGlow), 2f))
                        {
                            g.DrawPath(glowPen, path);
                        }
                    }
                    else
                    {
                        using (Pen normalPen = new Pen(Color.FromArgb(90, 255, 255, 255), 1.2f))
                        {
                            g.DrawPath(normalPen, path);
                        }
                    }

                    DibujarTexto(g, rect, this.ForeColor);
                }
            }
        }

        private void DibujarTexto(Graphics g, Rectangle rect, Color? colorForzado = null)
        {
            Color colorTexto = colorForzado ?? this.ForeColor;

            if (this.TextAlign == ContentAlignment.MiddleLeft || this.TextAlign == ContentAlignment.TopLeft || this.TextAlign == ContentAlignment.BottomLeft)
            {
                Rectangle textRect = new Rectangle(rect.X + PaddingIzquierdo, rect.Y, Math.Max(0, rect.Width - PaddingIzquierdo - 5), rect.Height);
                TextRenderer.DrawText(g, this.Text, this.Font, textRect, colorTexto,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
            else
            {
                TextRenderer.DrawText(g, this.Text, this.Font, rect, colorTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
