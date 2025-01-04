
using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;

namespace TwitchBot
{
    class Client
    {
        TwitchClient twitchClient;
        public string username; 
        public Client()
        {
            this.username = TwitchCredential.username;
            ConnectionCredentials credentials = new ConnectionCredentials(username, TwitchCredential.twitchOuth);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new(clientOptions);
            twitchClient = new(customClient);
            twitchClient.Initialize(credentials, username);

            twitchClient.OnMessageReceived += Client_OnMessageReceived;

            bool connected = twitchClient.Connect();
            if (!connected)
                throw new Exception("Error when connecting");
        }

        #region Events

        public event Action<OnMessageReceivedArgs> OnMessageReceived;

        #endregion Events

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            OnMessageReceived?.Invoke(e);
            
        }

        public void SendMessage(string message)
        {
            twitchClient.SendMessage(username, message);
        }

        

    }
}
