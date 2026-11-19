try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $pkgBitacora = $ea.GetPackageByID(16)
    Write-Output "Found package 16: $($pkgBitacora.Name)"

    # Check Elements count in pkgBitacora
    Write-Output "Elements in pkg: $($pkgBitacora.Elements.Count)"

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message) at line $($_.InvocationInfo.ScriptLineNumber)"
}
