using UnityEngine;

public class InAir : State
{
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
     * </summary>
     */ 
    public override PlayerState Update()
    {
        if (PlayerController.onGround)
        {
            return PlayerState.OnGround;
        } else if (PlayerController.jumpInput)
        {
            return PlayerState.AirJumping;
        }

        return this.name;
    }
}
