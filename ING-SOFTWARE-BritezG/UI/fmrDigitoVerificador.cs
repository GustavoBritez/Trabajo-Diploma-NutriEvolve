using BLL;
using Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    public partial class fmrDigitoVerificador : Form, IIdiomaObserver
    {
        private readonly DigitoVerificadorBLL _digitoVerificadorBLL = new();
        private readonly BackupBLL _backupBLL = new();
        private readonly IdiomaBLL _idiomaBLL = new();
        private List<string> _tablasAlteradas = new();

        public fmrDigitoVerificador(List<string>? tablasAlteradas = null)
        {
            InitializeComponent();

            if (tablasAlteradas != null && tablasAlteradas.Count > 0)
            {
                _tablasAlteradas = tablasAlteradas;
            }
            else
            {
                _tablasAlteradas = _digitoVerificadorBLL.ObtenerTablasAlteradas();
            }

            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
            ActualizarMensajeError();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirFormulario(this, _idiomaBLL);
                ActualizarMensajeError();
            }
        }

        private void ActualizarMensajeError()
        {
            string nombreTablas = (_tablasAlteradas != null && _tablasAlteradas.Count > 0)
                ? string.Join(", ", _tablasAlteradas)
                : "Desconocida";

            lblMensajeError.Text = $"Hubo Cambios en la Tabla {nombreTablas}";
        }

        private void btnRecalcularDV_Click(object sender, EventArgs e)
        {
            DialogResult confirm = _idiomaBLL.MostrarMensaje(
                "msg_confirmar_recalcular_dv",
                "titulo_confirmar_recalcular_dv",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _digitoVerificadorBLL.RecalcularYPersistir();
                ServicesSessionManager.Instancia.RegistrarEstadoIntegridad(false);

                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    if (dniActual == 0) dniActual = 12345678;
                    new EventoBLL().RegistrarEvento(1, "Recálculo y restauración manual de Dígitos Verificadores (DV) en la Base de Datos", dniActual, "Seguridad");
                }
                catch { }

                _idiomaBLL.MostrarMensaje(
                    "msg_dv_recalculado_ok",
                    "titulo_recalculo_exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (ServicesSessionManager.Instancia.ObtenerUsuarioActivo() != null)
                {
                    FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
                }
                else
                {
                    FormManager.Navegar(this, FormManager.ObtenerLogin());
                }
            }
            catch (Exception ex)
            {
                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    if (dniActual == 0) dniActual = 12345678;
                    new EventoBLL().RegistrarEvento(1, $"Error al recalcular Dígitos Verificadores: {ex.Message}", dniActual, "Seguridad");
                }
                catch { }

                _idiomaBLL.MostrarMensaje("msg_error_recalcular_dv", "titulo_error_recalcular", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnSubirBackup_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = _idiomaBLL.ExisteTraduccion("dialog_seleccionar_backup")
                ? _idiomaBLL.Traducir("dialog_seleccionar_backup")
                : "Seleccionar Backup";
            openFileDialog.Filter = _idiomaBLL.ExisteTraduccion("dialog_filtro_backup")
                ? _idiomaBLL.Traducir("dialog_filtro_backup")
                : "Backup (*.bak)|*.bak|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

            string rutaBackup = openFileDialog.FileName;
            if (string.IsNullOrWhiteSpace(rutaBackup)) return;

            DialogResult confirm = _idiomaBLL.MostrarMensaje(
                "msg_confirmar_restore",
                "titulo_confirmar_restore",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _backupBLL.RealizarRestore(rutaBackup);
                _digitoVerificadorBLL.RecalcularYPersistir();
                ServicesSessionManager.Instancia.RegistrarEstadoIntegridad(false);

                _idiomaBLL.MostrarMensaje(
                    "msg_restore_ok",
                    "titulo_restore_ok",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Application.Restart();
            }
            catch (Exception ex)
            {
                _idiomaBLL.MostrarMensaje("msg_error_restore", "titulo_error_restore", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            ServicesSessionManager.Instancia.Logout();
            Application.Exit();
        }
    }

    /// <summary>
    /// Alias de compatibilidad frmDigitoVerificador
    /// </summary>
    public class frmDigitoVerificador : fmrDigitoVerificador
    {
        public frmDigitoVerificador(List<string>? tablasAlteradas = null) : base(tablasAlteradas) { }
    }
}
