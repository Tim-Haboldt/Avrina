using UnityEngine;
using Mirror;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Animator))]
public class IceExplosion : Spell
{
    [Header("Explosion Settings")]
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
    [SerializeField] private float damage = 0f;

    [Header("General Settings")]
    /// <summary>
    ///  Length of the animation. The Explosion object will be destroyed afterwards
    /// </summary>
    [SerializeField] private float animationLength = 0f;
    /// <summary>
    ///  Bounds of the sprites
    /// </summary>
    [SerializeField] private Vector2 spriteSize = Vector2.zero;
    /// <summary>
    ///  How far strong is the knock back for the player
    /// </summary>
    [SerializeField] private float knockBack = 1f;

    [Header("Light Settings")]
    /// <summary>
    ///  Used to make the lights brighter and less bright
    /// </summary>
    [SerializeField] private Light2D explosionLight = null;
    /// <summary>
    ///  What is the maximal intensity of the light
    /// </summary>
    [SerializeField] private float maxIntesity = 0f;
    /// <summary>
    ///  What is the maximal outer line
    /// </summary>
    [SerializeField] private float maxOuterLine = 0f;
    /// <summary>
    ///  How long does it take until the explosion light is the strongest
    /// </summary>
    [SerializeField] private float timeTillExplosionStrongest = 0f;
    /// <summary>
    ///  How long does it take until the explosion light falls off again
    /// </summary>
    [SerializeField] private float timeTillLightFallsOff = 0f;
    /// <summary>
    ///  When is the explosion finished
    /// </summary>
    [SerializeField] private float timeTillExplosionFinished = 0f;

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
    ///  Will be called when the object is created on the client
    /// </summary>
    protected override void HandleClientStart()
    {
        this.animator = GetComponent<Animator>();
        this.animator.enabled = true;

        this.Explode();
    }

    /// <summary>
    ///  Will be called when the object is created on the server
    /// </summary>
    protected override void HandleServerStart()
    {
        this.Explode();

        var spriteOffset = new Vector3(this.spriteSize.x * 0.5f, -this.spriteSize.y * 0.5f);
        var spellPos = this.transform.position + spriteOffset;
        var colliders = Physics2D.OverlapCircleAll(spellPos, this.explosionRadius);
        foreach (var collider in colliders)
        {
            var playerStatus = collider.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                if (this.caster != playerStatus.GetComponent<NetworkIdentity>().netId)
                {
                    playerStatus.CmdHandleHit(this.damage, StatusEffect.FROZEN);

                    /*var playerRb = playerStatus.GetComponent<Rigidbody2D>();
                    var playerPos = playerStatus.transform.position;
                    var distance = Vector2.Distance(playerPos, spellPos);
                    var knockBackDirection = (playerPos - spellPos).normalized;

                    playerRb.AddForce(knockBackDirection * (this.explosionRadius - distance) * this.knockBack);*/
                }
                else
                {
                    playerStatus.CmdHandleHit(0, StatusEffect.FROZEN);
                }
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

        if (Time.time < this.startTime + this.timeTillExplosionStrongest)
        {
            this.explosionLight.intensity = (Time.time - this.startTime) / this.timeTillExplosionStrongest * this.maxIntesity;
            this.explosionLight.pointLightOuterRadius = (Time.time - this.startTime) / this.timeTillExplosionStrongest * this.maxOuterLine;
        }
        else if (Time.time < this.startTime + this.timeTillLightFallsOff)
        {
            this.explosionLight.intensity = this.maxIntesity;
            this.explosionLight.pointLightOuterRadius = this.maxOuterLine;
        }
        else if (Time.time < this.startTime + this.timeTillExplosionFinished)
        {
            this.explosionLight.intensity = (1 - ((Time.time - this.startTime - this.timeTillLightFallsOff) / this.timeTillExplosionFinished)) * this.maxIntesity;
            this.explosionLight.pointLightOuterRadius = (1 - ((Time.time - this.startTime - this.timeTillLightFallsOff) / this.timeTillExplosionFinished)) * this.maxOuterLine;
        }
        else
        {
            this.explosionLight.intensity = 0;
        }
    }

    /// <summary>
    ///  Returns the current player position
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        var halfBounds = new Vector2(this.spriteSize.x * 0.5f, -this.spriteSize.y * 0.5f);

        return playerPosition - halfBounds;
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
