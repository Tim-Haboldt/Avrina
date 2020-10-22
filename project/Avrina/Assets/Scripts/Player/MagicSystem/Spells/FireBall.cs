using UnityEngine;
using Mirror;

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
    ///  Used to define which elements trigger the explosion of the fireball
    /// </summary>
    [SerializeField] private LayerMask collisionMasks;
    /// <summary>
    ///  Used to move the spell
    /// </summary>
    [SerializeField] private Rigidbody2D rb;


    /// <summary>
    ///  Will set the start rotation and the start position of the spell
    /// </summary>
    protected override void InitSpell()
    {
        // Set position
        this.transform.position = this.playerPosition + this.castDirection * this.initalCastDistance;
        // Set rotation
        var angle = Vector2.SignedAngle(Vector2.right, this.castDirection);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // Set velocity of the object
        this.rb.velocity = this.castDirection * this.movementSpeed;
    }

    /// <summary>
    ///  Will be called if the trigger of the fireball was triggered.
    /// </summary>
    /// <param name="collision">The object the fireball collided with</param>
    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.collisionMasks & (1 << collision.gameObject.layer)) != 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
