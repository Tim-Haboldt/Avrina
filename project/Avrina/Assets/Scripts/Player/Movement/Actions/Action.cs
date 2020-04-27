using UnityEngine;

public interface Action
{
    /**
     * <summary>
     *  Will be called at the start of the game in order to setup an action
     * </summary>
     * <param name="config">Contains all constant variables</param>
     */
    void Setup(PlayerConfig config);

    /**
     * <summary>
     *  Used to initialize the action class.
     *  Will be called on entering a state.
     * </summary>
     */
    void OnEnter();

    /**
     * <summary>
     *  Will execute the action.
     * </summary>
     * <example>
     *  Apply gravity to player
     * </example>
     * <param name="velocity">Used to modify the velocity of the player</param>
     */ 
    void PerformAction(ref Vector2 velocity);

    /**
     * <summary>
     *  Used to reset the action class.
     *  Will be called at the end of a state.
     * </summary>
     */ 
    void OnExit();
}
