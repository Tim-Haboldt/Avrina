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
    ///  Will be created at the point where the fireball hit something
    /// </summary>
    [SerializeField] private Explosion explosionPrefab;


    /// <summary>
    ///  Starts the movement of the fireball and sets ht
    /// </summary>
    protected override void HandleClientStart()
    {
        // Sets the rotation of the fireball
        var angle = Vector2.SignedAngle(Vector2.right, this.castDirection);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // SEts the movement direction of the fireball
        this.rb.velocity = this.castDirection * this.movementSpeed;
    }

    /// <summary>
    ///  Starts the movement of the fireball
    /// </summary>
    protected override void HandleServerStart()
    {
        // Sets the movement direction of the fireball
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
            var explosion = Instantiate(this.explosionPrefab);
            explosion.castDirection = Vector2.zero;
            explosion.playerPosition = this.transform.position;
            explosion.caster = 9999999;
            NetworkServer.Spawn(explosion.gameObject);

            NetworkServer.Destroy(this.gameObject);
        }
    }

    /// <summary>
    ///  Calculates the start position of the spell
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        return playerPosition + castDirection * this.initalCastDistance;
    }
}
