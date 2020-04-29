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
    ///  The player can move vertically
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new VerticalMovement(),
        new VerticalFriction(),
    };


    /// <summary>
    ///  Returns the Jumping state if the jump input is pressed.
    ///  Returns the InAir state if the player is not touching the ground.
    /// </summary>
    /// <returns></returns>
    public override PlayerState Update()
    {
        if (PlayerController.jumpInput && !this.holdingJump)
        {
            return PlayerState.Jumping;
        }
        else if (!PlayerController.onGround)
        {
            return PlayerState.InAir;
        }

        if (this.holdingJump && !PlayerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    /**
     * <summary>
     *  Set the vertical movement and checks if the jump input is pressed while entering the state
     * </summary>
     */
    public override void OnStateEnter()
    {
        base.OnStateEnter();

        if (PlayerController.jumpInput)
        {
            this.holdingJump = true;
        }

        Rigidbody2D rb = StateManager.instance.rb;
        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(new Vector2(0, -10f));
        //TODO set player position
        //TODO add new states
    }
}
