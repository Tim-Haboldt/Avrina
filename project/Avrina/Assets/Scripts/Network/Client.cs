using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Client : NetworkManager
{
    /// <summary>
    ///  Those events will be called at the named events only if the current instance is the one of the client
    /// </summary>
    [Header("Events")]
    [SerializeField] private UnityEvent onClientConnected;
    [SerializeField] private UnityEvent onClientDisconnected;

    /// <summary>
    ///  Will be called at the start of the client and loads all client related resources
    /// </summary>
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    /// <summary>
    ///  Will be called if a client connects
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        this.onClientConnected?.Invoke();
    }

    /// <summary>
    ///  Will be called on the client if a client disconnects from the server
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        this.onClientDisconnected?.Invoke();
    }
}
