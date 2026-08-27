[Setup]
; Información básica de la aplicación
AppName=ING Software
AppVersion=1.0
AppPublisher=Tu Empresa
DefaultDirName={localappdata}\ING Software
DefaultGroupName=ING Software
; Archivo ejecutable de salida
OutputDir=.\Instalador
OutputBaseFilename=Instalador_ING
Compression=lzma
SolidCompression=yes
; Pide permisos de administrador al instalar
PrivilegesRequired=lowest

[Tasks]
Name: "desktopicon"; Description: "Crear acceso directo en el escritorio"; GroupDescription: "Accesos directos:"

[Files]
; Copiamos todos los archivos del publish de Visual Studio
Source: "UI\bin\Release\net9.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; Asegurarnos de copiar el script de la base de datos
Source: "script.sql"; DestDir: "{app}"; Flags: ignoreversion

; Empaquetar el instalador de LocalDB
Source: "SqlLocalDB.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall ignoreversion

[Icons]
; Acceso directo en el menú inicio y escritorio
Name: "{group}\ING Software"; Filename: "{app}\UI.exe"
Name: "{autodesktop}\ING Software"; Filename: "{app}\UI.exe"; Tasks: desktopicon

[Run]
; Instalar LocalDB pidiendo permisos de administrador solo para este paso (soluciona error 740)
Filename: "{tmp}\SqlLocalDB.exe"; Parameters: "/q /IACCEPTSQLLOCALDBLICENSETERMS=YES"; StatusMsg: "Instalando motor de Base de Datos (Esto puede tardar unos minutos)..."; Flags: waituntilterminated runhidden shellexec

; Ejecutar la aplicación al finalizar la instalación
Filename: "{app}\UI.exe"; Description: "Lanzar ING Software"; Flags: nowait postinstall skipifsilent

[Code]
// Aquí se podría agregar código en PascalScript para instalar LocalDB silenciosamente si lo necesitas,
// pero por ahora esta es la versión básica para compilar rápidamente.
