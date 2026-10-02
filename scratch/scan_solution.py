import os
import re

base_dir = r"c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG"

def scan_files():
    print("=== SCANNING C# SOLUTION ===")
    for root, dirs, files in os.walk(base_dir):
        for f in files:
            if f.endswith(".cs"):
                rel_path = os.path.relpath(os.path.join(root, f), base_dir)
                print(f"CS File: {rel_path}")

scan_files()
