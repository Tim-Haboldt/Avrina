using UnityEngine;

public class OnGround : StateInheritingAction
{
    /// <summary>
    ///  If the player was jumping before the OnGround state the Jumping state will not be triggered until the player is not pressing jump
    /// </summary>
    private bool holdingJump = false;
    /// <summary>
    ///  What is the mask of the ground objects
    /// </summary>
    private LayerMask groundMask;
    /// <summary>
    ///  Stores the last ground position before the player left the ground.
    /// </summary>
    private Vector2 lastGroundPosition;

    ///<summary>
    /// Name of the state is onGround
    ///</summary>
    public override PlayerState name { get; } = PlayerState.OnGround;
    /// <summary>
    ///  The player can move horizontally
    /// </summary>
    protected override Action[] actions { get; } = new Action[]
    {
        new HorizontalGroundMovement(),
    };


    /// <summary>
    ///  Reads some values from the config
    /// </summary>
    /// <param name="config"></param>
    protected override void Setup(PlayerConfig config)
    {
        this.groundMask = config.groundMask;
    }

    /// <summary>
    ///  Unused for now
    /// </summary>
    /// <param name="velocity"></param>
    protected override void PerformAction(ref Vector2 velocity) {}

    /// <summary>
    ///  Returns the Jumping state if the jump input is pressed.
    ///  Returns the InAir state if the player is not touching the ground.
    /// </summary>
    /// <returns></returns>
    public override PlayerState Update()
    {
        var movementInput = this.playerController.movementInput;

        if (this.playerController.jumpInput && !this.holdingJump && this.playerController.groundMaterial.canBeJumpedFrom)
        {
            return PlayerState.Jumping;
        }
        else if (!this.playerController.onGround)
        {
            Vector2 groundPos;
            if (this.getCurrentGroundPosition(out groundPos))
            {
                var distance = Vector2.Distance(this.lastGroundPosition, groundPos);
                Debug.Log(distance);
                if (distance < 1)
                {
                    this.lastGroundPosition = groundPos;

                    return this.name;
                }
            }

            return PlayerState.InAir;
        }
        else if ((movementInput > 0 && this.playerController.hasWallRight
            || movementInput < 0 && this.playerController.hasWallLeft)
            && this.playerController.wallMaterial.canBeClimedOn)
        {
            return PlayerState.WallSliding;
        }

        if (this.holdingJump && !this.playerController.jumpInput)
        {
            this.holdingJump = false;
        }

        return this.name;
    }

    /// <summary>
    ///  Gets the current ground position
    /// </summary>
    /// <returns></returns>
    private bool getCurrentGroundPosition(out Vector2 groundPos)
    {
        var capsuleCollider = this.playerController.transform.gameObject.GetComponent<CapsuleCollider2D>();
        var position = capsuleCollider.bounds.center;
        var halfSizeY = capsuleCollider.bounds.size.y * 0.5f;

        var playerBottom = new Vector2(position.x, position.y - halfSizeY);

        RaycastHit2D hit = Physics2D.Raycast(playerBottom, Vector2.down, 5, this.groundMask);
        if (hit.collider != null)
        {
            groundPos = hit.point;
            return true;
        }

        groundPos = new Vector2(0, 0);
        return false;
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
