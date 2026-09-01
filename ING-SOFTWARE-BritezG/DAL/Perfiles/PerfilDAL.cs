using Microsoft.Data.SqlClient;
using Services.Perfiles;
using System.Data;

namespace DAL.Perfiles
{
    public class PerfilDAL
    {
        private readonly Conexion _conexion;
        private readonly string PERFIL_PERMISO = "Perfil_Permiso";
        private readonly string FAMILIA_PERFIL = "Familia_Perfil";

        public PerfilDAL()
        {
            this._conexion = new();
        }

        #region Agregar

        public void InsertarPerfilNuevo(string nombrePerfil)
        {
            string query = "INSERT INTO Perfil (Nombre) VALUES (@nombre)";
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@nombre", nombrePerfil)
            };

            _conexion.ExecuteNonQuery(query, parametros);
        }
        public void AgregarFamiliaAlPerfil(int idPerfil, int idFamilia)
        {
                string query = $"INSERT INTO {FAMILIA_PERFIL} (ID_Perfil, ID_Familia) VALUES (@idPerfil, @idFamilia)";

                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@idPerfil", idPerfil),
                    new SqlParameter("@idFamilia", idFamilia)
                };

            _conexion.ExecuteNonQuery(query, parametros);
        }
        public bool ExisteRelacionFamiliaPerfil(int idPerfil, int idFamilia)
        {
            string query = "SELECT COUNT(1) FROM Familia_Perfil WHERE ID_Perfil = @idPerfil AND ID_Familia = @idFamilia";
             
            SqlParameter[] parametros = {
                            new SqlParameter("@idPerfil", idPerfil),
                            new SqlParameter("@idFamilia", idFamilia)
                        };

            DataTable dt = _conexion.ExecuteReader(query, parametros);

            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }
        public bool ExisteRelacionPermisoPerfil(int idPerfil, int idPermiso)
        {
            string query = "SELECT COUNT(1) FROM Perfil_Permiso WHERE ID_Perfil = @idPerfil AND ID_Permiso = @idPermiso";

            SqlParameter[] parametros = {
                    new SqlParameter("@idPerfil", idPerfil),
                    new SqlParameter("@idPermiso", idPermiso)
                };

            DataTable dt = _conexion.ExecuteReader(query, parametros);

            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public List<PatenteServices> ObtenerPermisosDeFamilia(int idFamilia)
        {
            // CTE Recursivo para extraer todos los permisos de la familia y sus subfamilias hijas
            string query = @"
                WITH FamiliasRecursivas AS (
                    SELECT ID_Familia = @idFamilia
                    UNION ALL
                    SELECT ff.ID_FamiliaHija 
                    FROM Familia_Familia ff
                    INNER JOIN FamiliasRecursivas fr ON ff.ID_FamiliaPadre = fr.ID_Familia
                )
                SELECT DISTINCT P.ID_Permiso, P.Nombre
                FROM Permiso P
                INNER JOIN Permiso_Familia PF ON P.ID_Permiso = PF.ID_Permiso
                WHERE PF.ID_Familia IN (SELECT ID_Familia FROM FamiliasRecursivas)";

                    SqlParameter[] parametros = new SqlParameter[] {
                new SqlParameter("@idFamilia", idFamilia)
            };

            DataTable dt = _conexion.ExecuteReader(query, parametros);
            List<PatenteServices> lista = new();

            if (dt != null)
            {
                foreach (DataRow fila in dt.Rows)
                {
                    lista.Add(new PatenteServices(fila["Nombre"].ToString())
                    {
                        Id = Convert.ToInt32(fila["ID_Permiso"])
                    });
                }
            }
            return lista;
        }
        public void InsertarPermisoAFamilia(int idFamilia, int idPermiso)
        {
            string query = "INSERT INTO Permiso_Familia (ID_Familia, ID_Permiso) " +
                           "VALUES (@idFamilia, @idPermiso)";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@idFamilia", idFamilia),
                new SqlParameter("@idPermiso", idPermiso)
            };

            _conexion.ExecuteNonQuery(query, parametros);
        }


        #endregion

        #region Eliminar
        // Método para eliminar la relación
        public void EliminarPermisoAPerfil(int idPerfil, int idPermiso)
        {
            string query = "DELETE FROM [ING].[dbo].[Perfil_Permiso] WHERE ID_Perfil = @idPerfil AND ID_Permiso = @idPermiso";

            SqlParameter[] param = {
                new SqlParameter("@idPerfil", idPerfil),
                new SqlParameter("@idPermiso", idPermiso)
            };

            _conexion.ExecuteNonQuery(query, param);
        }
        
        public void EliminarPerfilDefinitivo(int idPerfil)
        {

            string queryFamilia = "DELETE FROM Familia_Perfil WHERE ID_Perfil = @id";
            _conexion.ExecuteNonQuery(queryFamilia, new SqlParameter[] { new SqlParameter("@id", idPerfil) });

            string queryPermiso = "DELETE FROM Perfil_Permiso WHERE ID_Perfil = @id";
            _conexion.ExecuteNonQuery(queryPermiso, new SqlParameter[] { new SqlParameter("@id", idPerfil) });

            string queryPerfil = "DELETE FROM Perfil WHERE ID_Perfil = @id";
            _conexion.ExecuteNonQuery(queryPerfil, new SqlParameter[] { new SqlParameter("@id", idPerfil) });
        }

        public bool PerfilTieneUsuarios(int idPerfil)
        {
            string query = "SELECT COUNT(1) FROM Usuarios WHERE ID_Perfil = @idPerfil";
            SqlParameter[] parametros = { new SqlParameter("@idPerfil", idPerfil) };

            DataTable dt = _conexion.ExecuteReader(query, parametros);

            if (dt != null && dt.Rows.Count > 0)
            {
                int cantidad = Convert.ToInt32(dt.Rows[0][0]);
                return cantidad > 0;
            }

            return false;
        }

        public void EliminarFamiliaDePerfil(int idPerfil, int idFamilia)
        {
            // Borramos la fila exacta que une este Perfil con esta Familia
            string query = "DELETE FROM Familia_Perfil WHERE ID_Perfil = @idPerf AND ID_Familia = @idFam";

            SqlParameter[] param = {
                new SqlParameter("@idPerf", idPerfil),
                new SqlParameter("@idFam", idFamilia)
            };

            _conexion.ExecuteNonQuery(query, param);
        }


        #endregion

        #region Obtener
        public int ObtenerIdPerfilPorNombre(string nombreRol)
        {
            int idPerfil = 0;

            try
            {
                string query = "SELECT ID_Perfil FROM Perfil WHERE Nombre = @nombre";

                Microsoft.Data.SqlClient.SqlParameter[] param = {
            new Microsoft.Data.SqlClient.SqlParameter("@nombre", nombreRol)
        };

                // Asumo que tu objeto de conexión se llama "conexion" igual que en UsuarioDAL
                System.Data.DataTable dt = _conexion.ExecuteReader(query, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    idPerfil = Convert.ToInt32(dt.Rows[0]["ID_Perfil"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar el ID del Perfil: {ex.Message}");
            }

            return idPerfil;
        }
        //AQUITOY
        public void AgregarPermisoAPerfil(int idPerfil, int idPermiso)
        {
            string query = "INSERT INTO [ING].[dbo].[Perfil_Permiso] (ID_Perfil, ID_Permiso) VALUES (@idPerfil, @idPermiso)";

            SqlParameter[] param = {
                new SqlParameter("@idPerfil", idPerfil),
                new SqlParameter("@idPermiso", idPermiso)
            };

            _conexion.ExecuteNonQuery(query, param);
        }

        public List<Perfil> ObtenerComponentesTotales()
        {
            List<Perfil> _perfil = new();

            string queryFamilias = "SELECT ID_Familia as Id, Nombre FROM Familia";
            DataTable dtFamilias = _conexion.ExecuteReader(queryFamilias, null);

            foreach (DataRow fila in dtFamilias.Rows)
            {
                int id = Convert.ToInt32(fila["Id"]);
                string nombre = fila["Nombre"].ToString();
                _perfil.Add(new FamiliaServices(nombre) { Id = id });
            }

            string queryPermisos = "SELECT ID_Permiso as Id, Nombre FROM Permiso";
            DataTable dtPermisos = _conexion.ExecuteReader(queryPermisos, null);

            foreach (DataRow fila in dtPermisos.Rows)
            {
                int id = Convert.ToInt32(fila["Id"]);
                string nombre = fila["Nombre"].ToString();
                _perfil.Add(new PatenteServices(nombre) { Id = id });
            }

            return _perfil;
        }

        public List<Perfil> ObtenerPerfiles()
        {
            List<Perfil> lista = new List<Perfil>();
            string query = "SELECT ID_Perfil, Nombre FROM Perfil";
            DataTable dt = _conexion.ExecuteReader(query, null);

            foreach (DataRow fila in dt.Rows)
            {
                int id = Convert.ToInt32(fila["ID_Perfil"]);
                string nombre = fila["Nombre"].ToString();

                lista.Add(new FamiliaServices(nombre) { Id = id });
            }
            return lista;
        }

        public bool ExistePermisoEnPerfil(int idPerfil, int idPermiso)
        {
            string query = "SELECT COUNT(1) FROM [ING].[dbo].[Perfil_Permiso] WHERE ID_Perfil = @idPerfil AND ID_Permiso = @idPermiso";

            SqlParameter[] param = {
                new SqlParameter("@idPerfil", idPerfil),
                new SqlParameter("@idPermiso", idPermiso)
            };

            DataTable dt = _conexion.ExecuteReader(query, param);

            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            return false;
        }

        public FamiliaServices ObtenerArbolPerfil(int idPerfil)
        {
            string queryPadre = "SELECT Nombre FROM Perfil WHERE ID_Perfil = @id";
            SqlParameter[] paramPadre = { new SqlParameter("@id", idPerfil) };
            DataTable dtPadre = _conexion.ExecuteReader(queryPadre, paramPadre);
            if (dtPadre.Rows.Count == 0) return null;

            string nombrePerfil = dtPadre.Rows[0]["Nombre"].ToString();
            FamiliaServices perfilArmado = new FamiliaServices(nombrePerfil) { Id = idPerfil };

            string queryFamilias = @"
                    SELECT f.ID_Familia, f.Nombre 
                    FROM Familia_Perfil fp
                    INNER JOIN Familia f ON fp.ID_Familia = f.ID_Familia
                    WHERE fp.ID_Perfil = @idPerfil";

            SqlParameter[] paramFam = { new SqlParameter("@idPerfil", idPerfil) };
            DataTable dtFam = _conexion.ExecuteReader(queryFamilias, paramFam);

            FamiliaDAL familiaDAL = new FamiliaDAL();

            foreach (DataRow fila in dtFam.Rows)
            {
                int idFamilia = Convert.ToInt32(fila["ID_Familia"]);
                FamiliaServices subFamilia = familiaDAL.ObtenerArbolFamiliar(idFamilia);
                if (subFamilia != null)
                    perfilArmado.Agregar(subFamilia);
            }

            string queryPermisos = @"
                SELECT p.ID_Permiso, p.Nombre 
                FROM Perfil_Permiso pp
                INNER JOIN Permiso p ON pp.ID_Permiso = p.ID_Permiso
                WHERE pp.ID_Perfil = @idPerfil";

            SqlParameter[] paramPerm = { new SqlParameter("@idPerfil", idPerfil) };
            DataTable dtPerm = _conexion.ExecuteReader(queryPermisos, paramPerm);

            foreach (DataRow fila in dtPerm.Rows)
            {
                int idPermiso = Convert.ToInt32(fila["ID_Permiso"]);
                string nombrePermiso = fila["Nombre"].ToString();
                perfilArmado.Agregar(new PatenteServices(nombrePermiso) { Id = idPermiso });
            }

            return perfilArmado;
        }

        public bool ExistePerfilPorNombre(string nombrePerfil)
        {
            string query = "SELECT COUNT(1) FROM Perfil WHERE Nombre = @nombre";

            SqlParameter[] param = {
                new SqlParameter("@nombre", nombrePerfil)
            };

            DataTable dt = _conexion.ExecuteReader(query, param);

            // Validación defensiva
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]) > 0;
            }

            return false;
        }
        #endregion
    }
}
