using BE;
using Service;
using BLL;
using Consola;

internal class Program
{
    public static void Login()
    {
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Ingrese nombre ");
        string nombre = Console.ReadLine();
        Console.WriteLine("Ingrese apellido "); 
        string apellido = Console.ReadLine();

        bool loginExisoto = Singleton.Instance.ValidarLogin(nombre, apellido);

        if (loginExisoto)
        {
            EmpleadoBE empleadoActual = Singleton.Instance.EmpleadoConectado;
            Console.WriteLine($"Login exitoso. Bienvenido {empleadoActual.nombre}");

        }
        else
        {
            Console.WriteLine("Login fallido. Usuario no encontrado.");
        }
        Console.WriteLine();
        Console.WriteLine();
    }
    private static void Main(string[] args)
    {

        while (true)
        {
            Console.WriteLine("Ingrese el tipo de empleado ( Cajero = 0, JefeArea = 1, Gerente = 2, Operativo = 3)");
            Console.WriteLine("Ingrese 4 para Finalizar");
            bool esNumero = int.TryParse(Console.ReadLine(), out int emp);

            if (!esNumero)
            {
                Console.WriteLine("Ingrese un numero valido");
                continue; //Volvemos la inicio del while
            }
            if (emp == 4)
            {
                Console.WriteLine(" Finalizo la creacion de usuarios");
                Login();
                continue;
            }

            Console.WriteLine("Ingrese nombre ");
            string nombre = Console.ReadLine();
            Console.WriteLine("Ingrese apellido ");
            string apellido = Console.ReadLine();

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
            {
                Console.WriteLine("El nombre y apellido no pueden estar vacíos. Intente nuevamente.");
                continue;
            }
            TipoEmpleado tipoEmpleado = (TipoEmpleado)emp;
            EmpleadoBE empleado = FactoryMethod.CrearEmpleado(tipoEmpleado,nombre,apellido);

            switch(empleado)
            {
                case CajeroBE cajero:
                    Console.WriteLine($"Empleado creado: {cajero.nombre} {cajero.apellido}, Tipo: Cajero");
                    CajeroBLL cajeroBLL = new CajeroBLL();
                    cajeroBLL.Guardar(cajero);
                    break;
                case JefeAreaBE jefeArea:
                    Console.WriteLine($"Empleado creado: {jefeArea.nombre} {jefeArea.apellido}, Tipo: Jefe de Area");
                    JefeAreaBLL jefe = new();
                    jefe.Guardar(jefeArea);
                    break;
                case GerenteBE gerente:
                    Console.WriteLine($"Empleado creado: {gerente.nombre} {gerente.apellido}, Tipo: Gerente");
                    GerenteBLL gerenteBll = new();
                    gerenteBll.Guardar(gerente);
                    break;
                case OperativoBE operativo:
                    Console.WriteLine($"Empleado creado: {operativo.nombre} {operativo.apellido}, Tipo: Operativo");
                    OperativoBLL ope = new();
                    ope.Guardar(operativo);

                    break;
            }
            Console.WriteLine("----");
            Console.WriteLine("---");
            List<EmpleadoBE> empleados = FactoryMethod.ObtenerTodos();
            foreach (var item in empleados)
            {
                Console.WriteLine($"Empleado: {item.nombre} {item.apellido}, Tipo: {item.GetType().Name}");
            }
            Console.WriteLine("---");
            Console.WriteLine("---");
        }
    }
}