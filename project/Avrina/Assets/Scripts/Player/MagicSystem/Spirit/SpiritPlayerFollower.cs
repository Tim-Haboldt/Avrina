using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class SpiritPlayerFollower : NetworkBehaviour
{
    private enum FollowState
    {
        Staying,
        Following
    }

    [Header("Movement")]
    /// <summary>
    ///  Where is the default rest position of the spirit.
    ///  The white spirit will be one the oposit site of the black one
    /// </summary>
    [SerializeField] private Vector3 restPositionOffset = Vector3.zero;
    /// <summary>
    ///  How far away of the center can the spirit be before he starts following again
    /// </summary>
    [SerializeField] private float maxRadiusBeforeStartFollowing = 0f;
    /// <summary>
    ///  How much force will be added in order to catch up to the player
    /// </summary>
    [SerializeField] private float force = 0f;
    /// <summary>
    ///  How fast can the spirit move
    /// </summary>
    [SerializeField] private float maxMovementSpeed = 0f;
    /// <summary>
    ///  Slowest speed of the spirit before its finished slowing down
    /// </summary>
    [SerializeField] private float minimalMovementSpeed = 0f;
    /// <summary>
    ///  How much will the player slow down each update frame on the rest position
    /// </summary>
    [SerializeField] private float slowndownFactor = 0f;

    /// <summary>
    ///  Reference to the rigidbody
    /// </summary>
    private Rigidbody2D rb;
    /// <summary>
    ///  Reference to the state manager
    /// </summary>
    private SpiritStateManager stateManager = null;
    /// <summary>
    ///  Reference to its player object
    /// </summary>
    public PlayerStateManager player { private set; get; }
    /// <summary>
    ///  Current state of the follow manager
    /// </summary>
    [SyncVar]
    private FollowState state;


    /// <summary>
    ///  Will search for the rigidbody inside the spirit object
    /// </summary>
    private void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.state = FollowState.Staying;
    }

    /// <summary>
    ///  Checks if the spirit reached its gloal
    /// </summary>
    private void Update()
    {
        if (!this.hasAuthority || this.player == null)
        {
            return;
        }

        switch (this.state)
        {
            case FollowState.Staying:
                if (!this.IsSpiritInsideRestRadius(this.transform.position))
                {
                    this.CmdUpdateState(FollowState.Following);
                }
                break;
            case FollowState.Following:
                if (this.IsSpiritInsideRestRadius(this.transform.position) && this.rb.velocity.magnitude < this.minimalMovementSpeed)
                {
                    this.CmdUpdateState(FollowState.Staying);
                }
                break;
        }
    }

    /// <summary>
    ///  Is the spirit close enough to the player that is does not need to follow the player anymore
    /// </summary>
    /// <returns></returns>
    private bool IsSpiritInsideRestRadius(Vector3 spiritPosition)
    {
        var playerPosition = this.player.transform.position;
        var expctedSpiritPosition = playerPosition + this.restPositionOffset;
        Debug.DrawLine(expctedSpiritPosition - new Vector3(this.maxRadiusBeforeStartFollowing, 0), expctedSpiritPosition + new Vector3(this.maxRadiusBeforeStartFollowing, 0));
        Debug.DrawLine(expctedSpiritPosition - new Vector3(0, this.maxRadiusBeforeStartFollowing), expctedSpiritPosition + new Vector3(0, this.maxRadiusBeforeStartFollowing));

        return Vector2.Distance(spiritPosition, expctedSpiritPosition) <= this.maxRadiusBeforeStartFollowing;
    }

    /// <summary>
    ///  Will the spirit be inside the rest area next frame
    /// </summary>
    /// <returns></returns>
    private bool IsSpiritInsideRestRadiusNextFrame(Vector2 velocity)
    {
        var currentPos = this.transform.position;
        var currentPosAsV2 = new Vector2(currentPos.x, currentPos.y);
        var nextPosition = currentPosAsV2 + velocity;

        return this.IsSpiritInsideRestRadius(nextPosition);
    }

    /// <summary>
    ///  Will move the spirit to the player
    /// </summary>
    private void FixedUpdate()
    {
        if (!this.hasAuthority)
        {
            return;
        }

        switch (this.state)
        {
            case FollowState.Staying:
                this.SlowlyMoveAroundRestposition();
                break;
            case FollowState.Following:
                this.FollowPlayer();
                break;
        }
    }

    /// <summary>
    ///  Moves the spirit slowly around the restposition
    /// </summary>
    private void SlowlyMoveAroundRestposition()
    {
        if (this.IsSpiritInsideRestRadiusNextFrame(this.rb.velocity))
        {
            // Add randomness to the movement
            var randomAngle = Random.insideUnitCircle;
            var randomSpeed = Random.Range(0, 0.3f);
            this.rb.velocity += randomAngle * 0.5f;
        }

        // Make the movement slower each time
        var currentMovementSpeed = this.rb.velocity.magnitude;
        var slowedSpeed = currentMovementSpeed * (1 - this.slowndownFactor);
        this.rb.velocity = this.rb.velocity.normalized * slowedSpeed;
    }

    /// <summary>
    ///  Followes the player around
    /// </summary>
    private void FollowPlayer()
    {
        var playerPos = this.player.transform.position;
        var expectedSpiritPosition = playerPos + this.restPositionOffset;
        
        var nextPosition = Vector3.MoveTowards(this.transform.position, expectedSpiritPosition, this.force);
        var velocity = nextPosition - this.transform.position;

        var nextVelocity = this.rb.velocity;
        if (this.IsSpiritInsideRestRadiusNextFrame(nextVelocity))
        {
            nextVelocity -= new Vector2(velocity.x, velocity.y);
        }
        else
        {
            nextVelocity += new Vector2(velocity.x, velocity.y);
        }

        if (nextVelocity.magnitude >= this.maxMovementSpeed)
        {
            nextVelocity = nextVelocity.normalized * this.maxMovementSpeed;
        }

        this.rb.velocity = nextVelocity;
    }
    
    /// <summary>
    ///  Updates the follow state of the spirit
    /// </summary>
    /// <param name="state"></param>
    [Command]
    private void CmdUpdateState(FollowState state)
    {
        this.state = state;
    }

    /// <summary>
    ///  Initializes the spirit follow manager
    /// </summary>
    /// <param name="stateManager"></param>
    public void Init(SpiritStateManager stateManager, PlayerStateManager player)
    {
        this.stateManager = stateManager;
        this.player = player;

        if (this.stateManager.isWhiteSpirit)
        {
            this.restPositionOffset.x *= -1;
        }

        this.transform.position = this.player.transform.position + this.restPositionOffset;
    }
}
