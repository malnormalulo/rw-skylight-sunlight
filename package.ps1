dotnet clean Source
dotnet build Source
Remove-Item -Recurse -Force "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\Skylight Sunlight\*"
Copy-Item .\*  "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\Skylight Sunlight" -Include About, Assemblies, loadFolders.xml, LICENSE*.txt -Recurse
