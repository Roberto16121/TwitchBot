using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace TwitchBot;

public class TwitchConfiguration
{
    public string Username { get; private set; }
    public string AccessToken { get; private set; }
    public string ClientId { get; private set; }
    public string BroadcasterId { get; private set; }

    public TwitchConfiguration()
    {
        string filePath = "TwitchCredentia_Demo.json"; // FOR TESTING
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file not found: {filePath}");
        }

        string jsonContent = File.ReadAllText(filePath);
        var value = JsonConvert.DeserializeObject<TwitchConfig>(jsonContent);
        Username = value.Username;
        AccessToken = value.AccessToken;
        ClientId = value.ClientId;
        BroadcasterId = value.BroadcasterId;
    }
}