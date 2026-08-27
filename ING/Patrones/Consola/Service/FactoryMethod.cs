using BE;
using DAL;
using System.Runtime.CompilerServices;
namespace Service
{
    public enum TipoEmpleado
    {
        Cajero,
        JefeArea,
        Gerente,
        Operativo
    }
    public class FactoryMethod
    {

        public static EmpleadoBE CrearEmpleado(TipoEmpleado tipo, string nombre, string apellido)
        {
            return tipo switch 
            {

                TipoEmpleado.Gerente => new GerenteBE(nombre, apellido),
                TipoEmpleado.JefeArea => new JefeAreaBE(nombre,apellido),
                TipoEmpleado.Operativo => new OperativoBE(nombre, apellido),
                TipoEmpleado.Cajero => new CajeroBE(nombre, apellido),
                _ => throw new ArgumentException($"Tipo de empleado no valido {tipo}")

            };
        }

        public static List<EmpleadoBE>ObtenerTodos()
        {
            return EmpleadoDAL.ObtenerTodos();
        }
    }
}
