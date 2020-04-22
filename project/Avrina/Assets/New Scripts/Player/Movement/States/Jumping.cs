using UnityEngine;

public class Jumping : StateInheritingAction
{
    /// <summary>
    ///  Start time of the jump
    /// </summary>
    private float startTime;
    /// <summary>
    ///  What is the minimal jump duration
    /// </summary>
    public float minJumpDuration;
    /// <summary>
    ///  What is the maximal jump duration
    /// </summary>
    public float maxJumpDuration;
    /// <summary>
    ///  Is the velocity during the player jumping
    /// </summary>
    public float jumpVelocity;

    /// <summary>
    ///  Name of the state is Jumping
    /// </summary>
    public override PlayerState name { get; } = PlayerState.Jumping;
    /// <summary>
    ///  The player can jump and move vertical while jumping
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new VerticalMovement(),
    };

    /// <summary>
    ///  Caluclates the current jump velocity
    /// </summary>
    /// <param name="velocity"></param>
    protected override void PerformAction(ref Vector2 velocity)
    {
        velocity = new Vector2(velocity.x, this.jumpVelocity);
    }

    /// <summary>
    ///  Keeps returning the jump state until the jump button is not pressed or the maximal jumpduration was exceeded
    /// </summary>
    /// <returns>Returns the State InAir or Jumping</returns>
    public override PlayerState Update()
    {
        var passedTime = Time.time - this.startTime;
        var jumpInput = PlayerController.jumpInput;

        if ((passedTime >= this.minJumpDuration && !jumpInput) || passedTime >= this.maxJumpDuration)
        {
            return PlayerState.InAir;
        }

        return this.name;
    }

    /**
    * <summary>
    *  Reads the constants from the config
    * </summary>
    */
    protected override void Setup(PlayerConfig config)
    {
        this.minJumpDuration = config.minJumpDuration;
        this.maxJumpDuration = config.maxJumpDuration;
        this.jumpVelocity = config.jumpVelocity;
    }

    /**
     * <summary>
     *  Sets the start time of the jump
     * </summary>
     */
    protected override void OnEnter()
    {
        this.startTime = Time.time;
    }

    /**
     * <summary>
     *  Unused
     * </summary>
     */
    protected override void OnExit() { }
}
