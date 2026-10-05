import win32com.client

def check_736():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        el = ea.GetElementByID(736)
        print("Element 736 GUID:", el.ElementGUID)
        res = ea.SQLQuery(f"SELECT * FROM t_xref WHERE Client = '{el.ElementGUID}'")
        print("XREF for 736:", res)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_736()
