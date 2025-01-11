using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TwitchBot.UI_Parts;

public partial class CooldownPage : Page
{
    private Module _currentModule;
    private bool _custom;
    public CooldownPage(Module module)
    {
        InitializeComponent();
        _currentModule = module;
        LoadSettings();
    }

    private void LoadSettings()
    {
        CooldownText.Text = _currentModule.CooldownManager.Cooldown.ToString();
        CustomCooldown.IsChecked = _currentModule.CooldownManager.CustomCooldown;
        ModeratorText.Text = _currentModule.CooldownManager.ModeratorCooldown.ToString();
        SubscriberText.Text = _currentModule.CooldownManager.SubscriberCooldown.ToString();
        VipText.Text = _currentModule.CooldownManager.VipCooldown.ToString();
    }
    

    public void PageIsClosing()
    {
        
    }

    private static readonly Regex numbers = new Regex(@"^[0123456789]+$");
    private void CooldownText_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && numbers.IsMatch(CooldownText.Text))
            _currentModule.CooldownManager.SetCooldown(CooldownText.Text);
    }

    private void CustomCooldown_OnClick(object sender, RoutedEventArgs e)
    {
        _custom = !_custom;
        _currentModule.CooldownManager.SetCustomCooldown(_custom);
    }

    private void ModeratorText_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && numbers.IsMatch(ModeratorText.Text))
            _currentModule.CooldownManager.SetModeratorCooldown(ModeratorText.Text);
    }

    private void SubscriberText_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && numbers.IsMatch(SubscriberText.Text))
            _currentModule.CooldownManager.SetSubscriberCooldown(SubscriberText.Text);
    }

    private void VipText_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && numbers.IsMatch(VipText.Text))
            _currentModule.CooldownManager.SetVipCooldown(VipText.Text);
    }
}