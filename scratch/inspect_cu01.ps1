$conn = New-Object -ComObject "ADODB.Connection"
$dbPath = "c:\Users\Navegador\Desktop\td\Diagramas\TD.EAP"
$conn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=$dbPath;")

Write-Host "=== OBJECTS in Package 4 (Primary Use Cases) ==="
$rsObj = $conn.Execute("SELECT Object_ID, Name, Object_Type, Package_ID, ParentID, Stereotype, ea_guid FROM t_object WHERE Package_ID = 4 OR ParentID = 12")
while (-not $rsObj.EOF) {
    $oId = $rsObj.Fields.Item("Object_ID").Value
    $oName = $rsObj.Fields.Item("Name").Value
    $oType = $rsObj.Fields.Item("Object_Type").Value
    $pId = $rsObj.Fields.Item("Package_ID").Value
    $parId = $rsObj.Fields.Item("ParentID").Value
    Write-Host "ID: $oId | Type: $oType | ParentID: $parId | Name: $oName"
    $rsObj.MoveNext()
}

Write-Host "`n=== DIAGRAMS in Package 4 or under Object 12 ==="
$rsDiag = $conn.Execute("SELECT Diagram_ID, Name, Diagram_Type, Package_ID, ParentID, ea_guid FROM t_diagram WHERE Package_ID = 4 OR ParentID = 12")
while (-not $rsDiag.EOF) {
    $dId = $rsDiag.Fields.Item("Diagram_ID").Value
    $dName = $rsDiag.Fields.Item("Name").Value
    $dType = $rsDiag.Fields.Item("Diagram_Type").Value
    $pId = $rsDiag.Fields.Item("Package_ID").Value
    $parId = $rsDiag.Fields.Item("ParentID").Value
    Write-Host "ID: $dId | Type: $dType | Package: $pId | ParentID: $parId | Name: $dName"
    $rsDiag.MoveNext()
}

Write-Host "`n=== DIAGRAM OBJECTS FOR DIAGRAM 12 (CU01 Sequence) ==="
$rsDO = $conn.Execute("SELECT d.Diagram_ID, do.Object_ID, o.Name, o.Object_Type FROM t_diagramobjects do INNER JOIN t_diagram d ON do.Diagram_ID = d.Diagram_ID INNER JOIN t_object o ON do.Object_ID = o.Object_ID WHERE d.Diagram_ID = 12")
while (-not $rsDO.EOF) {
    $oId = $rsDO.Fields.Item("Object_ID").Value
    $oName = $rsDO.Fields.Item("Name").Value
    $oType = $rsDO.Fields.Item("Object_Type").Value
    Write-Host "Diagram 12 Object: ID $oId | Name: $oName | Type: $oType"
    $rsDO.MoveNext()
}

$conn.Close()
