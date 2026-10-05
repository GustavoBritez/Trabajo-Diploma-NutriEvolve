import win32com.client

def check_25_26():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        for did in [25, 26]:
            print(f"\n--- Diagram {did} Links ---")
            res = ea.SQLQuery(f"SELECT DiagramID, ConnectorID, Geometry, Style, Hidden FROM t_diagramlinks WHERE DiagramID = {did}")
            print(res[:1500])
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_25_26()
