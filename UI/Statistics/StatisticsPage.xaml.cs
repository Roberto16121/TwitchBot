using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TwitchBot.Interface;

namespace TwitchBot.UI.Statistics;

public partial class StatisticsPage : Page
{
    private FilterControl _filterControl;
    private UserListControl _userListControl;
    private ModuleListControl _moduleListControl;
    private IFilterable _filterable;
    private StatisticsTab _statsTab;
    private NavigationWindow _navigation;
    
    public StatisticsPage()
    {
        InitializeComponent();
        _filterControl = FilterControl;
        UserMode();
        _filterControl.SearchChanged += SetWordFilter;
        _filterControl.FilterClicked += AdvancedFilters;
        
    }

    void SetWordFilter(string word)
        => _filterable.SetSearch(word);

    private bool _navigationOpened;

    private void AdvancedFilters()
    {
        if (_navigationOpened)
        {
            _navigation.Close();
            return;
        }
        switch (_statsTab)
        {
            case StatisticsTab.User:
            {
                InitializeNavigation();
                UserFilterPage page = new(this);
                _navigation.Closed += PageIsClosing;
                _navigation.Navigate(page);
                _navigation.Show();
                _navigationOpened = true;
                _navigation.ResizeMode = ResizeMode.NoResize;
                page.FilterUpdated += GetUserFilter;
            }
                break;
            case StatisticsTab.Module:
            {
                
            }
                break;
        }
    }

    void GetUserFilter(UserFilter filter)
    {
        _filterable.SetFilter(filter);
    }

    private void PageIsClosing(object? sender, EventArgs e)
    {
        _navigationOpened = false;
    }

    private void InitializeNavigation()
    {
        _navigation = new()
        {
            Height = 200,
            Width = 500,
            ShowsNavigationUI = false
        };
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
        _filterable = _userListControl;
        _statsTab = StatisticsTab.User;
    }

    private void ModuleMode()
    {
        _moduleListControl = new();
        Stats.Content = _moduleListControl;
        UserMod.Background = Brushes.DarkGray;
        ModuleMod.Background = Brushes.Wheat;
        _filterable = _moduleListControl;
        _statsTab = StatisticsTab.Module;
    }
}