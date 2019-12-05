using UnityEngine;

public class Jumping : State, Action
{
    public override PlayerState name
    {
        get
        {
            return PlayerState.Jumping;
        }
    }

    protected override Action[] actions
    {
        get
        {
            return new Action[]
            {
                this,
                new VerticalMovement(),
            };
        }
    }

    public void PerformAction(ref Vector2 velocity)
    {
        throw new System.NotImplementedException();
    }

    public override PlayerState Update()
    {
        return PlayerState.InAir;
    }
}
