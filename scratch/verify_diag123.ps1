try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $diag = $ea.GetDiagramByID(123)
    Write-Output "Diagram ID 123: $($diag.Name) | Package ID: $($diag.PackageID)"
    Write-Output "--- Objects ($($diag.DiagramObjects.Count)) ---"
    foreach ($do in $diag.DiagramObjects) {
        $elem = $ea.GetElementByID($do.ElementID)
        Write-Output "  - ElemID: $($elem.ElementID) | Name: $($elem.Name) | Left: $($do.left), Right: $($do.right), Top: $($do.top), Bottom: $($do.bottom)"
    }
    Write-Output "--- Links ($($diag.DiagramLinks.Count)) ---"
    foreach ($dl in $diag.DiagramLinks) {
        $c = $ea.GetConnectorByID($dl.ConnectorID)
        $src = $ea.GetElementByID($c.ClientID)
        $tgt = $ea.GetElementByID($c.SupplierID)
        Write-Output "  - LinkID: $($c.ConnectorID) | Name: $($c.Name) | From: $($src.Name) -> To: $($tgt.Name)"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
