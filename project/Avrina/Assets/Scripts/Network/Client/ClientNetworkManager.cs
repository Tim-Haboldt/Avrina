using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class ClientNetworkManager : NetworkManager
{
    [Header("Events")]
    /// <summary>
    ///  Will be called if the connection to the server was successful
    /// </summary>
    [SerializeField] private UnityEvent clientConnected;
    /// <summary>
    ///  Will be called if the connection to the server was not successful
    /// </summary>
    [SerializeField] private UnityEvent clientConnectionFailed;
    /// <summary>
    ///  Name of the client
    /// </summary>
    public string ingameName;


    /// <summary>
    ///  Registers all client side prefabs
    /// </summary>
    public override void OnStartClient()
    {
        Debug.Log("Connected");
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    /// <summary>
    ///  Will be triggered if a client connects to a server
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        this.clientConnected.Invoke();
    }

    /// <summary>
    ///  Will be called if the client failed to connect
    /// </summary>
    /// <param name="error"></param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        this.clientConnectionFailed.Invoke();
    }
}
