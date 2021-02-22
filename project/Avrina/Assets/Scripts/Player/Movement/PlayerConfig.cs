using UnityEngine;

[System.Serializable]
public struct PlayerConfig
{
    /// <summary>
    ///  Gravity const for the gravity calculation
    /// </summary>
    public float gravity;
    /// <summary>
    ///  How much fast can the player fall during the float animation
    /// </summary>
    public float maxGravityWhileFloating;
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
    /// <summary>
    ///  How far can the slope go down per player movement to still act as if the player is on ground
    /// </summary>
    public float maxGroundDistanceForSlopes;
    /// <summary>
    ///  Used to determin the airjump sound
    /// </summary>
    public AudioClip jumpSound;
}
