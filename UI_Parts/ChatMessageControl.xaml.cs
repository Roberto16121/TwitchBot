using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace TwitchBot.UI_Parts
{
    public partial class ChatMessageControl : UserControl
    {

        public ChatMessage thisMessage;
        public ChatMessageControl()
        {
            InitializeComponent();
        }

        // Dependency Property for ChatMessage
        public static readonly DependencyProperty ChatMessageProperty =
            DependencyProperty.Register(nameof(ChatMessage), typeof(ChatMessage), typeof(ChatMessageControl), new PropertyMetadata(null, OnChatMessageChanged));

        public ChatMessageControl ChatMessage
        {
            get => (ChatMessageControl)GetValue(ChatMessageProperty);
            set => SetValue(ChatMessageProperty, value);
        }

        private static void OnChatMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChatMessageControl control && e.NewValue is ChatMessage message)
            {
                control.UsernameText.Text = message.username;
                control.MessageText.Text = message.messageText;
                control.TimestampText.Text = message.sentTime.ToString("hh:mm:ss tt");
                control.UsernameText.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(message.userColor == "" ? "#FF0000" : message.userColor));
                if(message.isModerator)
                {
                    control.BanButton.Visibility = Visibility.Collapsed;
                    control.TimeoutButton.Visibility = Visibility.Collapsed;
                    control.DeleteButton.Visibility = Visibility.Collapsed;
                }
                else control.ModImage.Visibility = Visibility.Collapsed;
                control.thisMessage = message;
                 
            }
        }

        #region Buttons
        private void BanButton_Click(object sender, RoutedEventArgs e)
        {
            Client.Instance.ModerationManager.BanUser(thisMessage.channel, thisMessage.username);
        }

        private void Timeout_Click(object sender, RoutedEventArgs e)
        {
            Client.Instance.ModerationManager.TimeoutUser(thisMessage.username, 1, "Moderator Decided");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (thisMessage.messageId == "")
                return;
            Client.Instance.ModerationManager.DeleteMessage(thisMessage.messageId);
        }


        #endregion Buttons

    }
}
