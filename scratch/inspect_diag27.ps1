try {
    $eaRepo = New-Object -ComObject EA.Repository
    $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    
    $diag = $eaRepo.GetDiagramByID(27)
    Write-Output "Diagram: $($diag.Name) (Type: $($diag.Type))"
    Write-Output "SwimlanesDef: $($diag.Swimlanes)"
    Write-Output "StyleEx: $($diag.StyleEx)"
    Write-Output "ExtendedStyle: $($diag.ExtendedStyle)"
    
    Write-Output "`n--- Diagram Objects Count: $($diag.DiagramObjects.Count) ---"
    foreach ($dObj in $diag.DiagramObjects) {
        $elem = $eaRepo.GetElementByID($dObj.ElementID)
        Write-Output "Object: ID=$($elem.ElementID), Name='$($elem.Name)', Type='$($elem.Type)', Pos=(L:$($dObj.left), R:$($dObj.right), T:$($dObj.top), B:$($dObj.bottom))"
    }

    Write-Output "`n--- Diagram Links Count: $($diag.DiagramLinks.Count) ---"
    foreach ($dLink in $diag.DiagramLinks) {
        $conn = $eaRepo.GetConnectorByID($dLink.ConnectorID)
        $src = $eaRepo.GetElementByID($conn.ClientID)
        $dst = $eaRepo.GetElementByID($conn.SupplierID)
        Write-Output "Link: ID=$($conn.ConnectorID), Type='$($conn.Type)', Guard='$($conn.TransitionGuard)', From='$($src.Name)' -> To='$($dst.Name)'"
    }

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
