using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
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

        this.animator.SetFloat("X", 0);
        this.animator.SetBool("isRunning", false);
        this.animator.SetBool("isFalling", false);
    }
    
    /// <summary>
    ///  Updates the animation inputs
    /// </summary>
    void Update()
    {
        float movementInput = PlayerController.movementInput;
        float movementAbs = Mathf.Abs(movementInput);

        if (movementAbs > 0)
        {
            this.spriteRenderer.flipX = Mathf.Sign(movementInput) == -1;
        }

        this.animator.SetFloat("X", movementAbs);
        this.animator.SetBool("isRunning", this.rb.velocity.x >= runningVelocity);
        this.animator.SetBool("isFalling", PlayerController.onGround);
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
