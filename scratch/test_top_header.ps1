try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    # In EA database for ActivityPartition:
    # Header orientation:
    # t_object.PDATA1 = '0' (Vertical orientation with header at top)
    # t_object.NType = 0 or 1
    # t_object.StyleEx = "isVertical=1;header=top;"
    $eaRepo.Execute("UPDATE t_object SET PDATA1 = '0', StyleEx = 'isVertical=1;' WHERE Object_ID IN (499, 500)")

    $diag = $eaRepo.GetDiagramByID(27)
    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result with PDATA1=0: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
