try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $diag = $eaRepo.GetDiagramByID(26)
    Write-Output "Original StyleEx: $($diag.StyleEx)"

    # Change connector notation to Information Engineering (Crow's Foot / Pata de Gallo)
    if ($diag.StyleEx -match "TConnectorNotation=[^;]+;") {
        $diag.StyleEx = $diag.StyleEx -replace "TConnectorNotation=[^;]+;", "TConnectorNotation=Information Engineering;"
    } else {
        $diag.StyleEx = $diag.StyleEx + ";TConnectorNotation=Information Engineering;"
    }

    # Also clean connector names to avoid redundant text over Crow's foot
    foreach ($dLink in $diag.DiagramLinks) {
        $c = $eaRepo.GetConnectorByID($dLink.ConnectorID)
        # Keep clean role name (e.g. 'es tutor de', 'posee', 'atiende')
        if ($c.Name -match "\(.*\)") {
            $c.Name = ($c.Name -replace "^\d+\s*:\s*[N\d\.]+\s*", "") -replace "[\(\)]", ""
            $c.Update()
        }
    }

    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # Export high-res diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Entidad Relacion\CU01 DER (Modelo Relacional).png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Exported DER with Crow's Foot (Pata de Gallo) notation: $res to $outPng"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Completed successfully!"
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
