using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MagicSystemSettings))]
public class MagicSystemSettingsEditor : Editor
{
    // Defines when the key selection section is open or closed
    private bool showKeyArray;
    // Which Magic System key should be remapped (array index)
    // If current state is -1 no key should be remapped
    private int magicKeyToBeRemapped;

    private void OnEnable()
    {
        // Set some variables
        this.showKeyArray = false;
        magicKeyToBeRemapped = -1;

        // Generate KeyMapper
        var settings = (MagicSystemSettings)target;

        var magicKeys = (MagicSystemKey[])Enum.GetValues(typeof(MagicSystemKey));

        // Only regenerate if the mapping does not exist or the enum size changed
        if (settings.keyMapper == null)
        {
            settings.keyMapper = new List<KeyCollection>();

            foreach (var magicKey in magicKeys)
            {
                // Default Key is space because it does not make any sence
                settings.keyMapper.Add(new KeyCollection(magicKey, KeyCode.None));
            }
        }
        else
        {
            if (settings.keyMapper.Count != magicKeys.Length)
            {
                settings.keyMapper = new List<KeyCollection>();

                foreach (var magicKey in magicKeys)
                {
                    // Default Key is space because it does not make any sence
                    settings.keyMapper.Add(new KeyCollection(magicKey, KeyCode.None));
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        // Cast target as MagicSystemSettings Element
        var settings = (MagicSystemSettings)target;

        // Redefine MagicKey if a magic key is selected and a keypress registered
        var eventKeyCode = Event.current.keyCode;
        if (this.magicKeyToBeRemapped != -1 && eventKeyCode != KeyCode.None)
        {
            var keyCollection = settings.keyMapper[this.magicKeyToBeRemapped];
            keyCollection.unityKey = eventKeyCode;
            settings.keyMapper[this.magicKeyToBeRemapped] = keyCollection;
            this.magicKeyToBeRemapped = -1;
            Repaint();
        }

        // Create Section where the magic keys will be shown only if the area is open
        this.showKeyArray = EditorGUILayout.Foldout(this.showKeyArray, "Magic Keys:");

        // Generate KeyMapper Layout
        if (this.showKeyArray)
        {
            for (int count = 0; count < settings.keyMapper.Count; count++)
            {
                var keyCollection = settings.keyMapper[count];

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(keyCollection.magicKey.ToString());
                if (GUILayout.Button(keyCollection.unityKey.ToString()))
                {
                    // When the button is pressed the magic key should be remapped
                    magicKeyToBeRemapped = count;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
