using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Moderation.BanUser;

namespace TwitchBot;

public class ModerationManager
{
    public static ModerationManager Instance { get; private set; }
    private readonly TwitchAPI _twitchAPI;
    private readonly string _broadcasterId;
    private string _accessToken;

    private readonly ChatHandler _chat;
    public ModerationManager(TwitchAPI twitchAPI, string broadcasterId, string token, ChatHandler handler)
    {
        Instance ??= this;
        _twitchAPI = twitchAPI;
        _broadcasterId = broadcasterId;
        _accessToken = token;
        _chat = handler;
    }

    public async Task BanUser(string userId, string reason)
    {
        await _twitchAPI.Helix.Moderation.BanUserAsync(_broadcasterId, _broadcasterId, new BanUserRequest()
        {
            UserId = userId,
            Reason = reason
        });
        DeleteAllMessages(userId);
    }

    public async Task TimeoutUser(string userId, int duration, string reason)
    {
        await _twitchAPI.Helix.Moderation.BanUserAsync(_broadcasterId, _broadcasterId, new BanUserRequest
        {
            UserId = userId,
            Duration = duration,
            Reason = reason
        });
        DeleteAllMessages(userId);
    }

    void DeleteAllMessages(string userId)
    {
        ObservableQueue<ChatMessage> ToReturn = new();
        foreach (var message in _chat.Messages)
        {
            if (message.userId != userId)
                ToReturn.Enqueue(new ChatMessage()
                {
                    userId = message.userId,
                    username = message.username,
                    messageText = message.messageText,
                    sentTime = message.sentTime,
                    isModerator = message.isModerator,
                    userColor = message.userColor,
                    messageId = message.messageId
                });
        }
        _chat.DeleteAllMessages(userId);
    }
    
    

    public async void DeleteMessage(string messageId)
    {
        await _twitchAPI.Helix.Moderation.DeleteChatMessagesAsync
            (_broadcasterId, _broadcasterId, messageId, _accessToken);
        _chat.DeleteMessageLocally(messageId);
    }
}