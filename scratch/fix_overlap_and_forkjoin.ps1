try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # 1. Clean connector guards: DO NOT include literal brackets [] because EA adds them automatically!
    function Set-CleanGuard($srcID, $dstID, $cleanText) {
        $src = $eaRepo.GetElementByID($srcID)
        foreach ($c in $src.Connectors) {
            if ($c.SupplierID -eq $dstID) {
                $c.Name = ""
                $c.TransitionGuard = $cleanText
                $c.Update()
                Write-Output "Set clean guard on link $srcID -> ${dstID}: '$cleanText'"
            }
        }
    }

    Set-CleanGuard 441 442 "No hay horarios - Flujo Alt 4.1"
    Set-CleanGuard 441 443 "Si - Horarios disponibles"
    Set-CleanGuard 446 447 "No registrado - Ext CU08"
    Set-CleanGuard 446 449 "Si - Paciente registrado"
    Set-CleanGuard 452 453 "Campos incompletos - Flujo Alt 9.1"
    Set-CleanGuard 452 454 "Si - Datos validos"
    Set-CleanGuard 454 455 "Conflicto concurrencia - Flujo Alt 9.2"
    Set-CleanGuard 454 456 "Si - Bloque disponible"

    # Find elements
    $altFinalElem = $null
    $b1 = $null
    $b2 = $null
    foreach ($el in $pkg.Elements) {
        if ($el.Name -eq "Fin_Alt4_1") { $altFinalElem = $el }
        if ($el.ElementID -eq 501 -or ($el.Type -eq "Boundary" -and $el.Name -like "*Nutricionista*")) { $b1 = $el }
        if ($el.ElementID -eq 502 -or ($el.Type -eq "Boundary" -and $el.Name -like "*Sistema*")) { $b2 = $el }
    }

    $forkElemID = 495
    $joinElemID = 496
    $confirmElemID = 497

    # 2. Precision Layout Coordinates
    # Canvas Width: 1150px
    # Lane 1 (Nutricionista): Left=40, Right=480 (Center=260)
    # Lane 2 (Sistema Main): Left=540, Right=800 (Center=670)
    # Lane 2 (Sistema Alt): Left=880, Right=1100 (Center=990)
    # Lane 2 (Fork/Join): Left=520, Right=1060 (Width=540px!)

    $layoutMap = @{
        437 = @{ L=250; R=270; T=-40; B=-60 }     # Initial Node
        438 = @{ L=130; R=390; T=-90; B=-135 }    # Acceder al modulo
        439 = @{ L=130; R=390; T=-165; B=-210 }   # Seleccionar fecha y profesional
        440 = @{ L=540; R=800; T=-165; B=-210 }   # Ejecutar CU07: Consultar Disponibilidad
        441 = @{ L=625; R=715; T=-240; B=-280 }   # Decision: Existen bloques?
        442 = @{ L=880; R=1100; T=-240; B=-285 }  # Alt 4.1: Mostrar alerta no hay horarios
        $altFinalElem.ElementID = @{ L=980; R=1000; T=-315; B=-335 } # Alt Final Node
        443 = @{ L=540; R=800; T=-315; B=-360 }   # Cargar selector bloques
        444 = @{ L=130; R=390; T=-390; B=-435 }   # Ingresar DNI y buscar
        445 = @{ L=540; R=800; T=-390; B=-435 }   # Buscar nino y tutor en BD
        446 = @{ L=625; R=715; T=-465; B=-505 }   # Decision: Paciente registrado?
        447 = @{ L=130; R=390; T=-465; B=-510 }   # CU08: Ingresar tutor
        448 = @{ L=120; R=400; T=-535; B=-585 }   # CU08: Ingresar nino (<19 anos)
        449 = @{ L=540; R=800; T=-535; B=-580 }   # Mostrar datos nino y tutor
        450 = @{ L=130; R=390; T=-615; B=-660 }   # Seleccionar bloque y motivo
        451 = @{ L=150; R=370; T=-690; B=-735 }   # Presionar boton Agendar Turno
        452 = @{ L=625; R=715; T=-690; B=-730 }   # Decision: Campos completos?
        453 = @{ L=880; R=1100; T=-690; B=-735 }  # Alt 9.1: Resaltar campos faltantes
        454 = @{ L=625; R=715; T=-770; B=-810 }   # Decision: Bloque continua disponible?
        455 = @{ L=880; R=1100; T=-770; B=-815 }  # Alt 9.2: Conflicto concurrencia
        456 = @{ L=530; R=810; T=-850; B=-900 }   # Generar codigo e instanciar TurnoSolicitado
        $forkElemID = @{ L=520; R=1060; T=-930; B=-942 } # Fork bar (540px wide!)
        457 = @{ L=520; R=760; T=-975; B=-1025 }  # Branch 1A: Insertar en Turnos y bloque Ocupado
        459 = @{ L=520; R=760; T=-1050; B=-1095 } # Branch 1B: Registrar Bitacora T06
        458 = @{ L=820; R=1060; T=-975; B=-1045 } # Branch 2: Recalcular DVH y DVV T08
        $joinElemID = @{ L=520; R=1060; T=-1130; B=-1142 } # Join bar (540px wide!)
        460 = @{ L=540; R=800; T=-1175; B=-1220 } # Mostrar mensaje exito y refrescar grilla
        $confirmElemID = @{ L=130; R=390; T=-1175; B=-1220 } # Visualizar confirmacion
        461 = @{ L=250; R=270; T=-1260; B=-1280 } # Final Node
        $b1.ElementID = @{ L=40; R=480; T=-10; B=-1310 }   # Boundary Nutricionista
        $b2.ElementID = @{ L=490; R=1140; T=-10; B=-1310 } # Boundary Sistema
    }

    foreach ($dObj in $diag.DiagramObjects) {
        if ($layoutMap.ContainsKey($dObj.ElementID)) {
            $coords = $layoutMap[$dObj.ElementID]
            $dObj.left = $coords.L
            $dObj.right = $coords.R
            $dObj.top = $coords.T
            $dObj.bottom = $coords.B
            if ($dObj.ElementID -eq $b1.ElementID -or $dObj.ElementID -eq $b2.ElementID) {
                $dObj.Sequence = 1000 # Send boundaries to background
            }
            $dObj.Update()
        }
    }
    $diag.DiagramObjects.Refresh()

    # 3. Clean and optimize connector routing (Mode=3 Orthogonal / Tree for clean layout)
    foreach ($dLink in $diag.DiagramLinks) {
        $dLink.Style = "Mode=3;" # Orthogonal routing
        $dLink.Geometry = ""     # Reset messy manual bends
        $dLink.Update()
    }
    $diag.DiagramLinks.Refresh()

    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # 4. Export high-res diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result with wide fork/join and orthogonal routing: $res to $outPng"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Completed successfully!"
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
