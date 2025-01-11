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
    public ObsController()
    {
        obs = new();
        Connect();

        obs.Connected += OnConnected;
        obs.Disconnected += OnDisconnected;
    }

    public async Task GetScene()
    {
        if (!Connected)
            return;
        var sceneName = "Scene"; //setat in Obs Module
        var scene = obs.GetSceneItemId(sceneName, "Test", -1);

        
        
        var test = obs.GetSourceActive("Test");
        if (test == null)
        {
            MessageBox.Show("Source not found");
            return;
        }
        obs.SetSceneItemEnabled(sceneName, scene, true);
        await Task.Delay(3 * 1000);//3 -> secunde setate in Obs Module
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
}