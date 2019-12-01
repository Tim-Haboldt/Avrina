using System.Collections;
using System.Collections.Generic;
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
                this
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
