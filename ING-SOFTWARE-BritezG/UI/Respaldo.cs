using BLL;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Respaldo : Form, IIdiomaObserver
    {
        private IdiomaBLL idiomaBLL = new IdiomaBLL();
        public Respaldo()
        {
            InitializeComponent();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();

            bool baseCorrupta = ServicesSessionManager.Instancia.BaseDatosCorruptaDetectada;
            if (baseCorrupta)
            {
                btnRealizarBackup.Enabled = false;
                btnBuscarBackup.Enabled = false;
                txtRutaBackup.Enabled = false;
            }
        }

        private void btnRealizarBackup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaBackup.Text))
            {
                idiomaBLL.MostrarMensaje("msg_falta_ruta_backup", "titulo_falta_ruta_backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BackupBLL backup = new BackupBLL();


                // 1. Esto hace el backup físico y registra el evento en la Bitácora
                backup.RealizarBackup(txtRutaBackup.Text);

                // =========================================================
                // 2. ACTUALIZACIÓN DEL DÍGITO VERIFICADOR (Absorbe el evento)
                // =========================================================
                DigitoVerificadorBLL dvBll = new DigitoVerificadorBLL();
                dvBll.RecalcularYPersistir();
                // =========================================================


                idiomaBLL.MostrarMensaje("msg_backup_ok", "titulo_backup_ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_backup", "titulo_error_backup", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void btnBuscarBackup_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = idiomaBLL.Traducir("dialog_guardar_backup");
            saveFileDialog1.Filter = idiomaBLL.Traducir("dialog_filtro_backup");
            saveFileDialog1.DefaultExt = "bak";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.FileName = $"BackupING_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRutaBackup.Text = saveFileDialog1.FileName;
            }
        }

        private void btnBuscarRestore_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = idiomaBLL.Traducir("dialog_seleccionar_backup");
            openFileDialog1.Filter = idiomaBLL.Traducir("dialog_filtro_backup");

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRutaRestore.Text = openFileDialog1.FileName;
            }
        }

        private void btnRealizarRestore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaRestore.Text))
            {
                idiomaBLL.MostrarMensaje("msg_falta_archivo_restore", "titulo_falta_archivo_restore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = idiomaBLL.MostrarMensaje("msg_confirmar_restore", "titulo_confirmar_restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado != DialogResult.Yes)
                return;

            try
            {
                BackupBLL backup = new BackupBLL();

                backup.RealizarRestore(txtRutaRestore.Text);

                idiomaBLL.MostrarMensaje("msg_restore_ok", "titulo_restore_ok", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Application.Restart();
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_restore", "titulo_error_restore", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }



        #region Idioma
        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirFormulario(this, idiomaBLL);
            }
        }
        #endregion

        private void btnSalirR_Click(object sender, EventArgs e)
        {
            FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
        }
    }

}
