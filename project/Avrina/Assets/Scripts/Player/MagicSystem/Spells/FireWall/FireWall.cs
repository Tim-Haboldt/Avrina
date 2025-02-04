﻿using UnityEngine;
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
    ///  How much damage will the firewall deal per damage tick
    /// </summary>
    [SerializeField] private float damage = 0.01f;
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
    ///  Will store the layer mask for every wall object
    /// </summary>
    [SerializeField] private LayerMask wallMask;
    /// <summary>
    ///  How far from the wall will the wall spawn
    /// </summary>
    [SerializeField] private float wallDistanceOffsetX;
    /// <summary>
    ///  Will define how far the firewall can be spawned from the current position
    /// </summary>
    [SerializeField] private float minSpawnOffset;

    /// <summary>
    ///  Used to store the start position after the is in wall check
    /// </summary>
    [SyncVar]
    private Vector2 spawnPosition;
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
        return this.spawnPosition;
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
                playerStatus.CmdHandleHit(this.damage, StatusEffect.ON_FIRE);
                
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

            if (!AudioStorage.areSoundEffectsMuted)
            {
                AudioSource.PlayClipAtPoint(this.castSound, this.transform.position, AudioStorage.soundEffectVolume);
            }
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

    /// <summary>
    ///  Will check the environment if there is space for the fire wall and spawn it. If not returns true
    /// </summary>
    /// <returns></returns>
    public override bool IsSpellInsideWall()
    {
        var localCastDirection = this.castDirection.x > 0 ? Vector2.right : Vector2.left;

        // Do a ray cast. If theres no object return position
        var hit = Physics2D.Raycast(this.playerPosition, localCastDirection, this.spawnDistance + this.wallDistanceOffsetX, this.wallMask);
        if (hit.collider == null)
        {
            this.spawnPosition = this.playerPosition + localCastDirection * this.spawnDistance - textureOffset;

            return false;
        }

        var hitPointDistance = Vector2.Distance(this.playerPosition, hit.point);
        var distanceWithWallOffset = hitPointDistance - this.wallDistanceOffsetX;
        if (distanceWithWallOffset >= this.minSpawnOffset)
        {
            this.spawnPosition = this.playerPosition + localCastDirection * distanceWithWallOffset - textureOffset;

            return false;
        }

        return true;
    }
}
