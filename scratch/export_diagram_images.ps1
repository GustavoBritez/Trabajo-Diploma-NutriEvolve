try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $outDir = "C:\Users\Navegador\Desktop\GIT\Diagramas\Exported"
    if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir | Out-Null }

    $project = $ea.GetProjectInterface()

    foreach ($dId in @(119, 120, 121, 122)) {
        $diag = $ea.GetDiagramByID($dId)
        $cleanName = ($diag.Name -replace "[:/\\]", "_")
        $imgFile = Join-Path $outDir "$cleanName.png"
        $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $imgFile, 1)
        Write-Output "Exported Diagram $dId ($cleanName) to $imgFile : Result = $res"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
