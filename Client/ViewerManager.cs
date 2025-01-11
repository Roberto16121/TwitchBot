using TwitchLib.Api;

namespace TwitchBot;

public class ViewerManager
{
    private readonly TwitchAPI _twitchAPI;
    private readonly string _broadcasterId;

    public ViewerManager(TwitchAPI twitchAPI, string broadcasterId)
    {
        _twitchAPI = twitchAPI;
        _broadcasterId = broadcasterId;
    }

    public async Task<ActiveViewer> GetViewerAsync(string username)
    {
        var userResponse = await _twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { username });
        var user = userResponse.Users.FirstOrDefault();
        if (user == null) return null;

        string id = user.Id;
        ViewerType type = ViewerType.Normal;
        if (id == _broadcasterId)
            type = ViewerType.Broadcaster;
        

        var mods = await _twitchAPI.Helix.Moderation.GetModeratorsAsync(_broadcasterId, new List<string> { id });
        if (mods.Data.Any() && type == ViewerType.Normal)
            type = ViewerType.Moderator;

        var vips = await _twitchAPI.Helix.Channels.GetVIPsAsync(_broadcasterId);
        if (vips.Data.Any(v => v.UserId == id) && type == ViewerType.Normal)
            type = ViewerType.VIP;

        var subs = await _twitchAPI.Helix.Subscriptions.GetUserSubscriptionsAsync
            (broadcasterId: _broadcasterId, new List<string>{id});
        if (subs.Data.Any(s => s.UserId == id) && type == ViewerType.Normal)
            type = ViewerType.Subscriber;

        return new ActiveViewer(username, type, GetColorForType(type));
    }

    private string GetColorForType(ViewerType type) =>
        type switch
        {
            ViewerType.Broadcaster => Client.BroadcasterColor,
            ViewerType.Moderator => Client.ModeratorColor,
            ViewerType.VIP or ViewerType.Subscriber => Client.SubscriberColor,
            _ => Client.NormalColor
        };
}