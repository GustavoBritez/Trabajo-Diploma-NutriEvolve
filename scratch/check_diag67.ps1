try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $diag = $ea.GetDiagramByID(67)
    Write-Output "Diagram 67: $($diag.Name) | Pkg: $($diag.PackageID)"
    foreach ($do in $diag.DiagramObjects) {
        $elem = $ea.GetElementByID($do.ElementID)
        Write-Output "  - Elem: $($elem.Name) | Rect: left=$($do.left), right=$($do.right), top=$($do.top), bottom=$($do.bottom)"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
