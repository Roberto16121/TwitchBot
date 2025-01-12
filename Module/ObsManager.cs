using System.Windows;

namespace TwitchBot;

public class ObsManager
{
    public string SourceName { get; private set; } = "Test";
    public int Duration { get; private set; } = 1;
    public bool Loop { get; private set; }
    public int Count { get; private set; }

    public void SetSourceName(string name) =>
        SourceName = name;

    public void SetDuration(string value)
    {
        try
        {
            Duration = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Invalid Input");
        }
    }

    public void SetDuration(int value) =>
        Duration = value;

    public void SetLoop(bool value) =>
        Loop = value;

    public void SetLoopCount(string value)
    {
        try
        {
            Count = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Invalid Input");
        }
    }

    public void SetLoopCount(int value) =>
        Count = value;
}