using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TwitchBot.UI_Parts;
using TwitchLib.Api.Helix.Models.Extensions.ReleasedExtensions;
using TwitchLib.Client.Events;

namespace TwitchBot
{
    public class ChatHandler
    {
        public ObservableQueue<ChatMessage> Messages { get; private set; } = [];

        public ObservableCollection<ActiveViewer> Users { get; private set; } = [];

        public EventHandler<OnMessageReceivedArgs> MessageReceived;

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
            if(Messages.Count > 50)
            {
                Messages.Dequeue();
            }
            MessageReceived?.Invoke(this, e);
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

        public void AddUser(ActiveViewer viewer)
        {
            if(Users.Contains(viewer)) 
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                int insertIndex = 0;

                for (; insertIndex < Users.Count; insertIndex++)
                {
                    var existingViewer = Users[insertIndex];

                    if (GetPriority(viewer) < GetPriority(existingViewer))
                        break;
                }

                Users.Insert(insertIndex, viewer);
            });

            
        }

        public void DeleteUser(string username)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ActiveViewer viewer = Users.First(user => user.username.Equals(username));
                Users.Remove(viewer);
            });
        }

        private int GetPriority(ActiveViewer viewer)
        {
            return viewer.viewerType switch
            {
                ViewerType.Broadcaster => 0,
                ViewerType.Moderator => 1,
                ViewerType.VIP => 2,
                ViewerType.Subscriber => 3,
                ViewerType.Normal => 4,
                _ => 5,
            };
        }

    }
}
