using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpellBase), true)]
public class SpellBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var spellBase = (SpellBase)target;

        // Need to convert all layernames into an string array
        string[] layerNames = new string[SortingLayer.layers.Length];
        for (int i = 0; i < SortingLayer.layers.Length; i++)
        {
            layerNames[i] = SortingLayer.layers[i].name;
        }

        // Sprite layer
        spellBase.spellLayerIndex = EditorGUILayout.Popup("Spell Sorting Layer", spellBase.spellLayerIndex, layerNames);

        // Check if the mask is not the same as the selected mask. 
        // If so change current mask to selected one
        if (spellBase.spellLayer.Equals(SortingLayer.layers[spellBase.spellLayerIndex]))
        {
            spellBase.spellLayer = SortingLayer.layers[spellBase.spellLayerIndex];
        }
    }
}
