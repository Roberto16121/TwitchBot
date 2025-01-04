

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
