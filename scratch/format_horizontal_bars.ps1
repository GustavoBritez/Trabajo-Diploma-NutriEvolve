try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # 1. Configure Fork and Join: Clear name so no text shows, set horizontal bar orientation
    $fork = $eaRepo.GetElementByID(495)
    $join = $eaRepo.GetElementByID(496)

    $fork.Name = ""
    $fork.StyleEx = "horiz=1;"
    $fork.Update()

    $join.Name = ""
    $join.StyleEx = "horiz=1;"
    $join.Update()

    # In EA database for Horizontal Synchronization bar: PDATA1 = 'H' or 'horizontal'
    $eaRepo.Execute("UPDATE t_object SET Name = '', PDATA1 = 'H', StyleEx = 'horiz=1;' WHERE Object_ID IN (495, 496)")

    # 2. Update layout of DiagramObjects
    foreach ($dObj in $diag.DiagramObjects) {
        if ($dObj.ElementID -eq 495) {
            $dObj.left = 520
            $dObj.right = 1060
            $dObj.top = -990
            $dObj.bottom = -998
            $dObj.Update()
        } elseif ($dObj.ElementID -eq 496) {
            $dObj.left = 520
            $dObj.right = 1060
            $dObj.top = -1190
            $dObj.bottom = -1198
            $dObj.Update()
        }
    }
    $diag.DiagramObjects.Refresh()

    # 3. Clean routing for return lines
    foreach ($dLink in $diag.DiagramLinks) {
        $c = $eaRepo.GetConnectorByID($dLink.ConnectorID)
        if ($c.ClientID -eq 453 -and $c.SupplierID -eq 450) {
            # Route through right and top
            $dLink.Style = "Mode=1;"
            $dLink.Geometry = "EDGE=2;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=1115:-752;1115:-635;260:-635;"
            $dLink.Update()
        } elseif ($c.ClientID -eq 455 -and $c.SupplierID -eq 450) {
            $dLink.Style = "Mode=1;"
            $dLink.Geometry = "EDGE=2;$LLB=;LLT=;LMT=;LMB=;LRT=;LRB=;IRHS=;ILHS=;Path=1125:-842;1125:-625;260:-625;"
            $dLink.Update()
        }
    }
    $diag.DiagramLinks.Refresh()

    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # 4. Export diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result with horizontal synchronization bars: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
