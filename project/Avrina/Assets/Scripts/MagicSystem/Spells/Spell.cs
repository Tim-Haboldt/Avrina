using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    /// <summary>
    ///  Stores the start position of the spell
    /// </summary>
    protected Vector2 startPosition;
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
        this.startPosition = playerPosition;
        this.castDirection = castDirection;

        this.Init();
    }

    /// <summary>
    ///  Will be called at the start of the lifetime of the spell
    /// </summary>
    protected abstract void Init();
}
