using System;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public abstract class SpellBase : MonoBehaviour
{
    [HideInInspector] public SortingLayer spellLayer;
    [HideInInspector] public int spellLayerIndex;
    [SerializeField] public SpriteAnimation spellAnimation;

    public abstract void CastSpell(Vector2 playerPosition, Vector2 castDirection);

    public void CopyComponent<T>(T destination, T toCopy) where T : Component
    {
        Type type = destination.GetType();
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(destination, pinfo.GetValue(toCopy, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(destination, finfo.GetValue(toCopy));
        }
    }
}
