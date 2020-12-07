using UnityEngine;
using Mirror;

[RequireComponent(typeof(Animator))]
public class FireMine : Spell
{
    private enum FireMineState
    {
        EXPLODING,
        IDLE
    }

    [Header("Visuals")]
    /// <summary>
    ///  Idle Animation
    /// </summary>
    [SerializeField] private RuntimeAnimatorController idleAnimatior = null;
    /// <summary>
    ///  How long will the i
    /// </summary>
    [SerializeField] private float idleAnimationLength = 0f;
    /// <summary>
    ///  Explosion Animation
    /// </summary>
    [SerializeField] private RuntimeAnimatorController explosionAnimatior = null;
    /// <summary>
    ///  How long is the explosion animation
    /// </summary>
    [SerializeField] private float explosionAnimationLength = 0f;
    /// <summary>
    ///  How much smaller of bigger are the sprites of the explosion animation
    /// </summary>
    [SerializeField] private float explosionAnimationScaling = 1f;
    /// <summary>
    ///  Sets the offset of the explosion animation
    /// </summary>
    [SerializeField] private Vector2 explosionAnimationOffset = Vector2.zero;

    [Header("Damage")]
    /// <summary>
    ///  Sets the center of the explosion corresponding to the sprite animation
    /// </summary>
    [SerializeField] private Vector2 damageOffset = Vector2.zero;
    /// <summary>
    ///  Sets the damage radius
    /// </summary>
    [SerializeField] private float explosionRadius = 1f;
    /// <summary>
    ///  Will be used to find the player objects and applies the damage to them
    /// </summary>
    [SerializeField] private LayerMask playerLayer;
    /// <summary>
    ///  How much damage will be dealt by the explosion
    /// </summary>
    [SerializeField] private float damage = 0.3f;

    [Header("Audio")]
    /// <summary>
    ///  Stores the audio clip of the explosion sound
    /// </summary>
    [SerializeField] private AudioClip explosionSound;
    /// <summary>
    ///  How load is the default explosion volume
    /// </summary>
    [SerializeField] private float explosionVolume = 1f;
    /// <summary>
    ///  Stores the audio clip of the hit sound
    /// </summary>
    [SerializeField] private AudioClip hitSound;
    /// <summary>
    ///  How load is the default hit volume
    /// </summary>
    [SerializeField] private float hitVolume = 1f;

    /// <summary>
    ///  Stores the animator instance
    /// </summary>
    private Animator animator = null;
    /// <summary>
    ///  Start time of the animation
    /// </summary>
    private float startTime;
    /// <summary>
    ///  Stores the current state of the fire mine
    /// </summary>
    private FireMineState currentState = FireMineState.IDLE;

    
    /// <summary>
    ///  Handles the animations
    /// </summary>
    private void Update()
    {
        switch (this.currentState) {
            case FireMineState.IDLE:
                if (this.startTime + this.idleAnimationLength < Time.time)
                {
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y) + this.explosionAnimationOffset;
                    this.ChangeAnimationOnClient();
                    this.startTime += this.idleAnimationLength;
                    this.currentState = FireMineState.EXPLODING;

                    this.ServerExplode();
                    this.ClientExplode();
                }
                break;
            case FireMineState.EXPLODING:
                if (this.startTime + this.explosionAnimationLength < Time.time)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    /// <summary>
    ///  Will only be called on the server and handels the explosion
    /// </summary>
    [Server]
    private void ServerExplode()
    {
        var explosionCenter = new Vector2(this.transform.position.x, this.transform.position.y) + this.damageOffset - this.explosionAnimationOffset;
        var colliders = Physics2D.OverlapCircleAll(explosionCenter, this.explosionRadius, this.playerLayer);
        foreach (var collider in colliders)
        {
            var playerStatus = collider.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                // TODO Play sound on hit (Client)
                playerStatus.CmdHandleHit(this.damage, StatusEffect.ON_FIRE);
            }
        }
    }

    /// <summary>
    ///  Will be called on the client to handle an explosion
    /// </summary>
    [Client]
    private void ClientExplode()
    {
        if (!AudioStorage.areSoundEffectsMuted)
        {
            var explosionCenter = new Vector2(this.transform.position.x, this.transform.position.y) + this.damageOffset - this.explosionAnimationOffset;
            AudioSource.PlayClipAtPoint(this.explosionSound, explosionCenter, this.explosionVolume * AudioStorage.soundEffectVolume);
        }
    }

    /// <summary>
    ///  Changes the animation but only on the client
    /// </summary>
    [Client]
    private void ChangeAnimationOnClient()
    {
        var oldLocalScale = this.transform.localScale;
        oldLocalScale.x *= this.explosionAnimationScaling;
        oldLocalScale.y *= this.explosionAnimationScaling;
        this.transform.localScale = oldLocalScale;

        this.animator.runtimeAnimatorController = this.explosionAnimatior;
    }

    /// <summary>
    ///  Returns the player position 
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        return playerPosition;
    }

    /// <summary>
    ///  Will be called when the server starts
    /// </summary>
    protected override void HandleServerStart()
    {
        this.startTime = Time.time;
    }

    /// <summary>
    ///  Will be called on the client start
    /// </summary>
    protected override void HandleClientStart()
    {
        this.animator = GetComponent<Animator>();
        this.animator.runtimeAnimatorController = this.idleAnimatior;
        this.startTime = Time.time;
    }

    /// <summary>
    ///  Will always be true
    /// </summary>
    /// <returns></returns>
    public override bool IsSpellInsideWall()
    {
        return false;
    }
}
