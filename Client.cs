
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

        readonly TwitchClient twitchClient;
        readonly TwitchAPI twitchAPI;

        readonly public ChatHandler chatHandler;

        public string username;
        string broadcasterID = "474003800";

        public static Client Instance { get; private set; }

        public static string BroadcasterColor = "#FF0000";
        public static string ModeratorColor = "#00FF00";
        public static string SubscriberColor = "#F020D8";
        public static string NormalColor = "#000000";

        DispatcherTimer timer = new();

        public Client()
        {
            if (Instance == null)
                Instance = this;

            WebSocketClient customClient = InitializeSocket();
            ConnectionCredentials credentials = new(username, TwitchCredential.accessToken);


            twitchAPI = new();
            twitchAPI.Settings.ClientId = TwitchCredential.clientID;
            twitchAPI.Settings.AccessToken = TwitchCredential.accessToken;
            GetBroadcasterID();

            twitchClient = new(customClient);
            twitchClient.Initialize(credentials, username);

            twitchClient.OnMessageReceived += Client_OnMessageReceived;
            twitchClient.OnUserJoined += Client_OnUserJoined;
            twitchClient.OnUserLeft += Client_OnUserLeft;

            bool connected = twitchClient.Connect();
            if (!connected)
                throw new Exception("Error when connecting");
            InitializeTimer();
            Timer_Tick(null, null);

            chatHandler = new ChatHandler();

        }

        #region Initialize

        private WebSocketClient InitializeSocket()
        {
            this.username = TwitchCredential.username;
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            return new(clientOptions);
        }

        private void InitializeTimer()
        {
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Start();
        }


        private async void GetBroadcasterID()
        {
            await SetBroadcasterID();
        }

        private async Task SetBroadcasterID()
        {
            var user = await twitchAPI.Helix.Users.GetUsersAsync(accessToken: TwitchCredential.accessToken);
            broadcasterID = user.Users.First().Id;
        }

        #endregion Initialize


        #region Events

        public event Action<OnMessageReceivedArgs> OnMessageReceived;

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            OnMessageReceived?.Invoke(e);
        }


        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            ActiveViewer viewer = GetViewerAsync(e).Result;
            if (viewer == null)
                return;

            chatHandler.AddUser(viewer);

        }

        private void Client_OnUserLeft(object sender, OnUserLeftArgs e) =>
            chatHandler.DeleteUser(e.Username);

        private void Timer_Tick(object sender, EventArgs e) =>
            GetStreamInfo();
            

        async Task GetStreamInfo()
        {

            var info = await twitchAPI.Helix.Streams.GetStreamsAsync(userLogins: new List<string> { TwitchCredential.username }, first: 1);
            string title = $"{info.Streams[0].Title} : {info.Streams[0].GameName}";
            int count = info.Streams[0].ViewerCount;
            DateTime time = info.Streams[0].StartedAt;
            MessageBox.Show(title);
            Application.Current.Dispatcher.Invoke(() =>
            {
                StreamInformationControl.Instance.UpdateInfo(title, count, time);
            });
        }


        #endregion Events
        public void AddMessageToHandler(OnMessageReceivedArgs e) =>
          chatHandler.AddMessage(e);

        public void AddMessageToHandler(string username, string messageText) =>
            chatHandler.AddMessage(username, messageText);

        private async Task<ActiveViewer> GetViewerAsync(OnUserJoinedArgs e)
        {
            if (broadcasterID == "")
                return null;
            return await Task.Run(() => ProcessViewer(e.Username));
        }

        private ActiveViewer ProcessViewer(string name)
        {
            List<string> users = [name];    
            var userResponse = twitchAPI.Helix.Users.GetUsersAsync(logins: users).Result.Users;
            
            string id = userResponse.FirstOrDefault()?.Id;
            if (id == null)
                return null;
            if (id == broadcasterID)
                return new(name, ViewerType.Broadcaster, BroadcasterColor);
            
            ViewerType type = ViewerType.Normal;
            string color = NormalColor;

            List<string> ids = [id];


            var modsResponse = twitchAPI.Helix.Moderation.GetModeratorsAsync
                (broadcasterId: broadcasterID, ids).Result.Data;

            if (modsResponse.FirstOrDefault() != null)
                type = ViewerType.Moderator;

            var vipResponse = twitchAPI.Helix.Channels.GetVIPsAsync
                (broadcasterID, ids).Result.Data;

            if (vipResponse.FirstOrDefault() != null && type == ViewerType.Normal)
                type = ViewerType.VIP;

            var subResponse = twitchAPI.Helix.Subscriptions.GetUserSubscriptionsAsync
                (broadcasterId: broadcasterID, ids).Result.Data;

            if (subResponse.FirstOrDefault() != null && type == ViewerType.Normal)
                type = ViewerType.Subscriber;

            switch(type)
            {
                case ViewerType.Moderator: color = ModeratorColor; break;
                case ViewerType.VIP:
                case ViewerType.Subscriber: color = SubscriberColor;break;
            }

            return new(name, type, color);
        }


        public void SendMessage(string message) =>
            twitchClient.SendMessage(username, message);

        public async void BanUser(string channel, string userToBan)
        {
            var usersResponse = await twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { userToBan });
            if (usersResponse.Users.Length > 0)
            {
                string userID = usersResponse.Users[0].Id;
                var ban = new BanUserRequest();
                await twitchAPI.Helix.Moderation.BanUserAsync
                    (broadcasterID, broadcasterID, new BanUserRequest()
                    { UserId = userID, Reason = "Ai facut prostii" });

            }
        }

        public async void TimeoutUser(string channel, string userToTimeout)
        {
            var usersResponse = await twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { userToTimeout });
            if (usersResponse.Users.Length > 0)
            {
                string userID = usersResponse.Users[0].Id;
                var ban = new BanUserRequest();
                await twitchAPI.Helix.Moderation.BanUserAsync
                    (broadcasterID, broadcasterID, new BanUserRequest()
                        { UserId = userID, Duration = 30, Reason = "Ai facut prostii, nu prea mari" });

            }
        }

        public async void DeleteMessage(string messageId) =>
            await twitchAPI.Helix.Moderation.DeleteChatMessagesAsync
                (broadcasterID, broadcasterID, messageId, TwitchCredential.accessToken);

        

    }
}
