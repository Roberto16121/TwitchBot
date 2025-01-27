using System.Text.RegularExpressions;
using System.Windows.Controls;
using TwitchBot.Interface;

namespace TwitchBot.UI.Statistics;


public partial class UserFilterPage : Page
{
    public event Action<UserFilter>? FilterUpdated;
    private bool init = false;
    public UserFilterPage(StatisticsPage statistics)
    {
        InitializeComponent();
        _userFilter = new();
        init = true;
    }

    private UserFilter _userFilter;
    private TimeEType _timeE;

    private void TimeType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!init)
            return;
        switch (TimeType.SelectedIndex)
        {
            case 0: _timeE = TimeEType.Minute; break;
            case 1: _timeE = TimeEType.Hour; break;
            case 2: _timeE = TimeEType.Day; break;
        }

        Viewtime_OnTextChanged(null, null);
        FilterUpdated?.Invoke(_userFilter);
    }
    private static readonly Regex numbers = new Regex(@"^[0123456789]+$");
    private void NumberOfModules_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!init)
            return;
        _userFilter.MinNrOfModUsages = numbers.IsMatch(NumberOfModules.Text) ? int.Parse(NumberOfModules.Text) : 0;
        FilterUpdated?.Invoke(_userFilter);
            
    }

    private void NumberOfMessages_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!init)
            return;
        _userFilter.MinNrMessages = numbers.IsMatch(NumberOfMessages.Text) ? int.Parse(NumberOfMessages.Text) : 0;
        FilterUpdated?.Invoke(_userFilter);
    }

    private void Viewtime_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if(!init)
            return;
        _userFilter.MinViewtime = numbers.IsMatch(Viewtime.Text) ? GetTimeInMinutes(int.Parse(Viewtime.Text)) : 0;
        FilterUpdated?.Invoke(_userFilter);
    }

    private int GetTimeInMinutes(int time)
    {
        int minutes = time;
        if (_timeE == TimeEType.Hour)
            minutes = time * 60;
        else if (_timeE == TimeEType.Day)
            minutes = time * 60 * 24;
        return minutes;
    }
}