try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $diag = $eaRepo.GetDiagramByID(26)
    Write-Output "Diagram ID: $($diag.DiagramID), Name: '$($diag.Name)', Type: '$($diag.Type)'"
    Write-Output "StyleEx: $($diag.StyleEx)"
    Write-Output "ExtendedStyle: $($diag.ExtendedStyle)"

    Write-Output "`n--- Diagram Objects in DER (Count: $($diag.DiagramObjects.Count)) ---"
    foreach ($dObj in $diag.DiagramObjects) {
        $elem = $eaRepo.GetElementByID($dObj.ElementID)
        Write-Output "Elem ID=$($elem.ElementID), Name='$($elem.Name)', Type='$($elem.Type)', Stereotype='$($elem.Stereotype)'"
    }

    Write-Output "`n--- Diagram Links in DER (Count: $($diag.DiagramLinks.Count)) ---"
    foreach ($dLink in $diag.DiagramLinks) {
        $c = $eaRepo.GetConnectorByID($dLink.ConnectorID)
        $src = $eaRepo.GetElementByID($c.ClientID)
        $dst = $eaRepo.GetElementByID($c.SupplierID)
        Write-Output "Link ID=$($c.ConnectorID), Type='$($c.Type)', Name='$($c.Name)', ClientCard='$($c.ClientEnd.Cardinality)', SupplierCard='$($c.SupplierEnd.Cardinality)', From='$($src.Name)' -> To='$($dst.Name)'"
    }

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
