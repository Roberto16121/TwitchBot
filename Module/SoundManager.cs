using System.Windows;

namespace TwitchBot;

public class SoundManager
{
    public string SoundLocation { get; private set; } = "";
    public int SoundVolume { get; private set; }
    public bool LoopSound { get; private set; }
    public int LoopCount { get; private set; }
    
    
    public void SetLocation(string path) =>
        SoundLocation = path;

    public void SetSoundVolume(int value) =>
        SoundVolume = value;

    public void SetSoundVolume(string value)
    {
        try
        {
            SoundVolume = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Invalid input");
        }
    }

    public void SetLoopingSound(bool value) =>
        LoopSound = value;

    public void SetLoopCount(int value) =>
        LoopCount = value;

    public void SetLoopCount(string value)
    {
        try
        {
            LoopCount = int.Parse(value);
        }
        catch (Exception e)
        {
            MessageBox.Show("Invalid input");
        }
    }


}