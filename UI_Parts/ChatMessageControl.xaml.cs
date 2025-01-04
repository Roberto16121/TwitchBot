using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwitchLib.Client.Events;

namespace TwitchBot.UI_Parts
{
    public partial class ChatMessageControl : UserControl
    {
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
                control.UsernameText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(message.userColor));
            }
        }
    }
}
