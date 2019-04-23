using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerJump : MonoBehaviour
{
    // Defines how long the Jump button can be pressed
    [SerializeField] public float maxJumpDuration;
    // How much foce will be applied when the jump button is pressed first
    [SerializeField] public float jumpForce;
    // How many jumps does the player has
    [SerializeField] public int maxNumberOfJumps;
    // the fall of the player should be a lot faster then the actual jump
    [SerializeField] public float fallMultiplier;
    // Size of the ground line which is used to define, when the player is touching the ground
    [SerializeField] public float groundWidth;
    // Defines where the bottom of the player is for the on ground check
    [SerializeField] public Vector2 groundPosition;
    // Defines which layers define ground
    [SerializeField] public LayerMask groundMask;

    // The variable is true if the jump button is pressed for the first time
    // in the current update go through
    private bool firstUpdateSincePress;
    // Passed time since the jump button was pressed first
    private float passedTimeSinceFirstPress;
    // How often did the player jump since he last touched the ground
    private int jumpCounter;

    // Used to get the Information which player input is given
    private PlayerInput inputs;
    // Rigidbody of the player used to apply forces
    private Rigidbody2D rb;

    private void Start()
    {
        this.inputs = GetComponent<PlayerInput>();
        this.rb = GetComponent<Rigidbody2D>();

        this.passedTimeSinceFirstPress = 0f;
        this.jumpCounter = 0;
    }

    /**
     * Applies the forces to the rigidbody 
     */
    private void FixedUpdate()
    {
        // Increase the fall speed
        if (rb.velocity.y < 0)
        {
        }
        // Is the player currently pressing the jump button and has he any jumpbs left
        if (this.inputs.jumpInput && this.jumpCounter <= this.maxNumberOfJumps)
        {
            // PassedTimeSinceFirstPress is zero if it is the first update
            // since the player is pressing the jump button
            if (this.passedTimeSinceFirstPress == 0f)
            {
                this.firstUpdateSincePress = false;
                this.jumpCounter++;

                // If the player is pressing the jump button without any jumps left
                if (this.jumpCounter > this.maxNumberOfJumps)
                {
                    // increase the gravity scale
                    this.IncreaseGravity();
                    return;
                }
            }

            // Only increase the timer if the jump button is pressed for more then one update go through
            if (!this.firstUpdateSincePress)
                this.passedTimeSinceFirstPress += Time.deltaTime;

            // Do not apply any forces if the player is pressing the jump button for to long
            if (this.passedTimeSinceFirstPress <= this.maxJumpDuration)
            {
                // Calculate the relative force the object gets this update tick
                // The force given to the player should reduce in relation to the passed time
                float jumpForceMuli = (this.maxJumpDuration - this.passedTimeSinceFirstPress) / this.maxJumpDuration;
                // Add the force
                rb.velocity += new Vector2(0, this.jumpForce * jumpForceMuli);
            } else
            {
                // If the player is pressing jump for to long increase the gravity scale
                this.IncreaseGravity();
            }
        }
        else
        {
            // Reset time since last jump
            this.firstUpdateSincePress = true;
            this.passedTimeSinceFirstPress = 0f;
            // If the player is not pressing jump increase the gravity scale
            this.IncreaseGravity();
            // If the player is touching the ground set the jump counter to zero
            if (this.IsGrounded())
            {
                this.jumpCounter = 0;
            }
        }
    }

    /**
     * Applies more gravitiy if the player is not pressing the jump button
     */
    private void IncreaseGravity()
    {
        rb.velocity += Vector2.up * Physics.gravity.y * (this.fallMultiplier - 1) * Time.deltaTime;
    } 

    /**
     * Checks if the player is currently touching the surface
     */
    private bool IsGrounded()
    {
        Vector2 playerPos = this.transform.position;
        Vector2 relativeGroundPos = new Vector2(playerPos.x + this.groundPosition.x, playerPos.y + this.groundPosition.y);
             
        Vector2 leftPoint = new Vector2(relativeGroundPos.x - this.groundWidth / 2, relativeGroundPos.y);
        Vector2 rightPoint = new Vector2(relativeGroundPos.x + this.groundWidth / 2, relativeGroundPos.y);

        return Physics2D.OverlapArea(leftPoint, rightPoint, this.groundMask);
    }
}
