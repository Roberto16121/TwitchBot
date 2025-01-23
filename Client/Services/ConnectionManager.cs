using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TwitchBot;

public class ConnectionManager
{
    public TwitchClient TwitchClient { get; private set; }
    public TwitchAPI TwitchAPI { get; private set; }

    public ConnectionManager(string username, string accessToken, string clientId)
    {
        var credentials = new ConnectionCredentials(username, accessToken);

        TwitchAPI = new TwitchAPI
        {
            Settings =
            {
                ClientId = clientId,
                AccessToken = accessToken
            }
        };

        var clientOptions = new ClientOptions()
        {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };

        var socketClient = new WebSocketClient(clientOptions);
        TwitchClient = new TwitchClient(socketClient);
        TwitchClient.Initialize(credentials, username);
    }

    public void Connect()
    {
        if (!TwitchClient.Connect())
        {
            throw new Exception("Error when connecting to Twitch");
        }
    }
}