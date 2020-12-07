using UnityEngine;
using Mirror;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(BoxCollider2D))]
public class AirPush : Spell
{
    /// <summary>
    ///  Will define how the collision box moves to the target
    /// </summary>
    [SerializeField] private float collisionBoxMovementSpeed = 1f;
    /// <summary>
    ///  How far from the player will the spell be spawned
    /// </summary>
    [SerializeField] private float castDistance = 1f;
    /// <summary>
    ///  How long will the spell life
    /// </summary>
    [SerializeField] private float spellDuration = 1f;
    /// <summary>
    ///  Used to move the light during the spell
    /// </summary>
    [SerializeField] private Light2D lights = null;

    /// <summary>
    ///  When was the spell created
    /// </summary>
    private float startTime = 0f;
    /// <summary>
    ///  Gets the box collider at the creation of the spell
    /// </summary>
    private BoxCollider2D boxCollider = null;


    /// <summary>
    ///  Sets the start position of the spell
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        return this.playerPosition + this.castDistance * castDirection;
    }

    /// <summary>
    ///  Will be called after the spell was initialized on the client
    /// </summary>
    protected override void HandleClientStart()
    {
        this.InitSpell();
    }

    /// <summary>
    ///  Will be called after the spell was initialized on the server
    /// </summary>
    protected override void HandleServerStart()
    {
        this.startTime = Time.time;

        this.InitSpell();
    }

    /// <summary>
    ///  Will be called on both the client and the server
    /// </summary>
    private void InitSpell()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, this.castDirection));
        this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    /// <summary>
    ///  Will be used to destory the spell
    /// </summary>
    [Server]
    private void Update()
    {
        if (Time.time > this.startTime + this.spellDuration)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

    /// <summary>
    ///  Will move the collision box
    /// </summary>
    private void FixedUpdate()
    {
        var additionalPos = this.castDirection * this.collisionBoxMovementSpeed;

        var oldLightPosition = this.lights.transform.position;
        oldLightPosition += new Vector3(additionalPos.x, additionalPos.y);
        this.lights.transform.position = oldLightPosition;

        var oldColliderPos = this.boxCollider.transform.position;
        oldColliderPos += new Vector3(additionalPos.x, additionalPos.y);
        this.boxCollider.transform.position = oldColliderPos;
    }

    /// <summary>
    ///  Will always be true. Because if theres a wall the player is pushed instead
    /// </summary>
    /// <returns></returns>
    public override bool IsSpellInsideWall()
    {
        return false;
    }
}
