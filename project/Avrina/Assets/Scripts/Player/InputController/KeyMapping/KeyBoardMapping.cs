using UnityEngine;

[System.Serializable]
public class KeyBoardMapping : KeyMapping
{
    /// <summary>
    ///  Defines the type of the key mapping
    /// </summary>
    public override MappingType type { get { return MappingType.KeyBoard; } }

    /// <summary>
    ///  What is the name of the horizontal movement input axis in the unity input controller
    /// </summary>
    public string horizontalMovement = "KeyBoardMoveHorizontal";
    /// <summary>
    ///  What is the name of the horizontal aim input axis in the unity input controller
    /// </summary>
    public string horizontalAim = "KeyBoardAimHorizontal";
    /// <summary>
    ///  What is the name of the vertical aim input axis in the unity input controller
    /// </summary>
    public string verticalAim = "KeyBoardAimVertical";
    /// <summary>
    ///  What is the key to lock and climb up
    /// </summary>
    public KeyCode lookUp = KeyCode.W;
    /// <summary>
    ///  What is the key to climb down and crouch
    /// </summary>
    public KeyCode duck = KeyCode.S;
    /// <summary>
    ///  What key opens the settings
    /// </summary>
    public KeyCode settings = KeyCode.Tab;
    /// <summary>
    ///  What key casts the spell
    /// </summary>
    public KeyCode cast = KeyCode.Q;
    /// <summary>
    ///  Resets the selected elements
    /// </summary>
    public KeyCode cancel = KeyCode.E;
    /// <summary>
    ///  What key can be used to jump
    /// </summary>
    public KeyCode jump = KeyCode.Space;
    /// <summary>
    ///  Selects the fire element for casting a spell
    /// </summary>
    public KeyCode fireElement = KeyCode.J;
    /// <summary>
    ///  Selects the water element for casting a spell
    /// </summary>
    public KeyCode waterElement = KeyCode.L;
    /// <summary>
    ///  Selects the air element for casting a spell
    /// </summary>
    public KeyCode airElement = KeyCode.I;
    /// <summary>
    ///  Selects the earth element for casting a spell
    /// </summary>
    public KeyCode earthElement = KeyCode.K;
}
