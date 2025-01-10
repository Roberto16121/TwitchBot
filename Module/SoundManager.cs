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

    public void SetLoopingSound(bool value) =>
        LoopSound = value;

    public void SetLoopingCount(int value)
    {
        if (LoopSound)
            LoopCount = value;
    }

}