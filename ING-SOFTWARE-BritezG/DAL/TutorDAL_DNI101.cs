using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class TutorDAL_DNI101
    {
        private readonly Conexion _conexion = new();

        public TutorDAL_DNI101()
        {
            TurnosDatabaseInitializer.AsegurarTablas();
        }

        public int Guardar(TutorBE_DNI101 tutor)
        {
            return 0;
        }

        public TutorBE_DNI101? ObtenerPorDNI(string dniTutor)
        {
            return null;
        }

        public List<TutorBE_DNI101> ListarTodos()
        {
            return new List<TutorBE_DNI101>();
        }
    }
}
