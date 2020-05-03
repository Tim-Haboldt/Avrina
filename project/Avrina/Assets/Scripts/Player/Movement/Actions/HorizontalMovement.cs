using UnityEngine;

public class HorizontalMovement : Action
{
    /// <summary>
    ///  How much force will be added each update tick while the player is in air
    /// </summary>
    private float accelerationInAir;
    /// <summary>
    ///  How much friction will occour in the air while no input is given
    /// </summary>
    private float airFrictionWhileNoInputGiven;
    /// <summary>
    ///  How much friction will occour in the air while moving
    /// </summary>
    private float airFrictionWhileMoving;
    /// <summary>
    ///  How much friction will occour in the air while turning
    /// </summary>
    private float airFrictionWhileTurning;

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
        if (playerController.onGround)
        {
            velocity = this.GroundMovement(velocity, playerController);
        }
        else
        {
            velocity = this.AirMovement(velocity, playerController);
        }
    }

    /// <summary>
    ///  Handels the movement while in air
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    private Vector2 AirMovement(Vector2 velocity, PlayerController playerController)
    {
        var movementInput = playerController.movementInput;

        if (movementInput == 0)
        {
            velocity.x -= this.airFrictionWhileNoInputGiven * velocity.x;
        }
        else if (Mathf.Sign(movementInput) == Mathf.Sign(velocity.x))
        {
            velocity.x += movementInput * this.accelerationInAir - this.airFrictionWhileMoving * velocity.x;
        }
        else
        {
            velocity.x += movementInput * this.accelerationInAir - this.airFrictionWhileTurning * velocity.x;
        }

        return new Vector2(velocity.x, velocity.y);
    }

    /// <summary>
    ///  Handels the movement while on ground
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    private Vector2 GroundMovement(Vector2 velocity, PlayerController playerController)
    {
        var movementInput = playerController.movementInput;
        var groundMaterial = playerController.groundMaterial;

        if (movementInput == 0) {
            velocity.x -= groundMaterial.frictionWhileNoInputGiven * velocity.x;

            if (Mathf.Abs(velocity.x) < groundMaterial.smallestMovementBeforeStop)
            {
                velocity.x = 0;
            }
        }
        else if (Mathf.Sign(movementInput) == Mathf.Sign(velocity.x))
        {
            velocity.x += movementInput * groundMaterial.acceleration - groundMaterial.frictionWhileMoving * velocity.x;
        }
        else
        {
            velocity.x += movementInput * groundMaterial.acceleration - groundMaterial.frictionWhileTurning * velocity.x;
        }

        return new Vector2(velocity.x, velocity.y);
    }

    /**
     * <summary>
     *  Reads the constants from the config
     * </summary>
     */
    public void Setup(PlayerConfig config)
    {
        this.accelerationInAir = config.horizontalAccelerationInAir;
        this.airFrictionWhileNoInputGiven = config.airFrictionWhileNoInputGiven;
        this.airFrictionWhileMoving = config.airFrictionWhileMoving;
        this.airFrictionWhileTurning = config.airFrictionWhileTurning;
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
