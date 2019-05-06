using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(MagicSystemSettings))]
public class MagicSystemSettingsEditor : Editor
{
    private void OnEnable()
    {
        // Generate KeyMapper
        var settings = (MagicSystemSettings)target;

        var magicKeys = (MagicSystemKey[])Enum.GetValues(typeof(MagicSystemKey));

        // Only regenerate if the mapping does not exist or the enum changed
        if (settings.keyMapper == null)
        {
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

    private void OnIspectorGUI()
    {
        
    }
}
