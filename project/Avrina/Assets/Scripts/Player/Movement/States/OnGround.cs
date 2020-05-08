using UnityEngine;

public class OnGround : State
{
    /// <summary>
    ///  If the player was jumping before the OnGround state the Jumping state will not be triggered until the player is not pressing jump
    /// </summary>
    private bool holdingJump = false;

    ///<summary>
    /// Name of the state is onGround
    ///</summary>
    public override PlayerState name { get; } = PlayerState.OnGround;
    /// <summary>
    ///  The player can move horizontally
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new HorizontalGroundMovement(),
    };


    /// <summary>
    ///  Returns the Jumping state if the jump input is pressed.
    ///  Returns the InAir state if the player is not touching the ground.
    /// </summary>
    /// <returns></returns>
    public override PlayerState Update()
    {
        var movementInput = this.playerController.movementInput;
        
        if (
            this.playerController.jumpInput && !this.holdingJump
            && (this.playerController.groundMaterial == null || this.playerController.groundMaterial.canBeJumpedFrom)
        ) {
            return PlayerState.Jumping;
        }
        else if (!this.playerController.onGround)
        {
            return PlayerState.InAir;
        }
        else if (
            (movementInput > 0 && this.playerController.hasWallRight
            || movementInput < 0 && this.playerController.hasWallLeft)
            && this.playerController.wallMaterial.canBeClimedOn
        ) {
            return PlayerState.WallSliding;
        }

        if (this.holdingJump && !this.playerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    /// <summary>
    ///  Sets the vertical velocity of the player to zero
    /// </summary>
    public override void OnStateEnter()
    {
        base.OnStateEnter();

        if (this.playerController.jumpInput)
        {
            this.holdingJump = true;
        }

        var velocity = this.rigidbody.velocity;
        this.rigidbody.velocity = new Vector2(velocity.x, 0);
    }
}
