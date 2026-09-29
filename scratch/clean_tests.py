import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

# Delete test diagrams 38 through 50
test_diagram_ids = [38, 39, 40, 41, 42, 43, 44, 46, 47, 48, 49, 50]
for did in test_diagram_ids:
    ea.Execute(f"DELETE FROM t_diagram WHERE Diagram_ID = {did}")
    ea.Execute(f"DELETE FROM t_diagramobjects WHERE Diagram_ID = {did}")
    ea.Execute(f"DELETE FROM t_diagramlinks WHERE DiagramID = {did}")
    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {did}")

# Delete orphan test connectors between 220 and 38 (except 1270 which is Extends)
ea.Execute("DELETE FROM t_connector WHERE Connector_Type = 'Sequence' AND DiagramID NOT IN (12, 13, 33, 34, 35)")

print("Cleaned all test diagrams and orphan connectors")

ea.CloseFile()
