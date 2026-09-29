import json
import os
import pypdf

# 1. Inspect Main App (ING-SOFTWARE-BritezG)
def inspect_main_app():
    base = r'C:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG'
    layers = ['BE', 'BLL', 'DAL', 'Services', 'UI']
    summary = {}
    for l in layers:
        l_path = os.path.join(base, l)
        summary[l] = []
        for root, dirs, files in os.walk(l_path):
            if 'obj' in root or 'bin' in root:
                continue
            for f in files:
                if f.endswith('.cs') or f.endswith('.sql') or f.endswith('.json') or f.endswith('.config'):
                    rel = os.path.relpath(os.path.join(root, f), l_path)
                    summary[l].append(rel)
    return summary

# 2. Inspect Diagramas Folder
def inspect_diagrams():
    base = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas'
    diag_files = {}
    for root, dirs, files in os.walk(base):
        rel = os.path.relpath(root, base)
        diag_files[rel] = files
    return diag_files

# 3. Read Prompts
def read_prompts():
    p1 = r'C:\Users\Danie\Desktop\GIT\TD\Promp - Turnos\Promp Para Turnos.txt'
    p2 = r'C:\Users\Danie\Desktop\GIT\TD\Promp - Enginner\Promp Para Turnos.txt'
    res = {}
    if os.path.exists(p1):
        with open(p1, 'r', encoding='utf-8', errors='ignore') as f:
            res['Promp_Turnos'] = f.read()
    if os.path.exists(p2):
        with open(p2, 'r', encoding='utf-8', errors='ignore') as f:
            res['Promp_Engineer'] = f.read()
    return res

# 4. Extract PDF Index / Sections
def inspect_thesis_pdf():
    pdf_path = r'C:\Users\Danie\Desktop\GIT\TD\Trabajo Diploma Britez.docx.pdf'
    reader = pypdf.PdfReader(pdf_path)
    toc_text = ""
    for i in range(len(reader.pages)):
        text = reader.pages[i].extract_text()
        if "Índice" in text or "Indice" in text or "Tabla de Contenido" in text or i < 6:
            toc_text += f"\n--- Page {i+1} ---\n" + text
    return {
        "pages_count": len(reader.pages),
        "toc_sample": toc_text
    }

main_app = inspect_main_app()
diag_folder = inspect_diagrams()
prompts = read_prompts()
thesis_info = inspect_thesis_pdf()

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\full_repo_details.json', 'w', encoding='utf-8') as f:
    json.dump({
        "main_app": main_app,
        "diag_folder": diag_folder,
        "prompts": prompts,
        "thesis_info": thesis_info
    }, f, indent=2, ensure_ascii=False)

print("Saved full_repo_details.json")
