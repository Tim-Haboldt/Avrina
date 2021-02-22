using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WallMaterial", menuName = "CustomMaterials/WallMaterial", order = 2)]
public class WallMaterial : ScriptableObject
{   
    /// <summary>
    ///  Sets the priority of the material
    /// </summary>
    public int priority;
    /// <summary>
    ///  Defines the friction of the material
    /// </summary>
    public float friction;
    /// <summary>
    ///  Enables or disables jumping for this material
    /// </summary>
    public bool canBeJumpedFrom;
    /// <summary>
    ///  Is the start velocity in the horizontal direction
    /// </summary>
    public float startJumpVelocityX;
    /// <summary>
    ///  Is the start velocity in the vertical direction
    /// </summary>
    public float startJumpVelocityY;
    /// <summary>
    ///  Can be climed on
    /// </summary>
    public bool canBeClimedOn;
    /// <summary>
    ///  What is the acceleration of the player object while climbing
    /// </summary>
    public float acceleration;
    /// <summary>
    ///  Changes the friction while the player is moving up
    /// </summary>
    public float frictionWhileMovingUp;
    /// <summary>
    ///  Changes the friction while the player is moving down
    /// </summary>
    public float frictionWhileMovingDown;

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
