import os
import re

files_to_check = [
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\BLL\Seguridad\UsuarioBLL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\DAL\Seguridad\UsuarioDAL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\BLL\Seguridad\BackupBLL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\DAL\Seguridad\BackupDAL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\BLL\Seguridad\IdiomaBLL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\DAL\Seguridad\IdiomaDAL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\BLL\Perfiles\PerfilBLL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\DAL\Perfiles\PerfilDAL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\BLL\Perfiles\FamiliaBLL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\DAL\Perfiles\FamiliaDAL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\BLL\Perfiles\PatenteBLL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\DAL\Perfiles\PatenteDAL.cs",
    r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\DAL\Conexion.cs"
]

method_pattern = re.compile(r'^\s*(public|private|protected|internal)\s+(static\s+)?([\w\<\>\[\],\s\?]+?)\s+([A-Z]\w*)\s*\((.*?)\)', re.MULTILINE)

for path in files_to_check:
    if os.path.exists(path):
        name = os.path.basename(path)
        content = open(path, encoding='utf-8', errors='ignore').read()
        print(f"\n=== {name} ===")
        matches = method_pattern.findall(content)
        for m in matches:
            vis, stat, ret, mname, params = m
            # skip constructors if same name as class
            cls_name = os.path.splitext(name)[0]
            params_clean = " ".join(params.replace("\n", " ").split())
            ret_clean = ret.strip()
            print(f"  + {mname}({params_clean}) : {ret_clean}")
