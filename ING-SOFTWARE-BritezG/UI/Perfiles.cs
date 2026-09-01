using BE;
using BLL;
using BLL.Perfiles;
using DAL;
using DAL.Perfiles;
using Microsoft.VisualBasic;
using Services;
using Services.Perfiles;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    public partial class Perfiles : Form, IIdiomaObserver
    {
        private FamiliaBLL _familiaBLL;
        private PatenteBLL _patenteBLL;
        private PerfilBLL _perfilBLL;

        private IdiomaBLL idiomaBLL = new IdiomaBLL();

        public Perfiles()
        {
            InitializeComponent();
            ServicesSessionManager.Instancia.Suscribir(this);
            ActualizarIdioma();

            _familiaBLL = new FamiliaBLL();
            _patenteBLL = new PatenteBLL();
            _perfilBLL = new PerfilBLL();

            dgvFamilias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPerfiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPermisos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvFamilias.MultiSelect = false;
            dgvPerfiles.MultiSelect = false;
            dgvPermisos.MultiSelect = false;

            dgvFamilias.ReadOnly = true;
            dgvPerfiles.ReadOnly = true;
            dgvPermisos.ReadOnly = true;
            ConfigurarEstiloGrillas();
        }

        private void Perfiles_Load(object sender, EventArgs e)
        {
            CargarGrillas();
        }

        private void CargarGrillas()
        {
            try
            {
                Idioma idioma = ServicesSessionManager.Instancia.ObtenerIdioma();

                string nombrePerfil = "Nombre del Perfil";
                string nombreFamilia = "Nombre de Familia";
                string nombrePermiso = "Acciones / Permisos";

                switch (idioma.Nombre)
                {
                    case "English":
                        nombrePerfil = "Profile Name";
                        nombreFamilia = "Family Name";
                        nombrePermiso = "Actions / Permissions";
                        break;

                    case "Portugues":
                        nombrePerfil = "Nome do Perfil";
                        nombreFamilia = "Nome da Família";
                        nombrePermiso = "Ações / Permissões";
                        break;
                }
                List<Perfil> listaPerfiles = _perfilBLL.ObtenerPerfiles();

                dgvPerfiles.DataSource = null;
                dgvPerfiles.DataSource = new List<Perfil>(listaPerfiles);
                dgvPerfiles.Columns["Id"].Visible = false;
                dgvPerfiles.Columns["Nombre"].HeaderText = nombrePerfil;

                List<Perfil> listaFamilias = _familiaBLL.ObtenerFamiliasPerfil();

                dgvFamilias.DataSource = null;
                dgvFamilias.DataSource = new List<Perfil>(listaFamilias);
                dgvFamilias.Columns["Id"].Visible = false;
                dgvFamilias.Columns["Nombre"].HeaderText = nombreFamilia;

                List<Perfil> listaPermisos = _patenteBLL.ObtenerPermisosPerfil();

                dgvPermisos.DataSource = null;
                dgvPermisos.DataSource = new List<Perfil>(listaPermisos);
                dgvPermisos.Columns["Id"].Visible = false;
                dgvPermisos.Columns["Nombre"].HeaderText = nombrePermiso;
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_cargar_datos", "titulo_error_cargar_datos", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                RefrescarPermisosUsuarioActivo();
            }
        }

        private void RefrescarPermisosUsuarioActivo()
        {
            UsuarioBE usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();

            if (usuarioActivo == null)
            {
                return;
            }

            PatenteBLL patenteBLL = new PatenteBLL();
            List<PatenteServices> listaPatentesActualizada = patenteBLL.ObtenerPermisosDePerfil(usuarioActivo._IdPerfil);
            List<string> nombresPermisosActualizados = listaPatentesActualizada.Select(p => p.Nombre).ToList();

            ServicesSessionManager.Instancia.CargarPermisosDelUsuario(nombresPermisosActualizados);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {

            RefrescarPermisosUsuarioActivo();

            FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
        }


        #region Arbol visual
        private void MostrarArbolEnTreeView_Perfil(int idPerfilSeleccionado)
        {
            try
            {
                Vista_Familia.Nodes.Clear();

                // 1. Buscamos el perfil y todo lo que tiene asignado
                FamiliaServices perfilRaiz = _perfilBLL.ObtenerArbolPerfil(idPerfilSeleccionado);
                if (perfilRaiz == null) return;

                // 2. Creamos el nodo principal
                TreeNode nodoRaiz = new TreeNode("👤 Perfil: " + perfilRaiz.Nombre);
                nodoRaiz.Tag = perfilRaiz.Id;

                // 3. Dibujamos las familias y permisos que cuelgan de este perfil
                DibujarNodosFamiliasRecursivo(perfilRaiz, nodoRaiz);

                // 4. Mostramos en pantalla
                Vista_Familia.Nodes.Add(nodoRaiz);
                Vista_Familia.ExpandAll();
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_arbol_perfil", "titulo_error_arbol_perfil", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void MostrarArbolEnTreeView_Familia(int idFamiliaSeleccionada)
        {
            try
            {
                Vista_Familia.Nodes.Clear();

                // 1. Buscamos la familia y sus componentes en la BLL
                FamiliaServices familiaRaiz = _familiaBLL.ObtenerArbolFamiliar(idFamiliaSeleccionada);
                if (familiaRaiz == null) return;

                // 2. Creamos el nodo principal
                TreeNode nodoRaiz = new TreeNode("📦 Familia: " + familiaRaiz.Nombre);
                nodoRaiz.Tag = familiaRaiz.Id;

                // 3. Dibujamos directamente los permisos y subfamilias que tiene adentro
                DibujarNodosFamiliasRecursivo(familiaRaiz, nodoRaiz);

                // 4. Mostramos en pantalla
                Vista_Familia.Nodes.Add(nodoRaiz);
                Vista_Familia.ExpandAll();
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_arbol_familia", "titulo_error_arbol_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void DibujarNodosFamiliasRecursivo(FamiliaServices familiaPadre, TreeNode nodoVisualPadre)
        {
            foreach (Perfil hijo in familiaPadre.Hijos)
            {
                if (hijo.EsCompuesto())
                {
                    TreeNode nodoHijo = new TreeNode(hijo.Nombre);
                    nodoHijo.Tag = hijo.Id;

                    FamiliaServices subFamilia = (FamiliaServices)hijo;
                    DibujarNodosFamiliasRecursivo(subFamilia, nodoHijo);

                    nodoVisualPadre.Nodes.Add(nodoHijo);
                }
                else
                {
                    TreeNode nodoHoja = new TreeNode(hijo.Nombre);
                    nodoHoja.Tag = hijo.Id;

                    nodoVisualPadre.Nodes.Add(nodoHoja);
                }
            }
        }

        private void dgvFamilias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFamilias.CurrentRow != null)
            {
                int idFamilia = (int)dgvFamilias.CurrentRow.Cells["Id"].Value;

                MostrarArbolEnTreeView_Familia(idFamilia);
            }
        }

        private void dgvPerfiles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPerfiles.CurrentRow != null)
            {
                int idPerfil = (int)dgvPerfiles.CurrentRow.Cells["Id"].Value;

                MostrarArbolEnTreeView_Perfil(idPerfil);
            }
        }
        #endregion

        #region Eliminar


        private void Eliminar_Permiso_A_Familia_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Apuntamos a la grilla de Familias (Centro)
                if (dgvFamilias.CurrentRow == null || dgvPermisos.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_familia_permiso", "titulo_sel_familia_permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Extraemos los IDs
                int idFamilia = (int)dgvFamilias.CurrentRow.Cells["Id"].Value;
                int idPermiso = (int)dgvPermisos.CurrentRow.Cells["Id"].Value;

                // 3. Extraemos los NOMBRES para la validación y el cartel
                string nombreFamilia = dgvFamilias.CurrentRow.Cells["Nombre"].Value.ToString();
                string nombrePermiso = dgvPermisos.CurrentRow.Cells["Nombre"].Value.ToString();

                DialogResult respuesta = idiomaBLL.MostrarMensaje(
                    "msg_confirmar_desvincular_permiso_familia", "titulo_confirmar_desvincular_permiso_familia",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, nombrePermiso, nombreFamilia);

                if (respuesta == DialogResult.Yes)
                {
                    // 4. Llamamos a la BLL de FAMILIAS, no de Perfiles
                    _familiaBLL.EliminarPermisoFamilia(idFamilia, idPermiso, nombreFamilia, nombrePermiso);

                    idiomaBLL.MostrarMensaje("msg_permiso_desvinculado_familia", "titulo_permiso_desvinculado_familia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                }
            }
            catch (ArgumentException argEx)
            {
                // Atrapa nuestra validación si el permiso en realidad no estaba en esa familia
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_sel_familia_permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_desvincular_permiso_familia", "titulo_error_desvincular_permiso_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }
        private void Eliminar_Permiso_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPermisos.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_permiso", "titulo_sel_permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPermiso = (int)dgvPermisos.CurrentRow.Cells["Id"].Value;
                string nombrePermiso = dgvPermisos.CurrentRow.Cells["Nombre"].Value.ToString();

                // Actualizamos el mensaje para reflejar el borrado en cascada
                DialogResult respuesta = idiomaBLL.MostrarMensaje(
                    "msg_confirmar_eliminar_permiso", "titulo_confirmar_eliminar_permiso",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, nombrePermiso);

                if (respuesta == DialogResult.Yes)
                {
                    // Ejecutamos la BLL
                    _patenteBLL.EliminarPermiso(idPermiso, nombrePermiso);

                    idiomaBLL.MostrarMensaje("msg_permiso_eliminado", "titulo_permiso_eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                }
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_eliminar_permiso", "titulo_error_eliminar_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void Eliminar_Perfil_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validamos que haya algo seleccionado en la grilla izquierda
                if (dgvPerfiles.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_perfil", "titulo_sel_perfil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Extraemos ID y Nombre
                int idPerfil = (int)dgvPerfiles.CurrentRow.Cells["Id"].Value;
                string nombrePerfil = dgvPerfiles.CurrentRow.Cells["Nombre"].Value.ToString();

                // 3. Advertencia de cascada
                DialogResult respuesta = idiomaBLL.MostrarMensaje(
                    "msg_confirmar_eliminar_perfil", "titulo_confirmar_eliminar_perfil",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, nombrePerfil);

                if (respuesta == DialogResult.Yes)
                {
                    // 4. Mandamos a borrar
                    _perfilBLL.EliminarPerfil(idPerfil, nombrePerfil);

                    // 5. Avisamos y refrescamos
                    idiomaBLL.MostrarMensaje("msg_perfil_eliminado", "titulo_perfil_eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                }
            }
            catch (ArgumentException argEx)
            {
                // ¡Atrapa tu validación y muestra SOLO tu cartel personalizado con ícono de advertencia!
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_sel_perfil", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                // Atrapa cualquier otro error real de base de datos
                idiomaBLL.MostrarMensaje("msg_error_eliminar_perfil", "titulo_error_eliminar_perfil", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void Eliminar_Familia_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validamos apuntando a la grilla CENTRAL
                if (dgvFamilias.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_familia", "titulo_sel_familia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Extraemos los datos
                int idFamilia = (int)dgvFamilias.CurrentRow.Cells["Id"].Value;
                string nombreFamilia = dgvFamilias.CurrentRow.Cells["Nombre"].Value.ToString();

                // 3. Advertimos al usuario sobre el borrado en cascada
                DialogResult respuesta = idiomaBLL.MostrarMensaje(
                    "msg_confirmar_eliminar_familia", "titulo_confirmar_eliminar_familia",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, nombreFamilia);

                if (respuesta == DialogResult.Yes)
                {
                    // 4. Ejecutamos la BLL
                    _familiaBLL.EliminarFamilia(idFamilia, nombreFamilia);

                    // 5. Éxito y recarga visual
                    idiomaBLL.MostrarMensaje("msg_familia_eliminada", "titulo_familia_eliminada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                }
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_eliminar_familia", "titulo_error_eliminar_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        #endregion Eliminar

        #region Agregar
        private void Agregar_Familia_A_Perfil(object sender, EventArgs e)
        {
            try
            {
                if (dgvPerfiles.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_perfil_izq", "titulo_sel_perfil_izq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvFamilias.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_familia_central", "titulo_sel_familia_central", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPerfil = (int)dgvPerfiles.CurrentRow.Cells["Id"].Value;
                int idFamilia = (int)dgvFamilias.CurrentRow.Cells["Id"].Value;

                string nombrePerfil = dgvPerfiles.CurrentRow.Cells["Nombre"].Value.ToString();
                string nombreFamilia = dgvFamilias.CurrentRow.Cells["Nombre"].Value.ToString(); // LÍNEA AGREGADA

                _perfilBLL.AgregarFamiliaAlPerfil(idPerfil, idFamilia, nombrePerfil, nombreFamilia); // LLAMADA MODIFICADA

                idiomaBLL.MostrarMensaje("msg_familia_asignada_perfil", "titulo_familia_asignada_perfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_sel_familia_permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_asignar_familia", "titulo_error_asignar_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }
        private void Agregar_Permiso_A_Familia(object sender, EventArgs e)
        {
            try
            {
                // 1. Cambiamos a la grilla de FAMILIAS (Centro)
                if (dgvFamilias.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_familia_central", "titulo_sel_familia_central", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Grilla de PERMISOS (Derecha)
                if (dgvPermisos.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_permiso_der", "titulo_sel_permiso_der", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. Extraemos los IDs y nombres correctos
                int idFamilia = (int)dgvFamilias.CurrentRow.Cells["Id"].Value;
                int idPermiso = (int)dgvPermisos.CurrentRow.Cells["Id"].Value;

                string nombrePermiso = dgvPermisos.CurrentRow.Cells["Nombre"].Value.ToString();
                string nombreFamilia = dgvFamilias.CurrentRow.Cells["Nombre"].Value.ToString();

                // 4. Llamamos al método correcto en la BLL de Familias
                _familiaBLL.AgregarPermisoAFamilia(idFamilia, idPermiso, nombrePermiso, nombreFamilia);

                idiomaBLL.MostrarMensaje("msg_permiso_asignado_familia", "titulo_permiso_asignado_familia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_sel_familia_permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_asignar_permiso_familia", "titulo_error_asignar_permiso_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }


        /*private void Agregar_Permiso(object sender, EventArgs e)
        {
            try
            {
                // CORRECCIÓN: Le pasamos explícitamente el ModoFormulario.Permiso
                using (FrmCrearPermiso frm = new FrmCrearPermiso(FrmCrearPermiso.ModoFormulario.Permiso))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string nuevoPermiso = frm.NombrePermiso;

                            // Creamos el permiso en la base de datos
                            _patenteBLL.CrearNuevoPermiso(nuevoPermiso);

                            // Si también seleccionó un botón en los combos, lo vinculamos
                            if (frm.TieneBotonAsignado)
                            {
                                _patenteBLL.VincularPermisoABoton(frm.NombreFormulario, frm.NombreBoton, nuevoPermiso);
                            }

                            idiomaBLL.MostrarMensaje("msg_permiso_creado", "titulo_permiso_creado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Actualizamos la vista para que el nuevo permiso aparezca al instante
                            CargarGrillas();
                        }
                        catch (Exception ex)
                        {
                            idiomaBLL.MostrarMensaje("msg_error_crear_permiso", "titulo_error_crear_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
                        }
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_crear_permiso", "titulo_error_crear_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }*/

        private void Agregar_Perfil(object sender, EventArgs e)
        {
            try
            {
                using (FrmCrearPermiso frm = new FrmCrearPermiso(FrmCrearPermiso.ModoFormulario.Perfil))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        string nombreNuevoPerfil = frm.NombrePermiso;

                        _perfilBLL.CrearNuevoPerfil(nombreNuevoPerfil);
                        idiomaBLL.MostrarMensaje("msg_perfil_creado", "titulo_perfil_creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarGrillas();
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_sel_perfil", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_crear_perfil", "titulo_error_crear_perfil", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void Agregar_Familia(object sender, EventArgs e)
        {
            try
            {
                if (dgvPermisos.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_permiso_antes_familia", "titulo_sel_permiso_antes_familia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPermisoSeleccionado = (int)dgvPermisos.CurrentRow.Cells["Id"].Value;
                string nombrePermisoSeleccionado = dgvPermisos.CurrentRow.Cells["Nombre"].Value.ToString();

                // ACÁ: Le pasamos el modo Familia
                using (FrmCrearPermiso frmPopup = new FrmCrearPermiso(FrmCrearPermiso.ModoFormulario.Familia))
                {
                    if (frmPopup.ShowDialog() == DialogResult.OK)
                    {
                        string nombreNuevaFamilia = frmPopup.NombrePermiso;
                        int idNuevaFamilia = _familiaBLL.CrearNuevaFamilia(nombreNuevaFamilia);
                        _familiaBLL.AgregarPermisoAFamilia(idNuevaFamilia, idPermisoSeleccionado, nombrePermisoSeleccionado, nombreNuevaFamilia);
                        idiomaBLL.MostrarMensaje("msg_familia_creada", "titulo_familia_creada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarGrillas();
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_sel_familia", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_crear_familia", "titulo_error_crear_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        #endregion

        #region GUI
        private void ConfigurarEstiloGrillas()
        {
            Color verdeOscuro = Color.FromArgb(46, 94, 67);
            Color verdeSeleccion = Color.FromArgb(180, 210, 190);
            Color fondoGrilla = Color.White;
            Color colorLineas = Color.FromArgb(200, 220, 205);

            DataGridView[] grillas = { dgvPerfiles, dgvFamilias, dgvPermisos };

            foreach (DataGridView dgv in grillas)
            {
                dgv.EnableHeadersVisualStyles = false;

                dgv.ColumnHeadersDefaultCellStyle.BackColor = verdeOscuro;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgv.ColumnHeadersHeight = 35;

                dgv.BackgroundColor = fondoGrilla;
                dgv.BorderStyle = BorderStyle.None;
                dgv.GridColor = colorLineas;
                dgv.DefaultCellStyle.BackColor = fondoGrilla;
                dgv.DefaultCellStyle.ForeColor = Color.Black;
                dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

                // --- Estilo de Selección ---
                dgv.DefaultCellStyle.SelectionBackColor = verdeSeleccion;
                dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

                // --- Comportamientos Generales ---
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.ReadOnly = true;
                dgv.RowHeadersVisible = false; // Oculta la columna vacía de la izquierda
                dgv.AllowUserToAddRows = false; // Saca la fila vacía extra del final
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Hace que las columnas ocupen todo el ancho
            }
            #endregion

        }
        #region Idioma
        private void TraducirToolStrip(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                string traduccion = idiomaBLL.Traducir(item.Name);

                if (traduccion != item.Name)
                    item.Text = traduccion;

                if (item is ToolStripDropDownItem dropDown)
                {
                    TraducirToolStrip(dropDown.DropDownItems);
                }
            }
        }
        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                Traducir(this.Controls);
                TraducirToolStrip(toolStripLabel1.DropDownItems);
                TraducirToolStrip(toolStripLabel2.DropDownItems);
                toolStripLabel1.Text = idiomaBLL.Traducir("toolStripLabel1");
                toolStripLabel2.Text = idiomaBLL.Traducir("toolStripLabel2");
                RelacionFamilia.Text = idiomaBLL.Traducir("RelacionFamilia");

            }
        }
        private void Traducir(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (!string.IsNullOrEmpty(control.Name))
                {
                    string traduccion = idiomaBLL.Traducir(control.Name);

                    if (traduccion != control.Name)
                        control.Text = traduccion;
                }

                if (control.HasChildren)
                    Traducir(control.Controls);
            }
        }
        #endregion

        private void familiaAFamiliaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Eliminar_Familia_A_Perfil_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validamos que haya selecciones en Izquierda (Perfil) y Centro (Familia)
                if (dgvPerfiles.CurrentRow == null || dgvFamilias.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_perfil_familia", "titulo_sel_perfil_familia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Extraemos los IDs
                int idPerfil = (int)dgvPerfiles.CurrentRow.Cells["Id"].Value;
                int idFamilia = (int)dgvFamilias.CurrentRow.Cells["Id"].Value;

                // 3. Extraemos los nombres para el cartel de confirmación
                string nombrePerfil = dgvPerfiles.CurrentRow.Cells["Nombre"].Value.ToString();
                string nombreFamilia = dgvFamilias.CurrentRow.Cells["Nombre"].Value.ToString();

                // 4. Pedimos confirmación
                DialogResult respuesta = idiomaBLL.MostrarMensaje(
                    "msg_confirmar_desvincular_familia_perfil", "titulo_confirmar_desvincular_familia_perfil",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, nombreFamilia, nombrePerfil);

                if (respuesta == DialogResult.Yes)
                {
                    // 5. Llamamos a la BLL
                    _perfilBLL.EliminarFamiliaDePerfil(idPerfil, idFamilia, nombrePerfil, nombreFamilia);

                    idiomaBLL.MostrarMensaje("msg_familia_desvinculada_perfil", "titulo_familia_desvinculada_perfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_desvincular_familia", "titulo_error_desvincular_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void RelacionFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                using (FrmSeleccionarFamilia frmPopup = new FrmSeleccionarFamilia())
                {
                    if (frmPopup.ShowDialog() == DialogResult.OK)
                    {
                        int idFamiliaHija = frmPopup.IdFamiliaOrigen;
                        int idFamiliaPadre = frmPopup.IdFamiliaDestino;

                        string nombreHija = frmPopup.NombreFamiliaOrigen;
                        string nombrePadre = frmPopup.NombreFamiliaDestino;

                        // Evaluamos qué botón presionó el usuario en el popup
                        if (frmPopup.EsVinculacion)
                        {
                            // Si EsVinculacion es TRUE, apretó "Vincular"
                            _familiaBLL.AgregarFamiliaAFamilia(idFamiliaPadre, idFamiliaHija, nombrePadre, nombreHija);
                            idiomaBLL.MostrarMensaje("msg_vincular_familia_ok", "titulo_vincular_familia_ok", MessageBoxButtons.OK, MessageBoxIcon.Information, nombreHija, nombrePadre);
                        }
                        else
                        {
                            // Si EsVinculacion es FALSE, apretó "Desvincular"
                            _familiaBLL.EliminarFamiliaDeFamilia(idFamiliaPadre, idFamiliaHija, nombrePadre, nombreHija);
                            idiomaBLL.MostrarMensaje("msg_desvincular_familia_ok", "titulo_desvincular_familia_ok", MessageBoxButtons.OK, MessageBoxIcon.Information, nombreHija, nombrePadre);
                        }

                        CargarGrillas();
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_gestionar_familia", "titulo_error_gestionar_familia", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }



        private void RelacionFamilia_MouseMove(object sender, MouseEventArgs e)
        {
            RelacionFamilia.BackColor = Color.FromArgb(200, 220, 205);
        }

        private void RelacionFamilia_MouseEnter(object sender, EventArgs e)
        {
            RelacionFamilia.BackColor = Color.FromArgb(200, 220, 205);

            // (Opcional) Si tu texto es blanco, quizás sobre el verde claro no se lea bien. 
            // Podés forzar que la letra se ponga negra al pasar el mouse:
            // RelacionFamilia.ForeColor = Color.Black; 
        }

        // 2. Cuando el mouse sale, lo devolvemos a la normalidad
        private void RelacionFamilia_MouseLeave(object sender, EventArgs e)
        {
            RelacionFamilia.Font = new Font(RelacionFamilia.Font, FontStyle.Bold);
        }

        private void RelacionFamilia_MouseHover(object sender, EventArgs e)
        {
            RelacionFamilia.Font = new Font(RelacionFamilia.Font, FontStyle.Regular);
        }

        private void Agregar_Permiso_A_Perfil_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPerfiles.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_perfil_izq", "titulo_sel_perfil_izq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvPermisos.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_permiso_der", "titulo_sel_permiso_der", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPerfil = (int)dgvPerfiles.CurrentRow.Cells["Id"].Value;
                int idPermiso = (int)dgvPermisos.CurrentRow.Cells["Id"].Value;

                string nombrePermiso = dgvPermisos.CurrentRow.Cells["Nombre"].Value.ToString();
                string nombrePerfil = dgvPerfiles.CurrentRow.Cells["Nombre"].Value.ToString();

                _perfilBLL.AgregarPermisoAPerfil(idPerfil, idPermiso, nombrePermiso, nombrePerfil);

                idiomaBLL.MostrarMensaje("msg_permiso_asignado_perfil", "titulo_permiso_asignado_perfil", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarGrillas();
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_asignar_permiso_perfil", "titulo_error_asignar_permiso_perfil", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void E_MenuItem_Permiso_A_Perfil_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPerfiles.CurrentRow == null || dgvPermisos.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_perfil_permiso", "titulo_sel_perfil_permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPerfil = (int)dgvPerfiles.CurrentRow.Cells["Id"].Value;
                int idPermiso = (int)dgvPermisos.CurrentRow.Cells["Id"].Value;
                string nombrePermiso = dgvPermisos.CurrentRow.Cells["Nombre"].Value.ToString();
                string nombrePerfil = dgvPerfiles.CurrentRow.Cells["Nombre"].Value.ToString();

                DialogResult confirmacion = idiomaBLL.MostrarMensaje(
                    "msg_confirmar_quitar_permiso_perfil", "titulo_confirmar_quitar_permiso_perfil",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, nombrePermiso, nombrePerfil);
                if (confirmacion == DialogResult.Yes)
                {
                    _perfilBLL.EliminarPermisoAPerfil(idPerfil, idPermiso, nombrePermiso, nombrePerfil);

                    idiomaBLL.MostrarMensaje("msg_permiso_eliminado_perfil", "titulo_permiso_eliminado_perfil", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarGrillas();
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_eliminar_permiso_perfil", "titulo_error_eliminar_permiso_perfil", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void Agregar_Permiso(object sender, EventArgs e)
        {
            try
            {
                // CORRECCIÓN: Le pasamos explícitamente el ModoFormulario.Permiso
                using (FrmCrearPermiso frm = new FrmCrearPermiso(FrmCrearPermiso.ModoFormulario.Permiso))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string nuevoPermiso = frm.NombrePermiso;

                            // Creamos el permiso en la base de datos
                            _patenteBLL.CrearNuevoPermiso(nuevoPermiso);

                            // Si también seleccionó un botón en los combos, lo vinculamos
                            if (frm.TieneBotonAsignado)
                            {
                                _patenteBLL.VincularPermisoABoton(frm.NombreFormulario, frm.NombreBoton, nuevoPermiso);
                            }

                            idiomaBLL.MostrarMensaje("msg_permiso_creado", "titulo_permiso_creado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Actualizamos la vista para que el nuevo permiso aparezca al instante
                            CargarGrillas();
                        }
                        catch (Exception ex)
                        {
                            idiomaBLL.MostrarMensaje("msg_error_crear_permiso", "titulo_error_crear_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
                        }
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_crear_permiso", "titulo_error_crear_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void permisoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // CORRECCIÓN: Le pasamos explícitamente el ModoFormulario.Permiso
                using (FrmCrearPermiso frm = new FrmCrearPermiso(FrmCrearPermiso.ModoFormulario.Permiso))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string nuevoPermiso = frm.NombrePermiso;

                            // Creamos el permiso en la base de datos
                            _patenteBLL.CrearNuevoPermiso(nuevoPermiso);

                            // Si también seleccionó un botón en los combos, lo vinculamos
                            if (frm.TieneBotonAsignado)
                            {
                                _patenteBLL.VincularPermisoABoton(frm.NombreFormulario, frm.NombreBoton, nuevoPermiso);
                            }

                            idiomaBLL.MostrarMensaje("msg_permiso_creado", "titulo_permiso_creado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Actualizamos la vista para que el nuevo permiso aparezca al instante
                            CargarGrillas();
                        }
                        catch (Exception ex)
                        {
                            idiomaBLL.MostrarMensaje("msg_error_crear_permiso", "titulo_error_crear_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
                        }
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                idiomaBLL.MostrarMensaje("msg_validacion", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning, argEx.Message);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_crear_permiso", "titulo_error_crear_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void permisoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPermisos.CurrentRow == null)
                {
                    idiomaBLL.MostrarMensaje("msg_sel_permiso", "titulo_sel_permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPermiso = (int)dgvPermisos.CurrentRow.Cells["Id"].Value;
                string nombrePermiso = dgvPermisos.CurrentRow.Cells["Nombre"].Value.ToString();

                // Actualizamos el mensaje para reflejar el borrado en cascada
                DialogResult respuesta = idiomaBLL.MostrarMensaje(
                    "msg_confirmar_eliminar_permiso", "titulo_confirmar_eliminar_permiso",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, nombrePermiso);

                if (respuesta == DialogResult.Yes)
                {
                    // Ejecutamos la BLL
                    _patenteBLL.EliminarPermiso(idPermiso, nombrePermiso);

                    idiomaBLL.MostrarMensaje("msg_permiso_eliminado", "titulo_permiso_eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                }
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_eliminar_permiso", "titulo_error_eliminar_permiso", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }
    }
}

