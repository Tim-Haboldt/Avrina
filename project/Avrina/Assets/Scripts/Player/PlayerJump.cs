using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerJump : MonoBehaviour
{
    // Defines how long the Jump button can be pressed
    [SerializeField] public float maxJumpDuration;
    // How much force will be applied when the jump button is pressed first
    [SerializeField] public float jumpForce;
    // How much force will be applied each frame while a wall jump is performed
    [SerializeField] public float wallJumpForce;
    // The fall of the player should be a lot faster then the actual jump
    [SerializeField] public float fallForce;
    // If the player is falling faster then max fall speed reduce speed
    [Range(0, 1)]
    [SerializeField] public float airFriction;
    [Range(0, 1)]
    [SerializeField] public float wallFriction;

    // The player can jump twice. This value defines if the player can jump again
    private bool hasExtraJump;
    // Has the player used his first jump
    private bool hasJump;
    // When did the player start jumping
    private float startTime;

    private bool isWallJumping;
    private bool isJumping;

    // Used to get the Information which player input is given
    private PlayerInput inputs;
    // Rigidbody of the player used to apply forces
    private Rigidbody2D rb;

    private void Start()
    {
        this.inputs = GetComponent<PlayerInput>();
        this.rb = GetComponent<Rigidbody2D>();
        
        this.hasExtraJump = true;
        this.hasJump = false;
        this.isJumping = false;
        this.isWallJumping = false;
    }

    private void FixedUpdate()
    {
        // Apply Jump forces
        if (this.inputs.onGround)
        {
            if (this.inputs.jumpInput)
            {
                if (!this.isJumping && !this.isWallJumping)
                {
                    if (this.hasJump)
                    {
                        this.isJumping = true;
                        this.hasJump = false;
                        this.startTime = Time.time;
                    }
                    else if (this.hasExtraJump)
                    {
                        this.isJumping = true;
                        this.hasExtraJump = false;
                        this.startTime = Time.time;
                    }
                }
            }
            else
            {
                // Reset jumps
                this.isJumping = false;
                this.isWallJumping = false;
                this.hasExtraJump = true;
                this.hasJump = true;
            }
        } else if (this.inputs.isSlidingTheWall)
        {
            if (this.inputs.jumpInput)
            {
                if (!this.isJumping && !this.isWallJumping)
                {
                    if (this.hasExtraJump)
                    {
                        this.isWallJumping = true;
                        this.hasExtraJump = false;
                        this.startTime = Time.time;
                    }
                }
            }
            else
            {
                this.hasExtraJump = true;
                this.isJumping = false;
                this.isWallJumping = false;
            }
        } else if (this.inputs.jumpInput)
        {
            if (!this.isJumping && !this.isWallJumping)
            {
                if (this.hasExtraJump)
                {
                    this.hasExtraJump = false;
                    this.isJumping = true;
                    this.startTime = Time.time;
                }
            }
        } else
        {
            this.isWallJumping = false;
            this.isJumping = false;
        }

        // Apply gravity
        var passedTime = Time.time - this.startTime;
        if (this.isJumping && this.inputs.jumpInput && passedTime < this.maxJumpDuration)
        {
            this.rb.velocity = new Vector2(this.rb.velocity.x, this.jumpForce);
        }
        else if (this.isWallJumping && this.inputs.jumpInput && passedTime < this.maxJumpDuration)
        {
            this.rb.velocity = new Vector2(this.rb.velocity.x, this.wallJumpForce);
        }
        else if (!this.inputs.onGround)
        {
            var currentVelocity = this.rb.velocity.y;
            currentVelocity -= this.fallForce;

            if (this.inputs.isSlidingTheWall)
                currentVelocity *= this.wallFriction;
            else
                currentVelocity *= this.airFriction;

            this.rb.velocity = new Vector2(this.rb.velocity.x, currentVelocity);
        } else
        {
            this.rb.velocity = new Vector2(this.rb.velocity.x, 0);
        }
    }
}
