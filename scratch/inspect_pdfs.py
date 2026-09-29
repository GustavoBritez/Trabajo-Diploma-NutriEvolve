import os
import glob
import pypdf

# Let's inspect the PDF content summary if pypdf or PyPDF2 is available, or check if we can read text
def inspect_pdf(path):
    try:
        reader = pypdf.PdfReader(path)
        print(f"\n=== PDF: {os.path.basename(path)} ===")
        print(f"Total Pages: {len(reader.pages)}")
        # Print outline/TOC or first few pages
        outline = reader.outline
        print(f"Outline entries: {len(outline) if outline else 0}")
        text_p1 = reader.pages[0].extract_text()
        print("Page 1 snippet:\n", text_p1[:400])
        if len(reader.pages) > 1:
            text_p2 = reader.pages[1].extract_text()
            print("Page 2 snippet:\n", text_p2[:400])
    except Exception as e:
        print(f"Error reading {path}: {e}")

try:
    inspect_pdf(r'C:\Users\Danie\Desktop\GIT\TD\Trabajo Diploma Britez.docx.pdf')
    inspect_pdf(r'C:\Users\Danie\Desktop\GIT\TD\Guia de Realizacion\Guia_Realizacion_TD.pdf')
    inspect_pdf(r'C:\Users\Danie\Desktop\GIT\TD\ING SOFTWARE\Proyecto ING. Software.pdf')
except Exception as e:
    print(f"General error: {e}")
