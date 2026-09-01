using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;

namespace Consola
{
    public sealed class Singleton // sealed evita herencia
    {
        // Instancia única y segura
        private static readonly Lazy<Singleton> _instance = new(() => new Singleton());
        public static Singleton Instance => _instance.Value;

        // Lock para seguridad en hilos
        private readonly object _lock = new object();

        // Lista thread-safe
        private readonly List<EmpleadoBE> _listaEmpleado;
        private EmpleadoBE _empleadoConectado;

        // Propiedad con acceso sincronizado
        public EmpleadoBE EmpleadoConectado
        {
            get { lock (_lock) return _empleadoConectado; }
            set { lock (_lock) _empleadoConectado = value; }
        }

        private Singleton()
        {
            _listaEmpleado = EmpleadoDAL.ObtenerTodos(); // Cargar desde DAL
            _empleadoConectado = null;
        }

        // Guardar de forma thread-safe
        public void Guardar(EmpleadoBE emp)
        {
            if (emp == null) return;

            lock (_lock)
            {
                _listaEmpleado.Add(emp);
            }
        }

        // Obtener copia de la lista (no la original)
        public List<EmpleadoBE> ObtenerEmpleados()
        {
            lock (_lock)
            {
                return new List<EmpleadoBE>(_listaEmpleado);
            }
        }

        // Validar login de forma atómica
        public bool ValidarLogin(string nombre, string apellido)
        {
            lock (_lock)
            {
                // 1. Verificar si ya hay alguien conectado
                if (_empleadoConectado != null)
                {
                    if (_empleadoConectado.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) &&
                        _empleadoConectado.apellido.Equals(apellido, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"✗ El usuario {nombre} {apellido} YA está conectado en el sistema.");
                        return false;
                    }

                    Console.WriteLine($"✗ Ya hay otro usuario conectado ({_empleadoConectado.nombre} {_empleadoConectado.apellido}). Debe desconectarse primero.");
                    return false;
                }

                // 2. Buscar el usuario en la lista
                EmpleadoBE emp = _listaEmpleado.FirstOrDefault(e =>
                    e.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) &&
                    e.apellido.Equals(apellido, StringComparison.OrdinalIgnoreCase));

                if (emp is null)
                {
                    Console.WriteLine($"✗ No existe usuario con nombre '{nombre}' y apellido '{apellido}'.");
                    return false;
                }

                // 3. Conectar al usuario
                _empleadoConectado = emp;
                Console.WriteLine($"✓ ¡Ingreso correcto! Bienvenido {emp.nombre} {emp.apellido}. Rol: {emp.GetType().Name}");
                return true;
            }
        }
    }
}
