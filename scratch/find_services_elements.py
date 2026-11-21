import win32com.client
import sys

ea = None
try:
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

    search_names = [
        "UsuarioBE", "BitacoraBE", "EventoBE", "Idioma", "IIdiomaObservable", "IIdiomaObserver",
        "ServicesSessionManager", "ServicioCriptografiaAES", "ServicioBcrypt", "Perfil",
        "FamiliaServices", "PatenteServices",
        "UsuarioBLL", "BitacoraBLL", "DigitoVerificadorBLL", "IdiomaBLL", "BackupBLL",
        "PerfilBLL", "FamiliaBLL", "PatenteBLL"
    ]

    print("=== SEARCHING FOR SERVICES ELEMENTS ===", flush=True)
    all_els = []
    def check_pkg(p):
        for e in p.Elements:
            for sname in search_names:
                if e.Name.lower() == sname.lower():
                    all_els.append((sname, e.ElementID, e.Type, e.Stereotype, p.PackageID, p.Name))
        for sp in p.Packages:
            check_pkg(sp)

    for m in ea.Models:
        check_pkg(m)

    found_names = set()
    for sname, eid, etype, estereo, pid, pname in all_els:
        found_names.add(sname.lower())
        print(f"FOUND: {sname:<25} -> ID={eid:<5} TYPE={etype:<10} STEREO={estereo:<15} PKG={pid} ({pname})", flush=True)

    for sname in search_names:
        if sname.lower() not in found_names:
            print(f"NOT FOUND: {sname}", flush=True)

finally:
    if ea:
        try:
            ea.CloseFile()
            ea.Exit()
        except:
            pass
