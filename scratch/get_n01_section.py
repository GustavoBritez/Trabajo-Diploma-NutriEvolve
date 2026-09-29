import docx

doc = docx.Document(r'C:\Users\Danie\Desktop\GIT\TD\TD.docx')

recording = False
n01_lines = []

for i, p in enumerate(doc.paragraphs):
    text = p.text.strip()
    if 'N01' in text or 'N01 Especificaci' in text:
        recording = True
    elif recording and (p.text.startswith('G06') or p.text.startswith('T01') or p.text.startswith('G07')):
        recording = False
    
    if recording:
        n01_lines.append(f"[P{i} | {p.style.name}] {text}")

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\n01_full_section.txt', 'w', encoding='utf-8') as f:
    f.write("\n".join(n01_lines))

print(f"Captured {len(n01_lines)} lines for N01 section.")
