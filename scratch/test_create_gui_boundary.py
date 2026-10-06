import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

uc06 = ea.GetElementByID(42)
print("Creating/checking boundary in UC 42...")
existing = None
for el in uc06.Elements:
    if el.Name == "GUI":
        existing = el
        break

if not existing:
    el = uc06.Elements.AddNew("GUI", "Sequence")
    el.Stereotype = "boundary"
    el.Update()
    uc06.Elements.Refresh()
    print("Created GUI element ID:", el.ElementID, "Type:", el.Type, "Stereo:", el.Stereotype)
else:
    print("Existing GUI element ID:", existing.ElementID, "Type:", existing.Type, "Stereo:", existing.Stereotype)

ea.CloseFile()
ea.Exit()
