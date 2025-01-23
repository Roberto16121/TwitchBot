namespace TwitchBot.Services;

public class ServiceManager
{
    public ConnectionManager ConnectionManager { get; }
    public ViewerManager ViewerManager { get; }
    public ModerationManager ModerationManager { get; }
    public ChatEventManager ChatEventManager { get; }
    public StreamInfoUpdater StreamInfoUpdater { get; }

    public ServiceManager(TwitchConfiguration configuration, ChatHandler chatHandler)
    {
        ConnectionManager = new ConnectionManager(configuration.Username, configuration.AccessToken, configuration.ClientId);
        ConnectionManager.Connect();

        ViewerManager = new ViewerManager(ConnectionManager.TwitchAPI, configuration.BroadcasterId);
        ModerationManager = new ModerationManager(ConnectionManager.TwitchAPI,
            configuration.BroadcasterId, configuration.AccessToken, chatHandler);
        ChatEventManager = new ChatEventManager(ConnectionManager.TwitchClient, ViewerManager, chatHandler);
        StreamInfoUpdater = new StreamInfoUpdater(ConnectionManager.TwitchAPI, configuration.Username);
    }
}