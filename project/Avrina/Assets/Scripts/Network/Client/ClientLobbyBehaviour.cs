using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClientLobbyBehaviour : NetworkBehaviour
{
    /// <summary>
    ///  What is the name of the player
    /// </summary>
    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string displayName = "Loading...";
    public void HandleDisplayNameChanged(string oldName, string newName) => this.UpdateDisplay();
    /// <summary>
    ///  What is the ready state of the player
    /// </summary>
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool isReady = false;
    public void HandleReadyStatusChanged(bool oldReadyStatus, bool newReadyStatus) => this.UpdateDisplay();

    [Header("UI")]
    /// <summary>
    ///  Stores an prefab to the lobby ui
    /// </summary>
    [SerializeField] private Canvas lobbyUI = null;
    /// <summary>
    ///  Used to start the game. Will be disabled if not enought players are in the lobby
    /// </summary>
    private Button startGameButton = null;
    /// <summary>
    ///  Used to change the ready state of the player
    /// </summary>
    private Button readyButton = null;
    /// <summary>
    ///  Stores all ui elements where the players are shown
    /// </summary>
    private List<GameObject> playerUi = null;
    
    [Header("Other")]
    /// <summary>
    ///  Will be opened if the player loses connection to the server
    /// </summary>
    [SerializeField] private SceneLoader onDisconnect = null;
    /// <summary>
    ///  Stores the tag all player lobby objects
    /// </summary>
    [SerializeField] private string objectTag = null;

    /// <summary>
    ///  Stores a reference to the client network manager object.
    ///  Will only be set on the client
    /// </summary>
    private ClientNetworkManager client;
    /// <summary>
    ///  Stores a reference to the server network manager object.
    ///  Will only be set on the server
    /// </summary>
    private ServerNetworkManager server;


    /// <summary>
    ///  Sets the server instance
    /// </summary>
    public override void OnStartClient()
    {
        Debug.Log("Client Started");
        this.client = NetworkManager.singleton as ClientNetworkManager;
        this.CmdSetServerManager();

        if (!this.hasAuthority)
        {
            return;
        }

        this.CreateUI();
    }

    /// <summary>
    ///  Sets the display name of the client
    /// </summary>
    [ClientRpc]
    public void RpcSetDisplayName()
    {
        if (!this.hasAuthority)
        {
            return;
        }

        this.CmdSetDisplayName(this.client.ingameName);
    }

    /// <summary>
    ///  Creates the local UI
    /// </summary>
    private void CreateUI()
    {
        // Create local instance of the UI
        var lobbyUI = Instantiate(this.lobbyUI);

        // Disable the start button
        this.startGameButton = lobbyUI.transform.Find("StartGame").GetComponent<Button>();
        this.startGameButton.interactable = false;

        // Make ready button usable
        this.readyButton = lobbyUI.transform.Find("Ready").GetComponent<Button>();
        this.readyButton.onClick.AddListener(this.OnReadyClick);

        // Get all player ui elements
        var players = lobbyUI.transform.Find("Players");
        this.playerUi = new List<GameObject>();
        for (var i = 1; i <= 8; i++)
        {
            this.playerUi.Add(players.Find("Player" + i).gameObject);
        }
    }

    /// <summary>
    ///  Will be called if the client disconnects from the server
    /// </summary>
    public override void OnStopClient()
    {
        Debug.Log("Connection Lost");
        this.CmdRemovePlayer();
        this.onDisconnect.LoadScene();

        base.OnStopClient();
    }

    /// <summary>
    ///  Will be called if the ready button is pressed
    /// </summary>
    private void OnReadyClick()
    {
        var nextState = !this.isReady;

        this.CmdReadUp();

        if (nextState)
        {
            this.readyButton.GetComponentInChildren<Text>().text = "<color=green>Ready</color>";
        }
        else
        {
            this.readyButton.GetComponentInChildren<Text>().text = "<color=red>Not Ready</color>";
        }
    }

    /// <summary>
    ///  Sets the display name of the player
    /// </summary>
    /// <param name="displayName"></param>
    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    /// <summary>
    ///  Sets the display name of the player
    /// </summary>
    /// <param name="displayName"></param>
    [Command]
    private void CmdReadUp()
    {
        this.isReady = !this.isReady;
        this.server.NotifyPlayersOfReadyState();
    }

    /// <summary>
    ///  Sets the server manager on the server.
    ///  Only on the server not on the clients
    /// </summary>
    [Command]
    private void CmdSetServerManager()
    {
        this.server = NetworkManager.singleton as ServerNetworkManager;
    }

    /// <summary>
    ///  Removes the player instance from the online player list
    /// </summary>
    /// <param name="client"></param>
    [Command]
    private void CmdRemovePlayer()
    {
        this.server.players.Remove(this);
    }

    /// <summary>
    ///  Send to all players if the game can be started
    /// </summary>
    /// <param name="readyToStartGame"></param>
    [ClientRpc]
    public void RpcHandleReadyToStartGame(bool readyToStartGame)
    {
        if (!this.hasAuthority)
        {
            return;
        }

        this.startGameButton.interactable = readyToStartGame;
    }

    /// <summary>
    ///  Updates the display of the client
    /// </summary>
    [ClientCallback]
    public void UpdateDisplay(GameObject[] givenPlayers = null)
    {
        if (!hasAuthority)
        {
            if (givenPlayers == null)
            {
                var tmpPlayers = GameObject.FindGameObjectsWithTag(this.objectTag);
                foreach (var player in tmpPlayers)
                {
                    player.GetComponent<ClientLobbyBehaviour>().UpdateDisplay(tmpPlayers);
                }
            }

            return;
        }

        Debug.Log("Called Dispaly Draw");

        var players = GameObject.FindGameObjectsWithTag(this.objectTag);

        var amountOfPlayers = players.Length;
        for (var i = 0; i < this.playerUi.Count; i++)
        {
            this.playerUi[i].SetActive(i < amountOfPlayers);

            if (i < amountOfPlayers)
            {
                var playerLobbyUiElement = this.playerUi[i].GetComponent<PlayerLobbyUIElement>();
                var clientBehaviour = players[i].GetComponent<ClientLobbyBehaviour>();
                playerLobbyUiElement.playerNameObject.text = clientBehaviour.displayName;

                var readyText = "<color=red>N</color>";
                if (clientBehaviour.isReady)
                {
                    readyText = "<color=green>Y</color>";
                }
                playerLobbyUiElement.isReadyObject.text = readyText;
            }
        }
    }
}
