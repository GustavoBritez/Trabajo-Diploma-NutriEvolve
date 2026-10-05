import win32com.client

def check_conn_pos():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        res = ea.SQLQuery("SELECT Connector_ID, Name, SubType, PtStartX, PtEndX, PtStartY, PtEndY FROM t_connector WHERE DiagramID = 12 AND Name LIKE '%TurnoBE%'")
        print("TurnoBE connectors in Diag 12:")
        print(res)

        res_obj = ea.SQLQuery("SELECT Object_ID, RectLeft, RectRight, RectTop, RectBottom FROM t_diagramobjects WHERE Diagram_ID = 12 AND Object_ID = 226")
        print("TurnoBE diagram object in Diag 12:")
        print(res_obj)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_conn_pos()
