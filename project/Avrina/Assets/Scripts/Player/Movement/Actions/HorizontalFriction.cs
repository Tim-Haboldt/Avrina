using UnityEngine;

public class HorizontalFriction : Action
{
    /// <summary>
    ///  What is the friction of the player in the air
    /// </summary>
    private float airFriction;
    /// <summary>
    ///  Default friction of the ground if no other was set
    /// </summary>
    private float defaultGroundFriction;

    /**
     * <summary>
     *  Applies friction to the player only if the player is currently not moving.
     * </summary>
     */ 
    public void PerformAction(ref Vector2 velocity)
    {
        if (PlayerController.movementInput == 0 && velocity.x != 0)
        {
            var movementDir = Mathf.Sign(velocity.x);

            var frictionMuliplier = 1f;
            if (PlayerController.onGround)
            {
                frictionMuliplier *= 1f; // Ground material friction
            } else
            {
                frictionMuliplier *= this.airFriction;
            }

            velocity = new Vector2(velocity.x - frictionMuliplier * movementDir, velocity.y);
        }
    }

    /**
     * <summary>
     *  Get the base friction from the player config and stores it at the start of the game
     * </summary>
     */ 
    public void Setup(PlayerConfig config)
    {
        this.airFriction = config.airFriction;
        this.defaultGroundFriction = config.defaultGroundFriction;
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
