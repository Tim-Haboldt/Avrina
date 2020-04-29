using UnityEngine;

public class InAir : State
{
    /// <summary>
    ///  If the player was jumping before the InAir state the AirJump state will not be triggered until the player is not pressing jump
    /// </summary>
    private bool holdingJump = false;

    /// <summary>
    ///  Name of the state is InAir
    /// </summary>
    public override PlayerState name { get; } = PlayerState.InAir;
    /// <summary>
    ///  The player is effected by gravity and can move while in air
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new Gravity(),
        new VerticalMovement(),
    };


    /**
     * <summary>
     *  Will exit the state if any of the following conditions is met:
     *   - Player touches the ground
     *   - Player is pressing jump
     * </summary>
     */ 
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
        }
        else if (PlayerController.jumpInput && !this.holdingJump)
        {
            return PlayerState.AirJumping;
        } else if (PlayerController.hasWallLeft && direction == -1 || PlayerController.hasWallRight && direction == 1)
        {
            return PlayerState.WallSliding;
        }

        if (this.holdingJump && !PlayerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    /// <summary>
    ///  Sets the holding jump variable if the player is pressing jumping while entering the state
    /// </summary>
    public override void OnStateEnter()
    {
        base.OnStateEnter();

        if (PlayerController.jumpInput)
        {
            this.holdingJump = true;
        }
    }
}
