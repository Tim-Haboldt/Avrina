using UnityEngine;

public class WallJumping : StateInheritingAction
{
    /// <summary>
    ///  How much velocity will be added each update tick on the vertical axis
    /// </summary>
    private float wallJumpVelocityY;
    /// <summary>
    ///  How much velocity will be added each update tick on the horizontal axis
    /// </summary>
    private float wallJumpVelocityX;
    /// <summary>
    ///  How long does the walljump have to at least take
    /// </summary>
    private float wallJumpMinDuration;
    /// <summary>
    ///  How long does the walljump can last
    /// </summary>
    private float wallJumpMaxDuration;
    /// <summary>
    ///  Start time of the air jump
    /// </summary>
    private float startTime;
    /// <summary>
    ///  Sets the direction of the jump
    /// </summary>
    private WallslidingDirection jumpDirection;
    /// <summary>
    ///  The name of the state is Walljumping
    /// </summary>
    public override PlayerState name { get; } = PlayerState.WallJumping;
    /// <summary>
    ///  Defines all actions that can occour in the walljumping state.
    /// </summary>
    protected override Action[] actions { get; } = new Action[] 
    {
        new HorizontalMovement(),
    };


    /// <summary>
    ///  Changes to the state immobile if the jump is finished
    /// </summary>
    /// <returns></returns>
    public override PlayerState Update()
    {
        var passedTime = Time.time - this.startTime;

        if ((passedTime >= this.wallJumpMinDuration && !this.playerController.jumpInput) || passedTime >= this.wallJumpMaxDuration)
        {
            return PlayerState.Immobile;
        }

        return this.name;
    }

    /// <summary>
    ///  Sets the movement speed and direction of the player
    /// </summary>
    /// <param name="velocity"></param>
    protected override void PerformAction(ref Vector2 velocity)
    {
        var direction = 1f;
        if (this.jumpDirection == WallslidingDirection.Right)
        {
            direction = -1f;
        }

        velocity = new Vector2(this.wallJumpVelocityX * direction, this.wallJumpVelocityY);
    }

    /// <summary>
    ///  Reads some values from the config and stores them locally
    /// </summary>
    /// <param name="config"></param>
    protected override void Setup(PlayerConfig config)
    {
        this.wallJumpVelocityX = config.wallJumpVelocityX;
        this.wallJumpVelocityY = config.wallJumpVelocityY;
        this.wallJumpMinDuration = config.wallJumpMinDuration;
        this.wallJumpMaxDuration = config.wallJumpMaxDuration;
    }

    /// <summary>
    ///  Sets the start time of the jump
    /// </summary>
    protected override void OnEnter()
    {
        this.startTime = Time.time;
        
        if (this.playerController.movementInput > 0)
        {
            this.jumpDirection = WallslidingDirection.Right;
        } else
        {
            this.jumpDirection = WallslidingDirection.Left;
        }
    }

    /// <summary>
    ///  Unused
    /// </summary>
    protected override void OnExit() {}
}
