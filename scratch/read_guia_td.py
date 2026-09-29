import pypdf

pdf_path = r'C:\Users\Danie\Desktop\GIT\TD\Guia de Realizacion\Guia_Realizacion_TD.pdf'
reader = pypdf.PdfReader(pdf_path)

print(f"Total pages: {len(reader.pages)}")

full_text = []
for idx, page in enumerate(reader.pages):
    text = page.extract_text()
    full_text.append(f"=== PAGE {idx+1} ===\n{text}\n")

output_path = r'C:\Users\Danie\Desktop\GIT\TD\scratch\guia_td_text.txt'
with open(output_path, 'w', encoding='utf-8') as f:
    f.writelines(full_text)

print("Saved full text of Guia_Realizacion_TD.pdf to scratch/guia_td_text.txt")
