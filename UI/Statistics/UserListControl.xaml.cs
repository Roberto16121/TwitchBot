using System.Collections.ObjectModel;
using System.Windows.Controls;
using TwitchBot.Database;

namespace TwitchBot.UI.Statistics;

public partial class UserListControl : UserControl
{
    public UserListControl()
    {
        InitializeComponent();
        PopulateList();
    }

    private void PopulateList()
    {
        int count = 0;
        List<UserStatistics> users = new();
        using var context = new AppDbContext();
        users = context.UserStatistics.ToList();
        foreach (var user in users)
        {
            Item item = new(user.Username, user.MessageCount.ToString(), user.ViewTime.ToString(), user.ModuleUsed.ToString());
            Items.Children.Add(item);
        }
    }
}