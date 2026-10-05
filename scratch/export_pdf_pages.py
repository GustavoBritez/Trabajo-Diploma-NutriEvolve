import pypdf

reader = pypdf.PdfReader(r"C:\Users\Danie\Desktop\GIT\TD\ING SOFTWARE\Proyecto ING. Software.pdf")
with open(r"C:\Users\Danie\Desktop\GIT\TD\scratch\pdf_pages_60_75.txt", "w", encoding="utf-8") as f:
    for p in range(60, 76):
        f.write(f"\n=================== PAGE {p} ===================\n")
        f.write(reader.pages[p-1].extract_text() or "")
print("Saved to scratch/pdf_pages_60_75.txt")
