

using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public class ChatMessage
{
    public string username = "";
    public string messageText = "";
    public DateTime sentTime;
    public bool isModerator;
    public string userColor = "";
    public string channel = "";
    public string messageId = "";
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
