public class Immobile : State
{
    /// <summary>
    ///  The name of the state is immobile
    /// </summary>
    public override PlayerState name { get; } = PlayerState.Immobile;
    /// <summary>
    ///  Defines all actions that can occour in the immobile state.
    ///  The player can move and is affected by gravity
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new Gravity(),
        new VerticalMovement(),
    };

    /// <summary>
    ///  Only changes the state if the player touches the ground
    /// </summary>
    /// <returns>New state. Default is the same state</returns>
    public override PlayerState Update()
    {
        if (PlayerController.onGround)
        {
            return PlayerState.OnGround;
        }

        return this.name;
    }
}
