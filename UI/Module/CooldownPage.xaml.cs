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

    private bool firstTime = true;
    private void LoadSettings()
    {
        CooldownText.Text = _currentModule.CooldownManager.Cooldown.ToString();
        CustomCooldown.IsChecked = _currentModule.CooldownManager.CustomCooldown;
        ModeratorText.Text = _currentModule.CooldownManager.ModeratorCooldown.ToString();
        SubscriberText.Text = _currentModule.CooldownManager.SubscriberCooldown.ToString();
        VipText.Text = _currentModule.CooldownManager.VipCooldown.ToString();
        firstTime = false;
    }
    

    private static readonly Regex numbers = new Regex(@"^[0123456789]+$");

    private void CustomCooldown_OnClick(object sender, RoutedEventArgs e)
    {
        _custom = !_custom;
        _currentModule.CooldownManager.SetCustomCooldown(_custom);
        _currentModule.SetModified();
    }

    private void CooldownText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (firstTime)
            return;
        if (numbers.IsMatch(CooldownText.Text))
        {
            _currentModule.CooldownManager.SetCooldown(CooldownText.Text);
            _currentModule.SetModified();
        }
    }

    private void ModeratorText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (firstTime)
            return;
        if (numbers.IsMatch(ModeratorText.Text))
        {
            _currentModule.CooldownManager.SetModeratorCooldown(ModeratorText.Text);
            _currentModule.SetModified();
        }
    }

    private void SubscriberText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (firstTime)
            return;
        if (numbers.IsMatch(SubscriberText.Text))
        {
            _currentModule.CooldownManager.SetSubscriberCooldown(SubscriberText.Text);
            _currentModule.SetModified();
        }
    }

    private void VipText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (firstTime)
            return;
        if (numbers.IsMatch(VipText.Text))
        {
            _currentModule.CooldownManager.SetVipCooldown(VipText.Text);
            _currentModule.SetModified();
        }
    }
}