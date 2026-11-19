try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $elem = $ea.GetElementByID(757) # PacienteBLL_DNI101
    Write-Output "Class: $($elem.Name) | Type: $($elem.Type) | Stereotype: $($elem.Stereotype)"
    Write-Output "Methods ($($elem.Methods.Count)):"
    foreach ($m in $elem.Methods) {
        Write-Output "  - Meth: $($m.Name) : $($m.ReturnType)"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
