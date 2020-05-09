using UnityEngine;

public class HorizontalAirMovement : Action
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
    public void PerformAction(ref Vector2 velocity, InputController inputController)
    {
        var movementInput = inputController.movementInput;
        var nextVelocityX = velocity.x;

        if (movementInput == 0)
        {
            nextVelocityX -= this.airFrictionWhileNoInputGiven * nextVelocityX;
        }
        else if (Mathf.Sign(movementInput) == Mathf.Sign(nextVelocityX))
        {
            nextVelocityX += movementInput * this.accelerationInAir - this.airFrictionWhileMoving * nextVelocityX;
        }
        else
        {
            nextVelocityX += movementInput * this.accelerationInAir - this.airFrictionWhileTurning * nextVelocityX;
        }

        velocity = new Vector2(nextVelocityX, velocity.y);
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
