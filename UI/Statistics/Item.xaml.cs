using System.Windows;
using System.Windows.Controls;

namespace TwitchBot.UI.Statistics;

public partial class Item : UserControl
{
    public Item(string item1, string item2, string item3, string item4)
    {
        InitializeComponent();
        Item1.Text = item1;
        Item2.Text = item2;
        Item3.Text = item3;
        Item4.Text = item4;
    }
    
    
}