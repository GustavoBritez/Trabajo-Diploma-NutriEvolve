try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $objs = $ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype FROM t_object WHERE Name LIKE '*GUI*' OR Object_Type LIKE '*Screen*' OR Stereotype LIKE '*GUI*' OR Stereotype LIKE '*screen*'")
    Write-Output "--- GUI OBJECTS ---"
    Write-Output $objs

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
