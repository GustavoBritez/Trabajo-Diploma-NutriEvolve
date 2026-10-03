using BLL;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public static class TraductorUI
    {
        public static void SuscribirFormulario(Form form, IIdiomaObserver observer)
        {
            if (form == null || observer == null) return;
            ServicesSessionManager.Instancia.Suscribir(observer);
            form.FormClosed -= Form_FormClosed;
            form.FormClosed += Form_FormClosed;
            form.Disposed -= Form_Disposed;
            form.Disposed += Form_Disposed;
        }

        private static void Form_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (sender is IIdiomaObserver observer)
            {
                ServicesSessionManager.Instancia.Desuscribir(observer);
            }
        }

        private static void Form_Disposed(object? sender, EventArgs e)
        {
            if (sender is IIdiomaObserver observer)
            {
                ServicesSessionManager.Instancia.Desuscribir(observer);
            }
        }

        public static void TraducirFormulario(Form form, IdiomaBLL idiomaBLL)
        {
            if (form == null || form.IsDisposed) return;
            if (ServicesSessionManager.Instancia.ObtenerIdioma() == null) return;

            // 1. Traducir el Título de la Ventana (Form.Text)
            if (!string.IsNullOrEmpty(form.Name) && idiomaBLL.ExisteTraduccion(form.Name))
            {
                form.Text = idiomaBLL.Traducir(form.Name);
            }

            // 2. Traducir Menú Principal si existe
            if (form.MainMenuStrip != null)
            {
                TraducirToolStrip(form.MainMenuStrip, idiomaBLL);
            }

            // 3. Traducir todos los controles y contenedores
            TraducirControles(form.Controls, idiomaBLL, form.Name);
        }

        public static void TraducirControles(Control.ControlCollection controles, IdiomaBLL idiomaBLL, string? formName = null)
        {
            if (controles == null) return;

            foreach (Control control in controles)
            {
                if (control == null || control.IsDisposed) continue;

                // Traducir texto del control si tiene clave definida y no es de entrada de datos
                if (!(control is TextBox) && !(control is DateTimePicker) && control.Name != "cmbIdioma")
                {
                    string? clave = null;

                    // 1. Claves contextuales por formulario
                    if (!string.IsNullOrEmpty(formName))
                    {
                        if (idiomaBLL.ExisteTraduccion($"{formName}_{control.Name}"))
                        {
                            clave = $"{formName}_{control.Name}";
                        }
                        else if (idiomaBLL.ExisteTraduccion($"{control.Name}_{formName}"))
                        {
                            clave = $"{control.Name}_{formName}";
                        }
                        else if (formName.StartsWith("Frm") && idiomaBLL.ExisteTraduccion($"{control.Name}{formName.Substring(3).Replace("_DNI101", "")}"))
                        {
                            clave = $"{control.Name}{formName.Substring(3).Replace("_DNI101", "")}";
                        }
                        else if (idiomaBLL.ExisteTraduccion($"{control.Name}{formName.Replace("_DNI101", "")}"))
                        {
                            clave = $"{control.Name}{formName.Replace("_DNI101", "")}";
                        }
                    }

                    // 2. Tag como clave explícita de traducción
                    if (clave == null && control.Tag is string tag && !string.IsNullOrWhiteSpace(tag) && idiomaBLL.ExisteTraduccion(tag))
                    {
                        clave = tag;
                    }

                    // 3. Nombre directo del control (evitando colisión de lblTitulo/lblSubtitulo si no es Login)
                    if (clave == null && !string.IsNullOrEmpty(control.Name) && idiomaBLL.ExisteTraduccion(control.Name))
                    {
                        if ((control.Name == "lblTitulo" || control.Name == "lblSubtitulo") && formName != null && formName != "Login")
                        {
                            // Evitar sobreescribir títulos específicos con el genérico de Login
                        }
                        else
                        {
                            clave = control.Name;
                        }
                    }

                    if (clave != null)
                    {
                        control.Text = idiomaBLL.Traducir(clave);
                    }
                }

                // Manejo de DataGridView y sus encabezados de columna
                if (control is DataGridView dgv)
                {
                    TraducirDataGridView(dgv, idiomaBLL, formName);
                }
                // Manejo de ToolStrip / MenuStrip / StatusStrip
                else if (control is ToolStrip ts)
                {
                    TraducirToolStrip(ts, idiomaBLL);
                }
                // Manejo de TabControl y sus TabPages
                else if (control is TabControl tc)
                {
                    foreach (TabPage tp in tc.TabPages)
                    {
                        if (!string.IsNullOrEmpty(tp.Name) && idiomaBLL.ExisteTraduccion(tp.Name))
                        {
                            tp.Text = idiomaBLL.Traducir(tp.Name);
                        }
                        if (tp.HasChildren)
                        {
                            TraducirControles(tp.Controls, idiomaBLL, formName);
                        }
                    }
                }

                // Menús contextuales del control
                if (control.ContextMenuStrip != null)
                {
                    TraducirToolStrip(control.ContextMenuStrip, idiomaBLL);
                }

                // Recursión para controles anidados dentro de paneles, groupboxes, etc.
                if (control.HasChildren && !(control is DataGridView))
                {
                    TraducirControles(control.Controls, idiomaBLL, formName);
                }
            }
        }

        public static void TraducirDataGridView(DataGridView dgv, IdiomaBLL idiomaBLL, string? formName = null)
        {
            if (dgv == null || dgv.IsDisposed) return;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!string.IsNullOrEmpty(formName) && !string.IsNullOrEmpty(col.Name) && idiomaBLL.ExisteTraduccion($"{formName}_{col.Name}"))
                {
                    col.HeaderText = idiomaBLL.Traducir($"{formName}_{col.Name}");
                }
                else if (!string.IsNullOrEmpty(col.Name) && idiomaBLL.ExisteTraduccion(col.Name))
                {
                    col.HeaderText = idiomaBLL.Traducir(col.Name);
                }
                else if (!string.IsNullOrEmpty(col.DataPropertyName) && idiomaBLL.ExisteTraduccion(col.DataPropertyName))
                {
                    col.HeaderText = idiomaBLL.Traducir(col.DataPropertyName);
                }
                else if (!string.IsNullOrEmpty(col.HeaderText) && idiomaBLL.ExisteTraduccion(col.HeaderText))
                {
                    col.HeaderText = idiomaBLL.Traducir(col.HeaderText);
                }
            }
        }

        public static void TraducirToolStrip(ToolStrip ts, IdiomaBLL idiomaBLL)
        {
            if (ts == null || ts.IsDisposed) return;

            foreach (ToolStripItem item in ts.Items)
            {
                TraducirItem(item, idiomaBLL);
            }
        }

        public static void TraducirToolStripItems(ToolStripItemCollection items, IdiomaBLL idiomaBLL)
        {
            if (items == null) return;
            foreach (ToolStripItem item in items)
            {
                TraducirItem(item, idiomaBLL);
            }
        }

        private static void TraducirItem(ToolStripItem item, IdiomaBLL idiomaBLL)
        {
            if (item == null) return;

            if (!string.IsNullOrEmpty(item.Name) && idiomaBLL.ExisteTraduccion(item.Name))
            {
                item.Text = idiomaBLL.Traducir(item.Name);
            }
            else if (!string.IsNullOrEmpty(item.Text) && idiomaBLL.ExisteTraduccion(item.Text))
            {
                item.Text = idiomaBLL.Traducir(item.Text);
            }

            if (item is ToolStripDropDownItem dropDown && dropDown.HasDropDownItems)
            {
                foreach (ToolStripItem subItem in dropDown.DropDownItems)
                {
                    TraducirItem(subItem, idiomaBLL);
                }
            }
        }

        public static void ConfigurarComboIdiomas(ComboBox cmb, IdiomaBLL idiomaBLL, Action<Idioma>? alCambiarIdioma = null)
        {
            if (cmb == null || cmb.IsDisposed) return;

            var idiomas = idiomaBLL.ObtenerIdiomas();
            if (idiomas == null || idiomas.Count == 0) return;

            cmb.SelectedIndexChanged -= Cmb_SelectedIndexChanged;

            cmb.DisplayMember = "Nombre";
            cmb.ValueMember = "Codigo";
            cmb.DataSource = new List<Idioma>(idiomas);

            var idiomaActual = ServicesSessionManager.Instancia.ObtenerIdioma();
            if (idiomaActual != null)
            {
                for (int i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i] is Idioma id && (id.Codigo == idiomaActual.Codigo || id.Nombre == idiomaActual.Nombre))
                    {
                        cmb.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (cmb.Items.Count > 0)
            {
                cmb.SelectedIndex = 0;
            }

            cmb.Tag = alCambiarIdioma;
            cmb.SelectedIndexChanged += Cmb_SelectedIndexChanged;
        }

        private static void Cmb_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender is ComboBox cmb && cmb.SelectedItem is Idioma seleccionado)
            {
                var idiomaActual = ServicesSessionManager.Instancia.ObtenerIdioma();
                if (idiomaActual == null || idiomaActual.Codigo != seleccionado.Codigo)
                {
                    ServicesSessionManager.Instancia.CambiarIdioma(seleccionado);
                    if (cmb.Tag is Action<Idioma> callback)
                    {
                        callback(seleccionado);
                    }
                }
            }
        }

        public static void SincronizarComboIdioma(ComboBox cmb)
        {
            if (cmb == null || cmb.IsDisposed) return;
            var idiomaActual = ServicesSessionManager.Instancia.ObtenerIdioma();
            if (idiomaActual == null) return;

            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i] is Idioma id && (id.Codigo == idiomaActual.Codigo || id.Nombre == idiomaActual.Nombre))
                {
                    if (cmb.SelectedIndex != i)
                    {
                        cmb.SelectedIndexChanged -= Cmb_SelectedIndexChanged;
                        cmb.SelectedIndex = i;
                        cmb.SelectedIndexChanged += Cmb_SelectedIndexChanged;
                    }
                    break;
                }
            }
        }
    }
}
