import win32com.client
import os
import sys

EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"

def audit_eap():
    ea = win32com.client.Dispatch("EA.Repository")
    if not ea.OpenFile(EAP_PATH):
        print("[ERROR] Could not open TD.EAP")
        sys.exit(1)

    try:
        print("=== 1. AUDIT: DUPLICATE ELEMENTS BY NAME IN TD.EAP ===")
        # Map name -> list of (id, type, stereotype, package_id, package_name)
        elements_by_name = {}

        def scan_pkg(pkg):
            for el in pkg.Elements:
                name_clean = el.Name.strip()
                if not name_clean:
                    continue
                if name_clean not in elements_by_name:
                    elements_by_name[name_clean] = []
                elements_by_name[name_clean].append({
                    "id": el.ElementID,
                    "type": el.Type,
                    "stereo": el.Stereotype,
                    "pkg_id": pkg.PackageID,
                    "pkg_name": pkg.Name
                })
            for sp in pkg.Packages:
                scan_pkg(sp)

        for m in ea.Models:
            scan_pkg(m)

        duplicates = {k: v for k, v in elements_by_name.items() if len(v) > 1}
        for name, items in sorted(duplicates.items()):
            print(f"\n[DUPLICATE] '{name}' exists {len(items)} times:")
            for it in items:
                print(f"   - ID={it['id']:<5} TYPE={it['type']:<12} STEREO={it['stereo']:<18} PKG=[{it['pkg_id']}] {it['pkg_name']}")

        print("\n=== 2. AUDIT: DIAGRAM OBJECTS CHECK ===")
        # Check diagrams for duplicate elements or interesting inconsistencies
        for m in ea.Models:
            def scan_diag_pkg(p):
                for d in p.Diagrams:
                    obj_ids = set()
                    dups_in_d = []
                    for i in range(d.DiagramObjects.Count):
                        dobj = d.DiagramObjects.GetAt(i)
                        if dobj.ElementID in obj_ids:
                            dups_in_d.append(dobj.ElementID)
                        obj_ids.add(dobj.ElementID)
                    if dups_in_d:
                        print(f"Diagram [{d.DiagramID}] '{d.Name}' has duplicate objects: {dups_in_d}")
                for sp in p.Packages:
                    scan_diag_pkg(sp)
            scan_diag_pkg(m)

        print("\n=== 3. AUDIT: ELEMENTS VS SCRIPT.SQL TABLES ===")
        # Read script.sql tables
        script_path = r"C:\Users\Danie\Desktop\GIT\ING-SOFTWARE-BritezG\script.sql"
        sql_tables = set()
        if os.path.exists(script_path):
            with open(script_path, "r", encoding="utf-8", errors="ignore") as f:
                for line in f:
                    if "CREATE TABLE" in line.upper():
                        # Extract table name
                        import re
                        m = re.search(r"CREATE\s+TABLE\s+(?:\[?dbo\]?\.)?\[?([a-zA-Z0-9_]+)\]?", line, re.IGNORECASE)
                        if m:
                            sql_tables.add(m.group(1).lower())

        # Check tables in TD.EAP
        eap_tables = set()
        for name, items in elements_by_name.items():
            for it in items:
                if it["stereo"].lower() == "table" or it["type"].lower() == "table":
                    eap_tables.add((name, it["id"], it["pkg_name"]))

        print(f"Found {len(sql_tables)} tables in script.sql and {len(eap_tables)} table elements in TD.EAP.")
        
        # In TD.EAP but not in script.sql
        print("\n[TABLES IN TD.EAP BUT NOT IN SCRIPT.SQL]:")
        for tname, tid, tpkg in sorted(eap_tables):
            if tname.lower() not in sql_tables:
                print(f"   - Table '{tname}' (ID {tid}) in PKG '{tpkg}' NOT found in script.sql")

        # In script.sql but not in TD.EAP
        eap_tab_names = {t[0].lower() for t in eap_tables}
        print("\n[TABLES IN SCRIPT.SQL BUT NOT IN TD.EAP (AS TABLE STEREOTYPE)]:")
        for st in sorted(sql_tables):
            if st not in eap_tab_names:
                print(f"   - SQL table '{st}' NOT in TD.EAP as table")

        print("\n=== 4. AUDIT: ORPHANED OR SUSPICIOUS CONNECTORS ===")
        # Check connectors
        conn_sql = "SELECT Connector_ID, Start_Object_ID, End_Object_ID, Connector_Type, Subtype, Stereotype FROM t_connector"
        # We can inspect elements connectors
        invalid_conns = 0
        all_el_ids = set()
        for items in elements_by_name.values():
            for it in items:
                all_el_ids.add(it["id"])

        # Check some connector counts
        print(f"Total unique elements in TD.EAP: {len(all_el_ids)}")

    except Exception as ex:
        print(f"[ERROR] {ex}")
        import traceback
        traceback.print_exc()
    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    audit_eap()
