try {
    $eaRepo = New-Object -ComObject EA.Repository
    $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    
    function Dump-Package($pkg, $indent) {
        Write-Output "$indent Package: $($pkg.Name) (ID: $($pkg.PackageID))"
        foreach ($diag in $pkg.Diagrams) {
            Write-Output "$indent   [Diagram] $($diag.Name) (Type: $($diag.Type), ID: $($diag.DiagramID))"
        }
        foreach ($elem in $pkg.Elements) {
            Write-Output "$indent   <Element> $($elem.Name) (Type: $($elem.Type), ID: $($elem.ElementID))"
            foreach ($subdiag in $elem.Diagrams) {
                Write-Output "$indent     [SubDiagram] $($subdiag.Name) (Type: $($subdiag.Type), ID: $($subdiag.DiagramID))"
            }
        }
        foreach ($subpkg in $pkg.Packages) {
            Dump-Package $subpkg "$indent  "
        }
    }
    
    foreach ($m in $eaRepo.Models) {
        Dump-Package $m ""
    }
    
    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
