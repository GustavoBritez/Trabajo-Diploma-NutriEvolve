using DAL;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BackupBLL
    {
        BackupDAL backupDAL;
        public BackupBLL()
        {
            backupDAL=new BackupDAL();
        }
        public void RealizarBackup(string ruta)
        {
            try
            {
                backupDAL.CrearBackup(ruta);
                BitacoraBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Copia de Seguridad (Backup) generada en: {ruta}";
                bitacoraBLL.RegistrarBitacora(3, descripcion, dniActual, "Respaldo");
            }
            catch (Exception ex)
            {
                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    new BitacoraBLL().RegistrarBitacora(1, $"Error al generar Copia de Seguridad: {ex.Message}", dniActual, "Respaldo");
                }
                catch { }
                throw;
            }
        }

        public void RealizarRestore(string ruta)
        {
            try
            {
                backupDAL.RestaurarBackup(ruta);
                BitacoraBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Restauración de Base de Datos (Restore) ejecutada desde: {ruta}";
                bitacoraBLL.RegistrarBitacora(5, descripcion, dniActual, "Respaldo");
            }
            catch (Exception ex)
            {
                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    new BitacoraBLL().RegistrarBitacora(1, $"Error al restaurar Base de Datos: {ex.Message}", dniActual, "Respaldo");
                }
                catch { }
                throw;
            }
        }
    }
}
