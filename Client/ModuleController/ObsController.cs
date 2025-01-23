using System.Text;
using System.Windows;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types;

namespace TwitchBot;

public class ObsController
{
    private readonly OBSWebsocket obs;
    private bool Connected;
    private ObsManager _manager;
    public static ObsController Instance { get; private set; }
    public ObsController()
    {
        obs = new();
        Connect();
        Instance = this;
        obs.Connected += OnConnected;
        obs.Disconnected += OnDisconnected;
    }

    private async Task GetScene()
    {
        if (!Connected)
            return;
        var sceneName = "TwitchBot"; //setat in Obs Module
        var scene = obs.GetSceneItemId(sceneName, _manager.SourceName, -1);

        
        
        var test = obs.GetSourceActive(_manager.SourceName);
        if (test == null)
        {
            MessageBox.Show("Source not found");
            return;
        }
        obs.SetSceneItemEnabled(sceneName, scene, true);
        await Task.Delay(_manager.Duration * 1000);
        obs.SetSceneItemEnabled(sceneName, scene, false);
        

    }

    private void OnConnected(object sender, EventArgs e)
    {
        Connected = true;
        return;
        var t = obs.GetCurrentProgramScene();
        StringBuilder sb = new(t);

        MessageBox.Show(sb.ToString());
    }

    private void OnDisconnected(object sender, ObsDisconnectionInfo e)
    {
        Connected = false;
    }
    
    public void Connect()
    {
        obs.ConnectAsync("ws://localhost:4455", TwitchCredential.Password);
    }

    public async Task Execute(ObsManager manager)
    {
        _manager = manager;
        await GetScene();
    }
}