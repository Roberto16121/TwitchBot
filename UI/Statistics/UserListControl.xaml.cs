using System.Collections.ObjectModel;
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
            Item item = new(user.Username, user.MessageCount.ToString(), user.ViewTime.ToString(), user.ModuleUsed.ToString());
            Items.Children.Add(item);
        }
    }

    public void SetFilter<T>(T filter) where T : struct
    {
        UserFilter userFilter = new();
        
        var filterProperties = typeof(T).GetProperties();
        var userFilterProperties = typeof(UserFilter).GetProperties();
        
        foreach (var filterProp in filterProperties)
        {
            var userFilterProp = userFilterProperties.FirstOrDefault(p => p.Name == filterProp.Name && p.PropertyType == filterProp.PropertyType);
            if (userFilterProp != null)
            {
                var value = filterProp.GetValue(filter);
                userFilterProp.SetValue(userFilter, value);
            }
        }

        _userFilter = userFilter;
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