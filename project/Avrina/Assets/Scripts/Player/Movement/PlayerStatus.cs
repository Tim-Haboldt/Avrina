using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float movementInputHorizontal { get; private set; }
    public float movementInputVertical { get; private set; }
    public bool jumpInput { get; private set; }
    public bool onGround { get; private set; }
    public bool isSlidingTheWall { get; private set; }
    public Direction currentSlidingWallDirection { get; private set; }
    public string groundTag { get; private set; }
    public PlayerState playerState;
    public JumpState jumpState;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public PlayerCollider wallSlideColliderLeft;
    [SerializeField] public PlayerCollider wallSlideColliderRight;
    [SerializeField] public PlayerCollider onGroundCollider;

    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;

    private void Start()
    {
        this.onGroundCollider.mask = this.groundMask;
        this.wallSlideColliderLeft.mask = this.groundMask;
        this.wallSlideColliderRight.mask = this.groundMask;
        this.playerState = PlayerState.Mobile;
        this.jumpState = JumpState.InAir;
    }

    void Update()
    {
        // Get the player Inputs and write them into the global variables
        this.movementInputHorizontal = Input.GetAxisRaw("Horizontal");
        this.movementInputVertical = Input.GetAxisRaw("Vertical");
        this.jumpInput = Input.GetKey(KeyCode.Space);

        // Update all colliding states
        this.onGround = this.onGroundCollider.isTriggered;

        if (this.wallSlideColliderLeft.isTriggered || this.wallSlideColliderRight.isTriggered)
            this.isSlidingTheWall = true;
        else
        {
            this.isSlidingTheWall = false;
            this.currentSlidingWallDirection = Direction.None;
        }

        if (this.currentSlidingWallDirection == Direction.None)
        {
            if (this.wallSlideColliderLeft.isTriggered)
                this.currentSlidingWallDirection = Direction.Left;
            if (this.wallSlideColliderRight.isTriggered)
                this.currentSlidingWallDirection = Direction.Right;
        }

        if (this.currentSlidingWallDirection == Direction.Left && !this.wallSlideColliderLeft.isTriggered && this.wallSlideColliderRight.isTriggered)
            this.currentSlidingWallDirection = Direction.Right;
        if (this.currentSlidingWallDirection == Direction.Right && !this.wallSlideColliderRight.isTriggered && this.wallSlideColliderLeft.isTriggered)
            this.currentSlidingWallDirection = Direction.Left;

        // Remove uncontrolled phase if the player is touching the wall or the ground
        if (this.playerState == PlayerState.Uncontrolled && (this.onGround || this.isSlidingTheWall))
            this.playerState = PlayerState.Mobile;
    }
}
