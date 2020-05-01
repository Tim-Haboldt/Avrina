using UnityEngine;

[System.Serializable]
public class KeyMappings
{
    /// <summary>
    ///  What is the name of the horizontal movement input axis in the unity input controller
    /// </summary>
    public string horizontalMovement;
    /// <summary>
    ///  What is the name of the horizontal aim input axis in the unity input controller
    /// </summary>
    public string horizontalAim;
    /// <summary>
    ///  What is the name of the vertical aim input axis in the unity input controller
    /// </summary>
    public string verticalAim;
    /// <summary>
    ///  What is the duck key
    /// </summary>
    public KeyCode duck;
    /// <summary>
    ///  What key opens the settings
    /// </summary>
    public KeyCode settings;
    /// <summary>
    ///  What key casts the spell
    /// </summary>
    public KeyCode cast;
    /// <summary>
    ///  Resets the selected elements
    /// </summary>
    public KeyCode cancel;
    /// <summary>
    ///  What key can be used to jump
    /// </summary>
    public KeyCode jump;
    /// <summary>
    ///  Selects the fire element for casting a spell
    /// </summary>
    public KeyCode fireElement;
    /// <summary>
    ///  Selects the water element for casting a spell
    /// </summary>
    public KeyCode waterElement;
    /// <summary>
    ///  Selects the air element for casting a spell
    /// </summary>
    public KeyCode airElement;
    /// <summary>
    ///  Selects the earth element for casting a spell
    /// </summary>
    public KeyCode earthElement;
}
