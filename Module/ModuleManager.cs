namespace TwitchBot;

public class ModuleManager
{
    public static ModuleManager? Instance { get; private set; }
    public List<Module> Modules { get; private set; } = new();

    public ModuleManager()
    {
        if (Instance == null)
            Instance = this;
    }
    
    
    
}