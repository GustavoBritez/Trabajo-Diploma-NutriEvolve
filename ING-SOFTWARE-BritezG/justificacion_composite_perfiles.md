# Justificación Técnica: Gestión de Perfiles, Familias y Permisos (Patrón Composite)

Este documento detalla la respuesta arquitectónica a las observaciones sobre la estructura de perfiles, la recursividad con la interfaz gráfica (`TreeView`), el modelo de persistencia y la flexibilidad dinámica del diseño en tiempo de ejecución.

## 1. Evidencia de la recursividad en el diseño de clases (Patrón Composite)

El diseño del sistema utiliza el **Patrón Estructural Composite** para resolver el problema de la jerarquía de niveles infinitos que tienen los permisos. En lugar de tratar a los Perfiles, Familias y Permisos (Patentes) como entidades separadas y no relacionadas en la lógica de negocio, se diseñó un árbol donde un nodo puede contener otros nodos o ser una hoja terminal.

**Evidencia en Código (BLL/Services):**
El patrón se encuentra implementado en el espacio de nombres `Services.Perfiles`:
1. **Component (Clase Base):** La clase abstracta `Perfil` define el contrato común. Tiene el método `public abstract bool EsCompuesto();`.
2. **Leaf (Hoja):** La clase `PatenteServices` hereda de `Perfil`, representa un permiso individual (ej. "btnGuardar") y su método `EsCompuesto()` devuelve `false`.
3. **Composite (Compuesto):** La clase `FamiliaServices` hereda de `Perfil`. Representa un nodo que puede tener hijos. Posee una lista interna `private List<Perfil> _hijos = new();` y un método `Agregar(Perfil c)`. Su método `EsCompuesto()` devuelve `true`.

Al tratar tanto a las Familias como a las Patentes como instancias de la misma interfaz base `Perfil`, el código backend se independiza de la profundidad del árbol.

## 2. Vínculo de la estructura recursiva con la Interfaz (TreeView)

El traslado de la memoria (el objeto Composite) a la vista de usuario se realiza de forma **estrictamente recursiva** reflejando la misma jerarquía. 

En la interfaz gráfica ([Perfiles.cs](file:///c:/Users/Navegador/Desktop/Nueva%20carpeta/ING/UI/Perfiles.cs)), cuando el administrador selecciona una Familia, el sistema extrae el árbol completo de objetos desde la base de datos y llama al método `DibujarNodosFamiliasRecursivo`:

```csharp
private void DibujarNodosFamiliasRecursivo(FamiliaServices familiaPadre, TreeNode nodoVisualPadre)
{
    foreach (Perfil hijo in familiaPadre.Hijos)
    {
        if (hijo.EsCompuesto())
        {
            // 1. Es una Familia. Creamos nodo visual.
            TreeNode nodoHijo = new TreeNode(hijo.Nombre);
            
            // 2. LLAMADA RECURSIVA: Nos adentramos un nivel más de profundidad.
            FamiliaServices subFamilia = (FamiliaServices)hijo;
            DibujarNodosFamiliasRecursivo(subFamilia, nodoHijo);
            
            // 3. Agregamos el nodo ensamblado al padre.
            nodoVisualPadre.Nodes.Add(nodoHijo);
        }
        else
        {
            // Es una hoja (Patente). Condición de parada / caso base de este nodo.
            TreeNode nodoHoja = new TreeNode(hijo.Nombre);
            nodoVisualPadre.Nodes.Add(nodoHoja);
        }
    }
}
```

Esta función garantiza que **no importa cuántos niveles de anidamiento** (Familia -> SubFamilia -> SubSubFamilia -> Permiso) tenga el árbol; el `TreeView` (nodos visuales) se construirá copiando matemáticamente la misma profundidad que los objetos en RAM.

## 3. Persistencia de la Jerarquía en la Base de Datos

En el lado de la base de datos (SQL Server), este árbol dinámico no se guarda como un XML o un bloque estático, sino a través de **Tablas de Relación (Muchos a Muchos)**.

Existen tablas maestras de nodos (`Familia`, `Patente`) y tablas conectoras para representar las uniones del árbol jerárquico:
- `Familia_Patente` (Vincula una Familia con un Permiso terminal).
- `Familia_Familia` (Recursividad de base de datos: vincula una Familia Padre con una Familia Hija).
- `Perfil_Familia` / `Perfil_Patente` (Vincula el grupo de acceso más alto con las ramas correspondientes).

**¿Cómo funciona la lectura (Rehidratación)?**
El módulo de Data Access Layer (DAL) ejecuta llamadas recursivas o consultas estructuradas (CTEs en SQL) que extraen las relaciones y van instanciando en memoria los objetos `FamiliaServices`, haciéndoles `Agregar(hijo)` hasta reconstruir todo el árbol de permisos asignados a ese perfil.

## 4. Flexibilidad: ¿Permite el diseño cambios dinámicos sin recarga completa?

**Sí, el diseño base lo permite nativamente.** 
A nivel arquitectónico (el código de Backend/BLL), el patrón Composite permite agarrar un objeto `FamiliaServices` instanciado y hacerle:
`familia.Agregar(nuevoPermiso);` 
Esto agrega una rama al árbol en memoria RAM (tiempo de ejecución) sin necesidad de consultar la base de datos. 

Si bien en la Capa de Presentación (UI) actual del proyecto a veces se invoca `CargarGrillas()` o se refresca el árbol por completo para sincronizar cambios con la grilla o por simplicidad en WinForms, **esto es netamente una decisión de interfaz gráfica**. 

Para responder de forma concreta a la evaluación: **El modelo de clases es completamente dinámico.** Podríamos interceptar el evento "Agregar Permiso a Familia", y en la interfaz (WinForms), invocar:
`treeView.SelectedNode.Nodes.Add(new TreeNode(nombreNuevoPermiso));`
Y paralelamente enviar el comando de persistencia a la Base de Datos (`_patenteBLL.Vincular...`). Esto mutaría el árbol visual en el momento sin generar una latencia por recarga de la base de datos, demostrando la escalabilidad en tiempo de ejecución de la arquitectura adoptada.
