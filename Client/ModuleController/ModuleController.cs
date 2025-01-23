namespace TwitchBot.ModuleHandler;

public class ModuleController
{
    public SoundController SoundController { get; } = new();
    public ObsController ObsController { get; } = new();
}