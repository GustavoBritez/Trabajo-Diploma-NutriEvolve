# Guía de Instalación: NutriEvolve

Este paquete contiene todo lo necesario para instalar y poner en marcha el sistema **NutriEvolve** junto con su **Base de Datos SQL Server (`ING`)** en cualquier computadora con Windows.

---

## 1. Contenido del Paquete de Instalación

```
Instalador/
├── Setup.exe                 # Asistente Gráfico de Instalación (Recomendado)
├── Instalar.bat              # Lanzador rápido con un clic
├── Instalar_Consola.ps1      # Script de instalación desatendida / automatizada por PowerShell
├── INSTRUCCIONES_INSTALACION.md
├── Datos/
│   ├── script.sql            # Script SQL maestro (esquema DDL, datos iniciales y percentiles OMS)
│   └── ING.bak               # Respaldo físico de seguridad de la base de datos
└── App/                      # Aplicación NutriEvolve compilada y publicada
    ├── UI.exe
    ├── UI.dll
    ├── UI.dll.config
    └── ... (dependencias y recursos)
```

---

## 2. Requisitos Previos

Antes de ejecutar la instalación, asegúrese de contar con:
1. **Sistema Operativo:** Windows 10 o Windows 11 (64-bit).
2. **Motor de Base de Datos Microsoft SQL Server:**
   - Puede ser **SQL Server Standard / Developer** (instancia predeterminada `.` o `localhost`),
   - O **SQL Server Express** (instancia `.\SQLEXPRESS`),
   - O **SQL Server LocalDB** (instancia `(LocalDB)\MSSQLLocalDB`).
3. **.NET Desktop Runtime 9.0** (x64) o superior.

---

## 3. ¿Cómo resuelve el Instalador el problema de `Data Source`?

En entornos de desarrollo o en máquinas de usuarios finales, el nombre de la instancia de SQL Server varía:
- En computadoras con SQL Server completo: `.` o `localhost`.
- En computadoras con SQL Server Express: `.\SQLEXPRESS`.
- En entornos con Visual Studio: `(LocalDB)\MSSQLLocalDB`.
- En servidores de red: una dirección IP o nombre de host remoto (ej. `192.168.1.50\SQLEXPRESS`).

### Mecanismos de Detección y Configuración Implementados:
1. **Detección Automática en Tiempo Real:** Al abrir `Setup.exe`, el asistente prueba conexiones ultrarrápidas contra las instancias habituales (`.`, `.\SQLEXPRESS`, `(LocalDB)\MSSQLLocalDB`, `localhost`) y selecciona automáticamente la primera que responda.
2. **Botón "Probar Conexión":** Permite al usuario verificar antes de comenzar si el servidor indicado está accesible y listo para recibir comandos.
3. **Soporte de Autenticación:** Permite tanto **Autenticación Integrada de Windows** como **Autenticación SQL Server** (usuario `sa` y contraseña).
4. **Actualización Automática de Configuración:** Durante la instalación, se actualiza el archivo `UI.dll.config` y se genera `db_connection.txt` con la cadena de conexión exacta correspondiente a la máquina de destino.
5. **Fallback Inteligente en la Aplicación:** Si por alguna razón la máquina cambia de configuración a futuro, la clase `Conexion` en `DAL` cuenta con un resolvedor inteligente que detecta y conmuta automáticamente a la instancia de SQL Server que esté activa.

---

## 4. Comportamiento Inteligente: Instalador y Reinstalador

El programa `Setup.exe` funciona automáticamente como **Instalador** y **Reinstalador**:

1. **Primera Ejecución (Instalación Nueva):**
   - El asistente detecta que no existen archivos ni base de datos previa.
   - Crea el directorio de destino, copia los archivos de la aplicación, crea el catálogo `ING`, corre el script SQL completo con datos de prueba iniciales y sella los Dígitos Verificadores de integridad.

2. **Ejecución Posterior (Modo Reinstalador / Reparador):**
   - El asistente detecta automáticamente la instalación previa (por la existencia de `UI.exe` en la carpeta y/o la existencia del catálogo `ING` en el servidor SQL).
   - **Cierre Seguro de Procesos:** Si `NutriEvolve` se encuentra abierto, el asistente lo cierra ordenadamente en segundo plano para evitar bloqueos de archivos en disco (`file in use`).
   - **Actualización de Archivos:** Reemplaza todos los ejecutables, librerías e interfaces gráficas por las versiones más recientes.
   - **Opciones de Base de Datos para Reinstalación:**
     - **Conservar datos existentes y reparar/sellar integridad (Recomendado):** Mantiene todos los pacientes, turnos, historias clínicas y mediciones ya registrados, actualiza el esquema si faltase alguna tabla o columna, y recalibra y sella los Dígitos Verificadores (DVH y DVV) para reparar cualquier inconsistencia.
     - **Recrear base de datos desde cero (Sobrescribir completamente):** Elimina y vuelve a crear la base de datos limpia con los datos de fábrica (solicita confirmación de seguridad).

---

## 5. Pasos para Instalar o Reinstalar mediante el Asistente Gráfico (`Setup.exe`)

1. Haga doble clic en `Setup.exe` (o ejecute `Instalar.bat`).
2. **Pantalla de Bienvenida:** Muestra si se encuentra en modo Instalación o Reinstalación. Presione **Siguiente**.
3. **Carpeta de Destino:**
   - Puede mantener la ruta por defecto (`%LocalAppData%\NutriEvolve`) o seleccionar otra (por ejemplo `C:\NutriEvolve`).
   - Opcionalmente mantenga tildada la opción para crear el acceso directo en el Escritorio.
   - Presione **Siguiente**.
4. **Configuración de SQL Server y Base de Datos:**
   - Seleccione la instancia detectada o ingrese el nombre de su instancia.
   - Presione **"Probar Conexión"** para validar el estado.
   - Deje tildadas las casillas:
     - *Crear e inicializar la base de datos 'ING' con esquema y datos iniciales*.
     - *Calcular y sellar Dígitos Verificadores de integridad*.
   - Presione **Instalar Ahora** (o **Reinstalar Ahora**).
5. **Progreso de Instalación / Reinstalación:**
   - El instalador cerrará instancias previas si están abiertas, copiará los archivos actualizados, preparará o reparará el catálogo `ING`, ejecutará el script SQL según la modalidad elegida, sellará los dígitos verificadores y configurará la conexión.
6. **Finalización:**
   - Deje tildada la opción **"Iniciar NutriEvolve ahora"** y presione **Finalizar**.

---

## 6. Instalación por Consola (Desatendida / Silenciosa)

Si desea realizar una instalación rápida por PowerShell sin interfaz gráfica:

```powershell
# Abre una consola PowerShell como Administrador en la carpeta Instalador:
cd "c:\Users\Danie\Desktop\GIT\TD\Instalador"

# Instalación con auto-detección automática:
.\Instalar_Consola.ps1

# O especificando una instancia y ruta personalizada:
.\Instalar_Consola.ps1 -Server ".\SQLEXPRESS" -InstallPath "C:\NutriEvolve"
```

---

## 7. Cuentas de Acceso Predefinidas

| Rol | Usuario | Contraseña | Perfil y Permisos |
| :--- | :--- | :--- | :--- |
| **Administrador General** | `admin` | `admin123` | Control total del sistema, seguridad, bitácora, respaldos, asignación de perfiles y patentes. |
| **Nutricionista Pediátrico** | `nutri` | `nutri123` | Turnero nutricional (PN1), evaluación antropométrica OMS, Z-Scores, percentiles, alertas clínicas y planes alimentarios (PN2). |
