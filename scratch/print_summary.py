import json

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\full_repo_details.json', 'r', encoding='utf-8') as f:
    repo = json.load(f)

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\eap_detailed_analysis.json', 'r', encoding='utf-8') as f:
    eap = json.load(f)

print("=== MAIN APP LAYERS ===")
for layer, files in repo['main_app'].items():
    print(f"\n--- {layer} ({len(files)} files) ---")
    for fl in files:
        print(f"  • {fl}")

print("\n=== DIAGRAMAS FOLDER CONTENTS ===")
for dirpath, files in repo['diag_folder'].items():
    print(f"Dir: {dirpath} -> {files}")

print("\n=== PROMPTS SNIPPET ===")
for k, v in repo['prompts'].items():
    print(f"Prompt {k} length: {len(v)}")
    print(f"Snippet: {v[:200]}...\n")

print("\n=== THESIS PDF SAMPLE ===")
print(repo['thesis_info']['toc_sample'][:1000])

print("\n=== EAP PACKAGES & DIAGRAMS ===")
for p in eap['packages']:
    print(f"Package ID {p['Package_ID']}: {p['Name']} (Parent: {p.get('Parent_ID')})")

print("\nDiagrams:")
for d in eap['diagrams']:
    print(f"  [{d['Diagram_Type']}] {d['Name']} (ID: {d['Diagram_ID']}, Package: {d.get('PackageName')}) - Elements: {len(d.get('objects', []))}")

print("\nUse Cases:")
for u in eap['use_cases']:
    print(f"  • {u['Name']} - Status: {u.get('Status')} - Notes: {u.get('Note')}")

print("\nClasses:")
for c in eap['classes']:
    attrs = [a['Name'] for a in c.get('attributes', [])]
    ops = [o['Name'] for o in c.get('operations', [])]
    print(f"  • {c['Name']} (Stereotype: {c.get('Stereotype')}) | Attrs: {attrs} | Methods: {ops}")
