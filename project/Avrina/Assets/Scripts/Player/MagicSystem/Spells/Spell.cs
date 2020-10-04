using UnityEngine;
using Mirror;

[RequireComponent(typeof(PositionSynchronizer))]
public abstract class Spell : NetworkBehaviour
{
    /// <summary>
    ///  Stores the player position at the creation time of the spell
    /// </summary>
    protected Vector2 playerPosition;
    /// <summary>
    ///  Stores the inital cast direction of the spell
    /// </summary>
    protected Vector2 castDirection;
    /// <summary>
    ///  Synchronizes the position of the spell to the clients
    /// </summary>
    private PositionSynchronizer posSynchronizer;


    /// <summary>
    ///  Will be called once the spell script is first initialized
    /// </summary>
    public void Awake()
    {
        this.posSynchronizer = GetComponent<PositionSynchronizer>();
    }

    /// <summary>
    ///  Will be called when the spell is cast
    /// </summary>
    /// <param name="playerPosition">Position of the player at the time of the spell cast</param>
    /// <param name="castDirection">Cast direction</param>
    public void CastSpell(Vector2 playerPosition, Vector2 castDirection)
    {
        // Server initialization
        this.playerPosition = playerPosition;
        this.castDirection = castDirection;
        this.InitSpell();
        // Activate Synchronization
        this.posSynchronizer.enabled = true;
        // Client initialization
        this.RpcInitializeVariables(playerPosition, castDirection);
        this.RpcInitSpell();
    }

    /// <summary>
    ///  Used to initialized all variables on all the clients too.
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    [ClientRpc]
    private void RpcInitializeVariables(Vector2 playerPosition, Vector2 castDirection)
    {
        this.playerPosition = playerPosition;
        this.castDirection = castDirection;
    }

    /// <summary>
    ///  Will be used to initialize the spell on all the clients.
    /// </summary>
    [ClientRpc]
    private void RpcInitSpell()
    {
        if (!this.isClientOnly)
        {
            return;
        }

        this.InitSpell();
        this.posSynchronizer.enabled = true;
    }

    /// <summary>
    ///  Will be called after the spell was cast
    /// </summary>
    protected abstract void InitSpell();
}
