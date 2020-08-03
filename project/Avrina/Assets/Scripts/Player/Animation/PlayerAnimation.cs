using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerStateManager))]
public class PlayerAnimation : MonoBehaviour
{
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
    ///  States what amount of velocity is necessary to switch to running
    /// </summary>
    public float runningVelocity;


    /// <summary>
    ///  Finds the animator in the player object and sets the default values for the animation
    /// </summary>
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.stateManager = GetComponent<PlayerStateManager>();

        this.animator.SetFloat("X", 0);
        this.animator.SetBool("isRunning", false);
        this.animator.SetBool("isFalling", false);
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
        this.animator.SetBool("isClimbing", currentState == PlayerState.WallSliding);

        // Set the direction the sprite is facing
        if (movementAbs > 0)
        {
            this.spriteRenderer.flipX = Mathf.Sign(movementInput) == 1;
        }
    }

    /// <summary>
    ///  Triggers animations caused by state change
    /// </summary>
    /// <param name="state">What is the new state</param>
    public void TriggerState(PlayerState state)
    {
        // this.animator.SetTrigger();
    }
}
