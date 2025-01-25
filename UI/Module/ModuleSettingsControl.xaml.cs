
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;


namespace TwitchBot.UI_Parts
{
    /// <summary>
    /// Interaction logic for ModuleSettingsControl.xaml
    /// </summary>
    public partial class ModuleSettingsControl : UserControl
    {
        private NavigationWindow? _navigationWdw;
        private ActionType _type = ActionType.Sound;
        private readonly Module _currentModule;
        private bool _enabled = true;
        private bool isInit = false;
        public ModuleSettingsControl(Module module)
        {
            InitializeComponent();
            isInit = true;
            _currentModule = module;
            LoadSettings();
        }
        public void ClosePage() =>
            _navigationWdw?.Close();

        public void Reset()
        {
            if(_navigationWdw != null)
                _navigationWdw.Close();
        }

        
        CooldownPage? _page;
        
        private void InitializeNavigation()
        {
            _navigationWdw = new()
            {
                Height = 400,
                Width = 800,
                ShowsNavigationUI = false
            };
        }
        
        private void OpenCooldown_OnClick(object sender, RoutedEventArgs e)
        {
            InitializeNavigation();
            _page = new(_currentModule);
            _navigationWdw.Closed += PageIsClosing;
            _navigationWdw.Navigate(_page);
            _navigationWdw.Show();
            _navigationWdw.ResizeMode = ResizeMode.NoResize;
        }

        private void PageIsClosing(object? sender, EventArgs e)
        {
            _page = null;
            _navigationWdw = null;
        }


        private void LoadSettings()
        {
            _enabled = _currentModule.Enabled;
            
            SetName(_currentModule.Name);
            SetKeywords(_currentModule.Keywords);
            SetActionType((int)_currentModule.Type);
            SetActionSettings();
            _firstTime = false;
            if(!_enabled)
                SetState();
                
        }

        private bool _firstTime = true;

        private void ActionTypeBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _type = ActionTypeBox.SelectedIndex == 0 ? ActionType.Sound : ActionType.Obs;
            if (!isInit)
                return;
            
            _currentModule.SetActionType(_type);
            if (!_firstTime)
                _currentModule.SetModified();
            SetActionSettings();
        }
        

        

        private void SetName(string name) =>
            NameText.Text = name;

        private void SetKeywords(List<string> keywords) =>
            KeywordsText.Text = string.Join(";", keywords);

        private void SetActionType(int type) =>
            ActionTypeBox.SelectedIndex = type;

        private SoundControl _soundControl;
        private ObsAction _obsAction;
        
        private void SetActionSettings()
        {
            if (_type == ActionType.Sound)
            {
                _soundControl = new (_currentModule);
                ActionSettings.Content = _soundControl;
            }
            else
            {
                _obsAction = new(_currentModule);
                ActionSettings.Content = _obsAction;
            }
        }
        
        private void KeywordsText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(_firstTime)
                return;
            if (KeywordsText.Text != String.Empty)
            {
                _currentModule.SetKeywords(KeywordsText.Text);
                _currentModule.SetModified();
            }
        }

        private void NameText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(_firstTime)
                return;
            if (NameText.Text != String.Empty)
            {
                _currentModule.SetName(NameText.Text);
                _currentModule.SetModified();
            }
        }

        private void DeleteModule_OnClick(object sender, RoutedEventArgs e)
        {
            _currentModule.Delete();
        }

        private void SetState()
        {
            State.IsChecked = _enabled;
            if (_enabled)
            {
                State.Background = Brushes.White;
                State.Content = "ON";
            }
            else
            {
                State.Background = Brushes.Gray;
                State.Content = "OFF";
            }
        }
            
        private void State_OnClick(object sender, RoutedEventArgs e)
        {
            _enabled = !_enabled;
            _currentModule.SetEnable(_enabled);
            SetState();
        }
    }
}
