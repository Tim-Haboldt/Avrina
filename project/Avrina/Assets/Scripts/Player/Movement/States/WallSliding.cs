using UnityEngine;

public class WallSliding : StateInheritingAction
{
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
        new VerticalMovement()
    };

    /// <summary>
    ///  Switches to the OnGround state if the ground is touched.
    ///  Switches to the InAir state if the wall is no touched anymore
    ///  Switches to the Walljumping state if the jump is performed
    /// </summary>
    /// <returns>New state. Default is the same state</returns>
    public override PlayerState Update()
    {
        var direction = PlayerController.movementInput;
        if (direction != 0)
        {
            direction = Mathf.Sign(direction);
        }

        if (PlayerController.onGround)
        {
            return PlayerState.OnGround;
        } else if (PlayerController.jumpInput && !this.holdingJump)
        {
            return PlayerState.WallJumping;
        }
        else if (!(this.dir == WallslidingDirection.Left && PlayerController.hasWallLeft && direction != -1)
            && !(this.dir == WallslidingDirection.Right && PlayerController.hasWallRight && direction != 1))
        {
            return PlayerState.InAir;
        }

        if (this.holdingJump && !PlayerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    protected override void PerformAction(ref Vector2 velocity)
    {
    }

    protected override void Setup(PlayerConfig config)
    {
    }

    /// <summary>
    ///  Sets the direction of the wallslide
    /// </summary>
    protected override void OnEnter()
    {
        if (PlayerController.hasWallRight)
        {
            this.dir = WallslidingDirection.Right;
        } else if (PlayerController.hasWallLeft)
        {
            this.dir = WallslidingDirection.Left;
        } else
        {
            this.dir = WallslidingDirection.Unknown;
        }

        if (PlayerController.jumpInput)
        {
            this.holdingJump = true;
        }
    }

    protected override void OnExit()
    {
    }
}
