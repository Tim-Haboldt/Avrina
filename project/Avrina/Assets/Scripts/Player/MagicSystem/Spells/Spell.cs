using UnityEngine;
using Mirror;

[RequireComponent(typeof(PositionSynchronizer))]
public abstract class Spell : NetworkBehaviour
{
    /// <summary>
    ///  Will be played on the client when the spell is spawned
    /// </summary>
    [SerializeField] private AudioClip castSound;
    /// <summary>
    ///  How load is the spell
    /// </summary>
    [SerializeField] private float castSoundVolume;

    /// <summary>
    ///  Stores the inital cast direction of the spell
    /// </summary>
    [SyncVar]
    [HideInInspector] public Vector2 castDirection;
    /// <summary>
    ///  Stores the player position at the creation time of the spell
    /// </summary>
    [SyncVar]
    [HideInInspector] public Vector2 playerPosition;
    /// <summary>
    ///  Stores the network id of the caster (player object)
    /// </summary>
    [SyncVar]
    [HideInInspector] public uint caster;


    /// <summary>
    ///  Will be called when the object is created on the client
    /// </summary>
    public override void OnStartClient()
    {
        this.transform.position = this.CalculateStartPosition(this.playerPosition, this.castDirection);

        if (!AudioStorage.areSoundEffectsMuted)
        {
            AudioSource.PlayClipAtPoint(this.castSound, this.transform.position, AudioStorage.soundEffectVolume * this.castSoundVolume);
        }

        this.HandleClientStart();
    }

    /// <summary>
    ///  Will be called when the object is created on the server
    /// </summary>
    public override void OnStartServer()
    {
        this.transform.position = this.CalculateStartPosition(this.playerPosition, this.castDirection);

        this.HandleServerStart();
    }

    /// <summary>
    ///  Will be called to set the start position of the spell
    /// </summary>
    /// <returns></returns>
    protected abstract Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection);

    /// <summary>
    ///  Will be called after the spell is created on the server
    /// </summary>
    protected abstract void HandleServerStart();

    /// <summary>
    ///  Will be called after the spell is created on the client
    /// </summary>
    protected abstract void HandleClientStart();
}
