import os
import json

def update_designer():
    path = r'ING-SOFTWARE-BritezG/UI/PN2/frmManualAyuda.Designer.cs'
    with open(path, 'r', encoding='utf-8') as f:
        c = f.read()

    replacements = {
        'Índice interactivo para navegación rápida (secciones extensas) y directivas operativas del sistema': 'Guía práctica de uso del sistema y funciones de cada botón',
        'DIRECTIVA CRÍTICA DE SEGURIDAD Y PREVENCIÓN:': 'CONSEJO PRÁCTICO:',
        'Realice un backup completo de la base de datos antes de reinstalar. El instalador inteligente detectará y protegerá sus datos existentes.': 'Recuerde guardar sus cambios al completar una ficha médica y realizar copias de seguridad periódicas.',
        'btnTema1.Text = "1. Arquitectura General";': 'btnTema1.Text = "1. Bienvenida y Ayuda";',
        'btnTema2.Text = "2. Maestro de Clientes (ABM-C)";': 'btnTema2.Text = "2. Gestión de Pacientes";',
        'btnTema3.Text = "3. Tablas 1:N y Baja Lógica";': 'btnTema3.Text = "3. Teléfonos y Correos";',
        'btnTema4.Text = "4. Consultas y Curvas OMS";': 'btnTema4.Text = "4. Consultas y Curvas OMS";',
        'btnTema5.Text = "5. Bitácora Trigger y Rollback";': 'btnTema5.Text = "5. Historial de Cambios";',
        'btnTema6.Text = "6. Reporte Predictivo";': 'btnTema6.Text = "6. Reporte Predictivo";',
        'btnTema7.Text = "7. Reinstalador y Seguridad";': 'btnTema7.Text = "7. Copias de Seguridad";'
    }

    for k, v in replacements.items():
        if k in c:
            c = c.replace(k, v)
            print('Replaced in designer:', k[:30])
        else:
            print('Not found in designer:', k[:30])

    with open(path, 'w', encoding='utf-8') as f:
        f.write(c)

def update_json_files():
    json_paths = [
        r'ING-SOFTWARE-BritezG/UI/Idiomas/es.json',
        r'Instalador/App/Idiomas/es.json'
    ]

    for p in json_paths:
        if not os.path.exists(p):
            continue
        with open(p, 'r', encoding='utf-8') as f:
            data = json.load(f)

        data['btnTema1'] = '1. Bienvenida y Ayuda'
        data['btnTema2'] = '2. Gestión de Pacientes'
        data['btnTema3'] = '3. Teléfonos y Correos'
        data['btnTema4'] = '4. Consultas y Curvas OMS'
        data['btnTema5'] = '5. Historial de Cambios'
        data['btnTema6'] = '6. Reporte Predictivo'
        data['btnTema7'] = '7. Copias de Seguridad'

        with open(p, 'w', encoding='utf-8') as f:
            json.dump(data, f, indent=2, ensure_ascii=False)
        print('Updated json:', p)

if __name__ == '__main__':
    update_designer()
    update_json_files()
