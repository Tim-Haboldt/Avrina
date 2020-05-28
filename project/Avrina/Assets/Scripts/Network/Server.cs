using System.Collections;
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

    [Header("Scenes")]
    /// <summary>
    ///  Stores a reference to the game scene. Will be opened if the server starts the game
    /// </summary>
    [Scene] [SerializeField] private string gameScene = null;

    /// <summary>
    ///  State of the server
    /// </summary>
    private ServerState state;


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
        }

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
                break;
            case ServerState.InGame:
                var playerInstance = Instantiate(this.playerBehaviourPrefab);
                NetworkServer.ReplacePlayerForConnection(conn, playerInstance.gameObject, true);
                break;
        }

        base.OnServerReady(conn);
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

        foreach (var connection in NetworkServer.connections.Values)
        {
            var lobbyInstance = connection.identity.gameObject;
            NetworkServer.Destroy(lobbyInstance);
        }

        this.ServerChangeScene(this.gameScene);
    }
}
