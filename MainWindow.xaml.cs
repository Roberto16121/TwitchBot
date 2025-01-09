using System.Windows;
using TwitchLib.Client.Events;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Controls;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Client client;


        public MainWindow()
        {
            InitializeComponent();
            client = new();

            client.OnMessageReceived += UpdateMessages;
            DataContext = client.chatHandler;
        }

        private void UpdateMessages(OnMessageReceivedArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                client.AddMessageToHandler(e);

                var scrollViewer = Helper.GetScrollViewer(ChatBox);
                if (scrollViewer == null)
                    return;

                bool isAtBottom = scrollViewer.VerticalOffset + scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight;
                if (!isAtBottom)
                    return;

                ChatBox.SelectedIndex = ChatBox.Items.Count - 1;
                ChatBox.ScrollIntoView(ChatBox.SelectedItem);
            });
        }

        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(MessageBox.Text != String.Empty)
                {
                    client.SendMessage(MessageBox.Text);
                    client.AddMessageToHandler(client.username, MessageBox.Text);
                    MessageBox.Text = String.Empty;
                }
            }
        }

        private void ModulesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}