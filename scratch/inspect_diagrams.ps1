try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    function InspectDiagram($diagId) {
        $diag = $ea.GetDiagramByID($diagId)
        Write-Output "=========================================="
        Write-Output "Diagram ID: $diagId | Name: $($diag.Name) | Type: $($diag.Type)"
        Write-Output "Diagram Objects ($($diag.DiagramObjects.Count)):"
        foreach ($do in $diag.DiagramObjects) {
            $elem = $ea.GetElementByID($do.ElementID)
            Write-Output "  - ElemID: $($elem.ElementID) | Name: $($elem.Name) | Type: $($elem.Type) | Stereotype: $($elem.Stereotype)"
        }
        Write-Output "Diagram Links ($($diag.DiagramLinks.Count)):"
        foreach ($dl in $diag.DiagramLinks) {
            $conn = $ea.GetConnectorByID($dl.ConnectorID)
            $src = $ea.GetElementByID($conn.ClientID)
            $tgt = $ea.GetElementByID($conn.SupplierID)
            Write-Output "  - ConnID: $($conn.ConnectorID) | Name: $($conn.Name) | Type: $($conn.Type) | From: $($src.Name) -> To: $($tgt.Name)"
        }
    }

    InspectDiagram(105)
    InspectDiagram(109)
    InspectDiagram(113)

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
