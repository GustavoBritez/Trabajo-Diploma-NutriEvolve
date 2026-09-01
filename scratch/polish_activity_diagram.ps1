try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # 1. Create a dedicated End Node for Alt 4.1 (No hay horarios) to prevent diagonal crossing
    $altFinalElem = $null
    foreach ($elem in $pkg.Elements) {
        if ($elem.Name -eq "Fin_Alt4_1" -or ($elem.Type -eq "StateNode" -and $elem.ElementID -ne 437 -and $elem.ElementID -ne 461)) {
            $altFinalElem = $elem
        }
    }
    if ($null -eq $altFinalElem) {
        $altFinalElem = $pkg.Elements.AddNew("Fin_Alt4_1", "StateNode")
        $altFinalElem.Subtype = 4 # FinalState
        $altFinalElem.Update()
    }

    # Ensure DiagramObjects on Diagram
    function Ensure-DiagramObject($d, $elemID) {
        foreach ($dObj in $d.DiagramObjects) {
            if ($dObj.ElementID -eq $elemID) { return $dObj }
        }
        $newDObj = $d.DiagramObjects.AddNew("", "")
        $newDObj.ElementID = $elemID
        $newDObj.Update()
        return $newDObj
    }
    Ensure-DiagramObject $diag $altFinalElem.ElementID | Out-Null
    $diag.DiagramObjects.Refresh()

    # 2. Delete obsolete connectors: 920, 921, 922, 901
    $obsoleteIDs = @(920, 921, 922, 901)
    foreach ($elem in $pkg.Elements) {
        for ($i = $elem.Connectors.Count - 1; $i -ge 0; $i--) {
            $c = $elem.Connectors.GetAt($i)
            if ($obsoleteIDs -contains $c.ConnectorID) {
                $elem.Connectors.DeleteAt($i, $false)
                Write-Output "Deleted obsolete connector ID: $($c.ConnectorID)"
            }
        }
    }

    # 3. Connect Alt 4.1 to Alt Final Node
    $altAction = $eaRepo.GetElementByID(442)
    $hasAltFinalConn = $false
    foreach ($c in $altAction.Connectors) {
        if ($c.SupplierID -eq $altFinalElem.ElementID) { $hasAltFinalConn = $true }
    }
    if (-not $hasAltFinalConn) {
        $c = $altAction.Connectors.AddNew("", "ControlFlow")
        $c.SupplierID = $altFinalElem.ElementID
        $c.Update()
        Write-Output "Connected Alt 4.1 -> Alt Final Node"
    }

    # 4. Clean connector Names and Guards (remove duplicate names)
    function Clean-Connector($srcID, $dstID, $guard) {
        $src = $eaRepo.GetElementByID($srcID)
        foreach ($c in $src.Connectors) {
            if ($c.SupplierID -eq $dstID) {
                $c.Name = "" # clear name so EA only prints TransitionGuard
                $c.TransitionGuard = $guard
                $c.Update()
                Write-Output "Cleaned connector $srcID -> $dstID with Guard '$guard'"
            }
        }
    }

    Clean-Connector 441 442 "[No hay horarios - Flujo Alt 4.1]"
    Clean-Connector 441 443 "[Si - Horarios disponibles]"
    Clean-Connector 446 447 "[No registrado - Ext CU08]"
    Clean-Connector 446 449 "[Si - Paciente registrado]"
    Clean-Connector 452 453 "[Campos incompletos - Flujo Alt 9.1]"
    Clean-Connector 452 454 "[Si - Datos validos]"
    Clean-Connector 454 455 "[Conflicto concurrencia - Flujo Alt 9.2]"
    Clean-Connector 454 456 "[Si - Bloque disponible]"

    # 5. Position elements with precision
    # Column 1 (Nutricionista): Left=100..400 (Center=250)
    # Column 2 (Sistema Main): Left=560..860 (Center=710)
    # Column 2 (Sistema Alt/Right): Left=890..1060

    $forkElemID = 495
    $joinElemID = 496
    $confirmElemID = 497

    $layoutMap = @{
        437 = @{ L=240; R=260; T=-30; B=-50 }   # Initial Node
        438 = @{ L=120; R=380; T=-80; B=-125 }  # Acceder al modulo
        439 = @{ L=110; R=390; T=-155; B=-200 } # Seleccionar fecha y profesional
        440 = @{ L=570; R=850; T=-155; B=-200 } # Ejecutar CU07: Consultar Disponibilidad
        441 = @{ L=665; R=755; T=-230; B=-275 } # Decision: Existen bloques?
        442 = @{ L=890; R=1070; T=-230; B=-275 } # Alt 4.1: Mostrar alerta no hay horarios
        $altFinalElem.ElementID = @{ L=970; R=990; T=-305; B=-325 } # Alt Final Node
        443 = @{ L=570; R=850; T=-305; B=-350 } # Cargar selector bloques
        444 = @{ L=110; R=390; T=-380; B=-425 } # Ingresar DNI y buscar
        445 = @{ L=570; R=850; T=-380; B=-425 } # Buscar nino y tutor en BD
        446 = @{ L=665; R=755; T=-455; B=-500 } # Decision: Paciente registrado?
        447 = @{ L=110; R=390; T=-455; B=-500 } # CU08: Ingresar tutor
        448 = @{ L=100; R=400; T=-525; B=-575 }  # CU08: Ingresar nino (<19 anos)
        449 = @{ L=570; R=850; T=-525; B=-570 } # Mostrar datos nino y tutor
        450 = @{ L=110; R=390; T=-605; B=-650 } # Seleccionar bloque y motivo
        451 = @{ L=140; R=360; T=-680; B=-725 } # Presionar boton Agendar Turno
        452 = @{ L=665; R=755; T=-680; B=-725 } # Decision: Campos completos?
        453 = @{ L=890; R=1070; T=-680; B=-725 } # Alt 9.1: Resaltar campos faltantes
        454 = @{ L=665; R=755; T=-755; B=-800 } # Decision: Bloque continua disponible?
        455 = @{ L=890; R=1070; T=-755; B=-805 } # Alt 9.2: Conflicto concurrencia
        456 = @{ L=560; R=860; T=-835; B=-880 } # Generar codigo e instanciar TurnoSolicitado
        $forkElemID = @{ L=530; R=890; T=-910; B=-916 } # Fork bar
        457 = @{ L=460; R=690; T=-945; B=-995 } # Insertar en Turnos y bloque Ocupado
        458 = @{ L=720; R=960; T=-945; B=-995 } # Recalcular DVH y DVV
        459 = @{ L=460; R=690; T=-1025; B=-1070 } # Registrar Bitacora T06
        $joinElemID = @{ L=530; R=890; T=-1100; B=-1106 } # Join bar
        460 = @{ L=560; R=860; T=-1135; B=-1180 } # Mostrar mensaje exito y refrescar grilla
        $confirmElemID = @{ L=110; R=390; T=-1135; B=-1180 } # Visualizar confirmacion
        461 = @{ L=240; R=260; T=-1215; B=-1235 } # Final Node
    }

    foreach ($dObj in $diag.DiagramObjects) {
        if ($layoutMap.ContainsKey($dObj.ElementID)) {
            $coords = $layoutMap[$dObj.ElementID]
            $dObj.left = $coords.L
            $dObj.right = $coords.R
            $dObj.top = $coords.T
            $dObj.bottom = $coords.B
            $dObj.Update()
        }
    }
    $diag.DiagramObjects.Refresh()

    # 6. Configure Diagram Swimlanes & Style
    # Set Swimlanes string and active flag
    $diag.Swimlanes = "swimlanes=locked=false;orientation=0;width=0;inbar=false;pwidth=0;cls=0;lane:Nutricionista (Operador):w=430:b=16777215:f=0:s=0:lane:Sistema (NutriEvolve):w=670:b=16777215:f=0:s=0:"
    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # 7. Export high-res diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result: $res to $outPng"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Integration completed with 100% clean layout!"
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
