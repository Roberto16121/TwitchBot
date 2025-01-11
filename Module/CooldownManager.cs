using System.Windows;

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

    public void SetCooldown(string value)
    {
        try
        {
            Cooldown = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Number in wrong format");
        }
    }
    public void SetCustomCooldown(bool value) =>
        CustomCooldown = value;

    public void SetModeratorCooldown(int value)
    {
        if (CustomCooldown)
            ModeratorCooldown = value;
    }
    
    public void SetModeratorCooldown(string value)
    {
        try
        {
            ModeratorCooldown = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Number in wrong format");
        }
    }
    
    public void SetVipCooldown(int value)
    {
        if (CustomCooldown)
            VipCooldown = value;
    }
    
    public void SetVipCooldown(string value)
    {
        try
        {
            VipCooldown = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Number in wrong format");
        }
    }
    
    public void SetSubscriberCooldown(int value)
    {
        if (CustomCooldown)
            SubscriberCooldown = value;
    }
    
    public void SetSubscriberCooldown(string value)
    {
        try
        {
            SubscriberCooldown = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Number in wrong format");
        }
    }
}