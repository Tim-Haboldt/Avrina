using UnityEngine;
using System.Collections.Generic;
using Mirror;
using System.Linq;

[RequireComponent(typeof(BoxCollider2D))]
public class FireWall : Spell
{
    /// <summary>
    ///  Sets the spawn distance of the fire wall
    /// </summary>
    [SerializeField] private float spawnDistance = 1f;
    /// <summary>
    ///  Sets the texture offset of the spell
    /// </summary>
    [SerializeField] private Vector2 textureOffset = Vector2.zero;
    /// <summary>
    ///  How long will the fire wall exist
    /// </summary>
    [SerializeField] private float lifeTime = 1f;
    /// <summary>
    ///  How long does it take until the next fire damage tick is applied
    /// </summary>
    [SerializeField] private float timeTillNextFireTick = 0f;
    /// <summary>
    ///  What is the layer of the objects that take damage
    /// </summary>
    [SerializeField] private LayerMask playerLayer;

    /// <summary>
    ///  Since wenn does the fire wall exist
    /// </summary>
    private float startTime;
    /// <summary>
    ///  Stores all collideres that entered the list already
    /// </summary>
    private Dictionary<Collider2D, float> colliders;

    /// <summary>
    ///  Sets the spawn position of the fire wall. (Can only be placed left and right right now)
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        if (this.castDirection.x > 0)
        {
            return this.playerPosition + Vector2.right * this.spawnDistance - this.textureOffset;
        }
        else
        {
            return this.playerPosition + Vector2.left * this.spawnDistance - this.textureOffset;
        }
    }

    /// <summary>
    ///  Deletes the fire wall after the life time is over
    /// </summary>
    private void Update()
    {
        if (Time.time > this.startTime + this.lifeTime)
        {
            Destroy(this.gameObject);
            return;
        }
        foreach (var collider in this.colliders.Keys.ToArray())
        {
            if (Time.time > this.colliders[collider] + this.timeTillNextFireTick)
            {
                var playerStatus = collider.GetComponent<PlayerStatus>();
                playerStatus.CmdHandleHit(0, StatusEffect.ON_FIRE);
                
                if (this.colliders.ContainsKey(collider))
                {
                    this.colliders[collider] += this.timeTillNextFireTick;
                }
            }
        }
    }

    /// <summary>
    ///  Will be called after the client starts
    /// </summary>
    protected override void HandleClientStart()
    {
        this.startTime = Time.time;
    }

    /// <summary>
    ///  Will be called after the server starts
    /// </summary>
    protected override void HandleServerStart()
    {
        this.startTime = Time.time;
        this.colliders = new Dictionary<Collider2D, float>();
    }

    /// <summary>
    ///  Will be called every time a gameobject enteres the object
    /// </summary>
    /// <param name="col"></param>
    [Server]
    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((this.playerLayer & (1 << col.gameObject.layer)) != 0)
        {
            this.colliders.Add(col, Time.time);

            var playerStatus = col.GetComponent<PlayerStatus>();
            playerStatus.CmdHandleHit(0, StatusEffect.ON_FIRE);
        }
    }

    /// <summary>
    ///  Will be called every time a gameobject leaves the object
    /// </summary>
    /// <param name="col"></param>
    [Server]
    private void OnTriggerExit2D(Collider2D col)
    {
        if ((this.playerLayer & (1 << col.gameObject.layer)) != 0)
        {
            this.colliders.Remove(col);
        }
    }
}
