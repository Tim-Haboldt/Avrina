using UnityEngine;

public class WallSliding : StateInheritingAction
{
    /// <summary>
    ///  What is the speed of the player sliding down the wall
    /// </summary>
    private float defaultWallslidingForce;
    /// <summary>
    ///  What is the maximal speed of the player sliding down the wall
    /// </summary>
    private float maxWallslidingForce;
    /// <summary>
    ///  Stores if the player was holding jump before the state was entered
    /// </summary>
    private bool holdingJump;
    /// <summary>
    ///  Stores the direction of the wall
    /// </summary>
    private WallslidingDirection dir;
    /// <summary>
    ///  The name of the state is Wallsliding
    /// </summary>
    public override PlayerState name { get; } = PlayerState.WallSliding;
    /// <summary>
    ///  Defines all actions that can occour in the wallsliding state.
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new HorizontalMovement(),
    };

    /// <summary>
    ///  Switches to the OnGround state if the ground is touched.
    ///  Switches to the InAir state if the wall is no touched anymore
    ///  Switches to the Walljumping state if the jump is performed
    /// </summary>
    /// <returns>New state. Default is the same state</returns>
    public override PlayerState Update()
    {
        var direction = this.playerController.movementInput;
        if (direction != 0)
        {
            direction = Mathf.Sign(direction);
        }

        if (this.playerController.onGround)
        {
            return PlayerState.OnGround;
        } else if (this.playerController.jumpInput && !this.holdingJump && this.playerController.wallMaterial.canBeJumpedFrom)
        {
            return PlayerState.WallJumping;
        }
        else if (!(this.dir == WallslidingDirection.Left && this.playerController.hasWallLeft && direction == -1)
            && !(this.dir == WallslidingDirection.Right && this.playerController.hasWallRight && direction == 1))
        {
            return PlayerState.InAir;
        }

        if (this.holdingJump && !this.playerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    /// <summary>
    ///  Sets the downwards velocity to the value of the variable wallslideSpeed
    /// </summary>
    /// <param name="velocity"></param>
    protected override void PerformAction(ref Vector2 velocity)
    {
        float velocityY = velocity.y;
        velocityY -= this.playerController.wallMaterial.acceleration;

        float maxVelocity = this.maxWallslidingForce * -1;
        maxVelocity *= 0.5f - this.playerController.wallMaterial.friction;

        if (velocityY < maxVelocity)
        {
            velocityY = maxVelocity;
        }

        velocity = new Vector2(velocity.x, velocityY);
    }

    /// <summary>
    ///  Reads and sets the wallslide speed of the player from the config
    /// </summary>
    /// <param name="config">Stores all parameter regarding player movement</param>
    protected override void Setup(PlayerConfig config)
    {
    }

    /// <summary>
    ///  Sets the direction of the wallslide and sets the vertial movement to zero
    /// </summary>
    protected override void OnEnter()
    {
        // Set the current horizontal movement to zero
        this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, 0);

        if (this.playerController.hasWallRight)
        {
            this.dir = WallslidingDirection.Right;
        } else if (this.playerController.hasWallLeft)
        {
            this.dir = WallslidingDirection.Left;
        } else
        {
            this.dir = WallslidingDirection.Unknown;
        }

        if (this.playerController.jumpInput)
        {
            this.holdingJump = true;
        }
    }

    /// <summary>
    ///  Unused
    /// </summary>
    protected override void OnExit() { }
}
