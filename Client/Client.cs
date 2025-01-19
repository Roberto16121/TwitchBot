
using System.Windows;
using TwitchBot.Database;
using TwitchBot.UI_Parts;

namespace TwitchBot
{
    class Client
    {
        public readonly ConnectionManager ConnectionManager;
        public readonly ViewerManager ViewerManager;
        public readonly ModerationManager ModerationManager;
        public readonly ChatEventManager ChatEventManager;
        public readonly ModuleManager ModuleManager;
        public readonly ChatHandler ChatHandler;
        public readonly Database.Database Database;
        public readonly StreamInfoUpdater StreamInfoUpdater;
        public readonly ObsController ObsController;
        public readonly SoundController SoundController;
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
            ConnectionManager = new ConnectionManager(Configuration.Username,
                Configuration.AccessToken, Configuration.ClientId);
            ConnectionManager.Connect();
            
            ViewerManager = new ViewerManager(ConnectionManager.TwitchAPI, Configuration.BroadcasterId);
            ChatHandler = new ChatHandler();
            
            ModerationManager = new ModerationManager(ConnectionManager.TwitchAPI, Configuration.BroadcasterId,
                Configuration.AccessToken, ChatHandler);


            ChatEventManager = new ChatEventManager(
                ConnectionManager.TwitchClient,
                ViewerManager,
                ChatHandler
            );
            StreamInfoUpdater = new StreamInfoUpdater(
                ConnectionManager.TwitchAPI,
                Configuration.Username
            );
            StreamInfoUpdater.OnStreamInfoUpdated += HandleStreamInfoUpdated;

            ObsController = new();
            ModuleManager = new(ChatHandler);
            SoundController = new();
            
            Database = new();
            Database.InitializeUsers();
            
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
