using UnityEngine;

[System.Serializable]
public struct PlayerConfig
{
    /// <summary>
    ///  Gravity const for the gravity calculation
    /// </summary>
    public float gravity;
    /// <summary>
    ///  Lowers the gravity during the jump
    /// </summary>
    public float gravityDuringJump;
    /// <summary>
    ///  What is the maximal gravity velocity
    /// </summary>
    public float maxGravityVelocity;
    /// <summary>
    ///  What is the maximal jump duration
    /// </summary>
    public float maxJumpDuration;
    /// <summary>
    ///  What is the layer mask of the ground objects
    /// </summary>
    public LayerMask groundMask;
    /// <summary>
    ///  How much acceleration will be added to the horizontal movement each update tick while in air
    /// </summary>
    public float horizontalAccelerationInAir;
    /// <summary>
    ///  What is the friction of the player in the air while no input is given
    /// </summary>
    public float airFrictionWhileNoInputGiven;
    /// <summary>
    ///  What is the friction of the player in the air while moving
    /// </summary>
    public float airFrictionWhileMoving;
    /// <summary>
    ///  What is the friction of the player while turning around
    /// </summary>
    public float airFrictionWhileTurning;
    /// <summary>
    ///  Velocity on the start of the air jump
    /// </summary>
    public float airJumpStartVelocity;
}
