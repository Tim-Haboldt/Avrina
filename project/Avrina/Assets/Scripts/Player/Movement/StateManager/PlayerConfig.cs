[System.Serializable]
public struct PlayerConfig
{
    /// <summary>
    ///  Gravity const for the gravity calculation
    /// </summary>
    public float gravity;
    /// <summary>
    ///  What is the maximal gravity velocity
    /// </summary>
    public float maxGravityVelocitry;
    /// <summary>
    ///  How much force will be added to the horizontal movement speed each update tick if the ground has not specified something else
    /// </summary>
    public float defaultHorizontalForceOnGround;
    /// <summary>
    ///  How much force will be added to the horizontal movement each update tick while in air
    /// </summary>
    public float horizontalForceInAir;
    /// <summary>
    ///  Maximal movement velocity
    /// </summary>
    public float maxHorizontalMovement;
    /// <summary>
    ///  If the grond object has no friction this one will be used
    /// </summary>
    public float defaultGroundFriction;
    /// <summary>
    ///  What is the friction of the player in the air
    /// </summary>
    public float airFriction;
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
    ///  Will be added each update tick while sliding down a the wall
    /// </summary>
    public float defaultWallslidingForce;
    /// <summary>
    ///  Maximal speed the player can slide down a wall
    /// </summary>
    public float maxWallslidingSpeed;
    /// <summary>
    ///  How much velocity will be added each update tick on the vertical axis
    /// </summary>
    public float wallJumpVelocityY;
    /// <summary>
    ///  How much velocity will be added each update tick on the horizontal axis
    /// </summary>
    public float wallJumpVelocityX;
    /// <summary>
    ///  How long does the walljump have to at least take
    /// </summary>
    public float wallJumpMinDuration;
    /// <summary>
    ///  How long does the walljump can last
    /// </summary>
    public float wallJumpMaxDuration;
}
