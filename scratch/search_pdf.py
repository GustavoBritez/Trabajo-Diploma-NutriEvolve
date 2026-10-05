import pypdf

reader = pypdf.PdfReader(r"C:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\Proyecto ING. Software.pdf")
print("Searching case-insensitively in PDF...")
matches = []
for idx, page in enumerate(reader.pages):
    text = page.extract_text() or ""
    t_lower = text.lower()
    if "proceso de negocio 2" in t_lower or "pn2" in t_lower or "seguimiento nutricional" in t_lower or "cun06" in t_lower or "cun6" in t_lower:
        matches.append((idx+1, text))

print(f"Found {len(matches)} matching pages.")
for p_num, text in matches[:5]:
    print(f"\n=================== PAGE {p_num} ===================")
    print(text[:1000])
