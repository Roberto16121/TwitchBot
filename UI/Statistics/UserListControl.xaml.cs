using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace TwitchBot.UI.Statistics;

public partial class UserListControl : UserControl
{
    public UserListControl()
    {
        InitializeComponent();
        Item item = new("Test", "asdasfga", "asdasgbsd", "ASfadas");
        Items.Children.Add(item);
    }
}