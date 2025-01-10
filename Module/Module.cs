namespace TwitchBot;

public class Module
{
    #region Values

    private string Name = "";
    private List<string> Keywords = new();
    
    
    private ActionType Type;

    public CooldownManager CooldownManager { get; private set; } = new();
    
    private SoundManager? _soundManager;
    public SoundManager SoundManager => _soundManager ??= new();
    
    private ObsManager? _obsManager;
    public ObsManager ObsManager => _obsManager ??= new();

    #endregion Values


    #region Methods

    public Module(string name)
    {
        Name = name;
    }
    
    public void SetKeywords(string values) // either a single value or values with ; in between
    {
        string[] words = values.Split(';', StringSplitOptions.TrimEntries);
        foreach (var item in words)
            Keywords.Add(item);
    }

    public void SetActionType(ActionType value) =>
        Type = value;

    
    public ModuleData ToSerializableData()
    {
        return new ModuleData
        {
            Name = Name,
            Keywords = new HashSet<string>(Keywords),
            Type = Type,
            Cooldown = new CooldownData
            {
                Cooldown = CooldownManager.Cooldown,
                CustomCooldown = CooldownManager.CustomCooldown,
                ModeratorCooldown = CooldownManager.ModeratorCooldown,
                VipCooldown = CooldownManager.VipCooldown,
                SubscriberCooldown = CooldownManager.SubscriberCooldown
            },
            Sound = _soundManager != null
                ? new SoundData
                {
                    Location = _soundManager.SoundLocation,
                    Volume = _soundManager.SoundVolume,
                    Loop = _soundManager.LoopSound,
                    LoopCount = _soundManager.LoopCount
                }
                : null,
            Obs = _obsManager != null
                ? new ObsData
                {
                    // Populate OBS data
                }
                : null
        };
    }

    public static Module FromSerializableData(ModuleData data)
    {
        var module = new Module(data.Name);
        module.SetActionType(data.Type);
        module.SetKeywords(string.Join(";", data.Keywords));
        module.CooldownManager.SetCooldown(data.Cooldown.Cooldown);
        module.CooldownManager.SetCustomCooldown(data.Cooldown.CustomCooldown);
        module.CooldownManager.SetModeratorCooldown(data.Cooldown.ModeratorCooldown);
        module.CooldownManager.SetVipCooldown(data.Cooldown.VipCooldown);
        module.CooldownManager.SetSubscriberCooldown(data.Cooldown.SubscriberCooldown);

        if (data.Sound != null)
        {
            module.SoundManager.SetLocation(data.Sound.Location);
            module.SoundManager.SetSoundVolume(data.Sound.Volume);
            module.SoundManager.SetLoopingSound(data.Sound.Loop);
            module.SoundManager.SetLoopingCount(data.Sound.LoopCount);
        }
        else if (data.Obs != null)
        {
            //setare valori obs
        }
        
        
        return module;
    }



    #endregion
}