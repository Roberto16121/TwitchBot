using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace TwitchBot.UI_Parts
{
    /// <summary>
    /// Interaction logic for ModuleSettingsControl.xaml
    /// </summary>
    public partial class ModuleSettingsControl : UserControl
    {
        NavigationWindow? _navigationWdw;
        private ActionType _type = ActionType.Sound;
        private Module? _currentModule;
        public ModuleSettingsControl(Module module)
        {
            InitializeComponent();
            _currentModule = module;
            LoadSettings();
        }
        public void ClosePage()
        {
            if(_navigationWdw != null)
                _navigationWdw.Close();
        }

        public void Reset()
        {
            if(_navigationWdw != null)
                _navigationWdw.Close();
        }

        private void NameText_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && NameText.Text != String.Empty)
                _currentModule.SetName(NameText.Text);
        }

        private void KeywordsText_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && NameText.Text != String.Empty)
                _currentModule.SetKeywords(KeywordsText.Text);
        }
        CooldownPage page;
        
        private void InitializeNavigation()
        {
            _navigationWdw = new NavigationWindow();
            _navigationWdw.Height = 400;
            _navigationWdw.Width = 800;
            _navigationWdw.ShowsNavigationUI = false;
        }
        
        private void OpenCooldown_OnClick(object sender, RoutedEventArgs e)
        {
            InitializeNavigation();
            page = new(_currentModule);
            _navigationWdw.Closed += PageIsClosing;
            _navigationWdw.Navigate(page);
            _navigationWdw.Show();
            _navigationWdw.ResizeMode = ResizeMode.NoResize;
        }

        private void PageIsClosing(object sender, EventArgs e)
        {
            page = null;
            _navigationWdw = null;
        }


        private void LoadSettings()
        {
            SetName(_currentModule.Name);
            SetKeywords(_currentModule.Keywords);
            SetActionType((int)_currentModule.Type);
        }

        private void ActionTypeBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _type = ActionTypeBox.SelectedIndex == 0 ? ActionType.Sound : ActionType.Obs;
            if (_currentModule == null)
                return;
            _currentModule.SetActionType(_type);
        }

        private void SetName(string name) =>
            NameText.Text = name;

        private void SetKeywords(List<string> keywords) =>
            KeywordsText.Text = string.Join(";", keywords);

        private void SetActionType(int type) =>
            ActionTypeBox.SelectedIndex = type;
        
        
    }
}
