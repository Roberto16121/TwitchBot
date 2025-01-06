
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.Api;
using System.Windows;
using TwitchLib.Api.Helix.Models.Moderation.BanUser;

namespace TwitchBot
{
    class Client
    {
        readonly TwitchClient twitchClient;
        readonly TwitchAPI twitchAPI;
        readonly JoinedChannel Channel;
        public string username;
        string broadcasterID = "";

        public static Client Instance { get; private set; }

        public Client()
        {
            if (Instance == null)
                Instance = this;
            else return;

            Channel = new(username);
            WebSocketClient customClient = InitializeSocket();
            ConnectionCredentials credentials = new(username, TwitchCredential.accessToken);

            twitchClient = new(customClient);
            twitchClient.Initialize(credentials, username);

            twitchAPI = new();
            twitchAPI.Settings.ClientId = TwitchCredential.clientID;
            twitchAPI.Settings.AccessToken = TwitchCredential.accessToken;

            twitchClient.OnMessageReceived += Client_OnMessageReceived;

            bool connected = twitchClient.Connect();
            if (!connected)
                throw new Exception("Error when connecting");

            GetBroadcasterID();

        }

        

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

        private async void GetBroadcasterID()
        {
            await SetBroadcasterID();
        }

        private async Task SetBroadcasterID()
        {
            var user = await twitchAPI.Helix.Users.GetUsersAsync(accessToken: TwitchCredential.accessToken);
            broadcasterID = user.Users.First().Id;
        }


        #region Events

        public event Action<OnMessageReceivedArgs> OnMessageReceived;

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            OnMessageReceived?.Invoke(e);
        }

        #endregion Events


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
