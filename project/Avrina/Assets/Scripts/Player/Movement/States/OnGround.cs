using UnityEngine;

public class OnGround : StateInheritingAction
{
    /// <summary>
    ///  If the player was jumping before the OnGround state the Jumping state will not be triggered until the player is not pressing jump
    /// </summary>
    private bool holdingJump = false;
    /// <summary>
    ///  If the ground is farther away then the max distance the player will not be clipped on the ground
    /// </summary>
    private float maxGroundDistance;
    /// <summary>
    ///  What is the mask of the ground objects
    /// </summary>
    private LayerMask groundMask;

    ///<summary>
    /// Name of the state is onGround
    ///</summary>
    public override PlayerState name { get; } = PlayerState.OnGround;
    /// <summary>
    ///  The player can move horizontally
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new HorizontalMovement(),
    };


    /// <summary>
    ///  Reads some values from the config
    /// </summary>
    /// <param name="config"></param>
    protected override void Setup(PlayerConfig config)
    {
        this.groundMask = config.groundMask;
        this.maxGroundDistance = config.maxGroundDistance;
    }

    /// <summary>
    ///  Sets the player to the groun dall the time
    /// </summary>
    /// <param name="velocity"></param>
    protected override void PerformAction(ref Vector2 velocity)
    {
        velocity = new Vector2(velocity.x, 0);

        var playerPosition = this.rigidbody.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, -Vector2.up, this.maxGroundDistance, this.groundMask);

        if (hit.collider != null) {
            float distance = Mathf.Abs(hit.point.y - playerPosition.y);
            playerPosition.y -= distance;
        }
    }

    /// <summary>
    ///  Returns the Jumping state if the jump input is pressed.
    ///  Returns the InAir state if the player is not touching the ground.
    /// </summary>
    /// <returns></returns>
    public override PlayerState Update()
    {
        if (this.playerController.jumpInput && !this.holdingJump && this.playerController.groundMaterial.canBeJumpedFrom)
        {
            return PlayerState.Jumping;
        }
        else if (!this.playerController.onGround)
        {
            return PlayerState.InAir;
        }

        if (this.holdingJump && !this.playerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    /// <summary>
    ///  Sets the vertical velocity of the player to zero
    /// </summary>
    protected override void OnEnter()
    {
        if (this.playerController.jumpInput)
        {
            this.holdingJump = true;
        }

        this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, 0);
    }

    /// <summary>
    ///  Unused
    /// </summary>
    protected override void OnExit() {}
}
