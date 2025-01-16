

using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public class ChatMessage
{
    public string username = "";
    public string userId = "";
    public string messageText = "";
    public DateTime sentTime;
    public bool isModerator;
    public bool isSubscriber;
    public bool isVIP;
    public string userColor = "";
    public string messageId = "";
}

public class ActiveViewer
{
    public string userId = "";
    public string username = "";
    public ViewerType viewerType;
    public string userColor = "";

    public ActiveViewer(string id , string name, ViewerType type, string color)
    {
        userId = id;
        username = name;
        viewerType = type;
        userColor = color;
    }
}

public enum ViewerType
{
    Broadcaster = 0,
    Moderator = 1,
    VIP = 2,
    Subscriber = 3,
    Normal = 4
}

public enum ActionType
{
    Sound = 0,
    Obs = 1,
}

public class ModuleData
{
    public bool Enabled { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public HashSet<string> Keywords { get; set; } = new();
    public ActionType Type { get; set; }
    public CooldownData Cooldown { get; set; } = new();
    public SoundData? Sound { get; set; }
    public ObsData? Obs { get; set; }
}

public class CooldownData
{
    public int Cooldown { get; set; }
    public bool CustomCooldown { get; set; }
    public int ModeratorCooldown { get; set; }
    public int VipCooldown { get; set; }
    public int SubscriberCooldown { get; set; }
}

public class SoundData
{
    public string Location { get; set; } = string.Empty;
    public int Volume { get; set; }
    public bool Loop { get; set; }
    public int LoopCount { get; set; }
}

public class ObsData
{
    public string SourceName { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool Loop { get; set; }
    public int Count { get; set; }
}

public class TwitchConfig()
{
    public string Username { get; set; }
    public string AccessToken { get; set; }
    public string ClientId { get; set; }
    public string BroadcasterId { get; set; }
}


public static class Helper
{
    public static ScrollViewer GetScrollViewer(DependencyObject element)
    {
        if (element is ScrollViewer scrollViewer)
        {
            return scrollViewer;
        }

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            var child = VisualTreeHelper.GetChild(element, i);
            var result = GetScrollViewer(child);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}

public class ObservableQueue<TItem> : Queue<TItem>, INotifyCollectionChanged, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    new public void Enqueue(TItem item)
    {
        base.Enqueue(item);
        OnPropertyChanged();
        OnCollectionChanged(NotifyCollectionChangedAction.Add, item, this.Count - 1);
    }

    new public TItem Dequeue()
    {
        TItem removedItem = base.Dequeue();
        OnPropertyChanged();
        OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, 0);
        return removedItem;
    }

    new public bool TryDequeue(out TItem? result)
    {
        if (base.TryDequeue(out result))
        {
            OnPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, result, 0);
            return true;
        }
        return false;
    }

    new public void Clear()
    {
        base.Clear();
        OnPropertyChanged();
        OnCollectionChangedReset();
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, TItem item, int index)
      => this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, index));

    private void OnCollectionChangedReset()
      => this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

    private void OnPropertyChanged() => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Count)));
}

