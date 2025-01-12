using System.Windows;

namespace TwitchBot;

public class Module
{
    #region Values

    public Guid Id { get; private set; }
    public string Name { get; private set; }= "";
    public List<string> Keywords { get; private set; } = new();
    public ActionType Type { get; private set; }

    public CooldownManager CooldownManager { get; private set; } = new();
    
    private SoundManager? _soundManager;
    public SoundManager SoundManager => _soundManager ??= new();
    
    private ObsManager? _obsManager;
    public ObsManager ObsManager => _obsManager ??= new();

    public bool Modified { get; private set; }
    
    #endregion Values

    public EventHandler<string> NameChanged;
    public EventHandler Deleted;
    #region Methods

    
    public Module(string name)
    {
        Name = name;
        Type = ActionType.Sound;
        Id = Guid.NewGuid();
    }

    public void SetId(Guid value) =>
        Id = value;

    public void SetName(string name)
    {
        Name = name;
        NameChanged?.Invoke(this, Name);
    }

    public void SetKeywords(string values) // either a single value or values with ; in between
    {
        Keywords.Clear();
        string[] words = values.Split(';', StringSplitOptions.TrimEntries);
        foreach (var item in words)
            Keywords.Add(item);
    }

    public void SetActionType(ActionType value) =>
        Type = value;

    public void Delete() =>
        Deleted?.Invoke(this, null);

    public void SetModified() =>
        Modified = true;
    
    public ModuleData ToSerializableData()
    {
        return new ModuleData
        {
            Id = Id,
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
                    SourceName = _obsManager.SourceName,
                    Duration = _obsManager.Duration,
                    Count = _obsManager.Count,
                    Loop = _obsManager.Loop
                }
                : null
        };
    }

    public static Module FromSerializableData(ModuleData data)
    {
        var module = new Module(data.Name);
        module.SetId(data.Id);
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
            module.SoundManager.SetLoopCount(data.Sound.LoopCount);
        }
        else if (data.Obs != null)
        {
            module.ObsManager.SetSourceName(data.Obs.SourceName);
            module.ObsManager.SetDuration(data.Obs.Duration);
            module.ObsManager.SetLoop(data.Obs.Loop);
            module.ObsManager.SetLoopCount(data.Obs.Count);
        }
        
        
        return module;
    }



    #endregion
}