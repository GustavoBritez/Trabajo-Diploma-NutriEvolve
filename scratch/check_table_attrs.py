import win32com.client

def check_tables():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        for tid in [430, 431, 435, 436]:
            el = ea.GetElementByID(tid)
            print(f"\nTable {tid}: {el.Name}")
            for a in el.Attributes:
                print(f"  {a.Name} : {a.Type} (PK: {a.IsOrdered}, Ster: {a.Stereotype})")
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_tables()
