import pypdf

reader = pypdf.PdfReader(r'C:\Users\Danie\Desktop\GIT\Manual_de_Ayuda_NutriEvolve.pdf')
print(f'Total pages in generated PDF: {len(reader.pages)}')

prohibited_terms = [
    'SISTEMA DE GESTIÓN CLÍNICA Y AUDITORÍA INTEGRAL',
    'NutriEvolve v2.0 • Especialidad Nutrición Infantil OMS • Documento de Soporte',
    'directivas operativas',
    'máquina de estados finitos',
    'arquitectura multicapa',
    'Repercusiones Transaccionales'
]

print('\n=== CHECKING PROHIBITED TERMS ===')
for term in prohibited_terms:
    found = False
    for i, p in enumerate(reader.pages):
        txt = p.extract_text() or ''
        if term.lower() in txt.lower():
            print(f'  [FOUND!] Term "{term}" found on page {i+1}')
            found = True
    if not found:
        print(f'  [CLEAN] Term "{term}" is NOT in the PDF.')

print('\n=== PAGE SUMMARIES ===')
for i, p in enumerate(reader.pages):
    txt = p.extract_text() or ''
    first_few = [l.strip() for l in txt.splitlines() if l.strip()][:4]
    print(f'Page {i+1}: {first_few}')
