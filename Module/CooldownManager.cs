namespace TwitchBot;

public class CooldownManager
{
    
    public int Cooldown { get; private set; }
    public bool CustomCooldown { get; private set; }
    public int ModeratorCooldown { get; private set; }
    public int VipCooldown { get; private set; }
    public int SubscriberCooldown { get; private set; }
    
    
    
    public void SetCooldown(int value) =>
        Cooldown = value;

    public void SetCustomCooldown(bool value) =>
        CustomCooldown = value;

    public void SetModeratorCooldown(int value)
    {
        if (CustomCooldown)
            ModeratorCooldown = value;
    }
    
    public void SetVipCooldown(int value)
    {
        if (CustomCooldown)
            VipCooldown = value;
    }
    
    public void SetSubscriberCooldown(int value)
    {
        if (CustomCooldown)
            SubscriberCooldown = value;
    }
}