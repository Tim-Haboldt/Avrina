using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO implement the player falls completly to the ground.
    // Mesure the dist between the colliders in the player collider and the player.
    // Think about including materials (air, ground materials through tags)
    // Air needs a new collider which checks the players position and what hes inside

    public static Vector2 movementInput { get; private set; }
    public static bool jumpInput { get; private set; }
    public static bool onGround { get; private set; }
    public static bool hasWallLeft { get; private set; }
    public static bool hasWallRight { get; private set; }
    public static string groundTag { get; private set; }
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public PlayerCollider wallSlideColliderLeft;
    [SerializeField] public PlayerCollider wallSlideColliderRight;
    [SerializeField] public PlayerCollider onGroundCollider;

    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;


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
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        PlayerController.movementInput = new Vector2(x, y);
        PlayerController.jumpInput = Input.GetKey(KeyCode.Space);

        // Update all colliding states
        PlayerController.onGround = this.onGroundCollider.isTriggered;
        PlayerController.hasWallLeft = this.wallSlideColliderLeft.isTriggered;
        PlayerController.hasWallRight = this.wallSlideColliderRight.isTriggered;
    }
}
