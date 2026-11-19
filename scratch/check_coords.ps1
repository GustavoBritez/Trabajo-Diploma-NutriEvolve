try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    function ShowDiagramCoords($diagId) {
        $diag = $ea.GetDiagramByID($diagId)
        Write-Output "=== Diagram $diagId : $($diag.Name) ==="
        foreach ($do in $diag.DiagramObjects) {
            $elem = $ea.GetElementByID($do.ElementID)
            Write-Output "Elem: $($elem.Name) | Rect: left=$($do.left), right=$($do.right), top=$($do.top), bottom=$($do.bottom), sequence=$($do.Sequence)"
        }
    }

    ShowDiagramCoords(105)
    ShowDiagramCoords(109)
    ShowDiagramCoords(113)

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
