using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BLL
{
    public class EmpleadoBLL
    {
        EmpleadoDAL bd = new EmpleadoDAL();

        public void Guardar(EmpleadoBE empleado)
        {
            bd.Guardar(empleado);
        }

        public List<EmpleadoBE> ObtenerEmpleados()
        {
            return EmpleadoDAL.ObtenerTodos();
        }
    }
}
