try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $objs = $ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype, Package_ID FROM t_object WHERE Name LIKE '*Auditar*' OR Name LIKE '*T06*' OR Name LIKE '*CUS8*'")
    Write-Output "--- T06 / CUS8 OBJECTS ---"
    Write-Output $objs

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
