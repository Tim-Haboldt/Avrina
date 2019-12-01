//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(PlayerInputController))]
//[RequireComponent(typeof(PlayerHorizontalMovement))]
//public class PlayerVerticalMovement : MonoBehaviour
//{
//    // Defines how long the Jump button can be pressed
//    [SerializeField] public float maxJumpDuration;
//    // Defines how long you can wall jump
//    [SerializeField] public float maxWallJumpDuration;
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
    
//    /*// When did the player start jumping
//    private float startTime;
//    // Does the player let go of the jump key
//    private bool letGoOfJumpInput;
//    // Will be set at the start of a wall jump
//    // If the player hits a wall while jumping the walljump will be canceled
//    private Direction wallJumpDirection;

//    // Used to get the Information which player input is given
//    private PlayerInputController inputs;
//    // Rigidbody of the player used to apply forces
//    private Rigidbody2D rb;
//    // Walljumps need to be able to disable all horizontal movement
//    private PlayerHorizontalMovement movement;

//    private void Start()
//    {
//        this.inputs = GetComponent<PlayerInputController>();
//        this.rb = GetComponent<Rigidbody2D>();
//        this.movement = GetComponent<PlayerHorizontalMovement>();

//        this.letGoOfJumpInput = true;
//    }

//    private void FixedUpdate()
//    {
//        // Important State changes which will be performed from each player state
//        this.checkForStateOnGround();
//        this.checkForStateSlidingWall();
//        // Does the player still hold the jump button
//        if (!this.inputs.jumpInput)
//            this.letGoOfJumpInput = true;
//        // Is the player still on ground or on the wall
//        if (!this.inputs.onGround)
//            this.inputs.jumpState = JumpState.InAir;
//        else if (!this.inputs.isSlidingTheWall)
//            this.inputs.jumpState = JumpState.InAir;

//        switch (this.inputs.playerState)
//        {
//            case PlayerState.Disabled:
//                break;
//            case PlayerState.Uncontrolled:
//                if (this.letGoOfJumpInput && this.inputs.jumpInput)
//                {
//                    this.inputs.playerState = PlayerState.Mobile;
//                    this.letGoOfJumpInput = false;
//                    this.updateCurrentJumpState();
//                }
//                break;
//            case PlayerState.Mobile:
//                this.updateCurrentJumpState();
//                break;
//        }

//        this.applyForces();

//        this.applyGravityAndFriction();
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
//        }
//        else if (this.inputs.isSlidingTheWall)
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
//        }
//        else if (this.inputs.jumpInput)
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
//        }
//        else
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

//            this.rb.velocity = new Vector2(horizontalForce, this.wallJumpForce * 2);
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
//        }
//        else
//        {
//            this.movement.enabled = true;
//        }
//    }

//    private void updateCurrentJumpState()
//    {
//        var passedTime = Time.time - this.startTime;

//        switch (this.inputs.jumpState)
//        {
//            case JumpState.Immobile:
//                // Player cannot do anything
//                break;
//            case JumpState.InAir:
//                if (this.inputs.jumpInput && this.letGoOfJumpInput)
//                {
//                    this.inputs.jumpState = JumpState.AirJumping;
//                    this.startTime = Time.time;
//                    this.letGoOfJumpInput = false;
//                }
//                break;
//            case JumpState.Jumping:
//                if (this.inputs.jumpInput)
//                {
//                    if (passedTime >= this.maxJumpDuration)
//                        this.inputs.jumpState = JumpState.InAir;
//                } else
//                    this.inputs.jumpState = JumpState.InAir;
//                break;
//            case JumpState.OnGround:
//                if (this.inputs.jumpInput && this.letGoOfJumpInput)
//                {
//                    this.inputs.jumpState = JumpState.Jumping;
//                    this.startTime = Time.time;
//                    this.letGoOfJumpInput = false;
//                }
//                break;
//            case JumpState.WallJumping:
//                if (this.inputs.jumpInput)
//                {
//                    if (passedTime >= this.maxWallJumpDuration)
//                        this.inputs.jumpState = JumpState.InAir;
//                }
//                else
//                    this.inputs.jumpState = JumpState.InAir;
//                break;
//            case JumpState.AirJumping:
//                if (this.inputs.jumpInput)
//                {
//                    if (passedTime >= this.maxJumpDuration)
//                        this.inputs.jumpState = JumpState.Immobile;
//                }
//                else
//                    this.inputs.jumpState = JumpState.Immobile;
//                break;
//            case JumpState.WallSliding:
//                if (this.inputs.jumpInput && this.letGoOfJumpInput)
//                {
//                    this.inputs.jumpState = JumpState.WallJumping;
//                    this.wallJumpDirection = this.inputs.currentSlidingWallDirection;
//                    this.startTime = Time.time;
//                    this.letGoOfJumpInput = false;
//                }
//                break;
//        }
//    }

//    private void checkForStateSlidingWall()
//    {
//        if (this.inputs.isSlidingTheWall && this.inputs.jumpState != JumpState.OnGround)
//        {
//            this.inputs.jumpState = JumpState.WallSliding;
//            this.inputs.playerState = PlayerState.Mobile;
//        }
//    }

//    private void checkForStateOnGround()
//    {
//        if (this.inputs.onGround)
//        {
//            this.inputs.playerState = PlayerState.Mobile;

//            if (this.inputs.jumpState != JumpState.OnGround)
//            {
//                this.rb.velocity = new Vector2(this.rb.velocity.x, 0);
//                this.inputs.jumpState = JumpState.OnGround;
//            }
//        }
//    }

//    private void applyGravityAndFriction()
//    {
//        if (this.inputs.jumpState == JumpState.OnGround
//            || this.inputs.jumpState == JumpState.WallJumping
//            || this.inputs.jumpState == JumpState.Jumping)
//            return;

//        var currentVelocity = this.rb.velocity.y;
//        currentVelocity -= this.fallForce;

//        if (this.inputs.jumpState == JumpState.WallSliding)
//            currentVelocity *= this.wallFriction;
//        else
//            currentVelocity *= this.airFriction;

//        this.rb.velocity = new Vector2(this.rb.velocity.x, currentVelocity);
//    }*/
//}
