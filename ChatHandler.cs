using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TwitchBot.UI_Parts;
using TwitchLib.Client.Events;

namespace TwitchBot
{
    internal class ChatHandler
    {
        public ObservableCollection<ChatMessage> Messages { get; private set; } = new ObservableCollection<ChatMessage>();

        public ChatHandler()
        {

        }

        public void AddMessage(OnMessageReceivedArgs e)
        {
            Messages.Add(new ChatMessage
            {
                username = e.ChatMessage.Username,
                messageText = e.ChatMessage.Message,
                sentTime = DateTime.Now,
                isModerator = e.ChatMessage.IsModerator,
                userColor = e.ChatMessage.ColorHex
            });
        }

        public void AddMessage(string username, string messageText)
        {
            Messages.Add(new ChatMessage
            {
                username = username,
                messageText = messageText,
                sentTime = DateTime.Now,
                isModerator = true,
                userColor = "#00FFFF"
            });
        }
        
    }
}
