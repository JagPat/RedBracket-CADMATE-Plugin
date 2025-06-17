using System;
using System.Reflection;
using System.Linq;

class AssemblyInspector
{
    static void Main()
    {
        string[] assemblyPaths = 
        {
            @"/Users/jagrutpatel/Projects/AutoCADIntegration/grxsdk/inc/GcCoreMgd.dll",
            @"/Users/jagrutpatel/Projects/AutoCADIntegration/grxsdk/inc/GcDbMgd.dll",
            @"/Users/jagrutpatel/Projects/AutoCADIntegration/grxsdk/inc/GcDbMgdBrep.dll",
            @"/Users/jagrutpatel/Projects/AutoCADIntegration/grxsdk/inc/GcMgd.dll",
            @"/Users/jagrutpatel/Projects/AutoCADIntegration/grxsdk/inc-x64/GrxCAD.Interop.dll"
        };

        foreach (var path in assemblyPaths)
        {
            try
            {
                Console.WriteLine($"\nInspecting: {path}");
                var assembly = Assembly.LoadFrom(path);
                
                // Get all public types and group by namespace
                var typesByNamespace = assembly.GetTypes()
                    .Where(t => t.IsPublic)
                    .GroupBy(t => t.Namespace)
                    .OrderBy(g => g.Key);

                foreach (var group in typesByNamespace)
                {
                    Console.WriteLine($"\nNamespace: {group.Key}");
                    foreach (var type in group.Take(5)) // Show first 5 types per namespace
                    {
                        Console.WriteLine($"  {type.Name}");
                    }
                    if (group.Count() > 5)
                        Console.WriteLine($"  ... and {group.Count() - 5} more types");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading assembly {path}: {ex.Message}");
            }
        }
    }
}
