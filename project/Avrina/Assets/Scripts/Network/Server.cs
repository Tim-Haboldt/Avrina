using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class Server : NetworkManager
{
    /// <summary>
    ///  Defines the possible states of the server
    /// </summary>
    private enum ServerState
    {
        InLobby,
        InGame
    }

    /// <summary>
    ///  Defines how many players can be on the server playing
    ///  Is different to max connections
    /// </summary>
    [Header("Server Settings")]
    [SerializeField] private int minPlayers = 2;
    [SerializeField] private int maxPlayers = 8;

    /// <summary>
    ///  Define all prefabs for the client and the player object
    /// </summary>
    [Header("Prefabs")]
    [SerializeField] private LobbyBehaviour lobbyBehaviourPrefab = null;
    [SerializeField] private PlayerStateManager playerBehaviourPrefab = null;
    [SerializeField] private SpiritAnimationHandler whiteSpirit = null;
    [SerializeField] private SpiritAnimationHandler blackSpirit = null;
    [SerializeField] private SpiritAnimationHandler whiteSpiritSecondPlayer = null;
    [SerializeField] private SpiritAnimationHandler blackSpiritSecondPlayer = null;

    [Header("Scenes")]
    /// <summary>
    ///  Stores a reference to the game scene. Will be opened if the server starts the game
    /// </summary>
    [Scene] [SerializeField] private string gameScene = null;

    [Header("Game End")]
    [SerializeField] private GameEndBehaviour gameEndGameObject;

    /// <summary>
    ///  Will be set to true after the game is over
    /// </summary>
    private bool isGameOver = false;

    /// <summary>
    ///  State of the server
    /// </summary>
    private ServerState state;
    /// <summary>
    ///  Stores additional information about connected clients
    /// </summary>
    private Dictionary<int, ClientInformation> clientInformations = new Dictionary<int, ClientInformation>(); 


    /// <summary>
    ///  Starts the server if the isServer flag is set
    /// </summary>
    public override void Start()
    {
        if (Server.isHeadless)
        {
            this.StartServer();
        }
        else
        {
            this.StartHost();
        }

        this.state = ServerState.InLobby;
    }

    /// <summary>
    ///  Will be called if a player tries to connect to a server
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (this.numPlayers > this.maxConnections)
        {
            conn.Disconnect();
            return;
        }
    }

    /// <summary>
    ///  Will be called if a client disconnects from the server
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var client = conn.identity.GetComponent<LobbyBehaviour>();

            NotifyPlayersOfReadyState();
            
            if (this.state == ServerState.InGame && this.IsGameOver())
            {
                Debug.Log("Game over");
            }
        }

        this.clientInformations.Remove(conn.connectionId);

        base.OnServerDisconnect(conn);
    }

    /// <summary>
    ///  Will be called if the client is ready for the server
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("Client ready");

        switch (this.state)
        {
            case ServerState.InLobby:
                var client = Instantiate(this.lobbyBehaviourPrefab);
                NetworkServer.AddPlayerForConnection(conn, client.gameObject);

                this.clientInformations.Add(conn.connectionId, new ClientInformation());
                break;
            case ServerState.InGame:
                var playerInstance = Instantiate(this.playerBehaviourPrefab);
                NetworkServer.ReplacePlayerForConnection(conn, playerInstance.gameObject, true);

                if (this.clientInformations[conn.connectionId].isControllingTwoPlayers)
                {
                    var secondPlayerInstance = Instantiate(this.playerBehaviourPrefab);
                    NetworkServer.Spawn(secondPlayerInstance.gameObject, conn);
                }

                break;
        }

        base.OnServerReady(conn);
    }

    /// <summary>
    ///  Will be called every time a player dies on the client
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="isSecondPlayer"></param>
    public void HandlePlayerDeath(NetworkConnection conn, bool isSecondPlayer)
    {
        if (this.state != ServerState.InGame)
        {
            return;
        }

        var clientInformation = this.clientInformations[conn.connectionId];

        if (isSecondPlayer)
        {
            clientInformation.isSecondPlayerAlive = false;
        }
        else
        {
            clientInformation.isFirstPlayerAlive = false;
        }

        this.clientInformations[conn.connectionId] = clientInformation;

        if (this.IsGameOver())
        {
            this.HandleGameOver();
        }
    }

    /// <summary>
    ///  Will be called on the server to make a rpc call to all clients
    /// </summary>
    private void HandleGameOver()
    {
        if (this.isGameOver)
        {
            return;
        }

        this.isGameOver = true;
        var endGame = Instantiate(this.gameEndGameObject);
        
        foreach (var id in this.clientInformations.Keys)
        {
            if (this.clientInformations[id].isFirstPlayerAlive)
            {
                endGame.wonPlayerId = id;
                endGame.wonSecondPlayer = false;
            }
            else if (this.clientInformations[id].isSecondPlayerAlive)
            {
                endGame.wonPlayerId = id;
                endGame.wonSecondPlayer = true;
            }
        }

        NetworkServer.Spawn(endGame.gameObject);
    }

    /// <summary>
    ///  Is there more then one player alive
    /// </summary>
    /// <returns></returns>
    private bool IsGameOver()
    {
        int amountOfPlayersAlive = 0;

        foreach (var information in this.clientInformations.Values)
        {
            if (information.isFirstPlayerAlive)
            {
                amountOfPlayersAlive++;
            }
            else if (information.isSecondPlayerAlive)
            {
                amountOfPlayersAlive++;
            }
        }

        return amountOfPlayersAlive <= 1;
    }

    /// <summary>
    ///  Spawns two spirits which are searching for the player object
    /// </summary>
    public void SpawnSpirits(NetworkConnection conn, bool isSecondPlayer = false)
    {
        SpiritAnimationHandler backSpiritInstance = null;
        SpiritAnimationHandler whiteSpiritInstance = null;

        if (isSecondPlayer)
        {
            backSpiritInstance = Instantiate(this.blackSpiritSecondPlayer);
            whiteSpiritInstance = Instantiate(this.whiteSpiritSecondPlayer);
        }
        else
        {
            backSpiritInstance = Instantiate(this.blackSpirit);
            whiteSpiritInstance = Instantiate(this.whiteSpirit);
        }
        
        NetworkServer.Spawn(backSpiritInstance.gameObject, conn);
        NetworkServer.Spawn(whiteSpiritInstance.gameObject, conn);
    }

    /// <summary>
    ///  Will be called at the start of the server and loads all server related resources
    /// </summary>
    public override void OnStartServer()
    {
        this.spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    /// <summary>
    ///  Will be called everytime the server needs to update the ready state of the clients
    /// </summary>
    public void NotifyPlayersOfReadyState()
    {
        if (this.state != ServerState.InLobby)
        {
            return;
        }

        foreach (var connection in NetworkServer.connections.Values)
        {
            var client = connection.identity.GetComponent<LobbyBehaviour>();
            client.RpcHandleReadyToStartGame(this.IsReadyToStart());
        }
    }

    /// <summary>
    ///  Checks if the game can be started by the client
    /// </summary>
    /// <returns></returns>
    private bool IsReadyToStart()
    {
        if (this.numPlayers < this.minPlayers) { return false; }

        foreach (var connection in NetworkServer.connections.Values)
        {
            var client = connection.identity.GetComponent<LobbyBehaviour>();
            if (!client.isReady) { return false; }
        }

        return true;
    }

    /// <summary>
    ///  Will be called to start the game
    /// </summary>
    public void StartGame()
    {
        if (!this.IsReadyToStart() || this.state != ServerState.InLobby)
        {
            return;
        }

        this.state = ServerState.InGame;
        this.isGameOver = false;

        foreach (var connection in NetworkServer.connections.Values)
        {
            var lobbyInstance = connection.identity.gameObject;

            var lobbyBehaviour = lobbyInstance.GetComponent<LobbyBehaviour>();
            var clientInformation = this.clientInformations[connection.connectionId];
            clientInformation.isControllingTwoPlayers = lobbyBehaviour.isRepresentingTwoPlayers;
            clientInformation.isFirstPlayerAlive = true;
            clientInformation.isSecondPlayerAlive = clientInformation.isControllingTwoPlayers;
            this.clientInformations[connection.connectionId] = clientInformation;

            NetworkServer.Destroy(lobbyInstance);
        }

        this.ServerChangeScene(this.gameScene);
    }
}
