using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerJump : MonoBehaviour
{
    // Defines how long the Jump button can be pressed
    public float maxJumpDuration;
    // How much foce will be applied when the jump button is pressed first
    public float jumpForce;
    // How many jumps does the player has
    public int maxNumberOfJumps;
    // Defines the distance between the player and the ground
    // until the player will be marked as on ground
    public float distanceToGround;

    // The variable is true if the jump button is pressed for the first time
    // in the current update go through
    private bool firstUpdateSincePress;
    // Passed time since the jump button was pressed first
    private float passedTimeSinceFirstPress;
    // How often did the player jump since he last touched the ground
    private int jumpCounter;

    private PlayerInput inputs;
    private Rigidbody2D rb;

    void Start()
    {
        this.inputs = GetComponent<PlayerInput>();
        this.rb = GetComponent<Rigidbody2D>();

        this.passedTimeSinceFirstPress = 0f;
        this.jumpCounter = 0;
    }
    
    void Update()
    {
        // Is the player currently pressing the jump button
        if (this.inputs.jumpInput && this.jumpCounter <= this.maxNumberOfJumps)
        {
            // PassedTimeSinceFirstPress is zero if it is the first update
            // since the player is pressing the jump button
            if (this.passedTimeSinceFirstPress == 0f)
            {
                this.firstUpdateSincePress = false;
                this.jumpCounter++;

                if (this.jumpCounter > this.maxNumberOfJumps)
                    return;
            }
            
            // Only increase the timer if the jump button is pressed for more then one update go through
            if (!this.firstUpdateSincePress)
                this.passedTimeSinceFirstPress += Time.deltaTime;

            // Do not apply any forces if the player is pressing the jump button for to long
            if (this.passedTimeSinceFirstPress <= this.maxJumpDuration)
            {
                this.rb.AddForce(new Vector2(0, jumpForce));
            }
        } else
        {
            this.firstUpdateSincePress = true;
            this.passedTimeSinceFirstPress = 0f;
            if (this.IsGrounded())
            {
                this.jumpCounter = 0;
            }
        }
    }

    /**
     * Checks if the player is currently touching the surface
     */ 
    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, this.distanceToGround, LayerMask.GetMask("Ground"));
    }
}
