using System.Windows;
using System.Windows.Controls;

namespace TwitchBot.UI.Statistics;

public partial class FilterControl : UserControl
{
    public event Action<string>? SearchChanged;
    public event Action? FilterClicked;
    
    public FilterControl()
    {
        InitializeComponent();
    }
    

    private void FilterButton_OnClick(object sender, RoutedEventArgs e)
    {
        FilterClicked?.Invoke();
    }

    private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SearchChanged?.Invoke(SearchBox.Text);
    }
}