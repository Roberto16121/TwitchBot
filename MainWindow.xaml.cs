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
        NavigationService _ns;
        NavigationWindow? _navigationWdw;
        
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

        private void ModulesButton_Click(object sender, RoutedEventArgs e)
        {
            if (_frameType != FrameType.Module)
            {
                MainFrame.Navigate(new ModulePage(this));
                _frameType = FrameType.Module;
                HomeButton.Foreground = Brushes.Black;
                ModulesButton.Foreground = Brushes.Wheat;
                StatisticsButton.Foreground = Brushes.Black;
                SettingsButton.Foreground = Brushes.Black;
            }
            
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_frameType != FrameType.Statistics)
            {
                MainFrame.Navigate(new StatisticsPage());
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



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _client.ModuleManager.SaveAllModules();
                
            if (_navigationWdw == null)
                return;
            if(_navigationWdw.Content != null)
                _navigationWdw.Close();
        }

        public void ClosePage(Page page)
        {
            _navigationWdw?.Close();
            page = null;
        }
        
    }
}