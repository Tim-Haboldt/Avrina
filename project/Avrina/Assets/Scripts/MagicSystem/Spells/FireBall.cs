using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireBall : Spell
{
    /// <summary>
    ///  How fast will the spell travel throught the world
    /// </summary>
    [SerializeField] private float movementSpeed;
    /// <summary>
    ///  How far will the spell start to exist after the spell was cast
    /// </summary>
    [SerializeField] private float initalCastDistance;
    /// <summary>
    ///  Used to move the spell
    /// </summary>
    private Rigidbody2D rb;


    /// <summary>
    ///  Get all needed components from the gameobejct
    /// </summary>
    private void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    ///  Will be called at the start of the lifetime of the spell
    /// </summary>
    protected override void Init()
    {
        this.transform.position = this.startPosition + this.castDirection * this.initalCastDistance;
        this.rb.velocity = this.castDirection * this.movementSpeed;
    }

    /// <summary>
    ///  Will be called each update tick
    /// </summary>
    private void Update()
    {
    }
}
