using UnityEngine;

public class WallJumping : StateInheritingAction
{
    /// <summary>
    ///  The name of the state is Walljumping
    /// </summary>
    public override PlayerState name { get; } = PlayerState.WallJumping;
    /// <summary>
    ///  Defines all actions that can occour in the walljumping state.
    /// </summary>
    protected override Action[] actions { get; } = new Action[] { };

    public override PlayerState Update()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    protected override void PerformAction(ref Vector2 velocity)
    {
        throw new System.NotImplementedException();
    }

    protected override void Setup(PlayerConfig config)
    {
        throw new System.NotImplementedException();
    }
}
