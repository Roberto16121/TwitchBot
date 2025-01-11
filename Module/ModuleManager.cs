using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Documents;

namespace TwitchBot;

public class ModuleManager
{
    public static ModuleManager? Instance { get; private set; }
    public List<Module> Modules { get;} = new();

    public ModuleManager()
    {
        Instance ??= this;
        
        string subPath ="Modules"; // Your code goes here

        bool exists = System.IO.Directory.Exists(subPath);

        if(!exists)
            System.IO.Directory.CreateDirectory(subPath);
        else LoadModules();
    }

    void LoadModules()
    {
        string[] files = Directory.GetFiles("Modules");
        foreach (var item in files)
        {
            var module = ModulePersistence.LoadModule(item);
            Modules.Add(module);
        }
    }

    public void SaveAllModules()
    {
        foreach (var module in Modules)
            ModulePersistence.SaveModule(module, $@"Modules\{module.Name}.json");
    }

    public Module AddNewModule(string name)
    {
        Module module = new(name);
        Modules.Add(module);
        return module;
    }

    public Module? GetModule(string name)
    {
        return Modules.Find(item => item.Name.Equals(name));
    }
    
    
    
    
    
    
    
}