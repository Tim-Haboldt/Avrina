using UnityEngine;

public class OnGround : State
{
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

    public override PlayerState Update()
    {
        if (PlayerController.jumpInput)
        {
            return PlayerState.Jumping;
        }
        if (!PlayerController.onGround)
        {
            return PlayerState.InAir;
        }

        return this.name;
    }

    /**
     * <summary>
     *  Set the vertical movmeent 
     * </summary>
     */
    public override void OnStateEnter()
    {
        base.OnStateEnter();

        Rigidbody2D rb = StateManager.instance.rb;
        rb.velocity = new Vector2(rb.velocity.x, 0);

        //TODO set player position
        //TODO add new states
    }
}
