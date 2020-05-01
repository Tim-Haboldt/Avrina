using UnityEngine;

public class HorizontalMovement : Action
{
    /// <summary>
    ///  How much force will be added each update tick
    /// </summary>
    private float defaultForceOnGround;
    /// <summary>
    ///  How much force will be added each update tick while the player is in air
    /// </summary>
    private float forceInAir;
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
    public void PerformAction(ref Vector2 velocity, PlayerController playerController)
    {
        var input = playerController.movementInput;
        // Only apply new forces if the player presses any key
        if (input != 0)
        {
            // Store the old movement speed with the direction and the absolute speed
            var currentMovementDir = Mathf.Sign(velocity.x);
            var oldMovementSpeed = velocity.x;
            var newMovementSpeed = velocity.x;
            var absOldMovementSpeed = Mathf.Abs(oldMovementSpeed);

            // Take a different velocity mulitplier corresponding to the player is on ground or in the air
            if (playerController.onGround)
            {
                float force = input;
                if (playerController.groundMaterial.isForceEnabled)
                {
                    force *= playerController.groundMaterial.force;
                } else
                {
                    force *= this.defaultForceOnGround;
                }
                newMovementSpeed += force;
            }
            else
            {
                newMovementSpeed += this.forceInAir * input;
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
        this.defaultForceOnGround = config.defaultHorizontalForceOnGround;
        this.forceInAir = config.horizontalForceInAir;
        this.maxVelocity = config.maxHorizontalMovement;
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
