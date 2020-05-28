using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(InputController))]
public class PlayerStateManager : NetworkBehaviour
{
    /// <summary>
    ///  Handels the animations of the player
    /// </summary>
    private PlayerAnimation playerAnimation;
    /// <summary>
    ///  Stores a reference to the rigidbody of the player
    /// </summary>
    private Rigidbody2D rb;
    /// <summary>
    ///  Player Material defines all movement related player constants
    /// </summary>
    [SerializeField]
    private PlayerConfig playerConfig;
    /// <summary>
    ///  Defines all states the player can possible have
    /// </summary>
    private Dictionary<PlayerState, State> states = new Dictionary<PlayerState, State>()
    {
        { PlayerState.InAir, new InAir() },
        { PlayerState.OnGround, new OnGround() },
        { PlayerState.Jumping, new Jumping() },
        { PlayerState.AirJumping, new AirJumping() },
        { PlayerState.Immobile, new Immobile() },
        { PlayerState.WallSliding, new WallSliding() },
        { PlayerState.WallJumping, new WallJumping() }
    };
    /// <summary>
    ///  Stores the name of the current state
    /// </summary>
    [SyncVar(hook = nameof(HandlePlayerStateChange))]
    public PlayerState currentState = PlayerState.InAir;


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
        this.playerAnimation = GetComponent<PlayerAnimation>();

        var inputController = GetComponent<InputController>();
        inputController.hasAuthority = this.hasAuthority;

        foreach (State state in this.states.Values)
        {
            state.Init(inputController, this.rb);
        }

        this.ApplyConfig();
    }

    /**
     * <summary>
     *  Used to change the state of the player
     * </summary>
     * <param name="state">Name of the next state</param>
     */
    public void ChangeState(PlayerState state)
    {
        this.CmdUpdatePlayerState(state);

        var previousState = this.states[this.currentState];
        var nextState = this.states[state];

        previousState.OnStateExit();
        nextState.previousState = this.currentState;
        nextState.OnStateEnter();
        this.currentState = state;

        this.playerAnimation.TriggerState(state);
    }

    /// <summary>
    ///  Will be called on all other clients to make sure the states are setup correctly
    /// </summary>
    /// <param name="oldState"></param>
    /// <param name="newState"></param>
    private void HandlePlayerStateChange(PlayerState oldState, PlayerState newState)
    {
        this.states[oldState].OnStateExit();
        this.states[newState].OnStateEnter();
    }

    /// <summary>
    ///  Updates the player state for all clients via the server
    /// </summary>
    /// <param name="state"></param>
    [Command]
    private void CmdUpdatePlayerState(PlayerState state)
    {
        this.currentState = state;
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
            iterator.Value.StateSetup(this.playerConfig);
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
        if (!this.hasAuthority)
        {
            return;
        }

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
