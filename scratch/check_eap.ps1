$conn = New-Object -ComObject ADODB.Connection
try {
    $conn.Open("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP;")
} catch {
    $conn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP;")
}
$rs = $conn.Execute("SELECT Diagram_ID, Diagram_Type, Name, Package_ID FROM t_diagram")
while (-not $rs.EOF) {
    Write-Output ($rs.Fields.Item("Diagram_ID").Value.ToString() + " | " + $rs.Fields.Item("Diagram_Type").Value + " | " + $rs.Fields.Item("Name").Value)
    $rs.MoveNext()
}
$conn.Close()
