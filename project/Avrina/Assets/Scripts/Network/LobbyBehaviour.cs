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
    /// <summary>
    ///  Does the local client reprensent two players
    /// </summary>
    [SyncVar(hook = nameof(HandleAmountOfPlayersChanged))]
    public bool isRepresentingTwoPlayers = false;
    public void HandleAmountOfPlayersChanged(bool oldReadyStatus, bool newReadyStatus) => this.UpdateUI();

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
    ///  Used to add remove the second player on the local client instance
    /// </summary>
    private Toggle secondPlayerToggle = null;
    /// <summary>
    ///  Stores the controlls of the first player
    /// </summary>
    private LabeledDropdown firstPlayerControlls = null;
    /// <summary>
    ///  Stores the controlls of the second player
    /// </summary>
    private LabeledDropdown secondPlayerControlls = null;
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
        var readyButtonNavigation = this.readyButton.navigation;
        readyButtonNavigation.selectOnDown = this.readyButton;
        readyButtonNavigation.selectOnUp = this.readyButton;
        this.readyButton.navigation = readyButtonNavigation;
        this.readyButton.interactable = true;
        this.readyButton.onClick.AddListener(this.OnReadyClick);

        this.firstPlayerControlls = this.lobbyUI.transform.Find("Controlls Player One").GetComponent<LabeledDropdown>();
        var firstPlayerControllsNavigation = this.firstPlayerControlls.dropdown.navigation;
        firstPlayerControllsNavigation.selectOnDown = this.firstPlayerControlls.dropdown;
        firstPlayerControllsNavigation.selectOnUp = this.firstPlayerControlls.dropdown;
        this.firstPlayerControlls.dropdown.navigation = firstPlayerControllsNavigation;
        this.firstPlayerControlls.dropdown.interactable = true;
        this.firstPlayerControlls.dropdown.onValueChanged.AddListener(this.OnPlayerOneControllsChanged);

        this.secondPlayerControlls = this.lobbyUI.transform.Find("Controlls Player Two").GetComponent<LabeledDropdown>();
        this.secondPlayerControlls.SetActive(false);
        this.secondPlayerControlls.dropdown.interactable = true;
        this.secondPlayerControlls.dropdown.onValueChanged.AddListener(this.OnPlayerTwoControllsChanged);

        this.startGameButton = this.lobbyUI.transform.Find("StartGame").GetComponent<Button>();
        this.startGameButton.interactable = false;
        var startGameButtonNavigation = this.startGameButton.navigation;
        startGameButtonNavigation.selectOnRight = this.firstPlayerControlls.dropdown;
        this.startGameButton.navigation = startGameButtonNavigation;
        this.readyButton.interactable = true;
        this.startGameButton.onClick.AddListener(this.CmdStartGame);

        this.secondPlayerToggle = this.lobbyUI.transform.Find("SecondPlayerToggle").GetComponent<Toggle>();
        this.secondPlayerToggle.interactable = true;
        this.secondPlayerToggle.onValueChanged.AddListener(this.OnSecondPlayerToggle);

        this.CmdSetDisplayName(PlayerInformation.playerName);

        this.UpdateUI();
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
            var readyButtonNavigation = this.readyButton.navigation;
            readyButtonNavigation.selectOnDown = this.startGameButton;
            readyButtonNavigation.selectOnUp = this.startGameButton;
            this.readyButton.navigation = readyButtonNavigation;
        }
        else
        {
            this.readyButton.GetComponentInChildren<Text>().text = "<color=red>Not Ready</color>";
            var readyButtonNavigation = this.readyButton.navigation;
            readyButtonNavigation.selectOnDown = this.readyButton;
            readyButtonNavigation.selectOnUp = this.readyButton;
            this.readyButton.navigation = readyButtonNavigation;
        }
    }

    /// <summary>
    ///  Will be called if the player changes if the client represents two or one player
    /// </summary>
    /// <param name="isRepresentingTwoPlayers"></param>
    private void OnSecondPlayerToggle(bool isRepresentingTwoPlayers)
    {
        this.CmdSetIsRepresentingTwoPlayers(isRepresentingTwoPlayers);

        this.secondPlayerControlls.SetActive(isRepresentingTwoPlayers);
        if (isRepresentingTwoPlayers)
        {
            this.OnPlayerTwoControllsChanged(this.secondPlayerControlls.dropdown.value);

            var firstPlayerControllsNavigation = this.firstPlayerControlls.dropdown.navigation;
            firstPlayerControllsNavigation.selectOnDown = this.secondPlayerControlls.dropdown;
            firstPlayerControllsNavigation.selectOnUp = this.secondPlayerControlls.dropdown;
            this.firstPlayerControlls.dropdown.navigation = firstPlayerControllsNavigation;

            var startGameButtonNavigation = this.startGameButton.navigation;
            startGameButtonNavigation.selectOnRight = this.secondPlayerControlls.dropdown;
            this.startGameButton.navigation = startGameButtonNavigation;
        }
        else
        {
            var firstPlayerControllsNavigation = this.firstPlayerControlls.dropdown.navigation;
            firstPlayerControllsNavigation.selectOnDown = this.firstPlayerControlls.dropdown;
            firstPlayerControllsNavigation.selectOnUp = this.firstPlayerControlls.dropdown;
            this.firstPlayerControlls.dropdown.navigation = firstPlayerControllsNavigation;

            var startGameButtonNavigation = this.startGameButton.navigation;
            startGameButtonNavigation.selectOnRight = this.firstPlayerControlls.dropdown;
            this.startGameButton.navigation = startGameButtonNavigation;
        }
    }

    /// <summary>
    ///  Will be called if the first player changes his controll scheme
    /// </summary>
    /// <param name="isRepresentingTwoPlayers"></param>
    private void OnPlayerOneControllsChanged(int controllScheme)
    {
        if (this.isRepresentingTwoPlayers && this.secondPlayerControlls.dropdown.value == controllScheme)
        {
            this.secondPlayerControlls.dropdown.value = (int) PlayerInformation.playerOneMapping;
            PlayerInformation.playerTwoMapping = PlayerInformation.playerOneMapping;
        }

        PlayerInformation.playerOneMapping = (MappingType) controllScheme;
    }

    /// <summary>
    ///  Will be called if the second player changes his controll scheme or the second player was enabled
    /// </summary>
    /// <param name="isRepresentingTwoPlayers"></param>
    private void OnPlayerTwoControllsChanged(int controllScheme)
    {
        if (this.firstPlayerControlls.dropdown.value == controllScheme)
        {
            var oldPlayerTwoMapping = (int)PlayerInformation.playerTwoMapping;

            if (oldPlayerTwoMapping == controllScheme)
            {
                if (oldPlayerTwoMapping == 0)
                {
                    this.firstPlayerControlls.dropdown.value = 1;
                }
                else
                {
                    this.firstPlayerControlls.dropdown.value = 0;
                }

                PlayerInformation.playerOneMapping = (MappingType) this.firstPlayerControlls.dropdown.value;
            }
            else
            {
                this.firstPlayerControlls.dropdown.value = oldPlayerTwoMapping;
                PlayerInformation.playerOneMapping = PlayerInformation.playerTwoMapping;
            }
        }

        PlayerInformation.playerTwoMapping = (MappingType)controllScheme;
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
    ///  Sets the variable is representing two players for all clients and the server
    /// </summary>
    /// <param name="isRepresentingTwoPlayers"></param>
    [Command]
    private void CmdSetIsRepresentingTwoPlayers(bool isRepresentingTwoPlayers)
    {
        this.isRepresentingTwoPlayers = isRepresentingTwoPlayers;
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
    ///  Will be called if the player wants to start the game
    /// </summary>
    [Command]
    public void CmdStartGame()
    {
        this.Server.StartGame();
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
    ///  Updates the player UI
    /// </summary>
    private void UpdateUI()
    {
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

        if (this.lobbyUI == null)
        {
            return;
        }

        Debug.Log("Called Dispaly Draw");

        var amountOfPlayers = 0;
        foreach (var client in LobbyBehaviour.clients)
        {
            amountOfPlayers++;
            if (client.isRepresentingTwoPlayers)
            {
                amountOfPlayers++;
            }
        }

        var clientCounter = 0;
        var nextRoutineWillBeSecondPlayer = false;
        for (var playerCounter = 0; playerCounter < this.lobbyUI.playerUIElements.Count; playerCounter++)
        {
            var uiElement = this.lobbyUI.playerUIElements[playerCounter];
            uiElement.gameObject.SetActive(playerCounter < amountOfPlayers);

            if (playerCounter < amountOfPlayers)
            {
                var playerLobbyUiElement = uiElement.GetComponent<PlayerLobbyUIElement>();
                var clientBehaviour = LobbyBehaviour.clients[clientCounter];

                playerLobbyUiElement.playerNameObject.text = clientBehaviour.displayName;
                if (!nextRoutineWillBeSecondPlayer && clientBehaviour.isRepresentingTwoPlayers)
                {
                    playerLobbyUiElement.playerNameObject.text += " (1)";
                    nextRoutineWillBeSecondPlayer = true;
                    clientCounter--;
                }
                else if (nextRoutineWillBeSecondPlayer)
                {
                    playerLobbyUiElement.playerNameObject.text += " (2)";
                    nextRoutineWillBeSecondPlayer = false;
                }
                
                var readyText = "<color=red>N</color>";
                if (clientBehaviour.isReady)
                {
                    readyText = "<color=green>Y</color>";
                }
                playerLobbyUiElement.isReadyObject.text = readyText;

                clientCounter++;
            }
        }
    }

    /// <summary>
    ///  Will be called if the gameobject is destroyed
    /// </summary>
    private void OnDestroy()
    {
        if (!this.hasAuthority)
        {
            return;
        }

        Destroy(this.lobbyUI);
    }
}
