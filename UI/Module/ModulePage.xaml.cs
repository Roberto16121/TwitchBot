using System.Windows;
using System.Windows.Controls;


namespace TwitchBot.UI_Parts
{
    /// <summary>
    /// Interaction logic for ModulePage.xaml
    /// </summary>
    public partial class ModulePage : Page
    {
        private readonly MainWindow _window;
        private ModuleSettingsControl? _moduleSettings;
        private readonly ModuleManager? _moduleManager;

        private Module? _currentModule;
        private List<ModuleButtonControl> _controlList = new();
        
        public ModulePage(MainWindow window)
        {
            InitializeComponent();
            _window = window;
            _moduleManager = ModuleManager.Instance;
            DataContext = _moduleManager;
            LoadModules();
        }

        private void LoadModules()
        {
            if (_moduleManager == null)
                return;
            foreach (var item in _moduleManager.Modules)
            {
                ModuleButtonControl module = new(item);
                _controlList.Add(module);
                ModuleList.Children.Add(module);
                module.Clicked += SetModule;
            }
        }
        
        
        public void PageIsClosing(object sender, EventArgs e)
        {
            if(_moduleSettings != null)
                _moduleSettings.ClosePage();
            _moduleManager?.SaveAllModules();
            _window.ClosePage(this);
            
        }

        private int _nr;
        private void AddModuleButton_OnClick(object sender, RoutedEventArgs e)
        {
            string name = $"Module{_nr++}";
            Module? temp = _moduleManager?.AddNewModule(name);
            if (temp == null)
                return;
            ModuleButtonControl module = new(temp);
            _controlList.Add(module);
            ModuleList.Children.Add(module);
            module.Clicked += SetModule;
        }
        
        private void SetModule(object? sender, string name)
        {
            _currentModule = _moduleManager?.GetModule(name);
            if (_currentModule == null)
                return;
            _currentModule.Deleted += DeleteModule;
            
            UpdateUi();
        }

        private void DeleteModule(object? sender, EventArgs e)
        {
            if (_currentModule == null)
                return;
            int index = _controlList.FindIndex(s => s.Name == _currentModule.Name);
            ModuleList.Children.RemoveAt(index+1);
            _controlList.RemoveAt(index);
            _moduleSettings?.ClosePage();
            _moduleSettings = null;
            ModuleSettings.Content = null;
            _moduleManager?.DeleteModule(_currentModule);
            _currentModule = null;
        }

        private void UpdateUi()
        {
            if (_moduleSettings != null)
                _moduleSettings.Reset();
            if (_currentModule == null)
                return;

            _moduleSettings = null;
            _moduleSettings = new(_currentModule);
            ModuleSettings.Content = _moduleSettings;
        }
        
    }
}
