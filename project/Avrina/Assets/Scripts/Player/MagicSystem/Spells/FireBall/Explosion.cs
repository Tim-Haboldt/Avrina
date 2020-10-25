using UnityEngine;
using Mirror;

[RequireComponent(typeof(Animator))]
public class Explosion : NetworkBehaviour
{
    /// <summary>
    ///  How big is the explosion
    /// </summary>
    [SerializeField] private float explosionRadius = 1f;
    /// <summary>
    ///  What layers will be effected by the explosion
    /// </summary>
    [SerializeField] private LayerMask collisionMask;
    /// <summary>
    ///  How much damage does the explosion deal to the player
    /// </summary>
    [SerializeField] private float damage;
    /// <summary>
    ///  Length of the animation. The Explosion object will be destroyed afterwards
    /// </summary>
    [SerializeField] private float animationLength;

    /// <summary>
    ///  Will be used to start the animation
    /// </summary>
    private Animator animator;
    /// <summary>
    ///  Will define when the explosion started
    /// </summary>
    private float startTime;
    /// <summary>
    ///  Will be set to true after the explosion startet
    /// </summary>
    private bool hasExplosionStarted = false;
    /// <summary>
    ///  Sets the position of the explosion after beeing spawned
    /// </summary>
    [SyncVar]
    [HideInInspector] public Vector2 startPosition;


    /// <summary>
    ///  Will get the animation element
    /// </summary>
    private void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    /// <summary>
    ///  Will be called when the object is created on the client
    /// </summary>
    public override void OnStartClient()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var bounds = spriteRenderer.sprite.bounds;
        Vector2 halfBounds = new Vector2(bounds.size.x * 0.5f, -bounds.size.y * 0.5f);
        this.transform.position = this.startPosition - halfBounds;
        this.animator.enabled = true;

        this.Explode();
    }

    /// <summary>
    ///  Will be called when the object is created on the server
    /// </summary>
    public override void OnStartServer()
    {
        this.Explode();

        var colliders = Physics2D.OverlapCircleAll(this.startPosition, this.explosionRadius);
        foreach (var collider in colliders)
        {
            var playerStatus = collider.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.CmdHandleHit(this.damage, StatusEffect.ON_FIRE);
            }
        }
    }

    /// <summary>
    ///  Will start the explosion
    /// </summary>
    private void Explode()
    {
        if (this.hasExplosionStarted)
        {
            return;
        }

        this.hasExplosionStarted = true;
        this.startTime = Time.time;
    }

    /// <summary>
    ///  Will destroy the element after the explosion finished
    /// </summary>
    private void Update()
    {
        if (this.hasExplosionStarted && Time.time > this.startTime + this.animationLength)
        {
            Destroy(this.gameObject);
        }
    }
}
