using UnityEngine;

public abstract class State
{
    ///<summary>
    /// What is the name of the State.
    /// Needs to be overriden in the child class
    ///</summary>
    public abstract PlayerState name { get; }
    ///<summary>
    /// What actions will be performed inside the state (like gravity, movement, etc..).
    /// The order inside the actions can effect the gameplay.
    ///</summary>
    protected abstract Action[] actions { get; }


    /**
     * <summary>
     *  Used to instanciate all states and actions
     * </summary>
     * <param name="config">Contains all constants</param>
     */
    public void Setup(PlayerConfig config)
    {
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
    public void OnEnter()
    {
        foreach (Action action in this.actions)
        {
            action.OnEnter();
        }
    }

    /**
     * <summary>
     *  Checks if the player state has to be changed.
     *  Returns a state different then the current state if a state change is required.
     *  If the name of this state is returned no change is performed
     * </summary>
     */
    public abstract PlayerState Update();

    /**
     * <summary>
     *  Handels every input and updates the player corresponding
     * </summary>
     * <param name="velocity">Used to change the velocity of the player</param>
     */
    public void PerformActions(ref Vector2 velocity)
    {
        foreach (Action action in this.actions)
        {
            action.PerformAction(ref velocity);
        }
    }

    /**
     * <summary>
     *  On exit event.
     *  Will be triggered if the state is exited
     * </summary>
     */
    public void OnExit()
    {
        foreach (Action action in this.actions)
        {
            action.OnExit();
        }
    }
}
