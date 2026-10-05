import win32com.client

def check_12_28():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        res = ea.SQLQuery("SELECT Connector_ID, Name, SubType, PtStartX, PtEndX, PtStartY, PtEndY FROM t_connector WHERE DiagramID = 12 AND SeqNo = 28")
        print("Diag 12 Seq 28:")
        print(res)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_12_28()
