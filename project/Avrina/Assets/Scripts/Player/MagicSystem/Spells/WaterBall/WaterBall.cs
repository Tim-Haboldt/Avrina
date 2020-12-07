using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class WaterBall : Spell
{
    /// <summary>
    ///  How far from the player will be water ball spawn
    /// </summary>
    [SerializeField] private float initalCastDistance = 1f;

    [Header("Hit Settings")]
    /// <summary>
    ///  The audio will be played on hitting something
    /// </summary>
    [SerializeField] private AudioClip hitSound;
    /// <summary>
    ///  How hight is the damage on hit
    /// </summary>
    [SerializeField] private float damage = 0.07f;
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

        this.UpdateRotation(velocity);
    }

    /// <summary>
    ///  Updates the rotation of the waterball. (Should always face in the direction moving)
    /// </summary>
    private void UpdateRotation(Vector2 velocity)
    {
        var angle = Vector2.SignedAngle(Vector2.left, velocity);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    /// <summary>
    ///  Where will the water ball be spawned
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        return playerPosition + castDirection * this.initalCastDistance;
    }

    /// <summary>
    ///  Will be called when the water ball is initialized on the client
    /// </summary>
    protected override void HandleClientStart()
    {
        this.InitWaterBall();
    }

    /// <summary>
    ///  Will be called when the water ball is initialized on the server
    /// </summary>
    protected override void HandleServerStart()
    {
        this.InitWaterBall();
    }

    /// <summary>
    ///  Will be called to initialize the water ball
    /// </summary>
    private void InitWaterBall()
    {
        // Sets the start movement speed
        this.rb = GetComponent<Rigidbody2D>();
        this.rb.velocity = this.verticalMovementSpeed * this.castDirection;
        // Sets the start angle
        var angle = Vector2.SignedAngle(Vector2.left, this.rb.velocity);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
                playerStatus.CmdHandleHit(this.damage, StatusEffect.WET);
            }
        }
    }

    /// <summary>
    ///  Can't spawn inside the wall
    /// </summary>
    /// <returns></returns>
    public override bool IsSpellInsideWall()
    {
        var rayCastHit = Physics2D.Raycast(this.playerPosition, this.castDirection, this.initalCastDistance, this.wallMask);

        return rayCastHit.collider != null;
    }
}
