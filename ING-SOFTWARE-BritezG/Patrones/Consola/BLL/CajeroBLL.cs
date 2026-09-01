using BE;
using DAL;
namespace BLL
{
    public class CajeroBLL
    {
        private EmpleadoDAL bd = new EmpleadoDAL();

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
