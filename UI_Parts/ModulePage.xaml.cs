using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace TwitchBot.UI_Parts
{
    /// <summary>
    /// Interaction logic for ModulePage.xaml
    /// </summary>
    public partial class ModulePage : Page
    {
        private readonly MainWindow _window;
        private ModuleSettingsControl? moduleSettings;
        private readonly ModuleManager? _moduleManager;

        private Module? _currentModule = null;
        
        public ModulePage(MainWindow window)
        {
            InitializeComponent();
            this._window = window;
            _moduleManager = TwitchBot.ModuleManager.Instance;
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
                ModuleList.Children.Add(module);
                module.Clicked += SetModule;
            }
        }
        
        
        public void PageIsClosing(object sender, EventArgs e)
        {
            if(moduleSettings != null)
                moduleSettings.ClosePage();
            _moduleManager?.SaveAllModules();
            _window.ClosePage(this);
            
        }

        private int nr = 0;
        private void AddModuleButton_OnClick(object sender, RoutedEventArgs e)
        {
            string name = $"Module{nr++}";
            Module temp = _moduleManager.AddNewModule(name);
            ModuleButtonControl module = new(temp);
            ModuleList.Children.Add(module);
            module.Clicked += SetModule;
        }
        
        private void SetModule(object sender, string name)
        {
            _currentModule = _moduleManager.GetModule(name);
            if (_currentModule == null)
                return;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (moduleSettings != null)
                moduleSettings.Reset();
            
            moduleSettings = new(_currentModule);
            ModuleSettings.Content = moduleSettings;
        }
        
    }
}
