try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # Remove elements 499 and 500 from diagram and package
    for ($j = $diag.DiagramObjects.Count - 1; $j -ge 0; $j--) {
        $do = $diag.DiagramObjects.GetAt($j)
        if ($do.ElementID -eq 499 -or $do.ElementID -eq 500) {
            $diag.DiagramObjects.DeleteAt($j, $false)
            Write-Output "Deleted diagram object for $($do.ElementID)"
        }
    }
    $diag.DiagramObjects.Refresh()

    for ($i = $pkg.Elements.Count - 1; $i -ge 0; $i--) {
        $el = $pkg.Elements.GetAt($i)
        if ($el.ElementID -eq 499 -or $el.ElementID -eq 500) {
            $pkg.Elements.DeleteAt($i, $false)
            Write-Output "Deleted package element $($el.ElementID)"
        }
    }

    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Re-exported clean diagram image: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
