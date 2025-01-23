
using TwitchBot.Database;
using TwitchLib.Client.Events;

namespace TwitchBot;

public class Module
{
    #region Values

    public bool Enabled { get; private set; } = true;
    public Guid Id { get; private set; }
    private string _id;
    public string Name { get; private set; }
    public List<string> Keywords { get; private set; } = new();
    public ActionType Type { get; private set; }

    public CooldownManager CooldownManager { get; private set; } = new();
    
    private SoundManager? _soundManager;
    public SoundManager SoundManager => _soundManager ??= new();
    
    private ObsManager? _obsManager;
    public ObsManager ObsManager => _obsManager ??= new();

    public bool Modified { get; private set; }

    private DateTime _moderator = DateTime.MinValue;
    private DateTime _subscriber = DateTime.MinValue;
    private DateTime _vip = DateTime.MinValue;
    private DateTime _normal = DateTime.MinValue;
    
    #endregion Values

    public EventHandler<string>? NameChanged;
    public EventHandler? Deleted;
    
    #region Methods

    
    public Module(string name)
    {
        Name = name;
        Type = ActionType.Sound;
        Id = Guid.NewGuid();
        _id = Id.ToString();
    }

    public void SetEnable(bool value) =>
        Enabled = value;


    private void SetId(Guid value)
    {
        Id = value;
        _id = Id.ToString();
    }

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
        Deleted?.Invoke(this, null!);

    public void SetModified() =>
        Modified = true;

    private bool _executed;
    public async Task TryExecute(OnMessageReceivedArgs message, ViewerType viewerType)
    {
        if (viewerType == ViewerType.Broadcaster)
        {
            Execute();
            await UpdateDatabase(message.ChatMessage.UserId);
            return;
        }

        if (!CooldownManager.CustomCooldown)
        {
            if (GetDuration(_normal) >= CooldownManager.Cooldown)
            {
                Execute(ref _normal);
                await UpdateDatabase(message.ChatMessage.UserId);
            }

            return;
        }
        _executed = false;
        switch (viewerType)
        {
            case ViewerType.Moderator:
            {
                if(GetDuration(_moderator) >= CooldownManager.ModeratorCooldown)
                    Execute(ref _moderator);
            } break;
            case ViewerType.Subscriber:
            {
                if(GetDuration(_subscriber) >= CooldownManager.SubscriberCooldown)
                    Execute(ref _subscriber);
            } break;
            case ViewerType.VIP:
            {
                if(GetDuration(_vip) >= CooldownManager.VipCooldown)
                    Execute(ref _vip);
            } break;
            case ViewerType.Normal:
            {
                if(GetDuration(_normal) >= CooldownManager.Cooldown)
                    Execute(ref _normal);
            } break;
        }
        if(_executed)
            await UpdateDatabase(message.ChatMessage.UserId);
    }

    int GetDuration(DateTime executed) =>
        (DateTime.Now - executed).Duration().Seconds;

    void Execute(ref DateTime time)
    {
        _executed = true;
        time = DateTime.Now;
        _ = Type == ActionType.Sound ? SoundController.Instance.Execute(SoundManager) 
            : ObsController.Instance.Execute(ObsManager);
    }

    void Execute()
    {
        _ = Type == ActionType.Sound ? SoundController.Instance.Execute(SoundManager) 
            : ObsController.Instance.Execute(ObsManager);
    }

    async Task UpdateDatabase(string userId)
    {
        await using var context = new AppDbContext();
        bool success = await context.IncreaseModuleUsed(_id);
        if(!success)
            await context.AddNewModuleStatistics(_id, Name, 1);
        await context.IncreaseUserModuleUsed(userId, _id);
    }
    
    #endregion Methods

    #region Serialization
    
    public ModuleData ToSerializableData()
    {
        return new ModuleData
        {
            Enabled = Enabled,
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
        module.SetEnable(data.Enabled);
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
        if (data.Obs != null)
        {
            module.ObsManager.SetSourceName(data.Obs.SourceName);
            module.ObsManager.SetDuration(data.Obs.Duration);
            module.ObsManager.SetLoop(data.Obs.Loop);
            module.ObsManager.SetLoopCount(data.Obs.Count);
        }
        
        
        return module;
    }



    #endregion Serialization
}