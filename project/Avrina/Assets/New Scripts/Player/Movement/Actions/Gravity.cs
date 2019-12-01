using UnityEngine;

public class Gravity : Action
{
    /// <summary>
    ///  Gravity const for the gravity calculation
    /// </summary>
    public float gravity = 1f;
    /// <summary>
    ///  What is the gravity velocity
    /// </summary>
    public float maxGravityVelocitry = 5f;
    /// <summary>
    ///  Current velocity
    /// </summary>
    private float currentVelocity = 0f;


    /**
     * <summary>
     *  Applys gravity to the player
     * </summary>
     * <param name="velocity">The gravity is applyed to</param>
     */ 
    public void PerformAction(ref Vector2 velocity)
    {
        this.currentVelocity += this.gravity;
        velocity.y -= this.currentVelocity * this.gravity;
        if (velocity.y < this.maxGravityVelocitry * -1)
        {
            velocity.y = this.maxGravityVelocitry * -1;
        }
    }

    /**
     * <summary>
     *  Resets the current velocity of the player
     * </summary>
     */
    public void OnExit()
    {
        this.currentVelocity = 0f;
    }

    /**
     * <summary>
     *  Reads all constants from the config and stores them inside the class
     * </summary>
     */
    public void Setup(PlayerConfig config)
    {
        this.gravity = config.gravity;
        this.maxGravityVelocitry = config.maxGravityVelocitry;
    }

    /**
     * <summary>
     *  Resets the current velocity of the player
     * </summary>
     */ 
    public void OnEnter()
    {
        this.currentVelocity = 0f;
    }
}
