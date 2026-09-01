try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # 1. Update Fork and Join Elements
    $eaRepo.Execute("UPDATE t_object SET Name = '', PDATA1 = 'H', StyleEx = 'horiz=1;' WHERE Object_ID IN (495, 496)")

    # 2. Precision Layout
    # Column 1 (Nutricionista): Left=40, Right=480 (Center=260)
    # Column 2 (Sistema): Left=490, Right=1140 (Center=815)
    # Main Actions in Sistema: Center=680 (Left=540, Right=820)
    # Decisions in Sistema: Center=680 (Left=635, Right=725)
    # Alt Actions in Sistema: Center=990 (Left=890, Right=1110)

    $altFinalElem = $null
    $b1 = $null
    $b2 = $null
    foreach ($el in $pkg.Elements) {
        if ($el.Name -eq "Fin_Alt4_1") { $altFinalElem = $el }
        if ($el.ElementID -eq 501 -or ($el.Type -eq "Boundary" -and $el.Name -like "*Nutricionista*")) { $b1 = $el }
        if ($el.ElementID -eq 502 -or ($el.Type -eq "Boundary" -and $el.Name -like "*Sistema*")) { $b2 = $el }
    }

    $layoutMap = @{
        437 = @{ L=250; R=270; T=-40; B=-60 }     # Initial Node
        438 = @{ L=130; R=390; T=-90; B=-135 }    # Acceder al modulo
        439 = @{ L=130; R=390; T=-175; B=-220 }   # Seleccionar fecha y profesional
        440 = @{ L=540; R=820; T=-175; B=-220 }   # Ejecutar CU07: Consultar Disponibilidad
        441 = @{ L=635; R=725; T=-260; B=-300 }   # Decision: Existen bloques?
        442 = @{ L=890; R=1110; T=-260; B=-305 }  # Alt 4.1: Mostrar alerta no hay horarios
        $altFinalElem.ElementID = @{ L=990; R=1010; T=-345; B=-365 } # Alt Final Node
        443 = @{ L=540; R=820; T=-345; B=-390 }   # Cargar selector bloques
        444 = @{ L=130; R=390; T=-430; B=-475 }   # Ingresar DNI y buscar
        445 = @{ L=540; R=820; T=-430; B=-475 }   # Buscar nino y tutor en BD
        446 = @{ L=635; R=725; T=-515; B=-555 }   # Decision: Paciente registrado?
        447 = @{ L=130; R=390; T=-515; B=-560 }   # CU08: Ingresar tutor
        448 = @{ L=120; R=400; T=-595; B=-645 }   # CU08: Ingresar nino (<19 anos)
        449 = @{ L=540; R=820; T=-595; B=-640 }   # Mostrar datos nino y tutor
        450 = @{ L=130; R=390; T=-685; B=-730 }   # Seleccionar bloque y motivo
        451 = @{ L=150; R=370; T=-770; B=-815 }   # Presionar boton Agendar Turno
        452 = @{ L=635; R=725; T=-770; B=-810 }   # Decision: Campos completos?
        453 = @{ L=890; R=1110; T=-770; B=-815 }  # Alt 9.1: Resaltar campos faltantes
        454 = @{ L=635; R=725; T=-865; B=-905 }   # Decision: Bloque continua disponible?
        455 = @{ L=890; R=1110; T=-865; B=-910 }  # Alt 9.2: Conflicto concurrencia
        456 = @{ L=530; R=830; T=-955; B=-1005 }  # Generar codigo e instanciar TurnoSolicitado
        495 = @{ L=500; R=1080; T=-1040; B=-1048 } # Fork bar (580px wide horizontal bar)
        457 = @{ L=510; R=760; T=-1085; B=-1135 } # Branch 1A: Insertar en Turnos
        459 = @{ L=510; R=760; T=-1165; B=-1210 } # Branch 1B: Registrar Bitacora T06
        458 = @{ L=820; R=1070; T=-1085; B=-1155 }# Branch 2: Recalcular DVH y DVV T08
        496 = @{ L=500; R=1080; T=-1245; B=-1253 } # Join bar (580px wide horizontal bar)
        460 = @{ L=540; R=820; T=-1290; B=-1335 } # Mostrar mensaje exito y refrescar grilla
        497 = @{ L=130; R=390; T=-1290; B=-1335 } # Visualizar confirmacion
        461 = @{ L=250; R=270; T=-1380; B=-1400 } # Final Node
        $b1.ElementID = @{ L=40; R=480; T=-10; B=-1430 }   # Boundary Nutricionista
        $b2.ElementID = @{ L=490; R=1140; T=-10; B=-1430 } # Boundary Sistema
    }

    foreach ($dObj in $diag.DiagramObjects) {
        if ($layoutMap.ContainsKey($dObj.ElementID)) {
            $coords = $layoutMap[$dObj.ElementID]
            $dObj.left = $coords.L
            $dObj.right = $coords.R
            $dObj.top = $coords.T
            $dObj.bottom = $coords.B
            if ($dObj.ElementID -eq 495 -or $dObj.ElementID -eq 496) {
                $dObj.Style = "horiz=1;"
            }
            if ($dObj.ElementID -eq $b1.ElementID -or $dObj.ElementID -eq $b2.ElementID) {
                $dObj.Sequence = 1000
            }
            $dObj.Update()
        }
    }
    $diag.DiagramObjects.Refresh()

    # 3. Clean routing on all connectors: Orthogonal / Right-Angle routing
    foreach ($dLink in $diag.DiagramLinks) {
        $c = $eaRepo.GetConnectorByID($dLink.ConnectorID)
        # For return lines from Alt 9.1 and Alt 9.2, set clean orthogonal style
        if ($c.ClientID -eq 453 -and $c.SupplierID -eq 450) {
            $dLink.Style = "Mode=3;Tree=0;EOAngle=1;"
            $dLink.Update()
        } elseif ($c.ClientID -eq 455 -and $c.SupplierID -eq 450) {
            $dLink.Style = "Mode=3;Tree=0;EOAngle=1;"
            $dLink.Update()
        } else {
            $dLink.Style = "Mode=3;Tree=0;"
            $dLink.Update()
        }
    }
    $diag.DiagramLinks.Refresh()

    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # 4. Export diagram
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result: $res to $outPng"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Clean layout completed successfully!"
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
