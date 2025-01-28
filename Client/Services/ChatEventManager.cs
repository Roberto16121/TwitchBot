using System.Windows.Threading;
using TwitchBot.Database;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace TwitchBot;

public class ChatEventManager
{
    private readonly ViewerManager _viewerManager;
    private readonly ChatHandler _chatHandler;
    private readonly TwitchClient _twitchClient;
    
    DispatcherTimer timer = new();

    public event Action<OnMessageReceivedArgs>? OnMessageReceived;
    //On left, Timer 5min, On App closes -> save ViewTime

    private List<UserTime> _userTimes = new();
    
    public ChatEventManager(TwitchClient twitchClient, ViewerManager viewerManager, ChatHandler chatHandler)
    {
        _viewerManager = viewerManager;
        _chatHandler = chatHandler;

        _twitchClient = twitchClient;
        
        twitchClient.OnMessageReceived += HandleMessageReceived;
        twitchClient.OnUserJoined += HandleUserJoined;
        twitchClient.OnUserLeft += HandleUserLeft;
        twitchClient.OnMessageCleared += TwitchClientOnMessageCleared;
        
        SetupTimer();
    }

    private void TwitchClientOnMessageCleared(object? sender, OnMessageClearedArgs e)
    {
        _chatHandler.DeleteMessageLocally(e.TargetMessageId);
    }

    public void SendMessage(string username, string text) =>
        _twitchClient.SendMessage(username, text);

    private void HandleMessageReceived(object? sender, OnMessageReceivedArgs e)
    {
        OnMessageReceived?.Invoke(e);
    }

    private async void HandleUserJoined(object? sender, OnUserJoinedArgs e)
    {
        var viewer = await _viewerManager.GetViewerAsync(e.Username);
        _chatHandler.AddUser(viewer);
        await using var context = new AppDbContext();
        await context.AddNewUser(viewer.userId, viewer.viewerType.ToString(), e.Username);
        _userTimes.Add(new UserTime{Username = e.Username, UserId = viewer.userId, Connected = DateTime.Now});
    }

    private void HandleUserLeft(object? sender, OnUserLeftArgs e)
    {
        _chatHandler.DeleteUser(e.Username);
        var user = _userTimes.Find(u => u.Username == e.Username);
        if (user == null)
            return;
        _ = UpdateUserTime(user.UserId);
    }
    
    private void SetupTimer()
    {
        timer.Tick += new EventHandler(Timer_Tick);
        timer.Interval = new TimeSpan(0, 5, 0);
        timer.Start();
    }
    
    private async void Timer_Tick(object? sender, EventArgs e)
    {
        await UpdateAllUsers();
    }

    async Task UpdateUserTime(string userId)
    {
        int amount = 0;
        var user = _userTimes.Find(u => u.UserId == userId);
        if (user == null)
            return;
        amount = (DateTime.Now - user.Connected).Minutes;
        await using var context = new AppDbContext();
        await context.IncreaseUserViewTime(userId, amount);
        _userTimes.Remove(user);
    }

    public async Task UpdateAllUsers()
    {
        int amount;
        DateTime now = DateTime.Now;
        await using var context = new AppDbContext();
        foreach (var user in _userTimes)
        {
            amount = (now - user.Connected).Minutes;
            await context.IncreaseUserViewTime(user.UserId, amount);
        }

        await context.SaveChangesAsync();
    }
}