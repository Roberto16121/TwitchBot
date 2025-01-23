
using System.Windows;
using TwitchBot.Database;
using TwitchBot.ModuleHandler;
using TwitchBot.Services;
using TwitchBot.UI_Parts;

namespace TwitchBot
{
    class Client
    {
        public readonly ModuleManager ModuleManager;
        public readonly ChatHandler ChatHandler;
        public readonly Database.Database Database;
        public readonly ServiceManager ServiceManager;
        public readonly ModuleController ModuleController;
        public readonly TwitchConfiguration Configuration;
        
        public static string BroadcasterColor = "#FF0000";
        public static string ModeratorColor = "#00FF00";
        public static string SubscriberColor = "#F020D8";
        public static string NormalColor = "#000000";

        public static Client Instance { get; private set; }
        

        public Client()
        {
            Configuration = new();
            Instance ??= this;
            
            ChatHandler = new ChatHandler();

            ServiceManager = new(Configuration, ChatHandler);
            
            ServiceManager.StreamInfoUpdater.GetEvent() += HandleStreamInfoUpdated;

            ModuleController = new();
            ModuleManager = new(ChatHandler);
            
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
        

        private void HandleStreamInfoUpdated(string title, int viewerCount, DateTime startedAt)
        {
            // Update the UI or handle the stream info as needed
            Application.Current.Dispatcher.Invoke(() =>
            {
                StreamInformationControl.Instance.UpdateInfo(title, viewerCount, startedAt);
            });
        }
        

    }
}
