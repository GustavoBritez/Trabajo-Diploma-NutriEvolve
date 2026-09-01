try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # 1. Configure Synchronization elements to be HORIZONTAL BARS (horiz=1 in StyleEx / PDATA1=1)
    $fork = $eaRepo.GetElementByID(495)
    $join = $eaRepo.GetElementByID(496)

    $fork.StyleEx = "horiz=1;"
    $fork.Update()
    $join.StyleEx = "horiz=1;"
    $join.Update()

    $eaRepo.Execute("UPDATE t_object SET PDATA1 = '1', StyleEx = 'horiz=1;' WHERE Object_ID IN (495, 496)")

    # 2. Generous vertical spacing layout
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
        439 = @{ L=130; R=390; T=-170; B=-215 }   # Seleccionar fecha y profesional
        440 = @{ L=540; R=800; T=-170; B=-215 }   # Ejecutar CU07: Consultar Disponibilidad
        441 = @{ L=630; R=710; T=-250; B=-290 }   # Decision: Existen bloques?
        442 = @{ L=870; R=1090; T=-250; B=-295 }  # Alt 4.1: Mostrar alerta no hay horarios
        $altFinalElem.ElementID = @{ L=970; R=990; T=-330; B=-350 } # Alt Final Node
        443 = @{ L=540; R=800; T=-330; B=-375 }   # Cargar selector bloques
        444 = @{ L=130; R=390; T=-410; B=-455 }   # Ingresar DNI y buscar
        445 = @{ L=540; R=800; T=-410; B=-455 }   # Buscar nino y tutor en BD
        446 = @{ L=630; R=710; T=-490; B=-530 }   # Decision: Paciente registrado?
        447 = @{ L=130; R=390; T=-490; B=-535 }   # CU08: Ingresar tutor
        448 = @{ L=120; R=400; T=-565; B=-615 }   # CU08: Ingresar nino (<19 anos)
        449 = @{ L=540; R=800; T=-565; B=-610 }   # Mostrar datos nino y tutor
        450 = @{ L=130; R=390; T=-650; B=-695 }   # Seleccionar bloque y motivo
        451 = @{ L=150; R=370; T=-730; B=-775 }   # Presionar boton Agendar Turno
        452 = @{ L=630; R=710; T=-730; B=-770 }   # Decision: Campos completos?
        453 = @{ L=870; R=1090; T=-730; B=-775 }  # Alt 9.1: Resaltar campos faltantes
        454 = @{ L=630; R=710; T=-820; B=-860 }   # Decision: Bloque continua disponible?
        455 = @{ L=870; R=1090; T=-820; B=-865 }  # Alt 9.2: Conflicto concurrencia
        456 = @{ L=530; R=810; T=-910; B=-960 }   # Generar codigo e instanciar TurnoSolicitado
        495 = @{ L=520; R=1060; T=-990; B=-998 }  # Fork bar (WIDE 540px, Height: 8px)
        457 = @{ L=520; R=760; T=-1030; B=-1080 } # Branch 1A: Insertar en Turnos
        459 = @{ L=520; R=760; T=-1110; B=-1155 } # Branch 1B: Registrar Bitacora T06
        458 = @{ L=820; R=1060; T=-1030; B=-1100 }# Branch 2: Recalcular DVH y DVV T08
        496 = @{ L=520; R=1060; T=-1190; B=-1198 }# Join bar (WIDE 540px, Height: 8px)
        460 = @{ L=540; R=800; T=-1235; B=-1280 } # Mostrar mensaje exito y refrescar grilla
        497 = @{ L=130; R=390; T=-1235; B=-1280 } # Visualizar confirmacion
        461 = @{ L=250; R=270; T=-1320; B=-1340 } # Final Node
        $b1.ElementID = @{ L=40; R=480; T=-10; B=-1370 }   # Boundary Nutricionista
        $b2.ElementID = @{ L=490; R=1140; T=-10; B=-1370 } # Boundary Sistema
    }

    foreach ($dObj in $diag.DiagramObjects) {
        if ($layoutMap.ContainsKey($dObj.ElementID)) {
            $coords = $layoutMap[$dObj.ElementID]
            $dObj.left = $coords.L
            $dObj.right = $coords.R
            $dObj.top = $coords.T
            $dObj.bottom = $coords.B
            if ($dObj.ElementID -eq $b1.ElementID -or $dObj.ElementID -eq $b2.ElementID) {
                $dObj.Sequence = 1000
            }
            $dObj.Update()
        }
    }
    $diag.DiagramObjects.Refresh()

    # 3. Clean waypoints for return lines (914 and 917) so they don't cut across the middle
    # Link 914: 453 (Resaltar campos) -> 450 (Seleccionar bloque)
    # Route it above: from X=1090, Y=-750 -> up to Y=-630 -> left to X=260 -> down to 450
    # Link 917: 455 (Mostrar alerta conflicto) -> 450 (Seleccionar bloque)
    # Route it around right: from X=1090, Y=-840 -> up to Y=-620 -> left to X=260 -> down to 450

    foreach ($dLink in $diag.DiagramLinks) {
        $c = $eaRepo.GetConnectorByID($dLink.ConnectorID)
        if ($c.ClientID -eq 453 -and $c.SupplierID -eq 450) {
            # Clean orthogonal path around right and top
            $dLink.Geometry = "EDGE=2;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=1110:-752;1110:-635;260:-635;"
            $dLink.Style = "Mode=3;"
            $dLink.Update()
            Write-Output "Cleaned route for Alt 9.1 return link"
        } elseif ($c.ClientID -eq 455 -and $c.SupplierID -eq 450) {
            $dLink.Geometry = "EDGE=2;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=1125:-842;1125:-620;260:-620;"
            $dLink.Style = "Mode=3;"
            $dLink.Update()
            Write-Output "Cleaned route for Alt 9.2 return link"
        } elseif ($c.ClientID -eq 458 -and $c.SupplierID -eq 496) {
            # From right branch to right of Join
            $dLink.Geometry = "EDGE=3;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=940:-1100;940:-1190;"
            $dLink.Style = "Mode=3;"
            $dLink.Update()
        } elseif ($c.ClientID -eq 459 -and $c.SupplierID -eq 496) {
            # From left branch to left of Join
            $dLink.Geometry = "EDGE=3;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=640:-1155;640:-1190;"
            $dLink.Style = "Mode=3;"
            $dLink.Update()
        } elseif ($c.ClientID -eq 495 -and $c.SupplierID -eq 458) {
            # From right of Fork to right branch
            $dLink.Geometry = "EDGE=1;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=940:-998;940:-1030;"
            $dLink.Style = "Mode=3;"
            $dLink.Update()
        } elseif ($c.ClientID -eq 495 -and $c.SupplierID -eq 457) {
            # From left of Fork to left branch
            $dLink.Geometry = "EDGE=1;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=640:-998;640:-1030;"
            $dLink.Style = "Mode=3;"
            $dLink.Update()
        } else {
            $dLink.Style = "Mode=3;"
            $dLink.Update()
        }
    }
    $diag.DiagramLinks.Refresh()

    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # 4. Export high-res diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result with cleaned routes: $res to $outPng"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Completed successfully!"
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
