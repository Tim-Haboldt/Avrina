using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(MagicSystemSettings))]
public class MagicSystemSettingsEditor : Editor
{
    private bool showKeyArray = false;

    private void OnEnable()
    {
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
                    foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKey(key))
                        {
                            Debug.Log(key.ToString());
                            settings.keyMapper[magicKey] = key;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
