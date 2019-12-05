using UnityEngine;

public class VerticalMovement : Action
{
    /// <summary>
    ///  How much force will be added for each update tick
    /// </summary>
    private float force;
    /// <summary>
    ///  What is the maximal velocity the player can have
    /// </summary>
    private float maxVelocity;

    /**
     * <summary>
     *  Will execute the action.
     * </summary>
     * <example>
     *  Apply gravity to player
     * </example>
     * <param name="velocity">Used to modify the velocity of the player</param>
     */
    public void PerformAction(ref Vector2 velocity)
    {
        var input = PlayerController.movementInput.x;
        // Only apply new forces if the player presses any key
        if (input != 0)
        {
            // Store the old movement speed with the direction and the absolute speed
            var currentMovementDir = Mathf.Sign(velocity.x);
            var oldMovementSpeed = velocity.x;
            var newMovementSpeed = velocity.x;
            var absOldMovementSpeed = Mathf.Abs(oldMovementSpeed);

            // Take a different velocity mulitplier corresponding to the player is on ground or in the air
            if (PlayerController.onGround)
            {
                newMovementSpeed += this.force * input; // * groundFriction(MaterialFriction)
            }
            else
            {
                newMovementSpeed += this.force * input; // * airFriction(GasFriction)
            }

            var absNewMovementSpeed = Mathf.Abs(newMovementSpeed);
            if (absOldMovementSpeed > this.maxVelocity)
            {
                // Only allow forces that reduce force if the force is over max already
                if (absOldMovementSpeed < absNewMovementSpeed)
                {
                    newMovementSpeed = oldMovementSpeed;
                }
            }
            else if (absNewMovementSpeed > this.maxVelocity)
            {
                newMovementSpeed = this.maxVelocity * currentMovementDir;
            }

            // Apply new velocity to the actual player object
            velocity = new Vector2(newMovementSpeed, velocity.y);
        }
    }

    /**
     * <summary>
     *  Reads the constants from the config
     * </summary>
     */
    public void Setup(PlayerConfig config)
    {
        this.force = config.verticalForce;
        this.maxVelocity = config.maxVerticalMovement;
    }

    /**
     * <summary>
     *  Unused
     * </summary>
     */
    public void OnEnter() { }

    /**
     * <summary>
     *  Unused
     * </summary>
     */
    public void OnExit() { }
}
