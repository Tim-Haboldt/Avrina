//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(PlayerStatus))]
//[RequireComponent(typeof(PlayerMovement))]
//public class PlayerJump : MonoBehaviour
//{
//    // Defines how long the Jump button can be pressed
//    [SerializeField] public float maxJumpDuration;
//    // How much force will be applied when the jump button is pressed first
//    [SerializeField] public float jumpForce;
//    // How much force will be applied each frame while a wall jump is performed
//    [SerializeField] public float wallJumpForce;
//    // The fall of the player should be a lot faster then the actual jump
//    [SerializeField] public float fallForce;
//    // If the player is falling faster then max fall speed reduce speed
//    [Range(0, 1)]
//    [SerializeField] public float airFriction;
//    [Range(0, 1)]
//    [SerializeField] public float wallFriction;

//    // The player can jump twice. This value defines if the player can jump again
//    private bool hasExtraJump;
//    // Has the player used his first jump
//    private bool hasJump;
//    // When did the player start jumping
//    private float startTime;

//    // The corresponding will be true if the player is jumping
//    private bool isWallJumping;
//    private bool isJumping;

//    // Will be set at the start of a wall jump
//    // If the player hits a wall while jumping the walljump will be canceled
//    private Direction wallJumpDirection;

//    // Used to get the Information which player input is given
//    private PlayerStatus inputs;
//    // Rigidbody of the player used to apply forces
//    private Rigidbody2D rb;
//    // Walljumps need to be able to disable all horizontal movement
//    private PlayerMovement movement;

//    private void Start()
//    {
//        this.inputs = GetComponent<PlayerStatus>();
//        this.rb = GetComponent<Rigidbody2D>();
//        this.movement = GetComponent<PlayerMovement>();
        
//        this.hasExtraJump = true;
//        this.hasJump = false;
//        this.isJumping = false;
//        this.isWallJumping = false;
//    }

//    private void FixedUpdate()
//    {
//        // Apply Jump forces
//        if (this.inputs.onGround)
//        {
//            if (this.inputs.jumpInput)
//            {
//                // Normal jump on the ground
//                if (!this.isJumping && !this.isWallJumping)
//                {
//                    if (this.hasJump)
//                    {
//                        this.isJumping = true;
//                        this.hasJump = false;
//                        this.startTime = Time.time;
//                    }
//                    else if (this.hasExtraJump)
//                    {
//                        this.isJumping = true;
//                        this.hasExtraJump = false;
//                        this.startTime = Time.time;
//                    }
//                }
//            }
//            else
//            {
//                // Reset jumps
//                this.isJumping = false;
//                this.isWallJumping = false;
//                this.hasExtraJump = true;
//                this.hasJump = true;
//            }
//        } else if (this.inputs.isSlidingTheWall)
//        {
//            // Jump from a wall
//            if (this.inputs.jumpInput)
//            {
//                if (!this.isJumping && !this.isWallJumping)
//                {
//                    this.isWallJumping = true;
//                    this.movement.enabled = false;
//                    this.wallJumpDirection = this.inputs.currentSlidingWallDirection;
//                    this.startTime = Time.time;
//                }
//            }
//            else
//            {
//                this.isJumping = false;
//                this.isWallJumping = false;
//            }
//        } else if (this.inputs.jumpInput)
//        {
//            // Jump in the air
//            if (!this.isJumping && !this.isWallJumping)
//            {
//                if (this.hasExtraJump)
//                {
//                    this.hasExtraJump = false;
//                    this.isJumping = true;
//                    this.startTime = Time.time;
//                }
//            }
//        } else
//        {
//            this.isWallJumping = false;
//            this.isJumping = false;
//        }

//        // Apply gravity
//        var passedTime = Time.time - this.startTime;
//        if (this.isJumping && this.inputs.jumpInput && passedTime < this.maxJumpDuration)
//        {
//            this.movement.enabled = true;
//            this.rb.velocity = new Vector2(this.rb.velocity.x, this.jumpForce);
//        }
//        else if (this.isWallJumping && this.inputs.jumpInput && passedTime < this.maxJumpDuration && this.wallJumpDirection == this.inputs.currentSlidingWallDirection)
//        {
//            var horizontalForce = this.wallJumpForce;

//            if (this.wallJumpDirection == Direction.Right)
//            {
//                horizontalForce *= -1;
//            }

//            this.rb.velocity = new Vector2(horizontalForce, this.wallJumpForce*2);
//        }
//        else if (!this.inputs.onGround)
//        {
//            var currentVelocity = this.rb.velocity.y;
//            currentVelocity -= this.fallForce;

//            if (this.inputs.isSlidingTheWall)
//                currentVelocity *= this.wallFriction;
//            else
//                currentVelocity *= this.airFriction;

//            this.rb.velocity = new Vector2(this.rb.velocity.x, currentVelocity);
//            this.movement.enabled = true;
//        } else
//        {
//            this.rb.velocity = new Vector2(this.rb.velocity.x, 0);
//            this.movement.enabled = true;
//        }
//    }
//}
