using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyBehaviour : NetworkBehaviour
{
    /// <summary>
    ///  What is the name of the player
    /// </summary>
    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string displayName = "Loading...";
    public void HandleDisplayNameChanged(string oldName, string newName) => this.UpdateUI();
    /// <summary>
    ///  What is the ready state of the player
    /// </summary>
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool isReady = false;
    public void HandleReadyStatusChanged(bool oldReadyStatus, bool newReadyStatus) => this.UpdateUI();

    [Header("UI")]
    /// <summary>
    ///  Prefab of the lobby UI
    /// </summary>
    [SerializeField] private PlayerLobbyUI lobbyUIPrefab = null;
    /// <summary>
    ///  Used to start the game. Will be disabled if not enought players are in the lobby
    /// </summary>
    private Button startGameButton = null;
    /// <summary>
    ///  Used to change the ready state of the player
    /// </summary>
    private Button readyButton = null;
    /// <summary>
    ///  Stores the instance of the lobby UI
    /// </summary>
    private PlayerLobbyUI lobbyUI;

    /// <summary>
    ///  Stores a list of all clients currently connected to the server
    /// </summary>
    private static List<LobbyBehaviour> clients { get; } = new List<LobbyBehaviour>();

    /// <summary>
    ///  Stores a reference to the server.
    ///  This will be only available on the server
    /// </summary>
    private Server server = null;
    private Server Server
    {
        get
        {
            if (this.server != null) return this.server;
            else return this.server = NetworkManager.singleton as Server;
        }
    }


    /// <summary>
    ///  Will be called if a player joins the server
    /// </summary>
    public override void OnStartClient()
    {
        LobbyBehaviour.clients.Add(this);

        this.UpdateUI();
    }

    /// <summary>
    ///  Will be called if a player disconnects from the server
    /// </summary>
    public override void OnStopClient()
    {
        LobbyBehaviour.clients.Remove(this);

        this.UpdateUI();
    }

    /// <summary>
    ///  Will be called if the client gets authority over the lobby behaviour 
    /// </summary>
    public override void OnStartAuthority()
    {
        Debug.Log("On Start Authority");

        this.lobbyUI = Instantiate(this.lobbyUIPrefab);

        this.readyButton = this.lobbyUI.transform.Find("Ready").GetComponent<Button>();
        this.readyButton.interactable = true;
        this.readyButton.onClick.AddListener(this.OnReadyClick);

        this.startGameButton = this.lobbyUI.transform.Find("StartGame").GetComponent<Button>();
        this.startGameButton.interactable = false;
        this.startGameButton.onClick.AddListener(this.OnReadyClick);

        this.CmdSetDisplayName(PlayerInformation.playerName);
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
        this.Server.NotifyPlayersOfReadyState();
    }

    /// <summary>
    ///  Send to all players if the game can be started
    /// </summary>
    /// <param name="readyToStartGame"></param>
    public void HandleReadyToStartGame(bool readyToStartGame)
    {
        if (!this.hasAuthority)
        {
            return;
        }

        this.startGameButton.interactable = readyToStartGame;
    }

    /// <summary>
    ///  Updates the player UI
    /// </summary>
    private void UpdateUI()
    {
        Debug.Log("Called Dispaly Draw");
        if (!this.hasAuthority)
        {
            foreach (var client in LobbyBehaviour.clients)
            {
                if (client.hasAuthority)
                {
                    client.UpdateUI();
                }
            }

            return;
        }

        var amountOfClients = LobbyBehaviour.clients.Count;
        for (var i = 0; i < this.lobbyUI.playerUIElements.Count; i++)
        {
            var uiElement = this.lobbyUI.playerUIElements[i];
            uiElement.gameObject.SetActive(i < amountOfClients);

            if (i < amountOfClients)
            {
                var playerLobbyUiElement = uiElement.GetComponent<PlayerLobbyUIElement>();
                var clientBehaviour = LobbyBehaviour.clients[i];
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
