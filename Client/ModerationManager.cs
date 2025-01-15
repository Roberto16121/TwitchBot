using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Moderation.BanUser;

namespace TwitchBot;

public class ModerationManager
{
    private readonly TwitchAPI _twitchAPI;
    private readonly string _broadcasterId;
    private string _accessToken;
    public ModerationManager(TwitchAPI twitchAPI, string broadcasterId, string token)
    {
        _twitchAPI = twitchAPI;
        _broadcasterId = broadcasterId;
        _accessToken = token;
    }

    public async Task BanUser(string username, string reason)
    {
        var userResponse = await _twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { username });
        var user = userResponse.Users.FirstOrDefault();
        if (user != null)
        {
            await _twitchAPI.Helix.Moderation.BanUserAsync(_broadcasterId, _broadcasterId, new BanUserRequest()
            {
                UserId = user.Id,
                Reason = reason
            });
        }
    }

    public async Task TimeoutUser(string username, int duration, string reason)
    {
        var userResponse = await _twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { username });
        var user = userResponse.Users.FirstOrDefault();
        if (user != null)
        {
            await _twitchAPI.Helix.Moderation.BanUserAsync(_broadcasterId, _broadcasterId, new BanUserRequest
            {
                UserId = user.Id,
                Duration = duration,
                Reason = reason
            });
        }
    }
    
    public async void DeleteMessage(string messageId) =>
        await _twitchAPI.Helix.Moderation.DeleteChatMessagesAsync
            (_broadcasterId, _broadcasterId, messageId, _accessToken);
}