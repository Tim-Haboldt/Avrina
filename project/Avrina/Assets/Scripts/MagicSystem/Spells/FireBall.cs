using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteAnimator))]
[RequireComponent(typeof(Rigidbody2D))]
public class FireBall : SpellBase
{
    [HideInInspector] public Vector3 direction;
    [SerializeField] public Vector2 offset;
    [SerializeField] public float fireBallVelocity;
    [SerializeField] public LayerMask collisionMasks;
    [SerializeField] public string playerTag;
    [SerializeField] public string nameOfExplosionAnimation;
    [SerializeField] public float explosionVelocity;

    private bool isExploding;

    private void Start()
    {
        this.isExploding = false;
    }

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
        var rotatedOffset = rotation * offset;
        fireBallObject.transform.position += new Vector3(rotatedOffset.x, rotatedOffset.y);
        // Set the rotation of the gameobject corresponding. (Sprite should look right)
        fireBallObject.transform.rotation = rotation;
        // Start moving the object (It is Kinematic. It only needs to start moving and it won't stop)
        var rigidBody = fireBallObject.GetComponent<Rigidbody2D>();
        rigidBody.velocity = fireBallScript.direction * fireBallScript.fireBallVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.collisionMasks & (1 << collision.gameObject.layer)) != 0)
        {
            if (!this.isExploding)
            {
                // Play the explosion animation
                var animator = gameObject.GetComponent<SpriteAnimator>();
                animator.Play(this.nameOfExplosionAnimation, false);
                var rigidBody = gameObject.GetComponent<Rigidbody2D>();
                rigidBody.velocity = Vector2.zero;
            }

            // Shot collider away if its a player
            if (collision.tag == this.playerTag)
            {
                var fireBallPos = this.transform.position;
                var playerPos = collision.transform.position;

                var playerDir = playerPos - fireBallPos;
                var playerDirNormalized = playerDir.normalized;

                // Add velocity to the player
                var player = collision.gameObject;
                var rb = player.GetComponent<Rigidbody2D>() as Rigidbody2D;
                rb.AddForce(this.explosionVelocity * playerDirNormalized);

                // Disable all movement of the player ecept special inputs
                var playerStatus = player.GetComponent<PlayerStatus>() as PlayerStatus;
                playerStatus.playerState = PlayerState.Uncontrolled;
            }
        }       
    }

    public void explosionFinsihed()
    {
        Destroy(this.gameObject);
    }
}
