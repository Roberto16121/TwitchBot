using System.Windows.Controls;
using TwitchBot.Database;
using TwitchBot.Interface;

namespace TwitchBot.UI.Statistics;

public partial class UserListControl : UserControl, IFilterable
{
    private string _word = string.Empty;
    private UserFilter _userFilter = new();
    public UserListControl()
    {
        InitializeComponent();
        PopulateList();
    }

    private void PopulateList()
    {
        Items.Children.Clear();
        List<UserStatistics> users = GetFilteredUsers(_userFilter);
        foreach (var user in users)
        {
            Item item = new(user.Username, user.MessageCount.ToString(), user.ModuleUsed.ToString(), user.ViewTime.ToString());
            Items.Children.Add(item);
        }
    }

    public void SetFilter<T>(T filter) where T : struct
    {
        if (filter is UserFilter userFilter)
            _userFilter = userFilter;
        else
        {
            // Manually map properties if T is a different struct
            _userFilter = new UserFilter
            {
                MinNrMessages = (int)(filter.GetType().GetProperty("MinNrMessages")?.GetValue(filter) ?? 0),
                MinNrOfModUsages = (int)(filter.GetType().GetProperty("MinNrOfModUsages")?.GetValue(filter) ?? 0),
                MinViewtime = (int)(filter.GetType().GetProperty("MinViewtime")?.GetValue(filter) ?? 0)
            };
        }

        PopulateList();
    }
    

    public void SetSearch(string word)
    {
        _word = word;
        PopulateList();
    }
    
    private List<UserStatistics> GetFilteredUsers(UserFilter filter)
    {
        using var context = new AppDbContext();
        var query = context.UserStatistics.AsQueryable();
        if (_word != string.Empty)
            query = query.Where(u => u.Username.Contains(_word));

        // Apply filters based on the UserFilter struct
        if (filter.MinNrMessages > 0)
            query = query.Where(u => u.MessageCount >= filter.MinNrMessages);
        if (filter.MinNrOfModUsages > 0)
            query = query.Where(u => u.ModuleUsed >= filter.MinNrOfModUsages);
        if (filter.MinViewtime > 0)
            query = query.Where(u => u.ViewTime >= filter.MinViewtime);

        return query.ToList();
    }
}