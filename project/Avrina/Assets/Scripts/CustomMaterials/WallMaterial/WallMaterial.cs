using UnityEngine;

[System.Serializable]
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
    ///  Defines how much of the vertical force will be applied each update tick
    /// </summary>
    public float acceleration;
    /// <summary>
    ///  How fast can the player glide on this material
    /// </summary>
    public float maxGlideSpeed;
    /// <summary>
    ///  Enables or disables jumping for this material
    /// </summary>
    public bool canBeJumpedFrom;
    /// <summary>
    ///  Is the start velocity in the horizontal direction
    /// </summary>
    public float startVelocityX;
    /// <summary>
    ///  Is the start velocity in the vertical direction
    /// </summary>
    public float startVelocityY;
    /// <summary>
    ///  Can be climed on
    /// </summary>
    public bool canBeClimedOn;
}
