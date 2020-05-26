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

        var inputDirection = this.inputController.movementInput;
        if (inputDirection != 0)
        {
            inputDirection = Mathf.Sign(inputDirection);
        }

        var wallMaterial = this.inputController.wallMaterial;
        if (wallMaterial == null)
        {
            return PlayerState.InAir;
        }
        else if (this.inputController.onGround && !wallMaterial.canBeClimedOn)
        {
            return PlayerState.OnGround;
        }
        else if (this.inputController.jumpInput && !this.holdingJump && wallMaterial.canBeJumpedFrom)
        {
            return PlayerState.WallJumping;
        }
        else if (!(this.direction == WallslidingDirection.Left && this.inputController.hasWallLeft && inputDirection == -1)
            && !(this.direction == WallslidingDirection.Right && this.inputController.hasWallRight && inputDirection == 1)
            || this.inputController.duckInput && !wallMaterial.canBeClimedOn)
        {
            return PlayerState.InAir;
        }

        if (this.holdingJump && !this.inputController.jumpInput)
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
        var wallMaterial = this.inputController.wallMaterial;
        if (wallMaterial == null)
        {
            return;
        }


        var newVelocityY = velocity.y;

        if (wallMaterial.canBeClimedOn)
        {
            if (this.inputController.duckInput)
            {
                newVelocityY -= wallMaterial.acceleration + newVelocityY * wallMaterial.frictionWhileMovingDown;
            }
            else if (this.inputController.lookUpInput)
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
        if (this.inputController.hasWallRight)
        {
            this.direction = WallslidingDirection.Right;
        }
        else if (this.inputController.hasWallLeft)
        {
            this.direction = WallslidingDirection.Left;
        }
        else
        {
            this.direction = WallslidingDirection.Unknown;
        }

        if (this.inputController.jumpInput)
        {
            this.holdingJump = true;
        }
    }

    /// <summary>
    ///  Unused
    /// </summary>
    protected override void OnExit() { }
}
