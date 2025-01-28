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
    private NavigationWindow? _navigation;
    
    public StatisticsPage()
    {
        InitializeComponent();
        _filterControl = FilterControl;
        UserMode();
        _filterControl.SearchChanged += SetWordFilter;
        _filterControl.FilterClicked += AdvancedFilters;
        
    }

    public void ClosePage()
    {
        if(_navigationOpened)
            _navigation?.Close();
        ResetFilters();
    }

    void SetWordFilter(string word)
        => _filterable.SetSearch(word);

    private bool _navigationOpened;

    private UserFilter _userFilter;
    private ModuleFilter _moduleFilter;

    private void ResetFilters()
    {
        _userFilter = new();
        _moduleFilter = new();
    }

    private void AdvancedFilters()
    {
        if (_navigationOpened)
        {
            _navigation?.Close();
            _navigationOpened = false;
            return;
        }
        switch (_statsTab)
        {
            case StatisticsTab.User:
            {
                InitializeNavigation();
                UserFilterPage page = new(_userFilter);
                _navigation.Closed += PageIsClosing;
                _navigation.Navigate(page);
                _navigation.Show();
                _navigationOpened = true;
                _navigation.ResizeMode = ResizeMode.NoResize;
                page.FilterUpdated += GetFilter;
            }
                break;
            case StatisticsTab.Module:
            {
                InitializeNavigation();
                ModuleFilterPage page = new(_moduleFilter);
                _navigation.Closed += PageIsClosing;
                _navigation.Navigate(page);
                _navigation.Show();
                _navigationOpened = true;
                _navigation.ResizeMode = ResizeMode.NoResize;
                page.FilterUpdated += GetFilter;
            }
                break;
        }
    }

    void GetFilter(UserFilter filter)
    {
        _userFilter = filter;
        _filterable.SetFilter(filter);
    }
    
    void GetFilter(ModuleFilter filter)
    {
        _moduleFilter = filter;
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
        ResetFilters();
        _navigation?.Close();
        _navigationOpened = false;
        UserMode();
    }

    private void ModuleMod_OnClick(object sender, RoutedEventArgs e)
    {
        ResetFilters();
        _navigation?.Close();
        _navigationOpened = false;
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