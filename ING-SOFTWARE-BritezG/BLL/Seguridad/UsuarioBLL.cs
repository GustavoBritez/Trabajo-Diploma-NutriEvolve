using BE;
using DAL;
using Microsoft.Data.SqlClient;
using Services;
using System.Net;

namespace BLL
{
    public class UsuarioBLL
    {
        private UsuarioDAL usuarioDAL;
        private ServicioBcrypt Bcryp;
        private readonly DigitoVerificadorBLL digitoVerificadorBLL = new();
        // Diccionario estático para guardar intentos fallidos en memoria
        public Dictionary<string, int> intentosFallidos = new Dictionary<string, int>();

        public UsuarioBLL()
        {
            usuarioDAL = new UsuarioDAL();
            Bcryp = new ServicioBcrypt();
        }

        public void CambiarEstado(UsuarioBE usuario)
        {
            try
            {
                usuario._Estado = !usuario._Estado;

                //
                //==========================
                //
                usuario.DV = ServicioBcrypt.CalcularDV(GenerarCadenaParaDV(usuario));
                //
                //==========================
                //
                usuarioDAL.CambioEstado(usuario);

                digitoVerificadorBLL.RecalcularYPersistir();


                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Cambio de Estado";
                bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "GestionUsuario");
            }
            catch (Exception ex)
            {
                string descripcion = $"ERROR: Cambio de Estado";
                new EventoBLL().RegistrarEvento(4, descripcion, ServicesSessionManager.Instancia.ObtenerDniUsuarioActual(), "GestionUsuario");
                throw new Exception($"Error en CambiarEstado: {ex.Message}");
            }
        }

        public void CambiarContraseña(UsuarioBE usuario)
        {
            try
            {
              
                usuario.DV = ServicioBcrypt.CalcularDV(GenerarCadenaParaDV(usuario));
              
                usuarioDAL.CambiarContraseña(usuario);

                digitoVerificadorBLL.RecalcularYPersistir();


                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Cambio de Clave";
                bitacoraBLL.RegistrarEvento(4, descripcion, dniActual, "GestionUsuario");
            }
            catch (Exception ex)
            {
                string descripcion = $"ERROR: Cambio de Clave";
                new EventoBLL().RegistrarEvento(4, descripcion, ServicesSessionManager.Instancia.ObtenerDniUsuarioActual(), "GestionUsuario");
                throw new Exception($"Error en Cambiar contraseña: {ex.Message}");
            }
        }

        public void CrearUsuario(UsuarioBE usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario._NombreDeUsuario) || string.IsNullOrWhiteSpace(usuario._Contraseña))
                {
                    throw new ArgumentException("El usuario o contraseña no pueden estar vacíos.");
                }

                List<UsuarioBE> todosLosUsuarios = usuarioDAL.ListaUsuarios() ?? new List<UsuarioBE>();

                if (todosLosUsuarios.Any(u => string.Equals(u._NombreDeUsuario, usuario._NombreDeUsuario, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"El nombre de usuario '{usuario._NombreDeUsuario}' ya existe.");
                }
                if (todosLosUsuarios.Any(u => u._Dni == usuario._Dni))
                {
                    throw new InvalidOperationException($"El DNI '{usuario._Dni}' ya está registrado con otro usuario.");
                }

                usuario._Contraseña = Bcryp.HashearContraseña(usuario._Contraseña);
                //
                //==========================
                //
                usuario.DV = ServicioBcrypt.CalcularDV(GenerarCadenaParaDV(usuario));
                //
                //==========================
                //
                usuarioDAL.CrearUsuario(usuario);

                digitoVerificadorBLL.RecalcularYPersistir();


                int dniActual;
                try
                {
                    int dniSesion = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    dniActual = dniSesion <= 0 ? 12345678 : dniSesion;
                }
                catch
                {
                    dniActual = 12345678;
                }
                string descripcion = $"Creacion de Usuario";
                new EventoBLL().RegistrarEvento(1, descripcion, dniActual, "GestionUsuario");
            }
            catch (Exception ex)
            {
                int dniActual;
                try
                {
                    int dniSesion = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    dniActual = dniSesion <= 0 ? 12345678 : dniSesion;
                }
                catch
                {
                    dniActual = 12345678;
                }
                string descripcion = $"ERROR: Creación de Usuario";
                new EventoBLL().RegistrarEvento(4, descripcion, dniActual, "GestionUsuario");
                Console.WriteLine($"Error al CrearUsuario: {ex.Message}");
                throw;
            }
        }

        public List<UsuarioBE> ListarUsuarios()
        {
            try
            {
                return usuarioDAL.ListaUsuarios();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarUsuarios: {ex.Message}");
                return new List<UsuarioBE>();
            }
        }

        public bool Login(string nombreDeUsuario, string contraseñaPlana)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreDeUsuario) || string.IsNullOrWhiteSpace(contraseñaPlana))
                {
                    return false;
                }

                string nombreNormalizado = nombreDeUsuario.ToLower();
                UsuarioBE usuarioEnBD = usuarioDAL.ObtenerUsuario(nombreNormalizado);

                if (usuarioEnBD == null)
                {
                    Console.WriteLine($"Error: Usuario '{nombreDeUsuario}' no existe");
                    return false;
                }
               

                /*if (!VerificarIntegridad(usuarioEnBD))

                {
                    Console.WriteLine($"ALERTA: Integridad de datos corrompida para el usuario '{nombreDeUsuario}'.");

                    new EventoBLL().RegistrarEvento(4, "ERROR: DV", 12345678, "Seguridad");

                    return false;

                }*/

                if (usuarioEnBD._Bloqueado)
                {
                    Console.WriteLine($"Error: Usuario '{nombreDeUsuario}' está bloqueado.");
                    return false;
                }

                bool contraseñaValida = Bcryp.ValidarContraseña(contraseñaPlana, usuarioEnBD._Contraseña);

                if (contraseñaValida)
                {
                    if (intentosFallidos.ContainsKey(nombreNormalizado))
                    {
                        intentosFallidos[nombreNormalizado] = 0;
                    }
                    Console.WriteLine($"Login exitoso para usuario '{nombreDeUsuario}'.");

                    ServicesSessionManager.Instancia.Login(usuarioEnBD);
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();

                    EventoBLL bitacoraBLL = new();
                    string descripcion = $"Inicio de Sesion";
                    bitacoraBLL.RegistrarEvento(4, descripcion, dniActual, "Login");
                    return true;
                }
                else
                {
                    if (!intentosFallidos.ContainsKey(nombreNormalizado))
                    {
                        intentosFallidos[nombreNormalizado] = 0;
                    }

                    intentosFallidos[nombreNormalizado]++;
                    int intentosActuales = intentosFallidos[nombreNormalizado];

                    Console.WriteLine($"Error: Contraseña incorrecta para usuario '{nombreDeUsuario}'. Intentos: {intentosActuales}/3");

                    if (intentosActuales >= 3)
                    {
                        usuarioEnBD._Bloqueado = true;

                        ModificarUsuario(usuarioEnBD);
                        Console.WriteLine($"Cuenta de usuario '{nombreDeUsuario}' bloqueada por 3 intentos fallidos.");

                        EventoBLL bitacoraBLL = new();
                        int dniActual = this.BuscarUsuario(nombreDeUsuario)._Dni;
                        string descripcion = $"Bloqueo de Cuenta";
                        bitacoraBLL.RegistrarEvento(1, descripcion, dniActual, "Login");
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Login: {ex.Message}");
                return false;
            }
        }

        public int ObtenerIntentosFallidos(string nombreDeUsuario)
        {
            string nombreNormalizado = nombreDeUsuario.ToLower();
            if (intentosFallidos.ContainsKey(nombreNormalizado))
            {
                return intentosFallidos[nombreNormalizado];
            }
            return 0;
        }

        public void LogOut(UsuarioBE usuario)
        {
            try
            {
                if (usuario != null)
                {
                    Console.WriteLine($"Usuario '{usuario._NombreDeUsuario}' (DNI: {usuario._Dni}) ha cerrado sesión.");
                }

                intentosFallidos.Clear();

                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Cierre de Sesion";
                bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en LogOut: {ex.Message}");
                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $" Error: Cierre de Sesion";
                bitacoraBLL.RegistrarEvento(2, descripcion, dniActual, "Login");
                throw;
            }
            finally
            {
                Services.ServicesSessionManager.Instancia.Logout();
            }
        }

        public void ModificarUsuario(UsuarioBE usuario)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(usuario._Contraseña) && !usuario._Contraseña.StartsWith("$2"))
                {
                    usuario._Contraseña = Bcryp.HashearContraseña(usuario._Contraseña);
                }

                // ⚠️ NUEVO (DV): Recalculamos el DV porque sus datos cambiaron
                //
                //==========================
                //
                usuario.DV = ServicioBcrypt.CalcularDV(GenerarCadenaParaDV(usuario));
                //
                //==========================
                //
                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Modificar Usuario";

                bitacoraBLL.RegistrarEvento(2, descripcion, dniActual, "GestionUsuario");

                usuarioDAL.ModificarUsuario(usuario);
                digitoVerificadorBLL.RecalcularYPersistir();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ModificarUsuario: {ex.Message}");
                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Error: Modificar Usuario";
                bitacoraBLL.RegistrarEvento(2, descripcion, dniActual, "GestionUsuario");
                throw;
            }
        }

        public UsuarioBE BuscarUsuario(string nombreDeUsuario)
        {
            try
            {
                return usuarioDAL.ObtenerUsuario(Convert.ToString(nombreDeUsuario));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerUsuario: {ex.Message}");
                return null;
            }
        }

        public void Desbloquear(UsuarioBE user)
        {
            try
            {
                user.DV = ServicioBcrypt.CalcularDV(GenerarCadenaParaDV(user));


                usuarioDAL.Desbloquear(user);
                digitoVerificadorBLL.RecalcularYPersistir();
                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"Desbloqueo de Usuario";
                bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "GestionUsuario");
            }
            catch (Exception ex)
            {
                EventoBLL bitacoraBLL = new();
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                string descripcion = $"ERROR: Desbloqueo de Usuario";
                bitacoraBLL.RegistrarEvento(1, descripcion, dniActual, "GestionUsuario");
            }
        }

        public void CambioDeIdiomaUser(UsuarioBE user)
        {
            usuarioDAL.CambiarIdiomaUsuario(user);
            digitoVerificadorBLL.RecalcularYPersistir();
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Cambio de Idioma";
            bitacoraBLL.RegistrarEvento(1, descripcion, dniActual, "GestionUsuario");
        }

        // metodos nuevos agregar en los diagramas
        // =========================================================================

        private string GenerarCadenaParaDV(UsuarioBE usuario)
        {


            return $"{usuario._Dni}{usuario._Nombre}{usuario._Apellido}{usuario._NombreDeUsuario}{usuario._Contraseña}{usuario._IdPerfil}{usuario._Bloqueado}{usuario._Estado}{usuario._Idioma}";

        }

        
    }
}