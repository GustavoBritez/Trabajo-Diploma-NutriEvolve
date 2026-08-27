using BE;

namespace BLL
{
    public interface IComponent
    {
        int Id { get; }
        string Nombre { get; }

        void AgregarPermiso(Permiso permiso);
        void EliminarPermiso(Permiso permiso);
        void AgregarComponente(IComponent componente);
        void EliminarComponente(IComponent componente);
        List<Permiso> ObtenerPermisos();
        void Execute();

    }
}
