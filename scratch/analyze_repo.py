import os
import glob
import json

def analyze_codebase(root_dir):
    code_summary = {
        "projects": [],
        "layers": {},
        "file_count": 0,
        "cs_files": [],
        "database_scripts": [],
        "forms": [],
        "models": [],
        "bll_services": [],
        "dal_repos": [],
        "security_services": [],
        "patterns": []
    }
    
    for root, dirs, files in os.walk(root_dir):
        if 'bin' in root or 'obj' in root or '.git' in root:
            continue
        for f in files:
            full_path = os.path.join(root, f)
            rel_path = os.path.relpath(full_path, root_dir)
            
            if f.endswith('.csproj'):
                code_summary["projects"].append(rel_path)
            elif f.endswith('.cs'):
                code_summary["file_count"] += 1
                code_summary["cs_files"].append(rel_path)
                if 'UI' in rel_path and ('Form' in f or 'Frm' in f or 'Designer.cs' in f or 'Menu' in f or 'Login' in f):
                    code_summary["forms"].append(rel_path)
                if 'BE' in rel_path:
                    code_summary["models"].append(rel_path)
                if 'BLL' in rel_path:
                    code_summary["bll_services"].append(rel_path)
                if 'DAL' in rel_path:
                    code_summary["dal_repos"].append(rel_path)
                if 'Services' in rel_path:
                    code_summary["security_services"].append(rel_path)
                if 'Patrones' in rel_path or 'Pattern' in rel_path or 'State' in rel_path:
                    code_summary["patterns"].append(rel_path)
            elif f.endswith('.sql'):
                code_summary["database_scripts"].append(rel_path)
                
    return code_summary

def analyze_diagrams_folder(diag_dir):
    res = {
        "files": [],
        "subdirs": {}
    }
    for root, dirs, files in os.walk(diag_dir):
        rel = os.path.relpath(root, diag_dir)
        res["subdirs"][rel] = [f for f in files if not f.endswith('.tmp')]
    return res

def analyze_prompts(prompts_dir):
    prompts = {}
    for root, dirs, files in os.walk(prompts_dir):
        for f in files:
            fp = os.path.join(root, f)
            with open(fp, 'r', encoding='utf-8', errors='ignore') as file_obj:
                prompts[f] = file_obj.read()
    return prompts

def analyze_pdf_toc(pdf_path):
    import pypdf
    reader = pypdf.PdfReader(pdf_path)
    text_sample = ""
    for i in range(min(5, len(reader.pages))):
        text_sample += f"\n--- Page {i+1} ---\n" + reader.pages[i].extract_text()
    return {
        "pages": len(reader.pages),
        "text_sample": text_sample
    }

base_dir = r'C:\Users\Danie\Desktop\GIT\TD'
code_data = analyze_codebase(os.path.join(base_dir, 'ING-SOFTWARE-BritezG'))
diag_data = analyze_diagrams_folder(os.path.join(base_dir, 'Diagramas'))
promp_turnos = analyze_prompts(os.path.join(base_dir, 'Promp - Turnos'))
promp_eng = analyze_prompts(os.path.join(base_dir, 'Promp - Enginner'))

full_summary = {
    "codebase": code_data,
    "diagrams_folder": diag_data,
    "prompts_turnos": promp_turnos,
    "prompts_engineer": promp_eng
}

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\repo_complete_analysis.json', 'w', encoding='utf-8') as f:
    json.dump(full_summary, f, indent=2, ensure_ascii=False)

print("Analysis of folder structure complete.")
print(f"Projects found: {code_data['projects']}")
print(f"Total C# files: {code_data['file_count']}")
print(f"Forms: {len(code_data['forms'])}")
print(f"Models: {len(code_data['models'])}")
print(f"BLL classes: {len(code_data['bll_services'])}")
print(f"DAL classes: {len(code_data['dal_repos'])}")
print(f"Services classes: {len(code_data['security_services'])}")
print(f"Patterns: {len(code_data['patterns'])}")
