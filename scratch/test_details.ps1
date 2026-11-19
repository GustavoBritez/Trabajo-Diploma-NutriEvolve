try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $bll = $ea.GetElementByID(757)
    Write-Output "Elem 757: $($bll.Name)"

    $pkg = $ea.GetPackageByID(16)
    foreach ($e in $pkg.Elements) {
        Write-Output "Elem in pkg16: $($e.Name)"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message) at line $($_.InvocationInfo.ScriptLineNumber)"
}
