import pypdf

reader = pypdf.PdfReader(r"C:\Users\Danie\Desktop\GIT\TD\ING SOFTWARE\Proyecto ING. Software.pdf")
for p in [5, 6, 7, 10, 66, 68]:
    print(f"\n=================== PAGE {p} ===================")
    print(reader.pages[p-1].extract_text())
