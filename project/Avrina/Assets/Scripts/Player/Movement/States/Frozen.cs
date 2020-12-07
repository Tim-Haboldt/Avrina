using UnityEngine;

public class Frozen : State
{
    /// <summary>
    ///  Name of the state is InAir
    /// </summary>
    public override PlayerState name { get; } = PlayerState.Frozen;
    /// <summary>
    ///  The player is effected by gravity and can move while in air
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new Gravity(),
    };


    /**
     * <summary>
     *  Will only exit if the player is not frozen anymore
     * </summary>
     */
    public override PlayerState Update()
    {
        if (this.playerEffectManager.statusEffect == StatusEffect.FROZEN)
        {
            return this.name;
        }

        return this.previousState;
    }
}
