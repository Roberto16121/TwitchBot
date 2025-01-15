using System.Windows;
using System.Windows.Controls;

namespace TwitchBot.UI_Parts;

public partial class ModuleButtonControl : UserControl
{
    public EventHandler<string>? Clicked;
    public string Name { get; private set; }

    public ModuleButtonControl(Module module)
    {
        InitializeComponent();
        Name = module.Name;
        ModuleButton.Content = Name;
        module.NameChanged += NameChanged;

    }

    private void NameChanged(object? sender, string name)
    {
        Name = name;
        ModuleButton.Content = Name;
    }

    private void ModuleButton_OnClick(object sender, RoutedEventArgs e)
    {
        Clicked?.Invoke(this, Name);
    }
}