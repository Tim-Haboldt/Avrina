using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(MagicSystemController))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerStateManager : NetworkBehaviour
{
    [Header("Input Prefabs")]
    [SerializeField] private InputController joyStickInputControllerPrefab = null;
    [SerializeField] private InputController keyboardInputControllerPrefab = null;
    [SerializeField] private InputController mouseInputControllerPrefab = null;
    [SerializeField] private InputController networkInputControllerPrefab = null;

    [Header("Input Controller")]
    /// <summary>
    ///  Left wall trigger. Stores the information about all close wall to the left of the player
    /// </summary>
    [SerializeField] private PlayerCollider wallSlideColliderLeft = null;
    /// <summary>
    ///  Right wall trigger. Stores the information about all close wall to the right of the player
    /// </summary>
    [SerializeField] private PlayerCollider wallSlideColliderRight = null;
    /// <summary>
    ///  Ground trigger. Stores the information about all ground colliders the player is staying on
    /// </summary>
    [SerializeField] private PlayerCollider onGroundCollider = null;
    /// <summary>
    ///  What is the mask of the ground or wall objects
    /// </summary>
    [SerializeField] private LayerMask groundMask = 0;

    [Header("Configuration")]
    /// <summary>
    ///  Player Material defines all movement related player constants
    /// </summary>
    [SerializeField] private PlayerConfig playerConfig;

    /// <summary>
    ///  Current status of the player object like health or current applied status effect
    /// </summary>
    private PlayerStatus playerStatus;
    /// <summary>
    ///  Handels the animations of the player
    /// </summary>
    private PlayerAnimation playerAnimation;
    /// <summary>
    ///  Stores a reference to the rigidbody of the player
    /// </summary>
    private Rigidbody2D rb;
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

        var inputController = this.CreateInputController();
        this.playerAnimation.inputController = inputController;

        foreach (State state in this.states.Values)
        {
            state.Init(inputController, this.rb);
        }

        var magicSystemController = this.GetComponent<MagicSystemController>();
        magicSystemController.inputController = inputController;

        this.playerStatus = this.GetComponent<PlayerStatus>();
        this.playerStatus.inputController = inputController;
        this.playerStatus.Init(
            this.isLocalPlayer ? PlayerStatus.InitStatus.PLAYER_ONE 
            : this.hasAuthority ? PlayerStatus.InitStatus.PLAYER_TWO 
            : PlayerStatus.InitStatus.UNKNOWN
        );

        if (this.hasAuthority)
        {
            this.CmdSpawnSpirits(!this.isLocalPlayer);
        }

        this.ApplyConfig();
    }

    /// <summary>
    ///  Spawns the spirits for the player
    /// </summary>
    [Command]
    private void CmdSpawnSpirits(bool isSecondPlayer)
    {
        (NetworkManager.singleton as Server).SpawnSpirits(this.connectionToClient, isSecondPlayer);
    }

    /// <summary>
    ///  Creates an input controller
    /// </summary>
    private InputController CreateInputController()
    {
        InputController inputController = null;
        if (this.hasAuthority)
        {
            Camera.main.GetComponent<CameraFollowObjects>().AddObjectToFollow(this.transform);

            if (this.isLocalPlayer)
            {
                inputController = this.GetInputControllerOfMappingType(PlayerInformation.playerOneMapping);
            }
            else
            {
                inputController = this.GetInputControllerOfMappingType(PlayerInformation.playerTwoMapping);
            }
        }
        else
        {
            inputController = Instantiate(this.networkInputControllerPrefab);
        }

        inputController.Init(
            this.GetComponent<CapsuleCollider2D>(),
            this.wallSlideColliderLeft,
            this.wallSlideColliderRight,
            this.onGroundCollider,
            this.groundMask
        );

        return inputController;
    }

    /// <summary>
    ///  Returns an instance of the input controller corresponding to the given mapping type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private InputController GetInputControllerOfMappingType(MappingType type)
    {
        switch (type)
        {
            case MappingType.KeyBoard:
                return Instantiate(this.keyboardInputControllerPrefab);
            case MappingType.Mouse:
                return Instantiate(this.mouseInputControllerPrefab);
            case MappingType.JoyStick:
            default:
                return Instantiate(this.joyStickInputControllerPrefab);
        }
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
        if (!this.hasAuthority)
        {
            return;
        }

        var playerVelocity = this.rb.velocity;
        this.states[this.currentState].PerformActions(ref playerVelocity);
        this.rb.velocity = playerVelocity;
    }
}
