using UnityEngine;

public class VerticalFriction : Action
{
    /// <summary>
    ///  Base friction of the player.
    ///  All friction calculation is based on this friction.
    ///  Material friction modifies the base friction.
    /// </summary>
    private float friction;

    /**
     * <summary>
     *  Applies friction to the player only if the player is currently not moving.
     * </summary>
     */ 
    public void PerformAction(ref Vector2 velocity)
    {
        if (PlayerController.movementInput.x == 0 && velocity.x != 0)
        {
            var movementDir = Mathf.Sign(velocity.x);

            var frictionMuliplier = this.friction;
            if (PlayerController.onGround)
            {
                frictionMuliplier *= 1f; // Ground material friction
            } else
            {
                frictionMuliplier *= 0.4f; // Air material friction
            }

            var fixedFriction = friction;
            var velocityRelatedFriction = frciton;

            //TODO move perform action into normal update loop and add time.deltaTime
        }
    }

    /**
     * <summary>
     *  Get the base friction from the player config and stores it at the start of the game
     * </summary>
     */ 
    public void Setup(PlayerConfig config)
    {
        this.friction = config.verticalFriction;
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
