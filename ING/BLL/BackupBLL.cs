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
            backupDAL.CrearBackup(ruta);
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"BackUp";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Respaldo");

        }
        public void RealizarRestore(string ruta)
        {
            backupDAL.RestaurarBackup(ruta);
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Restore";
            bitacoraBLL.RegistrarEvento(5, descripcion, dniActual, "Respaldo");
        }
    }
}
