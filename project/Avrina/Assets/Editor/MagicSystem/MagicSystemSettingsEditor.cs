using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(MagicSystemSettings))]
[InitializeOnLoad]
// https://answers.unity.com/questions/921989/get-keycode-events-in-editor-without-object-select.html
public class MagicSystemSettingsEditor : Editor
{
    // Defines when the key selection section is open or closed
    private bool showKeyArray;
    // When the boolean is true the next button press will be mapped as the new magic system key
    private static bool remapMagicKey;
    // Which Magic System key should be remapped
    private static MagicSystemKey magicKeyToBeRemapped;

    private void OnEnable()
    {
        // Set some variables
        this.showKeyArray = false;
        remapMagicKey = false;

        // Generate KeyMapper
        var settings = (MagicSystemSettings)target;

        var magicKeys = (MagicSystemKey[])Enum.GetValues(typeof(MagicSystemKey));

        // Only regenerate if the mapping does not exist or the enum changed
        if (settings.keyMapper == null)
        {
            settings.keyMapper = new Dictionary<MagicSystemKey, KeyCode>();

            foreach (var magicKey in magicKeys)
            {
                // Default Key is space because it does not make any sence
                settings.keyMapper.Add(magicKey, KeyCode.Space);
            }
        } else
        {
            var newMapper = new Dictionary<MagicSystemKey, KeyCode>();

            foreach (var magicKey in magicKeys)
            {
                // If the key exist add it to the new Mapper
                // Otherwise generate add a new key
                if (settings.keyMapper.ContainsKey(magicKey))
                {
                    newMapper.Add(magicKey, settings.keyMapper[magicKey]);
                } else
                {
                    newMapper.Add(magicKey, KeyCode.Space);
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        // Generate KeyMapper
        var settings = (MagicSystemSettings)target;

        // Create Section where the magic keys will be shown only if the area is open
        this.showKeyArray = EditorGUILayout.Foldout(this.showKeyArray, "Magic Keys:");

        if (this.showKeyArray)
        {
            var magicKeys = (MagicSystemKey[])Enum.GetValues(typeof(MagicSystemKey));

            // Go through all Magic keys and draw them
            foreach (var magicKey in magicKeys)
            {
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.LabelField(magicKey.ToString());
                if (GUILayout.Button(settings.keyMapper[magicKey].ToString()))
                {
                    // When the button is pressed the magic key should be remapped
                    magicKeyToBeRemapped = magicKey;
                    remapMagicKey = true;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }

    private static void OnSceneGUI()
    {
        if (remapMagicKey)
        {
            Debug.Log(remapMagicKey);
            // Get current key press
            var e = Event.current;
            if (e.type != EventType.Layout && e.type != EventType.Repaint && e.type != EventType.MouseMove && e.type != EventType.MouseDown && e.type != EventType.MouseEnterWindow && e.type != EventType.MouseLeaveWindow)
            {
                return;
                /*Debug.Log("HI");
                // Get target object
                var settings = (MagicSystemSettings)target;
                // Select pressed key as new magic key
                settings.keyMapper[this.magicKeyToBeRemapped] = Event.current.keyCode;
                this.remapMagicKey = false;*/
            }
        }
    }
}
