using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace TwitchBot.UI_Parts;

public partial class SoundControl : UserControl
{
    private Module _currentModule;
    private bool Checked;
    public SoundControl(Module module)
    {
        InitializeComponent();
        _currentModule = module;
        LoadSettings();
    }

    private bool firstTime = true;
    private void LoadSettings()
    {
        LocationText.Text = _currentModule.SoundManager.SoundLocation;
        Checked = _currentModule.SoundManager.LoopSound;
        VolumeText.Text = _currentModule.SoundManager.SoundVolume.ToString();
        LoopBox.IsChecked = Checked;
        CountText.Text = _currentModule.SoundManager.LoopCount.ToString();
        firstTime = false;
    }

    private void LocationText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if(firstTime)
            return;
        if (LocationText.Text != string.Empty)
        {
            _currentModule.SoundManager.SetLocation(LocationText.Text);
            _currentModule.SetModified();
        }
    }
    
    private static readonly Regex numbers = new(@"^[0123456789]+$");
    private void LoopBox_OnClick(object sender, RoutedEventArgs e)
    {
        Checked = !Checked;
        _currentModule.SoundManager.SetLoopingSound(Checked);
        _currentModule.SetModified();
    }

    private void CountText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if(firstTime)
            return;
        if (numbers.IsMatch(CountText.Text))
        {
            _currentModule.SoundManager.SetLoopCount(CountText.Text);
            _currentModule.SetModified();
        }
    }

    private void VolumeText_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if(firstTime)
            return;
        if (numbers.IsMatch(VolumeText.Text))
        {
            _currentModule.SoundManager.SetSoundVolume(VolumeText.Text);
            _currentModule.SetModified();
        }
    }

    
}