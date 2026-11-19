try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    # Also add to Package 5 (Domain Model)
    $pkg5 = $ea.GetPackageByID(5)
    $diag5 = $null
    foreach ($d in $pkg5.Diagrams) {
        if ($d.Name -eq "Tablas Asociadas en la Base de Datos") {
            $diag5 = $d
            break
        }
    }
    if ($diag5 -eq $null) {
        $diag5 = $pkg5.Diagrams.AddNew("Tablas Asociadas en la Base de Datos", "Logical")
        [void]$diag5.Update()
        $pkg5.Diagrams.Refresh()
    }

    # Clone objects into diag5
    $diag123 = $ea.GetDiagramByID(123)
    foreach ($do in $diag123.DiagramObjects) {
        $found = $false
        foreach ($d5o in $diag5.DiagramObjects) {
            if ($d5o.ElementID -eq $do.ElementID) { $found = $true; break }
        }
        if (-not $found) {
            $newDo = $diag5.DiagramObjects.AddNew("l=$($do.left);r=$($do.right);t=$($do.top);b=$($do.bottom);", "")
            $newDo.ElementID = $do.ElementID
            $newDo.left = $do.left
            $newDo.right = $do.right
            $newDo.top = $do.top
            $newDo.bottom = $do.bottom
            [void]$newDo.Update()
        }
    }
    $diag5.DiagramObjects.Refresh()
    [void]$ea.SaveDiagram($diag5.DiagramID)
    Write-Output "Diagram in Package 5 created / updated: $($diag5.Name) (ID: $($diag5.DiagramID))"

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
