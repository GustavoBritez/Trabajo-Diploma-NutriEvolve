try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $diag = $eaRepo.GetDiagramByID(27)

    foreach ($dObj in $diag.DiagramObjects) {
        if ($dObj.ElementID -eq 495 -or $dObj.ElementID -eq 496) {
            $dObj.Style = "horiz=1;"
            $dObj.left = 520
            $dObj.right = 1060
            if ($dObj.ElementID -eq 495) {
                $dObj.top = -990
                $dObj.bottom = -998
            } else {
                $dObj.top = -1190
                $dObj.bottom = -1198
            }
            $dObj.Update()
            Write-Output "Updated Synchronization dObj $($dObj.ElementID) to horiz=1"
        }
    }
    $diag.DiagramObjects.Refresh()
    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result with dObj.Style=horiz=1: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
