try {
    $eaRepo = New-Object -ComObject EA.Repository
    $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    
    $project = $eaRepo.GetProjectInterface()
    $diag = $eaRepo.GetDiagramByID(27)
    $diagGUID = $diag.DiagramGUID
    
    $outPath = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.SaveDiagramImageToFile($diagGUID, $outPath)
    Write-Output "Diagram image saved: $res to $outPath"
    
    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
