try {
    $eaRepo = New-Object -ComObject EA.Repository
    $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    $diag = $eaRepo.GetDiagramByID(27)

    Write-Output "--- Connectors on Diagram 27 ---"
    foreach ($dLink in $diag.DiagramLinks) {
        $c = $eaRepo.GetConnectorByID($dLink.ConnectorID)
        $src = $eaRepo.GetElementByID($c.ClientID)
        $dst = $eaRepo.GetElementByID($c.SupplierID)
        Write-Output "ID=$($c.ConnectorID) | Type=$($c.Type) | Name='$($c.Name)' | Guard='$($c.TransitionGuard)' | Src=[$($src.ElementID)] '$($src.Name)' -> Dst=[$($dst.ElementID)] '$($dst.Name)'"
    }

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
