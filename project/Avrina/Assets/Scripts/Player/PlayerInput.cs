using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float movementInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool onGround { get; private set; }
    public bool isSlidingTheWall { get; private set; }
    public Direction currentSlidingWallDirection { get; private set; }
    public string groundTag { get; private set; }
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
    }

    void Update()
    {
        // Get the player Inputs and write them into the global variables
        this.movementInput = Input.GetAxisRaw("Horizontal");
        this.jumpInput = Input.GetKey(KeyCode.Space);
    }

    private void LateUpdate()
    {
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
    }
}
