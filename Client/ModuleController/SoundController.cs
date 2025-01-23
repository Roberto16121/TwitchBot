using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TwitchBot;

public class SoundController
{
    public static SoundController Instance { get; private set; }
    private SoundManager _manager;
    private MediaPlayer _player;
    
    public SoundController()
    {
        Instance = this;
        _player = new();
    }

    private bool _busy;
    public async Task Execute(SoundManager manager)
    {
        if (_busy)
            return;
        _manager = manager;
        count = 0;
        await LoadSound();
    }
    [DllImport("NAudio.dll")]
    private static extern uint mciSendString(
        string command,
        StringBuilder returnValue,
        int returnLength,
        IntPtr winHandle);

    private int count = 0;
    private async Task LoadSound()
    {
        _busy = true;
        _player.Open(new Uri(_manager.SoundLocation));
        _player.Play();
        _player.Volume = _manager.SoundVolume;

        _player.MediaEnded += SoundEnded;
    }
    
    private async void SoundEnded(object? sender, EventArgs e)
    {
        count++;
        if (!_manager.LoopSound)
        {
            _busy = false;
            return;
        }

        if (count < _manager.LoopCount)
            await LoadSound();
        else _busy = false;
    }
    

}