using System.Windows;
using System.Windows.Controls;

namespace TwitchBot.UI.Statistics;

public partial class FilterControl : UserControl
{
    public event Action<string>? SearchChanged;
    
    public FilterControl()
    {
        InitializeComponent();
    }
    

    private void FilterButton_OnClick(object sender, RoutedEventArgs e)
    {
        
    }

    private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SearchChanged?.Invoke(SearchBox.Text);
    }
}