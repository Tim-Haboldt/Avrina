using UnityEngine;

public class Gravity : Action
{
    /// <summary>
    ///  Gravity const for the gravity calculation
    /// </summary>
    private float gravity = 1f;
    /// <summary>
    ///  What is the gravity velocity
    /// </summary>
    private float maxGravityVelocitry = 5f;


    /**
     * <summary>
     *  Applys gravity to the player
     * </summary>
     * <param name="velocity">The gravity is applyed to</param>
     */ 
    public void PerformAction(ref Vector2 velocity, InputController inputController)
    {
        velocity.y -= this.gravity;
        if (velocity.y < this.maxGravityVelocitry * -1)
        {
            velocity.y = this.maxGravityVelocitry * -1;
        }
    }

    /**
     * <summary>
     *  Reads all constants from the config and stores them inside the class
     * </summary>
     */
    public void Setup(PlayerConfig config)
    {
        this.gravity = config.gravity;
        this.maxGravityVelocitry = config.maxGravityVelocity;
    }

    /**
     * <summary>
     *  Unused
     * </summary>
     */ 
    public void OnEnter() {}

    /**
     * <summary>
     *  Unused
     * </summary>
     */
    public void OnExit() {}
}
