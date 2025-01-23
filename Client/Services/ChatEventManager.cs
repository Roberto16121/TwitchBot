using TwitchBot.Database;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace TwitchBot;

public class ChatEventManager
{
    private readonly ViewerManager _viewerManager;
    private readonly ChatHandler _chatHandler;
    private readonly TwitchClient _twitchClient;

    public event Action<OnMessageReceivedArgs>? OnMessageReceived;

    public ChatEventManager(TwitchClient twitchClient, ViewerManager viewerManager, ChatHandler chatHandler)
    {
        _viewerManager = viewerManager;
        _chatHandler = chatHandler;

        _twitchClient = twitchClient;
        
        twitchClient.OnMessageReceived += HandleMessageReceived;
        twitchClient.OnUserJoined += HandleUserJoined;
        twitchClient.OnUserLeft += HandleUserLeft;
        twitchClient.OnMessageCleared += TwitchClientOnMessageCleared;
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
    }

    private void HandleUserLeft(object? sender, OnUserLeftArgs e)
    {
        _chatHandler.DeleteUser(e.Username);
    }
}