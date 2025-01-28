using System.Windows;
using TwitchLib.Client.Events;
using System.Windows.Controls;
using System.Windows.Navigation;
using TwitchBot.UI_Parts;
using TwitchBot.UI;
using TwitchBot.UI.Statistics;
using Brushes = System.Windows.Media.Brushes;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Client _client;
        
        private FrameType _frameType = FrameType.Home;
        private HomePage? _home;
    
        public MainWindow()
        {
            InitializeComponent();

            _client = new();
            
            _client.ServiceManager.ChatEventManager.OnMessageReceived += UpdateMessages;
            DataContext = _client.ChatHandler;
            _home = new(_client);
            HomeButton.Foreground = Brushes.Wheat;
            MainFrame.Navigate(_home);
        }





        #region UI_Interaction

        private void UpdateMessages(OnMessageReceivedArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _client.ChatHandler?.AddMessage(e);
                
                _home?.ScrollIntoView();
            });
        }

        private ModulePage? _modulePage;
        private StatisticsPage? _statistics;
        private void ModulesButton_Click(object sender, RoutedEventArgs e)
        {
            _statistics?.ClosePage();
            _statistics = null;
            if (_frameType != FrameType.Module)
            {
                _modulePage = new(this);
                MainFrame.Navigate(_modulePage);
                _frameType = FrameType.Module;
                HomeButton.Foreground = Brushes.Black;
                ModulesButton.Foreground = Brushes.Wheat;
                StatisticsButton.Foreground = Brushes.Black;
                SettingsButton.Foreground = Brushes.Black;
            }
            
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            _modulePage?.PageIsClosing();
            _modulePage = null;
            if (_frameType != FrameType.Statistics)
            {
                _statistics = new();
                MainFrame.Navigate(_statistics);
                _frameType = FrameType.Statistics;
                HomeButton.Foreground = Brushes.Black;
                ModulesButton.Foreground = Brushes.Black;
                StatisticsButton.Foreground = Brushes.Wheat;
                SettingsButton.Foreground = Brushes.Black;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void HomeButton_OnClick(object sender, RoutedEventArgs e)
        {
            _modulePage?.PageIsClosing();
            _modulePage = null;
            _statistics?.ClosePage();
            _statistics = null;
            if (_frameType != FrameType.Home)
            {
                MainFrame.Navigate(_home);
                _frameType = FrameType.Home;
                HomeButton.Foreground = Brushes.Wheat;
                ModulesButton.Foreground = Brushes.Black;
                StatisticsButton.Foreground = Brushes.Black;
                SettingsButton.Foreground = Brushes.Black;
            }

        }

        #endregion UI_Interaction



        protected override async void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            await _client.ServiceManager.ChatEventManager.UpdateAllUsers();
            _client.ModuleManager.SaveAllModules();
            _statistics?.ClosePage();
            _modulePage?.PageIsClosing();
            
        }
        
        
    }
}