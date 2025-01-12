using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace TwitchBot.UI_Parts;

public partial class ObsAction : UserControl
{
    private Module _currentModule;
    private bool Checked;
    public ObsAction(Module module)
    {
        InitializeComponent();
        _currentModule = module;
        LoadSettings();
    }

    private bool firstTime = true;
    private void LoadSettings()
    {
        SourceText.Text = _currentModule.ObsManager.SourceName;
        Checked = _currentModule.ObsManager.Loop;
        LoopBox.IsChecked = Checked;
        DurationText.Text = _currentModule.ObsManager.Duration.ToString();
        CountText.Text = _currentModule.ObsManager.Count.ToString();
        firstTime = false;
    }
    
    
    private void SourceText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (firstTime)
            return;
        if (SourceText.Text != String.Empty)
        {
            _currentModule.ObsManager.SetSourceName(SourceText.Text);
            _currentModule.SetModified();
        }
    }
    
    private static readonly Regex numbers = new(@"^[0123456789]+$");
    private void DurationText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (firstTime)
            return;
        if (numbers.IsMatch(DurationText.Text))
        {
            _currentModule.ObsManager.SetDuration(DurationText.Text);
            _currentModule.SetModified();
        }

    }

    private void LoopBox_OnClick(object sender, RoutedEventArgs e)
    {
        Checked = !Checked;
        _currentModule.ObsManager.SetLoop(Checked);
        _currentModule.SetModified();
    }

    private void CountText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (firstTime)
            return;
        if (numbers.IsMatch(CountText.Text))
        {
            _currentModule.SetModified();
            _currentModule.ObsManager.SetLoopCount(CountText.Text);
        }
    }
}