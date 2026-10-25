# Generador Universal de Instaladores y Reinstaladores .NET

Esta herramienta fue diseñada para que cualquier alumno o desarrollador de proyectos en **.NET (C#) con arquitectura en capas (DAL, BE, BLL, GUI/Servicios)** y **SQL Server** pueda generar automáticamente un **Instalador y Reinstalador Inteligente**, sin tener que lidiar con configuraciones complejas de InnoSetup o del instalador de Visual Studio.

---

## 1. ¿Qué problemas resuelve frente a InnoSetup y Visual Studio Installer?

| Desafío en Proyectos Universitarios | InnoSetup / Visual Studio Installer | Generador Universal .NET |
| :--- | :--- | :--- |
| **Instancia SQL Server (`Data Source`)** | Falla si el usuario tiene `.\SQLEXPRESS`, `localhost` o `(LocalDB)` en vez de `.`. | **Prueba ultrarrápida automática** de todas las instancias (`.`, `.\SQLEXPRESS`, `(LocalDB)\MSSQLLocalDB`, `localhost`) y botón "Probar Conexión". |
| **Instalación de Base de Datos** | Requiere programar scripts Pascal/VBScript difíciles de mantener. | **Ejecutor SQL nativo** que crea el catálogo, divide bloques `GO` y carga el `script.sql` completo. |
| **Modo Reinstalador Inteligente** | Sobrescribe a ciegas o arroja error si la app está abierta (`File in use`). | **Cierra procesos activos** ordenadamente y pregunta si desea conservar los datos o resetear de fábrica. |
| **Actualización de Connection Strings** | Difícil de automatizar en archivos `.config` y `appsettings.json`. | **Actualiza automáticamente** todas las cadenas de conexión y genera `db_connection.txt`. |
| **Dígitos Verificadores (DV)** | No los reconoce ni calcula. | Detecta `DigitoVerificadorBLL` y **recalcula/sella la integridad** automáticamente. |

---

## 2. ¿Cómo funciona el flujo de trabajo?

```
                                  [Alumno / Compañero]
                                           │
                               Entrega archivo .ZIP
                        (o selecciona carpeta del proyecto)
                                           │
                                           ▼
                            ┌─────────────────────────────┐
                            │ GeneradorInstalador.exe     │
                            └──────────────┬──────────────┘
                                           │
                    ┌──────────────────────┴──────────────────────┐
                    ▼                                             ▼
       [Diagnóstico Automático]                        [Compilación y Ensamblado]
  • Detecta solución y proyectos .csproj          • Ejecuta 'dotnet publish' (Release)
  • Identifica proyecto GUI ejecutable            • Copia plantilla de Setup.exe inteligente
  • Localiza el script SQL único                  • Inyecta manifest.json personalizado
  • Extrae nombre de base de datos                • Genera Instalar.bat y consola PowerShell
  • Comprueba módulos DAL, BE, BLL, DV            • Comprime en paquete final [Proyecto]_Instalador.zip
                                           │
                                           ▼
                      ┌─────────────────────────────────────────┐
                      │  Paquete Listo para Entregar y Evaluar  │
                      └─────────────────────────────────────────┘
```

---

## 3. Estructura que debe tener el `.ZIP` del Alumno

El alumno solo necesita comprimir su solución tal como la tiene en su máquina. Por ejemplo:

```
MiProyecto.zip
├── MiProyecto.sln
├── BE/               # Entidades de negocio
├── DAL/              # Capa de acceso a datos
├── BLL/              # Lógica de negocio (con o sin Dígito Verificador)
├── Servicios/        # Opcional (idiomas, bitácora, seguridad)
├── UI/               # Interfaz gráfica (o GUI, Presentacion, Vistas)
│   ├── UI.csproj     # (Cualquier proyecto ejecutable WinExe)
│   └── ...
└── script.sql        # Archivo SQL único con esquema y datos iniciales
```

> **Nota:** No importa si la carpeta de UI se llama `UI`, `GUI`, `Presentacion`, `Vistas` o `Frontend`. El generador analiza el `<OutputType>` y el nombre de los proyectos para auto-vincular el ejecutable principal. Tampoco importa si el script SQL está en la raíz o en una subcarpeta `Datos/` o `Database/`.

---

## 4. Pasos para Utilizar el Generador

1. Ejecute el archivo **`Iniciar_Generador.bat`** (o `Lanzador\GeneradorInstalador.exe`).
2. **Cargar el Proyecto:**
   - Arrastre y suelte el archivo `.zip` sobre la ventana.
   - O presione **"Cargar .ZIP..."** (también puede presionar **"Seleccionar Carpeta..."** si el proyecto ya está descomprimido).
3. **Diagnóstico Automático:**
   - La aplicación mostrará los badges en verde: `DAL [OK]`, `BE [OK]`, `BLL [OK]`, `GUI / UI [OK]`, `SQL SCRIPT [OK]`.
   - Autocompletará el nombre de la aplicación, el ejecutable principal y el nombre de la base de datos detectado en el script SQL.
4. **Personalización (Opcional):**
   - Puede ajustar el nombre de la aplicación, descripción o catálogo SQL si desea personalizarlo.
   - Marque o desmarque si desea generar también el archivo `.ZIP` final listo para enviar por Teams/Drive/Campus Virtual.
5. Presione **"COMPILAR Y GENERAR REINSTALADOR AUTOMÁTICO"**.
6. **Resultados:**
   - En segundos, se compila la solución en modo Release, se arma la carpeta de instalación y se genera el archivo `.zip`.
   - Puede presionar **"Probar Instalador Ahora"** para verificarlo en vivo o **"Abrir Carpeta Generada"** para ver los archivos listos para entrega.

---

## 5. Contenido del Instalador / Reinstalador Generado

El producto final generado por esta herramienta contiene:

1. **`Setup.exe`**: Asistente gráfico moderno para Windows con soporte completo de Instalación inicial y Reinstalación.
2. **`Instalar.bat`**: Lanzador con un doble clic para el usuario o evaluador.
3. **`Instalar_Consola.ps1`**: Script para instalación silenciosa / desatendida por línea de comandos.
4. **`manifest.json`**: Metadatos descriptivos de la aplicación.
5. **`Datos/script.sql`**: El script SQL maestro del alumno.
6. **`App/`**: Todos los binarios y dependencias compiladas de la aplicación.
7. **`INSTRUCCIONES_INSTALACION.md`**: Manual de usuario formateado automáticamente con el nombre de su aplicación.
