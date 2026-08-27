// ¡Ya NO hay using BLL! La UI no sabe qué es un Composite o IComponent
using BE;
using Service;
using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== COMPOSITE PATTERN - DESDE LA UI LIMPIA ===\n");

        // 1. Solo manipulas tus entidades de negocio (BE)
        var p1 = new Permiso(1, "Leer");
        var p2 = new Permiso(2, "Escribir");
        var p3 = new Permiso(3, "Eliminar");
        var p4 = new Permiso(4, "Compartir");
        var p5 = new Permiso(5, "Administrar");

        var usuarioB = new Usuario(2, "B");
        usuarioB.Permisos.Add(p1);
        usuarioB.Permisos.Add(p2);

        var rol = new Rol(10, "Rol_A");

        // 2. Te comunicas con el Service
        var service = new Service.Service();

        // 3. Le pasas tus "clases tontas" (BE) al servicio. Él gestiona el Patrón adentro.
        service.IniciarRol(rol);
        service.AsignarUsuarioRolActivo(usuarioB);
        service.AgregarPermisoARolActivo(p5);

        // 4. El servicio te devuelve el resultado final
        service.EjecutarRolActivo();

        var permisosFinales = service.ObtenerPermisosDelRolActivo();
        Console.WriteLine($"\nPermisos finales: [{string.Join(", ", permisosFinales.Select(p => p.Id))}]");
    }
}