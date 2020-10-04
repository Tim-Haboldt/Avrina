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
    /// <summary>
    ///  How much gravity will be added while the player is floating
    /// </summary>
    private float maxGravityVelocitryWhileFloating = 5f;


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
        if (inputController.jumpInput && velocity.y < this.maxGravityVelocitryWhileFloating * -1)
        {
            velocity.y = this.maxGravityVelocitryWhileFloating * -1;
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
        this.maxGravityVelocitryWhileFloating = config.maxGravityWhileFloating;
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
