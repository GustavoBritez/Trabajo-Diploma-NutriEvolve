try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $p1 = $eaRepo.GetElementByID(499)
    $p2 = $eaRepo.GetElementByID(500)

    Write-Output "P1: Name='$($p1.Name)', Type='$($p1.Type)', Subtype=$($p1.Subtype), StyleEx='$($p1.StyleEx)', TaggedValues=$($p1.TaggedValues.Count)"

    # In EA, for ActivityPartition to be vertical (header at top):
    # Set Subtype or TaggedValues or StyleEx
    # In EA UML 2 Activity Partition:
    # Subtype: 0 or 1, or Tagged value 'isDimension'/'isExternal', or StyleEx 'isVertical=1;'
    # In EA, setting StyleEx = "isVertical=1;" or "vertical=1;" or Subtype
    $p1.StyleEx = "isVertical=1;"
    $p1.Update()
    $p2.StyleEx = "isVertical=1;"
    $p2.Update()

    # Also let's check t_object direct update via SQL if needed
    $eaRepo.Execute("UPDATE t_object SET StyleEx = 'isVertical=1;', PDATA1 = '1' WHERE Object_ID IN (499, 500)")

    $diag = $eaRepo.GetDiagramByID(27)
    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # Export diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Exported diagram with vertical header partitions: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
