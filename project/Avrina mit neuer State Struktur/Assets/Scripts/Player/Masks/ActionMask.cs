using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class ActionMask
{
    public int actions;

    public static List<string> layerNames;

    public static bool IsLayerActive(ActionMask mask, int layer)
    {
        return (mask.actions == (mask.actions | (1 << layer)));
    }

    public static void DrawActionMask(ActionMask mask)
    {
        if (ActionMask.layerNames == null)
            ActionMask.layerNames = new List<string>();

        ActionMask.layerNames.Add("Gravity");
        ActionMask.layerNames.Add("Friction");
        ActionMask.layerNames.Add("Movement");

        mask.actions = EditorGUILayout.MaskField("Actions", mask.actions, ActionMask.layerNames.ToArray());
    }
}
