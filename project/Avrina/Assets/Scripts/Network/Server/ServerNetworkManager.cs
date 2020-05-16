using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ServerNetworkManager : NetworkManager
{
    /// <summary>
    ///  Defines the possible states of the server
    /// </summary>
    private enum ServerState
    {
        InLobby,
        InGame
    }

    [Header("Room")]
    /// <summary>
    ///  How many players are needed to start the game
    /// </summary>
    [SerializeField] private int minPlayers = 2;
    /// <summary>
    ///  How many players can play the game
    /// </summary>
    [SerializeField] private int maxPlayers = 8;

    [Header("Prefabs")]
    /// <summary>
    ///  Prefab for all connection players
    /// </summary>
    [SerializeField] private ClientLobbyBehaviour lobbyPlayerPrefab = null;

    /// <summary>
    ///  Stores all players in the server lobby
    /// </summary>
    public List<ClientLobbyBehaviour> players = new List<ClientLobbyBehaviour>();
    /// <summary>
    ///  State of the server
    /// </summary>
    private ServerState state;


    /// <summary>
    ///  Start server at the start of the script
    /// </summary>
    public override void Start()
    {
        Debug.Log("Start Server");
        this.StartServer();

        this.state = ServerState.InLobby;
    }

    /// <summary>
    ///  Adds all spawnable prefabs to the server
    /// </summary>
    public override void OnStartServer() => this.spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    /// <summary>
    ///  Add the player to the game if a client connects
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        switch (this.state)
        {
            case ServerState.InLobby:
                var newLobbyPlayerInstance = Instantiate(this.lobbyPlayerPrefab);
                NetworkServer.AddPlayerForConnection(conn, newLobbyPlayerInstance.gameObject);
                newLobbyPlayerInstance.RpcSetDisplayName();
                this.players.Add(newLobbyPlayerInstance);

                this.NotifyPlayersOfReadyState();

                break;
        }
    }

    /// <summary>
    ///  Notifies all players if a ready state changes
    /// </summary>
    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in this.players)
        {
            player.RpcHandleReadyToStartGame(this.IsReadyToStart());
        }
    }

    /// <summary>
    ///  Checks if the game can be started
    /// </summary>
    /// <returns>Returns if the game can be started</returns>
    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in this.players)
        {
            if (!player.isReady) { return false; }
        }

        return true;
    }

    /// <summary>
    ///  Removes the player from the current player or lobby count and sends a notification to all other players
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<ClientLobbyBehaviour>();
            this.players.Remove(player);
        }

        base.OnServerDisconnect(conn);
    }

    /// <summary>
    ///  Remove all players from the lobby / game if the server is stopped
    /// </summary>
    public override void OnStopServer()
    {
        base.OnStopServer();
        this.players.Clear();
    }
}
