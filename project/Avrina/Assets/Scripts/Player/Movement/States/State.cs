using UnityEngine;

public abstract class State
{
    ///<summary>
    /// What is the name of the State.
    /// Needs to be overriden in the child class
    ///</summary>
    public abstract PlayerState name { get; }
    /// <summary>
    ///  Handels all key inputs and collisions
    /// </summary>
    protected PlayerController playerController { private set; get; }
    /// <summary>
    ///  Rigidbody of the player. Can be used to change position in more detail. Don`t use it everywhere.
    /// </summary>
    protected Rigidbody2D rigidbody { private set; get; }
    ///<summary>
    /// What actions will be performed inside the state (like gravity, movement, etc..).
    /// The order inside the actions can effect the gameplay.
    ///</summary>
    protected abstract Action[] actions { get; }

    /// <summary>
    ///  Sets the player controller
    /// </summary>
    /// <param name="playerController">Used to get all inputs and collisions</param>
    /// <param name="rb">Can be used to modify the player position in more detail.</param>
    public void Init(PlayerController playerController, Rigidbody2D rb)
    {
        this.playerController = playerController;
        this.rigidbody = rb;
    }

    /**
     * <summary>
     *  Used to instanciate all states and actions
     * </summary>
     * <param name="config">Contains all constants</param>
     */
    public virtual void StateSetup(PlayerConfig config)
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
    public virtual void OnStateEnter()
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
    public virtual void PerformActions(ref Vector2 velocity)
    {
        foreach (Action action in this.actions)
        {
            action.PerformAction(ref velocity, this.playerController);
        }
    }

    /**
     * <summary>
     *  On exit event.
     *  Will be triggered if the state is exited
     * </summary>
     */
    public virtual void OnStateExit()
    {
        foreach (Action action in this.actions)
        {
            action.OnExit();
        }
    }
}
