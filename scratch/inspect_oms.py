import pypdf

reader = pypdf.PdfReader(r"C:\Users\Danie\Desktop\GIT\TD\curvas_oms.pdf")
print(f"curvas_oms.pdf has {len(reader.pages)} pages")
for idx, page in enumerate(reader.pages):
    print(f"\n--- Page {idx+1} ---")
    txt = page.extract_text() or ""
    lines = txt.split("\n")
    print("\n".join(lines[:15]))
