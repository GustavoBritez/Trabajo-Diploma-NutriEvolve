import pypdf
import re

def extract_sections(pdf_path, keywords):
    reader = pypdf.PdfReader(pdf_path)
    results = []
    for idx, page in enumerate(reader.pages):
        text = page.extract_text()
        for kw in keywords:
            if kw.lower() in text.lower():
                results.append(f"--- {pdf_path} | Page {idx+1} (Keyword: {kw}) ---\n" + text)
                break
    return results

r1 = extract_sections(r'C:\Users\Danie\Desktop\GIT\TD\Trabajo Diploma Britez.docx.pdf', ['N01', 'PN1', 'Proceso #1', 'Secuencia General', 'Turnero Nutricional', 'Actor Activo', 'Actor Pasivo'])
with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\n01_thesis_text.txt', 'w', encoding='utf-8') as f:
    f.write("\n\n".join(r1))

print(f"Extracted {len(r1)} pages from thesis PDF matching N01/PN1 keywords.")
