using UnityEngine;

[System.Serializable]
public class JoyStickMapping : KeyMapping
{
    /// <summary>
    ///  Defines the type of the key mapping
    /// </summary>
    public override MappingType type { get { return MappingType.JoyStick; } }

    /// <summary>
    ///  What is the name of the horizontal movement input axis in the unity input controller
    /// </summary>
    public string horizontalMovement = "JoyStickMoveHorizontal";
    /// <summary>
    ///  What is the name of the vertical movement input axis in the unity input controller
    /// </summary>
    public string verticalMovement = "JoyStickMoveVertical";
    /// <summary>
    ///  What is the name of the horizontal aim input axis in the unity input controller
    /// </summary>
    public string horizontalAim = "JoyStickAimHorizontal";
    /// <summary>
    ///  What is the name of the vertical aim input axis in the unity input controller
    /// </summary>
    public string verticalAim = "JoyStickAimVertical";
    /// <summary>
    ///  What key opens the settings
    /// </summary>
    public KeyCode settings = KeyCode.Joystick1Button6;
    /// <summary>
    ///  What key casts the spell
    /// </summary>
    public KeyCode cast = KeyCode.Joystick1Button2;
    /// <summary>
    ///  Resets the selected elements
    /// </summary>
    public KeyCode cancel = KeyCode.Joystick1Button1;
    /// <summary>
    ///  What key can be used to jump
    /// </summary>
    public KeyCode jump = KeyCode.Joystick1Button0;
    /// <summary>
    ///  Selects the fire element for casting a spell
    /// </summary>
    public KeyCode fireElement = KeyCode.Joystick1Button4;
    /// <summary>
    ///  Selects the water element for casting a spell
    /// </summary>
    public KeyCode waterElement = KeyCode.Joystick1Button5;
    /// <summary>
    ///  Selects the air element for casting a spell
    /// </summary>
    public string airElement = "JoyStickLeftBack";
    /// <summary>
    ///  Selects the earth element for casting a spell
    /// </summary>
    public string earthElement = "JoyStickRightBack";
}
