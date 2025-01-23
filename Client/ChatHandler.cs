using System.Collections.ObjectModel;
using TwitchBot.Database;
using TwitchLib.Client.Events;
using Application = System.Windows.Application;

namespace TwitchBot
{
    public class ChatHandler
    {
        public ObservableQueue<ChatMessage> Messages { get; private set; } = [];

        public ObservableCollection<ActiveViewer> Users { get; private set; } = [];

        public EventHandler<OnMessageReceivedArgs>? MessageReceived;

        public async Task AddMessage(OnMessageReceivedArgs e)
        {
            Messages.Enqueue(new ChatMessage
            {
                userId = e.ChatMessage.UserId,
                username = e.ChatMessage.Username,
                messageText = e.ChatMessage.Message,
                sentTime = DateTime.Now,
                isModerator = e.ChatMessage.IsModerator || e.ChatMessage.IsBroadcaster,
                userColor = e.ChatMessage.ColorHex,
                messageId = e.ChatMessage.Id
            });
            if(Messages.Count > 50)
            {
                Messages.Dequeue();
            }
            MessageReceived?.Invoke(this, e);
            await using var context = new AppDbContext();
            bool success = await context.IncrementUserMessages(e.ChatMessage.UserId);
            if (success)
                return;
            await context.AddNewUserStatistics(e.ChatMessage.UserId, e.ChatMessage.Username);
        }

        public void AddMessage(string username, string messageText) // sent from app
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
        
        public void DeleteAllMessages(string userId)
        {
            ChatMessage[] toReturn = new ChatMessage[Messages.Count];
            Messages.CopyTo(toReturn, 0);
            Messages.Clear();
            foreach (var message in toReturn)
            {
                if (message.userId != userId)
                    Messages.Enqueue(new ChatMessage()
                    {
                        userId = message.userId,
                        username = message.username,
                        messageText = message.messageText,
                        sentTime = message.sentTime,
                        isModerator = message.isModerator,
                        userColor = message.userColor,
                        messageId = message.messageId
                    });
            }
        }
        
        public void DeleteMessageLocally(string messageId)
        {
            ChatMessage[] toReturn = new ChatMessage[Messages.Count];
            Messages.CopyTo(toReturn, 0);
            Messages.Clear();
            foreach (var message in toReturn)
            {
                if (message.messageId != messageId)
                    Messages.Enqueue(new ChatMessage()
                    {
                        userId = message.userId,
                        username = message.username,
                        messageText = message.messageText,
                        sentTime = message.sentTime,
                        isModerator = message.isModerator,
                        userColor = message.userColor,
                        messageId = message.messageId
                    });
            }
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
