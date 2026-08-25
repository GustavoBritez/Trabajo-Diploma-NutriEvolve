import win32com.client
import xml.etree.ElementTree as ET

ea_app = win32com.client.Dispatch("EA.App")
repo = ea_app.Repository
repo.OpenFile(r"c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
print("Opened TD.EAP!")

# Obtener TODOS los campos del conector 471 para ver que columnas existen
res = repo.SQLQuery("SELECT * FROM t_connector WHERE Connector_ID = 471")
print("ALL fields of connector 471:")
print(res)
print()

# Probar campo PDATA1 para Lifecycle
try:
    repo.Execute("UPDATE t_connector SET PDATA1 = '1' WHERE Connector_ID = 472")
    print("PDATA1 UPDATE on 472 worked!")
    res2 = repo.SQLQuery("SELECT Connector_ID, SubType, PDATA1, StyleEx FROM t_connector WHERE Connector_ID = 472")
    print(res2)
except Exception as e:
    print(f"PDATA1 UPDATE failed: {e}")

# Probar campo StyleEx con LifeCycle tag
try:
    repo.Execute("UPDATE t_connector SET StyleEx = 'LifeCycle=1;' WHERE Connector_ID = 473")
    print("\nStyleEx LifeCycle=1 UPDATE on 473 worked!")
    res3 = repo.SQLQuery("SELECT Connector_ID, SubType, StyleEx FROM t_connector WHERE Connector_ID = 473")
    print(res3)
except Exception as e:
    print(f"\nStyleEx LifeCycle UPDATE failed: {e}")

# Probar SubType = 'Create' (lo que EA usa internamente para New)
# Segun la imagen, cuando es New en Lifecycle, SubType en BD puede ser diferente
# Probar con StyleEx = 'IsCreate=1;'
try:
    repo.Execute("UPDATE t_connector SET StyleEx = 'IsCreate=1;' WHERE Connector_ID = 474")
    print("\nStyleEx IsCreate=1 UPDATE on 474 worked!")
    res4 = repo.SQLQuery("SELECT Connector_ID, SubType, StyleEx FROM t_connector WHERE Connector_ID = 474")
    print(res4)
except Exception as e:
    print(f"\nStyleEx IsCreate UPDATE failed: {e}")
