using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class ConnectToServer : MonoBehaviour
{
    [Header("Inputs")]
    /// <summary>
    ///  Gets the name of the player
    /// </summary>
    [SerializeField] private Text playerName;
    /// <summary>
    ///  Used to get the server ip
    /// </summary>
    [SerializeField] private Text serverIp;
    
    [Header("Canvas")]
    /// <summary>
    ///  Input canvas
    /// </summary>
    [SerializeField] private Canvas inputCanvas;
    /// <summary>
    ///  Shows the connecting text
    /// </summary>
    [SerializeField] private Canvas connectingCanvas;

    [Header("Network")]
    /// <summary>
    ///  Handles all network related traffic of the client
    /// </summary>
    [SerializeField] private ClientNetworkManager networkManager;

    /// <summary>
    ///  Used to change the scene once an connection occoured
    /// </summary>
    private SceneLoader sceneLoader;


    /// <summary>
    ///  Disables the connecting canvas at the start of the scene
    /// </summary>
    private void Start()
    {
        this.connectingCanvas.enabled = false;
        this.sceneLoader = this.GetComponent<SceneLoader>();
    }

    /// <summary>
    ///  Tries to join a server lobby
    /// </summary>
    public void JoinLobby()
    {
        var serverAdress = this.serverIp.text;
        var playerName = this.playerName.text;

        if (string.IsNullOrEmpty(serverAdress) || string.IsNullOrEmpty(playerName))
        {
            return;
        }

        this.inputCanvas.enabled = false;
        this.connectingCanvas.enabled = true;

        this.networkManager.networkAddress = serverAdress;
        this.networkManager.ingameName = playerName;
        this.networkManager.StartClient();
    }

    /// <summary>
    ///  Will be called if the connection was successful
    /// </summary>
    public void OnConnectionSuccess()
    {
        this.sceneLoader.LoadScene();
    }

    /// <summary>
    ///  Will be called if the connection failed
    /// </summary>
    public void OnConnectionFailed()
    {
        this.inputCanvas.enabled = true;
        this.connectingCanvas.enabled = false;
    }
}
