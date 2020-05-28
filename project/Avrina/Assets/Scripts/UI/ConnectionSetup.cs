using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class ConnectionSetup : MonoBehaviour
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
    [SerializeField] private Client networkManager;

    /// <summary>
    ///  Used to change to the host scene once the user hosts a server
    /// </summary>
    [Scene] [SerializeField] private string serverHostScene;
    /// <summary>
    ///  Used to change the scene once an connection occoured
    /// </summary>
    [Scene] [SerializeField] private string serverLobbyScene;
    /// <summary>
    ///  Used to change the scene once an disconnect occoured
    /// </summary>
    [Scene] [SerializeField] private string serverListScene;


    /// <summary>
    ///  Disables the connecting canvas at the start of the scene
    /// </summary>
    private void Start()
    {
        this.connectingCanvas.enabled = false;
    }

    /// <summary>
    ///  Tries to join a server lobby
    /// </summary>
    public void JoinLobby()
    {
        var serverAdress = this.serverIp.text;
        var playerName = this.playerName.text;

        if (string.IsNullOrEmpty(serverAdress))
        {
            serverAdress = "localhost";
        }
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Stranger";
        }

        this.inputCanvas.enabled = false;
        this.connectingCanvas.enabled = true;

        this.networkManager.networkAddress = serverAdress;
        PlayerInformation.playerName = playerName;
        this.networkManager.StartClient();
    }

    /// <summary>
    ///  Tries to create a host server
    /// </summary>
    public void HostLobby()
    {
        var playerName = this.playerName.text;

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Stranger";
        }

        this.inputCanvas.enabled = false;
        this.connectingCanvas.enabled = true;

        PlayerInformation.playerName = playerName;
        Destroy(this.networkManager.gameObject);
        SceneManager.LoadScene(this.serverHostScene, LoadSceneMode.Single);
    }

    /// <summary>
    ///  Will be called if the connection was successful
    /// </summary>
    public void OnConnectionSuccess()
    {
        SceneManager.LoadSceneAsync(this.serverLobbyScene, LoadSceneMode.Single);
    }

    /// <summary>
    ///  Will be called if the connection failed
    /// </summary>
    public void OnConnectionFailed()
    {
        Destroy(this.networkManager.gameObject);

        SceneManager.LoadSceneAsync(this.serverListScene, LoadSceneMode.Single);
    }
}
