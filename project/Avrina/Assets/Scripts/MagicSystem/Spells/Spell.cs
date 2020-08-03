using UnityEngine;
using Mirror;

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
    ///  Will be called when the spell is cast
    /// </summary>
    /// <param name="playerPosition">Position of the player at the time of the spell cast</param>
    /// <param name="castDirection">Cast direction</param>
    public void CastSpell(Vector2 playerPosition, Vector2 castDirection)
    {
        this.playerPosition = playerPosition;
        this.castDirection = castDirection;

        this.ServerInit();
        this.RpcInit(playerPosition, castDirection);
    }

    /// <summary>
    ///  Will be called at the start of the lifetime of the spell on the client
    /// <param name="playerPosition">Position of the player at the time of the spell cast</param>
    /// <param name="castDirection">Cast direction</param>
    /// </summary>
    protected abstract void RpcInit(Vector2 playerPosition, Vector2 castDirection);

    /// <summary>
    ///  Will be called at the start of the lifetime of the spell on the server.
    /// </summary>
    protected abstract void ServerInit();
}
