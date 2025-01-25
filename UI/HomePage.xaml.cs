using System.Windows.Controls;
using System.Windows.Input;

namespace TwitchBot.UI;

public partial class HomePage : Page
{
    private Client _client;
    public HomePage(Client client)
    {
        InitializeComponent();
        _client = client;
        DataContext = _client.ChatHandler;
    }

    private void MessageBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if(MessageBox.Text != String.Empty)
            {
                _client.ServiceManager.ChatEventManager.SendMessage(_client.Configuration.Username, MessageBox.Text);
                _client.ChatHandler.AddMessage(_client.Configuration.Username, MessageBox.Text);
                MessageBox.Text = String.Empty;
            }
        }
    }

    public void ScrollIntoView()
    {
        var scrollViewer = Helper.GetScrollViewer(ChatBox);

        bool isAtBottom = scrollViewer.VerticalOffset + scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight;
        if (!isAtBottom)
            return;

        ChatBox.SelectedIndex = ChatBox.Items.Count - 1;
        ChatBox.ScrollIntoView(ChatBox.SelectedItem);
    }
}