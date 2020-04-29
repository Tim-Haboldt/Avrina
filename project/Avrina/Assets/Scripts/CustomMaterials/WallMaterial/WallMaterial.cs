using UnityEngine;

[System.Serializable]
public class WallMaterial : ScriptableObject
{
    /// <summary>
    ///  Is the friction variable defined and used
    /// </summary>
    public bool isFrictionEnabled;
    /// <summary>
    ///  Defines the friction of the material
    /// </summary>
    public float friction;
    /// <summary>
    ///  Is the force variable defined and used
    /// </summary>
    public bool isForceEnabled;
    /// <summary>
    ///  Defines how much of the horizontal force will be applied each update tick
    /// </summary>
    public float force;
    /// <summary>
    ///  Enables or disables walljumps for this material
    /// </summary>
    public bool canBeWalljumpedFrom;
}
