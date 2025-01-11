using System.Windows;
using System.Windows.Controls;

namespace TwitchBot.UI_Parts;

public partial class ModuleButtonControl : UserControl
{
    public EventHandler<string> Clicked;
    private string _name;
    public ModuleButtonControl(Module module)
    {
        InitializeComponent();
        _name = module.Name;
        ModuleButton.Content = _name;
        module.NameChanged += NameChanged;

    }

    private void NameChanged(object sender, string name)
    {
        _name = name;
        ModuleButton.Content = _name;
    }

    private void ModuleButton_OnClick(object sender, RoutedEventArgs e)
    {
        Clicked?.Invoke(this, _name);
    }
}