try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) {
        Write-Error "Failed to open TD.EAP"
        exit 1
    }
    Write-Output "TD.EAP opened successfully."

    # Get Primary Use Cases Package (ID: 4) and Diagram (ID: 27)
    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    Write-Output "Configuring diagram: $($diag.Name)"

    # 1. Configure Swimlanes on Diagram
    $swimlanesDef = "swimlanes=locked=false;orientation=0;width=0;inbar=false;pwidth=0;cls=0;lane:Nutricionista (Operador):w=430:b=16777215:f=0:s=0:lane:Sistema (NutriEvolve):w=620:b=16777215:f=0:s=0:"
    $diag.Swimlanes = $swimlanesDef
    
    # Update StyleEx to ensure SwimlanesActive=1
    if ($diag.StyleEx -notmatch "SwimlanesActive=1") {
        $diag.StyleEx = $diag.StyleEx + ";SwimlanesActive=1;"
    }
    $diag.Update()

    # 2. Check / Create Synchronization Bars (Fork and Join)
    $forkElem = $null
    $joinElem = $null
    foreach ($elem in $pkg.Elements) {
        if ($elem.Type -eq "Synchronization") {
            if ($elem.Name -like "*Fork*" -or $elem.Name -eq "Fork_Persistencia") {
                $forkElem = $elem
            } elseif ($elem.Name -like "*Join*" -or $elem.Name -eq "Join_Persistencia") {
                $joinElem = $elem
            }
        }
    }

    if ($null -eq $forkElem) {
        $forkElem = $pkg.Elements.AddNew("Fork_Persistencia", "Synchronization")
        $forkElem.Subtype = 3
        $forkElem.Update()
        Write-Output "Created Fork element ID: $($forkElem.ElementID)"
    }
    if ($null -eq $joinElem) {
        $joinElem = $pkg.Elements.AddNew("Join_Persistencia", "Synchronization")
        $joinElem.Subtype = 4
        $joinElem.Update()
        Write-Output "Created Join element ID: $($joinElem.ElementID)"
    }

    # 3. Create or update Action for User confirmation if missing
    $confirmElem = $null
    foreach ($elem in $pkg.Elements) {
        if ($elem.Name -like "*Visualizar confirmaci*" -or $elem.Name -like "*confirmacion*") {
            $confirmElem = $elem
        }
    }
    if ($null -eq $confirmElem) {
        $confirmElem = $pkg.Elements.AddNew("Visualizar confirmacion de turno y codigo generado", "Action")
        $confirmElem.Update()
        Write-Output "Created Confirmation Action ID: $($confirmElem.ElementID)"
    }

    # Add Fork, Join, Confirm to Diagram if not present
    function Ensure-DiagramObject($d, $elemID) {
        foreach ($dObj in $d.DiagramObjects) {
            if ($dObj.ElementID -eq $elemID) { return $dObj }
        }
        $newDObj = $d.DiagramObjects.AddNew("", "")
        $newDObj.ElementID = $elemID
        $newDObj.Update()
        return $newDObj
    }

    Ensure-DiagramObject $diag $forkElem.ElementID | Out-Null
    Ensure-DiagramObject $diag $joinElem.ElementID | Out-Null
    Ensure-DiagramObject $diag $confirmElem.ElementID | Out-Null
    $diag.DiagramObjects.Refresh()

    # 4. Map Coordinates for all elements in the 2 Swimlanes
    # Lane 1 (Nutricionista): X=50..430 (Center=240, Width=240, Left=120, Right=360)
    # Lane 2 (Sistema): X=450..1070 (Center=740, Width=260, Left=610, Right=870)
    # Alt Column inside Sistema: Left=890, Right=1050

    $layoutMap = @{
        437 = @{ L=230; R=250; T=-30; B=-50 }   # Initial Node
        438 = @{ L=120; R=360; T=-80; B=-125 }  # Acceder al modulo
        439 = @{ L=110; R=370; T=-155; B=-200 } # Seleccionar fecha y profesional
        440 = @{ L=610; R=870; T=-155; B=-200 } # Ejecutar CU07: Consultar Disponibilidad
        441 = @{ L=700; R=780; T=-230; B=-275 } # Decision: Existen bloques?
        442 = @{ L=890; R=1060; T=-230; B=-275 } # Alt 4.1: Mostrar alerta no hay horarios
        443 = @{ L=610; R=870; T=-305; B=-350 } # Cargar selector bloques
        444 = @{ L=110; R=370; T=-380; B=-425 } # Ingresar DNI y buscar
        445 = @{ L=610; R=870; T=-380; B=-425 } # Buscar nino y tutor en BD
        446 = @{ L=700; R=780; T=-455; B=-500 } # Decision: Paciente registrado?
        447 = @{ L=100; R=380; T=-455; B=-500 } # CU08: Ingresar tutor
        448 = @{ L=90; R=390; T=-525; B=-575 }  # CU08: Ingresar nino (<19 anos)
        449 = @{ L=610; R=870; T=-525; B=-570 } # Mostrar datos nino y tutor
        450 = @{ L=110; R=370; T=-605; B=-650 } # Seleccionar bloque y motivo
        451 = @{ L=140; R=340; T=-680; B=-725 } # Presionar boton Agendar Turno
        452 = @{ L=700; R=780; T=-680; B=-725 } # Decision: Campos completos?
        453 = @{ L=890; R=1060; T=-680; B=-725 } # Alt 9.1: Resaltar campos faltantes
        454 = @{ L=690; R=790; T=-755; B=-800 } # Decision: Bloque continua disponible?
        455 = @{ L=890; R=1060; T=-755; B=-805 } # Alt 9.2: Conflicto concurrencia
        456 = @{ L=600; R=880; T=-835; B=-880 } # Generar codigo e instanciar TurnoSolicitado
        $forkElem.ElementID = @{ L=540; R=940; T=-910; B=-916 } # Fork bar
        457 = @{ L=480; R=700; T=-945; B=-995 } # Insertar en Turnos y bloque Ocupado
        458 = @{ L=730; R=970; T=-945; B=-995 } # Recalcular DVH y DVV
        459 = @{ L=480; R=700; T=-1025; B=-1070 } # Registrar Bitacora T06
        $joinElem.ElementID = @{ L=540; R=940; T=-1100; B=-1106 } # Join bar
        460 = @{ L=600; R=880; T=-1135; B=-1180 } # Mostrar mensaje exito y refrescar grilla
        $confirmElem.ElementID = @{ L=110; R=370; T=-1135; B=-1180 } # Visualizar confirmacion
        461 = @{ L=230; R=250; T=-1215; B=-1235 } # Final Node
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

    # 5. Set Guard Conditions on Connectors
    function Set-ConnectorGuard($srcID, $dstID, $guard) {
        $src = $eaRepo.GetElementByID($srcID)
        foreach ($conn in $src.Connectors) {
            if ($conn.SupplierID -eq $dstID) {
                $conn.TransitionGuard = $guard
                $conn.Update()
                Write-Output "Updated Guard on link $srcID -> $dstID to '$guard'"
            }
        }
    }

    Set-ConnectorGuard 441 442 "[No - Flujo Alt 4.1]"
    Set-ConnectorGuard 441 443 "[Si - Horarios Disponibles]"
    Set-ConnectorGuard 446 447 "[No - Ext CU08]"
    Set-ConnectorGuard 446 449 "[Si - Paciente Registrado]"
    Set-ConnectorGuard 452 453 "[No - Flujo Alt 9.1]"
    Set-ConnectorGuard 452 454 "[Si - Datos Validos]"
    Set-ConnectorGuard 454 455 "[No - Flujo Alt 9.2]"
    Set-ConnectorGuard 454 456 "[Si - Bloque Disponible]"

    # Ensure Fork/Join connections
    function Ensure-Connector($srcID, $dstID, $type, $guard) {
        $src = $eaRepo.GetElementByID($srcID)
        foreach ($c in $src.Connectors) {
            if ($c.SupplierID -eq $dstID -and $c.Type -eq $type) {
                if ($guard -ne "") { $c.TransitionGuard = $guard; $c.Update() }
                return $c
            }
        }
        $newConn = $src.Connectors.AddNew("", $type)
        $newConn.SupplierID = $dstID
        if ($guard -ne "") { $newConn.TransitionGuard = $guard }
        $newConn.Update()
        Write-Output "Created connector: $srcID -> $dstID ($type)"
        return $newConn
    }

    Ensure-Connector 456 $forkElem.ElementID "ControlFlow" "" | Out-Null
    Ensure-Connector $forkElem.ElementID 457 "ControlFlow" "" | Out-Null
    Ensure-Connector $forkElem.ElementID 458 "ControlFlow" "" | Out-Null
    Ensure-Connector 457 459 "ControlFlow" "" | Out-Null
    Ensure-Connector 459 $joinElem.ElementID "ControlFlow" "" | Out-Null
    Ensure-Connector 458 $joinElem.ElementID "ControlFlow" "" | Out-Null
    Ensure-Connector $joinElem.ElementID 460 "ControlFlow" "" | Out-Null
    Ensure-Connector 460 $confirmElem.ElementID "ControlFlow" "" | Out-Null
    Ensure-Connector $confirmElem.ElementID 461 "ControlFlow" "" | Out-Null

    # Remove direct old connector from 456 to 457 and 460 to 461 if bypassing fork/join
    $elem456 = $eaRepo.GetElementByID(456)
    for ($i = $elem456.Connectors.Count - 1; $i -ge 0; $i--) {
        $c = $elem456.Connectors.GetAt($i)
        if ($c.SupplierID -eq 457) {
            $elem456.Connectors.DeleteAt($i, $false)
            Write-Output "Removed old direct connector 456 -> 457"
        }
    }
    $elem460 = $eaRepo.GetElementByID(460)
    for ($i = $elem460.Connectors.Count - 1; $i -ge 0; $i--) {
        $c = $elem460.Connectors.GetAt($i)
        if ($c.SupplierID -eq 461) {
            $elem460.Connectors.DeleteAt($i, $false)
            Write-Output "Removed old direct connector 460 -> 461"
        }
    }

    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # 6. Export high-res diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Diagram image export result: $res to $outPng"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Completed successfully!"
} catch {
    Write-Error "Error during integration: $($_.Exception.Message)"
}
