using UnityEngine;

public class WallSliding : StateInheritingAction
{
    /// <summary>
    ///  Stores if the player was holding jump before the state was entered
    /// </summary>
    private bool holdingJump;
    /// <summary>
    ///  What is the gravitation
    /// </summary>
    private float gravity;
    /// <summary>
    ///  Stores the direction of the wall
    /// </summary>
    private WallslidingDirection direction;
    /// <summary>
    ///  The name of the state is Wallsliding
    /// </summary>
    public override PlayerState name { get; } = PlayerState.WallSliding;
    /// <summary>
    ///  Defines all actions that can occour in the wallsliding state.
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new HorizontalAirMovement(),
    };

    /// <summary>
    ///  Switches to the OnGround state if the ground is touched.
    ///  Switches to the InAir state if the wall is no touched anymore
    ///  Switches to the Walljumping state if the jump is performed
    /// </summary>
    /// <returns>New state. Default is the same state</returns>
    public override PlayerState Update()
    {
        if (this.direction == WallslidingDirection.Unknown)
        {
            return PlayerState.InAir;
        }

        var inputDirection = this.playerController.movementInput;
        if (inputDirection != 0)
        {
            inputDirection = Mathf.Sign(inputDirection);
        }


        var wallMaterial = this.playerController.wallMaterial;
        if (wallMaterial == null)
        {
            return PlayerState.InAir;
        }
        else if (this.playerController.onGround && !wallMaterial.canBeClimedOn)
        {
            return PlayerState.OnGround;
        }
        else if (this.playerController.jumpInput && !this.holdingJump && wallMaterial.canBeJumpedFrom)
        {
            return PlayerState.WallJumping;
        }
        else if (!(this.direction == WallslidingDirection.Left && this.playerController.hasWallLeft && inputDirection == -1)
            && !(this.direction == WallslidingDirection.Right && this.playerController.hasWallRight && inputDirection == 1)
            || this.playerController.duckInput && !wallMaterial.canBeClimedOn)
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
        var wallMaterial = this.playerController.wallMaterial;
        if (wallMaterial == null)
        {
            return;
        }


        var newVelocityY = velocity.y;

        if (wallMaterial.canBeClimedOn)
        {
            if (this.playerController.duckInput)
            {
                newVelocityY -= wallMaterial.acceleration + newVelocityY * wallMaterial.frictionWhileMovingDown;
            }
            else if (this.playerController.lookUpInput)
            {
                newVelocityY += wallMaterial.acceleration - newVelocityY * wallMaterial.frictionWhileMovingUp;
            }
            else
            {
                newVelocityY -= newVelocityY * wallMaterial.friction;
            }
        }
        else
        {
            newVelocityY -= this.gravity * wallMaterial.friction;
        }

        velocity = new Vector2(velocity.x, newVelocityY);
    }

    /// <summary>
    ///  Reads the gravitation from the config
    /// </summary>
    /// <param name="config">Stores all parameter regarding player movement</param>
    protected override void Setup(PlayerConfig config)
    {
        this.gravity = config.gravity;
    }

    /// <summary>
    ///  Reads the wallslide directions and resets the vertical player movement if the player can climb
    /// </summary>
    protected override void OnEnter()
    {
        if (this.playerController.hasWallRight)
        {
            this.direction = WallslidingDirection.Right;
        }
        else if (this.playerController.hasWallLeft)
        {
            this.direction = WallslidingDirection.Left;
        }
        else
        {
            this.direction = WallslidingDirection.Unknown;
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
