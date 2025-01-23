using System.IO;
using TwitchBot.Database;
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
            await using var context = new AppDbContext();
            ViewerType type = await context.GetViewerType(message.ChatMessage.UserId, message.ChatMessage.Username);
            await module.TryExecute(message, type);
            return;
        }
    }
    
    
    
    
    
}