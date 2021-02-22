using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GroundMaterial", menuName = "CustomMaterials/GroundMaterial", order = 1)]
public class GroundMaterial: ScriptableObject
{
    /// <summary>
    ///  Sets the priority of the material
    /// </summary>
    public int priority;
    /// <summary>
    ///  Defines how much of the horizontal force will be applied each update tick
    /// </summary>
    public float acceleration;
    /// <summary>
    ///  What is the friction while the player is not giving any movement input
    /// </summary>
    public float frictionWhileNoInputGiven;
    /// <summary>
    ///  What is the friction while the player is moving
    /// </summary>
    public float frictionWhileMoving;
    /// <summary>
    ///  What is the frction while the player is turning
    /// </summary>
    public float frictionWhileTurning;
    /// <summary>
    ///  Smallest possible movement speed on the material
    /// </summary>
    public float smallestMovementBeforeStop;
    /// <summary>
    ///  Enables or disables jumping for this material
    /// </summary>
    public bool canBeJumpedFrom;
    /// <summary>
    ///  Is the start velocity of the jump
    /// </summary>
    public float jumpVelocity;

    /// <summary>
    ///  This sound will be played everytime the player jumps
    /// </summary>
    public List<AudioClip> jumpSounds;
    /// <summary>
    ///  This sound will be played everytime the player lands on ground
    /// </summary>
    public List<AudioClip> landingSounds;
    /// <summary>
    ///  This sound will be played everytime the player jumps
    /// </summary>
    public List<AudioClip> walkSounds;
}