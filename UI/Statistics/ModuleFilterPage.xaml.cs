using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TwitchBot.Interface;

namespace TwitchBot.UI.Statistics;

public partial class ModuleFilterPage : Page
{
    public event Action<ModuleFilter>? FilterUpdated;
    private ModuleFilter _moduleFilter;
    public ModuleFilterPage(ModuleFilter _filter)
    {
        InitializeComponent();
        _moduleFilter = _filter;
    }

    private bool _hideObs, _hideSound;

    private static readonly Regex numbers = new Regex(@"^[0123456789]+$");
    private void MinUses_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        _moduleFilter.MinNrOfUses = numbers.IsMatch(MinUses.Text) ? int.Parse(MinUses.Text) : 0;
        FilterUpdated?.Invoke(_moduleFilter);
    }

    private void HideSound_OnClick(object sender, RoutedEventArgs e)
    {
        _hideSound = !_hideSound;
        _moduleFilter.HideSoundModules = _hideSound;
        FilterUpdated?.Invoke(_moduleFilter);
    }

    private void HideObs_OnClick(object sender, RoutedEventArgs e)
    {
        _hideObs = !_hideObs;
        _moduleFilter.HideObsModules = _hideObs;
        FilterUpdated?.Invoke(_moduleFilter);
    }
}