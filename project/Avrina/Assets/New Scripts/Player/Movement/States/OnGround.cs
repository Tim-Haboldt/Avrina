using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : State
{
    public override PlayerState name
    {
        get
        {
            return PlayerState.OnGround;
        }
    }

    protected override Action[] actions {
        get {
            return new Action[]
            {
                
            };
        }
    }

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
}
