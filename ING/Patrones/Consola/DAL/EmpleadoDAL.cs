using BE;

namespace DAL
{
    public class EmpleadoDAL
    {
        // Esta es LA ÚNICA lista. Al ser estática, todos comparten la misma.
        private static List<EmpleadoBE> _listaEmpleados = new();

        public void Guardar(EmpleadoBE empleado)
        {
            _listaEmpleados.Add(empleado);
        }

        // Método para recuperar todos
        public static List<EmpleadoBE> ObtenerTodos()
        {
            return _listaEmpleados;
        }
    }
}   