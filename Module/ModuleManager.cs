using System.IO;
using TwitchLib.Client.Events;


namespace TwitchBot;

public class ModuleManager
{
    public static ModuleManager? Instance { get; private set; }
    public List<Module> Modules { get;} = new();

    public ModuleManager(ChatHandler handler)
    {
        Instance ??= this;
        
        string subPath ="Modules"; // Your code goes here

        bool exists = Directory.Exists(subPath);

        if(!exists)
            Directory.CreateDirectory(subPath);
        else LoadModules();

        handler.MessageReceived += CheckForMatches;
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
            if(module.Modified)
                ModulePersistence.SaveModule(module, $@"Modules\{module.Id}.json");
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

    public void DeleteModule(Module value)
    {
        ModulePersistence.DeteleFile($@"Modules\{value.Id}.json");
        Modules.Remove(value);
    }

    private async void CheckForMatches(object? sender, OnMessageReceivedArgs message)
    {
        foreach (var module in Modules)
        {
            if(!module.Enabled)
                continue;
            if (!module.Keywords.Any(message.ChatMessage.Message.Contains)) continue;
            // PROBLEMA MARE : nu primesc raspuns in timp rezonabil (sub 1-3 secunde) ci in cateva minute => nefezabil
            // Trebuie implementat un sistem pentru salvarea id-urilor moderatorilor, subscriberilor, vip-urilor
            // asa verific in ce categorie se afla id-ul, iar daca nu se afla este viewer normal
            ViewerType type = ViewerManager.Instace.GetViewerAsync(message.ChatMessage.Username).Result.viewerType;
            module.TryExecute(type);
            return;
        }
    }
    
    
    
    
    
}