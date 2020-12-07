using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class Rock : Spell
{
    [Header("Hit Settings")]
    /// <summary>
    ///  The audio will be played on hitting something
    /// </summary>
    [SerializeField] private AudioClip hitSound;
    /// <summary>
    ///  How much damage will each rock deal
    /// </summary>
    [SerializeField] private float damage;
    /// <summary>
    ///  What are the layer masks of the objects the waterball can hit
    /// </summary>
    [SerializeField] private LayerMask collisionMask;
    /// <summary>
    ///  What is the layer mask of the player
    /// </summary>
    [SerializeField] private LayerMask playerMask;
    /// <summary>
    ///  Will store the layer mask for every wall object
    /// </summary>
    [SerializeField] private LayerMask wallMask;

    [Header("Movement")]
    /// <summary>
    ///  How hight is the gravity for the water ball
    /// </summary>
    [SerializeField] private float verticalMovementSpeed = 1f;
    /// <summary>
    ///  How hight is the gravity for the water ball
    /// </summary>
    [SerializeField] private float gravity = 1f;
    /// <summary>
    ///  What is the max vertical speed of the water ball
    /// </summary>
    [SerializeField] private float maxGravity = 1f;

    /// <summary>
    ///  Since when does the water ball exist
    /// </summary>
    private float startTime;
    /// <summary>
    ///  Stores the rigibody instance of the water ball
    /// </summary>
    private Rigidbody2D rb;


    /// <summary>
    ///  The position is already calculated in rock throw thus here the spawn position is given instead of the player position
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 spawnPosition, Vector2 castDirection)
    {
        return spawnPosition;
    }

    /// <summary>
    ///  Will be called after the rock was spawned on the client
    /// </summary>
    protected override void HandleClientStart()
    {
        this.InitRock();
    }

    /// <summary>
    ///  Will be called after the rock was spawned on the server
    /// </summary>
    protected override void HandleServerStart()
    {
        this.InitRock();
    }
    
    /// <summary>
    ///  Will be called to initialize the rock
    /// </summary>
    private void InitRock()
    {
        // Sets the start movement speed
        this.rb = GetComponent<Rigidbody2D>();
        this.rb.velocity = this.verticalMovementSpeed * this.castDirection;
    }

    /// <summary>
    ///  Will be called every time the physics of unity are updated
    /// </summary>
    private void FixedUpdate()
    {
        var velocity = this.rb.velocity;
        velocity.y -= this.gravity;

        if (velocity.y < this.maxGravity * -1)
        {
            velocity.y = this.maxGravity * -1;
        }

        this.rb.velocity = velocity;
    }

    /// <summary>
    ///  Will be called if the trigger of the waterball was triggered.
    /// </summary>
    /// <param name="collision">The object the fireball collided with</param>
    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.collisionMask & (1 << collision.gameObject.layer)) != 0)
        {
            NetworkServer.Destroy(this.gameObject);

            if (!AudioStorage.areSoundEffectsMuted)
            {
                AudioSource.PlayClipAtPoint(this.hitSound, this.transform.position, AudioStorage.soundEffectVolume);
            }

            if ((this.playerMask & (1 << collision.gameObject.layer)) != 0)
            {
                var playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
                playerStatus.CmdHandleHit(this.damage, StatusEffect.NONE);
            }
        }
    }

    /// <summary>
    ///  Will not spawn the rock if its inside the wall
    /// </summary>
    /// <returns></returns>
    public override bool IsSpellInsideWall()
    {
        var rayCastHit = Physics2D.Raycast(this.playerPosition, this.castDirection, 0.2f, this.wallMask);

        return rayCastHit.collider != null;
    }
}
