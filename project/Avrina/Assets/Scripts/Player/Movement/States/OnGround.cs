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
        new HorizontalMovement(),
        new HorizontalFriction(),
    };


    /// <summary>
    ///  Returns the Jumping state if the jump input is pressed.
    ///  Returns the InAir state if the player is not touching the ground.
    /// </summary>
    /// <returns></returns>
    public override PlayerState Update()
    {
        if (this.playerController.jumpInput && !this.holdingJump && this.playerController.groundMaterial.canBeJumpedFrom)
        {
            return PlayerState.Jumping;
        }
        else if (!this.playerController.onGround)
        {
            return PlayerState.InAir;
        }

        if (this.holdingJump && !this.playerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    /**
     * <summary>
     *  Set the horizontal movement and checks if the jump input is pressed while entering the state
     * </summary>
     */
    public override void OnStateEnter()
    {
        base.OnStateEnter();

        if (this.playerController.jumpInput)
        {
            this.holdingJump = true;
        }

        this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, 0);
        this.rigidbody.AddForce(new Vector2(0, -10f));
    }
}
