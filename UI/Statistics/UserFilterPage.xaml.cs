using System.Text.RegularExpressions;
using System.Windows.Controls;
using TwitchBot.Interface;

namespace TwitchBot.UI.Statistics;


public partial class UserFilterPage : Page
{
    public event Action<UserFilter>? FilterUpdated;
    private bool init = false;
    public UserFilterPage(UserFilter _filter)
    {
        InitializeComponent();
        _userFilter = new();
        _userFilter = _filter;
        LoadData();
        init = true;
    }

    private void LoadData()
    {
        TimeType.SelectedIndex = _userFilter.SelectedIndex;
        SetTimeType();
        if (_userFilter.MinNrOfModUsages > 0)
            NumberOfModules.Text = _userFilter.MinNrOfModUsages.ToString();
        if (_userFilter.MinNrMessages > 0)
            NumberOfMessages.Text = _userFilter.MinNrMessages.ToString();
        if (_userFilter.MinViewtime > 0)
            Viewtime.Text = ReverseTime(_userFilter.MinViewtime).ToString();
    }

    void SetTimeType()
    {
        switch (TimeType.SelectedIndex)
        {
            case 0: _timeE = TimeEType.Minute; break;
            case 1: _timeE = TimeEType.Hour; break;
            case 2: _timeE = TimeEType.Day; break;
        }
    }

    private UserFilter _userFilter;
    private TimeEType _timeE;

    private void TimeType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!init)
            return;
        SetTimeType();

        _userFilter.SelectedIndex = TimeType.SelectedIndex;
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

    private int ReverseTime(int time)
    {
        int toRet = time;
        if (_timeE == TimeEType.Hour)
            toRet = time / 60;
        else if (_timeE == TimeEType.Day)
            toRet = (time / 60) / 24;
        return toRet;
    }
}