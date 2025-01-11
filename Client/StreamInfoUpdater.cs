using System.Windows.Threading;
using TwitchLib.Api;

namespace TwitchBot;

public class StreamInfoUpdater
{
    private readonly TwitchAPI _twitchAPI;
    private readonly string _username;
    private readonly DispatcherTimer _timer;

    public event Action<string, int, DateTime>? OnStreamInfoUpdated;

    public StreamInfoUpdater(TwitchAPI twitchAPI, string username)
    {
        _twitchAPI = twitchAPI;
        _username = username;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(1)
        };
        _timer.Tick += TimerTick;
        _timer.Start();
    }

    private async void TimerTick(object sender, EventArgs e)
    {
        await FetchStreamInfo();
    }

    private async Task FetchStreamInfo()
    {
        var info = await _twitchAPI.Helix.Streams.GetStreamsAsync(userLogins: new List<string> { _username }, first: 1);
        if (info.Streams.Length > 0)
        {
            var stream = info.Streams[0];
            OnStreamInfoUpdated?.Invoke(stream.Title, stream.ViewerCount, stream.StartedAt);
        }
    }
}