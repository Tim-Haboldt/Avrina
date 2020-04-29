using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // TODO implement the player falls completly to the ground.
    // Mesure the dist between the colliders in the player collider and the player.
    // Think about including materials (air, ground materials through tags)
    // Air needs a new collider which checks the players position and what hes inside

    public static float movementInput { get; private set; }
    public static bool jumpInput { get; private set; }
    public static bool onGround { get; private set; }
    public static bool hasWallLeft { get; private set; }
    public static bool hasWallRight { get; private set; }
    public static Collider2D groundCollider { get; private set; }
    public static Collider2D wallColliderLeft { get; private set; }
    public static Collider2D wallColliderRight { get; private set; }
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public PlayerCollider wallSlideColliderLeft;
    [SerializeField] public PlayerCollider wallSlideColliderRight;
    [SerializeField] public PlayerCollider onGroundCollider;


    /**
     * <summary>
     *  Will be called at the start of the game.
     *  Sets the masks of the colliders.
     * </summary>
     */ 
    private void Start()
    {
        this.onGroundCollider.mask = this.groundMask;
        this.wallSlideColliderLeft.mask = this.groundMask;
        this.wallSlideColliderRight.mask = this.groundMask;
    }

    /**
     * <summary>
     *  Will check for all inputs and updates them.
     * </summary>
     */ 
    void Update()
    {
        // Get the player Inputs and write them into the global variables
        PlayerController.movementInput = Input.GetAxisRaw("Horizontal");
        PlayerController.jumpInput = Input.GetKey(KeyCode.Space);

        // Update all colliding states
        PlayerController.onGround = this.onGroundCollider.isTriggered;
        PlayerController.hasWallLeft = this.wallSlideColliderLeft.isTriggered;
        PlayerController.hasWallRight = this.wallSlideColliderRight.isTriggered;

        // Updates all colliders
        PlayerController.groundCollider = this.setCollider(PlayerController.onGround, this.onGroundCollider.colliders);
        PlayerController.wallColliderLeft = this.setCollider(PlayerController.hasWallLeft, this.wallSlideColliderLeft.colliders);
        PlayerController.wallColliderRight = this.setCollider(PlayerController.hasWallRight, this.wallSlideColliderRight.colliders);
    }

    /// <summary>
    ///  Selects the collider with the highest friction.
    /// </summary>
    /// <param name="possibleColliders">List of all possible colliders. Exepcted to contain at least one element.</param>
    /// <returns>Returns the collider with the highest friction</returns>
    private Collider2D setCollider(bool hasCollider, List<Collider2D> possibleColliders)
    {
        if (!hasCollider)
        {
            return null;
        }
    
        lock(this.onGroundCollider.colliders)
        {
            var colliders = this.onGroundCollider.colliders;
            var size = colliders.Count;

            if (size == 0)
            {
                return null;
            }
            
            var possibleCollider = colliders[0];

            if (size > 1)
            {
                for (int i = 1; i < size; i++)
                {
                    Collider2D otherCollider = colliders[i];

                    // Check the type of the ground
                }
            }

            return possibleCollider;
        }
    }
}
