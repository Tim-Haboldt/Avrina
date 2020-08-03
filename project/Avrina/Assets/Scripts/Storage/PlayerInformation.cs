using UnityEngine;

public class PlayerInformation
{
    /// <summary>
    ///  Stores the name of the player and displays it ingame
    /// </summary>
    public static string playerName = null;
    /// <summary>
    ///  Stores the ingame color of the player
    /// </summary>
    public static Color playerColor = new Color(0, 0, 0);
    /// <summary>
    ///  Stores the keyboard mapping for the first player
    /// </summary>
    public static MappingType playerOneMapping = MappingType.JoyStick;
    /// <summary>
    ///  Stores the keyboard mapping for the second player
    /// </summary>
    public static MappingType playerTwoMapping = MappingType.KeyBoard;
}
