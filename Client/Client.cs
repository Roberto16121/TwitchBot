
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Moderation.BanUser;
using System.Windows.Threading;
using System.Windows;
using TwitchBot.UI_Parts;

namespace TwitchBot
{
    class Client
    {
        public readonly ConnectionManager ConnectionManager;
        public readonly ViewerManager ViewerManager;
        public readonly ModerationManager ModerationManager;
        public readonly ChatEventManager ChatEventManager;
        public readonly StreamInfoUpdater StreamInfoUpdater;
        public readonly ObsController ObsController;
        
        public static string BroadcasterColor = "#FF0000";
        public static string ModeratorColor = "#00FF00";
        public static string SubscriberColor = "#F020D8";
        public static string NormalColor = "#000000";


        public static Client Instance { get; private set; }

        public ChatHandler ChatHandler { get; }

        public Client()
        {
            Instance ??= this;
            ConnectionManager = new ConnectionManager(TwitchCredential.username,
                TwitchCredential.accessToken, TwitchCredential.clientID);
            ConnectionManager.Connect();

            string broadcasterId = "474003800"; // Fetch from Twitch API
            ViewerManager = new ViewerManager(ConnectionManager.TwitchAPI, broadcasterId);
            ModerationManager = new ModerationManager(ConnectionManager.TwitchAPI, broadcasterId);

            ChatHandler = new ChatHandler();

            ChatEventManager = new ChatEventManager(
                ConnectionManager.TwitchClient,
                ViewerManager,
                ChatHandler
            );
            StreamInfoUpdater = new StreamInfoUpdater(
                ConnectionManager.TwitchAPI,
                "username"
            );
            StreamInfoUpdater.OnStreamInfoUpdated += HandleStreamInfoUpdated;

            ObsController = new();
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
