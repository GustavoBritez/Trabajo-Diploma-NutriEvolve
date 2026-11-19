try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $diag = $ea.GetDiagramByID(123)
    Write-Output "Cleaning duplicate diagram links in Diagram 123..."
    
    # We want exactly these clean connectors:
    # 1. Pacientes -> Telefonos
    # 2. Pacientes -> Emails
    # 3. Pacientes -> Pacientes_Cambios
    # 4. Pacientes -> Consultas
    # 5. Consultas -> Consultas_Cambios
    # 6. Usuarios -> Pacientes_Cambios
    # 7. Usuarios -> Consultas_Cambios
    # 8. Usuarios -> Consultas
    # 9. Usuarios -> Bitacora

    # Let's inspect all DiagramLinks in Diagram 123
    $seenPairs = @{}
    $toDelete = @()

    foreach ($dl in $diag.DiagramLinks) {
        $c = $ea.GetConnectorByID($dl.ConnectorID)
        $pairKey = "$($c.ClientID)_$($c.SupplierID)"
        if ($seenPairs.ContainsKey($pairKey)) {
            $toDelete += $c.ConnectorID
        } else {
            $seenPairs[$pairKey] = $c.ConnectorID
        }
    }

    Write-Output "Deleting $($toDelete.Count) redundant connectors..."
    foreach ($cId in $toDelete) {
        $conn = $ea.GetConnectorByID($cId)
        $srcElem = $ea.GetElementByID($conn.ClientID)
        for ($i = 0; $i -lt $srcElem.Connectors.Count; $i++) {
            if ($srcElem.Connectors.GetAt($i).ConnectorID -eq $cId) {
                $srcElem.Connectors.Delete($i)
                [void]$srcElem.Update()
                $srcElem.Connectors.Refresh()
                break
            }
        }
    }

    $diag.DiagramLinks.Refresh()
    [void]$ea.SaveDiagram(123)

    # Re-export diagram image
    $project = $ea.GetProjectInterface()
    $imgFile = "C:\Users\Navegador\Desktop\GIT\Diagramas\Exported\Tablas Asociadas en la Base de Datos.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $imgFile, 1)
    Write-Output "Exported clean diagram: $res"

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
