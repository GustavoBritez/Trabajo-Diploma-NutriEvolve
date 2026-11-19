try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { 
        Write-Output "Error opening file"
        exit 1 
    }

    $pkg = $ea.GetPackageByID(4)
    Write-Output "Package 4: $($pkg.Name)"

    # Test checking existing diagrams in pkg 4
    Write-Output "Existing diagrams count: $($pkg.Diagrams.Count)"

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
