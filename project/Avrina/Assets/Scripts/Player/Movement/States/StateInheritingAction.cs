using UnityEngine;

public abstract class StateInheritingAction : State
{
    /**
     * <summary>
     *  Will be called at the start of the game in order to setup an action
     * </summary>
     * <param name="config">Contains all constant variables</param>
     */
    protected abstract void Setup(PlayerConfig config);
    /**
     * <summary>
     *  Used to initialize the action class.
     *  Will be called on entering a state.
     * </summary>
     */
    protected abstract void OnEnter();
    /**
     * <summary>
     *  Will execute the action.
     * </summary>
     * <example>
     *  Apply gravity to player
     * </example>
     * <param name="velocity">Used to modify the velocity of the player</param>
     */
    protected abstract void PerformAction(ref Vector2 velocity);
    /**
     * <summary>
     *  Used to reset the action class.
     *  Will be called at the end of a state.
     * </summary>
     */
    protected abstract void OnExit();

    /**
    * <summary>
    *  Used to instanciate all states and actions
    * </summary>
    * <param name="config">Contains all constants</param>
    */
    public override void StateSetup(PlayerConfig config)
    {
        this.Setup(config);

        foreach (var action in this.actions)
        {
            action.Setup(config);
        }
    }

    /**
     * <summary>
     *  On enter event.
     *  Will be triggered if the state is entered
     * </summary>
     */
    public override void OnStateEnter()
    {
        this.OnEnter();

        foreach (Action action in this.actions)
        {
            action.OnEnter();
        }
    }

    /**
     * <summary>
     *  Handels every input and updates the player corresponding
     * </summary>
     * <param name="velocity">Used to change the velocity of the player</param>
     */
    public override void PerformActions(ref Vector2 velocity)
    {
        this.PerformAction(ref velocity);

        foreach (Action action in this.actions)
        {
            action.PerformAction(ref velocity, this.inputController);
        }
    }

    /**
     * <summary>
     *  On exit event.
     *  Will be triggered if the state is exited
     * </summary>
     */
    public override void OnStateExit()
    {
        this.OnExit();

        foreach (Action action in this.actions)
        {
            action.OnExit();
        }
    }
}
