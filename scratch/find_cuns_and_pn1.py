import win32com.client

def find_diagrams():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        res = ea.SQLQuery("SELECT Diagram_ID, Package_ID, ParentID, Name, Diagram_Type FROM t_diagram ORDER BY Diagram_ID")
        print("ALL DIAGRAMS IN TD.EAP:")
        print(res)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    find_diagrams()
