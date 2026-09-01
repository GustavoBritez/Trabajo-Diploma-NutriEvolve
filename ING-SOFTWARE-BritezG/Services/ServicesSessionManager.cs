using BE;
using Services.Perfiles;

namespace Services
{
    public class ServicesSessionManager : IIdiomaObservable
    {
        private static ServicesSessionManager _instancia;
        private static readonly object _lock = new object();

        private UsuarioBE usuarioActivo;

        private Idioma idiomaActual;
        private List<IIdiomaObserver> observadores;
        //Atributo nuevo
        private List<string> permisosDelUsuarioActivo = new List<string>();
        private List<PatenteServices> _permisosUsuario;
        public bool BaseDatosCorruptaDetectada { get; private set; }

        private ServicesSessionManager()
        {
            observadores = new List<IIdiomaObserver>();
        }

        public static ServicesSessionManager Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (_lock)
                    {
                        if (_instancia == null)
                        {
                            _instancia = new ServicesSessionManager();
                        }
                    }
                }
                return _instancia;
            }
        }
        // Metodo nuevo
        public void CargarPermisosDelUsuario(List<string> listaPermisos)
        {
            permisosDelUsuarioActivo = listaPermisos;
        }

        public bool EsAdministrador()
        {
            return usuarioActivo != null && usuarioActivo._IdPerfil == 1;
        }

        public void RegistrarEstadoIntegridad(bool baseDatosCorrupta)
        {
            BaseDatosCorruptaDetectada = baseDatosCorrupta;
        }
        // Metodo nuevo
        public bool TienePermiso(string nombrePermiso)
        {
            if (string.IsNullOrEmpty(nombrePermiso)) return true;

            return permisosDelUsuarioActivo.Contains(nombrePermiso);
        }
        public UsuarioBE ObtenerUsuarioActivo()
        {
            return usuarioActivo;
        }

        public int ObtenerDniUsuarioActual()
        {
            if (usuarioActivo != null)
            {
                return usuarioActivo._Dni;
            }
            return 0; // Retorna 0 si no hay usuario activo
        }

        public bool Login(UsuarioBE newUsuario)
        {
            try
            {
                if (newUsuario is not null)
                {
                    usuarioActivo = newUsuario;
                    return true;
                }
                else
                {
                    usuarioActivo = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                usuarioActivo = null;
                return false;
            }
        }

        public void Logout()
        {
            usuarioActivo = null;
            BaseDatosCorruptaDetectada = false;
        }

        public void CambiarIdioma(Idioma idioma)
        {
            idiomaActual = idioma;
            Notificar();
        }
        public void Suscribir(IIdiomaObserver observer)
        {
            if (!observadores.Contains(observer))
                observadores.Add(observer);
        }

        public void Desuscribir(IIdiomaObserver observer)
        {
            if (observadores.Contains(observer))
                observadores.Remove(observer);
        }

        public void Notificar()
        {
            foreach (var observer in observadores)
            {
                observer.ActualizarIdioma();
            }
        }
        public Idioma ObtenerIdioma() => this.idiomaActual;
    }
}
