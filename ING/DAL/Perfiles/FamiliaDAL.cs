using Microsoft.Data.SqlClient;
using Services.Perfiles;
using System.Data;


namespace DAL.Perfiles
{
    public class FamiliaDAL
    {
        private readonly Conexion _conexion = new();

        public FamiliaServices ObtenerArbolFamiliar(int idFamiliaRaiz)
        {
            string queryPadre = "SELECT Nombre FROM Familia WHERE ID_Familia = @id";
            SqlParameter[] paramPadre = new SqlParameter[] { new SqlParameter("@id", idFamiliaRaiz) };

            DataTable dtPadre = _conexion.ExecuteReader(queryPadre, paramPadre);
            if (dtPadre.Rows.Count == 0) return null;

            string nombreFamilia = dtPadre.Rows[0]["Nombre"].ToString();
            FamiliaServices familiaArmada = new FamiliaServices(nombreFamilia) { Id = idFamiliaRaiz };

            string queryFamHijas = @"
        SELECT f.ID_Familia, f.Nombre 
        FROM Familia_Familia ff
        INNER JOIN Familia f ON ff.ID_FamiliaHija = f.ID_Familia
        WHERE ff.ID_FamiliaPadre = @idPadre";

            SqlParameter[] paramFam = new SqlParameter[] { new SqlParameter("@idPadre", idFamiliaRaiz) };
            DataTable dtFamHijas = _conexion.ExecuteReader(queryFamHijas, paramFam);

            foreach (DataRow fila in dtFamHijas.Rows)
            {
                int idHijo = Convert.ToInt32(fila["ID_Familia"]);

                FamiliaServices subFamilia = ObtenerArbolFamiliar(idHijo);

                if (subFamilia is not null)
                    familiaArmada.Agregar(subFamilia);
            }

            string queryPermisos = @"
        SELECT p.ID_Permiso, p.Nombre 
        FROM Permiso_Familia pf
        INNER JOIN Permiso p ON pf.ID_Permiso = p.ID_Permiso
        WHERE pf.ID_Familia = @idPadre";

            SqlParameter[] paramPerm = new SqlParameter[] { new SqlParameter("@idPadre", idFamiliaRaiz) };
            DataTable dtPermisos = _conexion.ExecuteReader(queryPermisos, paramPerm);

            foreach (DataRow fila in dtPermisos.Rows)
            {
                int idHijo = Convert.ToInt32(fila["ID_Permiso"]);
                string nombreHijo = fila["Nombre"].ToString();

                PatenteServices permisoHoja = new PatenteServices(nombreHijo) { Id = idHijo };
                familiaArmada.Agregar(permisoHoja);
            }

            return familiaArmada;
        }

        public int InsertarFamiliaNueva(string nombreFamilia)
        {
            string query = @"
                INSERT INTO Familia (Nombre)
                OUTPUT INSERTED.ID_Familia
                VALUES (@nombre)";
            SqlParameter[] parametros = new SqlParameter[]
            {
                 new SqlParameter("@nombre", nombreFamilia)
            };

            DataTable dt = _conexion.ExecuteReader(query, parametros);

            if (dt == null || dt.Rows.Count == 0)
            {
                throw new Exception("No se pudo obtener el ID de la nueva familia.");
            }

            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public List<string> ObtenerPerfilesDeFamilia(int idFamilia)
        {
            List<string> nombresPerfiles = new List<string>();

            string query = @"
                SELECT p.Nombre 
                FROM Familia_Perfil fp
                INNER JOIN Perfil p ON fp.ID_Perfil = p.ID_Perfil
                WHERE fp.ID_Familia = @idFamilia";

            SqlParameter[] parametros = {
                     new SqlParameter("@idFamilia", idFamilia)
            };

            DataTable dt = _conexion.ExecuteReader(query, parametros);

            foreach (DataRow fila in dt.Rows)
            {
                nombresPerfiles.Add(fila["Nombre"].ToString());
            }

            return nombresPerfiles;
        }

        public bool ExisteRelacionPermisoFamilia(int idFamilia, int idPermiso)
        {
            string query = "SELECT COUNT(1) FROM Permiso_Familia WHERE ID_Familia = @idFam AND ID_Permiso = @idPerm";
            SqlParameter[] param = {
                new SqlParameter("@idFam", idFamilia),
                new SqlParameter("@idPerm", idPermiso)
            };
            DataTable dt = _conexion.ExecuteReader(query, param);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool ExisteFamiliaPorNombre(string nombreFamilia)
        {
            string query = "SELECT COUNT(1) FROM Familia WHERE Nombre = @nombre";

            SqlParameter[] param = {
                new SqlParameter("@nombre", nombreFamilia)
            };

            DataTable dt = _conexion.ExecuteReader(query, param);

            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]) > 0;
            }

            return false;
        }

        public bool ExisteRelacionFamiliaFamilia(int idFamiliaPadre, int idFamiliaHija)
        {
            string query = "SELECT COUNT(1) FROM Familia_Familia WHERE ID_FamiliaPadre = @idPadre AND ID_FamiliaHija = @idHija";

            Microsoft.Data.SqlClient.SqlParameter[] param = {
        new Microsoft.Data.SqlClient.SqlParameter("@idPadre", idFamiliaPadre),
        new Microsoft.Data.SqlClient.SqlParameter("@idHija", idFamiliaHija)
    };

            System.Data.DataTable dt = _conexion.ExecuteReader(query, param);

            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            return false;
        }

        public bool ExisteRelacionFamiliaPerfil(int idPerfil, int idFamilia)
        {
            string query = "SELECT COUNT(1) FROM Familia_Perfil WHERE ID_Perfil = @idPerf AND ID_Familia = @idFam";
            Microsoft.Data.SqlClient.SqlParameter[] param = {
        new Microsoft.Data.SqlClient.SqlParameter("@idPerf", idPerfil),
        new Microsoft.Data.SqlClient.SqlParameter("@idFam", idFamilia)
    };
            System.Data.DataTable dt = _conexion.ExecuteReader(query, param);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public void InsertarPermisoFamilia(int idFamilia, int idPermiso)
        {
            string query = "INSERT INTO Permiso_Familia (ID_Familia, ID_Permiso) VALUES (@idFam, @idPerm)";
            SqlParameter[] param = {
                new SqlParameter("@idFam", idFamilia),
                new SqlParameter("@idPerm", idPermiso)
            };
            _conexion.ExecuteNonQuery(query, param);
        }

        public void EliminarPermisoFamilia(int idFamilia, int idPermiso)
        {
            // Borramos de la tabla puente específica de las Familias
            string query = "DELETE FROM Permiso_Familia WHERE ID_Familia = @idFam AND ID_Permiso = @idPerm";

            SqlParameter[] param = {
                new SqlParameter("@idFam", idFamilia),
                new SqlParameter("@idPerm", idPermiso)
            };

            _conexion.ExecuteNonQuery(query, param);
        }

        public void EliminarFamilia(int idFamilia)
        {
            string queryPerfiles = "DELETE FROM Familia_Perfil WHERE ID_Familia = @id";
            SqlParameter[] paramPerfiles = { new SqlParameter("@id", idFamilia) };
            _conexion.ExecuteNonQuery(queryPerfiles, paramPerfiles);

            string queryPermisos = "DELETE FROM Permiso_Familia WHERE ID_Familia = @id";
            SqlParameter[] paramPermisos = { new SqlParameter("@id", idFamilia) };
            _conexion.ExecuteNonQuery(queryPermisos, paramPermisos);

            string queryFamilia = "DELETE FROM Familia WHERE ID_Familia = @id";
            SqlParameter[] paramFamilia = { new SqlParameter("@id", idFamilia) };
            _conexion.ExecuteNonQuery(queryFamilia, paramFamilia);
        }

        public void EliminarFamiliaDeFamilia(int idFamiliaPadre, int idFamiliaHija)
        {
            // Borramos la relación donde el padre contiene a la hija
            string query = "DELETE FROM Familia_Familia WHERE ID_FamiliaPadre = @idPadre AND ID_FamiliaHija = @idHija";

            Microsoft.Data.SqlClient.SqlParameter[] param = {
        new Microsoft.Data.SqlClient.SqlParameter("@idPadre", idFamiliaPadre),
        new Microsoft.Data.SqlClient.SqlParameter("@idHija", idFamiliaHija)
    };

            _conexion.ExecuteNonQuery(query, param);
        }

        public void InsertarFamiliaAFamilia(int idFamiliaPadre, int idFamiliaHija)
        {
            string query = "INSERT INTO Familia_Familia (ID_FamiliaPadre, ID_FamiliaHija) VALUES (@idPadre, @idHija)";

            SqlParameter[] param = {
        new SqlParameter("@idPadre", idFamiliaPadre),
        new SqlParameter("@idHija", idFamiliaHija)
    };

            _conexion.ExecuteNonQuery(query, param);
        }

        public List<FamiliaServices> ObtenerTodasLasFamilias()
        {
            List<FamiliaServices> listaFamilias = new List<FamiliaServices>();

            string query = "SELECT ID_Familia, Nombre FROM Familia";

            DataTable dt = _conexion.ExecuteReader(query, null);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow fila in dt.Rows)
                {
                    FamiliaServices familia = new();

                    familia.Id = Convert.ToInt32(fila["ID_Familia"]);
                    familia.Nombre = fila["Nombre"].ToString();

                    listaFamilias.Add(familia);
                }
            }

            return listaFamilias;
        }
    }
}
