import os
import re

base_dir = r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG"

def audit_code():
    print("=== STARTING AUTOMATED AUDIT OF C# CODEBASE ===")
    
    cs_files = []
    for root, dirs, files in os.walk(base_dir):
        if "obj" in root or "bin" in root or "Properties" in root:
            continue
        for f in files:
            if f.endswith(".cs") and not f.endswith(".Designer.cs"):
                cs_files.append(os.path.join(root, f))
                
    print(f"Total C# files found (excluding Designer and build artifacts): {len(cs_files)}")
    
    # 1. UI calling DAL directly
    ui_dal_calls = []
    for f in cs_files:
        if "\\UI\\" in f:
            content = open(f, encoding='utf-8', errors='ignore').read()
            # check if imports DAL or instantiates *DAL
            dal_inst = re.findall(r'new\s+([A-Z]\w*DAL\w*)\s*\(', content)
            if dal_inst:
                ui_dal_calls.append((os.path.basename(f), dal_inst))
                
    print("\n--- 1. Architecture: Direct UI -> DAL instantiations ---")
    if ui_dal_calls:
        for u, d in ui_dal_calls:
            print(f"  [VIOLATION] {u} directly instantiates DAL: {set(d)}")
    else:
        print("  [OK] No UI -> DAL direct instantiations found!")

    # 2. BLL calling UI
    bll_ui_calls = []
    for f in cs_files:
        if "\\BLL\\" in f:
            content = open(f, encoding='utf-8', errors='ignore').read()
            if "using System.Windows.Forms;" in content or "MessageBox." in content:
                bll_ui_calls.append(os.path.basename(f))
                
    print("\n--- 2. Architecture: BLL coupling with UI (MessageBox / Windows.Forms) ---")
    if bll_ui_calls:
        for b in bll_ui_calls:
            print(f"  [WARNING] {b} uses Windows.Forms or MessageBox directly in BLL!")
    else:
        print("  [OK] BLL is clean of UI dependencies!")

    # 3. Connection String storage
    hardcoded_conns = []
    for f in cs_files:
        content = open(f, encoding='utf-8', errors='ignore').read()
        if "Data Source=" in content:
            hardcoded_conns.append(os.path.basename(f))
            
    print("\n--- 3. Hardcoded Connection Strings ---")
    for h in hardcoded_conns:
        print(f"  [FINDING] Hardcoded connection string in: {h}")

    # 4. Check BCrypt and Password security
    print("\n--- 4. Password and Security Inspection ---")
    for f in cs_files:
        content = open(f, encoding='utf-8', errors='ignore').read()
        if "BCrypt" in content or "HashPassword" in content or "Verify" in content:
            print(f"  Found hashing reference in: {os.path.basename(f)}")

    # 5. Check Exception Silencing
    print("\n--- 5. Empty Catch Blocks Summary ---")
    catch_count = 0
    for f in cs_files:
        content = open(f, encoding='utf-8', errors='ignore').read()
        empty_catches = len(re.findall(r'catch\s*(\([^\)]*\))?\s*\{\s*\}', content))
        if empty_catches > 0:
            catch_count += empty_catches
            print(f"  {os.path.basename(f)}: {empty_catches} empty catch block(s)")
    print(f"Total silent catch blocks in solution: {catch_count}")

audit_code()
