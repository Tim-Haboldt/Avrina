using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : ElementBase
{
    protected override Element element { get { return Element.Fire; } }

    protected override void AirElementWasFirst()
    {
        throw new System.NotImplementedException();
    }

    protected override void EarthElementWasFirst()
    {
        throw new System.NotImplementedException();
    }

    protected override void FireElementWasFirst()
    {
        throw new System.NotImplementedException();
    }

    protected override void WaterElementWasFirst()
    {
        throw new System.NotImplementedException();
    }
}
