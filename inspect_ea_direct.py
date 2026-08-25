"""
Conecta directamente al .EAP como base de datos Access via ADODB (sin EA COM).
EA debe estar CERRADO para poder abrir el archivo directamente.
"""
import win32com.client
import subprocess, sys, time

# Verificar si EA esta corriendo y cerrarlo si es necesario
result = subprocess.run(['tasklist', '/FI', 'IMAGENAME eq EA.exe'], capture_output=True, text=True)
if 'EA.exe' in result.stdout:
    print("EA.exe esta corriendo. Cerrando...")
    subprocess.run(['taskkill', '/F', '/IM', 'EA.exe'], capture_output=True)
    time.sleep(2)
    print("EA cerrado.")
else:
    print("EA no esta corriendo. Conectando directamente al .EAP...")

EAP_PATH = r"c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"

# Conectar via ADODB directamente al archivo Access
adodb = win32com.client.Dispatch("ADODB.Connection")
try:
    # Intentar con Jet 4.0 (Access 97-2003 .mdb)
    conn_str = f"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={EAP_PATH};Mode=ReadWrite;"
    adodb.Open(conn_str)
    print("Connected via Jet OLEDB 4.0!")
except Exception as e1:
    print(f"Jet 4.0 failed: {e1}")
    try:
        # Intentar con ACE 12.0 (Access 2007+)
        conn_str = f"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={EAP_PATH};Mode=ReadWrite;"
        adodb.Open(conn_str)
        print("Connected via ACE OLEDB 12.0!")
    except Exception as e2:
        print(f"ACE 12.0 failed: {e2}")
        sys.exit(1)

# Leer schema completo de t_connector
rs = win32com.client.Dispatch("ADODB.Recordset")
rs.Open("SELECT TOP 1 * FROM t_connector", adodb, 1, 1)

print("\n=== t_connector ALL COLUMN NAMES ===")
fields = []
for i in range(rs.Fields.Count):
    f = rs.Fields.Item(i)
    fields.append(f.Name)
    print(f"  [{i:02d}] {f.Name:30s} Type={f.Type} Value={repr(f.Value)}")

rs.Close()

# Buscar columnas relacionadas con IsReturn / Lifecycle / SubType
print("\n=== RELEVANT FIELDS (IsReturn, SubType, Ptore, PDATA) ===")
for f in fields:
    fl = f.lower()
    if any(kw in fl for kw in ['subtype','ptore','pdata','style','return','lifecycle','flag','kind','synch']):
        print(f"  -> {f}")

# Ahora buscar conectores de CU01 (ParentID=12 del element que creo las lifelines)
# Pero primero necesitamos saber los IDs de las lifelines
print("\n=== Searching for CU01 child elements ===")
rs2 = win32com.client.Dispatch("ADODB.Recordset")
rs2.Open("SELECT Object_ID, Name, Object_Type FROM t_object WHERE ParentID = 12", adodb, 1, 1)
elem_ids = []
while not rs2.EOF:
    oid = rs2.Fields.Item("Object_ID").Value
    name = rs2.Fields.Item("Name").Value
    otype = rs2.Fields.Item("Object_Type").Value
    print(f"  Object_ID={oid}  Name={name}  Type={otype}")
    elem_ids.append(oid)
    rs2.MoveNext()
rs2.Close()

if elem_ids:
    # Buscar conectores de esos elementos
    ids_str = ','.join(str(i) for i in elem_ids[:5])
    print(f"\n=== t_connector for first 5 lifelines ({ids_str}) ===")
    rs3 = win32com.client.Dispatch("ADODB.Recordset")
    query = f"SELECT Connector_ID, Name, Connector_Type, SubType, StyleEx, PDATA1, PDATA2, PDATA3, PDATA4, PDATA5 FROM t_connector WHERE Start_Object_ID IN ({ids_str}) OR End_Object_ID IN ({ids_str})"
    rs3.Open(query, adodb, 1, 1)
    count = 0
    while not rs3.EOF and count < 10:
        cid = rs3.Fields.Item("Connector_ID").Value
        name = rs3.Fields.Item("Name").Value
        ctype = rs3.Fields.Item("Connector_Type").Value
        subtype = rs3.Fields.Item("SubType").Value
        style = rs3.Fields.Item("StyleEx").Value
        p1 = rs3.Fields.Item("PDATA1").Value
        p2 = rs3.Fields.Item("PDATA2").Value
        p4 = rs3.Fields.Item("PDATA4").Value
        print(f"  CID={cid} Name='{name}' SubType='{subtype}' StyleEx='{style}' PDATA1='{p1}' PDATA2='{p2}' PDATA4='{p4}'")
        rs3.MoveNext()
        count += 1
    rs3.Close()

adodb.Close()
print("\nDone! Schema investigation complete.")
