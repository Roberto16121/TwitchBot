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
        public ObservableQueue<ChatMessage> Messages { get; private set; } = [];

        public ChatHandler()
        {

        }

        public void AddMessage(OnMessageReceivedArgs e)
        {
            Messages.Enqueue(new ChatMessage
            {
                username = e.ChatMessage.Username,
                messageText = e.ChatMessage.Message,
                sentTime = DateTime.Now,
                isModerator = e.ChatMessage.IsModerator || e.ChatMessage.IsBroadcaster,
                userColor = e.ChatMessage.ColorHex,
                channel = e.ChatMessage.Channel,
                messageId = e.ChatMessage.Id
            });
            if(Messages.Count > 5)
            {
                Messages.Dequeue();
            }
        }

        public void AddMessage(string username, string messageText)
        {
            Messages.Enqueue(new ChatMessage
            {
                username = username,
                messageText = messageText,
                sentTime = DateTime.Now,
                isModerator = true,
                userColor = "#FF0000"
            });
        }
        
    }
}
