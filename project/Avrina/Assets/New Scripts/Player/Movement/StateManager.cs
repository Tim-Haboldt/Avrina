using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    /// <summary>
    ///  Rigid body of the player
    /// </summary>
    private Rigidbody2D rb;
    /// <summary>
    ///  Player Material defines all movement related player constants
    /// </summary>
    public PlayerConfig playerConfig;
    /// <summary>
    ///  Defines all states the player can possible have
    /// </summary>
    private Dictionary<PlayerState, State> states = new Dictionary<PlayerState, State>()
    {
        { PlayerState.InAir, new InAir() },
        { PlayerState.OnGround, new OnGround() },
        // { PlayerState.Jumping, new Jumping() },
    };
    /// <summary>
    ///  Stores the name of the current state
    /// </summary>
    public PlayerState currentState = PlayerState.InAir;


    /**
     * <summary>
     *  Used to change the state of the player
     * </summary>
     * <param name="state">Name of the next state</param>
     */
    public void ChangeState(PlayerState state)
    {
        this.states[this.currentState].OnExit();
        this.currentState = state;
    }
    
    /**
     * <summary>
     *  Will be called at the start of the game.
     *  Gets the rigid body of the player and stores it.
     *  Applies the player config to the states.
     * </summary>
     */
    private void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.ApplyConfig();
    }

    /**
     * <summary>
     *  Updates all states default values with the values from the player config
     * </summary>
     */ 
    public void ApplyConfig()
    {
        foreach (var iterator in this.states)
        {
            iterator.Value.Setup(this.playerConfig);
        }
    }

    /**
     * <summary>
     *  Default update function from unity.
     *  Keeps track of the current player state
     * </summary>
     */
    private void Update()
    {
        var nextState = this.states[this.currentState].Update();
        if (nextState != this.currentState)
        {
            this.ChangeState(nextState);
        }
    }

    /**
     * <summary>
     *  Default physics update function from unity.
     *  Listen to the inputs and move the player accordingly
     * </summary>
     */
    private void FixedUpdate()
    {
        var playerVelocity = this.rb.velocity;
        this.states[this.currentState].PerformActions(ref playerVelocity);
        this.rb.velocity = playerVelocity;
    }
}
