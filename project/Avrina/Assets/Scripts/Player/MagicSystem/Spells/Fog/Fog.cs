using UnityEngine;
using Mirror;

[RequireComponent(typeof(ParticleSystem))]
public class Fog : Spell
{
    /// <summary>
    ///  How long does the fog exist
    /// </summary>
    [SerializeField] private float lifeTime = 1f;
    /// <summary>
    ///  How long will the particle effect emit new particles
    /// </summary>
    [SerializeField] private float stopEmittingParticles = 1f;

    [Header("Collision")]
    /// <summary>
    ///  What is the layer mask of the player
    /// </summary>
    [SerializeField] private LayerMask playerMask;


    /// <summary>
    ///  Will only be enabled on the client in order to increase performance on the server
    /// </summary>
    private ParticleSystem fogAnimation;
    /// <summary>
    ///  When was the spell spawned on the server
    /// </summary>
    private float startTime;


    /// <summary>
    ///  Start position of the fog is always the player
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        return playerPosition;
    }

    /// <summary>
    ///  Will be called on the client and enables the particle animation
    /// </summary>
    protected override void HandleClientStart()
    {
        this.startTime = Time.time;
        this.fogAnimation = this.GetComponent<ParticleSystem>();
        this.fogAnimation.Play();
    }

    /// <summary>
    ///  Stores when the spell was created in order to destory it later again
    /// </summary>
    protected override void HandleServerStart()
    {
        this.startTime = Time.time;
    }

    /// <summary>
    ///  Will destroy the fog spell after its life time
    /// </summary
    private void Update()
    {
        this.ServerUpdate();
        this.ClientUpdate();
    }

    /// <summary>
    ///  Will only be executed on the server
    /// </summary>
    [Server]
    private void ServerUpdate()
    {
        if (Time.time > this.startTime + this.lifeTime)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

    /// <summary>
    ///  Will only be executed on the clients
    /// </summary>
    [Client]
    private void ClientUpdate()
    {
        if (Time.time > this.startTime + this.stopEmittingParticles)
        {
            this.fogAnimation.Stop();
        }
    }

    /// <summary>
    ///  Will be called if the trigger of the waterball was triggered.
    /// </summary>
    /// <param name="collision">The object the fireball collided with</param>
    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.playerMask & (1 << collision.gameObject.layer)) != 0)
        {
            var playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
            playerStatus.CmdHandleHit(0, StatusEffect.WET);
        }
    }
}
