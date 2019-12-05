using UnityEngine;

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
    ///  How much force will be added each update tick
    /// </summary>
    public float verticalForce;
    /// <summary>
    ///  Maximal movement velocity
    /// </summary>
    public float maxVerticalMovement;
}
