using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TwitchBot.UI.Statistics;

public partial class StatisticsPage : Page
{
    private FilterControl _filterControl;
    private UserListControl _userListControl;
    private ModuleListControl _moduleListControl;
    public StatisticsPage()
    {
        InitializeComponent();
        _filterControl = FilterControl;
        UserMode();
    }

    private void DaysAll_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Days30_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Days7_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void DayToday_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

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
    }

    private void ModuleMode()
    {
        _moduleListControl = new();
        Stats.Content = _moduleListControl;
        UserMod.Background = Brushes.DarkGray;
        ModuleMod.Background = Brushes.Wheat;
    }
}