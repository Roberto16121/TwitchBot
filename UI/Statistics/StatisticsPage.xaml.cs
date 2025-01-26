using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TwitchBot.Interface;

namespace TwitchBot.UI.Statistics;

public partial class StatisticsPage : Page
{
    private FilterControl _filterControl;
    private UserListControl _userListControl;
    private ModuleListControl _moduleListControl;
    private IFilterable _filterable;
    public StatisticsPage()
    {
        InitializeComponent();
        _filterControl = FilterControl;
        UserMode();
        _filterControl.SearchChanged += SetWordFilter;
    }

    void SetWordFilter(string word)
        => _filterable.SetSearch(word);
    

    private void UserMod_OnClick(object sender, RoutedEventArgs e)
    {
        UserMode();
    }

    private void ModuleMod_OnClick(object sender, RoutedEventArgs e)
    {
        ModuleMode();
    }

    private void UserMode()
    {
        _userListControl = new();
        Stats.Content = _userListControl;
        UserMod.Background = Brushes.Wheat;
        ModuleMod.Background = Brushes.DarkGray;
        _filterable = _userListControl;
    }

    private void ModuleMode()
    {
        _moduleListControl = new();
        Stats.Content = _moduleListControl;
        UserMod.Background = Brushes.DarkGray;
        ModuleMod.Background = Brushes.Wheat;
        _filterable = _moduleListControl;
    }
}