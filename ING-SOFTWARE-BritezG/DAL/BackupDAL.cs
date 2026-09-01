using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class BackupDAL
    {
        private readonly Conexion conexion = new Conexion();
        private const string NOMBRE_BD = "ING";

        public void CrearBackup(string ruta)
        {
            try
            {
                string query = $@"BACKUP DATABASE [{NOMBRE_BD}] TO DISK = @ruta WITH INIT";

                SqlParameter[] parametros =
                {
                    new SqlParameter("@ruta", ruta)
                };

                conexion.ExecuteNonQuery(query, parametros);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RestaurarBackup(string ruta)
        {
            try
            {
                string query = $@" ALTER DATABASE [{NOMBRE_BD}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                    RESTORE DATABASE [{NOMBRE_BD}] FROM DISK = @ruta WITH REPLACE;
                                    ALTER DATABASE [{NOMBRE_BD}] SET MULTI_USER;";

                SqlParameter[] parametros =
                {
                    new SqlParameter("@ruta", ruta)
                };

                conexion.ExecuteNonQueryMaster(query, parametros);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
