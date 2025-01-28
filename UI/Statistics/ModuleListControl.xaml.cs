using System.Windows.Controls;
using TwitchBot.Database;
using TwitchBot.Interface;

namespace TwitchBot.UI.Statistics;

public partial class ModuleListControl : UserControl, IFilterable
{
    private string _word = string.Empty;
    public ModuleListControl()
    {
        InitializeComponent();
        PopulateList();
    }
    
    private void PopulateList()
    {
        Items.Children.Clear();
        List<ModuleStatistics> module = new();
        using var context = new AppDbContext();
        if(_word != string.Empty)
            module = context.ModuleStatistics.Where(m => m.ModuleName.Contains(_word)).ToList();
        else module = context.ModuleStatistics.ToList();

        foreach (var mod in module)
        {
            Item item = new(mod.ModuleName, mod.UsedCount.ToString(), mod.ModuleId, "test");
            Items.Children.Add(item);
        }
    }

    public void SetFilter<T>(T filter) where T : struct
    {
        
    }
    

    public void SetSearch(string word)
    {
        _word = word;
        PopulateList();
    }
}