using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerStateManager))]
public class PlayerAnimation : MonoBehaviour
{
    /// <summary>
    ///  States what amount of velocity is necessary to switch to running
    /// </summary>
    [SerializeField] private float runningVelocity;
    /// <summary>
    ///  How fast will the climbing animation change into its next state
    /// </summary>
    [SerializeField] private float climbingAnimationChangeSpeed;

    /// <summary>
    ///  Used to controll the player animation
    /// </summary>
    private Animator animator;
    /// <summary>
    ///  Used to extract the information if the player is running
    /// </summary>
    private Rigidbody2D rb;
    /// <summary>
    ///  Used to change the direction of the sprite facing
    /// </summary>
    private SpriteRenderer spriteRenderer;
    /// <summary>
    ///  Used to get all inputs and collisions
    /// </summary>
    [HideInInspector] public InputController inputController;
    /// <summary>
    ///  Used to get the current state of the player
    /// </summary>
    private PlayerStateManager stateManager;
    /// <summary>
    ///  Used to get the current effect status
    /// </summary>
    private PlayerStatus effectStatusManager;
    /// <summary>
    ///  Stores the current climbing state
    /// </summary>
    private float climbingState = 0f;
    /// <summary>
    ///  Last y position of the player
    /// </summary>
    private float lastClimbingPos = 0f;


    /// <summary>
    ///  Finds the animator in the player object and sets the default values for the animation
    /// </summary>
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.rb = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.stateManager = this.GetComponent<PlayerStateManager>();
        this.effectStatusManager = this.GetComponent<PlayerStatus>();

        this.animator.SetFloat("X", 0);
        this.animator.SetBool("isRunning", false);
        this.animator.SetBool("isFalling", false);
        this.animator.SetFloat("Climbing", 0);
    }
    
    /// <summary>
    ///  Updates the animation inputs
    /// </summary>
    void Update()
    {
        if (!this.stateManager.hasAuthority)
        {
            return;
        }

        // Read the movement input
        var movementInput = this.inputController.movementInput;
        var movementAbs = Mathf.Abs(movementInput);
        this.animator.SetFloat("X", movementAbs);

        // Set the isRunning attribute
        this.animator.SetBool("isRunning", Mathf.Abs(this.rb.velocity.x) > this.runningVelocity);

        // Set the isFalling attribute
        var currentState = this.stateManager.currentState;
        var isFalling = (currentState == PlayerState.InAir || currentState == PlayerState.Immobile) && this.rb.velocity.y < 0;
        this.animator.SetBool("isFalling", isFalling);

        // Set the isClimbing attribute
        var isClimbing = currentState == PlayerState.WallSliding;
        this.animator.SetBool("isClimbing", isClimbing);

        if (isClimbing)
        {
            this.climbingState += (this.transform.position.y - this.lastClimbingPos) * this.climbingAnimationChangeSpeed;
            if (this.climbingState > 1)
            {
                this.climbingState -= 1;
            }
            else if (this.climbingState < 0)
            {
                this.climbingState += 1;
            }

            this.animator.SetFloat("Climbing", this.climbingState);
            this.lastClimbingPos = this.transform.position.y;
        }

        // Set the direction the sprite is facing
        if (movementAbs > 0)
        {
            if (this.effectStatusManager.statusEffect == StatusEffect.FROZEN)
            {
                return;
            }

            this.spriteRenderer.flipX = Mathf.Sign(movementInput) == 1;
        }
    }
}
