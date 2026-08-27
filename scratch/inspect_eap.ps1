$conn = New-Object -ComObject "ADODB.Connection"
$dbPath = "c:\Users\Navegador\Desktop\td\Diagramas\TD.EAP"
$conn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=$dbPath;")

Write-Host "=== PACKAGES ==="
$rsPkg = $conn.Execute("SELECT Package_ID, Name, Parent_ID, ea_guid FROM t_package")
while (-not $rsPkg.EOF) {
    $pkgId = $rsPkg.Fields.Item("Package_ID").Value
    $pkgName = $rsPkg.Fields.Item("Name").Value
    $parentId = $rsPkg.Fields.Item("Parent_ID").Value
    $guid = $rsPkg.Fields.Item("ea_guid").Value
    Write-Host "Package ID: $pkgId | Parent: $parentId | Name: $pkgName | GUID: $guid"
    $rsPkg.MoveNext()
}

Write-Host "`n=== DIAGRAMS ==="
$rsDiag = $conn.Execute("SELECT Diagram_ID, Name, Diagram_Type, Package_ID, ea_guid FROM t_diagram")
while (-not $rsDiag.EOF) {
    $diagId = $rsDiag.Fields.Item("Diagram_ID").Value
    $diagName = $rsDiag.Fields.Item("Name").Value
    $diagType = $rsDiag.Fields.Item("Diagram_Type").Value
    $pkgId = $rsDiag.Fields.Item("Package_ID").Value
    $guid = $rsDiag.Fields.Item("ea_guid").Value
    Write-Host "Diagram ID: $diagId | Type: $diagType | Pkg: $pkgId | Name: $diagName | GUID: $guid"
    $rsDiag.MoveNext()
}

Write-Host "`n=== OBJECTS IN CU01 ==="
$rsObj = $conn.Execute("SELECT Object_ID, Name, Object_Type, Package_ID, Stereotype, ea_guid FROM t_object WHERE Package_ID IN (SELECT Package_ID FROM t_package WHERE Name LIKE '%CU01%' OR Name LIKE '%Registrar%') OR Name LIKE '%CU01%' OR Name LIKE '%Registrar%'")
while (-not $rsObj.EOF) {
    $objId = $rsObj.Fields.Item("Object_ID").Value
    $objName = $rsObj.Fields.Item("Name").Value
    $objType = $rsObj.Fields.Item("Object_Type").Value
    $pkgId = $rsObj.Fields.Item("Package_ID").Value
    $st = $rsObj.Fields.Item("Stereotype").Value
    Write-Host "Object ID: $objId | Type: $objType | Pkg: $pkgId | Name: $objName | St: $st"
    $rsObj.MoveNext()
}

$conn.Close()
