using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteAnimator))]
[RequireComponent(typeof(Rigidbody2D))]
public class FireBall : SpellBase
{
    [HideInInspector] public Vector3 direction;
    [SerializeField] public Vector2 offset;
    [SerializeField] public float velocity;
    [SerializeField] public LayerMask collisionMasks;

    public override void CastSpell(Vector2 playerPosition, Vector2 castDirection)
    {
        // If the given direction was wrong return
        if (castDirection == Vector2.zero)
            return;

        // Create an copy of the current spell
        var fireBallObject = Instantiate(this.gameObject);
        // Find the FireBall script
        var fireBallScript = fireBallObject.GetComponent<FireBall>() as FireBall;
        // Set the cast direction
        fireBallScript.direction = castDirection.normalized;
        // Set the position of the fireball to the of the player
        fireBallObject.transform.position = playerPosition;
        // Add some offset in the direction the player cast the spell at
        var angle = Vector2.SignedAngle(Vector2.right, castDirection);
        var rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        var rotatedOffset = rotation * Vector2.right * offset;
        fireBallObject.transform.position += new Vector3(rotatedOffset.x, rotatedOffset.y);
        // Set the rotation of the gameobject corresponding. (Sprite should look right)
        fireBallObject.transform.rotation = rotation;
        // Start moving the object (It is Kinematic. It only needs to start moving and it won't stop)
        var rigidBody = fireBallObject.GetComponent<Rigidbody2D>();
        rigidBody.velocity = fireBallScript.direction * fireBallScript.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.collisionMasks & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(this.gameObject);
        }       
    }
}
